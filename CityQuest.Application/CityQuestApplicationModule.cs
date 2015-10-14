using System.Reflection;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using CityQuest.ApplicationServices.GameModule.Divisions;

namespace CityQuest
{
    [DependsOn(typeof(CityQuestCoreModule))]
    public class CityQuestApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.IocContainer.Register(
                //Component.For<IDivisionAppService>().ImplementedBy<DivisionAppService>().LifestyleTransient()
                );
            CityQuest.Mapping.CityQuestDtoMapper.Map();
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
