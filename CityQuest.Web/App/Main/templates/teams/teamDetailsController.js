(function () {
    var controllerId = 'app.templates.teams.teamDetailsController';
    angular.module('app').controller(controllerId, ['$scope', 'serviceData', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.team', 'abp.services.cityQuest.division',
        'abp.services.cityQuest.user',
        function ($scope, serviceData, constSvc, permissionSvc, teamSvc, divisionSvc, userSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize('TeamDetails');

            //---------------------------------------------------------------------------------------------------------
            //------------------------------------------Initializing---------------------------------------------------
            /// Is used to initialize Template
            var initFunctions = {
                loadDivisions: function() {
                    return divisionSvc.retrieveAllDivisionsLikeComboBoxes({ })
                        .success(function (data) {
                            vm.divisions = data.items.map(function (e) {
                                return {
                                    value: parseInt(e.value, 10),
                                    displayText: e.displayText
                                }
                            });
                        });
                },
                loadPlayers: function () {
                    return userSvc.retrieveAllUsersLikeComboBoxes({ PlayersOnly: true })
                        .success(function (data) {
                            vm.players = data.items.map(function (e) {
                                return {
                                    value: parseInt(e.value, 10),
                                    displayText: e.displayText
                                }
                            });
                        });
                },
                loadReferences: function() {
                    initFunctions.loadDivisions();
                    initFunctions.loadPlayers();
                },
                loadEntity: function (entityId) {
                    return teamSvc.retrieve({ Id: entityId, IsActive: false })
                        .success(function (data) {
                            vm.entity = data.retrievedEntity;
                        });
                },
                initDefaultEntity: function () {
                    var defaultEntity = {
                        name: null,
                        description: null,
                        slogan: null,
                        isActive: true,
                        players: [],
                        captainId: null,
                        divisionId: null,
                    };
                    vm.entity = defaultEntity;
                    return vm.entity;
                },
                initTemplateData: function () {
                    if (!(serviceData && serviceData.templateMode))
                        return false;

                    initFunctions.loadReferences();
                    switch (serviceData.templateMode) {
                        case constSvc.formModes.info:
                            if (serviceData.entityId) {
                                return initFunctions.loadEntity(serviceData.entityId);
                            } else {
                                return false;
                            }
                            break;
                        case constSvc.formModes.update:
                            if (serviceData.entityId) {
                                return initFunctions.loadEntity(serviceData.entityId);
                            } else {
                                return false;
                            }
                            break;
                        case constSvc.formModes.create:
                            return initFunctions.initDefaultEntity();
                            break;
                        default:
                            return false;
                    };
                },
            };
            initFunctions.initTemplateData();
            //----------------------------------------Template's modes-------------------------------------------------
            /// Is used to get bool result for conmaring template's mode with standart ones
            vm.templateMode = {
                isInfo: function () {
                    return serviceData && serviceData.templateMode &&
                        (serviceData.templateMode == constSvc.formModes.info);
                },
                isCreate: function () {
                    return serviceData && serviceData.templateMode &&
                        (serviceData.templateMode == constSvc.formModes.create);
                },
                isUpdate: function () {
                    return serviceData && serviceData.templateMode &&
                        (serviceData.templateMode == constSvc.formModes.update);
                }
            };
            //----------------------------------Template's actions service---------------------------------------------
            /// Is used to allow actions for this template
            vm.templateAvailableActions = {
                createEntity: function () {
                    return permissionSvc.team.canCreate(vm.entity) && vm.entity &&
                        serviceData.templateMode == constSvc.formModes.create;
                },
                updateEntity: function () {
                    return permissionSvc.team.canUpdate(vm.entity) && vm.entity &&
                        serviceData.templateMode == constSvc.formModes.update;
                },
                activateEntity: function () {
                    return permissionSvc.team.canActivate(vm.entity) && vm.entity && !(!!vm.entity.isActive) &&
                        serviceData.templateMode == constSvc.formModes.update;
                },
                deactivateEntity: function () {
                    return permissionSvc.team.canDeactivate(vm.entity) && vm.entity && (!!vm.entity.isActive) &&
                        serviceData.templateMode == constSvc.formModes.update;
                },
                deleteEntity: function () {
                    return permissionSvc.team.canDelete(vm.entity) && serviceData.templateMode == constSvc.formModes.update;
                },
                saveEntity: function () {
                    return vm.templateAvailableActions.createEntity() || vm.templateAvailableActions.updateEntity();
                },
            };
            //---------------------------------------------------------------------------------------------------------
            /// Is used to store actions can be allowed in this template
            vm.templateActions = {
                createEntity: function () {
                    if (!vm.templateAvailableActions.createEntity())
                        return false;

                    return teamSvc.create({ Entity: vm.entity })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('CreateSuccessMsgResult_Body'), '\'Team\'', data.createdEntity.id),
                                vm.localize('CreateSuccessMsgResult_Header'));
                            if (serviceData.jTableName) {
                                constSvc.jTableActions.createRecord(serviceData.jTableName, data.createdEntity);
                            }
                            if (serviceData.updateCallback) {
                                serviceData.updateCallback();
                            }
                            vm.templateActions.close();
                        });
                },
                updateEntity: function () {
                    if (!vm.templateAvailableActions.updateEntity())
                        return false;

                    return teamSvc.update({ Entity: vm.entity })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('UpdateSuccessMsgResult_Body'), '\'Team\'', data.updatedEntity.id),
                                vm.localize('UpdateSuccessMsgResult_Header'));
                            if (serviceData.jTableName) {
                                constSvc.jTableActions.updateRecord(serviceData.jTableName, data.updatedEntity);
                            }
                            if (serviceData.updateCallback) {
                                serviceData.updateCallback();
                            }
                            vm.templateActions.close();
                        });
                },
                activateEntity: function () {
                    if (!vm.templateAvailableActions.activateEntity())
                        return false;

                    return teamSvc.changeActivity({ EntityId: serviceData.entityId, IsActive: true })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('ActivateSuccessMsgResult_Body'), '\'Team\'', data.entity.id),
                                vm.localize('ActivateSuccessMsgResult_Header'));
                            if (serviceData.jTableName) {
                                constSvc.jTableActions.updateRecord(serviceData.jTableName, data.entity);
                            }
                            if (serviceData.updateCallback) {
                                serviceData.updateCallback();
                            }
                            vm.templateActions.close();
                        });
                },
                deactivateEntity: function () {
                    if (!vm.templateAvailableActions.deactivateEntity())
                        return false;

                    return teamSvc.changeActivity({ EntityId: serviceData.entityId, IsActive: false })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('DeactivateSuccessMsgResult_Body'), '\'Team\'', data.entity.id),
                                vm.localize('DeactivateSuccessMsgResult_Header'));
                            if (serviceData.jTableName) {
                                constSvc.jTableActions.updateRecord(serviceData.jTableName, data.entity);
                            }
                            if (serviceData.updateCallback) {
                                serviceData.updateCallback();
                            }
                            vm.templateActions.close();
                        });
                },
                deleteEntity: function () {
                    if (!vm.templateAvailableActions.deleteEntity())
                        return false;

                    return teamSvc.delete({ EntityId: serviceData.entityId })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('DeleteSuccessMsgResult_Body'), '\'Team\'', data.deletedEntityId),
                                vm.localize('DeleteSuccessMsgResult_Header'));
                            if (serviceData.jTableName) {
                                constSvc.jTableActions.deleteRecord(serviceData.jTableName, data.deletedEntityId);
                            }
                            if (serviceData.updateCallback) {
                                serviceData.updateCallback();
                            }
                            vm.templateActions.close();
                        });
                },
                saveEntity: function () {
                    if (!vm.templateAvailableActions.saveEntity())
                        return false;

                    if (serviceData.templateMode == constSvc.formModes.create) {
                        return vm.templateActions.createEntity();
                    } else if (serviceData.templateMode == constSvc.formModes.update) {
                        return vm.templateActions.updateEntity();
                    }
                },
                close: function () {
                    $scope.$close();
                },
            };
            //---------------------------------------------------------------------------------------------------------
        }
    ]);
})();