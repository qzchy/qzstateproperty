"use strict";
app.controller('MapCtrl', ['$window', '$scope', '$timeout','$filter', '$uibModal', 'MapService', 'PropertyService', 'GovernmentService',
    function ($window, $scope, $timeout, $filter, $uibModal, mapService, propertyService, governmentService) {
        $scope.time = 0;
        $scope.suggestionTime = 0;
        var init = true;
    var map = $scope.map = null;
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

    $scope.mapType =$scope.mapType?$scope.mapType: -1;   
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
            nextStep: { mapType: 0, auction: false, injection: false, storeUp: false, adjust: false, ct: false, jt: false },
            constructArea: { ranges: [], l: false, m: false, h: false, t: false },
            landArea: { ranges: [], l: false, m: false, h: false, t: false },
            price: { ranges: [], l: false, m: false, h: false, t: false },
            lifeTime: { min: 0, max: 70 },
            getedDate: { min: 0, max: 0 }
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

        var options = {
            position: 'bottomright',
            draw: {
                polyline: false,
                polygon: {
                    allowIntersection: false, // Restricts shapes to simple polygons
                    drawError: {
                        color: '#e1e100', // Color the shape will turn when intersects
                        message: '<strong>绘制错误！<strong> 多边形不能自重叠' // Message that will show when intersect
                    },
                    shapeOptions: {
                        color: '#bada55'
                    }
                },
                circle: {
                    showRadius: true,
                    metric: false, // Whether to use the metric measurement system or imperial
                    feet: false, // When not metric, use feet instead of yards for display
                    nautic: false // When not metric, not feet use nautic mile for display
                }, // Turns off this drawing tool
                rectangle: {
                    shapeOptions: {
                        clickable: false
                    }
                },
                marker: false
            },
            edit: {
                featureGroup: $scope.editableLayers, //REQUIRED!!
                remove: false
            }
        };

        var drawControl = new L.Control.Draw(options);
        //map.addControl(drawControl);

        //绘制空间查询范围

        var drawPolygon = new L.Draw.Polygon(map, options.polygon);
        var drawRectangle = new L.Draw.Rectangle(map, options.rectangle);
        var drawCircle = new L.Draw.Circle(map, options.circle);

        //要素绘制事件
        map.on(L.Draw.Event.CREATED, function (e) {
            var type = e.layerType, layer = e.layer;

            layer.addTo($scope.editableLayers);
            layer.editing.enable();
        });

        //汉化
        L.drawLocal.draw.handlers.circle = {
            tooltip: {
                start: '点击并拖拽生成圆'
            },
            radius: '半径'
        };
        L.drawLocal.draw.handlers.polygon = {
            tooltip: {
                start: '点击开始绘制多边形',
                cont: '点击继续绘制多边形',
                end: '点击第一个闭合多边形'
            }
        };
        L.drawLocal.draw.handlers.rectangle = {
            tooltip: {
                start: '点击并拖拽绘制矩形'
            }
        };
        L.drawLocal.draw.handlers.simpleshape = {
            tooltip: {
                end: '释放鼠标结束绘制'
            }
        };

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

            //console.log(newValue);
            //console.log(oldValue);

            if (!init) $scope.loadMore();
        });

        $scope.setMapType(0);
    }, 50);

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

        //var mapOverlayOption = {
        //    icon: new L.Icon.Default()
        //};

        cluseter.markers = [];
        cluseter.markerClusterGroup = L.markerClusterGroup({ chunkedLoading: true });

        angular.forEach($scope.properties, function (property, v) {
           
            var constructColor = $scope.mapType == 1 ? (property.next == 0 ? "red" : "green") : "red";
            var landColor = $scope.mapType == 1 ? (property.next == 0 ? "red" : "green") : "orange";
            var landUnderColor = $scope.mapType == 1 ? (property.next == 0 ? "red" : "green") : "purple";

            var constrcutOption = {
                icon: L.AwesomeMarkers.icon({ markerColor: constructColor, iconColor: 'white' })
            };
            var landOption = {
                icon: L.AwesomeMarkers.icon({ icon: 'clone', markerColor: landColor, prefix: 'fa', iconColor: 'white' }),
            };
            var landUnderConstructOption = {
                icon: L.AwesomeMarkers.icon({ icon: 'object-ungroup', markerColor: landUnderColor, iconColor: 'white' })
            };
            wkt.read(property.location);

            var location = null;

            switch (property.propertyType) {
                case "房屋":
                    location = wkt.toObject(constrcutOption);
                    break;
                case "土地":
                    location = wkt.toObject(landOption);
                    break;
                case "对应房屋土地":
                    location = wkt.toObject(landUnderConstructOption);
                    break;
            }

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

            //移出
            location.on('remove', function () {
                if ($scope.showHoverExtent != null)
                    map.removeLayer($scope.showHoverExtent);
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

    //设置地图类型
    $scope.setMapType = function (type,reset) {
        if ($scope.mapType == type && !reset) return;
        $scope.mapType = type;
        $scope.search.params.nextStep.mapType = type;
        switch (type) {
            case 0:
                //总图
                $scope.search.params.nextStep.auction = false;
                $scope.search.params.nextStep.injection = false;
                $scope.search.params.nextStep.storeUp = false;
                $scope.search.params.nextStep.adjust = false;
                $scope.search.params.nextStep.ct = false;
                $scope.search.params.nextStep.jt = false;
                break;
            case 1:
                //注入国资
                $scope.search.params.nextStep.auction = false;
                $scope.search.params.nextStep.injection = true;
                $scope.search.params.nextStep.storeUp = false;
                $scope.search.params.nextStep.adjust = false;
                $scope.search.params.nextStep.ct = true;
                $scope.search.params.nextStep.jt = true;
                break;
            case 2:
                //拍卖处置
                $scope.search.params.nextStep.auction = true;
                $scope.search.params.nextStep.injection = false;
                $scope.search.params.nextStep.storeUp = false;
                $scope.search.params.nextStep.adjust = false;
                $scope.search.params.nextStep.ct = false;
                $scope.search.params.nextStep.jt = false;
                break;
            case 3:
                //国有收储
                $scope.search.params.nextStep.auction = false;
                $scope.search.params.nextStep.injection = false;
                $scope.search.params.nextStep.storeUp = true;
                $scope.search.params.nextStep.adjust = false;
                $scope.search.params.nextStep.ct = false;
                $scope.search.params.nextStep.jt = false;
                break;
        }

        $scope.searchProperties(true);
    };

    //加载大数据
    $scope.loadPropertiesBigData = function () {
        propertyService.getPropertiesBigDataInMap().then(function (data) {
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

        switch ($scope.search.params.extent.type) {
            case 'rectangle':
            case 'circle':
            case 'polygon':
                $scope.editableLayers.eachLayer(function (layer) {

                    if ($scope.search.params.extent.type == "circle") {
                        wkt.fromObject(mapService.convertCircleToPolygon(layer));
                    }
                    else {
                        wkt.fromObject(layer);
                    }

                    $scope.search.params.extent.geo = wkt.write();
                });

                break;
                //#region 视野内搜索传入四至坐标
            case 'current':
                var bound = map.getBounds();

                var west = bound.getWest();
                var south = bound.getSouth();
                var east = bound.getEast();
                var north = bound.getNorth();
                $scope.search.params.extent.geo = "POLYGON((" + west + " " + south + ","
                    + west + " " + north + ","
                    + east + " " + north + ","
                    + east + " " + south + ","
                    + west + " " + south + "))";
                break;
                //#endregion
            default:
                $scope.search.params.extent.geo = "";
                break;
        }

        if ($scope.government.selected != null && $scope.government.selected != undefined) $scope.search.params.government.selectedId = $scope.government.selected.id;
        else $scope.search.params.government.selectedId = 0;

        //#region 建筑面积、土地面积、账面价值区间参数

        //#region 建筑面积
        $scope.search.params.constructArea.ranges = [];

        if ($scope.search.params.constructArea.l)
            $scope.search.params.constructArea.ranges.push({ min: 0, max: 5000 });

        if ($scope.search.params.constructArea.m)
            $scope.search.params.constructArea.ranges.push({ min: 5001, max: 10000 });

        if ($scope.search.params.constructArea.h)
            $scope.search.params.constructArea.ranges.push({ min: 10001, max: 20000 });

        if ($scope.search.params.constructArea.t)
            $scope.search.params.constructArea.ranges.push({ min: 20001, max: 0 });

        //#endregion

        //#region 土地面积
        $scope.search.params.landArea.ranges = [];

        if ($scope.search.params.landArea.l)
            $scope.search.params.landArea.ranges.push({ min: 0, max: 50 * 666.67 });

        if ($scope.search.params.landArea.m)
            $scope.search.params.landArea.ranges.push({ min: 51 * 666.67, max: 300 * 666.67 });

        if ($scope.search.params.landArea.h)
            $scope.search.params.landArea.ranges.push({ min: 301 * 666.67, max: 500 * 666.67 });

        if ($scope.search.params.landArea.t)
            $scope.search.params.landArea.ranges.push({ min: 501*666.67, max: 0 });

        //#endregion

        //#region 账面价值
        $scope.search.params.price.ranges = [];

        if ($scope.search.params.price.l)
            $scope.search.params.price.ranges.push({ min: 0, max: 500 });

        if ($scope.search.params.price.m)
            $scope.search.params.price.ranges.push({ min: 501, max: 5000 });

        if ($scope.search.params.price.h)
            $scope.search.params.price.ranges.push({ min: 5001, max: 10000 });

        if ($scope.search.params.price.t)
            $scope.search.params.price.ranges.push({ min: 10001, max: 0 });

        //#endregion

        //#region 下步打算
        $scope.search.params.nextStep.storeUp = $scope.search.params.nextStep.storeAndDev;
        $scope.search.params.nextStep.development = $scope.search.params.nextStep.storeAndDev;
        //#endregion

        //#endregion

        $scope.search.params.time = new Date().getTime();
        propertyService.getAllPropertiesInMap(params).then(function (response) {
            if ($scope.time > response.time) return;

            $scope.time = response.time;
            $scope.properties = response.data;
            suggestedProperties = $scope.suggestedProperties = [];

            $scope.search.config.filterModel = true;

            $scope.search.config.searching = false;

            $scope.selectedProperty = null;
        }, function () {
            alert("网络异常");

            //$scope.properties = [];
            suggestedProperties = $scope.suggestedProperties = [];

            $scope.search.config.filterModel = true;

            $scope.search.config.searching = false;

            $scope.selectedProperty = null;
        });
    };

    //重置高级搜索
    $scope.resetAdvance = function () {

        $scope.search.config.advanceModel = false;  //进入高级搜索模式
        $scope.search.config.filterModel = false;

        //参数重置
        $scope.search = angular.copy(search);

        //绘制范围清空
        $scope.editableLayers.clearLayers();

        $scope.setMapType($scope.mapType, true);
    };

    //选中资产
    $scope.selectProperty = function (propertyId, zoom) {

        if ($scope.selectedProperty != null && $scope.selectedProperty.id == propertyId) return;

        propertyService.getPropertyById(propertyId).then(function (data) {
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

        propertyService.propertiesSuggest(params).then(function (response) {
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

    //显示图表
    $scope.showCharts = function () {
        search.config.showChart = true;

        $scope.statistics = {
            overview: {
                totalCount: 0,
                constructCount: 0,
                landCount: 0,
                totalPrice: 0,
                constructPrice: 0,
                landPrice: 0,
                constructArea: 0,
                constrcutLandArea: 0,
                landArea: 0
            },
            currentUsage: {
                chart1: [{ name: "自用", value: 0 }, { name: "出借", value: 0 }, { name: "出租", value: 0 }, { name: "闲置", value: 0 }],
                chart2: [{ name: "自用", value: 0 }, { name: "出借", value: 0 }, { name: "出租", value: 0 }, { name: "闲置", value: 0 }]
            }
        };

        angular.forEach($scope.properties, function (i, v) {
            $scope.statistics.overview.totalCount++;
            $scope.statistics.overview.totalPrice += i.price;

            switch(i.propertyType)
            {
                case "房屋":
                    $scope.statistics.overview.constructCount++;
                    $scope.statistics.overview.constructArea += i.constructArea;
                    $scope.statistics.overview.constrcutLandArea += i.landArea;
                    $scope.statistics.overview.constructPrice += i.price;

                    $scope.statistics.currentUsage.chart1[0].value += i.c_Self;
                    $scope.statistics.currentUsage.chart1[1].value += i.c_Lend;
                    $scope.statistics.currentUsage.chart1[2].value += i.c_Rent;
                    $scope.statistics.currentUsage.chart1[3].value += i.c_Idle;

                    break;
                case "土地":
                    $scope.statistics.overview.landCount++;
                    $scope.statistics.overview.landArea += i.landArea;
                    $scope.statistics.overview.landPrice += i.price;

                    $scope.statistics.currentUsage.chart2[0].value += i.c_Self;
                    $scope.statistics.currentUsage.chart2[1].value += i.c_Lend;
                    $scope.statistics.currentUsage.chart2[2].value += i.c_Rent;
                    $scope.statistics.currentUsage.chart2[3].value += i.c_Idle;

                    break;
                case "对应房屋土地":
                    $scope.statistics.overview.constructCount++;
                    $scope.statistics.overview.constrcutLandArea += i.landArea;
                    break;
            }           
        });

        var modalInstance = $uibModal.open({
            templateUrl: 'dialog.html',
            controller: 'ModalDialogCtrl',
            size: 'lg',
            windowClass: "map-modal",
            //appendTo: '#app',
            resolve: {
                dialogHeight: function () { return $scope.dialogHeight; },
                statistics: function () { return $scope.statistics; },
                title: function () { return "统计结果"; },
                content: function () { },
                opened: function () {
                    $timeout(function () {

                        //#region 使用现状统计

                        var myChart1 = echarts.init(document.getElementById('chart1'));

                        var option1 = {
                            tooltip: {
                                trigger: 'item',
                                formatter: function (column) {
                                    var html = column.seriesName + '<br>';
                                    html += column.name + '：' + $filter('unit')(column.value, "carea") + ' (' + column.percent + '%)';
                                    return html;
                                }
                            },
                            toolbox: {
                                show: true,
                                feature: {
                                    mark: { show: true },
                                    dataView: { show: true, readOnly: true }
                                }
                            },
                            legend: {
                                orient: 'vertical',
                                left: 'left',
                                data: ['自用', '出租', '出借', '闲置']
                            },
                            series: [
                                {
                                    name: '使用现状',
                                    type: 'pie',
                                    radius: '55%',
                                    center: ['50%', '60%'],
                                    data: $scope.statistics.currentUsage.chart1,
                                    itemStyle: {
                                        emphasis: {
                                            shadowBlur: 10,
                                            shadowOffsetX: 0,
                                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                                        }
                                    }
                                }
                            ],
                            color: ['#c23531', '#2f4554', '#61a0a8', '#749f83']
                        };

                        myChart1.setOption(option1);
                        var myChart2 = echarts.init(document.getElementById('chart2'));
                        var option1 = {
                            tooltip: {
                                trigger: 'item',
                                formatter: function (column) {
                                    var html = column.seriesName + '<br>';
                                    html += column.name + '：' + $filter('unit')(column.value, "larea") + ' (' + column.percent + '%)';
                                    return html;
                                }
                            },
                            toolbox: option1.toolbox,
                            legend: option1.legend,
                            series: [
                                {
                                    name: '使用现状',
                                    type: 'pie',
                                    radius: '55%',
                                    center: ['50%', '60%'],
                                    data: $scope.statistics.currentUsage.chart2,
                                    itemStyle: {
                                        emphasis: {
                                            shadowBlur: 10,
                                            shadowOffsetX: 0,
                                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                                        }
                                    }
                                }
                            ],
                            color: option1.color
                        };
                        myChart2.setOption(option1);

                        //#endregion
                    }, 500);



                }
            }
        });

    };

    init = false;
}]);