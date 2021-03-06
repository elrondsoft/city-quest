﻿using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Games.Dtos
{
    public class ChangeGameStatusInput: IInputDto
    {
        public long GameId { get; set; }
        public long? NewGameStatusId { get; set; }
        public string NewGameStatusName { get; set; }
    }
}
