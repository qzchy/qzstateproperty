"use strict";

app.controller("AccountCtrl", ['$scope', '$rootScope', '$uibModal', '$state', 'AccountService', 'GovernmentService',
    function ($scope, $rootScope, $uibModal, $state, accountService, governmentService) {


        var account = $scope.account = {
            id: 0,
            governmentId:0,
            userName: "",
            isAdministrator: false,
            active: true,
            lastIpAddress: "",
            lastLoginDate: "",
            lastActivityDate: "",
            remark: ""
        };

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

        if ($scope.accountId != undefined) {
            accountService.getAccountById($scope.accountId).then(function (data) {
                account = $scope.account = data;
            }, function (message) {
                $state.go("app.systemmanage.account.list");
            }).then(function () {
                governmentService.getGovernmentById($scope.account.governmentId).then(function (data) {
                    government = $scope.government = data;
                }, function (message) {
                    $state.go("app.systemmanage.account.list");
                });
            }, function () { $state.go("app.systemmanage.account.list"); });

         
        }
        else
            $state.go("app.systemmanage.account.list");


        $scope.deleteAccount = function () {

            var modalInstance = $uibModal.open({
                templateUrl: 'dialog.html',
                controller: 'ModalDialogCtrl',
                windowClass: "map-modal",
                resolve: {
                    title: function () { return "提示"; },
                    content: function () { return "是否删除用户？"; },
                    dialogHeight: function () { return $rootScope.dialogHeight; },
                    statistics: function () { return null; }
                }
            });

            modalInstance.result.then(function () {
                accountService.deleteAccount($scope.accountId).then(function () {
                    $state.go("app.systemmanage.account.list");
                }, function (msg, status) {
                    $scope.errorMsg = msg;
                });
            }, function () {
                $state.reload();
            });

        };

    }]);