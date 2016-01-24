(function () {
    var controllerId = 'app.templates.games.gameProcessManagementController';
    angular.module('app').controller(controllerId, ['$scope', 'serviceData', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.game', 'abp.services.cityQuest.gameLight',
        function ($scope, serviceData, constSvc, permissionSvc, gameSvc, gameLightSvc) {
            //---------------------------------------------------------------------------------------------------------------
            //------------------------------------------PreInitialize--------------------------------------------------------
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize('GameProcessManagement');
            vm.gameStatistics = {
                series: [],
                labels: [""],
                data: [[0]]
            };
            //---------------------------------------------------------------------------------------------------------------
            //------------------------------------Template's promise service-------------------------------------------------
            vm.templatePromiseService = {
                templatePromisesGroups:{
                    globalTemplatePromises: [],
                    gameEntityPromises: [],
                    gameStatisticsPromises: [],
                },
                getPromisesGroup: function (promisesGroupName) {
                    var promisesGroup = null;
                    if (vm.templatePromiseService.templatePromisesGroups != null &&
                        vm.templatePromiseService.templatePromisesGroups[promisesGroupName] != null) {
                        promisesGroup = vm.templatePromiseService.templatePromisesGroups[promisesGroupName];
                    }
                    return promisesGroup;
                },
                addPromise: function (promisesGroupName, promise) {
                    var promisesGroup = vm.templatePromiseService.getPromisesGroup(promisesGroupName);
                    if (promisesGroup != null && promisesGroup.length && promisesGroup.length > 0 && promise != null) {
                        promisesGroup.push(promise);
                        return promisesGroup;
                    }
                    return false;
                },
                removePromise: function (promisesGroupName, promise) {
                    var promisesGroup = vm.templatePromiseService.getPromisesGroup(promisesGroupName);
                    if (promisesGroup != null && promisesGroup.length && promisesGroup.length > 0 && promise != null) {
                        var index = null;
                        for (var i = 0; i < promisesGroup.length; i++) {
                            index = promisesGroup[i] == promise ? i : index;
                            i = promisesGroup[i] == promise ? promisesGroup.length : i;
                        }
                        if (index != null && index > -1) {
                            promisesGroup.splice(index, 1);
                        }
                        return promisesGroup;
                    }
                    return false;
                },
            };
            //---------------------------------------------------------------------------------------------------------------
            //---------------------------------------------Helpers-----------------------------------------------------------
            var helpers = {
                retrieveGame: function (gameId) {
                    var promise = gameSvc.retrieve({
                        Id: gameId,
                        IsActive: false
                    }).success(function (data) {
                        vm.gameEntity = data.retrievedEntity;
                    }).finally(function (data) {
                        vm.templatePromiseService.removePromise('gameEntityPromises', promise);
                        $scope.$digest();
                    });

                    vm.templatePromiseService.addPromise('gameEntityPromises', promise);

                    return promise;
                },
                retrieveGameStatistics: function (gameId) {
                    if (vm.templatePromiseService.gameStatisticsPromises &&
                        vm.templatePromiseService.gameStatisticsPromises.length < 1) {
                        return false;
                    }

                    var promise = gameLightSvc.retrieveGameTaskResults({
                        GameId: gameId
                    }).success(function (data) {
                        vm.teamGameTaskStatistics = data.teamGameTaskStatistics;
                    }).finally(function (data) {
                        vm.templatePromiseService.removePromise('gameStatisticsPromises', promise);
                        $scope.$digest();
                    });

                    vm.templatePromiseService.addPromise('gameStatisticsPromises', promise);

                    return promise;
                },
                initTemplateData: function () {
                    if (serviceData.gameId) {
                        helpers.retrieveGame(serviceData.gameId);
                        helpers.retrieveGameStatistics(serviceData.gameId);
                        return true;
                    }
                    return false;
                },
                calculateGameStatistics: function (teamGameTaskStatistics) {
                    if (teamGameTaskStatistics != null) {
                        var sortTeamGameTaskStatisticsByValue = function (dataToSort) {
                            //TODO: sort

                            return dataToSort;
                        };
                        var mapTeamGameTaskStatisticsToStatisticsData = function (teamGameTaskStatisticsData) {
                            teamGameTaskStatisticsData = sortTeamGameTaskStatisticsByValue(teamGameTaskStatisticsData);
                            var mapDataValue = function (value) {
                                var result = moment(value).format("YYYY-MM-DD HH:mm:ss");
                                return result;
                            };
                            var statisticsData = [];
                            for (var i = 0; i < teamGameTaskStatisticsData.length; i++) {
                                var a = {
                                    seriesValue: teamGameTaskStatisticsData[i].teamName,
                                    labelValue: teamGameTaskStatisticsData[i].gameTaskName,
                                    dataValue: mapDataValue(teamGameTaskStatisticsData[i].GameTaskEndDateTime),
                                }
                                statisticsData.push();
                            }
                            return statisticsData;
                        };
                        var fillStatistics = function (data) {
                            var statisticsData = mapTeamGameTaskStatisticsToStatisticsData(data);
                            var unicValues = Enumerable.From(statisticsData).Select(function (x) { return x.dataValue; }).Distinct().ToArray();

                            for (var i = 0; i < unicValues.length; i++) {
                                //TODO:
                            }
                        };

                        fillStatistics(teamGameTaskStatistics);
                    }
                    return vm.gameStatistics;
                },
            };
            //---------------------------------------------------------------------------------------------------------------
            //--------------------------------Template's permissions on actions----------------------------------------------
            vm.templatePermissionsOnActions = {
                canStartGame: function (game) {
                    return permissionSvc.game.canStartGameProcess(game);
                },
                canPauseGame: function (game) {
                    return permissionSvc.game.canPauseGameProcess(game);
                },
                canResumeGame: function (game) {
                    return permissionSvc.game.canResumeGameProcess(game);
                },
                canEndGame: function (game) {
                    return permissionSvc.game.canEndGameProcess(game);
                },
            };
            //---------------------------------------------------------------------------------------------------------------
            //-----------------------------------------Template's actions----------------------------------------------------
            vm.templateActions = {
                startGame: function (gameId) {
                    var promise = gameSvc.startGame({ GameId: gameId })
                        .success(function (data) {
                            vm.gameEntity.gameStatus = data.newGameStatus;
                            vm.gameEntity.gameStatusId = data.newGameStatus.id;
                        }).finally(function (data) {
                            vm.templatePromiseService.removePromise('gameEntityPromises', promise);
                            $scope.$digest();
                        });
                    
                    vm.templatePromiseService.addPromise('gameEntityPromises', promise);

                    return promise;
                },
                pauseGame: function (gameId) {
                    var promise = gameSvc.pauseGame({ GameId: gameId })
                        .success(function (data) {
                            vm.gameEntity.gameStatus = data.newGameStatus;
                            vm.gameEntity.gameStatusId = data.newGameStatus.id;
                        }).finally(function (data) {
                            vm.templatePromiseService.removePromise('gameEntityPromises', promise);
                            $scope.$digest();
                        });
                    
                    vm.templatePromiseService.addPromise('gameEntityPromises', promise);

                    return promise;
                },
                resumeGame: function (gameId) {
                    var promise = gameSvc.resumeGame({ GameId: gameId })
                        .success(function (data) {
                            vm.gameEntity.gameStatus = data.newGameStatus;
                            vm.gameEntity.gameStatusId = data.newGameStatus.id;
                        }).finally(function (data) {
                            vm.templatePromiseService.removePromise('gameEntityPromises', promise);
                            $scope.$digest();
                        });
                    
                    vm.templatePromiseService.addPromise('gameEntityPromises', promise);

                    return promise;
                },
                endGame: function (gameId) {
                    var promise = gameSvc.endGame({ GameId: gameId })
                        .success(function (data) {
                            vm.gameEntity.gameStatus = data.newGameStatus;
                            vm.gameEntity.gameStatusId = data.newGameStatus.id;
                        }).finally(function (data) {
                            vm.templatePromiseService.removePromise('gameEntityPromises', promise);
                            $scope.$digest();
                        });
                    
                    vm.templatePromiseService.addPromise('gameEntityPromises', promise);

                    return promise;
                },
                close: function () {
                    $scope.$close();
                },
            };
            //---------------------------------------------------------------------------------------------------------------
            //--------------------------------------------Initialize---------------------------------------------------------
            var listenerForStatistics = $scope.$watch(vm.teamGameTaskStatistics,
                function (newValue, oldValue) {
                    return helpers.calculateGameStatistics(vm.teamGameTaskStatistics);
                }, true);

            helpers.initTemplateData();
            //---------------------------------------------------------------------------------------------------------------
        }
    ]);
})();