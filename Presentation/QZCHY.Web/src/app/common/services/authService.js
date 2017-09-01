'use strict';
//认证服务
app.factory('authService', ['$http', '$q', '$localStorage', 'ngAuthSettings', function ($http, $q, $localStorage,ngAuthSettings) {

    var serviceApiBase = ngAuthSettings.apiServiceBaseUri + "api/";
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        account: "",
        useRefreshTokens: false,
        roles: "",
        isAdmin: true
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });

    };

    var _login = function (loginData) {

        var data = "grant_type=password&userName=" + loginData.account + "&password=" + loginData.password;

        var deferred = $q.defer();

        $http.post(serviceApiBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

            //是否为管理员登录
            if (loginData.isAdmin && !(_userHasRole(response.userRoles, "管理员")||_userHasRole(response.userRoles, "注册单位"))) {
                _logOut();
                deferred.reject({ "error": "无效授权", "error_description": "账户无登录权限" });
            }
            else
            {
                var authroziationData = { token: response.access_token, userName: loginData.account, refreshToken:"", useRefreshTokens: false, roles: response.userRoles, isAdmin: _userHasRole(response.userRoles, "管理员") > -1 };
                
                if (loginData.useRefreshTokens) {
                    authroziationData.useRefreshTokens = true;
                    authroziationData.refreshToken = response.refresh_token;
                }
 
                $localStorage.authorizationData = authroziationData;

                _authentication.isAuth = true;
                _authentication.account = loginData.account;
                _authentication.useRefreshTokens = loginData.useRefreshTokens;
                _authentication.roles = response.userRoles;
                _authentication.isAdmin = _userHasRole(response.userRoles, "管理员") > -1;

                console.log($localStorage.authorizationData);

                deferred.resolve(response);
            }
        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _logOut = function () {

        $localStorage.authorizationData = null;

        _authentication.isAuth = false;
        _authentication.account = "";
        _authentication.useRefreshTokens = false;

    };

    var _isAdmin = function () {
        return _userHasRole(_authentication.roles, "管理员") > -1;
    };

    var _fillAuthData = function () {

        var authData = $localStorage.authorizationData;
        //var authData = $localStorage.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.account = authData.userName;
            _authentication.roles = authData.roles;
            _authentication.useRefreshTokens = authData.useRefreshTokens;
            _authentication.isAdmin = authData.isAdmin;
        }
    };

    var _refreshToken = function () {
        var deferred = $q.defer();
        var authData = $localStorage.authorizationData;
        //var authData = $localStorage.get('authorizationData');

        if (authData) {

            if (authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                $localStorage.authorizationData = null;
                //$localStorage.remove('authorizationData');

                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                    $localStorage.authorizationData = { token: response.access_token, userName: response.account, refreshToken: response.refresh_token, useRefreshTokens: true, isAdmin: _userHasRole(response.userRoles, "管理员") > -1 };

                    deferred.resolve(response);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });
            }
        }

        return deferred.promise;
    };

    var _userHasRole = function (roles, targetRole) {

        return roles.indexOf(targetRole);

        //for (var j = 0; j < roles.length; j++) {
        //    if (targetRole == roles[j]) {
        //        return true;
        //    }
        //}
        //return false;
    };
 
    //var _isUrlAccessibleForUser = function (route) {
    //    for (var i = 0; i < userRole.length; i++) {
    //        var role = userRole[i];
    //        var validUrlsForRole = userRoleRouteMap[role];
    //        if (validUrlsForRole) {
    //            for (var j = 0; j < validUrlsForRole.length; j++) {
    //                if (validUrlsForRole[j] == route)
    //                    return true;
    //            }
    //        }
    //    }
    //    return false;
    //}

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;
    authServiceFactory.isAdmin = _isAdmin;
    authServiceFactory.userHasRole = _userHasRole;

    return authServiceFactory;
}]);