﻿using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GameTaskTypes.Dtos
{
    public class RetrieveAllGameTaskTypesPagedResultInput : RetrieveAllPagedResultInput
    {
        public bool? IsActive { get; set; }
        public string Name { get; set; }
        public IList<long> GameTaskTypeIds { get; set; }
    }
}
