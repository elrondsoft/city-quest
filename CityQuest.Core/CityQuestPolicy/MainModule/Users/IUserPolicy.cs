using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.MainModule.Users
{
    public interface IUserPolicy : ICityQuestPolicyBase<User, long>
    {
    }
}
