using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Divisions.Dtos
{
    public class DivisionDto : FullAuditedEntityDto<long>, IPassivable
    {
        #region Relations

        public string LastModifierUserFullName { get; set; }
        public string CreatorUserFullName { get; set; }
        public int TeamsCount { get; set; }

        #endregion

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        #region Ctors

        public DivisionDto() { }

        #endregion
    }
}
