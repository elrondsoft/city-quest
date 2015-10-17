using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using CityQuest.ApplicationServices.GameModule.Divisions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Teams.Dtos
{
    public class TeamDto : FullAuditedEntityDto<long>, IPassivable
    {
        #region Relations

        public string CaptainName { get; set; }
        //public UserDto Captain { get; set; }

        //public string DivisionName { get; set; }
        public virtual DivisionDto Division { get; set; }

        //public virtual IList<UserDto> Players { get; set; }

        public string LastModifierUserFullName { get; set; }
        public string CreatorUserFullName { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public string Slogan { get; set; }

        public bool IsActive { get; set; }
    }
}
