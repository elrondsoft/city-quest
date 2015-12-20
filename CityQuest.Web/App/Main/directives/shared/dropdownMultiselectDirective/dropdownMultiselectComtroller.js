(function () {
    var app = angular.module('app');
    app.directive('dropdownMultiselect', [
        function () {
            return {
                restrict: 'E',
                templateUrl: '/App/Main/directives/shared/dropdownMultiselectDirective/dropdownMultiselectTemplate.cshtml',
                scope: {},
                bindToController: {
                    localize: '=',
                    items: '=',
                    itemIconClass: '=',
                    checkedItems: '=',
                    titleText: '=',
                    checkAllText: '=',
                    unCheckAllText: '=',
                    checkAllIconClass: '=',
                    unCheckAllIconClass: '=',
                },
                controllerAs: 'ddMultiselect',
                controller: function ($scope) {
                    var vm = this;

                    //------------------------------------------------------------------------------------------------------
                    //----------------------------------Preinitialize-------------------------------------------------------
                    vm.open = false;
                    vm.localize = vm.localize || function (key) { return key; };
                    vm.items = vm.items && vm.items.length ? vm.items : [];
                    vm.checkedItems = vm.checkedItems && vm.checkedItems.length ? vm.checkedItems : [];
                    vm.titleText = vm.titleText || 'ddMultiselectTitle';
                    vm.checkAllText = vm.checkAllText || 'CheckAll';
                    vm.unCheckAllText = vm.unCheckAllText || 'UnCheckAll';
                    vm.checkAllIconClass = vm.checkAllIconClass || 'fa fa-check';
                    vm.unCheckAllIconClass = vm.unCheckAllIconClass || 'fa fa-times';
                    //------------------------------------------------------------------------------------------------------
                    //-------------------------------------Actions----------------------------------------------------------
                    vm.actions = {
                        fillCheckedItems: function () {
                            vm.checkedItems.splice(0, vm.checkedItems.length);
                            for (var i = 0; i < vm.items.length; i++) {
                                if (vm.items[i].isSelected) {
                                    vm.checkedItems.push(vm.items[i]);
                                }
                            }
                            return vm.checkedItems;
                        },
                        checkAll: function () {
                            for (var i = 0; i < vm.items.length; i++) {
                                vm.items[i].isChecked = true;
                            }
                            vm.actions.fillCheckedItems();
                            return vm.items;
                        },
                        unCheckAll: function () {
                            for (var i = 0; i < vm.items.length; i++) {
                                vm.items[i].isChecked = false;
                            }
                            vm.checkedItems.splice(0, vm.checkedItems.length);
                            return vm.items;
                        },
                        changeCheckForItem: function (item) {
                            if (item) {
                                item.isChecked = !item.isChecked;
                            }
                            vm.actions.fillCheckedItems();
                            return false;
                        },
                    };
                    //------------------------------------------------------------------------------------------------------
                    //------------------------------------Initialize--------------------------------------------------------
                }
            }
        }]);
})();