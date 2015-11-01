(function () {
    var controllerId = 'app.views.users.userListController';
    angular.module('app').controller(controllerId, ['$scope', '$modal', 'clientCityQuestConstService',
       'clientPermissionService', 'abp.services.cityQuest.user',
       function ($scope, modal, constSvc, permissionSvc, userSvc) {
           var vm = this;
           vm.localize = constSvc.localize;
           vm.title = vm.localize("Users");

           //--------------------------------Helpers------------------------------------------------------------------
           //-----------------Object with functions that would be used in vm.fields-----------------------------------
           var fieldFunctions = {
               displayId: function (data) {
                   return data.record.id;
               },
               displayName: function (data) {
                   return data.record.name;
               },
               displaySurname: function (data) {
                   return data.record.surname;
               },
               displayUserName: function (data) {
                   return data.record.userName;
               },
               displayEmail: function (data) {
                   return data.record.emailAddress;
               },
               displayRoles: function (data) {
                   var result = '';
                   if (data.record.roles && data.record.roles.length > 0) {
                       angular.forEach(data.record.roles,function (value, key) {
                           result += value.displayName;
                           result = key < (data.record.roles.length - 1) ? (result + ', ') : result;
                       });
                   }
                   return result;
               },
               displayLastModifierName: function (data) {
                   return data.record.lastModifierUserFullName ? data.record.lastModifierUserFullName : '';
               },
               displayLastModificationTime: function (data) {
                   return data.record.lastModificationTime ?
                       moment(data.record.lastModificationTime).format('YYYY.MM.DD HH:mm:ss') : '';
               },
               displayCreationTime: function (data) {
                   return data.record.creationTime ? moment(data.record.creationTime).format('YYYY.MM.DD HH:mm:ss') : '';
               },
               displaySettings: function (data) {
                   var record = data.record;
                   var getInfoButton = function (entity) {
                       if (!permissionSvc.user.canRetrieve(entity))
                           return '';

                       var infoTittleText = vm.localize('InfoTittleText');
                       var infoButtonText = '<i class="fa fa-info"></i>'; //vm.localize('ButtonInfo');
                       var result = '<button class="btn btn-sm btn-info user-info" id="' + entity.id +
                           '" title="' + infoTittleText + '">' + infoButtonText + ' </button>';
                       return result;
                   };
                   var getEditButton = function (entity) {
                       if (!permissionSvc.user.canUpdate)
                           return '';

                       var editTittleText = vm.localize('UpdateTittleText');
                       var editButtonText = '<i class="fa fa-pencil-square-o"></i>';
                       var result = '<button class="btn btn-sm btn-primary user-edit" id="' + entity.id +
                           '" title="' + editTittleText + '">' + editButtonText + ' </button>';
                       return result;
                   };
                   var getDeleteButton = function (entity) {
                       if (!permissionSvc.user.canDelete(entity))
                           return '';

                       var deleteTittleText = vm.localize('DeleteTittleText');
                       var deleteButtonText = '<i class="fa fa-times"></i>';
                       var result = '<button class="btn btn-sm btn-danger user-delete" id="' + entity.id +
                           '" title="' + deleteTittleText + '">' + deleteButtonText + ' </button>';
                       return result;
                   };
                   return getInfoButton(record) + getEditButton(record) + getDeleteButton(record);
               }
           };
           //-----------------Object with actions (functions) for user--------------------------------------------
           var userActions = {
               openInfoTemplate: function (event) {
                   event.preventDefault();
                   var newModalOptions = {
                       templateUrl: constSvc.viewRoutes.userDetailsTemplate,
                       controller: constSvc.ctrlRoutes.userDetailsCtrl,
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
                       templateUrl: constSvc.viewRoutes.userDetailsTemplate,
                       controller: constSvc.ctrlRoutes.userDetailsCtrl,
                       resolve: {
                           serviceData: function () {
                               var result = {
                                   templateMode: constSvc.formModes.create,
                                   jTableName: 'userTable',
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
                       templateUrl: constSvc.viewRoutes.userDetailsTemplate,
                       controller: constSvc.ctrlRoutes.userDetailsCtrl,
                       resolve: {
                           serviceData: function () {
                               var result = {
                                   templateMode: constSvc.formModes.update,
                                   entityId: event.currentTarget.id,
                                   jTableName: 'userTable',
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
               deleteUser: function (event) {
                   event.preventDefault();
                   var entityId = event.currentTarget.id;
                   abp.message.confirm(abp.utils.formatString(
                               vm.localize('DeleteConfirmationMsg_Body'), '\'User\'', entityId),
                               vm.localize('DeleteConfirmationMsg_Header'),
                       function (answer) {
                           if (answer == true) {
                               return userSvc.delete({ EntityId: entityId })
                                   .success(function (data) {
                                       abp.message.success(abp.utils.formatString(
                                           vm.localize('DeleteSuccessMsgResult_Body'),
                                               '\'User\'', data.deletedEntityId),
                                           vm.localize('DeleteSuccessMsgResult_Header'));
                                       constSvc.jTableActions.deleteRecord('userTable', data.deletedEntityId);
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
                           method: abp.services.cityQuest.user.retrieveAllPagedResult,
                           parameters: {
                           },
                       }
                   };
                   return actions;
               },
               getJTableFilters: function () {
                   var filters = [
                       {
                           id: 'user-name-filter',
                           label: vm.localize('UserName'),
                           type: 'input',
                           width: '150px',
                           assignedField: 'UserName',
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
                       surname: {
                           title: vm.localize('Surname'),
                           display: fieldFunctions.displaySurname
                       },
                       username: {
                           title: vm.localize('UserName'),
                           display: fieldFunctions.displayUserName
                       },
                       email: {
                           title: vm.localize('Email'),
                           display: fieldFunctions.displayEmail
                       },
                       roles: {
                           title: vm.localize('Roles'),
                           display: fieldFunctions.displayRoles
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
                       Settings: {
                           title: '',
                           display: fieldFunctions.displaySettings
                       },
                   };
                   return fields;
               },
               getJtableRecordsLoaded: function () {
                   var recordsLoaded = function (event, data) {
                       $("#userTable .jtable").find(".btn").off("click");
                       $(".user-info").click(function (event) {
                           return userActions.openInfoTemplate(event);
                       });
                       $(".user-edit").click(function (event) {
                           return userActions.openUpdateTemplate(event);
                       });
                       $(".user-delete").click(function (event) {
                           return userActions.deleteUser(event);
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
                           return userActions.openCreateTemplate();
                       }
                   };
                   var filterItem = {
                       icon: '',
                       text: vm.localize('Filters'),
                       click: function () {
                           $('#jtable-filter-panel').slideToggle("slow");
                       }
                   };
                   if (permissionSvc.user.canCreate()) {
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