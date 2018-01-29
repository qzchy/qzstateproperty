//资产列表控制器
'use strict';
 
app.controller('PropertyListCtrl', ['$window', '$rootScope', '$uibModal', '$state', '$scope', '$timeout', 'PropertyService', 'GovernmentService', '$q', '$localStorage', 'toaster',
    function ($window, $rootScope, $uibModal, $state, $scope, $timeout, propertyService, governmentService, $q, $localStorage, toaster) {
        var error = $scope.error = "";

        $scope.processing = false;
        //高级搜索的部门搜索 
        $scope.governments = [];
        $scope.government = {};
        $scope.properties = [];

        //自定义参数集合
        var params = {
            showHidden: true,
            pageIndex:0,
            pageSize: 15,
            query:"",
            sort: "getedDate,desc;",
            manage: "true",
            isGovernment: false,
            isInstitution: false,
            isCompany: false,
            selectedId: 0,
            construct: false,
            land: false,
            constructOnLand: false,
            old: false,
            west: false,
            jjq: false,
            kc: false,
            qj: false,
            other: false,
            certi_both: false,
            certi_land: false,
            certi_construct: false,
            certi_none: false,
            current_self: false,
            current_rent: false,
            current_lend: false,
            current_idle: false,
            development: false,
            auction: false,
            injection:false,
            ct: false,
            jt: false,
            jk: false,
            self: false,
            storeUp: false,
            adjust: false,
            greenland: false,
            house: false,
            constructArea_L: false,
            constructArea_M: false,
            constructArea_H: false,
            constructArea_T: false,
            landArea_L: false,
            landArea_M: false,
            landArea_H: false,
            landArea_T: false,
            price_L: false,
            price_M: false,
            price_H: false,
            price_T: false,
            lifeTime_min: 0,
            lifeTime_max:70
        };

       
        $scope.params = angular.copy(params);
  
        //导出字段集合
        var fields = {
            isName: true,
            isGovernment: true,
            isPGovernment: true,
            isPropertyType: true,
            isGovernmentType: true,
            isRegion: true,
            isAddress: true,
            isConstructArea: true,
            isLandArea: true,
            isPropertyID: true,
            isPropertyNature: true,
            isLandNature: true,
            isPrice: true,
            isGetedDate: true,
            isLifeTime: true,
            isUsedPeople: true,
            isCurrentUse_Self: true,
            isCurrentUse_Rent: true,
            isCurrentUse_Lend: true,
            isCurrentUse_Idle: true,
            isNextStepUsage: true,
            isEstateId: true,
            isConstructId: true,
            isLandId: true,
            isHasConstructID: true,
            isHasLandID: true,
            isRent: true,
            isLend: true
        };
        $scope.fields = angular.copy(fields);

        //选中的行
        $scope.selectedItem = [];

        //#region table config

        //列集合
        $scope.columns = [{
            type: 'link',
            title: "名称",
            name: "name",
            orderable: true,
            linkField: "id",
            click: function (propertId) {
                $state.go("app.property.detail", { id: propertId });
            },
        }, {
            title: "地址",
            width: 220,
            name: "address",
            orderable: true
        }, {
            title: "区域",
            width: 100,
            name: "region",
            orderable: true,
            alignCenter: true
        }, {
            title: "取得日期",
            width: 150,
            name: "getedDate",
            orderable: true,
            alignCenter: true,
            dir: "desc"
        }, {
            type: 'link',
            title: "产权单位",
            width: 250,
            name: "governmentName",
            orderable: true,
            alignCenter: true,
            linkField: "governmentId",
            click: function (governmentId) {
                $state.go("app.property.government", { id: governmentId });
            },
        }];

        //编辑删除功能
        $scope.tableEidtAndDelete = [
             {
                 title: "资产变更",
                 handler: function (row) {
                     $state.go("app.property.edit", { id: row.id });
                 },
                 isEnable: function (row) { return row.canChange; }
             }
        ];

        //高级搜索
        $scope.advanceSearch = function () {
            var modalInstance = $uibModal.open({
                templateUrl: 'advDialog.html',
                controller: 'AdvanceModalDialogCtrl',
                size: 'lg',
                windowClass: "map-modal",
                //appendTo: '#app',
                resolve: {
                    dialogHeight: function () { return $scope.dialogHeight; },
                    params: function () { return $scope.params },
                    governmentService: function () { return governmentService; },
                    governments: function () { return $scope.governments; },
                    government: function () { return $scope.government },
                    ajax: function () { return $scope.ajax; },
                    resetParams: function () { return $scope.resetParams; }
                }
            });

            modalInstance.result.then(function () {
            }, function () {
               // $state.reload();
            });
        };

        $scope.resetParams = function () {
            $scope.params = angular.copy(params);
            $scope.governments = [];
            $scope.government = {};
            $scope.ajax();
        };

        $scope.reset = false;

        //#endregion    

        $scope.exportBtn = false;

        //region 资产导出
        $scope.Export = function () {
            var modalInstance = $uibModal.open({
                templateUrl: 'export.html',
                controller: 'exportCtrl',
                size: 'lg',
                windowClass: "map-modal",
                //appendTo: '#app',
                resolve: {
                    dialogHeight: function () { return $scope.dialogHeight; },
                    fields: function () { return $scope.fields },
                    governmentService: function () { return governmentService; },
                    governments: function () { return $scope.governments; },
                    government: function () { return $scope.government },
                    exportExcel: function () { return $scope.exportExcel; },
                    resetParams: function () { return $scope.resetParams; },
                    exportBtn: function () { return $scope.exportBtn; }
                }
            });

            modalInstance.result.then(function () {
            }, function () {
              //  $state.reload();
            });
        }

        $scope.ExportMonthTotal = function () {

            propertyService.exportMonthTotal().then(function () {
                alert("导出成功");
            })
        }


        $scope.selectedRows = [];
        //创建id字符串
        var buildIdsString = function () {
            var idsString = "";
                angular.forEach($scope.selectedRows, function (data, index) {
                    idsString += data.id + (index == $scope.selectedRows.length - 1 ? "" : ";");
                });
                    
            return idsString;
        };

        $scope.exportExcel = function () {

            var deferred = $q.defer();

            var ids = buildIdsString();
            if (ids == "") ids = "all";

            var params1 = {
                query:$scope.params.query,
                sort: "name,asc;",
                currentExtent: false,
                bbox: "",
                government: {
                    manage: 'true',
                    isGovernment: $scope.params.isGovernment,
                    isInstitution: false,
                    isCompany:$scope.params.isCompany,
                    selectedId: 0
                },
                extent: {
                    type: "",
                    geo: ""
                },
                propertyType: {
                    construct: $scope.params.construct,
                    land: $scope.params.land,
                    constructOnLand: $scope.params.constructOnLand
                },
                region: {
                    old: $scope.params.old,
                    west: $scope.params.west,
                    jjq: $scope.params.jjq,
                    kc: $scope.params.kc,
                    qj: $scope.params.qj,
                    other: $scope.params.other,
                },
                certificate: {
                    both: false,
                    land: false,
                    construct: false,
                    none: false
                },
                current: {
                    self: $scope.params.current_self,
                    rent: $scope.params.current_rent,
                    lend: $scope.params.current_lend,
                    idle: $scope.params.current_idle
                },
                nextStep: { mapType: 0, auction: false, injection: false, storeUp: false, adjust: false, ct: false, jt: false, jk: false },
                constructArea: { ranges: [], l: false, m: false, h: false, t: false },
                landArea: { ranges: [], l: false, m: false, h: false, t: false },
                price: { ranges: [], l: false, m: false, h: false, t: false },
                lifeTime: { min: 0, max: 70 },
                getedDate: { min: 0, max: 0 },
                fields:$scope.fields
            };
            $scope.params1 = angular.copy(params1);

            propertyService.export(ids, $scope.params1).then(function (response) {

                deferred.resolve(response);

            }, function (msg) {
                deferred.reject(msg);
            });
            return deferred.promise;
        }

      

        $scope.Import = function () {
         
            $("#filePicker input").click();
        }

        var fileUploader = null;
        var authData = $localStorage.authorizationData;
        if (fileUploader == null) {
            fileUploader = WebUploader.create({
                // swf文件路径
                swf: $rootScope.baseUrl + 'vender/libs/webimgUploader/Uploader.swf',
                // 文件接收服务端。
                server: $rootScope.apiUrl + 'Properties/import/excel',
                auto: true,
                // 选择文件的按钮。可选。
                // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                pick: '#filePicker',
                headers: {
                    Authorization: 'Bearer ' + authData.token  //"Bearer JUxMiWLI8-zNklXn6wSpYyiPbEiN8KQwDqf4zQfbPiYEwfca7BkBZg_nJcpHVj053X_hObBhSCI0PGYzbN560BloEUbUrmRq9aUkZhu2kSNuVJChHeV4B4bmvwu7C-XLv6_RDWxL2aBUnTpae7TBxAGUlrkX4ynEzhUO84GUE8viFC53Mj1H4OSqh0gQbfs3Tn2nkoxM36F43owSfDO4CoTEZD_wWR41d5PPM5lSM1rZziKCG-RcbSBazjAiXQiuosBvN5vnPDTfqBIrIJyJ46yzy-xjl0SeivftsuJOQLUntF_xUk2XWGJOIgHrDpdvZRhkCMRcVQxczRfQsvinim75yz2KKfyRHv7I2ddtpFqtId093rpupII7-fVUfwCu7uqjXQqWBFvUnkhGT97e6HIZv32fztmGUdFsNAe4wYcIFoMKalFAdobErxETfC5tpnP3F-cnyokbHyNmXMhJRTzzKMJHTsjaXm9wMzZ6jr4xAuQ-20gPgPbbpS-Cqk9G2AuczmGGrZYIart0PKc1ZZZAHgg31MQUBpccK2bzKtaVX40aaX0fYL2GK6R_xYwX"
                },
                // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                resize: true,
                fileNumLimit: 1,//上传数量限制
                fileSizeLimit: 100 * 5 * 1024 * 1024,//限制上传所有文件大小
                fileSingleSizeLimit: 5 * 1024 * 1024,//限制上传单个文件大小
            });

            fileUploader.on("error", function (type) {
                console.log(type);
                if (type == "F_DUPLICATE") {
                    toaster.pop('error', '请不要重复提交');
                } else if (type == "Q_EXCEED_SIZE_LIMIT") {
                    toaster.pop('error', '所选文件总大小不可超过500M！');
                }
                else if (type == "F_EXCEED_SIZE") {
                    toaster.pop('error', '所选文件单个大小不可超过5M！', '', 1000);
                }
            });
            // 当有文件添加进来的时候
            fileUploader.on('fileQueued', function (file) {

                $rootScope.thelist = true;

             //   $("#thelist").append('<div id="' + file.id + '" class="item">' +
             //'<h4 class="info">' + file.name + '</h4>' +
             //'<p class="state">等待上传...</p>' +
             //'</div>');
            });
            //文件上传进度
            fileUploader.on('uploadProgress', function (file, percentage) {
                $rootScope.thelist = true;
         //       var $li = $('#' + file.id),
         //       $percent = $li.find('.progress .progress-bar');

         //       // 避免重复创建
         //       if (!$percent.length) {
         //           $percent = $('<div class="progress progress-striped active">' +
         //             '<div class="progress-bar" role="progressbar" style="width: 0%">' +
         //             '</div>' +
         //           '</div>').appendTo($li).find('.progress-bar');
         //       }

         ////       $li.find('p.state').text('上传中');

         //       $percent.css('width', percentage * 100 + '%');
             //   if (percentage == 1) $li.find('p.state').text('上传完成');
            });

            //文件上传结果处理
            fileUploader.on('uploadSuccess', function (file, response) {
                $rootScope.thelist = false;
                if (response.count > 0) {                   
                    alert(response.appendLine);
                    $state.go("app.property.process_approve", { approveType: "newCreate" });
                }
                else {
                    alert(response.appendLine);
                    $state.reload();
                }                             
            });    

        }
     



     
        //#region 设置对话框高度
        var windowHeight = $window.innerHeight; //获取窗口高度
        var headerHeight = 50;
        var footerHeight = 51;
        var windowWidth = $window.innerWidth; //获取窗口宽度

        $scope.dialogHeight = { "height": windowHeight - headerHeight - footerHeight - 230 };
        //#endregion

        $scope.ajax = function () {

            if ($scope.processing) return;

            if ($scope.government.selected != null && $scope.government.selected != undefined) $scope.params.selectedId = $scope.government.selected.id;
            else $scope.params.selectedId = 0;

            $scope.processing = true;
            $scope.params.time = new Date().getTime();
            propertyService.getAllPropertiesByUrl($scope.params).then(function (response) {
                if ($scope.response != null && $scope.response != undefined && response.time < $scope.response.time) return;
                $scope.response = response;
            }, function () {
                alert("获取数据失败");
            }).finally(function () {
                $scope.processing = false;
            });
        };

    }]);

