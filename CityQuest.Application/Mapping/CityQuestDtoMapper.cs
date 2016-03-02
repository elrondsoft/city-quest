using CityQuest.ApplicationServices.GameModule.Conditions.Dtos;
using CityQuest.ApplicationServices.GameModule.ConditionTypes.Dtos;
using CityQuest.ApplicationServices.GameModule.Divisions.Dtos;
using CityQuest.ApplicationServices.GameModule.Games.Dtos;
using CityQuest.ApplicationServices.GameModule.GamesLight.Dtos;
using CityQuest.ApplicationServices.GameModule.GameStatuses.Dtos;
using CityQuest.ApplicationServices.GameModule.GameTasks.Dtos;
using CityQuest.ApplicationServices.GameModule.GameTaskTypes.Dtos;
using CityQuest.ApplicationServices.GameModule.Locations.Dtos;
using CityQuest.ApplicationServices.GameModule.PlayerCareers.Dtos;
using CityQuest.ApplicationServices.GameModule.TeamRequests.Dtos;
using CityQuest.ApplicationServices.GameModule.Teams.Dtos;
using CityQuest.ApplicationServices.GameModule.Tips.Dtos;
using CityQuest.ApplicationServices.MainModule.Permissions.Dto;
using CityQuest.ApplicationServices.MainModule.Roles.Dto;
using CityQuest.ApplicationServices.MainModule.Users.Dto;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameStatuses;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.ConditionTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using CityQuest.Entities.GameModule.Locations;
using CityQuest.Entities.GameModule.PlayerCareers;
using CityQuest.Entities.GameModule.Statistics.TeamGameTaskStatistics;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.GameModule.Teams.TeamRequests;
using CityQuest.Entities.MainModule.Authorization.RolePermissionSettings;
using CityQuest.Entities.MainModule.Authorization.UserRoles;
using CityQuest.Entities.MainModule.Roles;
using CityQuest.Entities.MainModule.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityQuest.Mapping
{
    public static class CityQuestDtoMapper
    {
        public static void Map()
        {
            #region Main module mapping

            AutoMapper.Mapper.CreateMap<RolePermissionSetting, PermissionDto>()
                .ForMember(r => r.DisplayText, r => r.MapFrom(e => e.Name))
                .ForMember(r => r.Value, r => r.MapFrom(e => e.Name));

            AutoMapper.Mapper.CreateMap<Role, RoleDto>()
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName));

            AutoMapper.Mapper.CreateMap<User, UserDto>()
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ForMember(r => r.Roles, r => r.MapFrom(k => k.Roles.Select(e => e.Role)))
                .ForMember(r => r.Password, r => r.MapFrom(e => CityQuestConsts.FakePassword))
                .ReverseMap()
                .ForMember(r => r.Roles, r => r.Ignore())
                .ForMember(r => r.Password, r => r.Ignore())
                .AfterMap((udto, u) =>
                {
                    u.Roles = udto.Roles.Select(r => new UserRole() 
                        { 
                            RoleId = r.Id,
                            User = u, 
                            UserId = u.Id 
                        }).ToList();
                    if (udto.Password != CityQuestConsts.FakePassword)
                        u.Password = new PasswordHasher().HashPassword(udto.Password);
                });

            #endregion

            #region Game module mapping

            #region GameLight entities mapping
            //These entities are using for retrieve thats why no ReverseMap!

            AutoMapper.Mapper.CreateMap<Game, GameLightDto>()
                .ForMember(r => r.GameTasksCount, r => r.MapFrom(e => e.GameTasks.Count))
                .ForMember(r => r.LocationName, r => r.MapFrom(e => e.Location.DisplayName))
                .ForMember(r => r.GameStatusName, r => r.MapFrom(e => e.GameStatus.Name))
                .ForMember(r => r.GameImageName, r => r.MapFrom(e => e.GameImageName.IsNullOrEmpty() ? null :
                    String.Format("{0}{1}", CityQuestConsts.GameImagesStorePathForClient, e.GameImageName)));

            AutoMapper.Mapper.CreateMap<GameTask, GameTaskLightDto>()
                .ForMember(r => r.Tips, r => r.MapFrom(e => e.Tips.OrderBy(k => k.Order)))
                .ForMember(r => r.Conditions, r => r.MapFrom(e => e.Conditions.OrderBy(k => k.Order)));

            AutoMapper.Mapper.CreateMap<GameTaskType, GameTaskTypeLightDto>();

            AutoMapper.Mapper.CreateMap<Condition, ConditionLightDto>();

            AutoMapper.Mapper.CreateMap<ConditionType, ConditionTypeLightDto>();

            AutoMapper.Mapper.CreateMap<Tip, TipLightDto>();

            #endregion

            #region GameStatistics entities mapping

            AutoMapper.Mapper.CreateMap<TeamGameTaskStatistic, TeamGameTaskStatisticDto>()
                .ForMember(r => r.GameTaskName, r => r.MapFrom(e => e.GameTask.Name))
                .ForMember(r => r.GameTaskOrder, r => r.MapFrom(e => e.GameTask.Order))
                .ForMember(r => r.TeamName, r => r.MapFrom(e => e.Team.Name));

            #endregion

            AutoMapper.Mapper.CreateMap<Division, DivisionDto>()
                .ForMember(r => r.TeamsCount, r => r.MapFrom(e => e.Teams.Count))
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<PlayerCareer, PlayerCareerDto>()
                .ForMember(r => r.FullUserName, r => r.MapFrom(e => e.User.FullUserName))
                .ForMember(r => r.TeamName, r => r.MapFrom(e => e.Team.Name))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<Team, TeamDto>()
                .ForMember(r => r.CaptainUserFullName, r => r.MapFrom(e => e.Captain != null ? e.Captain.User.FullUserName : "-"))
                .ForMember(r => r.CaptainUserId, r => r.MapFrom(e => e.Captain != null ? (long?)e.Captain.User.Id : null))
                .ForMember(r => r.Players, r => r.MapFrom(e => e.CurrentPlayers.ToList()))
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<Game, GameDto>()
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ForMember(r => r.GameTasks, r => r.MapFrom(e => e.GameTasks.OrderBy(k => k.Order)))
                .ForMember(r => r.LocationName, r => r.MapFrom(e => e.Location.DisplayName))
                .ForMember(r => r.GameImageName, r => r.MapFrom(e => e.GameImageName.IsNullOrEmpty() ? null :
                    String.Format("{0}{1}", CityQuestConsts.GameImagesStorePathForClient, e.GameImageName)))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<GameStatus, GameStatusDto>()
                .ForMember(r => r.NextAllowedStatuses, r => r.MapFrom(e => e.GetNextAllowedGameStatusNames))
                //.ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                //.ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<GameTask, GameTaskDto>()
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ForMember(r => r.Tips, r => r.MapFrom(e => e.Tips.OrderBy(k => k.Order)))
                .ForMember(r => r.Conditions, r => r.MapFrom(e => e.Conditions.OrderBy(k => k.Order)))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<GameTaskType, GameTaskTypeDto>()
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<Condition, ConditionDto>()
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<ConditionType, ConditionTypeDto>()
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<Tip, TipDto>()
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<Location, LocationDto>()
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<TeamRequest, TeamRequestDto>()
                .ForMember(r => r.InvitedFullUserName, r => r.MapFrom(e => e.InvitedUser.FullUserName))
                .ForMember(r => r.TeamName, r => r.MapFrom(e => e.Team.Name))
                .ReverseMap();

            #endregion
        }
    }
}