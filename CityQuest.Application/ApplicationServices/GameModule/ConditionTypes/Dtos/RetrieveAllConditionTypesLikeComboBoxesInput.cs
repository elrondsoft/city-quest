using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.ConditionTypes.Dtos
{
    public class RetrieveAllConditionTypesLikeComboBoxesInput : IInputDto
    {
        public bool? IsActive { get; set; }
    }
}
