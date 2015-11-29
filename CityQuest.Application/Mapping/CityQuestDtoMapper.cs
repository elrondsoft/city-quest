using CityQuest.ApplicationServices.GameModule.Conditions.Dtos;
using CityQuest.ApplicationServices.GameModule.ConditionTypes.Dtos;
using CityQuest.ApplicationServices.GameModule.Divisions.Dtos;
using CityQuest.ApplicationServices.GameModule.Games.Dtos;
using CityQuest.ApplicationServices.GameModule.GameTasks.Dtos;
using CityQuest.ApplicationServices.GameModule.GameTaskTypes.Dtos;
using CityQuest.ApplicationServices.GameModule.Locations.Dtos;
using CityQuest.ApplicationServices.GameModule.Teams.Dtos;
using CityQuest.ApplicationServices.GameModule.Tips.Dtos;
using CityQuest.ApplicationServices.MainModule.Permissions.Dto;
using CityQuest.ApplicationServices.MainModule.Roles.Dto;
using CityQuest.ApplicationServices.MainModule.Users.Dto;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Entities.GameModule.Games;
using CityQuest.Entities.GameModule.Games.GameTasks;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions;
using CityQuest.Entities.GameModule.Games.GameTasks.Conditions.ConditionTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.GameTaskTypes;
using CityQuest.Entities.GameModule.Games.GameTasks.Tips;
using CityQuest.Entities.GameModule.Locations;
using CityQuest.Entities.GameModule.Teams;
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

            AutoMapper.Mapper.CreateMap<Division, DivisionDto>()
                .ForMember(r => r.TeamsCount, r => r.MapFrom(e => e.Teams.Count))
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<Team, TeamDto>()
                .ForMember(r => r.Captain, r => r.MapFrom(e => e.Captain))
                .ForMember(r => r.Players, r => r.MapFrom(e => e.Players))
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<Game, GameDto>()
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ForMember(r => r.GameTasks, r => r.MapFrom(e => e.GameTasks.OrderBy(k => k.Order)))
                .ForMember(r => r.LocationName, r => r.MapFrom(e => e.Location.DisplayName))
                .ReverseMap()
                #warning TODO normanl datetime for game
                .ForMember(r => r.StartDate, r => r.MapFrom(e => DateTime.Now));


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

            #endregion
        }
    }
}