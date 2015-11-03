'use strict';
(function () {
    angular.module('app').factory('authInterceptorService', ['$q', '$injector', '$location', 'dataStorageService',
        function ($q, $injector, $location, dataStorageService) {

        var localize = function (key) {
            return abp.localization.localize(key, 'CityQuest');
        };

        var authInterceptorServiceFactory = {};

        var _request = function (config) {

            config.headers = config.headers || {};

            var authData = dataStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        }

        var _responseError = function (rejection) {
            if (rejection.status === 401) {
                var error = {
                    message: rejection.status + " - " + (rejection.statusText || "Unknown status text"),
                    details: rejection.data,
                    responseError: true
                }
                if (rejection.data.error === "invalid_grant") {
                    error.details = rejection.data.error_description;
                }
                else {
                    error.details = rejection.data;
                }
                var deferer = abp.ng.http.showError(error);
                abp.ng.http.logError(error);
                abp.ng.http.handleUnAuthorizedRequest(deferer);
            }
            return $q.reject(rejection);
        }

        authInterceptorServiceFactory.request = _request;
        authInterceptorServiceFactory.responseError = _responseError;

        return authInterceptorServiceFactory;
    }]);
})();