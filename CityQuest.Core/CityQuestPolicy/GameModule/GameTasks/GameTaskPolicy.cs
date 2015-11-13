using Abp.Authorization;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.GameTasks
{
    public class GameTaskPolicy : CityQuestPolicyBase<GameTask, long>, IGameTaskPolicy
    {
        public GameTaskPolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }

        public bool CanChangeActivityForEntity(long userId, GameTask entity)
        {
            return true;
        }

        public bool CanChangeActivityForEntity(GameTask entity)
        {
            return CanChangeActivityForEntity(Session.UserId ?? 0, entity);
        }
    }
}
