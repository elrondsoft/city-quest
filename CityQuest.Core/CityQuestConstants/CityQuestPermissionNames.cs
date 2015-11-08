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
        public const string CanCreateGame = "CanCreateGame";
        public const string CanUpdateGame = "CanUpdateGame";
        public const string CanDeleteGame = "CanDeleteGame";
        public const string CanChangeGameActivity = "CanChangeGameActivity";

        public const string CanAllTeam = "CanAllTeam";
        public const string CanRetrieveTeam = "CanRetrieveTeam";
        public const string CanCreateTeam = "CanCreateTeam";
        public const string CanUpdateTeam = "CanUpdateTeam";
        public const string CanDeleteTeam = "CanDeleteTeam";
        public const string CanChangeTeamActivity = "CanChangeTeamActivity";

        #endregion

        #region MainModule's CityQuest's permission names


        //TODO: add permission names

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