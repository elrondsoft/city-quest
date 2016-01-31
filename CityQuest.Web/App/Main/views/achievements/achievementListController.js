(function () {
    var controllerId = "app.views.achievements.achievementListController";

    angular.module('app').controller(controllerId, ['$scope', '$uibModal', 'clientCityQuestConstService', 'clientPermissionService', 'abp.services.cityQuest.achievement', function ($scope, modal, constSvc, permissionSvc, achievementSvc) {

        console.log(achievementSvc);
    }]);


})();