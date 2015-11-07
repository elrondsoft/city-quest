using Abp.Authorization;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Games
{
    public class GamePolicy : CityQuestPolicyBase<Game, long>, IGamePolicy
    {
        public GamePolicy(ICityQuestSession session, IPermissionChecker permissionChecker)
            : base(session, permissionChecker) { }

        public bool CanChangeActivityForEntity(long userId, Game entity)
        {
            return true;
        }

        public bool CanChangeActivityForEntity(Game entity)
        {
            return CanChangeActivityForEntity(Session.UserId ?? 0, entity);
        }
    }
}
