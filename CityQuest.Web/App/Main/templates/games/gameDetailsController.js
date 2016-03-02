(function () {
    var controllerId = 'app.templates.games.gameDetailsController';
    angular.module('app').controller(controllerId, ['$scope', 'serviceData', 'clientCityQuestConstService',
        'clientPermissionService', 'abp.services.cityQuest.game', 'abp.services.cityQuest.gameTaskType',
        'abp.services.cityQuest.conditionType', 'abp.services.cityQuest.location',
        function ($scope, serviceData, constSvc, permissionSvc, gameSvc, gameTaskTypeSvc, conditionTypeSvc, locationSvc) {
            //---------------------------------------------------------------------------------------------------------
            //----------------------------------------Pre-Initializing-------------------------------------------------
            var vm = this;
            vm.localize = constSvc.localize;
            vm.title = vm.localize('GameDetails');
            vm.templateModeState = serviceData.templateMode;
            vm.gameTaskTypes = [];
            vm.conditionTypes = [];
            //---------------------------------------------------------------------------------------------------------
            //------------------------------------------Initializing---------------------------------------------------
            /// Is used to initialize Template
            var initFunctions = {
                loadRelatedEntities: function () {
                    var promises = [];
                    var promise = locationSvc.retrieveAllLocationsLikeComboBoxes({})
                    .success(function (data) {
                        vm.locations = data.items.map(function (e) {
                            return {
                                value: parseInt(e.value, 10),
                                displayText: e.displayText
                            }
                        });
                    });
                    promises.push(promise);
                    promise = gameTaskTypeSvc.retrieveAllGameTaskTypesLikeComboBoxes({
                        IsActive: true
                    }).success(function (data) {
                        vm.gameTaskTypes = constSvc.mapComboboxes(data.items);
                    });
                    promises.push(promise);
                    promise = conditionTypeSvc.retrieveAllConditionTypesLikeComboBoxes({
                        IsActive: true
                    }).success(function (data) {
                        vm.conditionTypes = constSvc.mapComboboxes(data.items);
                    });
                    promises.push(promise);
                    return promises;
                },
                loadEntity: function (entityId) {
                    return gameSvc.retrieve({ Id: entityId, IsActive: false })
                        .success(function (data) {
                            vm.entity = data.retrievedEntity;
                        });
                },
                initDefaultEntity: function () {
                    var defaultEntity = {
                        name: null,
                        locationId: null,
                        startDate: moment().format(),
                        description: null,
                        gameImageName: null,
                        isActive: true,
                        gameTasks: []
                    };
                    vm.entity = defaultEntity;
                    return vm.entity;
                },
                initTemplateData: function () {
                    if (!(serviceData && serviceData.templateMode))
                        return false;

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
            initFunctions.loadRelatedEntities();
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
                    return permissionSvc.game.canCreate(vm.entity) && vm.entity &&
                        serviceData.templateMode == constSvc.formModes.create;
                },
                updateEntity: function () {
                    return permissionSvc.game.canUpdate(vm.entity) && vm.entity &&
                        serviceData.templateMode == constSvc.formModes.update;
                },
                activateEntity: function () {
                    return permissionSvc.game.canActivate(vm.entity) && vm.entity && !(!!vm.entity.isActive) &&
                        serviceData.templateMode == constSvc.formModes.update;
                },
                deactivateEntity: function () {
                    return permissionSvc.game.canDeactivate(vm.entity) && vm.entity && (!!vm.entity.isActive) &&
                        serviceData.templateMode == constSvc.formModes.update;
                },
                deleteEntity: function () {
                    return permissionSvc.game.canDelete(vm.entity) && serviceData.templateMode == constSvc.formModes.update;
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

                    return gameSvc.create({
                        Entity: vm.entity,
                        GameImageData: vm.croppedImage
                    }).success(function (data) {
                        abp.message.success(abp.utils.formatString(
                            vm.localize('CreateSuccessMsgResult_Body'), '\'Game\'', data.createdEntity.id),
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

                    return gameSvc.update({
                        Entity: vm.entity,
                        GameImageData: vm.croppedImage
                    }).success(function (data) {
                        abp.message.success(abp.utils.formatString(
                            vm.localize('UpdateSuccessMsgResult_Body'), '\'Game\'', data.updatedEntity.id),
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

                    return gameSvc.changeActivity({ EntityId: serviceData.entityId, IsActive: true })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('ActivateSuccessMsgResult_Body'), '\'Game\'', data.entity.id),
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

                    return gameSvc.changeActivity({ EntityId: serviceData.entityId, IsActive: false })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('DeactivateSuccessMsgResult_Body'), '\'Game\'', data.entity.id),
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

                    return gameSvc.delete({ EntityId: serviceData.entityId })
                        .success(function (data) {
                            abp.message.success(abp.utils.formatString(
                                vm.localize('DeleteSuccessMsgResult_Body'), '\'Game\'', data.deletedEntityId),
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
                clearImage: function () {
                    vm.uploadedImage = '';
                    vm.croppedImage = '';
                },
                close: function () {
                    $scope.$close();
                },
            };
            //---------------------------------------------------------------------------------------------------------
            //---------------------------------------Image management--------------------------------------------------
            var initFileImageCropper = function () {
                vm.uploadedImage = '';
                vm.croppedImage = '';
                var handleFileSelect = function (evt) {
                    var file = evt.currentTarget.files[0];
                    var reader = new FileReader();
                    reader.onload = function (evt) {
                        $scope.$apply(function ($scope) {
                            vm.uploadedImage = evt.target.result;
                        });
                    };
                    reader.readAsDataURL(file);
                };
                angular.element(document.querySelector('#fileImageInput')).on('change', handleFileSelect);
            };
            // Is used to watch on nothing but will works after modal loaded once and inits image cropper
            var initImageFileCropperListener = $scope.$watch('', function (newValue, oldValue) {
                initFileImageCropper();
                initImageFileCropperListener();
            });
            //---------------------------------------------------------------------------------------------------------
        }
    ]);
})();