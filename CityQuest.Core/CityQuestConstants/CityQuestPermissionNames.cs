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

        public const string CanAll = "CanAll";

        //public const string CanDoSomething = "CanDoSomething";

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