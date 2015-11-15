(function () {
    var app = angular.module('app');
    app.directive('tipsBuilder', ['clientCityQuestConstService', 'clientPermissionService', 'abp.services.cityQuest.tip',
        function (constSvc, permissionSvc, tipSvc) {
            return {
                restrict: 'E',
                templateUrl: '/App/Main/directives/games/tips/tipsBuilderTemplate.cshtml',
                scope: {},
                bindToController: {
                    templateMode: '=',
                    gameTaskId: '=',
                    tips: '=',
                },
                controllerAs: 'tipsBuilder',
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
                    /// Is used to allow actions for tipBuilder
                    vm.tipPermissionsOnActions = {
                        canReloadTips: function () {
                            return vm.templateModeState.isUpdate() && vm.gameTaskId && vm.gameTaskId > 0;
                        },
                        canAddTipOnTop: function () {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canAddTipOnBottom: function () {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canDeleteTip: function (entity) {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canDeleteAllTips: function () {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canMoveTipToTop: function (entity) {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canMoveTipToBottom: function (entity) {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canMoveTipUp: function (entity) {
                            return vm.templateModeState.isCreate() || vm.templateModeState.isUpdate();
                        },
                        canMoveTipDown: function (entity) {
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
                    /// Is used to store actions can be allowed in tipBuilder
                    vm.tipActions = {
                        reloadTips: function () {
                            vm.loadPromise = tipSvc.retrieveTipsForGameTask({
                                GameTaskId: vm.gameTaskId
                            }).success(function (data) {
                                vm.tips = data.tips;
                                vm.tipActions.setTipsOrders();
                            });
                            return vm.loadPromise;
                        },
                        setTipsOrders: function () {
                            angular.forEach(vm.tips, function (value, key) {
                                value.order = key + 1;
                            });
                            return vm.tips;
                        },
                        getNewTip: function (currentGameTaskId) {
                            var emptyTip = {
                                gameTaskId: currentGameTaskId,
                                name: null,
                                description: null,
                                order: null,
                            };
                            return emptyTip;
                        },
                        addTipOnTop: function () {
                            var newTip = vm.tipActions.getNewTip(vm.gameTaskId);
                            vm.tips.unshift(newTip);
                            vm.tipActions.setTipsOrders();
                            return vm.tips;
                        },
                        addTipOnBottom: function () {
                            var newTip = vm.tipActions.getNewTip(vm.gameTaskId);
                            newTip.order = vm.tips.length + 1;
                            vm.tips.push(newTip);
                            return vm.tips;
                        },
                        deleteTip: function (order) {
                            var index = order - 1;
                            if (!(index > -1 && index < vm.tips.length))
                                return false;

                            vm.tips.splice(index, 1);
                            vm.tipActions.setTipsOrders();
                            return vm.tips;
                        },
                        deleteAllTips: function () {
                            vm.tips = [];
                        },
                        moveTipToTop: function (order) {
                            var index = order - 1;
                            var movingTip = vm.tips[index];
                            vm.tips.splice(index, 1);
                            vm.tips.unshift(movingTip);
                            vm.tipActions.setTipsOrders();
                            return vm.tips;
                        },
                        moveTipToBottom: function (order) {
                            var index = order - 1;
                            var movingTip = vm.tips[index];
                            vm.tips.splice(index, 1);
                            vm.tips.push(movingTip);
                            vm.tipActions.setTipsOrders();
                            return vm.tips;
                        },
                        swapTips: function (order1, order2) {
                            var index1 = order1 - 1;
                            var index2 = order2 - 1;

                            if (!(index1 != index2 &&
                                index1 > -1 && index1 < vm.tips.length &&
                                index2 > -1 && index2 < vm.tips.length))
                                return false;

                            var store = vm.tips[index1];
                            vm.tips[index1] = vm.tips[index2];
                            vm.tips[index2] = store;

                            vm.tipActions.setTipsOrders();

                            return vm.tips;
                        },
                        moveTipUp: function (order) {
                            var index = order - 1;

                            if (!(index > 0 && index < vm.tips.length))
                                return false;

                            vm.tipActions.swapTips(index, order);
                            return vm.tips;
                        },
                        moveTipDown: function (order) {
                            var index = order - 1;

                            if (!(index > -1 && index < (vm.tips.length - 1)))
                                return false;

                            vm.tipActions.swapTips(order, (order + 1));
                            return vm.tips;
                        },
                        minimize: function (order) {
                            var index = order - 1;

                            if (!(index > -1 && index < vm.tips.length))
                                return false;

                            (vm.tips[index]).isMinimized = true;
                            return vm.tips;
                        },
                        maximize: function (order) {
                            var index = order - 1;

                            if (!(index > -1 && index < vm.tips.length))
                                return false;

                            (vm.tips[index]).isMinimized = false;
                            return vm.tips;
                        },
                    };
                    //---------------------------------------------------------------------------------------------------------
                }
            }
        }]);
})();
