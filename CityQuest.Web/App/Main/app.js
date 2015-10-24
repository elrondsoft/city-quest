(function () {
    'use strict';

    var app = angular.module('app', [
        'ngAnimate',
        'ngSanitize',

        'ui.router',
        'ui.bootstrap',
        'ui.jq',

        'abp'//,
        //'ngDialog'
    ]);

    //Configuration for Angular UI routing.
    app.config([
        '$stateProvider', '$urlRouterProvider',
        function($stateProvider, $urlRouterProvider) {
            $urlRouterProvider.otherwise('/');
            $stateProvider
                .state('home', {
                    url: '/',
                    templateUrl: '/App/Main/views/home/home.cshtml',
                    menu: 'Home' //Matches to name of 'Home' menu in CityQuestNavigationProvider
                })
                .state('divisions', {
                    url: '/divisions',
                    templateUrl: '/App/Main/views/divisions/divisionListView.cshtml',
                    menu: 'Divisions',
                })
                .state('teams', {
                    url: '/teams',
                    templateUrl: '/App/Main/views/teams/teamListView.cshtml',
                    menu: 'Teams',
                })
                .state('cities', {
                    url: '/cities',
                    templateUrl: '/App/Main/views/cities/cityListView.cshtml',
                    menu: 'Cities',
                });
        }
    ]);
})();