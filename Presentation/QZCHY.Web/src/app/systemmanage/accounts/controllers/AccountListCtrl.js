//用户列表控制器
'use strict';
 
app.controller('AccountListCtrl', ['$rootScope', '$uibModal', '$state', '$scope', 'AccountService',
    function ($rootScope, $uibModal, $state, $scope, accountService) {
    var error = $scope.error = "";
    var response = $scope.response = null;
    $scope.processing = false;
    //自定义参数集合
    $scope.params = { showHidden: false, pageSize: 15 };
        //选中的行
    $scope.selectedItem = [];

    //#region table config
    //列集合
    $scope.columns = [{
        type: 'link',
        title: "登陆名",
        name: "userName",
        orderable: true,
        linkField:"id",
        click: function (accountId) {
            $state.go("app.systemmanage.account.detail", { id: accountId });
        },
    }, {
        title: "用户名",
        name: "nickName",
        orderable: true,
    }, {
        title: "单位",
        width: 200,
        name: "governmentName",
        orderable: true,
        dir: "asc"
    }, {
        title: "角色",
        width: 200,
        name: "roleList",
        orderable: true
    }, 
 {
     type: 'bool',
     title: "激活",
     width: 80,
     name: "active",
     orderable: true,
     alignCenter: true
 },
  {
      title: "最近操作",
      width: 150,
      name: "lastActivityDate",
      orderable: true,
      alignCenter: true
  },
    {
        title: "最近登录",
        width: 150,
        name: "lastLoginDate",
        orderable: true,
        alignCenter: true
    }
    ];
  
    //编辑删除功能
    $scope.tableEidtAndDelete = {
        edit: function (accountId) {
            $state.go("app.systemmanage.account.edit", { id: accountId });
        },
        del: function (rows) {
            var idStr = "";
            var content = "";

            //遍历生成idStr
            if (rows.length == 1) {
                idStr = rows[0].id;
                content = "是否删除用户《" + rows[0].userName + "》？";
            }
            else if (rows.length > 1) {
                content = "是否删除当前选中的用户？";

                angular.forEach(rows, function (value, index) {
                    if (value.selected) idStr += value.id + '_';
                });

            }

            var modalInstance = $uibModal.open({
                templateUrl: 'deleteRow.html',
                controller: 'ModalDialogCtrl',
                windowClass: "map-modal",
                resolve: {
                    title: function () { return "提示"; },
                    content: function () { return content; },
                    dialogHeight: function () { return $rootScope.dialogHeight; }
                }
            });

            modalInstance.result.then(function () {

                //删除用户
                accountService.deleteAccountByIds(idStr).then(function (data, status) { }, function (message) { alert(message); }).finally(function () {
                    $state.reload();
                });

            }, function () { });
        }
    };
 
    
    $scope.reset = true;

        //#endregion    

        //ajax获取数据
    $scope.ajax = function (params) {

        if ($scope.processing) return;

        //if (params != null && params != undefined)
        //    angular.extend($scope.params, params);

        //#endregion

        $scope.processing = true;
        $scope.params.time = new Date().getTime();
        console.log($scope.params);
        accountService.getAllAccounts($scope.params).then(function (response) {
            if ($scope.response != null && $scope.response != undefined && response.time < $scope.response.time) return;
            $scope.response = response;
        }, function () {
            alert("获取数据失败");
        }).finally(function () {
            $scope.processing = false;
        });
    };

    $scope.advanceSearch = null;
}]);