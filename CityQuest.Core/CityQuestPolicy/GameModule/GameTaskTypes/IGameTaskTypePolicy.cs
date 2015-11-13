using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.GameTaskTypes
{
    public interface IGameTaskTypePolicy : ICityQuestPolicyBase<GameTaskType, long>
    {
        bool CanChangeActivityForEntity(long userId, GameTaskType entity);

        bool CanChangeActivityForEntity(GameTaskType entity);
    }
}
