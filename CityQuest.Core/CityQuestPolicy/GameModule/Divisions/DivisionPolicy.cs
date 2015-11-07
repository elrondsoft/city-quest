using Abp.Authorization;
using CityQuest.CityQuestConstants;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Divisions
{
    public class DivisionPolicy : CityQuestPolicyBase<Division, long>, IDivisionPolicy
    {
        public DivisionPolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }

        public bool CanChangeActivityForEntity(long userId, Division entity)
        {
            if (userId == 0) 
                return false;

            if (!entity.IsDefault &&
                (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdate) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllDivision) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdateDivision) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanChangeDivisionActivity)))
                return true;

            return false;
        }

        public bool CanChangeActivityForEntity(Division entity)
        {
            return CanChangeActivityForEntity(Session.UserId ?? 0, entity);
        }
    }
}
