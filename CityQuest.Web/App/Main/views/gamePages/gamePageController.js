(function () {
    var controllerId = 'app.views.gamePages.gamePageController';
    angular.module('app').controller(controllerId, ['$scope', '$stateParams', '$', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.gameLight',
        function ($scope, params, $, constSvc, permissionSvc, gameLightSvc) {
            //---------------------------------------------------------------------------------------------------------------
            //-----------------------------------------PreInitialize---------------------------------------------------------
            var vm = this;
            /// Is used for localization
            vm.localize = constSvc.localize;
            /// Is used to store template's title
            vm.title = vm.localize("GamePage");
            /// Is used to store game's id
            vm.gameId = params.gameId;
            /// Is used to store available game's tasks
            vm.gameTasks = [];
            /// Initialize PartialTemplate's routes
            vm.partialTemplates = constSvc.cityQuestPartialTemplates.gamePagePartialTemplates;
            //---------------------------------------------------------------------------------------------------------------
            //--------------------------------------------Helpers------------------------------------------------------------
            var helpers = {
                loadAvailableGameTasks: function () {
                    var promise = gameLightSvc.retrieveGameLightTasks({ GameId: vm.gameId })
                        .success(function (data) {
                            var checkIsCompleted = function (gameTask) {
                                var result = false;
                                for (var i = 0; i < data.completedGameTaskIds.length; i++) {
                                    result = data.completedGameTaskIds[i] == gameTask.id;
                                    i = result ? data.completedGameTaskIds.length : i;
                                }
                                return result;
                            };
                            vm.gameTasks = data.gameTasks;
                            for (var i = 0; i < vm.gameTasks.length; i++) {
                                vm.gameTasks[i].isCompleted = checkIsCompleted(vm.gameTasks[i]);
                            }
                            return vm.gameTasks;
                        })
                        .finally(function (data) {

                        });
                    return promise;
                },
                initStatisticsGhangedEvents: function () {
                    vm.signalRStatisticsChangesHub.client.onGameTaskCompleted = function (data) {
                        var needToReloadGameTasks = false;
                        for (var i = 0; i < data.UserCompleterIds.length; i++) {
                            needToReloadGameTasks = abp.session.userId == data.UserCompleterIds[i] ?
                                true : needToReloadGameTasks;
                            i = abp.session.userId == data.UserCompleterIds[i] ? data.UserCompleterIds.length : i;
                        }
                        if (needToReloadGameTasks) {
                            helpers.loadAvailableGameTasks();
                        };
                    };
                },
                initOnGameGhangedEvents: function () {
                    vm.signalRGameChangesHub.client.onGameAdded = function (data) {
                        vm.actions.loadAndAddNewGame(data.GameId);
                    };
                    vm.signalRGameChangesHub.client.onGameUpdated = function (data) {
                        vm.actions.loadAndAddNewGame(data.GameId);
                    };
                    vm.signalRGameChangesHub.client.onGameStatusChanged = function (data) {
                        vm.actions.loadAndAddNewGame(data.GameId);
                    };
                    vm.signalRGameChangesHub.client.onGameDeleted = function (data) {
                        vm.actions.removeGame(data.GameId);
                    };
                    vm.signalRGameChangesHub.client.onGameDeactivated = function (data) {
                        vm.actions.loadAndAddNewGame(data.GameId);
                    };
                    vm.signalRGameChangesHub.client.onGameActivated = function (data) {
                        vm.actions.loadAndAddNewGame(data.GameId);
                    };
                },
            };
            //---------------------------------------------------------------------------------------------------------------
            //--------------------------------------Permissions on actions---------------------------------------------------
            vm.permissionsOnActions = {
                canTryToPassCondition: function (gameTask, condition, value) {
                    return true;
                },
            };
            //---------------------------------------------------------------------------------------------------------------
            //--------------------------------------------Actions------------------------------------------------------------
            vm.actions = {
                tryToPassCondition: function (gameTask, condition, value) {
                    condition.lockedForPass = true;
                    var promise = gameLightSvc.tryToPassCondition({
                        ConditionId: condition.id,
                        Value: value
                    }).success(function (data) {
                        if (data.result == true) {
                            gameTask.isCompleted = true;
                            //TODO: pop-up msg
                        } else if(data.result == false) {
                            //TODO: pop-up msg
                        }
                    }).finally(function (data) {
                        condition.lockedForPass = false;
                    });
                    return promise;
                },
                getCompletedGameTasks: function () {
                    var result = [];
                    if (vm.gameTasks != null && vm.gameTasks.length > 0) {
                        for (var i = 0; i < vm.gameTasks.length; i++) {
                            if (vm.gameTasks[i].isCompleted == true) {
                                result.push(vm.gameTasks[i]);
                            }
                        }
                    }
                    return result;
                },
                getNotCompletedGameTasks: function () {
                    var result = [];
                    if (vm.gameTasks != null && vm.gameTasks.length > 0) {
                        for (var i = 0; i < vm.gameTasks.length; i++) {
                            if (vm.gameTasks[i].isCompleted == false) {
                                result.push(vm.gameTasks[i]);
                            }
                        }
                    }
                    return result;
                },
            };
            vm.gameRelationActions = {
                changeGameTaskViewMode: function (gameTask) {
                    gameTask.isShortViewMode = !gameTask.isShortViewMode;
                    return gameTask;
                },
                changeConditionViewMode: function (condition) {
                    condition.isShortViewMode = !condition.isShortViewMode;
                    return condition;
                },
                changeTipViewMode: function (tip) {
                    tip.isShortViewMode = !tip.isShortViewMode;
                    return tip;
                },
            };
            //---------------------------------------------------------------------------------------------------------------
            //------------------------------------------Initialize-----------------------------------------------------------
            helpers.loadAvailableGameTasks();

            /// Is used to start SignalR connection
            var startSignalRConnection = function () {
                $.connection.hub.stop();
                vm.signalRGameChangesHub = $.connection.signalRGameChangesHub;
                helpers.initOnGameGhangedEvents();
                vm.signalRStatisticsChangesHub = $.connection.signalRStatisticsChangesHub;
                helpers.initStatisticsGhangedEvents();
                $.connection.hub.start().done(function () { });
            }();
            /// Is used to restart SignalR connection
            var restartSignalRConnectionWithInterval = setInterval(function () {
                var needReconnect = $.connection.hub && $.connection.hub.state === $.signalR.connectionState.disconnected;
                if (needReconnect) {
                    startSignalRConnection();
                }
            }, 5000);
        }
    ]);
})();