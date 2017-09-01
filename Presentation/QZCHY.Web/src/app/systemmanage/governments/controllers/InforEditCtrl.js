"use strict";

app.controller("InforEditCtrl", ['$scope', '$rootScope', '$uibModal', '$state', 'AccountService', 'GovernmentService', 'w5cValidator', 'toaster', '$timeout',
    function ($scope, $rootScope, $uibModal, $state, accountService, governmentService, w5cValidator, toaster, $timeout) {

        var goverment = {
            id:0,
            name: "",
            governmentType: "",
            address: "",
            person: "",
            tel: "",
            remark: "",
            creditCode: "",
            personNumber:0
        };

        governmentService.getCurrentGovernment().then(function (response) {
            $scope.government = response;
            $scope.address = response.address;
            $scope.person = response.person;
            $scope.tel = response.tel;
            
        })


        //验证
        w5cValidator.setRules({
            gaddress: {
                required: "单位地址不能为空"
            },
            gperson: {
                required: "联系人不能为空"
            },
            gtel: {
                required: "联系方式不能为空"
            },
            ccname: {
                required: "同一信用代码不能为空"
            },
            pnname: {
                required: "人员编制不能为空"
            },

        });

        var validation = $scope.validation = {
            options: {
                blurTrig: true
            }
        };
        //提交编辑
        validation.saveEntity = function ($event) {

            goverment.name = $scope.government.name;
            goverment.governmentType = $scope.government.governmentType;
            goverment.address = $scope.address;
            goverment.person = $scope.person;
            goverment.tel = $scope.tel;
            goverment.remark = $scope.description;
            goverment.id = $scope.government.id;
            goverment.creditCode = $scope.creditCode;
            goverment.personNumber = $scope.personNumber;

            governmentService.updateGovernment(goverment).then(function () {
                toaster.pop("sucess", "修改成功", "", 500);
                $timeout(function () {
                 // $state.go("access.signin");
                }, 550);
            }, function (message) {
                $scope.errorMsg = message;
                toaster.pop("error", "修改失败", "", 500);
            }).finally(function () {
                $scope.processing = false;
            });
        }

       

    }]);