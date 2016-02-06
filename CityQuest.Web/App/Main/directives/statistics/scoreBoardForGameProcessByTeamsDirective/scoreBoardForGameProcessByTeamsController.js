(function () {
    var app = angular.module('app');
    app.directive('scoreBoardForGameProcessByTeams', ['$', 'clientCityQuestConstService', 'clientPermissionService', 'abp.services.cityQuest.gameLight',
        function ($, constSvc, permissionSvc, gameLightSvc) {
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
                    vm.gameId = $scope.gameId;
                    vm.scoreBoardLoadPromise = null;
                    vm.scoreBoardReloadPromise = null;
                    vm.scoreBoardData = [];
                    vm.signalRStatisticsChangesHub = null;
                    //---------------------------------------------------------------------------------------------------------
                    //----------------------------------------Helpers----------------------------------------------------------
                    vm.helpers = {
                        initStatisticsGhangedEvents: function () {
                            vm.signalRStatisticsChangesHub.client.onGameTaskCompleted = function (data) {
                                if (vm.scoreBoardReloadPromise == null) {
                                    vm.helpers.getScoreBoardForGame(vm.scoreBoardReloadPromise);
                                }
                            };
                        },
                        getScoreBoardForGame: function (promiseStore) {
                            var promise = gameLightSvc.retrieveScoreBoardForGame({
                                GameId: vm.gameId
                            }).success(function (data) {
                            vm.scoreBoardData = [
                                { place: 1, teamName: 'teamA', completedTasksCount: 3, score: 5 },
                                { place: 2, teamName: 'teamB', completedTasksCount: 3, score: 4 },
                                { place: 3, teamName: 'teamC', completedTasksCount: 3, score: 3 }
                            ];
                            }).finally(function () {
                                if (promiseStore != null) {
                                    vm.scoreBoardLoadPromise = null;
                                }
                            });
                            if (promiseStore != undefined) {
                                promiseStore = promise;
                            }
                            return promise;
                        },
                        reloadScoreBoard: function () {
                            return vm.helpers.getScoreBoardForGame(vm.scoreBoardLoadPromise);
                        },
                    };
                    //---------------------------------------------------------------------------------------------------------
                    //---------------------------------------Initialize--------------------------------------------------------
                    vm.helpers.getScoreBoardForGame(vm.scoreBoardLoadPromise);

                    vm.signalRHelper = {
                        /// Is used to start SignalR connection
                        startSignalRConnection: function () {
                            $.connection.hub.stop();
                            vm.signalRStatisticsChangesHub = $.connection.signalRStatisticsChangesHub;
                            vm.helpers.initStatisticsGhangedEvents();
                            $.connection.hub.start().done(function () { });
                        },
                    }
                    vm.signalRHelper.startSignalRConnection();
                    /// Is used to restart SignalR connection
                    var restartSignalRConnectionWithInterval = setInterval(function () {
                        var needReconnect = $.connection.hub && $.connection.hub.state === $.signalR.connectionState.disconnected;
                        if (needReconnect) {
                            vm.signalRHelper.startSignalRConnection();
                        }
                    }, constSvc.signalRReconnetInterval);
                    //---------------------------------------------------------------------------------------------------------
                }
            }
        }]);
})();