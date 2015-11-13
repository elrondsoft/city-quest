using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Tips
{
    public interface ITipPolicy : ICityQuestPolicyBase<Tip, long>
    {
        bool CanChangeActivityForEntity(long userId, Tip entity);

        bool CanChangeActivityForEntity(Tip entity);
    }
}
