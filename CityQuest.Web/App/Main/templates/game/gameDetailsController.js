(function () {
    var controllerId = 'app.templates.games.gameDetailsController';
    angular.module('app').controller(controllerId, ['$scope', 'serviceData', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.game',
        function ($scope, serviceData, constSvc, permissionSvc, gameSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize('GameDetails');

        }
    ]);
})();