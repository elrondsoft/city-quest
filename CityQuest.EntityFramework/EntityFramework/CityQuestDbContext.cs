using Abp.Domain.Entities;
using Abp.EntityFramework;
#region using CityQuest.GameModule...
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameStatuses;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.ConditionTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.PlayerAttempts;
using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using CityQuest.Entities.GameModule.Keys;
using CityQuest.Entities.GameModule.Locations;
using CityQuest.Entities.GameModule.PlayerCareers;
using CityQuest.Entities.GameModule.Statistics.PlayerGameStatistics;
using CityQuest.Entities.GameModule.Statistics.PlayerGameTaskStatistics;
using CityQuest.Entities.GameModule.Statistics.TeamGameStatistics;
using CityQuest.Entities.GameModule.Statistics.TeamGameTaskStatistics;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.GameModule.Teams.TeamRequests;
#endregion
#region using CityQuest.Entities.MainModule...
using CityQuest.Entities.MainModule.Authorization;
using CityQuest.Entities.MainModule.Authorization.RolePermissionSettings;
using CityQuest.Entities.MainModule.Authorization.UserLogins;
using CityQuest.Entities.MainModule.Authorization.UserRoles;
using CityQuest.Entities.MainModule.Authorization.UserServices;
using CityQuest.Entities.MainModule.Roles;
using CityQuest.Entities.MainModule.Users;
#endregion
using global::EntityFramework.DynamicFilters;
using System.Data.Entity;
using System.Diagnostics;


namespace CityQuest.EntityFramework
{
    public class CityQuestDbContext : AbpDbContext
    {
        #region Consts

        public const int CityQuestDatabaseCommandTimeout = 3600;

        #endregion

        #region DataBase Sets

        #region MainModule DbSets

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Role> Roles { get; set; }
        public virtual IDbSet<UserLogin> UserLogins { get; set; }
        public virtual IDbSet<PermissionSetting> Permissions { get; set; }
        public virtual IDbSet<RolePermissionSetting> RolePermissions { get; set; }
        public virtual IDbSet<UserPermissionSetting> UserPermissions { get; set; }
        //public virtual IDbSet<Setting> Settings { get; set; }

        #endregion

        #region GameModule DbSets

        public virtual IDbSet<Location> Locations { get; set; }
        public virtual IDbSet<Division> Divisions { get; set; }
        public virtual IDbSet<Team> Teams { get; set; }
        public virtual IDbSet<TeamRequest> TeamRequests { get; set; }
        public virtual IDbSet<Key> Keys { get; set; }
        public virtual IDbSet<Game> Games { get; set; }
        public virtual IDbSet<GameStatus> GameStatuses { get; set; }
        public virtual IDbSet<GameTask> GameTasks { get; set; }
        public virtual IDbSet<GameTaskType> GameTaskTypes { get; set; }
        public virtual IDbSet<Tip> Tips { get; set; }
        public virtual IDbSet<Condition> Conditions { get; set; }
        public virtual IDbSet<ConditionType> ConditionTypes { get; set; }
        public virtual IDbSet<PlayerCareer> PlayerCareers { get; set; }
        public virtual IDbSet<SuccessfulPlayerAttempt> SuccessfulPlayerAttempts { get; set; }
        public virtual IDbSet<UnsuccessfulPlayerAttempt> UnsuccessfulPlayerAttempts { get; set; }

        #region Statistics

        public virtual IDbSet<PlayerGameStatistic> PlayerGameStatistics { get; set; }
        public virtual IDbSet<PlayerGameTaskStatistic> PlayerGameTaskStatistics { get; set; }
        public virtual IDbSet<TeamGameStatistic> TeamGameStatistics { get; set; }
        public virtual IDbSet<TeamGameTaskStatistic> TeamGameTaskStatistics { get; set; }
        
        #endregion

        #endregion

        #endregion

        #region Ctors

        public CityQuestDbContext()
            : base(CityQuestConsts.ConnectionStringName)
        {
            this.Database.CommandTimeout = CityQuestDatabaseCommandTimeout;
            this.Database.Log = (r) => Trace.WriteLine(r);
        }

        public CityQuestDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Database.CommandTimeout = CityQuestDatabaseCommandTimeout;
            this.Database.Log = (r) => Trace.WriteLine(r);
        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Filters

            modelBuilder.Filter(CityQuest.CityQuestConstants.Filters.IPassivableFilter, (IPassivable e) => e.IsActive, true);

            #endregion

            #region MainModule

