//资产列表控制器
'use strict';
 
app.controller('PropertyProcessCtrl', ['$window', '$rootScope', '$uibModal', '$state', '$scope', '$timeout', 'toaster', 'PropertyService', 'GovernmentService', 'w5cValidator',
    function ($window, $rootScope, $uibModal, $state, $scope, $timeout, toaster,propertyService, governmentService, w5cValidator) {
 
        var _processproperties = [];
        var _processlist = [];
        var _lend = {
            name: "",
            lendTime: "",
            backTime: "",
            lendArea: 0,
            remark: "",
            dSuggestion: "",
            aSuggestion: "",
            property_Id: 0,
            lendPictures: [],
            lendFiles:[],
            submit:false
        };
        var _rent = {
            name: "",
            rentTime: "",
            backTime:"",
            rentArea: 0,
            rentMonth:0,
            rentPrice: 0,
            rentPictures: [],
            rentFiles: [],
            remark: "",
            dSuggestion: "",
            aSuggestion: "",
            submit: false,
            priceString:""
        };
        var _allot = {
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
        var _off = {
            reason: "",
            offTime: "",
            price: 0,
            offPictures: [],
            offFiles: [],
            dSuggestion: "",
            property_Id: 0,
            remark: "",
            offType:"",
            submit: false
        };

        var _process = {
            type: "lend",
            lend: _lend,
            rent: _rent,
            allot: _allot,
            off: _off
        };


        var params = {
            showHidden: true,
            query:"",
            sort: "name,asc;",
            manage: "true",
            isGovernment: false,
            isInstitution: false,
            isCompany: false,
            selectedId: 0,
            construct: false,
            land: false,
            constructOnLand: false,
            old: false,
            west: false,
            jjq: false,
            kc: false,
            qj: false,
            other: false,
            certi_both: false,
            certi_land: false,
            certi_construct: false,
            certi_none: false,
            current_self: false,
            current_rent: false,
            current_lend: false,
            current_idle: false,
            development: false,
            auction: false,
            injection: false,
            ct: false,
            jt: false,
            jk: false,
            self: false,
            storeUp: false,
            adjust: false,
            greenland: false,
            house: false
           // constructAreaRange: "",
           // landAreaRange: "",
          //  priceRange: "",
           // getDateRange: "",
        };
        $scope.params = angular.copy(params);
        //高级搜索的部门搜索 
        $scope.governments = [];
        $scope.government = {};
        //关键词搜索主管部门  
        $scope.refreshGovernments = function (governmentName) {
            if (governmentName == "" || governmentName == null || governmentName == undefined) return;
            governmentService.autocompleteByName(governmentName).then(function (response) {
                $scope.governments = response.data;
            });
        };
      
        //日期配置项         
        var _dateOption = {
            singleDatePicker: true,
            showDropdowns: true,
            locale: {
                format: 'YYYY/MM/DD'
            }
        };

        var _dateRangeOption = {
            showDropdowns: true,
            autoApply: true,
            locale: {
                format: 'YYYY/MM/DD'
            }
        };



        //高级搜索
        $scope.advanceSearch = function () {
            var modalInstance = $uibModal.open({
                templateUrl: 'advDialog.html',
                controller: 'AdvanceModalDialogCtrl',
                size: 'lg',
                windowClass: "map-modal",
                //appendTo: '#app',
                resolve: {
                    dialogHeight: function () { return $rootScope.dialogHeight; },
                    params: function () { return $scope.params },
                    governmentService: function () { return governmentService; },
                    governments: function () { return $scope.governments; },
                    government: function () { return $scope.government },
                    ajax: function () { return $scope.ajax; },
                    resetParams: function () { return $scope.resetParams; }
                }
            });

            modalInstance.result.then(function () {
            }, function () {
               // $state.reload();
            });
        };

        $scope.showDetail = function (property) {
            var modalInstance = $uibModal.open({
                templateUrl: 'propertyDetail.html',
                controller: 'propertyDetailModalDialogCtrl',
                size: 'lg',
                windowClass: "map-modal",
                //appendTo: '#app',
                resolve: {
                    dialogHeight: function () { return $scope.dialogHeight; },
                    property: function () { return property}
                }
            });

            modalInstance.result.then(function () {
            }, function () {
                //$state.reload();
            });

        }

        //获取所有需要处置的资产         
        propertyService.getPropertyProcess().then(function (response) {
            $scope.processproperties = response;
            $scope.loadMore();
            //var i = 0;
            //angular.forEach(response, function (data) {
            //    i = i + 1;
            //    data.index = i;
            //    _processproperties.push(data);
            //});
            //$scope.processlength = _processproperties.length;
        }).finally(function () {
            $scope.processing = false;
        });


        //滚动加载
        $scope.loadMore = function () {
            var last = $scope.showProperties.length;
            if ($scope.processproperties.length == 0) return;
            var newAdded = $scope.processproperties.slice(last, last + 20);

            $scope.showProperties = $scope.showProperties.concat(newAdded);
        };

        //左右关联
        $scope.selectedRowChange = function (property) {
            if (property.row.selected) {
                _processlist.push(property);
                $scope.listlength = _processlist.length;
            }
            else {
                for (var i = 0; i <= _processlist.length; i++) {
                    if (_processlist[i] == property) {
                        _processlist.splice(i, 1);
                        $scope.listlength = _processlist.length;
                    }
                }
            }
        };
        // 移除
        $scope.removeProcess = function (property) {

            for (var i = 0; i <= _processlist.length; i++) {
                if (_processlist[i] == property) {
                    _processlist.splice(i, 1);
                    $scope.listlength = _processlist.length;
                }
                if (property.row.selected) property.row.selected = false;

            }
        }
        //一键清空
        $scope.clearAll = function () {
            angular.forEach(_processlist, function (data) {
                if (data.row.selected) data.row.selected = false;
            })
            _processlist.splice(0, _processlist.length);
            $scope.listlength = _processlist.length;
        }
     


       // $scope.advanceSearch = _advanceSearch;
        $scope.process = _process;
        $scope.dateOption = _dateOption;
        $scope.dateRangeOption = _dateRangeOption;
        $scope.processproperties = _processproperties;
        $scope.showProperties = [];
        $scope.processlist = _processlist;
        $scope.listlength = 0;
        $scope.processing = true;

        $scope.ajax = function () {
            if ($scope.params.query == undefined || $scope.params.query == null || $scope.params.query == "") delete $scope.params.query;

            $scope.processing = true;
            propertyService.getFilterProcessProperties($scope.params).then(function (response) {
              //  if ($scope.response != null && $scope.response != undefined && response.time < $scope.response.time) return;
                $scope.processproperties = response;
                $scope.showProperties = [];
                $scope.loadMore();

            }, function () {
                alert("获取数据失败");
            }).finally(function () {
                $scope.processing = false;
            });

            //propertyService.getFilterProperty($scope.params).then(function (response) {
            //    $scope.processproperties = response;
            //})
        }

        //关键字搜索
        $scope.setParams = function (reset) {
           
            $scope.ajax();
        }


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

            rentName: {
                required: "租用方名称不能为空"
            },
            daterange: {
                required: "出租时间不能为空"
            },
            daterange1: {
                required: "归还时间不能为空"
            },
            rentArea: {
                required: "出租面积不能为空"
            },
            rentPrice: {
                required: "租用价格不能为空"
            },

            daterange: {
                required: "划拨时间不能为空"
            },

            offReason: {
                required: "核销原因不能为空"
            },
            daterange: {
                required: "出租时间不能为空"
            },
            offPrice: {
                required: "核销金额不能为空"
            },
            offType: {
                required: "核销金额不能为空"
            },
        });


       //判断出租总年份
        $scope.rentTimeChange = function () {
          
            var timestring = $scope.process.rent.rentTime;
            var time = timestring.split("-");

            var backtime = moment(time[1]);
            var renttime = moment(time[0]);
             
            var diff = backtime.diff(renttime, "year", true);  
            var num = Math.ceil(diff);
            $scope.yearNumber = [];
                for (var i = 1; i <= num; i++) {
                  var d = {
                        index: i,
                        model:0
                    }
                    $scope.yearNumber.push(d);
                }          
        }
      
        //每个表单的配置，如果不设置，默认和全局配置相同
        var validation = $scope.validation = {
            options: {
                blurTrig: true
            }
        };

        validation.saveEntity = function (type, submit) {
            var ids = "";
            if (_processlist.length == 0) {
                
                alert("请先选择需要处置的资产！");
                return false;
            }
            angular.forEach(_processlist, function (data) {
                ids += data.id + ";";
            });
            switch (type) {
                case "lend":
                    $scope.process.lend.submit = submit;
                    $scope.process.lend.ids = ids;
                    propertyService.createLend($scope.process.lend).then(function () {
                        successCallback();
                    }), function (message) {
                        $scope.errorMsg = message;
                    }
                    break;
                case "rent":
                    $scope.process.rent.submit = submit;
                    $scope.process.rent.ids = ids;
                    var timestring = $scope.process.rent.rentTime;
                    var time = timestring.split("-");
                    $scope.process.rent.rentTime = time[0];
                    $scope.process.rent.backTime = time[1];
                    angular.forEach($scope.yearNumber, function (data) {
                        $scope.process.rent.priceString += data.model +";";
                    })
               
                    propertyService.createRent($scope.process.rent).then(function () {
                        successCallback();
                    }), function (message) {
                        $scope.errorMsg = message;
                    }
                    break;
                case "allot":
                    $scope.process.allot.submit = submit;
                    $scope.process.allot.ids = ids;
                    propertyService.createAllot($scope.process.allot).then(function () {
                        successCallback();
                    }), function (message) {
                        $scope.errorMsg = message;
                    }
                    break;
                case "off":
                    $scope.process.off.submit = submit;
                    $scope.process.off.ids = ids;
                    propertyService.createOff($scope.process.off).then(function (repsonse) {
                        successCallback();
                    }), function (message) {
                        $scope.errorMsg = message;
                    }
                    break;
            }
        };

        var successCallback = function () {
            toaster.pop("sucess", "创建成功", "", 500);
            $timeout(function () {
                $state.go("app.property.process_approve", {approveType:$scope.process.type});
            }, 550);
        };


        $scope.swfUrl = $rootScope.baseUrl + 'vender/libs/webimgUploader/Uploader.swf';
        $scope.pictureUploadUrl = $rootScope.apiUrl + 'Media/pictures/Upload?size=500';
        $scope.fileUploadUrl = $rootScope.apiUrl + 'Media/files/Upload';

        //$scope.lendPictures = [];
        //$scope.lendFiles = [];

        //$scope.rentPictures = [];
        //$scope.rentFiles = [];

        //$scope.allotPictures = [];
        //$scope.allotFiles = [];

        //$scope.offPictures = [];
        //$scope.offFiles = [];

        $scope.show = function (type, msg, title, time) {
            alert("123");
            toaster.pop('error', '请不要重复提交');
        };
    }]);

