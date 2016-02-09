using Abp.Authorization;
using Abp.Dependency;
using Abp.Localization;
using CityQuest.CityQuestConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization
{
    public class CityQuestAuthorizationProvider : AuthorizationProvider, ISingletonDependency
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            foreach (var item in CityQuestPermissionNames.GetAllPermission())
            {
                context.CreatePermission(item, new FixedLocalizableString(item));//localize!
            }
        }
    }
}