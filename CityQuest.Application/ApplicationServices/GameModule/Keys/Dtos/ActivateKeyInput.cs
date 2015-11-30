using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Keys.Dtos
{
    public class ActivateKeyInput : IInputDto
    {
        public string Key { get; set; }
    }
}
