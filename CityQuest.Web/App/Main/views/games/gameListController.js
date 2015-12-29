(function () {
    var controllerId = 'app.views.games.gameListController';
    angular.module('app').controller(controllerId, ['$scope', '$uibModal', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.game',
        function ($scope, modal, constSvc, permissionSvc, gameSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize("Games");

            //--------------------------------Helpers------------------------------------------------------------------
            //-----------------Object with functions that would be used in vm.fields-----------------------------------
            var fieldFunctions = {
                displayId: function (data) {
                    return data.record.id;
                },
                displayName: function (data) {
                    return data.record.name;
                },
                displayDescription: function (data) {
                    return data.record.description;
                },
                displayLocation: function (data) {
                    return data.record.locationName;
                },
                displayGameTasksCount: function (data) {
                    return data.record.gameTasks && data.record.gameTasks.length ? data.record.gameTasks.length : '0';
                },
                displayIsActive: function (data) {
                    return data.record.isActive ? vm.localize('IsActiveTrue') : vm.localize('IsActiveFalse');
                },
                displayLastModifierName: function (data) {
                    return data.record.lastModifierUserFullName ? data.record.lastModifierUserFullName : '';
                },
                displayLastModificationTime: function (data) {
                    return data.record.lastModificationTime ?
                        moment(data.record.lastModificationTime).format('YYYY.MM.DD HH:mm:ss') : '';
                },
                displayCreatorName: function (data) {
                    return data.record.creatorUserFullName ? data.record.creatorUserFullName : '';
                },
                displayCreationTime: function (data) {
                    return data.record.creationTime ? moment(data.record.creationTime).format('YYYY.MM.DD HH:mm:ss') : '';
                },
                displaySettings: function (data) {
                    var record = data.record;
                    var getInfoButton = function (entity) {
                        if (!permissionSvc.game.canRetrieve(entity))
                            return '';

                        var infoTittleText = vm.localize('InfoTittleText');
                        var infoButtonText = '<i class="fa fa-info"></i>'; //vm.localize('ButtonInfo');
                        var result = '<button class="btn btn-sm btn-info game-info" id="' + entity.id +
                            '" title="' + infoTittleText + '">' + infoButtonText + ' </button>';
                        return result;
                    };
                    var getEditButton = function (entity) {
                        if (!permissionSvc.game.canUpdate)
                            return '';

                        var editTittleText = vm.localize('UpdateTittleText');
                        var editButtonText = '<i class="fa fa-pencil-square-o"></i>';
                        var result = '<button class="btn btn-sm btn-primary game-edit" id="' + entity.id +
                            '" title="' + editTittleText + '">' + editButtonText + ' </button>';
                        return result;
                    };
                    var getDeleteButton = function (entity) {
                        if (!permissionSvc.game.canDelete(entity))
                            return '';

                        var deleteTittleText = vm.localize('DeleteTittleText');
                        var deleteButtonText = '<i class="fa fa-times"></i>';
                        var result = '<button class="btn btn-sm btn-danger game-delete" id="' + entity.id +
                            '" title="' + deleteTittleText + '">' + deleteButtonText + ' </button>';
                        return result;
                    };
                    var getActivateButton = function (entity) {
                        if (!permissionSvc.game.canActivate(entity))
                            return '';

                        var activateTittleText = vm.localize('ActivateTittleText');
                        var activateButtonText = '<i class="fa fa-toggle-on"></i>';
                        var result = '<button class="btn btn-sm btn-warning game-activate" id="' + entity.id +
                            '" title="' + activateTittleText + '">' + activateButtonText + ' </button>';
                        return result;
                    };
                    var getDeactivateButton = function (entity) {
                        if (!permissionSvc.game.canDeactivate(entity))
                            return '';

                        var deactivateTittleText = vm.localize('DeactivateTittleText');
                        var deactivateButtonText = '<i class="fa fa-toggle-off"></i>';
                        var result = '<button class="btn btn-sm btn-warning game-deactivate" id="' + entity.id +
                            '" title="' + deactivateTittleText + '">' + deactivateButtonText + ' </button>';
                        return result;
                    };
                    var getGameProcessManagementButton = function (entity) {
                        if (!permissionSvc.game.canManageGameProcess(entity))
                            return '';

                        var tittleText = vm.localize('ButtonGameProcessManagementTittleText');
                        var buttonText = '<i class="fa fa-cogs"></i>';
                        var result = '<button class="btn btn-sm btn-success game-process-management" id="' + entity.id +
                            '" title="' + tittleText + '">' + buttonText + ' </button>';
                        return result;
                    };
                    return getInfoButton(record) + getGameProcessManagementButton(record) + getEditButton(record) +
                        getActivateButton(record) + getDeactivateButton(record) + getDeleteButton(record);
                }
            };
            //---------------------------------------------------------------------------------------------------------
            //-----------------Object with actions (functions) for game------------------------------------------------
            var gameActions = {
                openInfoTemplate: function (event) {
                    event.preventDefault();
                    var newModalOptions = {
                        templateUrl: constSvc.viewRoutes.gameDetailsTemplate,
                        controller: constSvc.ctrlRoutes.gameDetailsCtrl,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.info,
                                    entityId: event.currentTarget.id
                                };
                                return result;
                            },
                        }
                    };
                    var modalOptions = angular.merge({}, constSvc.defaultModalOptions, newModalOptions);
                    modal.open(modalOptions);
                    return false;
                },
                openCreateTemplate: function () {
                    var newModalOptions = {
                        templateUrl: constSvc.viewRoutes.gameDetailsTemplate,
                        controller: constSvc.ctrlRoutes.gameDetailsCtrl,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.create,
                                    jTableName: 'gameTable',
                                    updateCallback: function () {
                                        vm.recordsLoaded();
                                    }
                                };
                                return result;
                            },
                        }
                    };
                    var modalOptions = angular.merge({}, constSvc.defaultModalOptions, newModalOptions);
                    modal.open(modalOptions);
                    return false;
                },
                openUpdateTemplate: function (event) {
                    event.preventDefault();
                    var newModalOptions = {
                        templateUrl: constSvc.viewRoutes.gameDetailsTemplate,
                        controller: constSvc.ctrlRoutes.gameDetailsCtrl,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.update,
                                    entityId: event.currentTarget.id,
                                    jTableName: 'gameTable',
                                    updateCallback: function () {
                                        vm.recordsLoaded();
                                    }
                                };
                                return result;
                            },
                        }
                    };
                    var modalOptions = angular.merge({}, constSvc.defaultModalOptions, newModalOptions);
                    modal.open(modalOptions);
                    return false;
                },
                deleteGame: function (event) {
                    event.preventDefault();
                    var entityId = event.currentTarget.id;
                    abp.message.confirm(abp.utils.formatString(
                                vm.localize('DeleteConfirmationMsg_Body'), '\'Game\'', entityId),
                                vm.localize('DeleteConfirmationMsg_Header'),
                        function (answer) {
                            if (answer == true) {
                                return gameSvc.delete({ EntityId: entityId })
                                    .success(function (data) {
                                        abp.message.success(abp.utils.formatString(
                                            vm.localize('DeleteSuccessMsgResult_Body'),
                                                '\'Game\'', data.deletedEntityId),
                                            vm.localize('DeleteSuccessMsgResult_Header'));
                                        constSvc.jTableActions.deleteRecord('gameTable', data.deletedEntityId);
                                    });
                            }
                        });
                },
                activateGame: function (event) {
                    event.preventDefault();
                    var entityId = event.currentTarget.id;
                    abp.message.confirm(abp.utils.formatString(
                                vm.localize('ActivateConfirmationMsg_Body'), '\'Game\'', entityId),
                                vm.localize('ActivateConfirmationMsg_Header'),
                        function (answer) {
                            if (answer == true) {
                                return gameSvc.changeActivity({ EntityId: entityId, IsActive: true })
                                    .success(function (data) {
                                        abp.message.success(abp.utils.formatString(
                                            vm.localize('ActivateSuccessMsgResult_Body'),
                                                '\'Game\'', data.entity.id),
                                            vm.localize('ActivateSuccessMsgResult_Header'));
                                        constSvc.jTableActions.updateRecord('gameTable', data.entity);
                                        vm.recordsLoaded();
                                    });
                            }
                        });
                },
                deactivateGame: function (event) {
                    event.preventDefault();
                    var entityId = event.currentTarget.id;
                    abp.message.confirm(abp.utils.formatString(
                                vm.localize('DeactivateConfirmationMsg_Body'), '\'Game\'', entityId),
                                vm.localize('DeactivateConfirmationMsg_Header'),
                        function (answer) {
                            if (answer == true) {
                                return gameSvc.changeActivity({ EntityId: entityId, IsActive: false })
                                    .success(function (data) {
                                        abp.message.success(abp.utils.formatString(
                                            vm.localize('DeactivateSuccessMsgResult_Body'),
                                                '\'Game\'', data.entity.id),
                                            vm.localize('DeactivateSuccessMsgResult_Header'));
                                        constSvc.jTableActions.updateRecord('gameTable', data.entity);
                                        vm.recordsLoaded();
                                    });
                            }
                        });
                },
                openGameProcessManagementTemplate: function (event) {
                    event.preventDefault();
                    var newModalOptions = {
                        templateUrl: constSvc.viewRoutes.gameProcessManagementTemplate,
                        controller: constSvc.ctrlRoutes.gameProcessManagementCtrl,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    gameId: event.currentTarget.id,
                                    jTableName: 'gameTable',
                                    updateCallback: function () {
                                        vm.recordsLoaded();
                                    }
                                };
                                return result;
                            },
                        }
                    };
                    var modalOptions = angular.merge({}, constSvc.defaultModalOptions, newModalOptions);
                    modal.open(modalOptions);
                    return false;
                },
            };
            //---------------------------------------------------------------------------------------------------------
            //-------------------------------Initializing jTable-------------------------------------------------------
            /// Is used to initialize jTable
            var jTableInitializer = {
                getJTableMessages: function () {
                    var messages = {
                        apply: vm.localize('Apply')
                    };
                    return messages;
                },
                getJTableActions: function () {
                    var actions = {
                        listAction: {
                            method: gameSvc.retrieveAllPagedResult,
                            parameters: {
                                IsActive: false,
                            },
                        }
                    };
                    return actions;
                },
                getJTableFilters: function () {
                    var filters = [
                        {
                            id: 'name-filter',
                            label: vm.localize('Name'),
                            type: 'input',
                            width: '150px',
                            assignedField: 'Name',
                        },
                    ];
                    return filters;
                },
                getJTableFields: function () {
                    var fields = {
                        id: {
                            key: true,
                            visibility: 'hidden',
                            title: vm.localize('Id'),
                            display: fieldFunctions.displayId
                        },
                        name: {
                            title: vm.localize('Name'),
                            display: fieldFunctions.displayName
                        },
                        description: {
                            title: vm.localize('Description'),
                            display: fieldFunctions.displayDescription
                        },
                        location: {
                            title: vm.localize('Location'),
                            display: fieldFunctions.displayLocation
                        },
                        gameTasksCount: {
                            title: vm.localize('CountOfTasks'),
                            display: fieldFunctions.displayGameTasksCount
                        },
                        lastModificationTime: {
                            visibility: 'hidden',
                            title: vm.localize('LastModificationTime'),
                            display: fieldFunctions.displayLastModificationTime
                        },
                        lastModifierName: {
                            visibility: 'hidden',
                            title: vm.localize('LastModifierName'),
                            display: fieldFunctions.displayLastModifierName
                        },
                        creationTime: {
                            visibility: 'hidden',
                            title: vm.localize('CreationTime'),
                            display: fieldFunctions.displayCreationTime
                        },
                        creatorName: {
                            visibility: 'hidden',
                            title: vm.localize('CreatorName'),
                            display: fieldFunctions.displayCreatorName
                        },
                        activity: {
                            title: vm.localize('IsActive'),
                            display: fieldFunctions.displayIsActive
                        },
                        settings: {
                            title: '',
                            display: fieldFunctions.displaySettings
                        },
                    };
                    return fields;
                },
                getJtableRecordsLoaded: function () {
                    var recordsLoaded = function (event, data) {
                        $("#gameTable .jtable").find(".btn").off("click");
                        $(".game-info").click(function (event) {
                            return gameActions.openInfoTemplate(event);
                        });
                        $(".game-edit").click(function (event) {
                            return gameActions.openUpdateTemplate(event);
                        });
                        $(".game-delete").click(function (event) {
                            return gameActions.deleteGame(event);
                        });
                        $(".game-activate").click(function (event) {
                            return gameActions.activateGame(event);
                        });
                        $(".game-deactivate").click(function (event) {
                            return gameActions.deactivateGame(event);
                        });
                        $(".game-process-management").click(function (event) {
                            return gameActions.openGameProcessManagementTemplate(event);
                        });
                    };
                    return recordsLoaded;
                },
                getJTableToolbarItems: function () {
                    var items = [];
                    var createItem = {
                        icon: '',
                        text: vm.localize('Create'),
                        click: function () {
                            return gameActions.openCreateTemplate();
                        }
                    };
                    var filterItem = {
                        icon: '',
                        text: vm.localize('Filters'),
                        click: function () {
                            $('#jtable-filter-panel').slideToggle("slow");
                        }
                    };
                    if (permissionSvc.game.canCreate()) {
                        items.push(createItem);
                    }
                    items.push(filterItem);
                    return items;
                },
                getJTableToolsbar: function () {
                    var toolbar = {
                        hoverAnimation: true,
                        hoverAnimationDuration: 60,
                        hoverAnimationEasing: undefined,
                        items: jTableInitializer.getJTableToolbarItems()
                    };
                    return toolbar;
                },
                getJTableLoaded: function () {
                    var loaded = function () {

                    };
                    return loaded;
                },
                initJTable: function () {
                    vm.paging = true;
                    vm.messages = jTableInitializer.getJTableMessages();
                    vm.actions = jTableInitializer.getJTableActions();
                    vm.filters = jTableInitializer.getJTableFilters();
                    vm.fields = jTableInitializer.getJTableFields();
                    vm.recordsLoaded = jTableInitializer.getJtableRecordsLoaded();
                    vm.toolbar = jTableInitializer.getJTableToolsbar();
                    vm.loaded = jTableInitializer.getJTableLoaded();
                }
            };

            jTableInitializer.initJTable();
        }
    ]);
})();