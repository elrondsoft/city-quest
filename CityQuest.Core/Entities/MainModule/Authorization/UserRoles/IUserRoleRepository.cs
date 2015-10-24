using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Entities.MainModule.Authorization.UserRoles
{
    public interface IUserRoleRepository : ICityQuestRepositoryBase<UserRole, long>, ITransientDependency
    {
    }
}
