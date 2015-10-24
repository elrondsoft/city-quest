using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Users
{
    public interface IUserRepository : ICityQuestRepositoryBase<User, long>, ITransientDependency
    {
    }
}
