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