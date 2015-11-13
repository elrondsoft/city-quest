using Abp.Authorization;
using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.GameTaskTypes
{
    public class GameTaskTypePolicy : CityQuestPolicyBase<GameTaskType, long>, IGameTaskTypePolicy
    {
        public GameTaskTypePolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }

        public bool CanChangeActivityForEntity(long userId, GameTaskType entity)
        {
            return true;
        }

        public bool CanChangeActivityForEntity(GameTaskType entity)
        {
            return CanChangeActivityForEntity(Session.UserId ?? 0, entity);
        }
    }
}