app.controller('exportCtrl', function ($scope, $uibModalInstance, dialogHeight, fields, governmentService, governments, government, exportExcel, resetParams, exportBtn) {

    $scope.dialogHeight = dialogHeight;
    $scope.okText = "确定";
    $scope.cancelText = "取消";
    $scope.fields = fields;
    $scope.exportExcel = exportExcel;
    $scope.exportBtn = exportBtn;

    $scope.ok = function () {
        $scope.exportBtn = true;
        $scope.exportExcel().then(function (response) {
            alert("导出成功！");
            var fileName = "资产导出.xls";
            var blob = new Blob([response], { type: "application/vnd.ms-excel" });
            var objectUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            document.body.appendChild(a);
            a.setAttribute('style', 'display:none');
            a.setAttribute('href', objectUrl);
            a.setAttribute('download', fileName);
            a.click();
            URL.revokeObjectURL(objectUrl);
        }, function (msg) { }).finally(function () {
            $scope.exportBtn = false;
        });;
       // $uibModalInstance.dismiss();
    }

    //全选
    $scope.selectAll = function () {

        for (var data in $scope.fields) {
            $scope.fields[data] = true;
        }

    }
    //重置
    $scope.resetFields = function () {

        for (var data in $scope.fields) {
            $scope.fields[data] = false;
        }

    }
    //取消
    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    };

})

