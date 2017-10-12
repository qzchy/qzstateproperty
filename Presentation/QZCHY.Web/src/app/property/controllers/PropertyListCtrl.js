﻿//资产列表控制器
'use strict';
 
app.controller('PropertyListCtrl', ['$window', '$rootScope', '$uibModal', '$state', '$scope', '$timeout', 'PropertyService', 'GovernmentService', 
    function ($window, $rootScope, $uibModal, $state, $scope, $timeout, propertyService, governmentService) {
        var error = $scope.error = "";

        $scope.processing = false;
        //高级搜索的部门搜索 
        $scope.governments = [];
        $scope.government = {};
        $scope.properties = [];

        //自定义参数集合
        var params = {
            showHidden: true,
            pageIndex:0,
            pageSize: 15,
            query:"",
            sort: "getedDate,desc;",
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
            injection:false,
            ct: false,
            jt: false,
            jk: false,
            self: false,
            storeUp: false,
            adjust: false,
            greenland: false,
            house: false,
            constructAreaRange: "",
            landAreaRange: "",
            priceRange: "",
            getDateRange: "",       
        };

       
        $scope.params = angular.copy(params);

     

        //导出字段集合
        var fields = {
            isName: false,
            isGovernment: false,
            isPGovernment: false,
            isPropertyType: false,
            isGovernmentType: false,
            isRegion: false,
            isAddress: false,
            isConstructArea: false,
            isLandArea: false,
            isPropertyID: false,
            isPropertyNature: false,
            isLandNature: false,
            isPrice: false,
            isGetedDate: false,
            isLifeTime: false,
            isUsedPeople: false,
            isCurrentUse_Self: false,
            isCurrentUse_Rent: false,
            isCurrentUse_Lend: false,
            isCurrentUse_Idle:false,
            isNextStepUsage: false,
            isEstateId: false,
            isConstructId: false,
            isLandId: false,
            isHasConstructID: false,
            isHasLandID: false,
            isRent: false,
            isLend: false      
        };
        $scope.fields = angular.copy(fields);

        //选中的行
        $scope.selectedItem = [];

        //#region table config

        //列集合
        $scope.columns = [{
            type: 'link',
            title: "名称",
            name: "name",
            orderable: true,
            linkField: "id",
            click: function (propertId) {
                $state.go("app.property.detail", { id: propertId });
            },
        }, {
            title: "地址",
            width: 220,
            name: "address",
            orderable: true
        }, {
            title: "区域",
            width: 100,
            name: "region",
            orderable: true,
            alignCenter: true
        }, {
            title: "取得日期",
            width: 150,
            name: "getedDate",
            orderable: true,
            alignCenter: true,
            dir: "desc"
        }, {
            type: 'link',
            title: "产权单位",
            width: 250,
            name: "governmentName",
            orderable: true,
            alignCenter: true,
            linkField: "governmentId",
            click: function (governmentId) {
                $state.go("app.property.government", { id: governmentId });
            },
        }];

        //编辑删除功能
        $scope.tableEidtAndDelete = [
             {
                 title: "资产变更",
                 handler: function (row) {                 
                         $state.go("app.property.edit", { id: row.id });               
                 },
                 isEnable: function (row) { return row.canChange; }
             }
  ]

        //$scope.tableEidtAndDelete = {
        //    edit: function (propertId) {
        //        $state.go("app.property.edit", { id: propertId });
        //    },
        //    del: function (rows) {
        //        var idStr = "";
        //        var content = "";

        //        //遍历生成idStr
        //        if (rows.length == 1) {
        //            idStr = rows[0].id;
        //            content = "是否删除资产《" + rows[0].name + "》？";
        //        }
        //        else if (rows.length > 1) {
        //            content = "是否删除当前选中的资产？";

        //            angular.forEach(rows, function (value, index) {
        //                if (value.selected) idStr += value.id + '_';
        //            });

        //        }

        //        var modalInstance = $uibModal.open({
        //            templateUrl: 'deleteRow.html',
        //            controller: 'ModalDialogCtrl',
        //            windowClass: "map-modal",
        //            resolve: {
        //                title: function () { return "提示"; },
        //                content: function () { return content; },
        //                dialogHeight: function () { return $rootScope.dialogHeight; },
        //                statistics: function () { return null; }
        //            }
        //        });

        //        modalInstance.result.then(function () {

        //            //删除用户
        //            propertyService.deleteProperty(idStr).then(function (data, status) { }, function (message) { alert(message); }).finally(function () {
        //                $state.reload();
        //            });

        //        }, function () { });
        //    },
      
        //};

        //高级搜索
        $scope.advanceSearch = function () {
            var modalInstance = $uibModal.open({
                templateUrl: 'advDialog.html',
                controller: 'AdvanceModalDialogCtrl',
                size: 'lg',
                windowClass: "map-modal",
                //appendTo: '#app',
                resolve: {
                    dialogHeight: function () { return $scope.dialogHeight; },
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
                $state.reload();
            });
        };

        $scope.resetParams = function () {
            $scope.params = angular.copy(params);
            $scope.governments = [];
            $scope.government = {};
            $scope.ajax();
        };

        $scope.reset = true;

        //#endregion    


        //region 资产导出
        $scope.Export = function () {
            var modalInstance = $uibModal.open({
                templateUrl: 'export.html',
                controller: 'exportCtrl',
                size: 'lg',
                windowClass: "map-modal",
                //appendTo: '#app',
                resolve: {
                    dialogHeight: function () { return $scope.dialogHeight; },
                    fields: function () { return $scope.fields },
                    governmentService: function () { return governmentService; },
                    governments: function () { return $scope.governments; },
                    government: function () { return $scope.government },
                    exportExcel: function () { return $scope.exportExcel; },
                    resetParams: function () { return $scope.resetParams; }
                }
            });

            modalInstance.result.then(function () {
            }, function () {
                $state.reload();
            });
        }


        $scope.selectedRows = [];
        //创建id字符串
        var buildIdsString = function () {
            var idsString = "";
                angular.forEach($scope.selectedRows, function (data, index) {
                    idsString += data.id + (index == $scope.selectedRows.length - 1 ? "" : ";");
                });
                    
            return idsString;
        };

        $scope.exportExcel = function () {
            var ids = buildIdsString();
            if (ids == "") ids = "all";

            var params1 = {
                query:$scope.params.query,
                sort: "name,asc;",
                currentExtent: false,
                bbox: "",
                government: {
                    manage: 'true',
                    isGovernment: $scope.params.isGovernment,
                    isInstitution: false,
                    isCompany:$scope.params.isCompany,
                    selectedId: 0
                },
                extent: {
                    type: "",
                    geo: ""
                },
                propertyType: {
                    construct: $scope.params.construct,
                    land: $scope.params.land,
                    constructOnLand: $scope.params.constructOnLand
                },
                region: {
                    old: $scope.params.old,
                    west: $scope.params.west,
                    jjq: $scope.params.jjq,
                    kc: $scope.params.kc,
                    qj: $scope.params.qj,
                    other: $scope.params.other,
                },
                certificate: {
                    both: false,
                    land: false,
                    construct: false,
                    none: false
                },
                current: {
                    self: $scope.params.current_self,
                    rent: $scope.params.current_rent,
                    lend: $scope.params.current_lend,
                    idle: $scope.params.current_idle
                },
                nextStep: { mapType: 0, auction: false, injection: false, storeUp: false, adjust: false, ct: false, jt: false, jk: false },
                constructArea: { ranges: [], l: false, m: false, h: false, t: false },
                landArea: { ranges: [], l: false, m: false, h: false, t: false },
                price: { ranges: [], l: false, m: false, h: false, t: false },
                lifeTime: { min: 0, max: 70 },
                getedDate: { min: 0, max: 0 },
                fields:$scope.fields
            };
            $scope.params1 = angular.copy(params1);

            propertyService.export(ids,$scope.params1).then(function (response) {
               // alert("导出成功！");
            })
        }


        //#endregion    


        //#region 设置对话框高度
        var windowHeight = $window.innerHeight; //获取窗口高度
        var headerHeight = 50;
        var footerHeight = 51;
        var windowWidth = $window.innerWidth; //获取窗口宽度

        $scope.dialogHeight = { "height": windowHeight - headerHeight - footerHeight - 230 };
        //#endregion

        $scope.ajax = function () {

            if ($scope.processing) return;

            if ($scope.government.selected != null && $scope.government.selected != undefined) $scope.params.selectedId = $scope.government.selected.id;
            else $scope.params.selectedId = 0;

            //#region 建筑面积、土地面积、账面价值区间参数

            ////#region 建筑面积
            //$scope.params.constructArea.ranges = [];

            //if ($scope.params.constructArea.l)
            //    $scope.params.constructArea.ranges.push({ min: 0, max: 5000 });

            //if ($scope.params.constructArea.m)
            //    $scope.params.constructArea.ranges.push({ min: 5001, max: 10000 });

            //if ($scope.params.constructArea.h)
            //    $scope.params.constructArea.ranges.push({ min: 10001, max: 20000 });

            //if ($scope.params.constructArea.t)
            //    $scope.params.constructArea.ranges.push({ min: 20001, max: 0 });

            ////#endregion

            ////#region 土地面积
            //$scope.params.landArea.ranges = [];

            //if ($scope.params.landArea.l)
            //    $scope.params.landArea.ranges.push({ min: 0, max: 10000 });

            //if ($scope.params.landArea.m)
            //    $scope.params.landArea.ranges.push({ min: 10001, max: 50000 });

            //if ($scope.params.landArea.h)
            //    $scope.params.landArea.ranges.push({ min: 50001, max: 100000 });

            //if ($scope.params.landArea.t)
            //    $scope.params.landArea.ranges.push({ min: 100001, max: 0 });

            ////#endregion

            ////#region 账面价值
            //$scope.params.price.ranges = [];

            //if ($scope.params.price.l)
            //    $scope.params.price.ranges.push({ min: 0, max: 500 });

            //if ($scope.params.price.m)
            //    $scope.params.price.ranges.push({ min: 501, max: 5000 });

            //if ($scope.params.price.h)
            //    $scope.params.price.ranges.push({ min: 5001, max: 10000 });

            //if ($scope.params.price.t)
            //    $scope.params.price.ranges.push({ min: 10001, max: 0 });

            ////#endregion

            //#endregion

            $scope.processing = true;
            $scope.params.time = new Date().getTime();
            propertyService.getAllPropertiesByUrl($scope.params).then(function (response) {
                if ($scope.response != null && $scope.response != undefined && response.time < $scope.response.time) return;
                $scope.response = response;
            }, function () {
                alert("获取数据失败");
            }).finally(function () {
                $scope.processing = false;
            });
        };

    }]);

app.controller('exportCtrl', function ($scope, $uibModalInstance, dialogHeight, fields, governmentService, governments, government, exportExcel, resetParams) {

    $scope.dialogHeight = dialogHeight;
    $scope.okText = "确定";
    $scope.cancelText = "取消";
    $scope.fields = fields;
    $scope.exportExcel = exportExcel;

    $scope.ok = function () {
        $scope.exportExcel();
    }

    //全选
    $scope.selectAll = function () {

        for (var data in $scope.fields) {
            $scope.fields[data] = true;
        }

    }
    //重置
    $scope.resetFields = function () {

        for (var data in $scope.fields) {
            $scope.fields[data] = false;
        }

    }
    //取消
    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    };

})

