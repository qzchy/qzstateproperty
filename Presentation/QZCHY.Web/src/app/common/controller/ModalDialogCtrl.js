﻿"use strict";
//对话框控制器
app.controller('ModalDialogCtrl', function ($scope, $uibModalInstance, title, content, dialogHeight, statistics) {

    $scope.title = title;
    $scope.content = content;
    $scope.dialogHeight = dialogHeight;
    $scope.statistics = statistics;
    $scope.okText = "确定";
    $scope.cancelText = "取消";

    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    };
});