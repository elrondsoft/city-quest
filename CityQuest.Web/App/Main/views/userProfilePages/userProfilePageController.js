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
           //---------------------------------------------------------------------------------------------------------------
           //-----------------------------------------Promise store---------------------------------------------------------
           vm.promiseStore = {
               loadTeamPromise: null,
               loadInvitesToTeamPromise: null,
               loadInvitesFromOtherTeamsPromise: null,
           };
           //---------------------------------------------------------------------------------------------------------------
           //--------------------------------------------Helpers------------------------------------------------------------
           vm.helpers = {

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