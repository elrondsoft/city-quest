(function () {
    var controllerId = 'app.views.userTeams.userTeamController';
    angular.module('app').controller(controllerId, ['$scope', '$uibModal', 'clientCityQuestConstService',
       'clientPermissionService', 'abp.services.cityQuest.team',
       function ($scope, modal, constSvc, permissionSvc, teamSvc) {
           //---------------------------------------------------------------------------------------------------------------
           //-----------------------------------------PreInitialize---------------------------------------------------------
           var vm = this;
           vm.localize = constSvc.localize;
           vm.team = null;
           vm.invitesToTeam = [];
           vm.invitesFromOtherTeams = [];
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
               loadTeam: function () {
                   var promise = null;

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
           vm.actions.loadTeam();
           vm.actions.loadInvitesToTeam();
           vm.actions.loadInvitesFromOtherTeams();
           //---------------------------------------------------------------------------------------------------------------
       }
    ]);
})();