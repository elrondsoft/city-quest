(function () {
    var controllerId = 'app.views.cities.cityListController';

    angular.module('app').controller(controllerId, ['$scope', '$modal', 'clientCityQuestConstService',
       'clientPermissionService',
       function ($scope, modal, constSvc, permissionSvc) {
           var vm = this;

           vm.test = "test!";
           

       }]);


})();