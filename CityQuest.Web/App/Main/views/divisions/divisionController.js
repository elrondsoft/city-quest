(function () {
    var controllerId = 'app.views.divisions.divisionController';
    angular.module('app').controller(controllerId, ['$scope', //'ngDialog',
        'abp.services.cityQuest.division',
        function ($scope) {
            var vm = this;
            vm.localize = function (key) {
                return abp.localization.localize(key, 'CityQuest');
            };
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
                    return data.record.lastModifierName ? data.record.lastModifierName : '';
                },
                displayLastModificationTime: function (data) {
                    return data.record.lastModificationTime ?
                        moment(data.record.lastModificationTime).format('YYYY:MM:DD HH:mm:ss') : '';
                },
                displayCreatorName: function (data) {
                    return data.record.creatorName ? data.record.creatorName : '';
                },
                displayCreationTime: function (data) {
                    return data.record.creationTime ? moment(data.record.creationTime).format('YYYY:MM:DD HH:mm:ss') : '';
                },
                displaySettings: function (data) {
                    var recordId = data.record.id;
                    var getInfoButton = function (id) {
                        var infoTittleText = vm.localize('InfoTittleText');
                        var infoButtonText = '<i class="fa fa-info"></i>'; //vm.localize('InfoButton');
                        var result = '<button class="btn btn-sm btn-info division-info" id="' + id +
                            '" title="' + infoTittleText + '">' + infoButtonText + ' </button>';
                        return result;
                    };
                    var getEditButton = function (id) {
                        var editTittleText = vm.localize('EditTittleText');
                        var editButtonText = '<i class="fa fa-pencil-square-o"></i>'; //vm.localize('EditButton');
                        var result = '<button class="btn btn-sm btn-primary division-edit" id="' + id +
                            '" title="' + editTittleText + '">' + editButtonText + ' </button>';
                        return result;
                    };
                    var getDeleteButton = function (id) {
                        var deleteTittleText = vm.localize('DeleteTittleText');
                        var deleteButtonText = '<i class="fa fa-times"></i>';//vm.localize('DeleteButton');
                        var result = '<button class="btn btn-sm btn-danger division-delete" id="' + id +
                            '" title="' + deleteTittleText + '">' + deleteButtonText + ' </button>';
                        return result;
                    };
                    var getActivateButton = function (id) {
                        var activateTittleText = vm.localize('ActivateTittleText');
                        var activateButtonText = '<i class="fa fa-toggle-on"></i>'; //vm.localize('ActivateButton');
                        var result = '<button class="btn btn-sm btn-warning division-activate" id="' + id +
                            '" title="' + activateTittleText + '">' + activateButtonText + ' </button>';
                        return result;
                    };
                    var getDeactivateButton = function (id) {
                        var deactivateTittleText = vm.localize('DeactivateTittleText');
                        var deactivateButtonText = '<i class="fa fa-toggle-off"></i>'; //vm.localize('DeactivateButton');
                        var result = '<button class="btn btn-sm btn-warning division-deactivate" id="' + id +
                            '" title="' + deactivateTittleText + '">' + deactivateButtonText + ' </button>';
                        return result;
                    };
                    //TODO: check permissions in each function
                    return getInfoButton(recordId) + getEditButton(recordId) + getActivateButton(recordId) +
                        getDeactivateButton(recordId) + getDeleteButton(recordId);
                }
            };
            //---------------------------------------------------------------------------------------------------------
            vm.title = vm.localize('Divisions');
            vm.paging = true;
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
                    event.preventDefault();
                    var divisionBodyScope = $scope.$new();
                    var formModes = {
                        create: 'create',
                        update: 'update',
                        info: 'info'
                    };
                    divisionBodyScope.mode = formModes.info;
                    divisionBodyScope.divisionId = event.currentTarget.id;
                    ngDialog.open({
                        template: '/App/Main/templates/...cshtml',
                        scope: divisionBodyScope
                    });
                    return false;
                });
            };
            vm.toolbar = {
                hoverAnimation: true,
                hoverAnimationDuration: 60,
                hoverAnimationEasing: undefined,
                items: [{
                    icon: '',
                    text: vm.localize('Filters'),
                    click: function () {
                        $('#jtable-filter-panel').slideToggle("slow");
                    }
                }]
            };
            vm.toolbar = {
                hoverAnimation: true, 
                hoverAnimationDuration: 60, 
                hoverAnimationEasing: undefined, 
                items: [
                    {
                        icon: '',
                        text: vm.localize('Create'),
                        click: function () {
                            
                        }
                    },
                    {
                        icon: '',
                        text: vm.localize('Filters'),
                        click: function () {
                            $('#jtable-filter-panel').slideToggle("slow");
                        }
                    }
                ]
            };
            vm.messages = { apply: vm.localize('Apply') };
            vm.loaded = function () { };

        }
    ]);
})();