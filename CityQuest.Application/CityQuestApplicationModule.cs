using System.Reflection;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using CityQuest.ApplicationServices.GameModule.Divisions;
using CityQuest.Events.Notifiers;

namespace CityQuest
{
    [DependsOn(typeof(CityQuestCoreModule))]
    public class CityQuestApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.IocContainer.Register(
                Component.For<IGameChangesNotifier>().ImplementedBy<GameChangesNotifier>().LifestyleSingleton(),
                Component.For<IStatisticsChangesNotifier>().ImplementedBy<StatisticsChangesNotifier>().LifestyleSingleton()
                );

            CityQuest.Mapping.CityQuestDtoMapper.Map();

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
