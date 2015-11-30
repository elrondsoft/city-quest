using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Keys.Dtos
{
    public class GenerateKeysForGameInput : IInputDto
    {
        public int Count { get; set; }
        public long GameId { get; set; }
    }
}
