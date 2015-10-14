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
            AutoMapper.Mapper.CreateMap<Division, DivisionDto>().ReverseMap();

        }
    }
}