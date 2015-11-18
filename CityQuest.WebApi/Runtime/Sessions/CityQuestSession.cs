using Abp.Dependency;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CityQuest.Runtime.Sessions
{
    public class CityQuestSession : ICityQuestSession, ISingletonDependency
    {
        public string UserName
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.GetUserName();
            }
        }

        public long? UserId
        {
            get
            {
                var userId = Thread.CurrentPrincipal.Identity.GetUserId();
                if (userId == null)
                {
                    return null;
                }

                return Convert.ToInt64(userId);
            }
        }

        public int? ImpersonatorTenantId
        {
            get { return null; }
        }

        public long? ImpersonatorUserId
        {
            get { return null; }
        }

        public Abp.MultiTenancy.MultiTenancySides MultiTenancySide
        {
            get { return Abp.MultiTenancy.MultiTenancySides.Host; }
        }

        public int? TenantId
        {
            get { return null; }
        }

        public string[] Permissions
        {
            get
            {
                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {

                    return new string[0];
                }

                var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == CityQuestConsts.PermissionKey);
                if (claim == null)
                {
                    return new string[0];
                }

                return Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(claim.Value);
            }
        }
    }
}