"use strict";

//统计服务服务
app.service("StatisticsService", ['$rootScope', '$http', '$q', function ($rootScope, $http, $q) {

    //概况统计
    this.getOverviewStatistics = function (showSelf, includeChildren) {
        var deferred = $q.defer();

        var req = {
            method: "get",
            url: $rootScope.apiUrl + 'Statistics/Ovewview',
            params: {
                showSelf: showSelf,
                includeChildren: includeChildren
            }
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //根据行政区域统计
    this.getGroupStatistics = function (showSelf,includeChildren,propertyType,statistics) {
        var deferred = $q.defer();

        var req = {
            method: "get",
            url: $rootScope.apiUrl + 'Statistics/Group',
            params: {
                showSelf: showSelf,
                includeChildren: includeChildren,
                propertyType: propertyType,
                statistics: statistics
            }
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };

    //使用现状统计
    this.getCurrentUsageStatistics = function (showSelf, includeChildren, land) {
        var deferred = $q.defer();

        var req = {
            method: "get",
            url: $rootScope.apiUrl + 'Statistics/CurrentUsage',
            params: {
                showSelf: showSelf,
                includeChildren: includeChildren,
                land: land
            }
        };

        $http(req).success(function (data, status) {
            deferred.resolve(data);
        }).error(function (data, status) {
            deferred.reject(data.Message);
        });

        return deferred.promise;
    };
}]);
