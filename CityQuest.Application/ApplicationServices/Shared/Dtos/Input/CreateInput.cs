using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.Shared.Dtos.Input
{
    public class CreateInput<TEntityDto, TPrimaryKey> : IInputDto
        where TEntityDto : class, IEntityDto<TPrimaryKey>
    {
        public TEntityDto EntityDto { get; set; }
    }
}
