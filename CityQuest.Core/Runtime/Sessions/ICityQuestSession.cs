using Abp.Dependency;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Runtime.Sessions
{
    public interface ICityQuestSession : IAbpSession, ISingletonDependency
    {
        string UserName { get; }

        string[] Permissions { get; }
    }
}
