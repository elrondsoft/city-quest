using Abp.Application.Services.Dto;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using CityQuest.CityQuestConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Permissions
{
    public class PermissionAppService : IPermissionAppService
    {
        public RetrieveAllLikeComboBoxesOutput RetrieveAllLikeComboBoxes(RetrieveAllLikeComboBoxesInput input)
        {
            IReadOnlyList<ComboboxItemDto> permissions = CityQuestPermissionNames.GetAllPermission()
                .Select(r => new ComboboxItemDto() { Value = r, DisplayText = r })
                .ToList();
            return new RetrieveAllLikeComboBoxesOutput()
            {
                Items = permissions
            };
        }
    }
}
