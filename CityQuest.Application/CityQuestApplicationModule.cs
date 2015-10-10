using System.Reflection;
using Abp.Modules;

namespace CityQuest
{
    [DependsOn(typeof(CityQuestCoreModule))]
    public class CityQuestApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
