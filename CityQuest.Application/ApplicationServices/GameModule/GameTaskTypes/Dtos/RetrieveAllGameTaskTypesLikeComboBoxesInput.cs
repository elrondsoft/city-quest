using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.GameTaskTypes.Dtos
{
    public class RetrieveAllGameTaskTypesLikeComboBoxesInput : IInputDto
    {
        public bool? IsActive { get; set; }
    }
}
