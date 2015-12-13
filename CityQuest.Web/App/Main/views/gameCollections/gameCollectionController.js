(function () {
    var controllerId = 'app.views.gameCollections.gameCollectionController';
    angular.module('app').controller(controllerId, ['$scope', '$', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.key', 'abp.services.cityQuest.gameLight',
        function ($scope, $, constSvc, permissionSvc, keySvc, gameLightSvc) {
            var vm = this;

            //---------------------------------------------------------------------------------------------------------
            //-----------------------------------------PreInitialize---------------------------------------------------
            /// Is used to store load promises
            vm.loadPromisesService = {
                loadPromises: [],
                addLoadPromise: function (loadPromise) {
                    vm.loadPromisesService.loadPromises.push(loadPromise);
                    return vm.loadPromisesService.loadPromises;
                },
                removeLoadPromise: function (loadPromise) {
                    if (loadPromise && vm.loadPromisesService.loadPromises &&
                        vm.loadPromisesService.loadPromises.length &&
                        vm.loadPromisesService.loadPromises.length > 0) {
                        var index = null;
                        for (var i = 0; i < vm.loadPromisesService.loadPromises.length; i++) {
                            if (vm.loadPromisesService.loadPromises[i] == loadPromise) {
                                index = i;
                                i = vm.loadPromisesService.loadPromises.length;
                            }
                        }
                        if (index != null && index > -1) {
                            vm.loadPromisesService.loadPromises.splice(index, 1);
                        }
                    }
                    return vm.loadPromisesService.loadPromises;
                },
            };
            /// Is used for localization
            vm.localize = constSvc.localize;
            /// Is used to store template's title
            vm.title = vm.localize("GameCollection");
            /// Is used to store game collection
            vm.gameCollection = [];
            //---------------------------------------------------------------------------------------------------------
            //-------------------------------------Key activator service-----------------------------------------------
            vm.keyActivatorService = {
                keyValue: null,
                keyActivationInProgress: false,
                validateKeyValue: function () {
                    return true;
                },
                canActivateKey: function () {
                    return permissionSvc.key.canActivate();
                },
                activateKey: function () {
                    vm.keyActivatorService.keyActivationInProgress = true;
                    var promise = keySvc.activateKey({
                        Key: vm.keyActivatorService.keyValue
                    }).success(function (data) {
                        vm.keyActivatorService.keyValue = null;
                        vm.helpers.addGame(data.activatedGame);
                    }).finally(function (data) {
                        vm.loadPromisesService.removeLoadPromise(promise);
                        vm.keyActivatorService.keyActivationInProgress = false;
                        $scope.$digest();
                    });
                    vm.loadPromisesService.addLoadPromise(promise);
                    return promise;
                },
            };
            //---------------------------------------------------------------------------------------------------------
            //-------------------------------------------Helpers-------------------------------------------------------
            vm.helpers = {
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
                getGameById: function (gameId) {
                    var game = null;
                    if (vm.gameCollection && vm.gameCollection.length && vm.gameCollection.length > 0) {
                        for (var i = 0; i < vm.gameCollection.length; i++) {
                            if (vm.gameCollection[i].id == gameId) {
                                game = vm.gameCollection[i];
                                i = vm.gameCollection.length;
                            }
                        }
                    }
                    return game;
                },
                getGamesIndex: function (gameId) {
                    var index = null;
                    if (vm.gameCollection && vm.gameCollection.length && vm.gameCollection.length > 0) {
                        for (var i = 0; i < vm.gameCollection.length; i++) {
                            if (vm.gameCollection[i].id == gameId) {
                                index = i;
                                i = vm.gameCollection.length;
                            }
                        }
                    }
                    return index;
                },
                addGame: function (game) {
                    if (game && game.id) {
                        var existGame = vm.helpers.getGameById(game.id);
                        if (existGame) {
                            var newIsActual = true;
                            if (newIsActual) {
                                angular.merge(existGame, game);
                            }
                        } else {
                            vm.gameCollection.push(game);
                        }
                        return true;
                    }
                    return false;
                },
                removeGame: function (gameId) {
                    var index = vm.helpers.getGamesIndex(gameId);
                    if (index != null) {
                        vm.gameCollection.splice(index, 1);
                        return true;
                    }
                    return false;
                }
            };
            //---------------------------------------------------------------------------------------------------------
            //---------------------------------Template's permissions on actions---------------------------------------
            vm.permissionsOnActions = {

            };
            //---------------------------------------------------------------------------------------------------------
            //--------------------------------------Template's actions-------------------------------------------------
            vm.actions = {
                retrieveGameCollection: function () {
                    var promise = gameLightSvc.retrieveGameCollection({})
                        .success(function (data) {
                            vm.gameCollection = data.gameCollection;
                        }).finally(function (data) {
                            vm.loadPromisesService.removeLoadPromise(promise);
                            $scope.$digest();
                        });
                    vm.loadPromisesService.addLoadPromise(promise);
                    return promise;
                },
                loadAndAddNewGame: function (gameId) {
                    var promise = gameLightSvc.retrieveGameLight({
                        GameId: gameId
                    }).success(function (data) {
                        vm.helpers.addGame(data.game);
                    }).finally(function (data) {
                        $scope.$digest();
                    });
                    return promise;
                },
                removeGame: function (gameId) {
                    vm.helpers.addGame(gameId);
                    $scope.$digest();
                },
            };
            //---------------------------------------------------------------------------------------------------------
            //-------------------------------------------Initialize----------------------------------------------------
            vm.actions.retrieveGameCollection();

            $.connection.hub.stop();
            vm.signalRGameChangesHub = $.connection.signalRGameChangesHub;
            vm.helpers.initOnGameGhangedEvents();
            $.connection.hub.start().done(function () { });
        }
    ]);
})();