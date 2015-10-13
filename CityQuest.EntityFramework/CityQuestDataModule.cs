﻿using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using CityQuest.EntityFramework;
using Castle.MicroKernel.Registration;
using CityQuest.EntityFramework.Repositories;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Entities.GameModule.Teams;

namespace CityQuest
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(CityQuestCoreModule))]
    public class CityQuestDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "CityQuest";

            IocManager.IocContainer.Register(
                //Component.For<typeof(ICityQuestRepositoryBase<,>)>().ImplementedBy<typeof(CityQuestRepositoryBase<,>)>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<Division, long>, IDivisionRepository>().ImplementedBy<CityQuestRepositoryBase<Division, long>>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<Team, long>, ITeamRepository>().ImplementedBy<CityQuestRepositoryBase<Team, long>>().LifestyleTransient()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<CityQuestDbContext>(null);
        }
    }
}
