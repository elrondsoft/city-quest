/// Is used to get all Application constants/enums/primitive actions/routes by one service
angular.module('app').service('clientCityQuestConstService', function () {
        //----------------------------------------Global constants-----------------------------------------------------
        /// Is used for localization
        this.localize = function (key) {
            return abp.localization.localize(key, 'CityQuest');
        };
        /// Is used to store form modes  
        this.formModes = {
            create: 'create',
            update: 'update',
            info: 'info'
        };
        /// Is used like default controllerAs name
        this.controllerAsName = 'vm';
        //-------------------------------------------------------------------------------------------------------------
        //----------------------------------------Constants for Controllers--------------------------------------------
        /// Is used to store Angular's controllers's ids for CityQuest  
        this.ctrlRoutes = {
            divisionViewCtrl: 'app.views.divisions.divisionController',
            divisionTemplateCtrl: 'app.templates.divisions.divisionTemplateController',
        };
        //-------------------------------------------------------------------------------------------------------------
        //----------------------------------------Constants for Templates/Views----------------------------------------
        /// Is used to store Angular's Templates/Views routes for CityQuest  
        this.viewRoutes = {
            divisionView: '/App/Main/views/divisions/divisionView.cshtml',
            divisionTemplate: '/App/Main/templates/divisions/divisionTemplate.cshtml',
        };
        //-------------------------------------------------------------------------------------------------------------
});
