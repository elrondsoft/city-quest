using Abp.EntityFramework;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using CityQuest.CityQuestConstants;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.ConditionTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using CityQuest.Entities.GameModule.Locations;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.MainModule.Authorization.RolePermissionSettings;
using CityQuest.Entities.MainModule.Authorization.UserLogins;
using CityQuest.Entities.MainModule.Authorization.UserRoles;
using CityQuest.Entities.MainModule.Roles;
using CityQuest.Entities.MainModule.Users;
using CityQuest.EntityFramework;
using CityQuest.EntityFramework.Repositories;
using CityQuest.EntityFramework.Repositories.GameModule;
using System.Data.Entity;
using System.Reflection;

namespace CityQuest
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(CityQuestCoreModule))]
    public class CityQuestDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = CityQuestConsts.ConnectionStringName;
            Configuration.UnitOfWork.RegisterFilter(Filters.IPassivableFilter, false);

            IocManager.IocContainer.Register(
                //Component.For(typeof(ICityQuestRepositoryBase<,>)).ImplementedBy(typeof(CityQuestRepositoryBase<,>)),

                Component.For<ICityQuestRepositoryBase<Division, long>, IDivisionRepository>().ImplementedBy<DivisionRepository>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<Team, long>, ITeamRepository>().ImplementedBy<TeamRepository>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<Location, long>, ILocationRepository>().ImplementedBy<LocationRepository>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<Game, long>, IGameRepository>().ImplementedBy<GameRepository>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<GameTask, long>, IGameTaskRepository>().ImplementedBy<GameTaskRepository>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<GameTaskType, long>, IGameTaskTypeRepository>().ImplementedBy<GameTaskTypeRepository>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<Condition, long>, IConditionRepository>().ImplementedBy<ConditionRepository>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<ConditionType, long>, IConditionTypeRepository>().ImplementedBy<ConditionTypeRepository>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<Tip, long>, ITipRepository>().ImplementedBy<TipRepository>().LifestyleTransient(),

                Component.For<ICityQuestRepositoryBase<UserRole, long>, IUserRoleRepository>().ImplementedBy<CityQuestRepositoryBase<UserRole, long>>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<User, long>, IUserRepository>().ImplementedBy<CityQuestRepositoryBase<User, long>>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<Role, long>, IRoleRepository>().ImplementedBy<CityQuestRepositoryBase<Role, long>>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<UserLogin, long>, IUserLoginRepository>().ImplementedBy<CityQuestRepositoryBase<UserLogin, long>>().LifestyleTransient(),
                Component.For<ICityQuestRepositoryBase<RolePermissionSetting, long>, IRolePermissionSettingRepository>().ImplementedBy<CityQuestRepositoryBase<RolePermissionSetting, long>>().LifestyleTransient()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<CityQuestDbContext>(null);
        }
    }
}
