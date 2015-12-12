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
                .state('gameCollection', {
                    url: '/gameCollection',
                    templateUrl: '/App/Main/views/gameCollections/gameCollectionView.cshtml',
                    menu: 'GameCollection', 
                    onEnter: onEnterAuthorizedRequired,
                    controller: 'app.views.gameCollections.gameCollectionController',
                    controllerAs: 'vm',
                })
                .state('users', {
                    url: '/users',
                    templateUrl: '/App/Main/views/users/userListView.cshtml',
                    menu: 'UsersMenuItem',
                    onEnter: onEnterAuthorizedRequired,
                })
                .state('roles', {
                    url: '/roles',
                    templateUrl: '/App/Main/views/roles/roleListView.cshtml',
                    menu: 'RolesMenuItem',
                    onEnter: onEnterAuthorizedRequired,
                })
                .state('divisions', {
                    url: '/divisions',
                    templateUrl: '/App/Main/views/divisions/divisionListView.cshtml',
                    menu: 'DivisionsMenuItem',
                    onEnter: onEnterAuthorizedRequired,
                })
                .state('teams', {
                    url: '/teams',
                    templateUrl: '/App/Main/views/teams/teamListView.cshtml',
                    menu: 'TeamsMenuItem',
                    onEnter: onEnterAuthorizedRequired,
                })
                .state('games', {
                    url: '/games',
                    templateUrl: '/App/Main/views/games/gameListView.cshtml',
                    menu: 'GamesMenuItem',
                    onEnter: onEnterAuthorizedRequired,
                })
                .state('locations', {
                    url: '/locations',
                    templateUrl: '/App/Main/views/locations/locationListView.cshtml',
                    menu: 'LocationsMenuItem',
                    onEnter: onEnterAuthorizedRequired,
                });
        }
    ]);
})();