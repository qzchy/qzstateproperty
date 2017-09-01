"use strict";

app.controller("ResetpwdCtrl", ['$scope', '$rootScope', '$uibModal', '$state', 'AccountService', 'GovernmentService', 'w5cValidator', 'toaster', '$timeout',
    function ($scope, $rootScope, $uibModal, $state, accountService, governmentService, w5cValidator, toaster, $timeout) {


        var params= {
            userName: "",
            password: "",
            newpassword: "",
            againpassword:""
        };

        console.log();

        //AccountService.getAccountByName().then(function (response) {

        //    $scope.account = response;

        //})

        //验证
        w5cValidator.setRules({
            opassword: {
                required: "原密码不能为空"
            },
            npassword: {
                required: "新密码不能为空"
            },
            apassword: {
                required: "新密码不能为空"
            },

        });

        var validation = $scope.validation = {
            options: {
                blurTrig: true
            }
        };
        //提交编辑
        validation.saveEntity = function ($event) {

            params.userName = $scope.accountName;
            params.password = $scope.password;
            params.newpassword = $scope.newpassword;
            params.againpassword = $scope.againpassword;

            accountService.resetPwd(params).then(function () {
                toaster.pop("sucess", "修改成功", "", 500);
                $timeout(function () {
                  $state.go("access.signin");
                }, 550);
            }, function (message) {
                $scope.errorMsg = message;
                toaster.pop("error", "修改失败", "", 500);
            }).finally(function () {
                $scope.processing = false;
            });
        }

       

    }]);