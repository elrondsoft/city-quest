(function () {
    var controllerId = 'app.views.gamePages.gamePageController';
    angular.module('app').controller(controllerId, ['$scope', '$stateParams', '$', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.gameLight',
        function ($scope, params, $, constSvc, permissionSvc, gameLightSvc) {
            var vm = this;
            //---------------------------------------------------------------------------------------------------------
            //-----------------------------------------PreInitialize---------------------------------------------------
            /// Is used for localization
            vm.localize = constSvc.localize;
            /// Is used to store template's title
            vm.title = vm.localize("GameCollection");

            vm.gameId = params.gameId;

        }
    ]);
})();