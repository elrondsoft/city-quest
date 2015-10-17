﻿/// Is used to get all Application constants/enums/primitive actions/routes by one service
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
        /// Is used to manipulate with jTable (use vm.recordsLoaded(); after this action)
        this.jTableActions = {
            loadJTable: function (jTableName) {
                $('#' + jTableName).jtable('load');
            },
            deleteRecord: function (jTableName, recordKey) {
                $('#' + jTableName).jtable('deleteRecord',
                    {
                        key: recordKey,
                        clientOnly: true
                    });
                this.jTableActions.loadJTable(jTableName);
            },
            updateRecord: function (jTableName, recordEntity) {
                $('#' + jTableName).jtable('updateRecord',
                    {
                        record: recordEntity,
                        clientOnly: true
                    });
                this.jTableActions.loadJTable(jTableName);
            },
            createRecord: function (jTableName, recordEntity) {
                $('#' + jTableName).jtable('addRecord',
                    {
                        record: recordEntity,
                        clientOnly: true
                    });
                this.jTableActions.loadJTable(jTableName);
            }
        };
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