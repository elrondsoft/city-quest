/// Is used to get all Application constants/enums/primitive actions/routes by one service
angular.module('app').service('clientCityQuestConstService', function () {
    //----------------------------------------Global constants-----------------------------------------------------
    /// Is used for localization
    this.localize = function (key) {
        return abp.localization.localize(key, 'CityQuest');
    };
    /// Is used to map data from server Comboboxes to client Comboboxes
    this.mapComboboxes = function (items) {
        var result = items.map(function (e) {
            return {
                value: parseInt(e.value, 10), displayText: e.displayText
            }
        });
        return result;
    }
    /// Is used to store form modes  
    this.formModes = {
        create: 'create',
        update: 'update',
        info: 'info'
    };
    /// Is used like default controllerAs name
    this.controllerAsName = 'vm';
    /// Is used like default options for modal view (bootstrap)
    this.defaultModalOptions = {
        animation: true,
        backdrop: true, //use 'static' to disable closing modal view after clicking on backdrop
        keyboard: true,
        backdropClass: '',
        windowClass: '',
        windowTopClass: '',
        size: 'lg',
        openedClass: 'modal-open',
        templateUrl: null,
        controller: null,
        controllerAs: this.controllerAsName,
        resolve: {
            serviceData: null
        },
    };
    //----------------------------------------Features for jTable--------------------------------------------------
    /// Is used to reload jTable data
    this.loadJTable = function (jTableName) {
        $('#' + jTableName).jtable('load');
    };
    /// Is used to manipulate with jTable (use vm.recordsLoaded(); after this action)
    this.jTableActions = {
        deleteRecord: function (jTableName, recordKey) {
            $('#' + jTableName).jtable('deleteRecord',
                {
                    key: recordKey,
                    clientOnly: true
                });
        },
        updateRecord: function (jTableName, recordEntity) {
            $('#' + jTableName).jtable('updateRecord',
                {
                    record: recordEntity,
                    clientOnly: true
                });
        },
        createRecord: function (jTableName, recordEntity) {
            $('#' + jTableName).jtable('addRecord',
                {
                    record: recordEntity,
                    clientOnly: true
                });
        }
    };
    //-------------------------------------------------------------------------------------------------------------
    //----------------------------------------Constants for Controllers--------------------------------------------
    /// Is used to store Angular's controllers's ids for CityQuest  
    this.ctrlRoutes = {
        divisionViewCtrl: 'app.views.divisions.divisionListController',
        divisionDetailsCtrl: 'app.templates.divisions.divisionDetailsController',
        teamListCtrl: 'app.views.teams.teamListController',
        teamDetailsCtrl: 'app.templates.teams.teamDetailsController',
        locationListCtrl: 'app.views.locations.locationListController',
        locationDetailsCtrl: 'app.templates.locations.locationDetailsController',
        userListCtrl: 'app.views.users.userListController',
        userDetailsCtrl: 'app.templates.users.userDetailsController',
        roleListCtrl: 'app.views.roles.roleListController',
        roleDetailsCtrl: 'app.templates.roles.roleDetailsController',
        gameListCtrl: 'app.views.games.gameListController',
        gameDetailsCtrl: 'app.templates.games.gameDetailsController',
        gameProcessManagementCtrl: 'app.templates.games.gameProcessManagementController',
    };
    //-------------------------------------------------------------------------------------------------------------
    //----------------------------------------Constants for Templates/Views----------------------------------------
    /// Is used to store Angular's Templates/Views routes for CityQuest  
    this.viewRoutes = {
        divisionView: '/App/Main/views/divisions/divisionListView.cshtml',
        divisionDetailsTemplate: '/App/Main/templates/divisions/divisionDetailsTemplate.cshtml',
        teamListView: '/App/Main/views/teams/teamListView.cshtml',
        teamDetailsTemplate: '/App/Main/templates/teams/teamDetailsTemplate.cshtml',
        locationListView: '/App/Main/views/locations/locationListView.cshtml',
        locationDetailsTemplate: '/App/Main/templates/locations/locationDetailsTemplate.cshtml',
        userListView: '/App/Main/views/users/userListView.cshtml',
        userDetailsTemplate: '/App/Main/templates/users/userDetailsTemplate.cshtml',
        roleListView: '/App/Main/views/roles/roleListView.cshtml',
        roleDetailsTemplate: '/App/Main/templates/roles/roleDetailsTemplate.cshtml',
        gameListView: '/App/Main/views/games/gameListView.cshtml',
        gameDetailsTemplate: '/App/Main/templates/games/gameDetailsTemplate.cshtml',
        gameProcessManagementTemplate: '/App/Main/templates/games/gameProcessManagementTemplate.cshtml',
    };
    //-------------------------------------------------------------------------------------------------------------
    //----------------------------------------Constants for Templates/Views----------------------------------------
    /// Is used to store Angular's partial template's routes for CityQuest  
    this.cityQuestPartialTemplates = {
        gameCollectionPartialTemplates: {
            gamePicturePartialTemplate: '/App/Main/views/gameCollections/partialTemplates/gamePicturePartialTemplate.cshtml',
            gameDescriptionPartialTemplate: '/App/Main/views/gameCollections/partialTemplates/gameDescriptionPartialTemplate.cshtml',
            gameStatusPartialTemplate: '/App/Main/views/gameCollections/partialTemplates/gameStatusPartialTemplate.cshtml',
        },
    };
    //-------------------------------------------------------------------------------------------------------------
});
