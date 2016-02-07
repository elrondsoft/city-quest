(function () {
    var app = angular.module('app');
    app.directive('statisticsForGameProcessByTeams', ['$', 'clientCityQuestConstService', 'clientPermissionService', 'abp.services.cityQuest.gameLight',
        function ($, constSvc, permissionSvc, gameLightSvc) {
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
                    vm.statisticsLoadPromise = null;
                    vm.statisticsReloadPromise = null;
                    vm.statisticsData = [];
                    vm.signalRStatisticsChangesHub = null;
                    //---------------------------------------------------------------------------------------------------------
                    //----------------------------------------Helpers----------------------------------------------------------
                    vm.helpers = {
                        initStatisticsGhangedEvents: function () {
                            vm.signalRStatisticsChangesHub.client.onGameTaskCompleted = function (data) {
                                if (vm.statisticsReloadPromise == null) {
                                    vm.helpers.getStatisticsForGame('statisticsReloadPromise');
                                }
                            };
                        },
                        getStatisticsForGame: function (promiseStoreName) {
                            var promise = gameLightSvc.retrieveGameTaskResults({
                                GameId: vm.gameId
                            }).success(function (data) {
                                vm.statisticsData = data.teamGameTaskStatistics;
                                vm.helpers.prepareStatisticsData(vm.statisticsData);
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
                        reloadStatistics: function () {
                            return vm.helpers.getStatisticsForGame('statisticsReloadPromise');
                        },
                        prepareStatisticsData: function (statisticsData) {
                            var teamNames = Enumerable.From(statisticsData)
                                .Select(function (x) { return x.teamName }).Distinct().ToArray();
                            var labels = Enumerable.From(statisticsData)
                                .Select(function (x) { return x.gameTaskEndDateTime; })
                                .OrderBy(function (x) { return x; })
                                .Distinct().ToArray();
                            statisticsData = Enumerable.From(statisticsData)
                                .OrderBy(function (x) { return x.gameTaskOrder; })
                                .ToArray();
                            vm.data = [];
                            for (var i = 0; i < teamNames.length; i++) {
                                var dataForTeam = [];
                                var statisticsDataForTeam = Enumerable.From(statisticsData)
                                    .Where(function (x) { return x.teamName === teamNames[i]; })
                                    .ToArray();
                                var currentGameTaskName = 0;
                                for (var j = 0; j < labels.length; j++) {
                                    var dataForLabel = Enumerable.From(statisticsData)
                                        .Where(function (x) { return x.gameTaskEndDateTime === labels[j]; })
                                        .ToArray();
                                    if (dataForLabel != null && dataForLabel.length > 0) {
                                        currentGameTaskName = dataForLabel[0].gameTaskOrder;
                                    }
                                    dataForTeam.push(currentGameTaskName);
                                }
                                vm.data.push(dataForTeam);
                            }
                            vm.series = teamNames;
                            vm.labels = [];

                            var dateFormat = 'YYYY-MM-DD HH:mm:ss';
                            if (moment(labels[0]).format('YYYY') === moment(labels[labels.length - 1]).format('YYYY')) {
                                dateFormat = 'MM-DD HH:mm:ss';
                            } else if (moment(labels[0]).format('YYYY-MM') === moment(labels[labels.length - 1]).format('YYYY-MM')) {
                                dateFormat = 'DD HH:mm:ss';
                            } else if (moment(labels[0]).format('YYYY-MM-DD') === moment(labels[labels.length - 1]).format('YYYY-MM-DD')) {
                                dateFormat = 'HH:mm:ss';
                            }

                            for (var k = 0; k < labels.length; k++) {
                                vm.labels.push(moment(labels[k]).format(dateFormat));
                            }
                        },
                        isStatisticsAvailable: function () {
                            var result = ((vm.statisticsData != null) && (vm.statisticsData.length > 0)) &&
                                ((vm.labels != null) && (vm.labels.length > 0)) &&
                                ((vm.series != null) && (vm.series.length > 0)) &&
                                ((vm.data != null) && (vm.data.length > 0));
                            return result;
                        },
                    };
                    //---------------------------------------------------------------------------------------------------------
                    //---------------------------------------Initialize--------------------------------------------------------
                    vm.helpers.getStatisticsForGame('statisticsLoadPromise');

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