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


           
            vm.labels = ['12:00', '12:10', '12:20', '12:30', '12:40', '12:50', '13:00', '14:10', '15:10'];
            vm.series = ['Team A', 'Team B', 'Team C'];

            vm.data = [
                  [0, 1, 1, 1, 1, 2, 2, 2, 2, 3],
                  [0, 0, 1, 1, 2, 2, 2, 2, 3, 3],
                  [0, 0, 0, 1, 1, 1, 2, 3, 3, 3],
                ];

        }
    ]);
})();