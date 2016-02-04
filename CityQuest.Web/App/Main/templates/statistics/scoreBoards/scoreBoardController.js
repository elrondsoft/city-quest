(function () {
    var controllerId = 'app.templates.statistics.scoreBoards.scoreBoardController';
    angular.module('app').controller(controllerId, ['$scope', 'serviceData', 'clientCityQuestConstService',
        'clientPermissionService',
        function ($scope, serviceData, constSvc, permissionSvc) {
            //---------------------------------------------------------------------------------------------------------
            //-----------------------------------------PreInitializing-------------------------------------------------
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize('ScoreBoardTemplateTitle');
            vm.templateModeState = serviceData.templateMode;
            vm.gameId = serviceData.gameId;
            //---------------------------------------------------------------------------------------------------------
            //----------------------------------------Template's modes-------------------------------------------------
            /// Is used to get bool result for conmaring template's mode with standart ones
            vm.templateMode = {
                isInfo: function () {
                    return serviceData && serviceData.templateMode &&
                        (serviceData.templateMode == constSvc.formModes.info);
                },
                isCreate: function () {
                    return serviceData && serviceData.templateMode &&
                        (serviceData.templateMode == constSvc.formModes.create);
                },
                isUpdate: function () {
                    return serviceData && serviceData.templateMode &&
                        (serviceData.templateMode == constSvc.formModes.update);
                }
            };
            //---------------------------------------------------------------------------------------------------------
            //---------------------------------------------Helpers-----------------------------------------------------

            //---------------------------------------------------------------------------------------------------------
            //----------------------------------------Template actions-------------------------------------------------
            vm.templateActions = {
                close: function () {
                    $scope.$close();
                },
            };
            //---------------------------------------------------------------------------------------------------------
            //------------------------------------------Initializing---------------------------------------------------
        }
    ]);
})();