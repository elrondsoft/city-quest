using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.TeamRequests.Dtos
{
    public class TeamRequestDto : FullAuditedEntityDto<long>
    {
        #region Relations

        public long InvitedUserId { get; set; }
        public string InvitedFullUserName { get; set; }

        public long TeamId { get; set; }
        public string TeamName { get; set; }

        #endregion

        public bool? InvitedUserResponse { get; set; }
        public DateTime? InvitedUserResponseDateTime { get; set; }

        public bool? DeclinedByInviter { get; set; }
        public DateTime? DeclinedByInviterDateTime { get; set; }

        #region Ctors

        public TeamRequestDto() { }

        #endregion
    }
}
