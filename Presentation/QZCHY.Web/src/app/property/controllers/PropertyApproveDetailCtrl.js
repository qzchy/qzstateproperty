//资产列表控制器
'use strict';
 
app.controller('PropertyApproveDetailCtrl', ['$window', '$rootScope', '$uibModal', '$state', '$scope', '$timeout', 'toaster','PropertyService', 'MapService',
    function ($window, $rootScope, $uibModal, $state, $scope, $timeout, toaster,propertyService, mapService) {
        var mapOverlayOption = {
            icon: new L.Icon.Default(),
            color: '#AA0000',
            weight: 3,
            opacity: 1.0,
            fillColor: '#AA0000',
            fillOpacity: 0.2
        };
        var wkt = new Wkt.Wkt();
        var map = null;
        var map1 = null;

        var approveId = $scope.approveId;
        var approveType = $scope.approveType;

        propertyService.getApproveDetail(approveType, approveId).then(function (response) {
            $scope.approve = response;
            var approve = $scope.approve;

            if (approveType == "rent") {
 
                var prices = approve.propertyRent.priceString.split(";");
                var backtime = moment(approve.propertyRent.backTime);
                var renttime = moment(approve.propertyRent.rentTime);

                var diff = backtime.diff(renttime, "year", true);
                var num = Math.ceil(diff);
                $scope.yearNumber = [];
              
                for (var i = 1; i <= num; i++) {
                    var d = {
                        index: i,
                        model: prices[i - 1]
                    }
                    $scope.yearNumber.push(d);
                }

                approve.propertyRent.rentTime = moment(approve.propertyRent.rentTime).format("MM/DD/YYYY") + " - " + moment(approve.propertyRent.backTime).format("MM/DD/YYYY");

            }

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

                try {
                    if (approve.property.extent != null && approve.property.extent != "") {
                        wkt.read(approve.property.extent);
                        var extent = wkt.toObject(mapOverlayOption);
                        extent.addTo(map);

                        map.fitBounds(extent.getBounds());
                    }
                    else {
                        wkt.read(approve.property.location);

                        var location = L.marker([wkt.components[0].y, wkt.components[0].x]);// wkt.toObject(mapOverlayOption);

                        location.addTo(map);
                        map.setView(location.getLatLng(), 18);
                    }
                }
                catch (e) {
                    //alert("加载位置失败！")
                    wkt.read(approve.property.location);

                    var location = L.marker([wkt.components[0].y, wkt.components[0].x]);// wkt.toObject(mapOverlayOption);

                    location.addTo(map);
                    map.setView(location.getLatLng(), 18);
                };

                if (approveType == 'edit') {
                    var normal1 = mapService.getLayer("vector", "Normal");
                    var satellite1 = mapService.getLayer("img", "Satellite");

                    map1 = L.map('map1', {
                        crs: L.CRS.EPSG4326, center: { lon: 118.8656, lat: 28.9718 }, zoom: 15, layers: [normal1]
                    });


                    mapService.setMapAttribute(map1);

                    var baseMaps1 = {
                        "矢量": normal1,
                        "影像": satellite1
                    };
                    L.control.layers(baseMaps).addTo(map);
                    L.control.layers(baseMaps1).addTo(map1);

                    try {
                        if (approve.copyProperty.extent != null && approve.copyProperty.extent != "") {
                            wkt.read(approve.copyProperty.extent);
                            var extent = wkt.toObject(mapOverlayOption);
                            extent.addTo(map1);

                            map1.fitBounds(extent.getBounds());
                        }
                        else {
                            wkt.read(approve.copyProperty.location);

                            var location = L.marker([wkt.components[0].y, wkt.components[0].x]);// wkt.toObject(mapOverlayOption);

                            location.addTo(map1);
                            map1.setView(location.getLatLng(), 18);
                        }
                    }
                    catch (e) {
                        //alert("加载位置失败！")
                        wkt.read(approve.copyProperty.location);

                        var location = L.marker([wkt.components[0].y, wkt.components[0].x]);// wkt.toObject(mapOverlayOption);

                        location.addTo(map1);
                        map1.setView(location.getLatLng(), 18);
                    };
                }


            }, 50);
        });



      

        $scope.approve = {};

        $scope.deleteProperty = function () {

            var modalInstance = $uibModal.open({
                templateUrl: 'dialog.html',
                controller: 'ModalDialogCtrl',
                resolve: {
                    dialogHeight: function () { return $rootScope.dialogHeight; },
                    title: function () { return "提示"; },
                    content: function () { return "是否删除该处置申请？"; }
                }
            });

            modalInstance.result.then(function () {

                if(approveType=="newCreate")
                {
                    propertyService.deleteProperty($scope.approve.property.id).then(function () {
                        $state.go("app.property.process_approve");
                        toaster.pop("success", "删除成功", "", 500);
                    }, function (msg, status) {
                        $scope.errorMsg = msg;
                    });
                }
                else
                {
                    propertyService.deletePropertyApprove($scope.approveId, approveType).then(function () {
                        $state.go("app.property.process_approve");
                        toaster.pop("success", "删除成功", "", 500);
                    }, function (msg, status) {
                        $scope.errorMsg = msg;
                    });
                }
                    
            }, function () {
                $state.reload();
            });

        };

        $scope.showApproveDialog = function () {

            var modalInstance = $uibModal.open({
                templateUrl: 'approveDialog.html',
                controller: 'ExcuteApproveDialogCtrl',
                size: 'lg',
                resolve: {
                    dialogHeight: function () { return $rootScope.dialogHeight; },
                    excuteApprove: function () { return $scope.excuteApprove; }
                }
            });

            modalInstance.result.then(function () {
            }, function () {
                $state.reload();
            });

        };

        //提交申请
        $scope.submitApprove = function () {

            propertyService.submitApprove($scope.approveId, approveType).then(function () {
                toaster.pop('success', '', '提交成功');
                setTimeout(function () {
                    $state.go("app.property.process_approve", { approveType: approveType });
                }, 600);
                
            }, function (msg, status) {
                toaster.pop('error', '', '提交失败');
                $scope.errorMsg = msg;
            });

        };

        //资产审批提交
        $scope.excuteApprove = function (agree, suggestion) {
            propertyService.applyApprove($scope.approveId, agree, suggestion, approveType).then(function (response) {
                toaster.pop("success", "审核成功", "", 500);
            },
            function (msg) {
                toaster.pop("error", "审核失败，" + msg, "", 500);
            }).finally(function () {
                setTimeout(function () {
                    $state.go("app.property.process_approve", { approveType: approveType });
                },600);
            });
        };
    }]);
app.controller('ExcuteApproveDialogCtrl', function ($scope, $uibModalInstance, dialogHeight, excuteApprove) {

    $scope.dialogHeight = dialogHeight;
    $scope.suggestion = "";

    $scope.ok = function () {
        excuteApprove(true, $scope.suggestion);

        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        excuteApprove(false, $scope.suggestion);

        $uibModalInstance.close();
    };

});