(function () {
    var controllerId = 'app.views.layout.header';
    angular.module('app').controller(controllerId, [
        '$rootScope', '$state', "authService", 'ngAuthSettings', '$timeout', '$window',
        function ($rootScope, $state, authService, ngAuthSettings, $timeout, $window) {
            var vm = this;

            vm.languages = abp.localization.languages;
            vm.currentLanguage = abp.localization.currentLanguage;

            vm.menu = abp.nav.menus.MainMenu;
            vm.currentMenuName = $state.current.menu;

            vm.authSettings = ngAuthSettings;

            vm.onLangChange = function () {
                authService.clearScript();
            };

            vm.logout = function () {
                abp.auth.grantedPermissions = {};
                //abp.session = {};
                authService.logOut();
                $state.go(ngAuthSettings.loginStateName);
            };

            vm.isActive = function (menuItem) {
                var result = vm.currentMenuName == menuItem.name || (menuItem.items.length && menuItem.items.any(function (elm) { return elm.name === vm.currentMenuName }));
                return result;
            };

            vm.displayMenuItemNameWithSubItem = function (menuItem) {
                var result = menuItem.displayName;
                if (menuItem.items.length) {
                    var activeSubItem = Enumerable.From(menuItem.items)
                        .Where(function (elm) { return vm.isActive(elm) })
                        .SingleOrDefault(null);
                    if (activeSubItem) {
                        result += ' - ';
                        result += activeSubItem.displayName;
                    }
                }
                return result;
            };

            vm.isCurrentStateLogin = function () {
                return !$state.$current || !$state.$current.self || $state.$current.self.name === ngAuthSettings.loginStateName;
            }

            vm.getAuthentication = function () {
                return authService.authentication;
            }

            $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
                vm.currentMenuName = toState.menu;
            });
        }
    ]);
})();