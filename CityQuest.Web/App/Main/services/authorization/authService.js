'use strict';
(function () {
    angular.module('app').factory('authService', ['$http', '$q', 'dataStorageService', 'ngAuthSettings',
        function ($http, $q, dataStorageService, ngAuthSettings) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: "",
            isPersist: false,
            abpScript: ""
        };


        var _login = function (loginData) {

            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            var deferred = $q.defer();

            $http.post(
                serviceBase + 'token',
                data,
                {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }).success(function (response) {

                var authData = { token: response.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false, isPersist: loginData.isPersist };
                dataStorageService.set('authorizationData', authData, loginData.isPersist);

                _authentication.isAuth = true;
                _authentication.userName = loginData.userName;
                _authentication.isPersist = loginData.isPersist;
                _reloadScript().finally(function () {
                    deferred.resolve(response);
                });

            }).error(function (err, status) {
                _logOut();
                deferred.reject(err);
            });

            return deferred.promise;

        };

        var _logOut = function () {

            dataStorageService.remove('authorizationData');

            _authentication.isAuth = false;
            _authentication.userName = "";
            _authentication.isPersist = false;

        };

        var _reloadScript = function () {
            return $http.get('/AbpScripts/GetScripts', { cache: false }).success(function (data) {
                _authentication.abpScript = data;
                var authData = dataStorageService.get('authorizationData');
                if (authData) {
                    dataStorageService.set('authorizationData', authData, authData.isPersist);
                }
                _setAbpScript();
            });
        };

        var _clearScript = function () {
            _authentication.abpScript = null;
            var authData = dataStorageService.get('authorizationData');
            if (authData) {
                authData.abpScript = null;
                dataStorageService.set('authorizationData', authData, authData.isPersist);
                var a = dataStorageService.get('authorizationData');
                abp.log.info(a);
            }
        }

        var _setAbpScript = function () {
            //this is safa but bad, we change non angular static part of code
            var head = $('head');
            $("#getScripts").remove();
            var script = document.createElement("script");
            script.id = "getScripts";
            script.type = "text/javascript";
            script.text = _authentication.abpScript;
            head.append(script);
        };

        var _fillAuthData = function () {
            var authData = dataStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.userName = authData.userName;
                _authentication.isPersist = authData.isPersist;
                _authentication.abpScript = authData.abpScript;
                if (_authentication.abpScript) {
                    _setAbpScript();
                } else {
                    _reloadScript();
                }
                return true;
            }
            return false;
        };

        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;
        authServiceFactory.clearScript = _clearScript;
        return authServiceFactory;
    }]);
})();