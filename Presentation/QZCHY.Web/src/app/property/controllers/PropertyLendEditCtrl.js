//资产列表控制器
'use strict';
 
app.controller('PropertyLendEditCtrl', ['$window', '$rootScope', '$uibModal', '$state', '$scope', '$timeout','toaster','moment', 'PropertyService', 'GovernmentService', 'w5cValidator',
    function ($window, $rootScope, $uibModal, $state, $scope, $timeout, toaster, moment,propertyService, governmentService, w5cValidator) {
        $scope.swfUrl = $rootScope.baseUrl + 'vender/libs/webimgUploader/Uploader.swf';
        $scope.pictureUploadUrl = $rootScope.apiUrl + 'Media/pictures/Upload?size=500';
        $scope.fileUploadUrl = $rootScope.apiUrl + 'Media/files/Upload';
        $scope.processing = true;

        var id = $scope.approveId;
        $scope.lend = {
            name: "",
            lendTime: "",
            backTime: "",
            lendArea: 0,
            remark: "",
            dSuggestion: "",
            aSuggestion: "",
            property_Id: 0,
            lendPictures: [],
            lendFiles: [],
            submit: false
        };

        propertyService.getLend(id).then(function (response) {
            $scope.lend = response.propertyLend;

            $scope.lend.lendTime = moment(response.propertyLend.lendTime).format("MM/DD/YYYY");
         
        }, function (message) {
            toaster.pop("error", "错误：" + message);
            $timeout(function () { $state.go("app.property.process_approve", { approveType: "off" }); }, 550);
        }).finally(function () {
            $scope.processing = false;
        });

      //验证
        w5cValidator.setRules({
            lendName: {
                required: "借用方名称不能为空"
            },
            daterange: {
                required: "出借时间不能为空"
            },
            lendArea: {
                required: "出借面积不能为空"
            },
            //backtime: {
            //    required: "归还时间不能为空"
            //},
        });

        var validation = $scope.validation = {
            options: {
                blurTrig: true
            }
        };
        //提交编辑
        validation.saveEntity = function ($event, submit) {
            $scope.processing = true;
            $scope.lend.submit = submit;

            propertyService.updateLend($scope.lend).then(function () {

                toaster.pop("sucess", "保存成功", "", 500);
                $timeout(function () {
                    if (submit) $state.go("app.property.process_approve", { approveType: "lend" });
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