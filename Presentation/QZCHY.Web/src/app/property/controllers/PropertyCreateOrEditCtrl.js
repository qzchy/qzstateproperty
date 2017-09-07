"use strict";

app.controller("PropertyCreateOrEditCtrl", ['$scope', '$rootScope', '$state', '$timeout', 'moment', 'toaster', 'w5cValidator', '$uibModal', 'GovernmentService', 'MapService', 'PropertyService',
function ($scope, $rootScope, $state, $timeout, moment, toaster, w5cValidator, $uibModal, governmentService, mapService, propertyService) {

    //#region 变量定义

    var _wkt = new Wkt.Wkt();
    var _map = null;
    var marker = null, extent = null;
    var editableLayers = new L.FeatureGroup();
    var _mapOverlayOption = {
        edit:true,
        //icon: new L.Icon.Default(),
        //color: '#AA0000',
        //weight: 3,
        //opacity: 1.0,
        //fillColor: '#AA0000',
        //fillOpacity: 0.2
    };

    //日期配置项         
    var _dateOption = {
        singleDatePicker: true,
        showDropdowns: true
    };

    var _property = {
        name: "",
        propertyTypeId: "",
        address: "",
        owner_self: 'true',
        governmentId: 0,
        price: 0,
        description: "",
        lifeTime: 0,

        estate: 'true',
        estateId: "",
        constructId: "",
        landId: "",
        constructArea: 0,
        landArea: 0,
        propertyNature: "",
        landNature: "",

        usedPeople: "",
        currentUse_Self: 0,
        currentUse_Rent: 0,
        currentUse_Lend: 0,
        currentUse_Idle: 0,

        logo: null,
        logoUrl: null,
        logoPictureId: 0,
        pictures: [],
        files:[],

        location: "",
        extent: "",

        submit: false
    };

    var _governments = [];
    var _selectedGovernment = {};
    //#endregion

    //验证规则
     w5cValidator.setRules({
            propertyname: {
                required: "资产名称不能为空",
                w5cuniquecheck: "输入的资产已经存在，请重新输入"
            },
            propertyTypeId: {
                required: "资产类别不能为空"              
            },
            propertyaddress: {
                required: "坐落地址不能为空"
            },
            propertyOwner: {
                required: "产权单位不能为空"
            },
            propertyOwner: {
                required: "产权单位不能为空"
            },          
            price: {
                required: "账面价值不能为空"
            },        
            username: {
                required: "使用单位不能为空"
            },
            plandNature: {
                required: "土地性质不能为空"
            },
            propertyLocation: {
                required: "空间信息不能为空"
            },
            pLogo: {
                required: "封面图片不能为空"
            },
        });

    //每个表单的配置，如果不设置，默认和全局配置相同
    var validation = $scope.validation = {
            options: {
                blurTrig: true
            }
        };        

    //地图初始化
    var _mapInit = function () {
        $timeout(function () {

            var normal = mapService.getLayer("vector", "Normal");
            var satellite = mapService.getLayer("img", "Satellite");

            _map = L.map('map', {
                crs: L.CRS.EPSG4326, center: { lon: 118.8656, lat: 28.9718 }, zoom: 15, layers: [normal]
            });
            mapService.setMapAttribute(_map);
            var baseMaps = {
                "矢量": normal,
                "影像": satellite
            };
            L.control.layers(baseMaps).addTo(_map);

            //#region 编辑工具

            _map.addLayer(editableLayers);

            var options = {
                position: 'topleft',
                draw: {
                    polyline: false,
                    polygon: {
                        allowIntersection: false, // Restricts shapes to simple polygons
                        drawError: {
                            color: '#e1e100', // Color the shape will turn when intersects
                            message: '<strong>绘制错误！<strong> 多边形不能自重叠' // Message that will show when intersect
                        },
                        shapeOptions: {
                            color: 'blue'
                        }
                    },
                    circle: false,
                    rectangle: false,
                    marker: true
                },
                edit: {
                    featureGroup: editableLayers, //REQUIRED!!
                    remove: true
                }
            };

            var drawControl = new L.Control.Draw(options);
            _map.addControl(drawControl);

            //绘制空间查询范围

            var drawPolygon = new L.Draw.Polygon(_map, options.polygon);
            var drawRectangle = new L.Draw.Rectangle(_map, options.rectangle);
            var drawCircle = new L.Draw.Circle(_map, options.circle);

            //要素绘制事件
            _map.on(L.Draw.Event.CREATED, function (e) {
                var type = e.layerType, layer = e.layer;
                var geojson = layer.toGeoJSON();

                switch(type)
                {
                    case "marker":
                        editableLayers.removeLayer(marker);
                        marker = layer;
                        marker.addTo(editableLayers);
                        $scope.property.location = _wkt.fromJson(geojson).write();
                        break;
                    case "polygon":
                        editableLayers.removeLayer(extent);
                        extent = layer;
                        extent.addTo(editableLayers);
                        $scope.property.extent = _wkt.fromJson(geojson).write();
                        break;                        
                }

                $scope.$apply();

                layer.editing.enable();
            });

            //要素编辑事件
            _map.on(L.Draw.Event.EDITED, function (e) {

                angular.forEach(e.layers._layers, function (layer) {

                    if (layer._leaflet_id == marker._leaflet_id)
                    {
                        var geojson = marker.toGeoJSON();
                        $scope.property.location = _wkt.fromJson(geojson).write();
                    }
                    else if (layer._leaflet_id == extent._leaflet_id)
                    {
                        var geojson = extent.toGeoJSON();
                        $scope.property.extent = _wkt.fromJson(geojson).write();
                    }

                });
                $scope.$apply();
            });

            //要素删除事件
            _map.on(L.Draw.Event.DELETED, function (e) {

                angular.forEach(e.layers._layers, function (layer) {

                    if (layer._leaflet_id == marker._leaflet_id)
                        $scope.property.location = "";
                    else if (layer._leaflet_id == extent._leaflet_id)
                        $scope.property.extent = "";

                }); 
                $scope.$apply();
            });

            //汉化
            L.drawLocal.draw.handlers.polygon = {
                tooltip: {
                    start: '点击开始绘制多边形',
                    cont: '点击继续绘制多边形',
                    end: '点击第一个闭合多边形'
                }
            };
            L.drawLocal.draw.handlers.simpleshape = {
                tooltip: {
                    end: '释放鼠标结束绘制'
                }
            };

            //#endregion
 
        }, 50);
    };

    //#region 文件上传模块    
    var imgUploader = null;
    var fileUploader = null;

    //上传初始化
    var _uploadInit = function () {

        //删除所有图片
        $scope.removeAll = function (type) {
            if (type == 'img') {
                $scope.property.pictures = [];
                $scope.selectedImg = null;

                var imgs = imgUploader.getFiles();

                angular.forEach(imgs, function (i, v) {
                    imgUploader.removeFile(i, true);
                });
            }
            else {
                $scope.property.files = [];
                var files = fileUploader.getFiles();

                angular.forEach(files, function (i, v) {
                    fileUploader.removeFile(i, true);
                });
            }
        };

        //删除文件
        $scope.removeFile = function (file, index) {
            $scope.property.files.splice(index, 1);
            if (file.fileId != undefined || file.fileId != null)
            fileUploader.removeFile(file.fileId, true);
        };

        //#region 图片操作function

        //删除图片
        $scope.removeImg = function (img, index) {
            $scope.property.pictures.splice(index, 1);
            if (img.fileId != undefined || img.fileId != null)
            imgUploader.removeFile(img.fileId, true);

            var allFiles = imgUploader.getFiles();
            console.log(allFiles);

            if ($scope.property.pictures.length > 0)
                $scope.selectPicutre($scope.property.pictures[0]);
            else $scope.selectedImg = null;
        };

        $scope.selectPicutre = function (img) {
            if ($scope.property.selectedImg != undefined && $scope.property.selectedImg != null) {
                $scope.property.selectedImg.selected = false;
            }

            img.selected = true;

            $scope.property.selectedImg = img;
        };
      

        if ($scope.property.pictures.length > 0)
            $scope.selectPicutre($scope.property.pictures[0]);

        //#endregion 图片操作function    

        $timeout(function () {
            //图片上传实例化
            if (imgUploader == null) {
                // 实例化
                imgUploader = WebUploader.create({
                    // swf文件路径
                    swf: $rootScope.baseUrl + 'vender/libs/webimgUploader/Uploader.swf',
                    // 文件接收服务端。
                    server: $rootScope.apiUrl + 'Media/pictures/Upload?size=500',
                    auto: true,
                    // 选择文件的按钮。可选。
                    // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                    pick: '#filePicker0',
                    // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                    resize: false,
                    accept: {
                        title: 'Images',
                        extensions: 'gif,jpg,jpeg,bmp,png',
                        mimeTypes: 'image/*'
                    },
                    fileNumLimit: 100,//上传数量限制
                    fileSizeLimit:100* 4*1024*1024,//限制上传所有文件大小
                    fileSingleSizeLimit: 4*1024*1024,//限制上传单个文件大小
                });

                imgUploader.on("error", function (type) {
                    console.log(type);
                    if (type == "F_DUPLICATE") {
                        toaster.pop('error', '请不要重复提交');
                    } else if (type == "Q_EXCEED_SIZE_LIMIT") {
                        toaster.pop('error', '所选图片总大小不可超过200M！"');
                    }
                    else if(type=="F_EXCEED_SIZE")
                    {
                        toaster.pop('error', '所选图片单个大小不可超过2M！"');
                    }
                });
                // 当有文件添加进来的时候
                imgUploader.on('fileQueued', function (file) {

                    var img = {
                        fileId: file.id,
                        title: file.name.replace("." + file.ext, ""),
                        alt: "",
                        percentage: 0,
                        uploaded: false,
                        propertyId: $scope.property.id
                    };
                    $scope.property.pictures.push(img);

                    // 创建缩略图
                    // 如果为非图片文件，可以不用调用此方法。
                    // thumbnailWidth x thumbnailHeight 为 100 x 100
                    imgUploader.makeThumb(file, function (error, src) {
                        if (error) {
                            img.src = $rootScope.baseUrl + "img/webUploader/notpreviewed.png";
                        }
                        else {
                            img.src = src;
                        }
                        //非ng环境下，需要手动触发
                        $scope.$apply();
                    }, 200, 200);
                });
                //文件上传进度
                imgUploader.on('uploadProgress', function (file, percentage) {
                    angular.forEach($scope.property.pictures, function (i, v) {

                        if (v.id == file.id) {
                            v.percentage = percentage;
                            console.log(percentage);

                            $scope.$apply();
                            return;
                        }

                    });


                });
                //文件上传结果处理
                imgUploader.on('uploadSuccess', function (file, response) {
                    angular.forEach($scope.property.pictures, function (v, i) {
                        if (v.fileId == file.id) {
                            v.percentage = 100;
                            v.uploaded = true;
                            v.error = false;
                            v.status = "上传成功";
                            v.pictureId = response[0].id;
                            v.href = response[0].url;
                            return;
                        }

                    });

                });

                imgUploader.on('uploadError', function (file, reason) {


                    angular.forEach($scope.property.pictures, function (i, v) {

                        if (v.fileId == file.id) {
                            v.percentage = 100;
                            v.uploaded = true;
                            v.error = true;
                            v.status = "上传失败";
                            toaster.pop('error', "上传失败：" + reason);
                            return;
                        }

                    });
                });
                imgUploader.on('uploadComplete', function (file) {
                    //非ng环境下，需要手动触发
                    $scope.$apply();
                });
            }

            //文件上传实例化
            if(fileUploader==null)
            {
                // 实例化
                fileUploader = WebUploader.create({
                    // swf文件路径
                    swf: $rootScope.baseUrl + 'vender/libs/webimgUploader/Uploader.swf',
                    // 文件接收服务端。
                    server: $rootScope.apiUrl + 'Media/files/Upload',
                    auto: true,
                    // 选择文件的按钮。可选。
                    // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                    pick: '#filePicker1',
                    // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                    resize: false,
                    fileNumLimit: 100,//上传数量限制
                    fileSizeLimit: 100*5*1024*1024,//限制上传所有文件大小
                    fileSingleSizeLimit: 5*1024*1024,//限制上传单个文件大小
                });

                fileUploader.on("error", function (type) {
                    console.log(type);
                    if (type == "F_DUPLICATE") {
                        toaster.pop('error', '请不要重复提交');
                    } else if (type == "Q_EXCEED_SIZE_LIMIT") {
                        toaster.pop('error', '所选文件总大小不可超过500M！');
                    }
                    else if (type == "F_EXCEED_SIZE") {
                        toaster.pop('error', '所选文件单个大小不可超过5M！','',1000);
                    }
                });
                // 当有文件添加进来的时候
                fileUploader.on('fileQueued', function (file) {

                    var f = {
                        fileId: file.id,
                        title: file.name.replace("." + file.ext, ""),
                        percentage: 0,
                        uploaded: false,
                        propertyId: $scope.property.id
                    };
                    $scope.property.files.push(f);
                });
                //文件上传进度
                fileUploader.on('uploadProgress', function (file, percentage) {
                    angular.forEach($scope.property.pictures, function (i, v) {
                        if (v.id == file.id) {
                            v.percentage = percentage;
                            $scope.$apply();
                            return;
                        }
                    });
                });
                //文件上传结果处理
                fileUploader.on('uploadSuccess', function (file, response) {
                    angular.forEach($scope.property.files, function (v, i) {
                        if (v.fileId == file.id) {
                            v.percentage = 100;
                            v.uploaded = true;
                            v.error = false;
                            v.status = "上传成功";
                            v.fileId = response[0].id;
                            v.src = response[0].url;
                            return;
                        }

                    });
                });
                fileUploader.on('uploadError', function (file, reason) {
                    angular.forEach($scope.property.files, function (i, v) {

                        if (v.fileId == file.id) {
                            v.percentage = 100;
                            v.uploaded = true;
                            v.error = true;
                            v.status = "上传失败";
                            toaster.pop('error', "上传失败：" + reason);
                            return;
                        }

                    });
                    $scope.$apply();
                });
                fileUploader.on('uploadComplete', function (file) {
                    //非ng环境下，需要手动触发
                    $scope.$apply();
                });
            }
        });
    };
    ////#endregion

    //初始化函数
    var _init = function () {

        //加载子单位
        governmentService.getCurrentAccountChidlrenGovernemnts().then(function (response) {
            _governments = response;
            $scope.governments = _governments;
        }, function () {
            alert("加载子单位错误");
        }).finally(function () {
            $scope.governmentProcessing = false;
            _updateProcessingStatus();
        });

        _mapInit();

        //获取当前的资产
        if ($scope.propertyId > 0) {
            propertyService.getUpdatedPropertyById($scope.propertyId).then(function (data) {
                $scope.property = data;
                $scope.property.propertyTypeId = String($scope.property.propertyTypeId);
                $scope.property.governmentId = String($scope.property.governmentId);
                $scope.property.owner_self = String($scope.property.owner_self);
                $scope.property.estate = String($scope.property.estate);

 
                //100ms监测一次
                var interval = setInterval(function () {
                    if($scope.governments.length>0)
                    {
                        angular.forEach($scope.governments, function (i, v) {

                            if(i.value==$scope.property.governmentId)
                            {
                                $scope.selectedGovernment = i;
                                clearInterval(interval);
                                return false;
                            }

                        });
                    }
                }, 100);

                $timeout(function () {
                    //$scope.property.pictures = data.pictures;
                    _wkt.read($scope.property.location);

                    marker = L.marker([_wkt.components[0].y, _wkt.components[0].x]);// _wkt.toObject(mapOverlayOption);

                    marker.addTo(editableLayers);
                    _map.setView(marker.getLatLng(), 18);

                    if ($scope.property.extent != null && $scope.property.extent != "") {
                        _wkt.read($scope.property.extent);
                        extent = _wkt.toObject(_mapOverlayOption);
                        extent.addTo(editableLayers);

                        _map.fitBounds(extent.getBounds());
                    }

                }, 100);
            }, function (data) {
                $scope.errorMsg = data.message;

                if (data.status == 404)
                {
                    toaster.pop("error", "资产不存在或者处于锁定等无法编辑状态","",500);
                    $timeout(function () {
                        $state.go("app.property.list");
                    }, 550)
                }

            }).finally(function () {
                $scope.propertyProcessing = false;
                _updateProcessingStatus();
            })
        }
        else
        {            
            $scope.propertyProcessing = false;
            _updateProcessingStatus();
        }

        //保存实体，是否提交
        validation.saveEntity = function ($event, submit) {
            $scope.submitProcessing = true;
            _updateProcessingStatus();

            if ($scope.property.logoUrl == null)
                $scope.property.logo = $scope.tempLogo;

            $scope.property.submit = submit;  //是否保存后提交

            if ($scope.property.id != undefined && $scope.property.id != null) {

                propertyService.updateProperty($scope.property).then(function (data) {
                    $scope.errorMsg = "";
                    
                    toaster.pop('success', '', '保存成功', '', 500);

                    $timeout(function () {
                        if (!submit) $state.reload();
                        else $state.go("app.property.list");
                    }, 500);

                }, function (message) {
                    toaster.pop('error', '', '保存失败');

                    $scope.errorMsg = message;
                })
                .finally(function () {
                    $scope.submitProcessing = false;
                    _updateProcessingStatus();
                });
            }
            else {

                //新增
                propertyService.createProperty($scope.property).then(function (data) {
                    $scope.errorMsg = "";
                    toaster.pop('success', '', '保存成功');

                    $timeout(function () {
                        if (!submit) {
                            $scope.property = data;
                            $state.go("app.property.edit", { id: data.id });
                        }
                        else $state.go("app.property.list");
                    }, 500);

                }, function (message) {
                    $scope.errorMsg = message;
                    toaster.pop('error', '', '保存失败');
                }).finally(function () {
                    $scope.submitProcessing = false;
                    _updateProcessingStatus();
                });
            }
        };

        $timeout(function () {
            _uploadInit();
            
            //默认的图片base64重置
            if ($scope.tempLogo == "data:image/png;base64,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")
                $scope.tempLogo = null;
        }, 100);
        

        _updateProcessingStatus(); //更新状态
    };

    //监测编号
    var _governmentSelected = function () {
        $scope.property.governmentId = $scope.selectedGovernment.value;
    };

    var _updateProcessingStatus = function () {
        $scope.processingStatus = $scope.propertyProcessing || $scope.governmentProcessing || $scope.submitProcessing;
    };

    //#region 资产封面图片生成

    //选择图片
    var _handleFileSelect = function (evt) {
        var file = evt.currentTarget.files[0];
        //格式判断
        if (!(file.type == "image/png" || file.type == "image/jpeg" || file.type == "image/bmp")) {
            alert("上传文件必须为jpg bmp或者png图片格式");
            $scope.logo = null;
            $scope.tempLogo = null;
        }
        else if (file.size > 4 * 1024 * 1024) {
            alert("图片不能大于4M");
            $scope.logo = null;
            $scope.tempLogo = null;
        }
        else {
            var reader = new FileReader();
            console.log(reader);
            reader.onload = function (evt) {
                $scope.sourceImg = evt.target.result;
                $scope.$apply();
            };
            reader.readAsDataURL(file);
        }

        $scope.$apply();
    };
    angular.element(document.querySelector('#fileInput')).on('change', _handleFileSelect);

    //#endregion

    //#region 变量公开给views
    $scope.sourceImg = null;   //上传图片         
    $scope.tempLogo = null;
    $scope.dateOption = _dateOption;
    $scope.property = _property;
    $scope.governments = _governments;
    $scope.selectedGovernment = _selectedGovernment;
    $scope.governmentSelected = _governmentSelected;
    $scope.propertyProcessing = true;    //正在请求property数据
    $scope.governmentProcessing = true;     //正在请求government数据
    $scope.submitProcessing = false;    //正在提交表单
    $scope.processingStatus = false;
    //#endregion

    return _init(); 
}]);


//var _fileUpload = function () {

//    //var modalInstance = $uibModal.open({
//    //    templateUrl: 'uploadDialog.html',
//    //    controller: 'UploadModalDialogCtrl',
//    //    size: 'lg',
//    //    windowClass: "_map-modal",
//    //    //appendTo: '#app',
//    //    resolve: {
//    //        dialogHeight: function () { return $rootScope.dialogHeight; },
//    //        //params: function () { return $scope.params },
//    //        //governmentService: function () { return governmentService; },
//    //        //governments: function () { return $scope.governments; },
//    //        //government: function () { return $scope.government },
//    //        //ajax: function () { return $scope.ajax; },
//    //        //resetParams: function () { return $scope.resetParams; }
//    //    }
//    //});

//    //modalInstance.result.then(function () {
//    //}, function () {
//    //    $state.reload();
//    //});

//};