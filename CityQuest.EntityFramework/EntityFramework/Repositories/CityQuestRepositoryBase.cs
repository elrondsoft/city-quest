using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace CityQuest.EntityFramework.Repositories
{
    public class CityQuestRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<CityQuestDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected CityQuestRepositoryBase(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class CityQuestRepositoryBase<TEntity> : CityQuestRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected CityQuestRepositoryBase(IDbContextProvider<CityQuestDbContext> dbContextProvider)
            : base(dbContextProvider) { }
    }
}
