using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using CityQuest.ApplicationServices.GameModule.Teams.Dtos;
using CityQuest.ApplicationServices.MainModule.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.PlayerCareers.Dtos
{
    public class PlayerCareerDto : FullAuditedEntityDto<long>, IPassivable
    {
        #region Relations

        public long UserId { get; set; }
        //public virtual UserDto User { get; set; }
        public string FullUserName { get; set; }

        public long TeamId { get; set; }
        //public virtual TeamDto Team { get; set; }
        public string TeamName { get; set; }

        #endregion

        public bool IsCaptain { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public bool IsActive { get; set; }
    }
}
