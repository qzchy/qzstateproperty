//资产列表控制器
'use strict';
 
app.controller('PropertyProcessApproveCtrl', ['$window', '$rootScope', '$uibModal', '$state', '$scope', '$timeout','toaster', 'PropertyService', 'GovernmentService',
    function ($window, $rootScope, $uibModal, $state, $scope, $timeout,toaster, propertyService, governmentService) {
        //#region 列集合
        var columns1 = [{
            title: "申请时间",
            width: 180,
            name: "processDate",
            orderable: true,
            alignCenter: true,
            dir: "desc"
        }, {
            type: 'link',
            title: "资产名称",
            name: "title",
            orderable: true,
            linkField: "property_Id",
            click: function (propertId) {
                $state.go("app.property.detail", { id: propertId });
            },
        }, {
            title: "审批状态",
            width: 150,
            name: "state",
            orderable: true,
            alignCenter: true
        }];

        var columns2 = [{
            title: "申请时间",
            width: 180,
            name: "processDate",
            orderable: true,
            alignCenter: true,
            dir: "desc"
        }, {
            type: 'link',
            title: "资产名称",
            name: "title",
            orderable: true,
            linkField: "property_Id",
            click: function (propertId) {
                $state.go("app.property.detail", { id: propertId });
            },
        }, {
            title: "审批状态",
            width: 150,
            name: "state",
            orderable: true,
            alignCenter: true
        }, {
            title: "借用方",
            width: 150,
            name: "name",
            orderable: true,
            alignCenter: true
        }, {
            title: "借用面积",
            width: 100,
            name: "lendArea",
            orderable: true,
            alignCenter: true
        }];

        var columns3 = [{
            title: "申请时间",
            width: 180,
            name: "processDate",
            orderable: true,
            alignCenter: true,
            dir: "desc"
        }, {
            type: 'link',
            title: "资产名称",
            name: "title",
            orderable: true,
            linkField: "property_Id",
            click: function (propertId) {
                $state.go("app.property.detail", { id: propertId });
            },
        }, {
            title: "审批状态",
            width: 150,
            name: "state",
            orderable: true,
            alignCenter: true
        }, {
            title: "租用方",
            width: 150,
            name: "name",
            orderable: true,
            alignCenter: true
        }, {
            title: "租用面积",
            width: 60,
            name: "rentArea",
            orderable: true,
            alignCenter: true
        }, {
            title: "租用起止时间",
            width: 100,
            name: "rentTime",
            orderable: true,
            alignCenter: true
        }, {
            title: "每年租金",
            width: 100,
            name: "priceString",
            orderable: true,
            alignCenter: true
        }];

        var columns4 = [{
            title: "申请时间",
            width: 180,
            name: "processDate",
            orderable: true,
            alignCenter: true,
            dir: "desc"
        }, {
            type: 'link',
            title: "资产名称",
            name: "title",
            orderable: true,
            linkField: "property_Id",
            click: function (propertId) {
                $state.go("app.property.detail", { id: propertId });
            },
        }, {
            title: "审批状态",
            width: 150,
            name: "state",
            orderable: true,
            alignCenter: true
        }, {
            title: "原产权方",
            width: 200,
            name: "prePropertyOwner",
            orderable: true,
            alignCenter: true
        }, {
            title: "新产权方",
            width: 200,
            name: "nowPropertyOwner",
            orderable: true,
            alignCenter: true
        }];

        var columns5 = [{
            title: "申请时间",
            width: 180,
            name: "processDate",
            orderable: true,
            alignCenter: true,
            dir: "desc"
        }, {
            type: 'link',
            title: "资产名称",
            name: "title",
            orderable: true,
            linkField: "property_Id",
            click: function (propertId) {
                $state.go("app.property.detail", { id: propertId });
            },
        }, {
            title: "审批状态",
            width: 150,
            name: "state",
            orderable: true,
            alignCenter: true
        }, {
            title: "核销方式",
            width: 100,
            name: "offType",
            orderable: true,
            alignCenter: true
        }, {
            title: "金额",
            width: 100,
            name: "price",
            orderable: true,
            alignCenter: true
        }];

        var columns6 = [{
            title: "申请时间",
            width: 180,
            name: "processDate",
            orderable: true,
            alignCenter: true,
            dir: "desc"
        }, {
            type: 'link',
            title: "资产名称",
            name: "title",
            orderable: true,
            linkField: "property_Id",
            click: function (propertId) {
                $state.go("app.property.detail", { id: propertId });
            },
        }, {
            title: "审批状态",
            width: 150,
            name: "state",
            orderable: true,
            alignCenter: true
        }];

        $scope.columns = columns1;
        //#endregion

        var first = true;
        //日期配置项         
        var _dateOption = {
            singleDatePicker: true,
            showDropdowns: true
        };

        $scope.dateOption = _dateOption;
        $scope.canMultiApprove = false;
        $scope.canMultiSubmit = false;

        $scope.processing = false;
        if ($scope.checkState == undefined || $scope.checkState == null || $scope.checkState == "")
            $scope.checkState = "unchecked";


        if ($scope.approveType == undefined || $scope.approveType == null || $scope.approveType == "")
            $scope.approveType = "newCreate";

        switch ($scope.approveType) {
            case "newCreate":
                $scope.columns = columns1;
                $scope.approveTypeText = "新增审批";
                break;
            case "lend":
                $scope.columns = columns2;
                $scope.approveTypeText = "出借审批";
                break;
            case "rent":
                $scope.columns = columns3;
                $scope.approveTypeText = "出租审批";
                break;
            case "allot":
                $scope.columns = columns4;
                $scope.approveTypeText = "划拨审批";
                break;
            case "off":
                $scope.columns = columns5;
                $scope.approveTypeText = "核销审批";
                break;
            case "edit":
                $scope.columns = columns6;
                $scope.approveTypeText = "变更审批";
                break;
        };

        switch ($scope.checkState) {
            case "unchecked":
                $scope.checkStateText = "待处理";
                break;
            case "checked":
                $scope.checkStateText = "已处理";
                break;
            case "all":
                $scope.checkStateText = "全部";
                break;
        };

        $scope.$watch('params', function (newvalue,oldvalue) {
            if (!first) $scope.ajax();
            else first = false;

            $scope.canMultiApprove = false;
            $scope.canMultiSubmit = false;
        }, true);


        $scope.$watch('selectedRows', function () {
            $scope.canMultiSubmit = $scope.selectedRows.length > 0;
            $scope.canMultiApprove = $scope.selectedRows.length > 0;

            angular.forEach($scope.selectedRows, function (data, index) {

                if (!data.canEditAndDelete) $scope.canMultiSubmit = false;
                if (!data.canApprove) $scope.canMultiApprove = false;
            });
        }, true);

        //创建id字符串
        var buildIdsString = function () {
            var idsString = "";
            angular.forEach($scope.selectedRows, function (data, index) {
                idsString += data.id + (index == $scope.selectedRows.length - 1 ? "" : ";");
            });

            return idsString;
        };


        //#region 参数

        //自定义参数集合
        var params = {
            pageIndex:0,
            pageSize: 15,
            query: "",
            sort: "processDate,desc;",
            approveType: $scope.approveType,
            checkState: $scope.checkState,
            time: 0
        };

        $scope.params = angular.copy(params);

        //#endregion


        $scope.selectedRows = [];

        $scope.ajax = function () {

            if ($scope.processing) return;        

            $scope.processing = true;
            $scope.params.time = new Date().getTime();
            propertyService.getAllPropertyNewCreateRecords($scope.params).then(function (response) {
                if ($scope.response != undefined && response.time < $scope.response.time) return;

                $scope.response = response;
            }, function () {
                alert("获取数据失败");
            }).finally(function () {
                $scope.processing = false;
            });
        };

        //扩展按钮
        $scope.tableEidtAndDelete = [
            {
                title: "查看审批",
                handler: function (row) {
                    var state = "app.property.process_" + row.approveType + "detail";
                    $state.go(state, { id: row.id });
                },
                isEnable: function (row) { return true; }
            },
            {
                title: "编辑",
                handler: function (row) {
                    if (row.approveType != 'newCreate' && row.approveType!='edit') {
                        $state.go("app.property.process_" + row.approveType + "edit", { id: row.id });
                    }
                    else {
                        $state.go("app.property.edit", { id: row.property_Id });
                    }
                },
                isEnable: function (row) { return row.canEditAndDelete; }
            },
            {
                title: "删除",
                handler: function (row) {
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

                        if (row.approveType == "newCreate") {
                            propertyService.deleteProperty(row.property_Id).then(function () {
                                toaster.pop('success', '', '提交成功');
                                $state.reload();
                            }, function (msg, status) {
                                $scope.errorMsg = msg;
                                toaster.pop('error', '', '删除失败');
                            });
                        }
                        else {
                            propertyService.deletePropertyApprove(row.id, row.approveType).then(function () {
                                toaster.pop('success', '', '提交成功');
                                $state.reload();
                            }, function (msg, status) {
                                $scope.errorMsg = msg;
                                toaster.pop('error', '', '删除失败');
                            });
                        }

                    }, function () {
                        $state.reload();
                    });
                },
                isEnable: function (row) { return row.canEditAndDelete; }
            },
            {
                title: "提交",
                handler: function (row) {
                    var state = "app.property.process_" + row.approveType + "edit";                    
                    propertyService.submitApprove(row.id, row.approveType).then(function (data) {
                        toaster.pop('success', '', '提交成功');
                        $state.reload();
                    }, function (msg) {
                        toaster.pop('error', '', '提交失败');
                    });
                },
                isEnable: function (row) { return row.canEditAndDelete; }
            },
            //{
            //    title: "审批",
            //    handler: function (row) {
            //        var state = "app.property.process_" + row.approveType + "edit";                    
            //        propertyService.submitApprove(row.id, row.approveType).then(function (data) {
            //            toaster.pop('success', '', '提交成功');
            //            $state.reload();
            //        }, function (msg) {
            //        });
            //    },
            //    isEnable: function (row) { return row.canApprove; }
            //}
        ];

        $scope.resetParams = function () {
            $scope.params = angular.copy(params);
            $scope.ajax();
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

        //资产审批提交
        $scope.excuteApprove = function (agree, suggestion) {
            propertyService.multiApplyApprove(buildIdsString(), agree, suggestion, $scope.approveType).then(function (response) {
                toaster.pop("success", "审核成功", "", 500);
            },
            function (msg) {
                toaster.pop("error", "审核失败，" + msg, "", 500);
            }).finally(function () {
                setTimeout(function () {
                    $state.reload();
                }, 600);
            });
        };

        //提交申请
        $scope.submitApprove = function () {

            propertyService.multiSubmitApprove(buildIdsString(), $scope.approveType).then(function () {
                toaster.pop('success', '', '提交成功');
                setTimeout(function () {
                    $state.reload();
                }, 600);
            }, function (msg, status) {
                toaster.pop('error', '', '提交失败');
                $scope.errorMsg = msg;
            });

        };

 
    }]);