using CityQuest.Entities.GameModule.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Teams
{
    public interface ITeamPolicy : ICityQuestPolicyBase<Team, long>
    {
        bool CanChangeActivityForEntity(long userId, Team entity);

        bool CanChangeActivityForEntity(Team entity);
    }
}
