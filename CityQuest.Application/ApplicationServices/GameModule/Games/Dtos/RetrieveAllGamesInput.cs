using CityQuest.ApplicationServices.Shared.Dtos.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Games.Dtos
{
    public class RetrieveAllGamesInput : RetrieveAllInput
    {
        public bool? IsActive { get; set; }
        public string Name { get; set; }
        public IList<long> GameIds { get; set; }
    }
}