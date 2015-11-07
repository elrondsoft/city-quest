(function () {
    var controllerId = 'app.views.locations.locationListController';

    angular.module('app').controller(controllerId, ['$scope', '$modal', 'clientCityQuestConstService', 'clientPermissionService', function ($scope, $modal, constSvc, permissionSvc) {
        var vm = this;
        vm.localize = constSvc.localize;
        vm.title = vm.localize("Locations");

        console.log(abp);

        var fieldFuncions = {};

        var locationActions = {};

        var jTableInitializer = {
            getJTableMessages: function () {
                var messages = {
                    apply: vm.localize('Apply')
                };
                return messages;
            },

            getJtableActions: function () {
                var actions = {
                    listAction: {
                        method: abp.services.cityQuest.team.retrieveAllPagedResult,
                        parameters: {

                        }
                    }
                };
                return actions;
            }


        };



    }]);


})();