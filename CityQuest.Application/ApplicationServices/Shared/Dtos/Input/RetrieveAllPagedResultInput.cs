using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.Shared.Dtos.Input
{
    public class RetrieveAllPagedResultInput : IInputDto, IPagedResultRequest
    {
        public int SkipCount { get; set; }

        public int MaxResultCount { get; set; }
    }
}
