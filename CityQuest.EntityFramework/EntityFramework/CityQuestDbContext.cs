using Abp.Domain.Entities;
using Abp.EntityFramework;
#region using CityQuest.GameModule...
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.ConditionTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using CityQuest.Entities.GameModule.Keys;
using CityQuest.Entities.GameModule.Teams;
#endregion
#region using CityQuest.Entities.MainModule...
using CityQuest.Entities.MainModule.Users;
#endregion
using System.Data.Entity;
using global::EntityFramework.DynamicFilters;
using System.Diagnostics;

namespace CityQuest.EntityFramework
{
    public class CityQuestDbContext : AbpDbContext
    {
        #region DataBase Sets

        #region MainModule DbSets

        public virtual IDbSet<User> Users { get; set; }

        #endregion

        #region GameModule DbSets

        public virtual IDbSet<Division> Divisions { get; set; }
        public virtual IDbSet<Team> Teams { get; set; }
        public virtual IDbSet<Key> Keys { get; set; }
        public virtual IDbSet<Game> Games { get; set; }
        public virtual IDbSet<GameTask> GameTasks { get; set; }
        public virtual IDbSet<GameTaskType> GameTaskTypes { get; set; }
        public virtual IDbSet<Tip> Tips { get; set; }
        public virtual IDbSet<Condition> Conditions { get; set; }
        public virtual IDbSet<ConditionType> ConditionTypes { get; set; }

        #endregion
        
        #endregion

        public CityQuestDbContext()
            : base(CityQuestConsts.ConnectionStringName)
        {
            this.Database.CommandTimeout = 3600;
            this.Database.Log = (r) => Trace.WriteLine(r);
        }

        public CityQuestDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Database.CommandTimeout = 3600;
            this.Database.Log = (r) => Trace.WriteLine(r);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Filters

            modelBuilder.Filter(CityQuest.CityQuestConstants.Filters.IPassivableFilter, (IPassivable e) => e.IsActive, true);

            #endregion

            #region MainModule



            #endregion

            #region GameModule

            modelBuilder.Entity<Team>()
                .HasRequired(r => r.Division)
                .WithMany(r => r.Teams)
                .HasForeignKey(r => r.DivisionId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Team>()
                .HasRequired(r => r.Captain)
                .WithMany(r => r.LeadedTeams)
                .HasForeignKey(r => r.CaptainId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Team>()
                .HasMany(r => r.Players)
                .WithMany(r => r.Teams);

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
