(function () {
    var controllerId = 'app.views.userProfilePages.userProfilePageController';
    angular.module('app').controller(controllerId, ['$scope', '$uibModal', 'clientCityQuestConstService',
       'clientPermissionService', 'abp.services.cityQuest.user', 'abp.services.cityQuest.team',
       'abp.services.cityQuest.teamRequest', 'abp.services.cityQuest.user',
       function ($scope, modal, constSvc, permissionSvc, userSvc, teamSvc, teamRequestSvc, userSvc) {
           //---------------------------------------------------------------------------------------------------------------
           //-----------------------------------------PreInitialize---------------------------------------------------------
           var vm = this;
           vm.localize = constSvc.localize;
           vm.title = 'UserProfilePageTitle';
           vm.partialTemplates = constSvc.cityQuestPartialTemplates.userProfilePartialTemplates;
           vm.userId = abp.session.userId;
           vm.user = null;
           vm.team = null;
           vm.userTeamRequests = null;
           vm.teamInvites = null;
           vm.newInvitedUserId = null;
           vm.newCaptainCareerId = null;
           vm.users = null;
           vm.isEditUserProfile = false;
           vm.isChangePasswordUserProfile = false;
           //---------------------------------------------------------------------------------------------------------------
           //-----------------------------------------Promise store---------------------------------------------------------
           vm.promiseStore = {
               changeUserPasswordPromise: null,
               updateUserProfile: null,
               loadTeamPromise: null,
               loadInvitesToTeamPromise: null,
               loadUserTeamRequestsPromise: null,
               answerOnTeamRequestPromise: null,
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
               canLeaveCurrentTeam: function (currentTeamEntity) {
                   var result = currentTeamEntity != null ? !!currentTeamEntity.isDefault : false;

                   return result;
               },
               canCreateNewTeam: function (currentTeamEntity) {
                   return true;
               },
               canSeeTeamCaptainManagement: function () {
                   var result = vm.team != null && vm.team.captainUserId == vm.userId;
                  
                   return result;
               },
           };
           //---------------------------------------------------------------------------------------------------------------
           //--------------------------------------------Actions------------------------------------------------------------
           vm.actions = {
               loadUsers: function () {
                   var promise = userSvc.retrieveAllUsersLikeComboBoxes({
                       OnlyWithDefaultRole: true
                   }).success(function (data) {
                       vm.users = data.items.map(function (e) {
                           return {
                               value: parseInt(e.value, 10),
                               displayText: e.displayText
                           }
                       });
                   });
                   return promise;
               },
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
               loadUserTeamRequests: function () {
                   var promise = null;
                   var promise = teamRequestSvc.retrieveAll({
                       UserId: vm.userId
                   }).success(function (data) {
                       vm.userTeamRequests = data.retrievedEntities;
                   }).finally(function () {
                       vm.promiseStore.loadUserTeamRequestsPromise = null;
                   });
                   vm.promiseStore.loadUserTeamRequestsPromise = promise;
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
               leaveCurrentTeam: function () {
                   var promise = teamRequestSvc.leaveCurrentTeam({
                   }).success(function (data) {
                   }).finally(function (data) {
                       vm.promiseStore.leaveCurrentTeamPromise = null;
                       vm.actions.loadTeam();
                   });
                   vm.promiseStore.leaveCurrentTeamPromise = promise;
                   return promise;
               },
               createNewTeam: function () {
                   //TODO: open new dialog with team creation
               },
               answerOnTeamRequest: function (teamRequestId, answer) {
                   var promise = null;
                   var promise = teamRequestSvc.answerOnRequest({
                       TeamRequestId: teamRequestId,
                       Answer: answer
                   }).success(function (data) {
                       vm.actions.loadUser();
                       vm.actions.loadTeam();
                       vm.actions.loadInvitesToTeam();
                       vm.actions.loadUserTeamRequests();
                   }).finally(function () {
                       vm.promiseStore.answerOnTeamRequestPromise = null;
                   });
                   vm.promiseStore.answerOnTeamRequestPromise = promise;
                   return promise;
               },
               loadInvitesToTeam: function () {
                   var promise = teamRequestSvc.retrieveAll({
                       CaptainId: vm.userId
                   }).success(function (data) {
                       vm.teamInvites = data.retrievedEntities;
                   }).finally(function () {
                       vm.promiseStore.loadInvitesToTeamPromise = null;
                   });

                   vm.promiseStore.loadInvitesToTeamPromise = promise;
                   return promise;
               },
               commitNewCaptain: function () {
                   var promise = teamSvc.changeCaptain({
                       TeamId: vm.team.id,
                       NewCaptainCareerId: vm.newCaptainCareerId,
                   }).success(function (data) {
                       vm.actions.loadUser();
                       vm.actions.loadTeam();
                       vm.actions.loadInvitesToTeam();
                       vm.actions.loadUserTeamRequests();
                   }).finally(function () {
                       vm.newCaptainCareerId = null;
                   });

                   return promise;
               },
               sendTeamInvite: function () {
                   var promise = teamRequestSvc.create({
                       InvitedUserId: vm.newInvitedUserId,
                   }).success(function (data) {
                       vm.actions.loadInvitesToTeam();
                   }).finally(function () {
                       vm.newInvitedUserId = null;
                   });

                   return promise;
               },
               denyTeamInvite: function (teamInviteId) {
                   var promise = teamRequestSvc.denyRequest({
                       TeamRequestId: teamInviteId,
                   }).success(function (data) {
                       vm.actions.loadInvitesToTeam();
                   }).finally(function () {

                   });

                   return promise;
               },
           };
           //---------------------------------------------------------------------------------------------------------------
           //-------------------------------------------Initialize----------------------------------------------------------
           vm.actions.loadUsers();
           vm.actions.loadUser();
           vm.actions.loadTeam();
           vm.actions.loadInvitesToTeam();
           vm.actions.loadUserTeamRequests();
           //---------------------------------------------------------------------------------------------------------------
       }
    ]);
})();