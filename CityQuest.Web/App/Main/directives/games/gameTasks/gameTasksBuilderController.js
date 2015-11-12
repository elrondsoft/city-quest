(function () {
    var app = angular.module('app');
    app.directive('gameTasksBuilder', ['clientCityQuestConstService', 'clientPermissionService', //'abp.services.cityQuest.gameTask',
        function (constSvc, permissionSvc) {
        return {
            restrict: 'E',
            templateUrl: '/App/Main/directives/games/gameTasks/gameTasksBuilderTemplate.cshtml',
            scope: {},
            bindToController: {
                templateModeState: '=',
                gameId: '=',
                gameTasks: '=',
                gameTaskTypes: '=',
            },
            controllerAs: 'gameTasksBuilder',
            controller: function ($scope) {
                var vm = this;
                vm.localize = constSvc.localize;

                //---------------------------------------------------------------------------------------------------------
                /// Is used to store current load promise
                vm.loadPromise = null;
                //----------------------------------------Template's modes-------------------------------------------------
                /// Is used to get bool result for conmaring template's mode with standart ones
                vm.templateMode = {
                    isInfo: function () {
                        return vm.templateModeState && (vm.templateModeState == constSvc.formModes.info);
                    },
                    isCreate: function () {
                        return vm.templateModeState && (vm.templateModeState == constSvc.formModes.create);
                    },
                    isUpdate: function () {
                        return vm.templateModeState && (vm.templateModeState == constSvc.formModes.update);
                    }
                };
                //----------------------------------Template's actions service---------------------------------------------
                /// Is used to allow actions for gameTaskBuilder
                vm.gameTaskAvailableActions = {
                    canReloadGameTasks: function () {
                        return vm.templateMode.isUpdate();
                    },
                    canAddGameTaskOnTop: function () {
                        return vm.templateMode.isCreate() || vm.templateMode.isUpdate();
                    },
                    canAddGameTaskOnBottom: function () {
                        return vm.templateMode.isCreate() || vm.templateMode.isUpdate();
                    },
                    canDeleteGameTask: function (entity) {
                        return vm.templateMode.isCreate() || vm.templateMode.isUpdate();
                    },
                    canDeleteAllGameTasks: function () {
                        return vm.templateMode.isCreate() || vm.templateMode.isUpdate();
                    },
                    canMoveGameTaskToTop: function (entity) {
                        return vm.templateMode.isCreate() || vm.templateMode.isUpdate();
                    },
                    canMoveGameTaskToBottom: function (entity) {
                        return vm.templateMode.isCreate() || vm.templateMode.isUpdate();
                    },
                    canMoveGameTaskUp: function (entity) {
                        return vm.templateMode.isCreate() || vm.templateMode.isUpdate();
                    },
                    canMoveGameTaskDown: function (entity) {
                        return vm.templateMode.isCreate() || vm.templateMode.isUpdate();
                    },
                    canMinimize: function (entity) {
                        return entity && !entity.isMinimized;
                    },
                    canMaximize: function (entity) {
                        return entity && entity.isMinimized;
                    },
                    canActivateGameTask: function (entity) {
                        return !entity.isActive;
                    },
                    canDeactivateGameTask: function (entity) {
                        return entity.isActive;
                    },
                };
                //---------------------------------------------------------------------------------------------------------
                /// Is used to store actions can be allowed in gameTaskBuilder
                vm.gameTaskActions = {
                    reloadGameTasks: function () {
//TODO:
                        //vm.loadPromise = gameTaskSvc...
                        return vm.loadPromise;
                    },
                    setGameTasksOrders: function () {
                        angular.forEach(vm.gameTasks, function (value, key) {
                            value.order = key + 1;
                        });
                        return vm.gameTasks;
                    },
                    getNewGameTask: function (currentGameId) {
                        var emptyGameTask = {
                            gameId: currentGameId,
                            gameTaskTypeId: null,
                            tips: [],
                            conditions: [],
                            name: null,
                            description: null,
                            taskText: null,
                            isActive: true,
                            order: null,
                        };
                        return emptyGameTask;
                    },
                    addGameTaskOnTop: function () {
                        var newTask = vm.gameTaskActions.getNewGameTask(vm.gameId);
                        vm.gameTasks.unshift(newTask);
                        vm.gameTaskActions.setGameTasksOrders();
                        return vm.gameTasks;
                    },
                    addGameTaskOnBottom: function () {
                        var newTask = vm.gameTaskActions.getNewGameTask(vm.gameId);
                        newTask.order = vm.gameTasks.length + 1;
                        vm.gameTasks.push(newTask);
                        return vm.gameTasks;
                    },
                    deleteGameTask: function (order) {
                        var index = order - 1;
                        if (!(index > -1 && index < vm.gameTasks.length))
                            return false;

                        vm.gameTasks.splice(index, 1);
                        vm.gameTaskActions.setGameTasksOrders();
                        return vm.gameTasks;
                    },
                    deleteAllGameTasks: function () {
                        vm.gameTasks = [];
                    },
                    moveGameTaskToTop: function (order) {
                        var index = order - 1;
                        var movingGameTask = vm.gameTasks[index];
                        vm.gameTasks.splice(index, 1);
                        vm.gameTasks.unshift(movingGameTask);
                        vm.gameTaskActions.setGameTasksOrders();
                        return vm.gameTasks;
                    },
                    moveGameTaskToBottom: function (order) {
                        var index = order - 1;
                        var movingGameTask = vm.gameTasks[index];
                        vm.gameTasks.splice(index, 1);
                        vm.gameTasks.push(movingGameTask);
                        vm.gameTaskActions.setGameTasksOrders();
                        return vm.gameTasks;
                    },
                    swapGameTasks: function (order1, order2) {
                        var index1 = order1 - 1;
                        var index2 = order2 - 1;

                        if (!(index1 != index2 &&
                            index1 > -1 && index1 < vm.gameTasks.length && 
                            index2 > -1 && index2 < vm.gameTasks.length))
                            return false;

                        var store = vm.gameTasks[index1];
                        vm.gameTasks[index1] = vm.gameTasks[index2];
                        vm.gameTasks[index2] = store;

                        vm.gameTaskActions.setGameTasksOrders();

                        return vm.gameTasks;
                    },
                    moveGameTaskUp: function (order) {
                        var index = order - 1;

                        if (!(index > 0 && index < vm.gameTasks.length))
                            return false;

                        vm.gameTaskActions.swapGameTasks(index, order);
                        return vm.gameTasks;
                    },
                    moveGameTaskDown: function (order) {
                        var index = order - 1;

                        if (!(index > -1 && index < (vm.gameTasks.length - 1)))
                            return false;

                        vm.gameTaskActions.swapGameTasks(order, (order + 1));
                        return vm.gameTasks;
                    },
                    minimize: function (order) {
                        var index = order - 1;

                        if (!(index > -1 && index < vm.gameTasks.length))
                            return false;

                        (vm.gameTasks[index]).isMinimized = true;
                        return vm.gameTasks;
                    },
                    maximize: function (order) {
                        var index = order - 1;

                        if (!(index > -1 && index < vm.gameTasks.length))
                            return false;

                        (vm.gameTasks[index]).isMinimized = false;
                        return vm.gameTasks;
                    },
                    activateGameTask: function (order) {
                        var index = order - 1;

                        if (!(index > -1 && index < vm.gameTasks.length))
                            return false;

                        (vm.gameTasks[index]).isActive = true;
                        return vm.gameTasks;
                    },
                    deactivateGameTask: function (order) {
                        var index = order - 1;

                        if (!(index > -1 && index < vm.gameTasks.length))
                            return false;

                        (vm.gameTasks[index]).isActive = false;
                        return vm.gameTasks;
                    },
                };
            }
        }
    }]);
})();