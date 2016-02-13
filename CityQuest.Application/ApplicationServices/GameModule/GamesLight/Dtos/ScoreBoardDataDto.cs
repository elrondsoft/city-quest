using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GamesLight.Dtos
{
    public class ScoreBoardDataDto : IDto
    {
        public int Position { get; set; }

        public long TeamId { get; set; }
        public string TeamName { get; set; }

        public int CompletedTasksCount { get; set; }
        public long TotalDuration { get; set; }
        public int Score { get; set; }

        #region Ctors

        public ScoreBoardDataDto() { }

        #endregion
    }
}
