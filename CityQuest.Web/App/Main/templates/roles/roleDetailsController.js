(function () {
    var controllerId = 'app.templates.roles.roleDetailsController';
    angular.module('app').controller(controllerId, ['$scope', 'serviceData', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.role', 'abp.services.cityQuest.permission',
        function ($scope, serviceData, constSvc, permissionSvc, roleSvc, serverPermissionSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize('RoleDetails');

            //---------------------------------------------------------------------------------------------------------
            //------------------------------------------Initializing---------------------------------------------------
            /// Is used to initialize Template
            var initFunctions = {
                loadRelations: function () {
                    serverPermissionSvc.retrieveAllLikeComboBoxes({})
                        .success(function (data) {
                            vm.allPermissions = constSvc.mapComboboxes(data.items);
                        });
                },
                loadEntity: function (entityId) {
                    return roleSvc.retrieve({ Id: entityId })
                        .success(function (data) {
                            vm.entity = data.retrievedEntity;
                        });
                },
                initDefaultEntity: function () {
                    var defaultEntity = {
                        name: null,
                        displayName: null,
                        isDefault: false,
                        isStatic: false,
                        permissions: []
                    };
                    vm.entity = defaultEntity;
                    return vm.entity;
                },
                initTemplateData: function () {
                    if (!(serviceData && serviceData.templateMode))
                        return false;

                    vm.allPermissions = [];
                    initFunctions.loadRelations();
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
                    return permissionSvc.role.canCreate(vm.entity) && vm.entity &&
                        serviceData.templateMode == constSvc.formModes.create;
                },
                updateEntity: function () {
                    return permissionSvc.role.canUpdate(vm.entity) && vm.entity &&
                        serviceData.templateMode == constSvc.formModes.update;
                },
                deleteEntity: function () {
                    return permissionSvc.role.canDelete(vm.entity) && serviceData.templateMode == constSvc.formModes.update;
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

                    return roleSvc.create({ Entity: vm.entity })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('CreateSuccessMsgResult_Body'), '\'Role\'', data.createdEntity.id),
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

                    return roleSvc.update({ Entity: vm.entity })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('UpdateSuccessMsgResult_Body'), '\'Role\'', data.updatedEntity.id),
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
                deleteEntity: function () {
                    if (!vm.templateAvailableActions.deleteEntity())
                        return false;

                    return roleSvc.delete({ EntityId: serviceData.entityId })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('DeleteSuccessMsgResult_Body'), '\'Role\'', data.deletedEntityId),
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