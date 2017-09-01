//部门列表控制器
'use strict'

app.controller('GovernmentListCtrl', ['$window','$scope','$rootScope','$timeout', 'w5cValidator', '$uibModal', 'GovernmentService',
    function ($window, $scope, $rootScope, $timeout, w5cValidator, $uibModal, governmentService) {
        $scope.error = "";
        $scope.my_tree = {};   //目录树对象
        $scope.my_data = [];   //目录树数据
        $scope.selectedGovernment = null;
        $scope.nameDefaultValue = "";   //原始name值
        $scope.initial_selection = null;

        $scope.firstGovernmentsCount = 0;   //一级单位数目
        $scope.allGovernmentsCount = 0; //子类单位数目

        $scope.selectedGovernmentType = null;
        $scope.governmentTypes = [
           { value: 0, label: "行政机关" },
           { value: 1, label: "事业单位" },
           { value: 2, label: "国有企业" }
        ];

        var windowHeight = $window.innerHeight; //获取窗口高度
        var headerHeight = 50;
        var footerHeight = 51;
        var windowWidth = $window.innerWidth; //获取窗口宽度
        var padding = 40;
        $scope.maxHeight = windowHeight - headerHeight - footerHeight - padding;

        $scope.refresh = function (clearInitialSelection) {
            if (clearInitialSelection) $scope.initial_selection = null;
            $scope.selectedGovernment = null;
            $scope.doing_async = true;
            $scope.my_data = [];
            //清空错误
            $scope.error = "";

            //加载目录
            governmentService.loadTreeData().then(function (data, status) {
                if ($scope.initial_selection == null)
                    $scope.initial_selection = data.length > 0 ? data[0].label : null;
                $scope.my_data = data;
                $scope.doing_async = false;

                //统计单位数目

                $scope.firstGovernmentsCount = data.length;
                $scope.allGovernmentsCount = $scope.calCount(data);

                //延迟选中
                $timeout(function () { $scope.my_tree.select_branch_by_label($scope.initial_selection); }, 1);
            }, function (msg, status) {
                $scope.error = msg;
            });
        };

        //计算节点数目
        $scope.calCount = function (nodes) {
            var count = 0;
            angular.forEach(nodes, function (item) {
                count++;

                if (item.children.length > 0) {
                    count += $scope.calCount(item.children);
                }
            });

            return count;
        };

        //目录树节点选中事件
        $scope.my_tree_handler = function (branch) {

            //清空错误
            $scope.error = "";

            //表单重置
            $scope.selectedGovernment = null;

            $timeout(function () {
                $scope.selectedGovernment = angular.copy(branch.data);
                $scope.nameDefaultValue = branch.data.id == undefined ? "" : $scope.selectedGovernment.name;
                
                //遍历政府机构
                angular.forEach($scope.governmentTypes, function (item) {
                    if (item.value == $scope.selectedGovernment.governmentTypeId)
                    {
                        $scope.selectedGovernmentType = angular.copy(item);
                    }
                });

                if (branch.children.length > 0) {
                    $scope.my_tree.expand_branch();
                }
            }, 1);
        };

        //增加新节点
        $scope.adding_a_branch = function () {
            var b_new_node = null;

            var nextDisplayOrder = 0;
            var b = $scope.my_tree.get_selected_branch();

            if (b == null) {
                //#region 根节点

                b_new_node = $scope.my_tree.add_branch(null, {
                    label: '新增单位',
                    data: {
                        name: '新增单位',
                        governmentTypeId: 0,
                        address: "",
                        person: "",
                        tel:"",
                        displayOrder: nextDisplayOrder,
                        description: "描述",
                        parentId: 0
                    }
                });

                //#endregio
            }
            else {
                if (b.data.id == undefined) {
                    var modalInstance = $uibModal.open({
                        templateUrl: 'dialog.html',
                        controller: 'ModalDialogCtrl',
                        windowClass: "map-modal",
                        resolve: {
                            title: function () { return "提示"; },
                            content: function () { return "必须保存当前单位 《" + b.label + "》后，才能在此节点下增加节点"; },
                            dialogHeight: function () { return $rootScope.dialogHeight; },
                            statistics: function () { return null; }
                        }
                    });

                    modalInstance.result.then(function (selectedItem) { }, function () { });

                    return;
                }

                var children = $scope.my_tree.get_children(b);
                if (children.length > 0)
                    nextDisplayOrder = children[children.length - 1].data.displayOrder + 1;

                b_new_node = $scope.my_tree.add_branch(b, {
                    label: '新增单位',
                    data: {
                        name: '新增单位',
                        governmentTypeId: -1,
                        address: "",
                        person: "",
                        tel: "",
                        displayOrder: nextDisplayOrder,
                        description: "描述",
                        parentId: b.data.id
                    }
                });
            }



            //选中
            $scope.my_tree.select_branch(b_new_node);

            return b_new_node;
        };

        //删除节点
        $scope.removing_a_branch = function (size) {
            var b = $scope.my_tree.get_selected_branch();

            var modalInstance = $uibModal.open({
                templateUrl: 'dialog.html',
                controller: 'ModalDialogCtrl',
                windowClass: "map-modal",
                size: size,
                resolve: {
                    title: function () { return "提示"; },
                    content: function () { return "是否删除单位 《" + b.label + "》？"; },
                    dialogHeight: function () { return $rootScope.dialogHeight; },
                    statistics: function () { return null; }
                }
            });

            modalInstance.result.then(function (selectedItem) {

                if (b.data.id != undefined) {
                    governmentService.deleteGovernment(b.data.id).then(function () {

                        //获取父节点
                        var p = $scope.my_tree.get_parent_branch(b);
                        if (p != null) $scope.initial_selection = p.label;
                        else $scope.my_tree.select_first_branch();
                        $scope.refresh();
                    }, function (msg, status) {
                        $scope.error = msg;
                    });
                }
                else {

                    b.deleted = true;
                    var p = $scope.my_tree.get_parent_branch(b);
                    if (p != null) $scope.my_tree.select_branch(p);
                    else {
                        var firtst = $scope.my_tree.get_first_branch();
                        if (firtst != null) $scope.my_tree.select_branch(firtst);//$scope.initial_selection = firtst.label;
                        else $scope.my_tree.select_first_branch();  //$scope.initial_selection = "";
                    }
                }
            }, function () { });
        };

        //删除选中
        $scope.clearSelection = function () {
            $scope.my_tree.select_branch(null);
            $scope.selectedGovernment = null;
        };

        //保存单位
        $scope.saveGovernment = function () {

            var b = $scope.my_tree.get_selected_branch();
            if (b == null) {
            }
            else
            {
                $scope.init = b;

                if (b.data.id == undefined) {
                    
                    $scope.selectedGovernment.parentGovernmentId = b.data.parentId;
                    //#region 新增单位
                    governmentService.createGovernment($scope.selectedGovernment).then(function (data, message) {
                        //获取父节点
                        var p = $scope.my_tree.get_parent_branch(b);
                        if (p != null && p != undefined) $scope.initial_selection = p.label;

                        //刷新
                        $scope.refresh();
                    }, function (msg, status) {
                        $scope.error = msg;
                    }).finally(function () { });

                    //#endregion
                }
                else {
                    //#region 更新
                    governmentService.updateGovernment($scope.selectedGovernment).then(function (data, message) {
                        //获取父节点
                        var p = $scope.my_tree.get_parent_branch(b);
                        if (p != null) $scope.initial_selection = p.label;
                        else {
                            //获取第一个节点
                            var first = $scope.my_tree.get_first_branch();
                            if (first != null) $scope.initial_selection = first.label;
                            else $scope.initial_selection = "";
                        }

                        //刷新
                        $scope.refresh();
                    }, function (msg, status) {
                        $scope.error = msg;
                    }).finally(function () {

                    });
                    //#endregion
                }
            }




        };

        //#region 验证配置

        w5cValidator.setRules({
            gname: {
                required: "单位名不能为空",
                w5cuniquecheck: "输入单位名已经存在，请重新输入"
            },
            gtype: {
                required: "单位性质不能为空"
            },
            gaddress: {
                required: "单位地址不能为空"
            },
            gperson: {
                required: "联系人不能为空",
                minlength: "联系人长度应为2-5个汉字",
                maxlength: "联系人长度应为2-5个汉字"
            },
            gtel: {
                required: "联系方式不能为空",
                pattern: "联系方式格式不正确"
            },
        });

        //每个表单的配置，如果不设置，默认和全局配置相同
        var vm = $scope.vm = {             
            validateOptions: {
                blurTrig: true
            }
        };
       
        //#endregion

        //清空错误
        $scope.clearError = function (index) {        
            $scope.error = "";
        };

        //类型选择
        $scope.governmentTypeSelected = function (selectedGovernmentType) {
            $scope.selectedGovernment.governmentTypeId = selectedGovernmentType.value;
            $scope.selectedGovernment.governmentType = selectedGovernmentType.label;
        };

        //初始化操作
        return $scope.refresh();

    }]);