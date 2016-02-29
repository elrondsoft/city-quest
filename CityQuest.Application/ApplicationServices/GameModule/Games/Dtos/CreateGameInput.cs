using Abp.Runtime.Validation;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Games.Dtos
{
    public class CreateGameInput : CreateInput<GameDto, long>, IShouldNormalize
    {
        public string ImageData { get; set; }

        public void Normalize()
        {
            Entity.GameStatusId = 0;
        }
    }
}
