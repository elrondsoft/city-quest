(function () {
    var app = angular.module('app');
    app.directive('gameTasksBuilder', [
        function () {
        return {
            restrict: 'E',
            templateUrl: '/App/Main/directives/games/gameTasks/gameTasksBuilderTemplate.cshtml',
            scope: {},
            bindToController: {
                gameId: '=',
                gameTasks: '=',
                gameTaskTypes: '='
            },
            controllerAs: 'gameTasksBuilder',
            controller: function ($scope) {
                var vm = this;

                vm.gameTaskAvailableActions = {
                    canAddGameTaskOnTop: function () {
                        return true;
                    },
                    canAddGameTaskOnBottom: function () {
                        return true;
                    },
                    canDeleteGameTask: function (entity) {
                        return true;
                    },
                    canDeleteAllGameTasks: function () {
                        return true;
                    },
                    canMoveGameTaskToTop: function (entity) {
                        return true;
                    },
                    canMoveGameTaskToBottom: function (entity) {
                        return true;
                    },
                    canMoveGameTaskUp: function (entity) {
                        return true;
                    },
                    canMoveGameTaskDown: function (entity) {
                        return true;
                    },
                    canMinimize: function (entity) {
                        return entity && !entity.isMinimized;
                    },
                    canMaximize: function (entity) {
                        return entity && entity.isMinimized;
                    },
                };

                vm.gameTaskActions = {
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
                };
                

            }
        }
    }]);
})();