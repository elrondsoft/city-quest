using Abp.Dependency;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest
{
    public interface ICityQuestRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>, ITransientDependency 
        where TEntity : class, Abp.Domain.Entities.IEntity<TPrimaryKey>
    {
        TEntity Attach(TEntity entity);
        TEntity UpdateFields(TEntity entity, params System.Linq.Expressions.Expression<Func<TEntity, object>>[] fieldSelectors);
        void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property) where TProperty : class;
        IList<Expression<Func<TEntity, object>>> Includes { get; }
        IQueryable<TEntity> GetAllLocal();
        IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
    }
}