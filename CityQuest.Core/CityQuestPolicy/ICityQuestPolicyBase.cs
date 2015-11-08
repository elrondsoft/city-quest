using Abp.Dependency;
using Abp.Domain.Policies;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy
{
    public interface ICityQuestPolicyBase<TEntity, TEntityPrimaryKey> : IPolicy, ITransientDependency 
        where TEntity : class, Abp.Domain.Entities.IEntity<TEntityPrimaryKey>
    {
        bool CanRetrieveEntity(long userId, TEntity entity);
        IQueryable<TEntity> CanRetrieveManyEntities(long userId, IQueryable<TEntity> entities);
        bool CanCreateEntity(long userId, TEntity entity);
        bool CanUpdateEntity(long userId, TEntity entity);
        bool CanDeleteEntity(long userId, TEntity entity);
        bool CanRetrieveEntity(TEntity entity);
        IQueryable<TEntity> CanRetrieveManyEntities(IQueryable<TEntity> entities);
        bool CanCreateEntity(TEntity entity);
        bool CanUpdateEntity(TEntity entity);
        bool CanDeleteEntity(TEntity entity);
    }
}