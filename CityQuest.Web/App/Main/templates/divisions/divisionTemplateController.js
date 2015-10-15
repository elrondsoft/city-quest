(function () {
    var controllerId = 'app.templates.divisions.divisionTemplateController';
    angular.module('app').controller(controllerId, ['$scope', 'serviceData', 'clientCityQuestConstService',
        'clientPermissionService', //'abp.services.cityQuest.division',
        function ($scope, serviceData, constSvc, permissionSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            
            vm.a = serviceData;

        }
    ]);
})();