app.controller('propertyDetailModalDialogCtrl', function ($scope,$uibModalInstance, dialogHeight, property) {
    $scope.okText = "确定";
    $scope.dialogHeight = dialogHeight;
    $scope.property = property;

    $scope.ok = function () {
        $uibModalInstance.dismiss();

    }

})





app.controller('AdvanceModalDialogCtrl', function ($scope, $uibModalInstance, dialogHeight, params, governmentService, governments, government, ajax, resetParams) {

    $scope.okText = "确定";
    $scope.cancelText = "取消";
    $scope.dialogHeight = dialogHeight;
    $scope.governmentService = governmentService;
    $scope.params = params;
    var region, propertytype,name;

    //高级搜索的部门搜索 
    $scope.governments = governments;
    $scope.government = government;

    //关键词搜索主管部门  
    $scope.refreshGovernments = function (governmentName) {
        if (governmentName == "" || governmentName == null || governmentName == undefined) return;
        governmentService.autocompleteByName(governmentName).then(function (response) {
            $scope.governments = response.data;
        });
    };

    $scope.ok = function () {
        //if (government.selected.name != undefined) name = government.selected.name;
        //else name = "全部";

        $uibModalInstance.close();
        ajax();
       // var aaa = $scope.params;
        //if ($scope.params.construct == true) propertytype = 0;
        //else if ($scope.params.land == true) propertytype = 1;
        //else if ($scope.params.constructOnLand == true) propertytype = 2;
        //else propertytype = -1;

        //if ($scope.params.old == true) region = 0;
        //else if ($scope.params.west == true) region = 1;
        //else if ($scope.params.jjq == true) region = 2;
        //else if ($scope.params.kc == true) region = 3;
        //else if ($scope.params.qj == true) region = 4;
        //else if ($scope.params.other == true) region = 5;
        //else region = -1;

      

     

    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    };

});