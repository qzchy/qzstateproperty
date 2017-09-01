"use strict";

app.controller("PropertyCtrl", ['$scope', '$rootScope', '$uibModal', '$timeout', '$filter', '$state', 'PropertyService', 'MapService',
    function ($scope, $rootScope,$uibModal,$timeout,$filter, $state, propertyService,mapService) {
        var wkt = new Wkt.Wkt();

   
        var property = $scope.property = {
            id: 0,
            name: "",
            propertyType: "",
            propertyTypeId: 0,
            region: "",
            regionId: 0,
            address: "",
            constructArea: 0,
            landArea: 0,
            landPropertyID: "",
            constructPropertyID: "",
            propertyNature: "",
            landNature: "",
            price: 0,
            getedDate: "",
            lifeTime: 0,
            usedPeople: "",
            currentUse_Self: 0,
            currentUse_Rent: 0,
            currentUse_Lend: 0,
            currentUse_Idle: 0,
            nextStepUsage: "",
            nextStepUsageId: 0,
            location: "",
            extent: "",
            description: "",
            governmentId: 0,
            governmentName: "",
            landAreaRange:""
            //lends: [],
            //rents: [],
            //allots: [],
            //newCreate: {},
            //off: {}
        };
        var map = $scope.map;
        var mapOverlayOption = {
            icon: new L.Icon.Default(),
            color: '#AA0000',
            weight: 3,
            opacity: 1.0,
            fillColor: '#AA0000',
            fillOpacity: 0.2
        };

        if ($scope.propertyId != undefined && $scope.propertyId != "" && $scope.propertyId != null) {
        propertyService.getPropertyById($scope.propertyId).then(function (data) {
            property = $scope.property = data;
            console.log(property);


            // #engion 基于准备好的dom，初始化echarts实例
            var myChart = echarts.init(document.getElementById('chart'));

            var option = {
                tooltip: {
                    trigger: 'item',
                    formatter: function (column) {
                        var html = column.seriesName + '<br>';
                        html += column.name + '：' + $filter('unit')(column.value, "carea") + ' (' + column.percent + '%)';
                        return html;
                    }
                },
                legend: {
                    x: 'center',
                    y: 'bottom',
                    data: ['自用面积', '出租面积', '出借面积', '闲置面积']
                },
                toolbox: {
                    show: true,
                    feature: {
                        mark: { show: true },
                        dataView: { show: true, readOnly: true },
                        //magicType: {
                        //    show: true,
                        //    type: ['pie', 'funnel']
                        //},
                        //restore: { show: true },
                        //saveAsImage: { show: true }
                    }
                },
                calculable: true,
                series: [
                    {
                        name: '使用现状',
                        type: 'pie',
                        radius: [20, 110],
                        //center: ['50%', '50%'],
                        roseType: 'radius',
                        label: {
                            normal: {
                                show: false
                            },
                            emphasis: {
                                show: true
                            }
                        },
                        lableLine: {
                            normal: {
                                show: false
                            },
                            emphasis: {
                                show: true
                            }
                        },
                        data: []
                    }
                ],
                color: ['#c23531', '#2f4554', '#61a0a8', '#749f83']
            };

            // 使用刚指定的配置项和数据显示图表。
            myChart.setOption(option);


            // 填入数据
            myChart.setOption({
                //xAxis: {
                //    data: data.categories
                //},
                series: [{
                    // 根据名字对应到相应的系列
                    data: [
                        { value: property.currentUse_Self, name: '自用面积' },
                        { value: property.currentUse_Rent, name: '出租面积' },
                        { value: property.currentUse_Lend, name: '出借面积' },
                        { value: property.currentUse_Idle, name: '闲置面积' }
                    ]
                }]
            });

            $timeout(function () {

                var normal = mapService.getLayer("vector", "Normal");
                var satellite = mapService.getLayer("img", "Satellite");

                map = L.map('map', {
                    crs: L.CRS.EPSG4326, center: { lon: 118.8656, lat: 28.9718 }, zoom: 15, layers: [normal]
                });
                mapService.setMapAttribute(map);
                var baseMaps = {
                    "矢量": normal,
                    "影像": satellite
                }; 
                L.control.layers(baseMaps).addTo(map);                         

                try
                {
                    if (property.extent != null && property.extent != "") {
                        wkt.read(property.extent);
                        var extent = wkt.toObject(mapOverlayOption);
                        extent.addTo(map);

                        map.fitBounds(extent.getBounds());
                    }
                    else {
                        wkt.read(property.location);

                        var location = L.marker([wkt.components[0].y, wkt.components[0].x]);// wkt.toObject(mapOverlayOption);

                        location.addTo(map);
                        map.setView(location.getLatLng(), 18);
                    }
                }
                catch (e) {
                    //alert("加载位置失败！")
                    wkt.read(property.location);

                    var location = L.marker([wkt.components[0].y, wkt.components[0].x]);// wkt.toObject(mapOverlayOption);

                    location.addTo(map);
                    map.setView(location.getLatLng(), 18);
                }
            }, 50);
        }, function (message) {
            $state.go("app.property.list");
        });
    }
    else $state.go("app.property.list");


    $scope.deleteProperty = function () {

        var modalInstance = $uibModal.open({
            templateUrl: 'dialog.html',
            controller: 'ModalDialogCtrl',
            resolve: {
                title: function () { return "提示"; },
                content: function () { return "是否删除资产？"; }
            }
        });

        modalInstance.result.then(function () {
            propertyService.deleteProperty($scope.propertyId).then(function () {
                $state.go("app.property.list");
            }, function (msg, status) {
                $scope.errorMsg = msg;
            });
        }, function () {
            $state.reload();
        });

    };

        //加载所有出借信息
    $scope.showLends = function (id) {

        propertyService.getLendsByPropertyId(id).then(function (response) {
            $scope.lends = response;
        })
    };
     

    }]);