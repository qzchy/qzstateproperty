"use strict";

app.controller("AccountCreateOrEditCtrl", ['$scope', '$rootScope', '$state', 'moment', 'toaster','w5cValidator', '$uibModal', 'AccountService','GovernmentService',
    function ($scope, $rootScope, $state, moment,toaster, w5cValidator, $uibModal, accountService, governmentService) {

        $scope.errorMsg = null;
        $scope.accountProcessing = true;    //正在请求product数据
        $scope.submitProcessing = false;    //正在提交表单
        $scope.processingStatus = false;
        //设置页面处理状态
        $scope.updateProcessingStatus = function () {
            $scope.processingStatus = $scope.accountProcessing || $scope.submitProcessing;
        };

        //高级搜索的部门搜索 
        $scope.governments = [];
        $scope.government = {};

        //实体
        $scope.account = {
            userName: "",
            nickName: "",
            governmentId: 0,
            governmentName:"",
            //password: "",
            roleName: "",
            active: false,
            remark: "",
            initPassword: $scope.accountId == undefined
        };

        //初始化处理状态
        $scope.updateProcessingStatus();

        //#region 验证

        //验证规则
        w5cValidator.setRules({
            aname: {
                required: "用户名称不能为空",
                w5cuniquecheck: "输入用户名已经存在，请重新输入"
            },
            anickname: {
                required: "昵称不能为空"
            },
            apassword:{
                required: "用户名称不能为空",
                minlength: "密码应在6-15位",
                maxlength: "密码应在6-15位"
            },
            agovernment: {
                required: "用户单位不能为空"
            }
        });

        //每个表单的配置，如果不设置，默认和全局配置相同
        var validation = $scope.validation = {
            options: {
                blurTrig: true
            }
        };

        //保存实体
        validation.saveEntity = function ($event, continueEditing) {
            $scope.submitProcessing = true;
            $scope.updateProcessingStatus();

            if ($scope.account.id != undefined && $scope.account.id != null) {
                
                accountService.updateAccount($scope.account).then(function (data) {
                    $scope.errorMsg = "";

                    toaster.pop('success', '', '保存成功');

                    if (continueEditing) $state.reload();
                    else $state.go("app.systemmanage.account.list");
                }, function (message) {
                    toaster.pop('error', '', '保存失败');

                    $scope.errorMsg = message;
                })
                .finally(function () {
                    $scope.submitProcessing = false;
                    $scope.updateProcessingStatus();
                });
            }
            else {
            
                //新增用户
                accountService.createAccount($scope.account).then(function (data) {
                    $scope.errorMsg = "";
                    toaster.pop('success', '', '保存成功');

                    if (continueEditing) $state.go("app.systemmanage.account.edit", { id: data.Id });
                    else $state.go("app.systemmanage.account.list");

                }, function (message) {
                    toaster.pop('error', '', '保存失败');
                    $scope.errorMsg = message;
                }).finally(function () {
                    $scope.submitProcessing = false;
                    $scope.updateProcessingStatus();
                });
            }
        };

        //#endregion

        //获取account
        if ($scope.accountId >0) {
            accountService.getAccountById($scope.accountId).then(function (data) {
                $scope.account = data;
                //设置默认值
                $scope.nameDefaultValue = $scope.account.userName;

                $scope.governments = [{ "name": $scope.account.governmentName, "id": $scope.account.governmentId, "customProperties": {} }];
                $scope.government = { "selected": { "name": $scope.account.governmentName, "id": $scope.account.governmentId, "customProperties": {} } };

            }, function (message) {

                $scope.errorMsg = message;
            }).finally(function () {
                $scope.accountProcessing = false;
                $scope.updateProcessingStatus();
            });
        }
        else
        {
            $scope.accountProcessing = false;
            $scope.updateProcessingStatus();
        }

        //关键词搜索主管部门  
        $scope.refreshGovernments = function (governmentName) {
            if (governmentName == "" || governmentName == null || governmentName == undefined) return;
            governmentService.autocompleteByName(governmentName).then(function (response) {
                $scope.governments = response.data;
            });
        };

        $scope.clearGovernment = function () {

            $scope.government.selected = null;
        };

        $scope.$watch("government", function (newValue, oldValue) {

            if (newValue != null && newValue != undefined&&newValue.selected!=null) {
                $scope.account.governmentId = newValue.selected.id;
                $scope.account.governmentName = newValue.selected.name;
            }
            else
            {

                $scope.account.governmentId = 0;
                $scope.account.governmentName = "";
            }
        }, true);
    }]);