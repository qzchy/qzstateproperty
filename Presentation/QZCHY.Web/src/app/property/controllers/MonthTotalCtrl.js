//资产列表控制器
'use strict';
 
app.controller('MonthTotalCtrl', ['$window', '$rootScope', '$uibModal', '$state', '$scope', '$timeout', 'toaster', 'PropertyService', 'GovernmentService', 'w5cValidator',
    function ($window, $rootScope, $uibModal, $state, $scope, $timeout, toaster,propertyService, governmentService, w5cValidator) {

        var param = {
            property_ID: 0,
            property_Name: "",
            currentUse_Self: 0,
            currentUse_Rent: 0,
            currentUse_Lend: 0,
            currentUse_Idle: 0,
            price: 0,
            month: 0,
            income:0
        }

        w5cValidator.setRules({
            income: {
                required: "本月收入不能为空"
            }
        });

        var validation = $scope.validation = {
            options: {
                blurTrig: true
            }
        };

        var total = [];
        var subtotal = [];
        $scope.monthtotal = [];
        $scope.submonthtotal = [];
        $scope.search = "";
        $scope.search1 = "";

        $scope.submitRecord= true;

        propertyService.getSubmitRecord().then(function (response) {
            if (response == true) {
                $scope.submitRecord = false;
            }

        });

        propertyService.getMonthTotal().then(function (response)
        {
            $scope.allmodel = response;
            var now = new Date();
            var monthtotal = [];
            var submonthtotal = [];
            $scope.NowTime = now.getFullYear() + "年" + now.getMonth()+1 + "月" + now.getDate() + "日";
            angular.forEach(response, function (data, i) {
                if (data.logoUrl == "") data.logoUrl = "img/timg.jpg";
                if (data.monthTotalModel.length==0) {
                 monthtotal.push(data);

                }
                else {
                    
                    var t = now.getMonth() + 1;
                    var m =parseInt(data.monthTotalModel[data.monthTotalModel.length - 1].month.substring(5, 7));
                    if (t == m) {
                      submonthtotal.push(data);
                    }
                    else {
                     monthtotal.push(data);
                    }                                 
                }
            });

            $scope.monthtotal = monthtotal;
            $scope.submonthtotal = submonthtotal;
            total = $scope.monthtotal;
            subtotal = $scope.submonthtotal;
          
        });
        

        //选中资产
        $scope.showMonthTotal = function (p) {
            $scope.property = p;
            var length = $scope.property.monthTotalModel.length;
            if (length == 0) {
                $scope.property.monthTotalModel = [];
                $scope.property.monthTotalModel.income = 0;
            }
            else {
                $scope.property.currentUse_Self = $scope.property.monthTotalModel[length - 1].currentUse_Self;
                $scope.property.currentUse_Rent = $scope.property.monthTotalModel[length - 1].currentUse_Rent;
                $scope.property.currentUse_Lend = $scope.property.monthTotalModel[length - 1].currentUse_Lend;
                $scope.property.currentUse_Idle = $scope.property.monthTotalModel[length - 1].currentUse_Idle;
                $scope.property.price = $scope.property.monthTotalModel[length - 1].price;
                $scope.property.monthTotalModel.income = $scope.property.monthTotalModel[length - 1].income;
            }
        }
     
        //提交单条资产每月收入情况
        validation.saveEntity = function (property) {

            param.property_ID = property.id;
            param.property_Name = property.name;
            param.currentUse_Idle = property.currentUse_Self;
            param.currentUse_Lend = property.currentUse_Rent;
            param.currentUse_Rent = property.currentUse_Lend;
            param.currentUse_Self = property.currentUse_Idle;
            param.price = property.price;
            param.month = 1;
            param.income = property.monthTotalModel.income;

            propertyService.submitparam(param).then(function () {

                for (var i = 0; i <= $scope.submonthtotal.length; i++) {
                    if ($scope.submonthtotal[i] == property) {
                        $scope.submonthtotal.splice(i, 1);
                    }
                }

                for (var i = 0; i <= $scope.monthtotal.length; i++) {
                    if ($scope.monthtotal[i] == property) {
                        $scope.monthtotal.splice(i, 1);
                        $scope.submonthtotal.push(property);
                    }
                }              

                alert("提交成功！");
            })
        }


        $scope.submitsubparam = function (subproperty) {
            param.property_ID = subproperty.id;
            param.property_Name = subproperty.name;
            param.currentUse_Idle = $(".subidle").val();
            param.currentUse_Lend = $(".sublend").val();
            param.currentUse_Rent = $(".subrent").val();
            param.currentUse_Self = $(".subself").val();
            param.price = $(".subprice").val();
            param.month = 1;
            param.income = $(".subincome").val();

            propertyService.submitparam(param).then(function () {

                for (var i = 0; i <= $scope.submonthtotal.length; i++) {
                    if ($scope.submonthtotal[i].id == subproperty.id) {
                        $scope.submonthtotal.splice(i, 1);
                    }
                }

                alert("提交成功！");
            })

        }


        //全部提交
        $scope.submitall = function () {
            var all = [];
            var allmodel = [];
            angular.forEach($scope.submonthtotal, function (data, i) {
                allmodel.push(data);
            });
            angular.forEach($scope.monthtotal, function (data, i) {
                allmodel.push(data);
            });

            angular.forEach(allmodel, function (model, i) {

                var params = {
                    property_ID: 0,
                    property_Name: "",
                    currentUse_Self: 0,
                    currentUse_Rent: 0,
                    currentUse_Lend: 0,
                    currentUse_Idle: 0,
                    price: 0,
                    month: 0,
                    income: 0
                }

                params.property_ID = model.id;
                params.property_Name = model.name;

                if (model.monthTotalModel.length == 0) {
                    params.currentUse_Idle = model.currentUse_Idle;
                    params.currentUse_Lend = model.currentUse_Lend;
                    params.currentUse_Rent = model.currentUse_Rent;
                    params.currentUse_Self = model.currentUse_Self;
                    params.price = model.price;
                    params.month = 1;
                    params.income = 0;
                }

                else {
                    var m = model.monthTotalModel[model.monthTotalModel.length - 1];
                    params.currentUse_Idle = m.currentUse_Idle;
                    params.currentUse_Lend = m.currentUse_Lend;
                    params.currentUse_Rent = m.currentUse_Rent;
                    params.currentUse_Self = m.currentUse_Self;
                    params.price = m.price;
                    params.month = 1;
                    params.income = m.income;
                }
          
                all.push(params);
            });
            propertyService.submitall(all).then(function () {
                alert("全部提交成功！");
            });

        }
      

        app.filter('search1', function () {

            return function () {
                var searchproperties = [];
                var search = $scope.search;
                if (search != null || search != ""||search!=undefined) {

                    angular.forEach($scope.monthtotal, function (model, i) {
                        if (model.name.indexOf(search) > -1 || model.address.indexOf(search) > -1) {
                            searchproperties.push(model);
                        }
                    });
                    total = searchproperties;
                    $scope.monthtotalcount = total.length;
                }
                return total;
            }  

        });

        app.filter('search2', function () {

            return function () {
                var searchproperties = [];
                var search = $scope.search1;
                if (search != null || search != "" || search != undefined) {

                    angular.forEach($scope.submonthtotal, function (model, i) {
                        if (model.name.indexOf(search) > -1 || model.address.indexOf(search) > -1) {
                            searchproperties.push(model);
                        }
                    });
                    subtotal = searchproperties;
                    $scope.submonthtotalcount = subtotal.length;
                }
                return subtotal;
            }

        });

    }]);

