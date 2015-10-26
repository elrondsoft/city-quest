(function () {
    'use strict';

    var onEnterAuthorizedRequired = function ($state, ngAuthSettings, authService) {
        if (!authService.authentication.isAuth)
            $state.go(ngAuthSettings.loginStateName, { returnState: this.name });
    };

    //Configuration for Angular UI routing.
    angular.module('app').config([
        '$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise('/home');
            $stateProvider
                .state('home', {
                    url: '/home',
                    templateUrl: '/App/Main/views/home/home.cshtml',
                    menu: 'Home', //Matches to name of 'Home' menu in CityQuestNavigationProvider
                    onEnter: onEnterAuthorizedRequired,
                    controller: 'app.views.home',
                    controllerAs: 'vm',
                })
                .state('login', {
                    url: '/login',
                    templateUrl: '/App/Main/views/authorization/login.cshtml',
                    controller: 'app.controllers.authorization.login',
                    controllerAs: 'login',
                    menu: 'Login',
                    params: { returnState: null }
                })
                .state('divisions', {
                    url: '/divisions',
                    templateUrl: '/App/Main/views/divisions/divisionListView.cshtml',
                    menu: 'Divisions',
                    onEnter: onEnterAuthorizedRequired,
                })
                .state('teams', {
                    url: '/teams',
                    templateUrl: '/App/Main/views/teams/teamListView.cshtml',
                    menu: 'Teams',
                    onEnter: onEnterAuthorizedRequired,
                });
        }
    ]);
})();