using Abp.Application.Services;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        RetrieveAllLikeComboBoxesOutput RetrieveAllLikeComboBoxes(RetrieveAllLikeComboBoxesInput input);
    }
}
