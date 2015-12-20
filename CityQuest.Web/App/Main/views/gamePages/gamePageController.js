(function () {
    var controllerId = 'app.views.gamePages.gamePageController';
    angular.module('app').controller(controllerId, ['$scope', '$stateParams', '$', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.gameLight',
        function ($scope, params, $, constSvc, permissionSvc, gameLightSvc) {
            //---------------------------------------------------------------------------------------------------------
            //-----------------------------------------PreInitialize---------------------------------------------------
            var vm = this;
            /// Is used for localization
            vm.localize = constSvc.localize;
            /// Is used to store template's title
            vm.title = vm.localize("GamePage");
            /// Is used to store game's id
            vm.gameId = params.gameId;
            //---------------------------------------------------------------------------------------------------------
            //--------------------------------------------Helpers------------------------------------------------------
            vm.helpers = {

            };
            //---------------------------------------------------------------------------------------------------------
            //------------------------------------------Initialize-----------------------------------------------------
        }
    ]);
})();