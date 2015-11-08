using Abp.Authorization;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy
{
    public class CityQuestPolicyBase<TEntity, TEntityPrimaryKey> : ICityQuestPolicyBase<TEntity, TEntityPrimaryKey>
        where TEntity : class, Abp.Domain.Entities.IEntity<TEntityPrimaryKey>
    {

        #region Injected Dependencies

        protected ICityQuestSession Session { get; set; }
        protected IPermissionChecker PermissionChecker { get; set; }

        #endregion

        #region ctors

        public CityQuestPolicyBase(ICityQuestSession session, IPermissionChecker permissionChecker)
        {
            Session = session;
            PermissionChecker = permissionChecker;
        }

        #endregion

        public virtual bool CanRetrieveEntity(long userId, TEntity entity)
        {
            return true;
        }

        public virtual IQueryable<TEntity> CanRetrieveManyEntities(long userId, IQueryable<TEntity> entities)
        {
            return entities;
        }

        public virtual bool CanCreateEntity(long userId, TEntity entity)
        {
            return true;
        }

        public virtual bool CanUpdateEntity(long userId, TEntity entity)
        {
            return true;
        }

        public virtual bool CanDeleteEntity(long userId, TEntity entity)
        {
            return true;
        }

        public bool CanRetrieveEntity(TEntity entity)
        {
            return CanRetrieveEntity(Session.UserId ?? 0, entity);
        }

        public IQueryable<TEntity> CanRetrieveManyEntities(IQueryable<TEntity> entities)
        {
            return CanRetrieveManyEntities(Session.UserId ?? 0, entities);
        }

        public bool CanCreateEntity(TEntity entity)
        {
            return CanCreateEntity(Session.UserId ?? 0, entity);
        }

        public bool CanUpdateEntity(TEntity entity)
        {
            return CanUpdateEntity(Session.UserId ?? 0, entity);
        }

        public bool CanDeleteEntity(TEntity entity)
        {
            return CanDeleteEntity(Session.UserId ?? 0, entity);
        }
    }
}
