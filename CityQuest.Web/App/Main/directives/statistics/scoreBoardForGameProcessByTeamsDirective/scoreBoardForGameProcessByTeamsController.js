(function () {
    var app = angular.module('app');
    app.directive('scoreBoardForGameProcessByTeams', ['clientCityQuestConstService', 'clientPermissionService',
        function (constSvc, permissionSvc) {
            return {
                restrict: 'E',
                templateUrl: '/App/Main/directives/statistics/scoreBoardForGameProcessByTeamsDirective/scoreBoardForGameProcessByTeamsTemplate.cshtml',
                scope: {},
                bindToController: {
                    gameId: '=',
                },
                controllerAs: 'vm',
                controller: function ($scope) {
                    //---------------------------------------------------------------------------------------------------------
                    //-------------------------------------Pre-initialize------------------------------------------------------
                    var vm = this;
                    vm.localize = constSvc.localize;
                    //---------------------------------------------------------------------------------------------------------
                    //---------------------------------------Initialize--------------------------------------------------------

                    //---------------------------------------------------------------------------------------------------------
                }
            }
        }]);
})();