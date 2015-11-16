(function () {
    var controllerId = 'app.views.locations.locationListController';
    angular.module('app').controller(controllerId, ['$scope', '$modal', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.location',
        function ($scope, modal, constSvc, permissionSvc, locationSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize("Locations");

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
                displayShortName: function (data) {
                    return data.record.shortName;
                },
                displayDisplayName: function (data) {
                    return data.record.displayName;
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
                        if (!permissionSvc.location.canRetrieve(entity))
                            return '';

                        var infoTittleText = vm.localize('InfoTittleText');
                        var infoButtonText = '<i class="fa fa-info"></i>'; //vm.localize('ButtonInfo');
                        var result = '<button class="btn btn-sm btn-info location-info" id="' + entity.id +
                            '" title="' + infoTittleText + '">' + infoButtonText + ' </button>';
                        return result;
                    };
                    var getEditButton = function (entity) {
                        if (!permissionSvc.location.canUpdate)
                            return '';

                        var editTittleText = vm.localize('UpdateTittleText');
                        var editButtonText = '<i class="fa fa-pencil-square-o"></i>';
                        var result = '<button class="btn btn-sm btn-primary location-edit" id="' + entity.id +
                            '" title="' + editTittleText + '">' + editButtonText + ' </button>';
                        return result;
                    };
                    var getDeleteButton = function (entity) {
                        if (!permissionSvc.location.canDelete(entity))
                            return '';

                        var deleteTittleText = vm.localize('DeleteTittleText');
                        var deleteButtonText = '<i class="fa fa-times"></i>';
                        var result = '<button class="btn btn-sm btn-danger location-delete" id="' + entity.id +
                            '" title="' + deleteTittleText + '">' + deleteButtonText + ' </button>';
                        return result;
                    };
                    return getInfoButton(record) + getEditButton(record) + getDeleteButton(record);
                }
            };
            //-----------------Object with actions (functions) for location--------------------------------------------
            var locationActions = {
                openInfoTemplate: function (event) {
                    event.preventDefault();
                    var newModalOptions = {
                        templateUrl: constSvc.viewRoutes.locationDetailsTemplate,
                        controller: constSvc.ctrlRoutes.locationDetailsCtrl,
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
                        templateUrl: constSvc.viewRoutes.locationDetailsTemplate,
                        controller: constSvc.ctrlRoutes.locationDetailsCtrl,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.create,
                                    jTableName: 'locationTable',
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
                        templateUrl: constSvc.viewRoutes.locationDetailsTemplate,
                        controller: constSvc.ctrlRoutes.locationDetailsCtrl,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.update,
                                    entityId: event.currentTarget.id,
                                    jTableName: 'locationTable',
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
                deleteLocation: function (event) {
                    event.preventDefault();
                    var entityId = event.currentTarget.id;
                    abp.message.confirm(abp.utils.formatString(
                                vm.localize('DeleteConfirmationMsg_Body'), '\'Location\'', entityId),
                                vm.localize('DeleteConfirmationMsg_Header'),
                        function (answer) {
                            if (answer == true) {
                                return locationSvc.delete({ EntityId: entityId })
                                    .success(function (data) {
                                        abp.message.success(abp.utils.formatString(
                                            vm.localize('DeleteSuccessMsgResult_Body'),
                                                '\'Location\'', data.deletedEntityId),
                                            vm.localize('DeleteSuccessMsgResult_Header'));
                                        constSvc.jTableActions.deleteRecord('locationTable', data.deletedEntityId);
                                    });
                            }
                        });
                },
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
                            method: locationSvc.retrieveAllPagedResult,
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
                        shortName: {
                            title: vm.localize('ShortName'),
                            display: fieldFunctions.displayShortName
                        },
                        displayName: {
                            title: vm.localize('DisplayName'),
                            display: fieldFunctions.displayDisplayName
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
                        settings: {
                            title: '',
                            display: fieldFunctions.displaySettings
                        },
                    };
                    return fields;
                },
                getJtableRecordsLoaded: function () {
                    var recordsLoaded = function (event, data) {
                        $("#locationTable .jtable").find(".btn").off("click");
                        $(".location-info").click(function (event) {
                            return locationActions.openInfoTemplate(event);
                        });
                        $(".location-edit").click(function (event) {
                            return locationActions.openUpdateTemplate(event);
                        });
                        $(".location-delete").click(function (event) {
                            return locationActions.deleteLocation(event);
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
                            return locationActions.openCreateTemplate();
                        }
                    };
                    var filterItem = {
                        icon: '',
                        text: vm.localize('Filters'),
                        click: function () {
                            $('#jtable-filter-panel').slideToggle("slow");
                        }
                    };
                    if (permissionSvc.location.canCreate()) {
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