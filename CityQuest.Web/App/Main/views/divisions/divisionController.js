(function () {
    var controllerId = 'app.views.divisions.divisionController';
    angular.module('app').controller(controllerId, ['$scope', //'ngDialog',
        'abp.services.cityQuest.division',
        function ($scope,
            divisionSvc
            ) {
            var vm = this;
            vm.localize = function (key) {
                return abp.localization.localize(key, 'CityQuest');
            };
            vm.title = vm.localize('Divisions');
            vm.paging = true;
            vm.actions = {
                listAction: {
                    method: abp.services.cityQuest.division.retrieveAllPagedResult
                }
            };
            vm.filters = [

            ];
            vm.fields = {
                id: {
                    key: true,
                    title: vm.localize('Id')
                },
            };
            vm.recordsLoaded = function (event, data) {

            };
            vm.toolbar = {
                hoverAnimation: true,
                hoverAnimationDuration: 60,
                hoverAnimationEasing: undefined,
                items: [{
                    icon: '',
                    text: vm.localize('Create'),
                    click: function () {

                    }
                }]
            };
            vm.messages = abp.appConfig.jtableMessages;
            vm.loaded = function () { };

        }
    ]);
})();