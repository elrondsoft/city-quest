using Abp.Dependency;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var a = Thread.CurrentPrincipal.Identity;
                if (userId == null)
                {
                    return null;
                }

                return Convert.ToInt64(userId);
            }
        }

        public int? ImpersonatorTenantId
        {
            get { throw new NotImplementedException(); }
        }

        public long? ImpersonatorUserId
        {
            get { throw new NotImplementedException(); }
        }

        public Abp.MultiTenancy.MultiTenancySides MultiTenancySide
        {
            get { return Abp.MultiTenancy.MultiTenancySides.Host; }
        }

        public int? TenantId
        {
            get { return null; }
        }
    }
}