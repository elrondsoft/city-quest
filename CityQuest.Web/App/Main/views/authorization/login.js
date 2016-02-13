(function () {
    var controllerId = 'app.views.authorization.login';
    angular.module('app').controller(controllerId, [
        '$scope', '$state', 'authService', 'ngAuthSettings', '$stateParams', '$http', 'abp.services.cityQuest.user',
        function ($scope, $state, authService, ngAuthSettings, $stateParams, $http, userSvc) {
            var vm = this;
            vm.localize = function (key) {
                return abp.localization.localize(key, 'CityQuest');
            };
            vm.loginStates = {
                signIn: 1,
                signUp: 2
            }
            vm.currentState = vm.loginStates.signIn;
            vm.changeState = function (state) {
                vm.currentState = state;
            };
            vm.loginData = {
                userName: "",
                password: "",
                isPersist: false
            };
            vm.loginPagePromise = null;
            vm.login = function () {
                vm.loginPagePromise = authService.login(vm.loginData).then(function (response) {
                    if ($stateParams.returnState) {
                        $state.go($stateParams.returnState);
                    } else {
                        $state.go(ngAuthSettings.homeStateName);
                    }
                },
                 function (err) {

                 });
            };

            vm.newUser = {
                userName: null,
                name: null,
                surname: null,
                emailAddress: null,
                phoneNumber: null,
                password: null
            };

            vm.signUp = function () {
                return userSvc.create({ Entity: vm.newUser })
                            .success(function (data) {
                            abp.message.success(vm.localize('SignUpSuccess_Body'), vm.localize('SignUpSuccess_Header'));
                            //vm.currentState = vm.loginStates.signIn;
                            vm.loginData = {
                                userName: vm.newUser.userName,
                                password: vm.newUser.password,
                                isPersist: false
                            };
                            vm.login();
                        }).error(function (data) {
                            abp.message.error(vm.localize('SignUpFailed_Body'), vm.localize('SignUpFailed_Header'));
                });
            };
        }
    ]);
})();