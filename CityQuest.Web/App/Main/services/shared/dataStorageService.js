'use strict';
(function () {
    angular.module('app').factory('dataStorageService', ['$localStorage', '$sessionStorage',
        function ($localStorage, $sessionStorage) {
        var dataStorageServiceFactory = {};
        var _get = function (key) {
            var output = window.localStorage.getItem(key) || window.sessionStorage.getItem(key);
            return JSON.parse(output);
        };
        var _set = function (key, val, isPersist) {
            if (isPersist) {
                window.localStorage.setItem(key, JSON.stringify(val));
            }
            else {
                window.sessionStorage.setItem(key, JSON.stringify(val));
            }
        };
        var _remove = function (key) {
            window.localStorage.removeItem(key);
            window.sessionStorage.removeItem(key);
        }

        dataStorageServiceFactory.get = _get;
        dataStorageServiceFactory.set = _set;
        dataStorageServiceFactory.remove = _remove;
        return dataStorageServiceFactory;
    }]);
})();