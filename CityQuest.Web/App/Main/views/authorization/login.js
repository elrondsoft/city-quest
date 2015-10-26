(function () {
    var controllerId = 'app.controllers.authorization.login';
    angular.module('app').controller(controllerId, [
        '$scope', '$state', 'authService', 'ngAuthSettings', '$stateParams', '$http',
        function ($scope, $state, authService, ngAuthSettings, $stateParams, $http) {
            var vm = this;
            vm.loginData = {
                userName: "",
                password: "",
                isPersist: false
            };
            vm.loginPromise = null;
            vm.login = function () {
                vm.loginPromise = authService.login(vm.loginData).then(function (response) {
                    if ($stateParams.returnState) {
                        $state.go($stateParams.returnState);
                    } else {
                        $state.go(ngAuthSettings.homeStateName);
                    }
                },
                 function (err) {

                 });
            };

            vm.loginStates = {
                signIn: 1,
                signUp: 2
            }

            vm.currentState = vm.loginStates.signIn;
            vm.changeState = function (state) {
                vm.currentState = state;
            };
        }
    ]);
})();