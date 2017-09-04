"use strict";

//单位服务
app.service("GovernmentService", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {

    var baseUrl = $rootScope.apiUrl + 'Systemmanage/Governments/';

    //获取目录树
    this.loadTreeData = function () {
        var deferred = $q.defer();
        var url = baseUrl + "treeview";

        $http.get(url).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //增加单位
    this.createGovernment = function (government) {
        var deferred = $q.defer();

        $http.post(baseUrl, government).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };
    
    //删除单位
    this.deleteGovernment = function (government_id) {
        var deferred = $q.defer();

        $http.delete(baseUrl + government_id).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //更新单位
    this.updateGovernment = function (government) {
        var deferred = $q.defer();

        $http.put(baseUrl + government.id, government).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //获取用户
    this.getGovernmentById = function (government_id) {
        var deferred = $q.defer();

        $http.get(baseUrl + government_id).success(function (data, status) {
            deferred.resolve(data, status);
        }).error(function (data, status) {
            deferred.reject("error");
        });

        return deferred.promise;
    }

    //获取当前用户
    this.getCurrentGovernment = function () {
        var deferred = $q.defer();

        $http.get(baseUrl + 'CurrentUser').success(function (data, status) {
            deferred.resolve(data, status);
        }).error(function (data, status) {
            deferred.reject("error");
        });

        return deferred.promise;
    }

    //获取子单位
    this.getChildrenGovernmentById = function (government_id) {
        var deferred = $q.defer();

        $http.get(baseUrl +"children/"+ government_id).success(function (data, status) {
            deferred.resolve(data, status);
        }).error(function (data, status) {
            deferred.reject("error");
        });

        return deferred.promise;
    }

    //获取当前账号的部门
    this.getCurrentAccountGovernemnt = function () {
        return alert("暂未实现");

    };

    //获取当前账号的子部门
    this.getCurrentAccountChidlrenGovernemnts = function () {
        var deferred = $q.defer();
        var url = baseUrl + "currentAccount/children/selectlist";

        $http.get(url).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //获取下拉列表集合
    this.loadSelectListItems = function () {
        var deferred = $q.defer();
        var url = baseUrl + "selectlist";

        $http.get(url).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    }
    
    //机关名称搜索
    this.autocompleteByName = function (name) {
        var deferred = $q.defer();
        var url = baseUrl + "Autocomplete/" + name;

        $http.get(url).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    }

    //获取地图大数据
    this.getGovernmentsBigDataInMap = function (params) {
        var deferred = $q.defer();

        var req = {
            method: "get",
            url: baseUrl + '/Geo/bigdata' + '?_=' + new Date().getTime(),
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
    this.suggestion = function (params) {
        var deferred = $q.defer();

        var req = {
            method: "get",
            url: baseUrl + 'geo/suggestion' + '?_=' + new Date().getTime(),
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
    this.getAllGovernmentsInMap = function (params) {
        var deferred = $q.defer();
        var url = baseUrl + 'geo' + '?_=' + new Date().getTime();

        $http.post(url, params).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };
}]);
 