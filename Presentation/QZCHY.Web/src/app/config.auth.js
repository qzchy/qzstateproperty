'use strict';

angular.module('app').config(function ($httpProvider) {
	$httpProvider.interceptors.push('authInterceptorService');
}).run(['authService', function (authService) {
	authService.fillAuthData();
}]);

angular.module('app').constant('ngAuthSettings', {
    apiServiceBaseUri: 'http://localhost/qzstateproperty/',
    clientId: 'administration',
});