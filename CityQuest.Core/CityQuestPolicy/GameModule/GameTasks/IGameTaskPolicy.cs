using CityQuest.Entities.GameModule.Games.GameTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.GameTasks
{
    public interface IGameTaskPolicy : ICityQuestPolicyBase<GameTask, long>
    {
        bool CanChangeActivityForEntity(long userId, GameTask entity);

        bool CanChangeActivityForEntity(GameTask entity);
    }
}
