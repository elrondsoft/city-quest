using Abp.Authorization;
using CityQuest.CityQuestConstants;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.MainModule.Users;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.CityQuestPolicy.GameModule.Teams
{
    public class TeamPolicy : CityQuestPolicyBase<Team, long>, ITeamPolicy
    {
        protected IUserRepository UserRepository { get; set; }
        public TeamPolicy(ICityQuestSession session, IPermissionChecker permissionChecker) 
            : base(session, permissionChecker) { }

        public override bool CanRetrieveEntity(long userId, Team entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieve) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllTeam) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveTeam))
                return true;

            return false;
        }

        public bool CanRetrieveOwnTeam(long userId, Team entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieve) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllTeam) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveTeam))
                return true;
            
            User user = UserRepository.Get(userId);
            if (entity.Players.Contains(user))
            {
                return true;
            }

            return false;
        }

        public bool CanRetrieveForeignTeam(long userId, Team entity)
        {
            return false;
        }

        public override IQueryable<Team> CanRetrieveManyEntities(long userId, IQueryable<Team> entities)
        {
            if (userId == 0)
                return new List<Team>().AsQueryable();

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieve) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllTeam) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveTeam))
                return entities;

            return new List<Team>().AsQueryable();
        }

        public override bool CanCreateEntity(long userId, Team entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanCreate) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllTeam) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanCreateTeam))
                return true;

            return false;
        }

        public override bool CanUpdateEntity(long userId, Team entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdate) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllTeam) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdateTeam))
                return true;

            return false;
        }

        public override bool CanDeleteEntity(long userId, Team entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanDelete) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllTeam) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanDeleteTeam))
                return true;

            return false;
        }

        public bool CanChangeActivityForEntity(long userId, Team entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdate) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllTeam) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdateTeam) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanChangeTeamActivity))
                return true;

            return false;
        }

        public bool CanChangeActivityForEntity(Team entity)
        {
            return CanChangeActivityForEntity(Session.UserId ?? 0, entity);
        }
    }
}
