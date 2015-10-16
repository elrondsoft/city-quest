(function () {
    var controllerId = 'app.templates.divisions.divisionTemplateController';
    angular.module('app').controller(controllerId, ['$scope', 'serviceData', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.division',
        function ($scope, serviceData, constSvc, permissionSvc, divisionSvc) {
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize('DivisionDetails');

            var initTemplateData = function () {
                if (serviceData.entityId) {
                    return divisionSvc.retrieve({ Id: serviceData.entityId, IsActive: false })
                        .success(function (data) {
                            vm.entity = data.retrievedEntity;
                        });
                }
            };
            initTemplateData();

            //-------------------------------------------------------------------------------------------------------------
            //----------------------------------Template's actions service-------------------------------------------------
            /// Is used to allow actions for this template
            vm.templateAvailableActions = {
                createEntity: function () {
                    return true && vm.entity && serviceData.templateMode == constSvc.formModes.create;
                },
                updateEntity: function () {
                    return true && vm.entity && serviceData.templateMode == constSvc.formModes.update;
                },
                activateEntity: function () {
                    return true && vm.entity && !(!!vm.entity.isActive) &&
                        serviceData.templateMode == constSvc.formModes.update;
                },
                deactivateEntity: function () {
                    return true && vm.entity && (!!vm.entity.isActive) &&
                        serviceData.templateMode == constSvc.formModes.update;
                },
                deleteEntity: function () {
                    return true && serviceData.templateMode == constSvc.formModes.update;
                },
                saveEntity: function () {
                    return vm.templateAvailableActions.createEntity() || vm.templateAvailableActions.updateEntity();
                },
            };
            //-------------------------------------------------------------------------------------------------------------
            /// Is used to store actions can be allowed in this template
            vm.templateActions = {
                createEntity: function () {
                    if (!vm.templateAvailableActions.createEntity())
                        return false;

                    return divisionSvc.create({ Entity: vm.entity })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('CreateSuccessMsgResult_Body'), '\'Division\'', data.createdEntity.id),
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

                    return divisionSvc.update({ Entity: vm.entity })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('UpdateSuccessMsgResult_Body'), '\'Division\'', data.updatedEntity.id),
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

                    return divisionSvc.changeActivity({ EntityId: serviceData.entityId, IsActive: true })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('ActivateSuccessMsgResult_Body'), '\'Division\'', data.entity.id),
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

                    return divisionSvc.changeActivity({ EntityId: serviceData.entityId, IsActive: false })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('DeactivateSuccessMsgResult_Body'), '\'Division\'', data.entity.id),
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

                    return divisionSvc.delete({ EntityId: serviceData.entityId })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('DeleteSuccessMsgResult_Body'), '\'Division\'', data.deletedEntityId),
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
            //-------------------------------------------------------------------------------------------------------------
        }
    ]);
})();