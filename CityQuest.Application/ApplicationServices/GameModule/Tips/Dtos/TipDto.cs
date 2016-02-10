using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using CityQuest.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Tips.Dtos
{
    public class TipDto : FullAuditedEntityDto<long>, IHasOrder
    {
        #region Relations

        public long GameTaskId { get; set; }

        public string LastModifierUserFullName { get; set; }
        public string CreatorUserFullName { get; set; }

        #endregion

        public string Name { get; set; }
        public string TipText { get; set; }
        public int Order { get; set; }

        #region Ctors

        public TipDto() { }

        #endregion
    }
}
