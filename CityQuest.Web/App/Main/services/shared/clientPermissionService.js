/// Is used to emulate server's policy in client 
/// (this service grants permissions for common actions at client to prevent server's policy rejecting)
angular.module('app').service('clientPermissionService', function () {
    //-----------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------PreInitialize-----------------------------------------------------
    var clientPermissions = {};
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Division----------------------------------------
    clientPermissions.division = {
        canRetrieve: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanRetrieve ||
                !!abp.auth.grantedPermissions.CanRetrieveDivision)
                return true;
            return false;
        },
        canCreate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanCreate ||
                !!abp.auth.grantedPermissions.CanCreateDivision)
                return true;
            return false;
        },
        canUpdate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanUpdate ||
                !!abp.auth.grantedPermissions.CanUpdateDivision)
                return true;
            return false;
        },
        canDelete: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanDelete ||
                !!abp.auth.grantedPermissions.CanDeleteDivision)
                return true;
            return false;
        },
        canActivate: function (entity) {
            if ((entity && (entity.isActive == false)) && 
                (!!abp.auth.grantedPermissions.CanAll || 
                !!abp.auth.grantedPermissions.CanChangeDivisionActivity))
                return true;
            return false;
        },
        canDeactivate: function (entity) {
            if ((entity && (entity.isActive == true)) &&
                (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanChangeDivisionActivity))
                return true;
            return false;
        }
    };
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Team--------------------------------------------
    clientPermissions.team = {
        canRetrieve: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanRetrieve ||
                !!abp.auth.grantedPermissions.CanRetrieveTeam)
                return true;
            return false;
        },
        canCreate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanCreate ||
                !!abp.auth.grantedPermissions.CanCreateTeam)
                return true;
            return false;
        },
        canUpdate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanUpdate ||
                !!abp.auth.grantedPermissions.CanUpdateTeam)
                return true;
            return false;
        },
        canDelete: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanDelete ||
                !!abp.auth.grantedPermissions.CanDeleteTeam)
                return true;
            return false;
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
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanRetrieve ||
                !!abp.auth.grantedPermissions.CanRetrieveUser)
                return true;
            return false;
        },
        canCreate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanCreate ||
                !!abp.auth.grantedPermissions.CanCreateUser)
                return true;
            return false;
        },
        canUpdate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanUpdate ||
                !!abp.auth.grantedPermissions.CanUpdateUser)
                return true;
            return false;
        },
        canDelete: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanDelete ||
                !!abp.auth.grantedPermissions.CanDeleteUser)
                return true;
            return false;
        },
    };
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Role--------------------------------------------
    clientPermissions.role = {
        canRetrieve: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanRetrieve ||
                !!abp.auth.grantedPermissions.CanRetrieveRole)
                return true;
            return false;
        },
        canCreate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanCreate ||
                !!abp.auth.grantedPermissions.CanCreateRole)
                return true;
            return false;
        },
        canUpdate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanUpdate ||
                !!abp.auth.grantedPermissions.CanUpdateRole)
                return true;
            return false;
        },
        canDelete: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanDelete ||
                !!abp.auth.grantedPermissions.CanDeleteRole)
                return true;
            return false;
        },
    };
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Game--------------------------------------------
    clientPermissions.game = {
        canRetrieve: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanRetrieve ||
                !!abp.auth.grantedPermissions.CanRetrieveGame ||
                !!abp.auth.grantedPermissions.CanRetrieveSameLocationGame ||
                !!abp.auth.grantedPermissions.CanRetrieveGameForActivate ||
                !!abp.auth.grantedPermissions.CanRetrieveActivatedGame)
                return true;
            return false;
        },
        canCreate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanCreate ||
                !!abp.auth.grantedPermissions.CanCreateGame)
                return true;
            return false;
        },
        canUpdate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanUpdate ||
                !!abp.auth.grantedPermissions.CanUpdateGame)
                return true;
            return false;
        },
        canDelete: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanDelete ||
                !!abp.auth.grantedPermissions.CanDeleteGame)
                return true;
            return false;
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
        canGenerateKeys: function (entity) {
            var result = true;
            return result;
        }
    };
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Location----------------------------------------
    clientPermissions.location = {
        canRetrieve: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanRetrieve ||
                !!abp.auth.grantedPermissions.CanRetrieveLocation)
                return true;
            return false;
        },
        canCreate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanCreate ||
                !!abp.auth.grantedPermissions.CanCreateLocation)
                return true;
            return false;
        },
        canUpdate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanUpdate ||
                !!abp.auth.grantedPermissions.CanUpdateLocation)
                return true;
            return false;
        },
        canDelete: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanDelete ||
                !!abp.auth.grantedPermissions.CanDeleteLocation)
                return true;
            return false;
        },
    };
    //-----------------------------------------------------------------------------------------------------------------
    //----------------------------------------Client's permissions for Key---------------------------------------------
    clientPermissions.key = {
        canGenerate: function (entity) {
            if (!!abp.auth.grantedPermissions.CanAll ||
                !!abp.auth.grantedPermissions.CanCreate ||
                !!abp.auth.grantedPermissions.CanGenerateKeysForGame)
                return true;
            return false;
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