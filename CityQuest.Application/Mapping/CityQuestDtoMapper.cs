using CityQuest.ApplicationServices.GameModule.Divisions.Dtos;
using CityQuest.Entities.GameModule.Divisions;
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
            AutoMapper.Mapper.CreateMap<Division, DivisionDto>()
                .ForMember(r => r.TeamsCount, r => r.MapFrom(e => e.Teams.Count))
                .ForMember(r => r.CreatorUserFullName, r => r.MapFrom(e => e.CreatorUser.FullName))
                .ForMember(r => r.LastModifierUserFullName, r => r.MapFrom(e => e.LastModifierUser.FullName))
                .ReverseMap();

        }
    }
}