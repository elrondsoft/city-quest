using Abp.Authorization;
using CityQuest.CityQuestConstants;
using CityQuest.Entities.GameModule.Locations;
using CityQuest.Runtime.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityQuest.CityQuestPolicy.GameModule.Locations
{
    public class LocationPolicy : CityQuestPolicyBase<Location, long>, ILocationPolicy
    {
        public LocationPolicy(ICityQuestSession session, IPermissionChecker permissionChecker)
            : base(session, permissionChecker)
        { }

        public override bool CanRetrieveEntity(long userId, Location entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieve) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllLocation) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveLocation))
                return true;

            return false;
        }

        public override IQueryable<Location> CanRetrieveManyEntities(long userId, IQueryable<Location> entities)
        {
            if (userId == 0)
                return new List<Location>().AsQueryable();

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieve) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllLocation) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanRetrieveLocation))
                return entities;

            return new List<Location>().AsQueryable();
        }

        public override bool CanCreateEntity(long userId, Location entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanCreate) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllLocation) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanCreateLocation))
                return true;

            return false;
        }

        public override bool CanUpdateEntity(long userId, Location entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdate) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllLocation) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanUpdateLocation))
                return true;

            return false;
        }

        public override bool CanDeleteEntity(long userId, Location entity)
        {
            if (userId == 0)
                return false;

            if (PermissionChecker.IsGranted(CityQuestPermissionNames.CanAll) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanDelete) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanAllLocation) ||
                PermissionChecker.IsGranted(CityQuestPermissionNames.CanDeleteLocation))
                return true;

            return false;
        }
    }
}
