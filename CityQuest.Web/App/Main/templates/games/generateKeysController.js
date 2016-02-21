(function () {
    var controllerId = 'app.templates.games.generateKeysController';
    angular.module('app').controller(controllerId, ['$scope', 'serviceData', 'clientCityQuestConstService',
        'clientPermissionService',
        function ($scope, serviceData, constSvc, permissionSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize('keyGenerationServiceTitle'); // TODO 

            vm.gameId = serviceData.entityId;


            vm.templateActions = {
                close: function () {
                    $scope.$close();
                }
            }

        }
    ]);
})();