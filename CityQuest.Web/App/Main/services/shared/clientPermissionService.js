﻿/// Is used to emulate server's policy in client 
/// (this service grants permissions for common actions at client to prevent server's policy rejecting)
angular.module('app').service('clientPermissionService', function () {

    //TODO: add permission (Policy logic) for client here

    //----------------------------------------Client's permissions for Division----------------------------------------
    this.division = {
        canRetrieve: function (entity) {
            //if (!!abp.auth.grantedPermissions.CityQuestCanAll ||
            //    !!abp.auth.grantedPermissions.CityQuestCanRetrieve ||
            //    !!abp.auth.grantedPermissions.CanRetrieveDivision)
            //    return true;
            //return false;
            return true;
        },
        canCreate: function () {
            return true;
        },
        canUpdate: function (entity) {
            return true;
        },
        canDelete: function (entity) {
            return true;
        },
        canActivate: function (entity) {
            if (entity && entity.isActive == false)
                return true;
            return false;
        },
        canDeactivate: function (entity) {
            if (entity && entity.isActive == true)
                return true;
            return false;
        },
    };
    //-----------------------------------------------------------------------------------------------------------------

});