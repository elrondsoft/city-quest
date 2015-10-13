using System.Reflection;
using Abp.Modules;
using Castle.MicroKernel.Registration;

namespace CityQuest
{
    public class CityQuestCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //IocManager.IocContainer.Register();
            base.PreInitialize();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
