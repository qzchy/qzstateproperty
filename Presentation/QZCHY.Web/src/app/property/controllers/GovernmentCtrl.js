"use strict";

app.controller("GovernmentCtrl", ['$scope', '$rootScope', '$uibModal', '$timeout', '$state', 'GovernmentService', 'PropertyService',
    function ($scope, $rootScope, $uibModal, $timeout, $state, governmentService, propertyService) {

        var loadChildrenProperties = $scope.loadChildrenProperties = true;

        var government = $scope.government = {
            id: 0,
            name: "",
            governmentType: false,
            address: true,
            person: "",
            tel: "",
            parentGovernment: "",
            childrenGorvernmentsName: ""
        };
       
        if ($scope.governmentId != undefined && $scope.governmentId != "" && $scope.governmentId != null) {
            governmentService.getGovernmentById($scope.governmentId).then(function (data) {
                government = $scope.government = data;

                $scope.getProperties();
            
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
            propertyService.deleteProperty($scope.governmentId).then(function () {
                $state.go("app.property.list");
            }, function (msg, status) {
                $scope.errorMsg = msg;
            });
        }, function () {
            $state.reload();
        });

    };

    $scope.getProperties = function () {
        //获取单位下的资产
        propertyService.getPropertiesByGovernment(government.id, $scope.loadChildrenProperties).then(function (data) {
            government.properties = data;
        }, function () {

        });
    };

}]);