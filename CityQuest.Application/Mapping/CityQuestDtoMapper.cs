using CityQuest.ApplicationServices.GameModule.Divisions.Dtos;
using CityQuest.ApplicationServices.GameModule.Teams.Dtos;
using CityQuest.ApplicationServices.MainModule.Roles.Dto;
using CityQuest.ApplicationServices.MainModule.Users.Dto;
using CityQuest.Entities.GameModule.Divisions;
using CityQuest.Entities.GameModule.Teams;
using CityQuest.Entities.MainModule.Authorization.UserRoles;
using CityQuest.Entities.MainModule.Roles;
using CityQuest.Entities.MainModule.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Mapping
{
    public static class CityQuestDtoMapper
    {
        public static void Map()
        {
            #region Main module mapping

            AutoMapper.Mapper.CreateMap<Role, RoleDto>()
                .ReverseMap()
                .AfterMap((s, d) =>
                {
                    foreach (var item in d.Permissions)
                    {
                        item.Role = d;
                        item.RoleId = d.Id;
                    }
                });

            AutoMapper.Mapper.CreateMap<User, UserDto>()
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
                .ForMember(r => r.CaptainName, r => r.MapFrom(e => e.Captain.FullUserName))
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullUserName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullUserName))
                .ReverseMap();

            #endregion
        }
    }
}