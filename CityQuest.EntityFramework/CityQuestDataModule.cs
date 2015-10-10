using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using CityQuest.EntityFramework;

namespace CityQuest
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(CityQuestCoreModule))]
    public class CityQuestDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "CityQuest";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<CityQuestDbContext>(null);
        }
    }
}
