/// Is used to emulate server's policy in client 
/// (this service grants permissions for common actions at client to prevent server's policy rejecting)
angular.module('app').service('clientPermissionService', function () {
    //-----------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------PreInitialize-----------------------------------------------------
    //TODO: add permission (Policy logic) for client here
    var clientPermissions = {};
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Division----------------------------------------
    clientPermissions.division = {
        canRetrieve: function (entity) {
            //if (!!abp.auth.grantedPermissions.CityQuestCanAll ||
            //    !!abp.auth.grantedPermissions.CityQuestCanRetrieve ||
            //    !!abp.auth.grantedPermissions.CanRetrieveDivision)
            //    return true;
            //return false;
            return true;
        },
        canCreate: function (entity) {
            return true;
        },
        canUpdate: function (entity) {
            return true;
        },
        canDelete: function (entity) {
            return entity && (entity.isDefault == false) && true;
        },
        canActivate: function (entity) {
            if (entity && (entity.isActive == false) && (entity.isDefault == false))
                return true;
            return false;
        },
        canDeactivate: function (entity) {
            if (entity && (entity.isActive == true) && (entity.isDefault == false))
                return true;
            return false;
        },
    };
    //-----------------------------------------------------------------------------------------------------------------
    //------------------------------------------Client's permissions for Team------------------------------------------
    clientPermissions.team = {
        canRetrieve: function (entity) {
            return true;
        },
        canCreate: function (entity) {
            return true;
        },
        canUpdate: function (entity) {
            return true;
        },
        canDelete: function (entity) {
            return true;
        },
        canActivate: function (entity) {
            if (entity && (entity.isActive == false))
                return true;
            return false;
        },
        canDeactivate: function (entity) {
            if (entity && (entity.isActive == true))
                return true;
            return false;
        },
    };
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for User--------------------------------------------
    clientPermissions.user = {
        canRetrieve: function (entity) {
            return true;
        },
        canCreate: function (entity) {
            return true;
        },
        canUpdate: function (entity) {
            return true;
        },
        canDelete: function (entity) {
            return false;
        }
    };
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Role--------------------------------------------
    clientPermissions.role = {
        canRetrieve: function (entity) {
            return true;
        },
        canCreate: function (entity) {
            return true;
        },
        canUpdate: function (entity) {
            return true;
        },
        canDelete: function (entity) {
            return true;
        }
    };
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Game--------------------------------------------
    clientPermissions.game = {
        canRetrieve: function (entity) {
            return true;
        },
        canCreate: function (entity) {
            return true;
        },
        canUpdate: function (entity) {
            return true;
        },
        canDelete: function (entity) {
            return true;
        },
        canActivate: function (entity) {
            if (entity && (entity.isActive == false))
                return true;
            return false;
        },
        canDeactivate: function (entity) {
            if (entity && (entity.isActive == true))
                return true;
            return false;
        },
        canStartGameProcess: function (entity) {
            var newGameStatusName = 'GameStatus_InProgress';
            var currentRequiredStatus = 'GameStatus_Planned';
            var result = true && entity && entity.gameStatus && entity.gameStatus.name == currentRequiredStatus &&
                entity.gameStatus.nextAllowedStatuses && entity.gameStatus.nextAllowedStatuses.length;
            if (result && entity.gameStatus.nextAllowedStatuses.length > 0) {
                result = false;
                for (var i = 0; i < entity.gameStatus.nextAllowedStatuses.length; i++) {
                    result = entity.gameStatus.nextAllowedStatuses[i] == newGameStatusName ? true : result;
                    i = entity.gameStatus.nextAllowedStatuses[i] == newGameStatusName ?
                        entity.gameStatus.nextAllowedStatuses.length : i;
                }
            } else {
                result = false;
            }
            return result;
        },
        canPauseGameProcess: function (entity) {
            var newGameStatusName = 'GameStatus_Paused';
            var result = true && entity && entity.gameStatus && entity.gameStatus.nextAllowedStatuses &&
                entity.gameStatus.nextAllowedStatuses.length;
            if (result && entity.gameStatus.nextAllowedStatuses.length > 0) {
                result = false;
                for (var i = 0; i < entity.gameStatus.nextAllowedStatuses.length; i++) {
                    result = entity.gameStatus.nextAllowedStatuses[i] == newGameStatusName ? true : result;
                    i = entity.gameStatus.nextAllowedStatuses[i] == newGameStatusName ?
                        entity.gameStatus.nextAllowedStatuses.length : i;
                }
            } else {
                result = false;
            }
            return result;
        },
        canResumeGameProcess: function (entity) {
            var newGameStatusName = 'GameStatus_InProgress';
            var currentRequiredStatus = 'GameStatus_Paused';
            var result = true && entity && entity.gameStatus && entity.gameStatus.name == currentRequiredStatus &&
                entity.gameStatus.nextAllowedStatuses && entity.gameStatus.nextAllowedStatuses.length;
            if (result && entity.gameStatus.nextAllowedStatuses.length > 0) {
                result = false;
                for (var i = 0; i < entity.gameStatus.nextAllowedStatuses.length; i++) {
                    result = entity.gameStatus.nextAllowedStatuses[i] == newGameStatusName ? true : result;
                    i = entity.gameStatus.nextAllowedStatuses[i] == newGameStatusName ?
                        entity.gameStatus.nextAllowedStatuses.length : i;
                }
            } else {
                result = false;
            }
            return result;
        },
        canEndGameProcess: function (entity) {
            var newGameStatusName = 'GameStatus_Completed';
            var result = true && entity && entity.gameStatus && entity.gameStatus.nextAllowedStatuses &&
                entity.gameStatus.nextAllowedStatuses.length;
            if (result && entity.gameStatus.nextAllowedStatuses.length > 0) {
                result = false;
                for (var i = 0; i < entity.gameStatus.nextAllowedStatuses.length; i++) {
                    result = entity.gameStatus.nextAllowedStatuses[i] == newGameStatusName ? true : result;
                    i = entity.gameStatus.nextAllowedStatuses[i] == newGameStatusName ?
                        entity.gameStatus.nextAllowedStatuses.length : i;
                }
            } else {
                result = false;
            }
            return result;
        },
        canManageGameProcess: function (entity) {
            return clientPermissions.game.canStartGameProcess(entity) || clientPermissions.game.canPauseGameProcess(entity) || 
                clientPermissions.game.canResumeGameProcess(entity) || clientPermissions.game.canEndGameProcess(entity);
        },

    };
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Location----------------------------------------
    clientPermissions.location = {
        canRetrieve: function (entity) {
            return true;
        },
        canCreate: function (entity) {
            return true;
        },
        canUpdate: function (entity) {
            return true;
        },
        canDelete: function (entity) {
            return true;
        },
    };
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Key---------------------------------------------
    clientPermissions.key = {
        canGenerate: function () {
            return true;
        },
        canActivate: function () {
            return true;
        },
    };
    //-----------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------Initialize------------------------------------------------------
    angular.merge(this, clientPermissions);
    //-----------------------------------------------------------------------------------------------------------------
});