using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Conditions
{
    public interface IConditionPolicy : ICityQuestPolicyBase<Condition, long>
    {
    }
}
