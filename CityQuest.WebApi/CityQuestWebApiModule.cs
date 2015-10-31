using System.Reflection;
using Abp.Application.Services;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;
using CityQuest.ApplicationServices.GameModule.Divisions;
using CityQuest.ApplicationServices.GameModule.Teams;
using CityQuest.Runtime.Sessions;
using Castle.MicroKernel.Registration;
using Abp.Runtime.Session;
using CityQuest.ApplicationServices.MainModule.Roles;
using CityQuest.ApplicationServices.MainModule.Users;

namespace CityQuest
{
    [DependsOn(typeof(AbpWebApiModule), typeof(CityQuestApplicationModule))]
    public class CityQuestWebApiModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.IocContainer.Register(
                Component.For<IAbpSession, ICityQuestSession>().ImplementedBy<CityQuestSession>().LifestyleSingleton()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            #region Main module's dynamic api controller builders

            DynamicApiControllerBuilder.For<IRoleAppService>("cityQuest/role").Build();

            DynamicApiControllerBuilder.For<IUserAppService>("cityQuest/user").Build();

            #endregion

            #region Main module's dynamic api controller builders

            DynamicApiControllerBuilder.For<IDivisionAppService>("cityQuest/division").Build();

            DynamicApiControllerBuilder.For<ITeamAppService>("cityQuest/team").Build();

            #endregion
        }
    }
}
