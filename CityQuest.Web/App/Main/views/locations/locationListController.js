(function () {
    var controllerId = 'app.views.locations.locationListController';

    angular.module('app').controller(controllerId, ['$scope', '$modal', 'clientCityQuestConstService',
       'clientPermissionService',
       function ($scope, modal, constSvc, permissionSvc) {
           var vm = this;

           vm.test = "test!";
           

       }]);


})();