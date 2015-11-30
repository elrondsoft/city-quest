using Abp.Authorization;
using CityQuest.CityQuestConstants;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Keys;
using CityQuest.Entities.MainModule.Users;
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
        protected IUserRepository UserRepository { get; set; }
        protected IKeyRepository KeyRepository { get; set; }


        public GamePolicy(ICityQuestSession session, IPermissionChecker permissionChecker, IUserRepository userRepository, IKeyRepository keyRepository)
            : base(session, permissionChecker)
        {
            UserRepository = userRepository;
            KeyRepository = keyRepository;
        }

        public override bool CanRetrieveEntity(long userId, Game entity)
        {
            if (userId == 0)
                return false;
            bool result1 = false, result2 = false, result3 = false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieve) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllGame) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveGame))
                return true;
            
            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveSameLocationGame))
            {
                long? userLocationId = UserRepository.Get(userId).LocationId;
                result1 = userLocationId != null && entity.LocationId == (long)userLocationId;
            }
            
            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveGameForActivate))
            {
                result2 = entity.IsActive && (DateTime.Now < entity.StartDate);
            }

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveActivatedGame))
            {
                long count = KeyRepository.GetAll().Where(r => r.GameId == entity.Id && r.OwnerUserId == userId).Count();
                result3 =  count > 0;
            }
            #warning fix this results1, results2, results3 -> нормальное 
            return (result1 || result2 || result3);
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

        public bool CanGenerateKeysForEntity(long userId, Game entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanGenerateKeysForGame))
                return true;

            return false;
        }

        public bool CanGenerateKeysForEntity(Game entity)
        {
            return CanGenerateKeysForEntity(Session.UserId ?? 0, entity);
        }
    }
}
