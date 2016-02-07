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
                                    vm.helpers.getScoreBoardForGame('scoreBoardReloadPromise');
                                }
                            };
                        },
                        getScoreBoardForGame: function (promiseStoreName) {
                            var promise = gameLightSvc.retrieveScoreBoardForGame({
                                GameId: vm.gameId
                            }).success(function (data) {
                                vm.scoreBoardData = data.scoreBoardData;
                            }).finally(function () {
                                if (vm[promiseStoreName] != null) {
                                    vm[promiseStoreName] = null;
                                }
                            });
                            if (!((promiseStoreName != null) && (typeof vm[promiseStoreName] === "undefined"))) {
                                vm[promiseStoreName] = promise;
                            }
                            return promise;
                        },
                        reloadScoreBoard: function () {
                            return vm.helpers.getScoreBoardForGame('scoreBoardLoadPromise');
                        },
                        isGoldenPosition: function (teamScore) {
                            var result = teamScore.position != null && teamScore.position == 1;
                            return result;
                        },
                        isSilverPosition: function (teamScore) {
                            var result = teamScore.position != null && teamScore.position == 2;
                            return result;
                        },
                        isBronzePosition: function (teamScore) {
                            var result = teamScore.position != null && teamScore.position == 3;
                            return result;
                        },
                        isCommonPosition: function (teamScore) {
                            var result = !(vm.helpers.isGoldenPosition(teamScore) ||
                                vm.helpers.isSilverPosition(teamScore) || vm.helpers.isBronzePosition(teamScore));
                            return result;
                        },
                    };
                    //---------------------------------------------------------------------------------------------------------
                    //---------------------------------------Initialize--------------------------------------------------------
                    vm.helpers.getScoreBoardForGame('scoreBoardLoadPromise');

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