app.controller('AdvanceModalDialogCtrl', function ($scope, $uibModalInstance, dialogHeight, params, governmentService, governments, government, ajax, resetParams) {

    $scope.dialogHeight = dialogHeight;
    $scope.okText = "确定";
    $scope.cancelText = "取消";

    $scope.params = params;
  
    $scope.governmentService = governmentService;

    //高级搜索的部门搜索 
    $scope.governments = governments;
    $scope.government = government;

    //关键词搜索主管部门  
    $scope.refreshGovernments = function (governmentName) {
        if (governmentName == "" || governmentName == null || governmentName == undefined) return;
        governmentService.autocompleteByName(governmentName).then(function (response) {
            $scope.governments = response.data;
        });
    };

    $scope.ok = function () {
        $uibModalInstance.close();
        ajax();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    };

    $scope.resetParams = function () {
        resetParams();

        $scope.params = {
            showHidden: true,
            pageSize: 15,
            query: "",
            sort: "name,asc;",
            government: {
                manage: 'true',
                isGovernment: false,
                isInstitution: false,
                isCompany: false,
                selectedId: 0
            },
            extent: {
                type: "",
                geo: ""
            },
            propertyType: {
                construct: false,
                land: false,
                constructOnLand: false
            },
            region: {
                old: false,
                west: false,
                jjq: false,
                kc: false,
                qj: false,
                other: false,
            },
            certificate: {
                both: false,
                land: false,
                construct: false,
                none: false
            },
            current: {
                self: false,
                rent: false,
                lend: false,
                idle: false
            },
            nextStep: {
                development: false,
                auction: false,
                injection: false,
                self: false,
                storeUp: false,
                adjust: false,
                greenland: false
            },
            constructArea: { ranges: [], l: false, m: false, h: false, t: false },
            landArea: { ranges: [], l: false, m: false, h: false, t: false },
            price: { ranges: [], l: false, m: false, h: false, t: false },
            lifeTime: { min: 0, max: 70 },
            getedDate: { min: 1956, max: new Date().getFullYear() }
        };
        $scope.governments = [];
        $scope.government = {};
    };
});