using System.Reflection;
using Abp.Application.Services;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;
using CityQuest.ApplicationServices.GameModule.Divisions;

namespace CityQuest
{
    [DependsOn(typeof(AbpWebApiModule), typeof(CityQuestApplicationModule))]
    public class CityQuestWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //DynamicApiControllerBuilder
            //    .ForAll<IApplicationService>(typeof(CityQuestApplicationModule).Assembly, "cityQuest")
            //    .Build();

            DynamicApiControllerBuilder.For<IDivisionAppService>("cityQuest/division").Build();
        }
    }
}
