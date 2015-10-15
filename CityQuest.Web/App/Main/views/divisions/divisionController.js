(function () {
    var controllerId = 'app.views.divisions.divisionController';
    angular.module('app').controller(controllerId, ['$scope', '$modal', 'clientCityQuestConstService', 'clientPermissionService',
        //'abp.services.cityQuest.division',
        function ($scope, modal, constSvc, permissionSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize("Divisions");
            //--------------------------------Helpers------------------------------------------------------------------
            //---------------- Object with functions that would be used in vm.fields ----------------------------------
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
                displayTeamsCount: function (data) {
                    return data.record.teamsCount;
                },
                displayIsActive: function (data) {
                    return data.record.isActive;
                },
                displayLastModifierName: function (data) {
                    return data.record.lastModifierUserFullName ? data.record.lastModifierUserFullName : '';
                },
                displayLastModificationTime: function (data) {
                    return data.record.lastModificationTime ?
                        moment(data.record.lastModificationTime).format('YYYY:MM:DD HH:mm:ss') : '';
                },
                displayCreatorName: function (data) {
                    return data.record.creatorUserFullName ? data.record.creatorUserFullName : '';
                },
                displayCreationTime: function (data) {
                    return data.record.creationTime ? moment(data.record.creationTime).format('YYYY:MM:DD HH:mm:ss') : '';
                },
                displaySettings: function (data) {
                    var record = data.record;
                    var getInfoButton = function (entity) {
                        if (!permissionSvc.division.canRetrieve(entity))
                            return '';

                        var infoTittleText = vm.localize('InfoTittleText');
                        var infoButtonText = '<i class="fa fa-info"></i>'; //vm.localize('InfoButton');
                        var result = '<button class="btn btn-sm btn-info division-info" id="' + entity.id +
                            '" title="' + infoTittleText + '">' + infoButtonText + ' </button>';
                        return result;
                    };
                    var getEditButton = function (entity) {
                        if (!permissionSvc.division.canUpdate)
                            return '';

                        var editTittleText = vm.localize('EditTittleText');
                        var editButtonText = '<i class="fa fa-pencil-square-o"></i>'; //vm.localize('EditButton');
                        var result = '<button class="btn btn-sm btn-primary division-edit" id="' + entity.id +
                            '" title="' + editTittleText + '">' + editButtonText + ' </button>';
                        return result;
                    };
                    var getDeleteButton = function (entity) {
                        if (!permissionSvc.division.canDelete)
                            return '';

                        var deleteTittleText = vm.localize('DeleteTittleText');
                        var deleteButtonText = '<i class="fa fa-times"></i>';//vm.localize('DeleteButton');
                        var result = '<button class="btn btn-sm btn-danger division-delete" id="' + entity.id +
                            '" title="' + deleteTittleText + '">' + deleteButtonText + ' </button>';
                        return result;
                    };
                    var getActivateButton = function (entity) {
                        if (!permissionSvc.division.canActivate(entity))
                            return '';

                        var activateTittleText = vm.localize('ActivateTittleText');
                        var activateButtonText = '<i class="fa fa-toggle-on"></i>'; //vm.localize('ActivateButton');
                        var result = '<button class="btn btn-sm btn-warning division-activate" id="' + entity.id +
                            '" title="' + activateTittleText + '">' + activateButtonText + ' </button>';
                        return result;
                    };
                    var getDeactivateButton = function (entity) {
                        if (!permissionSvc.division.canDeactivate(entity))
                            return '';

                        var deactivateTittleText = vm.localize('DeactivateTittleText');
                        var deactivateButtonText = '<i class="fa fa-toggle-off"></i>'; //vm.localize('DeactivateButton');
                        var result = '<button class="btn btn-sm btn-warning division-deactivate" id="' + entity.id +
                            '" title="' + deactivateTittleText + '">' + deactivateButtonText + ' </button>';
                        return result;
                    };
                    return getInfoButton(record) + getEditButton(record) + getActivateButton(record) +
                        getDeactivateButton(record) + getDeleteButton(record);
                }
            };
            //---------------- Object with actions (functions) for division -------------------------------------------
            var divisionActions = {
                openInfoTemplate: function (event) {
                    event.preventDefault();
                    modal.open({
                        templateUrl: constSvc.viewRoutes.divisionTemplate,
                        controller: constSvc.ctrlRoutes.divisionTemplateCtrl,
                        controllerAs: constSvc.controllerAsName,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.info,
                                    divisionId: event.currentTarget.id
                                };
                                return result;
                            },
                        }
                    });
                    return false;
                },
                openCreateTemplate: function () {
                    modal.open({
                        templateUrl: constSvc.viewRoutes.divisionTemplate,
                        controller: constSvc.ctrlRoutes.divisionTemplateCtrl,
                        controllerAs: constSvc.controllerAsName,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.create
                                };
                                return result;
                            },
                        }
                    });
                    return false;
                },
                openUpdateTemplate: function (event) {
                    event.preventDefault();
                    modal.open({
                        templateUrl: constSvc.viewRoutes.divisionTemplate,
                        controller: constSvc.ctrlRoutes.divisionTemplateCtrl,
                        controllerAs: constSvc.controllerAsName,
                        resolve: {
                            serviceData: function () {
                                var result = {
                                    templateMode: constSvc.formModes.update,
                                    divisionId: event.currentTarget.id
                                };
                                return result;
                            },
                        }
                    });
                    return false;
                },
                deleteDivision: function (event) { },
                activateDivision: function (event) { },
                deactivateDivision: function (event) { }
            };
            //---------------------------------------------------------------------------------------------------------
            vm.title = vm.localize('Divisions');
            vm.paging = true;
            vm.messages = { apply: vm.localize('Apply') };
            vm.actions = {
                listAction: {
                    method: abp.services.cityQuest.division.retrieveAllPagedResult,
                    methodParams: {
                        IsActive: false,
                    },
                }
            };
            vm.filters = [
                {
                    id: 'name-filter',
                    label: vm.localize('Name'),
                    type: 'input',
                    width: '150px',
                    assignedField: 'Name',
                },
            ];
            vm.fields = {
                Id: {
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
                TeamsCount: {
                    title: vm.localize('TeamsCount'),
                    display: fieldFunctions.displayTeamsCount
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
            vm.recordsLoaded = function (event, data) {
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
            // Is used to get toolbar's items
            var getToolbarItems = function () {
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
            };
            vm.toolbar = {
                hoverAnimation: true, 
                hoverAnimationDuration: 60, 
                hoverAnimationEasing: undefined, 
                items: getToolbarItems()
            };
            vm.loaded = function () { };
        }
    ]);
})();