(function () {
    var app = angular.module('app');
    app.directive('conditionsBuilder', ['clientCityQuestConstService', 'clientPermissionService', 'abp.services.cityQuest.condition',
        function (constSvc, permissionSvc, conditionSvc) {
            return {
                restrict: 'E',
                templateUrl: '/App/Main/directives/games/conditions/conditionsBuilderTemplate.cshtml',
                scope: {},
                bindToController: {
                    templateMode: '=',
                    gameTaskId: '=',
                    conditions: '=',
                    conditionTypes: '=',
                },
                controllerAs: 'conditionsBuilder',
                controller: function ($scope) {
                    var vm = this;

                    vm.localize = constSvc.localize;

                    //---------------------------------------------------------------------------------------------------------
                    //-----------------------------------------Load promise----------------------------------------------------
                    /// Is used to store current load promise
                    vm.loadPromise = null;
                    //---------------------------------------------------------------------------------------------------------
                    //----------------------------------------Template's modes-------------------------------------------------
                    /// Is used to get bool result for conmaring template's mode with standart ones
                    vm.templateModeState = {
                        templateMode: vm.templateMode,
                        isInfo: function () {
                            return vm.templateModeState.templateMode &&
                                (vm.templateModeState.templateMode == constSvc.formModes.info);
                        },
                        isCreate: function () {
                            return vm.templateModeState.templateMode &&
                                (vm.templateModeState.templateMode == constSvc.formModes.create);
                        },
                        isUpdate: function () {
                            return vm.templateModeState.templateMode &&
                                (vm.templateModeState.templateMode == constSvc.formModes.update);
                        },
                    };
                    //---------------------------------------------------------------------------------------------------------
                    //----------------------------------Template's actions service---------------------------------------------
                    /// Is used to allow actions for conditionBuilder
                    vm.conditionPermissionsOnActions = {
                        canReloadConditions: function () {
                            return vm.templateModeState.isUpdate() && vm.gameTaskId && vm.gameTaskId > 0;
                        },
                        canAddConditionOnTop: function () {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canAddConditionOnBottom: function () {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canDeleteCondition: function (entity) {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canDeleteAllConditions: function () {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canMoveConditionToTop: function (entity) {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canMoveConditionToBottom: function (entity) {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canMoveConditionUp: function (entity) {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canMoveConditionDown: function (entity) {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canMinimize: function (entity) {
                            return entity && !entity.isMinimized;
                        },
                        canMaximize: function (entity) {
                            return entity && entity.isMinimized;
                        },
                    };
                    //---------------------------------------------------------------------------------------------------------
                    //---------------------------------------Game task actions-------------------------------------------------
                    /// Is used to store actions can be allowed in conditionBuilder
                    vm.conditionActions = {
                        reloadConditions: function () {
                            vm.loadPromise = conditionSvc.retrieveConditionsForGameTask({
                                GameTaskId: vm.gameTaskId
                            }).success(function (data) {
                                vm.conditions = data.conditions;
                                vm.conditionActions.setConditionsOrders();
                            });
                            return vm.loadPromise;
                        },
                        setConditionsOrders: function () {
                            angular.forEach(vm.conditions, function (value, key) {
                                value.order = key + 1;
                            });
                            return vm.conditions;
                        },
                        getNewCondition: function (currentGameTaskId) {
                            var emptyCondition = {
                                gameTaskId: currentGameTaskId,
                                conditionTypeId: null,
                                name: null,
                                description: null,
                                valueToPass: null,
                                order: null,
                            };
                            return emptyCondition;
                        },
                        addConditionOnTop: function () {
                            var newCondition = vm.conditionActions.getNewCondition(vm.gameTaskId);
                            vm.conditions.unshift(newCondition);
                            vm.conditionActions.setConditionsOrders();
                            return vm.conditions;
                        },
                        addConditionOnBottom: function () {
                            var newCondition = vm.conditionActions.getNewCondition(vm.gameTaskId);
                            newCondition.order = vm.conditions.length + 1;
                            vm.conditions.push(newCondition);
                            return vm.conditions;
                        },
                        deleteCondition: function (order) {
                            var index = order - 1;
                            if (!(index > -1 && index < vm.conditions.length))
                                return false;

                            vm.conditions.splice(index, 1);
                            vm.conditionActions.setConditionsOrders();
                            return vm.conditions;
                        },
                        deleteAllConditions: function () {
                            vm.conditions = [];
                        },
                        moveConditionToTop: function (order) {
                            var index = order - 1;
                            var movingCondition = vm.conditions[index];
                            vm.conditions.splice(index, 1);
                            vm.conditions.unshift(movingCondition);
                            vm.conditionActions.setConditionsOrders();
                            return vm.conditions;
                        },
                        moveConditionToBottom: function (order) {
                            var index = order - 1;
                            var movingCondition = vm.conditions[index];
                            vm.conditions.splice(index, 1);
                            vm.conditions.push(movingCondition);
                            vm.conditionActions.setConditionsOrders();
                            return vm.conditions;
                        },
                        swapConditions: function (order1, order2) {
                            var index1 = order1 - 1;
                            var index2 = order2 - 1;

                            if (!(index1 != index2 &&
                                index1 > -1 && index1 < vm.conditions.length &&
                                index2 > -1 && index2 < vm.conditions.length))
                                return false;

                            var store = vm.conditions[index1];
                            vm.conditions[index1] = vm.conditions[index2];
                            vm.conditions[index2] = store;

                            vm.conditionActions.setConditionsOrders();

                            return vm.conditions;
                        },
                        moveConditionUp: function (order) {
                            var index = order - 1;

                            if (!(index > 0 && index < vm.conditions.length))
                                return false;

                            vm.conditionActions.swapConditions(index, order);
                            return vm.conditions;
                        },
                        moveConditionDown: function (order) {
                            var index = order - 1;

                            if (!(index > -1 && index < (vm.conditions.length - 1)))
                                return false;

                            vm.conditionActions.swapConditions(order, (order + 1));
                            return vm.conditions;
                        },
                        minimize: function (order) {
                            var index = order - 1;

                            if (!(index > -1 && index < vm.conditions.length))
                                return false;

                            (vm.conditions[index]).isMinimized = true;
                            return vm.conditions;
                        },
                        maximize: function (order) {
                            var index = order - 1;

                            if (!(index > -1 && index < vm.conditions.length))
                                return false;

                            (vm.conditions[index]).isMinimized = false;
                            return vm.conditions;
                        },
                        hasValueInput: function (condition) {
                            var result = false;
                            if (condition != null && condition.conditionTypeId != null) {
                                for (var i = 0; i < vm.conditionTypes.length; i++) {
                                    if (condition.conditionTypeId == vm.conditionTypes[i].value) {
                                        result = vm.conditionTypes[i].displayText === 'Condition_JustInputCode';
                                        i = vm.conditionTypes.length;
                                    }
                                }
                            }
                            return result;
                        },
                    };
                    //---------------------------------------------------------------------------------------------------------
                }
            }
        }]);
})();
