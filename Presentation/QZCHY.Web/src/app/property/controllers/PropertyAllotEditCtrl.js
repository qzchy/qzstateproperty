//资产列表控制器
'use strict';
 
app.controller('PropertyAllotEditCtrl', ['$window', '$rootScope', '$uibModal', '$state', '$scope', '$timeout', 'PropertyService', 'GovernmentService', 'w5cValidator','toaster',
    function ($window, $rootScope, $uibModal, $state, $scope, $timeout, propertyService, governmentService, w5cValidator, toaster) {
        $scope.swfUrl = $rootScope.baseUrl + 'vender/libs/webimgUploader/Uploader.swf';
        $scope.pictureUploadUrl = $rootScope.apiUrl + 'Media/pictures/Upload?size=500';
        $scope.fileUploadUrl = $rootScope.apiUrl + 'Media/files/Upload';
        $scope.processing = true;

        var id = $scope.approveId;
        $scope.allot = {
            prePropertyOwner: "",
            nowPropertyOwner: "",
            allotTime: "",
            allotPictures: [],
            allotFiles: [],
            remark: "",
            dSuggestion: "",
            aSuggestion: "",
            submit: false
        };
        $scope.government = {};
        $scope.governments = [];
        propertyService.getAllot(id).then(function (response) {
            $scope.allot = response.propertyAllot;

            $scope.allot.allotTime = moment(response.propertyAllot.allotTime).format("MM/DD/YYYY");
            $scope.government.selected = response.property.governmentName;
        }, function () {
            toaster.pop("error", "错误：" + message);
            $timeout(function () { $state.go("app.property.process_approve", { approveType: "allot" }); }, 550);
        }).finally(function () {
            $scope.processing = false;
        });

       
        //关键词搜索主管部门  
        $scope.refreshGovernments = function (governmentName) {
            if (governmentName == "" || governmentName == null || governmentName == undefined) return;
            governmentService.autocompleteByName(governmentName).then(function (response) {
                $scope.governments = response.data;
            });
        };

        //验证
        w5cValidator.setRules({
            daterange: {
                required: "划拨时间不能为空"
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
            $scope.allot.submit = submit;
            $scope.allot.nowPropertyOwner = $scope.government.selected.name;

            propertyService.updateAllot($scope.allot).then(function () {
                toaster.pop("sucess", "创建成功", "", 500);
                $timeout(function () {
                    if (submit) $state.go("app.property.process_approve", { approveType: "allot" });
                    else $state.reload();
                }, 550);
            },function (message) {
                scope.errorMsg = message;
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
