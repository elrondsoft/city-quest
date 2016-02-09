using Abp.Domain.Entities.Auditing;
using CityQuest.Entities.MainModule.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityQuest.Entities.GameModule.Teams.TeamRequests
{
    public class TeamRequest : FullAuditedEntity<long, User>
    {
        #region Relations

        public long InvitedUserId { get; set; }
        public virtual User InvitedUser { get; set; }

        //public long InviterUserId { get; set; }
        //public virtual User InviterUser { get; set; }

        public long TeamId { get; set; }
        public virtual Team Team { get; set; }

        #endregion

        public bool? InvitedUserResponse { get; set; }
        public DateTime? InvitedUserResponseDateTime { get; set; }

        public bool? DeclinedByInviter { get; set; }
        public DateTime? DeclinedByInviterDateTime { get; set; }

        #region Ctors

        public TeamRequest() { }

        #endregion
    }
}