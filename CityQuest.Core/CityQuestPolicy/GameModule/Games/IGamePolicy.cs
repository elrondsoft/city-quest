using CityQuest.Entities.GameModule.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Games
{
    public interface IGamePolicy : ICityQuestPolicyBase<Game, long>
    {
        bool CanChangeActivityForEntity(long userId, Game entity);
        bool CanChangeActivityForEntity(Game entity);

        bool CanGenerateKeysForEntity(long userId, Game entity);
        bool CanGenerateKeysForEntity(Game entity);
    }
}
