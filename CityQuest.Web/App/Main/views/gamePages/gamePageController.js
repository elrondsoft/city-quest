(function () {
    var controllerId = 'app.views.gamePages.gamePageController';
    angular.module('app').controller(controllerId, ['$scope', '$uibModal', '$stateParams', '$', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.gameLight',
        function ($scope, modal, params, $, constSvc, permissionSvc, gameLightSvc) {
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
            vm.game = null;
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
                            var setIsShortViewMode = function (gameTask) {
                                gameTask.isShortViewMode = gameTask.isCompleted;
                                if (gameTask.conditions != null && gameTask.conditions.length > 0) {
                                    for (var r = 0; r < gameTask.conditions.length; r++) {
                                        gameTask.conditions[r].isShortViewMode = true;
                                    }
                                }
                                if (gameTask.tips != null && gameTask.tips.length > 0) {
                                    for (var r = 0; r < gameTask.tips.length; r++) {
                                        gameTask.tips[r].isShortViewMode = true;
                                    }
                                }
                                return gameTask;
                            };
                            vm.game = data.game;
                            vm.gameTasks = data.gameTasks;
                            for (var i = 0; i < vm.gameTasks.length; i++) {
                                vm.gameTasks[i].isCompleted = checkIsCompleted(vm.gameTasks[i]);
                                setIsShortViewMode(vm.gameTasks[i]);
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
                        if (data.GameId == vm.gameId)
                            return helpers.loadAvailableGameTasks();
                        return false;
                    };
                    vm.signalRGameChangesHub.client.onGameUpdated = function (data) {
                        if (data.GameId == vm.gameId)
                            return helpers.loadAvailableGameTasks();
                        return false;
                    };
                    vm.signalRGameChangesHub.client.onGameStatusChanged = function (data) {
                        if (data.GameId == vm.gameId)
                            return helpers.loadAvailableGameTasks();
                        return false;
                    };
                    vm.signalRGameChangesHub.client.onGameDeleted = function (data) {
                        if (data.GameId == vm.gameId)
                            return helpers.loadAvailableGameTasks();
                        return false;
                    };
                    vm.signalRGameChangesHub.client.onGameDeactivated = function (data) {
                        if (data.GameId == vm.gameId)
                            return helpers.loadAvailableGameTasks();
                        return false;
                    };
                    vm.signalRGameChangesHub.client.onGameActivated = function (data) {
                        if (data.GameId == vm.gameId)
                            return helpers.loadAvailableGameTasks();
                        return false;
                    };
                },
            };
            //---------------------------------------------------------------------------------------------------------------
            //--------------------------------------Permissions on actions---------------------------------------------------
            vm.permissionsOnActions = {
                canTryToPassCondition: function (gameTask, condition, value) {
                    return vm.actions.isGameInProgress();
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
                isGamePaused: function () {
                    var result = false;
                    if (vm.game != null && vm.game.gameStatusName === 'GameStatus_Paused') {
                        result = true;
                    }
                    return result;
                },
                isGamePlanned: function () {
                    var result = false;
                    if (vm.game != null && vm.game.gameStatusName === 'GameStatus_Planned') {
                        result = true;
                    }
                    return result;
                },
                isGameInProgress: function () {
                    var result = false;
                    if (vm.game != null && vm.game.gameStatusName === 'GameStatus_InProgress') {
                        result = true;
                    }
                    return result;
                },
            };
            vm.gameRelationActions = {
                openScoreBoardTemplate: function () {
                    var newModalOptions = {
                        size: '',
                        templateUrl: constSvc.viewRoutes.scoreBoardTemplate,
                        controller: constSvc.ctrlRoutes.scoreBoardController,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.info,
                                    gameId: vm.gameId
                                };
                                return result;
                            },
                        }
                    };
                    var modalOptions = angular.merge({}, constSvc.defaultModalOptions, newModalOptions);
                    modal.open(modalOptions);
                    return false;
                },
                openGameTaskByTeamsStatisticTemplate: function () {
                    var newModalOptions = {
                        size: 'lg',
                        templateUrl: constSvc.viewRoutes.gameTaskByTeamStatisticTemplate,
                        controller: constSvc.ctrlRoutes.gameTaskByTeamStatisticController,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.info,
                                    gameId: vm.gameId
                                };
                                return result;
                            },
                        }
                    };
                    var modalOptions = angular.merge({}, constSvc.defaultModalOptions, newModalOptions);
                    modal.open(modalOptions);
                    return false;
                },
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

            vm.signalRHelper = {
                /// Is used to start SignalR connection
                startSignalRConnection: function () {
                    $.connection.hub.stop();
                    vm.signalRGameChangesHub = $.connection.signalRGameChangesHub;
                    helpers.initOnGameGhangedEvents();
                    vm.signalRStatisticsChangesHub = $.connection.signalRStatisticsChangesHub;
                    helpers.initStatisticsGhangedEvents();
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
            }, 5000);
        }
    ]);
})();