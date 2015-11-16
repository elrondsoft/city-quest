(function () {
    'use strict';

    var app = angular.module('app', [
        'ngAnimate',
        'ngSanitize',
        'ngStorage',
        'ui.router',
        'ui.bootstrap',
        'ui.jq',

        'abp'
    ]);

    app.constant('ngAuthSettings', {
        apiServiceBaseUri: abp.appPath,
        clientId: 'ngAuthApp',
        loginStateName: 'login',
        loginMenuName: 'Login',
        homeStateName: 'home'

    });

    app.value('params', null);

    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    });

    app.run(['authService', '$state', 'ngAuthSettings', function (authService, $state, ngAuthSettings) {
        authService.fillAuthData()
    }]);

})();