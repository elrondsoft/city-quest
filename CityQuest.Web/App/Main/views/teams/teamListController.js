(function () {
    var controllerId = 'app.views.teams.teamListController';
    angular.module('app').controller(controllerId, ['$scope', '$modal', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.team',
        function ($scope, modal, constSvc, permissionSvc, teamSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize("Teams");

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
                    return data.record.description;;
                },
                displayCaptain: function (data) {
                    var result = '-';
                    if (data.record.captain) {
                        result = data.record.captain.userName +
                            '(' + data.record.captain.name + ' ' + data.record.captain.surname + ')';
                    } else if (data.record.captainName) {
                        result = data.record.captainName;
                    }
                    return result;
                },
                displayDivision: function (data) {
                    var result = '-';
                    if (data.record.division) {
                        result = data.record.division.name;
                    }
                    return result;
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
                        if (!permissionSvc.team.canRetrieve(entity))
                            return '';

                        var infoTittleText = vm.localize('InfoTittleText');
                        var infoButtonText = '<i class="fa fa-info"></i>'; //vm.localize('ButtonInfo');
                        var result = '<button class="btn btn-sm btn-info team-info" id="' + entity.id +
                            '" title="' + infoTittleText + '">' + infoButtonText + ' </button>';
                        return result;
                    };
                    var getEditButton = function (entity) {
                        if (!permissionSvc.team.canUpdate)
                            return '';

                        var editTittleText = vm.localize('UpdateTittleText');
                        var editButtonText = '<i class="fa fa-pencil-square-o"></i>';
                        var result = '<button class="btn btn-sm btn-primary team-edit" id="' + entity.id +
                            '" title="' + editTittleText + '">' + editButtonText + ' </button>';
                        return result;
                    };
                    var getDeleteButton = function (entity) {
                        if (!permissionSvc.team.canDelete)
                            return '';

                        var deleteTittleText = vm.localize('DeleteTittleText');
                        var deleteButtonText = '<i class="fa fa-times"></i>';
                        var result = '<button class="btn btn-sm btn-danger team-delete" id="' + entity.id +
                            '" title="' + deleteTittleText + '">' + deleteButtonText + ' </button>';
                        return result;
                    };
                    var getActivateButton = function (entity) {
                        if (!permissionSvc.team.canActivate(entity))
                            return '';

                        var activateTittleText = vm.localize('ActivateTittleText');
                        var activateButtonText = '<i class="fa fa-toggle-on"></i>';
                        var result = '<button class="btn btn-sm btn-warning team-activate" id="' + entity.id +
                            '" title="' + activateTittleText + '">' + activateButtonText + ' </button>';
                        return result;
                    };
                    var getDeactivateButton = function (entity) {
                        if (!permissionSvc.team.canDeactivate(entity))
                            return '';

                        var deactivateTittleText = vm.localize('DeactivateTittleText');
                        var deactivateButtonText = '<i class="fa fa-toggle-off"></i>';
                        var result = '<button class="btn btn-sm btn-warning team-deactivate" id="' + entity.id +
                            '" title="' + deactivateTittleText + '">' + deactivateButtonText + ' </button>';
                        return result;
                    };
                    return getInfoButton(record) + getEditButton(record) + getActivateButton(record) +
                        getDeactivateButton(record) + getDeleteButton(record);
                }
            };
            //-----------------Object with actions (functions) for team--------------------------------------------
            var teamActions = {
                openInfoTemplate: function (event) {
                    event.preventDefault();
                    var newModalOptions = {
                        templateUrl: constSvc.viewRoutes.teamDetailsTemplate,
                        controller: constSvc.ctrlRoutes.teamDetailsCtrl,
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
                        templateUrl: constSvc.viewRoutes.teamDetailsTemplate,
                        controller: constSvc.ctrlRoutes.teamDetailsCtrl,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.create,
                                    jTableName: 'teamTable',
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
                        templateUrl: constSvc.viewRoutes.teamDetailsTemplate,
                        controller: constSvc.ctrlRoutes.teamDetailsCtrl,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.update,
                                    entityId: event.currentTarget.id,
                                    jTableName: 'teamTable',
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
                deleteTeam: function (event) {
                    event.preventDefault();
                    var entityId = event.currentTarget.id;
                    abp.message.confirm(abp.utils.formatString(
                                vm.localize('DeleteConfirmationMsg_Body'), '\'Team\'', entityId),
                                vm.localize('DeleteConfirmationMsg_Header'),
                        function (answer) {
                            if (answer == true) {
                                return teamSvc.delete({ EntityId: entityId })
                                    .success(function (data) {
                                        abp.message.success(abp.utils.formatString(
                                            vm.localize('DeleteSuccessMsgResult_Body'),
                                                '\'Team\'', data.deletedEntityId),
                                            vm.localize('DeleteSuccessMsgResult_Header'));
                                        constSvc.jTableActions.deleteRecord('teamTable', data.deletedEntityId);
                                    });
                            }
                        });
                },
                activateTeam: function (event) {
                    event.preventDefault();
                    var entityId = event.currentTarget.id;
                    abp.message.confirm(abp.utils.formatString(
                                vm.localize('ActivateConfirmationMsg_Body'), '\'Team\'', entityId),
                                vm.localize('ActivateConfirmationMsg_Header'),
                        function (answer) {
                            if (answer == true) {
                                return teamSvc.changeActivity({ EntityId: entityId, IsActive: true })
                                    .success(function (data) {
                                        abp.message.success(abp.utils.formatString(
                                            vm.localize('ActivateSuccessMsgResult_Body'),
                                                '\'Team\'', data.entity.id),
                                            vm.localize('ActivateSuccessMsgResult_Header'));
                                        constSvc.jTableActions.updateRecord('teamTable', data.entity);
                                    });
                            }
                        });
                },
                deactivateTeam: function (event) {
                    event.preventDefault();
                    var entityId = event.currentTarget.id;
                    abp.message.confirm(abp.utils.formatString(
                                vm.localize('DeactivateConfirmationMsg_Body'), '\'Team\'', entityId),
                                vm.localize('DeactivateConfirmationMsg_Header'),
                        function (answer) {
                            if (answer == true) {
                                return teamSvc.changeActivity({ EntityId: entityId, IsActive: false })
                                    .success(function (data) {
                                        abp.message.success(abp.utils.formatString(
                                            vm.localize('DeactivateSuccessMsgResult_Body'),
                                                '\'Team\'', data.entity.id),
                                            vm.localize('DeactivateSuccessMsgResult_Header'));
                                        constSvc.jTableActions.updateRecord('teamTable', data.entity);
                                    });
                            }
                        });
                }
            };
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
                            method: abp.services.cityQuest.team.retrieveAllPagedResult,
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
                        Name: {
                            title: vm.localize('Name'),
                            display: fieldFunctions.displayName
                        },
                        Description: {
                            title: vm.localize('Description'),
                            display: fieldFunctions.displayDescription
                        },
                        Captain: {
                            title: vm.localize('Captain'),
                            display: fieldFunctions.displayCaptain
                        },
                        Division: {
                            title: vm.localize('Division'),
                            display: fieldFunctions.displayDivision
                        },
                        LastModificationTime: {
                            visibility: 'hidden',
                            title: vm.localize('LastModificationTime'),
                            display: fieldFunctions.displayLastModificationTime
                        },
                        LastModifierName: {
                            visibility: 'hidden',
                            title: vm.localize('LastModifierName'),
                            display: fieldFunctions.displayLastModifierName
                        },
                        CreationTime: {
                            //visibility: 'hidden',
                            title: vm.localize('CreationTime'),
                            display: fieldFunctions.displayCreationTime
                        },
                        CreatorName: {
                            //visibility: 'hidden',
                            title: vm.localize('CreatorName'),
                            display: fieldFunctions.displayCreatorName
                        },
                        Activity: {
                            title: vm.localize('IsActive'),
                            display: fieldFunctions.displayIsActive
                        },
                        Settings: {
                            title: '',
                            display: fieldFunctions.displaySettings
                        },
                    };
                    return fields;
                },
                getJtableRecordsLoaded: function () {
                    var recordsLoaded = function (event, data) {
                        $("#teamTable .jtable").find(".btn").off("click");
                        $(".team-info").click(function (event) {
                            return teamActions.openInfoTemplate(event);
                        });
                        $(".team-edit").click(function (event) {
                            return teamActions.openUpdateTemplate(event);
                        });
                        $(".team-delete").click(function (event) {
                            return teamActions.deleteTeam(event);
                        });
                        $(".team-activate").click(function (event) {
                            return teamActions.activateTeam(event);
                        });
                        $(".team-deactivate").click(function (event) {
                            return teamActions.deactivateTeam(event);
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
                            return teamActions.openCreateTemplate();
                        }
                    };
                    var filterItem = {
                        icon: '',
                        text: vm.localize('Filters'),
                        click: function () {
                            $('#jtable-filter-panel').slideToggle("slow");
                        }
                    };
                    if (permissionSvc.team.canCreate()) {
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