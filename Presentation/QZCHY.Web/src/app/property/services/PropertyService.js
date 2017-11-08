"use strict";

//资产服务
app.service("PropertyService", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {

    //获取资产列表
    this.getAllProperties = function (params) {
        var deferred = $q.defer();
        var url = $rootScope.apiUrl + 'Properties' + '?_=' + new Date().getTime();

        $http.post(url, params).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    this.getAllPropertiesByUrl = function (params) {
        var deferred = $q.defer();
        var url = $rootScope.apiUrl + 'Properties' + '?_=' + new Date().getTime();

        var req = {
            method: "get",
            url: url,
            params: params
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    this.getFilterProcessProperties= function (params) {
        var deferred = $q.defer();
        var url = $rootScope.apiUrl + 'Properties/ProcessFilter' + '?_=' + new Date().getTime();

        var req = {
            method: "get",
            url: url,
            params: params
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };


    //获取地图大数据
    this.getPropertiesBigDataInMap = function (params) {
        var deferred = $q.defer();

        var req = {
            method: "get",
            url: $rootScope.apiUrl + 'Properties/Geo/bigdata' + '?_=' + new Date().getTime(),
            params: params
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //关键字联想
    this.propertiesSuggest = function (params) {
        var deferred = $q.defer();

        var req = {
            method: "get",
            url: $rootScope.apiUrl + 'Properties/geo/suggestion' + '?_=' + new Date().getTime(),
            params: params
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //地图中获取资产列表
    this.getAllPropertiesInMap = function (params) {
        var deferred = $q.defer();
        var url = $rootScope.apiUrl + 'Properties/geo' + '?_=' + new Date().getTime();

        $http.post(url, params).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    this.getPropertiesByGovernment = function (governmentId, loadChildren) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/Governments/' + governmentId + "?loadChildren=" + loadChildren).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //创建资产
    this.createProperty = function (property) {
        var deferred = $q.defer();

        $http.post($rootScope.apiUrl + 'Properties/Create', property).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //更新资产
    this.updateProperty = function (property) {
        var deferred = $q.defer();

        $http.put($rootScope.apiUrl + 'Properties/' + property.id, property).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //获取资产
    this.getPropertyById = function (property_id ){
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/' + property_id).success(function (data, status) {
            deferred.resolve(data, status);
        }).error(function (data, status) {
            deferred.reject("error");
        });

        return deferred.promise;
    };

    //获取资产的更新类型
    this.getUpdatedPropertyById = function (property_id) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/Update/' + property_id).success(function (data, status) {
            deferred.resolve(data, status);
        }).error(function (message, status) {
            deferred.reject({ status: status, message: message });
        });

        return deferred.promise;
    };

    //获取待处置资产
    this.getPropertyProcess = function () {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/PropertyProcess').success(function (data, status) {
            deferred.resolve(data, status);
        }).error(function (data, status) {
            deferred.reject("error");
        });

        return deferred.promise;
    };

    //删除资产
    this.deleteProperty = function (propertyId) {
        var deferred = $q.defer();

        $http.delete($rootScope.apiUrl + 'Properties/' + propertyId).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //批量删除资产
    this.deletePropertyByIds = function (property_ids) {
        var deferred = $q.defer();

        $http.delete($rootScope.apiUrl + 'Properties/' + property_ids).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //获取所有资产创建新增记录
    this.getAllPropertyNewCreateRecords = function (params) {
        var deferred = $q.defer();
        var url = $rootScope.apiUrl + 'Properties/Process/Records'
            + '?_=' + new Date().getTime();

        var req = {
            method: "get",
            url: url,
            params: params
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //创建资产出借表
    this.createLend = function (lend) {
        var deferred = $q.defer();

        $http.post($rootScope.apiUrl + 'Properties/Lend' ,lend).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //创建资产出租表
    this.createRent = function (rent) {
        var deferred = $q.defer();

        $http.post($rootScope.apiUrl + 'Properties/Rent', rent).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //创建资产划拨表
    this.createAllot = function (allot) {
        var deferred = $q.defer();

        $http.post($rootScope.apiUrl + 'Properties/Allot', allot).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };
   
    //获取筛选后的待处置资产
    this.getFilterProperty = function (params) {

        var deferred = $q.defer();
        var url = $rootScope.apiUrl + 'Properties/PropertyFilter';

        var req = {
            method: "get",
            url: url,
            params: params
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
      
    };

    //创建资产消核
    this.createOff = function (off) {
        var deferred = $q.defer();

        $http.post($rootScope.apiUrl + 'Properties/Off', off).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //编辑资产消核
    this.updateOff = function (off) {
        var deferred = $q.defer();

        $http.put($rootScope.apiUrl + 'Properties/UpdateOff/'+off.id, off).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //编辑资产出借
    this.updateLend = function (lend) {
        var deferred = $q.defer();

        $http.put($rootScope.apiUrl + 'Properties/UpdateLend/'+lend.id, lend).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //编辑资产出租表
    this.updateRent = function (rent) {
        var deferred = $q.defer();

        $http.put($rootScope.apiUrl + 'Properties/UpdateRent/'+rent.id, rent).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //编辑资产划拨表
    this.updateAllot = function (allot) {
        var deferred = $q.defer();

        $http.put($rootScope.apiUrl + 'Properties/UpdateAllot/'+allot.id, allot).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //查看资产出借
    this.getApproveDetail = function (approveType,id) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/'+approveType+'/' + id).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //查看资产出借
    this.getLend = function (id) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/Lend/' + id).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //查看资产出租表
    this.getRent = function (id) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/Rent/' + id).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //查看资产划拨表
    this.getAllot = function (id) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/Allot/' + id).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //查看资产消核
    this.getOff = function (id) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/Off/' + id).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //提交资产审批
    this.submitApprove=function(id,approveType)
    {
        var deferred = $q.defer();
        $http.post($rootScope.apiUrl + 'Properties/SubmitApprove/' + id + "?approveType=" + approveType)
            .success(function (data, status) {
                deferred.resolve(data);
            }).error(function (data, status) {
                deferred.reject(data.message);
            });

        return deferred.promise;
    }

    this.multiSubmitApprove = function (idString, approveType) {
        var deferred = $q.defer();
        $http.post($rootScope.apiUrl + 'Properties/SubmitApprove/Multi/' + idString + "?approveType=" + approveType)
            .success(function (data, status) {
                deferred.resolve(data);
            }).error(function (data, status) {
                deferred.reject(data.message);
            });

        return deferred.promise;
    }

    //审批申请
    this.applyApprove = function (id, agree, suggestion,approveType) {

        var deferred = $q.defer();

        var approve = {
            agree: agree,
            suggestion: suggestion,
            approveType: approveType
        };

        $http.put($rootScope.apiUrl + 'Properties/ApplyApprove/' + id, approve)
            .success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    this.multiApplyApprove = function (idString, agree, suggestion, approveType) {

        var deferred = $q.defer();

        var approve = {
            agree: agree,
            suggestion: suggestion,
            approveType: approveType
        };

        $http.put($rootScope.apiUrl + 'Properties/ApplyApprove/Multi/' + idString, approve)
            .success(function (data, status) {
                deferred.resolve(data);
            }).error(function (data, status) {
                deferred.reject(data.message);
            });

        return deferred.promise;
    };

    //删除资产
    this.deletePropertyApprove = function (approveId,approveType) {
        var deferred = $q.defer();

        $http.delete($rootScope.apiUrl + 'Properties/Approve/' + approveId + "?approveType=" + approveType).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //获取资产相关出租情况
    this.getLendsByPropertyId = function (id) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/Lends/' + id).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //获取资产相关出租情况
    this.getLendsByPropertyId = function (id) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/Lends/' + id).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //获取资产相关出租情况
    this.getLendsByPropertyId = function (id) {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/Lends/' + id).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //获取处置统计信息
    this.approveStatistics = function () {
        var deferred = $q.defer();

        $http.get($rootScope.apiUrl + 'Properties/Approve/Statistics').success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //导出EXCEL
    this.export = function (ids,params1) {
        var deferred = $q.defer();
        $http({  
            method: "POST",
            //headers: {
            //    'Content-type': 'application/vnd.ms-excel'
            //},
            url:$rootScope.apiUrl + 'Properties/Export/' + ids,  
            data:  params1,
            responseType: "arraybuffer"
        }).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        }); 
        
        return deferred.promise;
    };

    //资产批量导入
    this.Import = function () {
        var deferred = $q.defer();

        $http.post($rootScope.apiUrl + 'Properties/Import').success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };
  
    
}]);
 