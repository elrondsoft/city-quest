using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.TeamRequests.Dtos
{
    public class CreateTeamRequestInput: IInputDto
    {
        public long InvitedUserId { get; set; }
        //better to get from session
        //public long UserInviterId { get; set; }
    }
}
