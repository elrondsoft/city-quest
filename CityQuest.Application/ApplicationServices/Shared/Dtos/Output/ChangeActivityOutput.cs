using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.Shared.Dtos.Output
{
    public class ChangeActivityOutput<TEntityDto, TPrimaryKey> : IOutputDto
        where TEntityDto : class, IEntityDto<TPrimaryKey>
    {
        public TEntityDto Entity { get; set; }
    }
}
