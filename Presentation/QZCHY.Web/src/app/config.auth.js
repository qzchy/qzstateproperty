'use strict';

angular.module('app').config(function ($httpProvider) {
	$httpProvider.interceptors.push('authInterceptorService');
}).run(['authService', function (authService) {
	authService.fillAuthData();
}]);

angular.module('app').constant('ngAuthSettings', {
    apiServiceBaseUri: 'http://172.27.45.53/',
    clientId: 'administration',
});