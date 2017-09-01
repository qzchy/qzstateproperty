"use strict";

app.controller("HomeCtrl", ['$scope', '$rootScope', '$window', '$timeout', '$state', '$uibModal','PropertyService',
function ($scope, $rootScope, $window, $timeout, $state, $uibModal, propertyService) {
    //#region 设置对话框高度
    var windowHeight = $window.innerHeight; //获取窗口高度
    var headerHeight = 50;
    var footerHeight = 51;
    var windowWidth = $window.innerWidth; //获取窗口宽度

    $scope.dialogHeight = { "height": windowHeight - headerHeight - footerHeight - 230 };
    //#endregion

    $scope.showContent = function (title,content) {
  
        var modalInstance = $uibModal.open({
            templateUrl: 'content.html',
            controller: 'ContentModalDialogCtrl',
            size: 'lg',
            windowClass: "map-modal",
            //appendTo: '#app',
            resolve: {
                title: function () { return title },
                content: function () { return content },
                dialogHeight: function () { return $scope.dialogHeight; }
            }
        });

        modalInstance.result.then(function () {
        }, function () {
            $state.reload();
        });

    };

    $scope.approve = {
        newCreate: 0,
        edit:0,
        lend: 0,
        rent: 0,
        allot: 0,
        off: 0
    };

    propertyService.approveStatistics().then(function (response) {
        $scope.approve = response;
    }, function (msg) {
        alert(msg);
    });

}]);

app.controller('ContentModalDialogCtrl', function ($scope, $uibModalInstance, dialogHeight, title, content) {

    $scope.dialogHeight = dialogHeight;
    $scope.title = title;
    $scope.content = content;

    $scope.ok = function () {
        $uibModalInstance.close();
    };
});