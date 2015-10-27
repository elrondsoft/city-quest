using System.Reflection;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using Microsoft.AspNet.Identity;
using CityQuest.Entities.MainModule.Users;
using CityQuest.Entities.MainModule.Authorization.UserServices;
using CityQuest.Entities.MainModule.Authorization.RoleServices;
using CityQuest.Entities.MainModule.Roles;
using CityQuest.Entities.MainModule.Authorization;

namespace CityQuest
{
    public class CityQuestCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.IocContainer.Register(
                Component.For<IUserStore<User, long>, IUserPasswordStore<User, long>, UserStore>().ImplementedBy<UserStore>().LifestyleTransient(),
                Component.For<IRoleStore<Role, long>, RoleStore>().ImplementedBy<RoleStore>().LifestyleTransient()
                );
            base.PreInitialize();
        }

        public override void Initialize()
        {
            Configuration.Authorization.Providers.Add(typeof(CityQuestAuthorizationProvider));
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
