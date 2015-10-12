using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.Shared.Dtos.Input
{
    public class DeleteInput<TPrimaryKey> : IInputDto
    {
        public TPrimaryKey EntityId { get; set; }
    }
}
