(function () {
    var app = angular.module('app');
    app.directive('statisticsForGameProcessByTeams', ['clientCityQuestConstService', 'clientPermissionService',
        function (constSvc, permissionSvc) {
            return {
                restrict: 'E',
                templateUrl: '/App/Main/directives/statistics/statisticsForGameProcessByTeamsDirective/statisticsForGameProcessByTeamsTemplate.cshtml',
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
                    vm.labels = ['12:00', '12:10', '12:20', '12:30', '12:40', '12:50', '13:00', '14:10', '15:10'];
                    vm.series = ['Team A', 'Team B', 'Team C'];

                    vm.data = [
                          [0, 1, 1, 1, 1, 2, 2, 2, 2, 3],
                          [0, 0, 1, 1, 2, 2, 2, 2, 3, 3],
                          [0, 0, 0, 1, 1, 1, 2, 3, 3, 3],
                    ];
                    //---------------------------------------------------------------------------------------------------------
                }
            }
        }]);
})();