using Abp.Application.Services;
using CityQuest.ApplicationServices.MainModule.Roles.Dto;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        RetrieveAllPagedResultOutput<RoleDto, long> RetrieveAllPagedResult(RetrieveAllRolesPagedResultInput input);

        RetrieveAllRolesLikeComboBoxesOutput RetrieveAllRolesLikeComboBoxes(RetrieveAllRolesLikeComboBoxesInput input);

        RetrieveAllOutput<RoleDto, long> RetrieveAll(RetrieveAllRoleInput input);

        RetrieveOutput<RoleDto, long> Retrieve(RetrieveRoleInput input);

        CreateOutput<RoleDto, long> Create(CreateInput<RoleDto, long> input);

        UpdateOutput<RoleDto, long> Update(UpdateInput<RoleDto, long> input);

        DeleteOutput<long> Delete(DeleteInput<long> input);
    }
}