            modelBuilder.Entity<RolePermissionSetting>()
                .HasRequired(r => r.Role)
                .WithMany(r => r.Permissions)
                .HasForeignKey(r => r.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRole>()
                .HasRequired(r => r.User)
                .WithMany(r => r.Roles)
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRole>()
                .HasRequired(r => r.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(r => r.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserPermissionSetting>()
                .HasRequired(r => r.User)
                .WithMany(r => r.Permissions)
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(r => r.Logins)
                .WithMany(r => r.Users);
            modelBuilder.Entity<User>()
                .HasOptional(r => r.Location)
                .WithMany(r => r.Users)
                .HasForeignKey(r => r.LocationId)
                .WillCascadeOnDelete(false);

            #endregion

            #region GameModule

            #region Statistics

            modelBuilder.Entity<PlayerGameStatistic>()
                .HasRequired(r => r.PlayerCareer)
                .WithMany(r => r.PlayerGameStatistics)
                .HasForeignKey(r => r.PlayerCareerId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PlayerGameStatistic>()
                .HasRequired(r => r.Game)
                .WithMany(r => r.PlayerGameStatistics)
                .HasForeignKey(r => r.GameId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlayerGameTaskStatistic>()
                .HasRequired(r => r.PlayerCareer)
                .WithMany(r => r.PlayerGameTaskStatistics)
                .HasForeignKey(r => r.PlayerCareerId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PlayerGameTaskStatistic>()
                .HasRequired(r => r.GameTask)
                .WithMany(r => r.PlayerGameTaskStatistics)
                .HasForeignKey(r => r.GameTaskId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TeamGameStatistic>()
                .HasRequired(r => r.Team)
                .WithMany(r => r.TeamGameStatistics)
                .HasForeignKey(r => r.TeamId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<TeamGameStatistic>()
                .HasRequired(r => r.Game)
                .WithMany(r => r.TeamGameStatistics)
                .HasForeignKey(r => r.GameId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TeamGameTaskStatistic>()
                .HasRequired(r => r.Team)
                .WithMany(r => r.TeamGameTaskStatistics)
                .HasForeignKey(r => r.TeamId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<TeamGameTaskStatistic>()
                .HasRequired(r => r.GameTask)
                .WithMany(r => r.TeamGameTaskStatistics)
                .HasForeignKey(r => r.GameTaskId)
                .WillCascadeOnDelete(false);

            #endregion

            modelBuilder.Entity<Team>()
                .HasRequired(r => r.Division)
                .WithMany(r => r.Teams)
                .HasForeignKey(r => r.DivisionId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<TeamRequest>()
                .HasRequired(r => r.Team)
                .WithMany(r => r.TeamRequests)
                .HasForeignKey(r => r.TeamId)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<TeamRequest>()
                .HasRequired(r => r.InvitedUser)
                .WithMany(r => r.TeamRequests)
                .HasForeignKey(r => r.InvitedUserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<PlayerCareer>()
                .HasRequired(r => r.User)
                .WithMany(r => r.PlayerCareers)
                .HasForeignKey(r => r.UserId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<PlayerCareer>()
                .HasRequired(r => r.Team)
                .WithMany(r => r.PlayerCareers)
                .HasForeignKey(r => r.TeamId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SuccessfulPlayerAttempt>()
                .HasRequired(r => r.PlayerCareer)
                .WithMany(r => r.SuccessfulPlayerAttempts)
                .HasForeignKey(r => r.PlayerCareerId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<SuccessfulPlayerAttempt>()
                .HasRequired(r => r.Condition)
                .WithMany(r => r.SuccessfullPlayerAttempts)
                .HasForeignKey(r => r.ConditionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnsuccessfulPlayerAttempt>()
                .HasRequired(r => r.PlayerCareer)
                .WithMany(r => r.UnsuccessfulPlayerAttempts)
                .HasForeignKey(r => r.PlayerCareerId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<UnsuccessfulPlayerAttempt>()
                .HasRequired(r => r.Condition)
                .WithMany(r => r.UnsuccessfullPlayerAttempts)
                .HasForeignKey(r => r.ConditionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Key>()
                .HasOptional(r => r.OwnerUser)
                .WithMany(r => r.ActivatedKeys)
                .HasForeignKey(r => r.OwnerUserId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Key>()
                .HasRequired(r => r.Game)
                .WithMany(r => r.Keys)
                .HasForeignKey(r => r.GameId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Game>()
                .HasRequired(r => r.Location)
                .WithMany(r => r.Games)
                .HasForeignKey(r => r.LocationId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Game>()
                .HasRequired(r => r.GameStatus)
                .WithMany(r => r.Games)
                .HasForeignKey(r => r.GameStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GameTask>()
                .HasRequired(r => r.Game)
                .WithMany(r => r.GameTasks)
                .HasForeignKey(r => r.GameId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<GameTask>()
                .HasRequired(r => r.GameTaskType)
                .WithMany(r => r.GameTasks)
                .HasForeignKey(r => r.GameTaskTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tip>()
                .HasRequired(r => r.GameTask)
                .WithMany(r => r.Tips)
                .HasForeignKey(r => r.GameTaskId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Condition>()
                .HasRequired(r => r.GameTask)
                .WithMany(r => r.Conditions)
                .HasForeignKey(r => r.GameTaskId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Condition>()
                .HasRequired(r => r.ConditionType)
                .WithMany(r => r.Conditions)
                .HasForeignKey(r => r.ConditionTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
