using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.Shared.Dtos.Input
{
    public class UpdateInput<TEntityDto, TPrimaryKey> : IInputDto
        where TEntityDto : class, IEntityDto<TPrimaryKey>
    {
        public TEntityDto Entity { get; set; }
    }
}