app.controller('AdvanceModalDialogCtrl', function ($scope, $uibModalInstance, dialogHeight, params, governmentService, governments, government, ajax, resetParams) {

    $scope.dialogHeight = dialogHeight;
    $scope.okText = "确定";
    $scope.cancelText = "取消";

    $scope.params = params;
  
    $scope.governmentService = governmentService;

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
        $uibModalInstance.close();
        ajax();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    };

    $scope.resetParams = function () {
        resetParams();

        $scope.params = {
            showHidden: true,
            pageSize: 15,
            query: "",
            sort: "name,asc;",
            government: {
                manage: 'true',
                isGovernment: false,
                isInstitution: false,
                isCompany: false,
                selectedId: 0
            },
            extent: {
                type: "",
                geo: ""
            },
            propertyType: {
                construct: false,
                land: false,
                constructOnLand: false
            },
            region: {
                old: false,
                west: false,
                jjq: false,
                kc: false,
                qj: false,
                other: false,
            },
            certificate: {
                both: false,
                land: false,
                construct: false,
                none: false
            },
            current: {
                self: false,
                rent: false,
                lend: false,
                idle: false
            },
            nextStep: {
                development: false,
                auction: false,
                injection: false,
                self: false,
                storeUp: false,
                adjust: false,
                greenland: false
            },
            constructArea: { ranges: [], l: false, m: false, h: false, t: false },
            landArea: { ranges: [], l: false, m: false, h: false, t: false },
            price: { ranges: [], l: false, m: false, h: false, t: false },
            lifeTime: { min: 0, max: 70 },
            getedDate: { min: 1956, max: new Date().getFullYear() }
        };
        $scope.governments = [];
        $scope.government = {};
    };
});