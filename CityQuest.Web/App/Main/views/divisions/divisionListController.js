(function () {
    var controllerId = 'app.views.divisions.divisionListController';
    angular.module('app').controller(controllerId, ['$scope', '$uibModal', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.division',
        function ($scope, modal, constSvc, permissionSvc, divisionSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize("Divisions");

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
                displayTeamsCount: function (data) {
                    return data.record.teamsCount;
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
                        if (!permissionSvc.division.canRetrieve(entity))
                            return '';

                        var infoTittleText = vm.localize('InfoTittleText');
                        var infoButtonText = '<i class="fa fa-info"></i>'; //vm.localize('ButtonInfo');
                        var result = '<button class="btn btn-sm btn-info division-info" id="' + entity.id +
                            '" title="' + infoTittleText + '">' + infoButtonText + ' </button>';
                        return result;
                    };
                    var getEditButton = function (entity) {
                        if (!permissionSvc.division.canUpdate)
                            return '';

                        var editTittleText = vm.localize('UpdateTittleText');
                        var editButtonText = '<i class="fa fa-pencil-square-o"></i>';
                        var result = '<button class="btn btn-sm btn-primary division-edit" id="' + entity.id +
                            '" title="' + editTittleText + '">' + editButtonText + ' </button>';
                        return result;
                    };
                    var getDeleteButton = function (entity) {
                        if (!permissionSvc.division.canDelete(entity))
                            return '';

                        var deleteTittleText = vm.localize('DeleteTittleText');
                        var deleteButtonText = '<i class="fa fa-times"></i>';
                        var result = '<button class="btn btn-sm btn-danger division-delete" id="' + entity.id +
                            '" title="' + deleteTittleText + '">' + deleteButtonText + ' </button>';
                        return result;
                    };
                    var getActivateButton = function (entity) {
                        if (!permissionSvc.division.canActivate(entity))
                            return '';

                        var activateTittleText = vm.localize('ActivateTittleText');
                        var activateButtonText = '<i class="fa fa-toggle-on"></i>'; 
                        var result = '<button class="btn btn-sm btn-warning division-activate" id="' + entity.id +
                            '" title="' + activateTittleText + '">' + activateButtonText + ' </button>';
                        return result;
                    };
                    var getDeactivateButton = function (entity) {
                        if (!permissionSvc.division.canDeactivate(entity))
                            return '';

                        var deactivateTittleText = vm.localize('DeactivateTittleText');
                        var deactivateButtonText = '<i class="fa fa-toggle-off"></i>'; 
                        var result = '<button class="btn btn-sm btn-warning division-deactivate" id="' + entity.id +
                            '" title="' + deactivateTittleText + '">' + deactivateButtonText + ' </button>';
                        return result;
                    };
                    return getInfoButton(record) + getEditButton(record) + getActivateButton(record) +
                        getDeactivateButton(record) + getDeleteButton(record);
                }
            };
            //-----------------Object with actions (functions) for division--------------------------------------------
            var divisionActions = {
                openInfoTemplate: function (event) {
                    event.preventDefault();
                    var newModalOptions = {
                        templateUrl: constSvc.viewRoutes.divisionDetailsTemplate,
                        controller: constSvc.ctrlRoutes.divisionDetailsCtrl,
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
                        templateUrl: constSvc.viewRoutes.divisionDetailsTemplate,
                        controller: constSvc.ctrlRoutes.divisionDetailsCtrl,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.create,
                                    jTableName: 'divisionTable',
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
                        templateUrl: constSvc.viewRoutes.divisionDetailsTemplate,
                        controller: constSvc.ctrlRoutes.divisionDetailsCtrl,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.update,
                                    entityId: event.currentTarget.id,
                                    jTableName: 'divisionTable',
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
                deleteDivision: function (event) {
                    event.preventDefault();
                    var entityId = event.currentTarget.id;
                    abp.message.confirm(abp.utils.formatString(
                                vm.localize('DeleteConfirmationMsg_Body'), '\'Division\'', entityId),
                                vm.localize('DeleteConfirmationMsg_Header'),
                        function (answer) {
                            if (answer == true) {
                                return divisionSvc.delete({ EntityId: entityId })
                                    .success(function (data) {
                                        abp.message.success(abp.utils.formatString(
                                            vm.localize('DeleteSuccessMsgResult_Body'),
                                                '\'Division\'', data.deletedEntityId),
                                            vm.localize('DeleteSuccessMsgResult_Header'));
                                        constSvc.jTableActions.deleteRecord('divisionTable', data.deletedEntityId);
                                    });
                            }
                        });
                },
                activateDivision: function (event) {
                    event.preventDefault();
                    var entityId = event.currentTarget.id;
                    abp.message.confirm(abp.utils.formatString(
                                vm.localize('ActivateConfirmationMsg_Body'), '\'Division\'', entityId),
                                vm.localize('ActivateConfirmationMsg_Header'),
                        function (answer) {
                            if (answer == true) {
                                return divisionSvc.changeActivity({ EntityId: entityId, IsActive: true })
                                    .success(function (data) {
                                        abp.message.success(abp.utils.formatString(
                                            vm.localize('ActivateSuccessMsgResult_Body'),
                                                '\'Division\'', data.entity.id),
                                            vm.localize('ActivateSuccessMsgResult_Header'));
                                        constSvc.jTableActions.updateRecord('divisionTable', data.entity);
                                        vm.recordsLoaded();
                                    });
                            }
                        });
                },
                deactivateDivision: function (event) {
                    event.preventDefault();
                    var entityId = event.currentTarget.id;
                    abp.message.confirm(abp.utils.formatString(
                                vm.localize('DeactivateConfirmationMsg_Body'), '\'Division\'', entityId),
                                vm.localize('DeactivateConfirmationMsg_Header'),
                        function (answer) {
                            if (answer == true) {
                                return divisionSvc.changeActivity({ EntityId: entityId, IsActive: false })
                                    .success(function (data) {
                                        abp.message.success(abp.utils.formatString(
                                            vm.localize('DeactivateSuccessMsgResult_Body'),
                                                '\'Division\'', data.entity.id),
                                            vm.localize('DeactivateSuccessMsgResult_Header'));
                                        constSvc.jTableActions.updateRecord('divisionTable', data.entity);
                                        vm.recordsLoaded();
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
                            method: divisionSvc.retrieveAllPagedResult,
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
                        teamsCount: {
                            title: vm.localize('TeamsCount'),
                            display: fieldFunctions.displayTeamsCount
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
                            //visibility: 'hidden',
                            title: vm.localize('CreationTime'),
                            display: fieldFunctions.displayCreationTime
                        },
                        creatorName: {
                            //visibility: 'hidden',
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
                        $("#divisionTable .jtable").find(".btn").off("click");
                        $(".division-info").click(function (event) {
                            return divisionActions.openInfoTemplate(event);
                        });
                        $(".division-edit").click(function (event) {
                            return divisionActions.openUpdateTemplate(event);
                        });
                        $(".division-delete").click(function (event) {
                            return divisionActions.deleteDivision(event);
                        });
                        $(".division-activate").click(function (event) {
                            return divisionActions.activateDivision(event);
                        });
                        $(".division-deactivate").click(function (event) {
                            return divisionActions.deactivateDivision(event);
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
                            return divisionActions.openCreateTemplate();
                        }
                    };
                    var filterItem = {
                        icon: '',
                        text: vm.localize('Filters'),
                        click: function () {
                            $('#jtable-filter-panel').slideToggle("slow");
                        }
                    };
                    if (permissionSvc.division.canCreate()) {
                        items.push(createItem);
                    }
                    items.push(filterItem);
                    return items;
                },
                getJTableToolsbar: function() {
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