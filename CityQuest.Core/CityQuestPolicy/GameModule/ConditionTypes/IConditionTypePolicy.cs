using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.ConditionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.ConditionTypes
{
    public interface IConditionTypePolicy : ICityQuestPolicyBase<ConditionType, long>
    {
        bool CanChangeActivityForEntity(long userId, ConditionType entity);

        bool CanChangeActivityForEntity(ConditionType entity);
    }
}
