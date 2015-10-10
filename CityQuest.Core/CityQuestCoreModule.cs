using System.Reflection;
using Abp.Modules;

namespace CityQuest
{
    public class CityQuestCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
