"use strict";

//用户服务
app.service("AccountService", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {

    var baseUrl = $rootScope.apiUrl + 'Systemmanage/Accounts/';

    //获取用户列表
    this.getAllAccounts = function (params) {
        var deferred = $q.defer();

        var req = {
            method: "get",
            url: baseUrl + '?_=' + new Date().getTime(),
            params: params
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //创建用户
    this.createAccount = function (accountUser) {       
        var deferred = $q.defer();

        $http.post(baseUrl, accountUser).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //更新用户
    this.updateAccount = function (accountUser) {
        var deferred = $q.defer();

        $http.put(baseUrl + accountUser.id, accountUser).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //修改密码
    this.resetPwd = function (accountUser) {
        var deferred = $q.defer();

        $http.put(baseUrl +"resetpwd", accountUser).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.message);
        });

        return deferred.promise;
    };

    //获取用户
    this.getAccountById = function (account_id ){
        var deferred = $q.defer();

        $http.get(baseUrl + account_id).success(function (data, status) {
            deferred.resolve(data, status);
        }).error(function (data, status) {
            deferred.reject("error");
        });

        return deferred.promise;
    };

    //删除商家
    this.deleteAccount = function (accountId) {
        var deferred = $q.defer();

        $http.delete(baseUrl + accountId).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //批量删除用户
    this.deleteAccountByIds = function (store_ids) {
        var deferred = $q.defer();

        $http.delete(baseUrl + store_ids).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };
}]);
 