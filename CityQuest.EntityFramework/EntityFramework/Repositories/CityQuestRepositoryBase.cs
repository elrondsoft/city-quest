using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CityQuest.EntityFramework.Repositories
{
    public class CityQuestRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<CityQuestDbContext, TEntity, TPrimaryKey>, 
        ICityQuestRepositoryBase<TEntity, TPrimaryKey>, ITransientDependency
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public CityQuestRepositoryBase(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual TEntity UpdateFields(TEntity entity, params System.Linq.Expressions.Expression<Func<TEntity, object>>[] fieldSelectors)
        {
            Table.Attach(entity);
            foreach (var fieldSelector in fieldSelectors)
            {
                Context.Entry<TEntity>(entity).Property(fieldSelector).IsModified = true;
            }
            return entity;
        }

        public virtual TEntity Attach(TEntity entity)
        {
            Table.Attach(entity);
            return entity;
        }

        public virtual void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property) 
            where TProperty : class
        {
            Context.Entry(entity).Reference(property).Load();
        }

        public override System.Linq.IQueryable<TEntity> GetAll()
        {
            var query = Table.AsQueryable<TEntity>();

            foreach (var include in Includes)
                query = query.Include(include);

            return query;
        }

        private IList<Expression<Func<TEntity, object>>> includes = new List<Expression<Func<TEntity, object>>>();
        public IList<Expression<Func<TEntity, object>>> Includes
        {
            get
            {
                return includes;
            }
        }


        public virtual IQueryable<TEntity> GetAllLocal()
        {
            return Context.Set<TEntity>().Local.AsQueryable();
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            return Context.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            return Context.Set<TEntity>().RemoveRange(entities);
        }

        /// <summary>
        /// Using to change entity state to Detached (changes state only for transmitted entity)
        /// </summary>
        /// <param name="entity">entity</param>
        public void Detach(TEntity entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Detached;
        }
    }

    public abstract class CityQuestRepositoryBase<TEntity> : CityQuestRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected CityQuestRepositoryBase(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
