(function () {
    var controllerId = 'app.views.userProfilePages.userProfilePageController';
    angular.module('app').controller(controllerId, ['$scope', '$uibModal', 'clientCityQuestConstService',
       'clientPermissionService', 'abp.services.cityQuest.user', 'abp.services.cityQuest.team',
       function ($scope, modal, constSvc, permissionSvc, userSvc, teamSvc) {
           //---------------------------------------------------------------------------------------------------------------
           //-----------------------------------------PreInitialize---------------------------------------------------------
           var vm = this;
           vm.localize = constSvc.localize;
           vm.title = 'UserProfilePageTitle';
           vm.partialTemplates = constSvc.cityQuestPartialTemplates.userProfilePartialTemplates;
           vm.userId = abp.session.userId;
           vm.user = null;
           vm.team = null;
           vm.isEditUserProfile = false;
           vm.isChangePasswordUserProfile = false;
           //---------------------------------------------------------------------------------------------------------------
           //-----------------------------------------Promise store---------------------------------------------------------
           vm.promiseStore = {
               changeUserPasswordPromise: null,
               updateUserProfile: null,
               loadTeamPromise: null,
               loadInvitesToTeamPromise: null,
               loadInvitesFromOtherTeamsPromise: null,
           };
           //---------------------------------------------------------------------------------------------------------------
           //--------------------------------------------Helpers------------------------------------------------------------
           vm.helpers = {
               changeIsEditUserProfile: function (value) {
                   vm.helpers.changeIsChangePasswordUserProfile(false);
                   if (value != null && (value === true || value === false)) {
                       vm.isEditUserProfile = value;
                   } else {
                       vm.isEditUserProfile = !vm.isEditUserProfile;
                   }
                   return vm.isEditUserProfile;
               },
               changeIsChangePasswordUserProfile: function (value) {
                   if (vm.user != null) {
                       vm.user.currentPassword = null;
                       vm.user.newPassword = null;
                       vm.user.newPasswordRepeat = null;
                   }
                   if (value != null && (value === true || value === false)) {
                       vm.isChangePasswordUserProfile = value;
                   } else {
                       vm.isChangePasswordUserProfile = !vm.isChangePasswordUserProfile;
                   }
                   return vm.isChangePasswordUserProfile;
               },
           };
           //---------------------------------------------------------------------------------------------------------------
           //--------------------------------------Permissions on actions---------------------------------------------------
           vm.permissionOnActions = {
               canLoadTeam: function () {
                   var result = true;
                   return result;
               },
           };
           //---------------------------------------------------------------------------------------------------------------
           //--------------------------------------------Actions------------------------------------------------------------
           vm.actions = {
               loadUser: function () {
                   var promise = userSvc.retrieve({
                       Id: vm.userId
                   }).success(function (data) {
                       vm.user = data.retrievedEntity;
                   }).finally(function () {
                       vm.promiseStore.loadUserPromise = null;
                   });
                   vm.promiseStore.loadUserPromise = promise;
                   return promise;
               },
               loadTeam: function () {
                   var promise = null;
                   var promise = teamSvc.retrieve({
                       UserId: vm.userId
                   }).success(function (data) {
                       vm.team = data.retrievedEntity;
                   }).finally(function () {
                       vm.promiseStore.loadTeamPromise = null;
                   });
                   vm.promiseStore.loadTeamPromise = promise;
                   return promise;
               },
               loadInvitesToTeam: function () {
                   var promise = null;

                   vm.promiseStore.loadInvitesToTeamPromise = promise;
                   return promise;
               },
               loadInvitesFromOtherTeams: function () {
                   var promise = null;

                   vm.promiseStore.loadInvitesFromOtherTeamsPromise = promise;
                   return promise;
               },
               saveUserProfileChanges: function () {
                   var promise = userSvc.updatePublicUserFields({
                       Id: vm.user.id,
                       Name: vm.user.name,
                       Surname: vm.user.surname,
                       PhoneNumber: vm.user.phoneNumber,
                       EmailAddress: vm.user.emailAddress,
                   }).success(function (data) {
                       vm.user = data.user;
                       abp.message.success(
                        vm.localize('SaveSuccessUserProfileChangesMsgResult_Body'),
                        vm.localize('SaveSuccessUserProfileChangesMsgResult_Header'));
                   }).finally(function (data) {
                       vm.helpers.changeIsEditUserProfile(false);
                       vm.promiseStore.updateUserProfile = null;
                   });
                   vm.promiseStore.updateUserProfile = promise;
                   return promise;
               },
               commitNewPassword: function () {
                   var promise = userSvc.changePassword({
                       CurrentPassword: vm.user.currentPassword,
                       NewPassword: vm.user.newPassword,
                       NewPasswordRepeat: vm.user.newPasswordRepeat,
                   }).success(function (data) {
                       abp.message.success(
                        vm.localize('PasswordChangedSuccessfullyMsgResult_Body'),
                        vm.localize('PasswordChangedSuccessfullyMsgResult_Header'));
                   }).finally(function (data) {
                       vm.helpers.changeIsChangePasswordUserProfile(false);
                       vm.promiseStore.changeUserPasswordPromise = null;
                   });
                   vm.promiseStore.changeUserPasswordPromise = promise;
                   return promise;
               },
               cancelUserProfileChanges: function () {
                   vm.helpers.changeIsEditUserProfile(false);
                   return vm.actions.loadUser();
               },
               cancelChangePassword: function () {
                   return vm.helpers.changeIsChangePasswordUserProfile(false);
               },
           };
           //---------------------------------------------------------------------------------------------------------------
           //-------------------------------------------Initialize----------------------------------------------------------
           vm.actions.loadUser();
           vm.actions.loadTeam();
           vm.actions.loadInvitesToTeam();
           vm.actions.loadInvitesFromOtherTeams();
           //---------------------------------------------------------------------------------------------------------------
       }
    ]);
})();