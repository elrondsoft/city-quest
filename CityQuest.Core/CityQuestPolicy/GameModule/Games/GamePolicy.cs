using Abp.Authorization;
using CityQuest.CityQuestConstants;
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

        public override bool CanRetrieveEntity(long userId, Game entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieve) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllGame) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveGame))
                return true;

            return false;
        }

        public override IQueryable<Game> CanRetrieveManyEntities(long userId, IQueryable<Game> entities)
        {
            if (userId == 0)
                return new List<Game>().AsQueryable();

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieve) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllGame) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveGame))
                return entities;

            return new List<Game>().AsQueryable();
        }

        public override bool CanCreateEntity(long userId, Game entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanCreate) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllGame) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanCreateGame))
                return true;

            return false;
        }

        public override bool CanUpdateEntity(long userId, Game entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdate) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllGame) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdateGame))
                return true;

            return false;
        }

        public override bool CanDeleteEntity(long userId, Game entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanDelete) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllGame) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanDeleteGame))
                return true;

            return false;
        }

        public bool CanChangeActivityForEntity(long userId, Game entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdate) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllGame) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdateGame) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanChangeGameActivity))
                return true;

            return false;
        }

        public bool CanChangeActivityForEntity(Game entity)
        {
            return CanChangeActivityForEntity(Session.UserId ?? 0, entity);
        }
    }
}
