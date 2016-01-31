(function() {
    var controllerId = 'app.views.home';
    angular.module('app').controller(controllerId, [
        '$scope', 'clientCityQuestConstService', 'clientPermissionService', 'abp.services.cityQuest.key', 'abp.services.cityQuest.game', '$',
        function ($scope, constSvc, permissionSvc, keySvc, gameSvc, $) {
            var vm = this;
            vm.keyGeneratorService = {
                countForGenerate: 0,
                gameIdForGenerate: null,
                gamesStore: [],
                initGamesStore: function () {
                    var promise = gameSvc.retrieveAllGamesLikeComboBoxes({})
                        .success(function (data) {
                            vm.keyGeneratorService.gamesStore = data.items.map(function (e) {
                                return {
                                    value: parseInt(e.value, 10),
                                    displayText: e.displayText
                                };
                            });
                        });
                    return promise;
                },
                canGenerateKeys: function () {
                    return permissionSvc.key.canGenerate();
                },
                generateKeys: function () {
                    var promise = keySvc.generateKeysForGame({
                        Count: vm.keyGeneratorService.countForGenerate,
                        GameId: vm.keyGeneratorService.gameIdForGenerate
                    }).success(function (data) {
                        var keys = data.keys;
                    });
                    return promise;
                },
            };

            vm.keyGeneratorService.initGamesStore();
        }
    ]);
})();