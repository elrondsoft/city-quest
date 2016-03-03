using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestConstants
{
    public class CityQuestPermissionNames
    {
        #region CityQuest's permission names

        //public const string CanDoSomething = "CanDoSomething";

        #region CityQuest's menu visibility permissions

        //public const string CanViewMenu[MenuItemName] = "CanViewMenu[MenuItemName]";

        public const string CanViewMenuHome = "CanViewMenuHome";
        public const string CanViewMenuGameCollection = "CanViewMenuGameCollection";
        public const string CanViewMenuAdministrationMenuItem = "CanViewMenuAdministrationMenuItem";
        public const string CanViewMenuUsersMenuItem = "CanViewMenuUsersMenuItem";
        public const string CanViewMenuRolesMenuItem = "CanViewMenuRolesMenuItem";
        public const string CanViewMenuLocationsMenuItem = "CanViewMenuLocationsMenuItem";
        public const string CanViewMenuDivisionsMenuItem = "CanViewMenuDivisionsMenuItem";
        public const string CanViewMenuTeamsMenuItem = "CanViewMenuTeamsMenuItem";
        public const string CanViewMenuGamesMenuItem = "CanViewMenuGamesMenuItem";

        #endregion

        #region Global CityQuest's permission names

        public const string CanAll = "CanAll";
        public const string CanRetrieve = "CanRetrieve";
        public const string CanCreate = "CanCreate";
        public const string CanUpdate = "CanUpdate";
        public const string CanDelete = "CanDelete";

        #endregion

        #region GameModule's CityQuest's permission names

        public const string CanAllDivision = "CanAllDivision";
        public const string CanRetrieveDivision = "CanRetrieveDivision";
        public const string CanCreateDivision = "CanCreateDivision";
        public const string CanUpdateDivision = "CanUpdateDivision";
        public const string CanDeleteDivision = "CanDeleteDivision";
        public const string CanChangeDivisionActivity = "CanChangeDivisionActivity";

        public const string CanAllGame = "CanAllGame";
        public const string CanRetrieveGame = "CanRetrieveGame";
        public const string CanRetrieveSameLocationGame = "CanRetrieveSameLocationGame";
        public const string CanRetrieveGameForActivate = "CanRetrieveGameForActivate";
        public const string CanRetrieveActivatedGame = "CanRetrieveActivatedGame";
        public const string CanCreateGame = "CanCreateGame";
        public const string CanUpdateGame = "CanUpdateGame";
        public const string CanDeleteGame = "CanDeleteGame";
        public const string CanChangeGameActivity = "CanChangeGameActivity";
        public const string CanGenerateKeysForGame = "CanGenerateKeysForGame";
        public const string CanChangeGameStatus = "CanChangeGameStatus";

        public const string CanAllLocation = "CanAllLocation";
        public const string CanRetrieveLocation = "CanRetrieveLocation";
        public const string CanCreateLocation = "CanCreateLocation";
        public const string CanUpdateLocation = "CanUpdateLocation";
        public const string CanDeleteLocation = "CanDeleteLocation";

        public const string CanAllTeam = "CanAllTeam";
        public const string CanRetrieveTeam = "CanRetrieveTeam";
        public const string CanRetrieveOwnTeam = "CanRetrieveOwnTeam";
        public const string CanRetrieveForeignTeam = "CanRetrieveForeignTeam";
        public const string CanCreateTeam = "CanCreateTeam";
        public const string CanUpdateTeam = "CanUpdateTeam";
        public const string CanDeleteTeam = "CanDeleteTeam";
        public const string CanChangeTeamActivity = "CanChangeTeamActivity";

        #endregion

        #region MainModule's CityQuest's permission names

        public const string CanAllRole = "CanAllRole";
        public const string CanRetrieveRole = "CanRetrieveRole";
        public const string CanCreateRole = "CanCreateRole";
        public const string CanUpdateRole = "CanUpdateRole";
        public const string CanDeleteRole = "CanDeleteRole";

        public const string CanAllUser = "CanAllUser";
        public const string CanRetrieveUser = "CanRetrieveUser";
        
        #endregion

        #endregion

        /// <summary>
        /// Is used to get all permission names
        /// </summary>
        /// <returns>all permission names</returns>
        public static IList<string> GetAllPermission()
        {
            return typeof(CityQuestPermissionNames).
                GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(r => r.IsLiteral || !r.IsInitOnly).Select(r => r.GetValue(null)).Cast<string>().ToList();
        }
    }
}