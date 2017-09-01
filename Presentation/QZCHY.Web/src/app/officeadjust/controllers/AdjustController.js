"use strict";
app.controller('AdjustCtrl', ['$window', '$scope', '$timeout','$filter', '$uibModal', 'MapService', 'GovernmentService',
    function ($window, $scope, $timeout, $filter, $uibModal, mapService, governmentService) {
        var map = $scope.map = null;
        var init = true;
        //颜色分别对应 房产开发 国有收储 拍卖处置 公共绿地 调剂说明 注入国有公司  自用
        var colors = $scope.colors = ["red", "green", "orange", "pink"];

        $scope.time = 0;
        $scope.suggestionTime = 0;
        var mapHeight = $scope.mapHeight = {};
        var cluseter = $scope.cluseter = {
            active: true,
            init: true,
            labelName: true,
            markers: [],  //点集合
            markerClusterGroup: null  //聚合图层
        };
        var heatmap = $scope.heatmap = {
            active: false,
            init: true,
            data: {},
            config: {
                //the minimum opacity the heat will start at
                minOpacity: 0.8,
                //- zoom level where the points reach maximum intensity (as intensity scales with zoom), equals maxZoom of the map by default
                maxZoom: 18,
                //maximum point intensity, 1.0 by default
                max: 1,
                //radius of each "point" of the heatmap, 25 by default
                radius: 10,
                //amount of blur, 15 by default
                blur: 15,
                //  color gradient config, e.g. {0.4: 'blue', 0.65: 'lime', 1: 'red'}
                gradient: { 0.4: 'blue', 0.65: 'lime', 1: 'red' }
            },
            layer: null,
            field: 'constructArea'
        };

        $scope.properties = [];
        $scope.showProperties = [];  //面板要显示的资产
        $scope.selectedProperty = null;
        var suggestedProperties = $scope.suggestedProperties = [];

        var search = {
            config: {
                searching: false, //标识是否正在执行搜索任务
                filterModel: false, //是否进入条件搜索模式
                searchResultBoxFolder: true,  //搜索面板折叠
                advanceModel: false,    //是否为高级搜索
                showSort: false,   //弹出排序面板
                showFilter: false, //弹出过滤面板
                showAdvance: false,  //高级搜索面板
                showChart: false      //图表面板
            },
            params: {
                query: "",
                sort: "name,asc;",
                currentExtent: false,
                bbox: "",
                government: {
                    manage: 'true',
                    isGovernment: false,
                    isInstitution: false,
                    isCompany: false,
                    selectedId: 0
                }
            }
        };

        $scope.search = angular.copy(search);

        $scope.showExtent = null;  //要显示的范围
        $scope.hoverShowExtent = null;  //鼠标移动显示范围

        var mapOverlayOption = {
            editable: false,
            color: '#AA0000',
            weight: 3,
            opacity: 1.0,
            fillColor: '#AA0000',
            fillOpacity: 0.2
        };

        var wkt = new Wkt.Wkt();

        //高级搜索的部门搜索 
        $scope.governments = [];
        $scope.government = {};

        $scope.editableLayers = new L.FeatureGroup();

        // #region 界面初始化

        var windowHeight = $window.innerHeight; //获取窗口高度
        var headerHeight = 50;
        var footerHeight = 51;
        var windowWidth = $window.innerWidth; //获取窗口宽度

        $scope.mapHeight = { "height": windowHeight - headerHeight - footerHeight, "width": windowWidth - 80 };

        $scope.mapSearchResultHeight = { "height": $scope.mapHeight.height - 150 };

        $scope.mapDetailHeight = { "height": $scope.mapSearchResultHeight.height + 35 - 150 - 39 };

        $scope.dialogHeight = { "height": $scope.mapHeight.height - 170 };

        //#endregion

        $timeout(function () {
            //#region 地图配置 

            var normal = mapService.getLayer("vector", "Normal");
            var satellite = mapService.getLayer("img", "Satellite");

            map = L.map('map', {
                crs: L.CRS.EPSG4326, center: { lon: 118.8656, lat: 28.9718 }, zoom: 15, layers: [normal]
            });

            var iconLayersControl = new L.Control.IconLayers(
                [
                    {
                        title: '矢量', // use any string
                        layer: normal, // any ILayer
                        icon: 'img/dx.png' // 80x80 icon
                    },
                    {
                        title: '影像',
                        layer: satellite,
                        icon: 'img/yx.png'
                    }
                ], {
                    position: 'bottomleft',
                    maxLayersInRow: 5
                }
            );

            var zoomControl = map.zoomControl;

            zoomControl.setPosition("bottomright");

            iconLayersControl.addTo(map);

            mapService.setMapAttribute(map);

            //#region 编辑工具

            map.addLayer($scope.editableLayers);



            $scope.drawExtent = function () {
                //先清空清空
                $scope.editableLayers.clearLayers();

                switch ($scope.search.params.extent.type) {
                    case 'rectangle':
                        drawPolygon.disable();
                        drawCircle.disable();
                        drawRectangle.enable();
                        break;
                    case 'circle':
                        drawPolygon.disable();
                        drawCircle.enable();
                        drawRectangle.disable();
                        break;
                    case 'polygon':
                        drawPolygon.enable();
                        drawCircle.disable();
                        drawRectangle.disable();
                        break;
                    default:
                        drawPolygon.disable();
                        drawCircle.disable();
                        drawRectangle.disable();
                        break;
                }
            };

            //#endregion

            //#endregion

            //添加对资产数据的监听
            $scope.$watch('properties', function (newValue, oldValue, scope) {

                if ($scope.search.config.searching) return;

                $scope.showProperties = [];

                if (cluseter.active) $scope.showCluster(true);
                else $scope.showHeatmap(true);

                if (!init) $scope.loadMore();
            });

            $scope.loadPropertiesBigData();
        }, 50);


        $scope.setNextStepParams = function (type) {
            switch (type) {
                case 1:           
                    $scope.search.params.government.isGovernment = !$scope.search.params.government.isGovernment;
                    break;
                case 2:
                    $scope.search.params.government.isInstitution = !$scope.search.params.government.isInstitution;
                    break;
                case 3:
                    $scope.search.params.government.isCompany = !$scope.search.params.government.isCompany;
                    break;
            }
            $scope.searchProperties(true);
        };

        //关键词搜索主管部门  
        $scope.refreshGovernments = function (governmentName) {
            if (governmentName == "" || governmentName == null || governmentName == undefined) return;
            governmentService.autocompleteByName(governmentName).then(function (response) {
                $scope.governments = response.data;
            });
        };

        //显示为聚合图
        $scope.showCluster = function (force) {
            if (cluseter.active && !force) return;

            cluseter.active = true;
            heatmap.active = false;

            if (heatmap.layer != null) {
                map.removeLayer(heatmap.layer);
                heatmap.layer = null;
            }

            $scope.setClusterData();
        };

        $scope.setClusterData = function () {
            if (!cluseter.active) return;

            try {
                map.removeLayer(cluseter.markerClusterGroup);
            }
            catch (e) {

            }

            wkt = new Wkt.Wkt();

            cluseter.markers = [];
            cluseter.markerClusterGroup = L.markerClusterGroup({ chunkedLoading: true });

            angular.forEach($scope.properties, function (property, v) {

                var colorIndex = property.governmentTypeId;
                console.log(colorIndex);
                var option1 = {
                    icon: L.AwesomeMarkers.icon({ markerColor: $scope.colors[colorIndex], iconColor: 'white' })
                };
                var option2 = {
                    icon: L.AwesomeMarkers.icon({ icon: 'clone', markerColor: $scope.colors[colorIndex], prefix: 'fa', iconColor: 'white' }),
                };
                var option3 = {
                    icon: L.AwesomeMarkers.icon({ icon: 'object-ungroup', markerColor: $scope.colors[colorIndex], iconColor: 'white' })
                };
                if (property.location == "") return false;
                wkt.read(property.location);

                var location = wkt.toObject(option3);

                //switch (property.propertyType) {
                //    case "房屋":
                //        location = wkt.toObject(option1);
                //        break;
                //    case "土地":
                //        location = wkt.toObject(option2);
                //        break;
                //    case "对应房屋土地":
                //        location = wkt.toObject(option3);
                //        break;
                //}

                location.bindLabel(property.name, { noHide: cluseter.labelName });

                cluseter.markers.push(location);

                cluseter.markerClusterGroup.addLayer(location);

                //hover事件
                location.on('mouseover', function () {

                    if (property.extent != null && property.extent != undefined && property.extent != "") {
                        wkt.read(property.extent);

                        $scope.showHoverExtent = wkt.toObject(mapOverlayOption);
                        $scope.showHoverExtent.addTo(map);
                    }
                });

                location.on('mouseout', function () {

                    if ($scope.showHoverExtent != null)
                        map.removeLayer($scope.showHoverExtent);
                });

                //点击事件
                location.on('click', function () {
                    $scope.selectProperty(property.id, true);
                });
            });

            // cluseter.init = false;

            map.addLayer(cluseter.markerClusterGroup);
        };

        //标注名称显示切换
        $scope.labelName = function () {
            if (!cluseter.active) return;
            cluseter.labelName = !cluseter.labelName;

            angular.forEach(cluseter.markers, function (marker, i) {

                marker.setLabelNoHide(cluseter.labelName);
            });
        };

        //显示为热力图
        $scope.showHeatmap = function (force) {
            if (heatmap.active && !force) return;

            cluseter.active = false;
            heatmap.active = true;

            search.config.showAdvance = false;

            if (cluseter.markerClusterGroup != null) {
                map.removeLayer(cluseter.markerClusterGroup);
                cluseter.markerClusterGroup = null;
            }

            $scope.refreshHeatmap(true);
        };

        //刷新热力图
        $scope.refreshHeatmap = function (refreshData) {
            if (!heatmap.active) return;


            if (refreshData) {
                //修改数据
                heatmap.data = []; //清空数据
                angular.forEach($scope.properties, function (property, v) {

                    wkt.read(property.location);

                    heatmap.data.push([
                        wkt.components[0].y, wkt.components[0].x, heatmap.field == "normal" ? 1 : property[heatmap.field]
                    ]);
                });
            }

            if (heatmap.layer != null) map.removeLayer(heatmap.layer);
            heatmap.layer = L.heatLayer(heatmap.data, heatmap.config);
            map.addLayer(heatmap.layer);
        };

        //加载大数据
        $scope.loadPropertiesBigData = function () {
            governmentService.getGovernmentsBigDataInMap().then(function (data) {
                //console.log(data);
                $scope.properties = data;
                //$scope.showCluster();
            }, function () {

            });
        };

        //滚动加载
        $scope.loadMore = function () {
            var last = $scope.showProperties.length;

            var newAdded = $scope.properties.slice(last, last + 20);

            $scope.showProperties = $scope.showProperties.concat(newAdded);
        };

        //搜索
        $scope.searchProperties = function (advanceSearch) {

            if ($scope.search.config.searching) return;

            if ($scope.showExtent != null) {
                map.removeLayer($scope.showExtent);
            }

            if (advanceSearch) {
                $scope.search.config.advanceModel = true;  //进入高级搜索模式
                $scope.search.config.filterModel = false;
            }

            $scope.search.config.searching = true;

            var params = $scope.search.params;

            if (params.government.isGovernment == false &&
                params.government.isInstitution == false &&
               params.government.isCompany == false) {
                $scope.loadPropertiesBigData();

                $scope.search.config.filterModel = false;

                $scope.search.config.searching = false;
            }
            else {
                $scope.search.params.time = new Date().getTime();
                governmentService.getAllGovernmentsInMap(params).then(function (response) {
                    if ($scope.time > response.time) return;

                    $scope.time = response.time;
                    $scope.properties = response.data;
                    suggestedProperties = $scope.suggestedProperties = [];

                    $scope.search.config.filterModel = true;

                    $scope.search.config.searching = false;

                    $scope.selectedProperty = null;
                }, function () {
                    // alert("网络异常");

                    //$scope.properties = [];
                    suggestedProperties = $scope.suggestedProperties = [];

                    $scope.search.config.filterModel = true;

                    $scope.search.config.searching = false;

                    $scope.selectedProperty = null;
                });
            }



        };

        //重置高级搜索
        $scope.resetAdvance = function () {

            $scope.search.config.advanceModel = false;  //进入高级搜索模式
            $scope.search.config.filterModel = false;

            //参数重置
            $scope.search = angular.copy(search);

            $scope.loadPropertiesBigData();
        };

        //选中资产
        $scope.selectProperty = function (governmentId, zoom) {

            if ($scope.selectedProperty != null && $scope.selectedProperty.id == governmentId) return;

            governmentService.getGovernmentById(governmentId).then(function (data) {
                $scope.selectedProperty = data;

                if (zoom) {
                    wkt.read($scope.selectedProperty.location);
                    if (map.getZoom() < 17)
                        map.setView([wkt.components[0].y, wkt.components[0].x], 17);
                    else
                        map.panTo([wkt.components[0].y, wkt.components[0].x]);
                }

                if ($scope.selectedProperty.extent != null && $scope.selectedProperty.extent != "") {
                    wkt.read($scope.selectedProperty.extent);

                    var mapOverlayOption = {
                        icon:
                   new L.Icon.Default(),
                        editable: false,
                        color: '#AA0000',
                        weight: 3,
                        opacity: 1.0,
                        fillColor: '#AA0000',
                        fillOpacity: 0.2
                    };

                    if ($scope.showExtent != null) {
                        map.removeLayer($scope.showExtent);
                    }

                    $scope.showExtent = wkt.toObject(mapOverlayOption);
                    $scope.showExtent.addTo(map);
                }

            }, function () { });
        };

        //关键字联想
        $scope.propertiesSuggest = function () {
            $scope.search.config.searching = true;
            var params = {
                query: $scope.search.params.query,
                time: new Date().getTime()
            };

            if (params.query == "" || params.query == null) {
                suggestedProperties = $scope.suggestedProperties = [];
                $scope.search.config.searching = false;
                $scope.search.config.filterModel = false;
                $scope.selectedProperty = null;

                if (!$scope.search.config.advanceModel) {
                    $scope.loadPropertiesBigData();
                }

                return;
            }

            governmentService.suggestion(params).then(function (response) {
                if ($scope.suggestionTime > response.time) return;
                else {
                    suggestedProperties = $scope.suggestedProperties = response.data;

                    $scope.search.config.searching = false;
                }
            }, function () { });
        };

        //根据联想选择结果
        $scope.searchPropertyBySuggestion = function (selectedProperty) {
            $scope.properties = [selectedProperty];
            suggestedProperties = $scope.suggestedProperties = [];

            $scope.search.config.filterModel = true;

            $scope.selectedProperty = null;

        };

        ////标注并缩放至
        //$scope.labelAndZoomTo = function () {
        //    var wkt = new Wkt.Wkt();

        //    wkt.read(selectedProperty.location);

        //    //缩放至点 18级
        //    if (map != null) {
        //        map.setView([wkt.components[0].y, wkt.components[0].x], 18);
        //    }
        //};   

        //过滤。搜索弹出框
        $scope.showSortAndFilter = function (isSort) {

            if (isSort) {
                if ($scope.search.config.showSort) $scope.search.config.showSort = false;
                else {
                    $scope.search.config.showSort = true;
                    $scope.search.config.showFilter = false;
                }
            }
            else {
                if ($scope.search.config.showFilter) $scope.search.config.showFilter = false;
                else {
                    $scope.search.config.showSort = false;
                    $scope.search.config.showFilter = true;
                }
            }
        }

        //清除选中
        $scope.clearSelcted = function () {
            if ($scope.showExtent != null) map.removeLayer($scope.showExtent);
            $scope.selectedProperty = null;
            $scope.showExtent = null;
        };

        init = false;
    }]);