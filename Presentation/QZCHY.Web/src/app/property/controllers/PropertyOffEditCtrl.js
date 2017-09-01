//资产列表控制器
'use strict';
 
app.controller('PropertyOffEditCtrl', ['$window', '$rootScope', '$uibModal', '$state', '$scope', '$timeout', 'PropertyService', 'GovernmentService', 'w5cValidator',
    function ($window, $rootScope, $uibModal, $state, $scope, $timeout, propertyService, governmentService, w5cValidator) {
        $scope.swfUrl = $rootScope.baseUrl + 'vender/libs/webimgUploader/Uploader.swf';
        $scope.pictureUploadUrl = $rootScope.apiUrl + 'Media/pictures/Upload?size=500';
        $scope.fileUploadUrl = $rootScope.apiUrl + 'Media/files/Upload';
        $scope.processing = true;
        var id = $scope.approveId;
        $scope.off = {
            reason: "",
            offTime: "",
            price: 0,
            offPictures: [],
            offFiles: [],
            dSuggestion: "",
            property_Id: 0,
            remark: "",
            offType: "",
            submit: false
        };
        propertyService.getOff(id).then(function (response) {
            $scope.off = response.propertyOff;
            $scope.off.offTime = moment(response.propertyOff.offTime).format("MM/DD/YYYY");
        }, function (message) {
            toaster.pop("error", "错误：" + message);
            $timeout(function () { $state.go("app.property.process_approve", { approveType: "off" }); }, 550);
        }).finally(function () {
            $scope.processing = false;
        });

        //验证
        w5cValidator.setRules({
            offReason: {
                required: "核销原因不能为空"
            },
            daterange: {
                required: "出租时间不能为空"
            },
            offPrice: {
                required: "核销金额不能为空"
            },
        });

        var validation = $scope.validation = {
            options: {
                blurTrig: true
            }
        };
        //提交编辑
        validation.saveEntity = function ($event, submit) {
            $scope.processing = true;
            $scope.off.submit = submit;

            propertyService.updateOff($scope.off).then(function () {
                toaster.pop("sucess", "保存成功", "", 500);
                $timeout(function () {
                    if (submit) $state.go("app.property.process_approve", { approveType: "off" });
                    else $state.reload();
                }, 550);
            }, function (message) {
                $scope.errorMsg = message;
                toaster.pop("error", "保存失败", "", 500);
            }).finally(function () {
                $scope.processing = false;
            });
        }

        //日期配置项         
        $scope.dateOption = {
            singleDatePicker: true,
            showDropdowns: true
        };
    }]);
