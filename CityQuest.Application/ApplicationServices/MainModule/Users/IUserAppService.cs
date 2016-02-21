using Abp.Application.Services;
using CityQuest.ApplicationServices.MainModule.Users.Dto;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.MainModule.Users
{
    public interface IUserAppService : IApplicationService
    {
        RetrieveAllPagedResultOutput<UserDto, long> RetrieveAllPagedResult(RetrieveAllUsersPagedResultInput input);

        RetrieveAllUsersLikeComboBoxesOutput RetrieveAllUsersLikeComboBoxes(RetrieveAllUsersLikeComboBoxesInput input);

        RetrieveAllOutput<UserDto, long> RetrieveAll(RetrieveAllUsersInput input);

        RetrieveOutput<UserDto, long> Retrieve(RetrieveUserInput input);

        RetrieveOutput<UserDto, long> RetrieveCurrentUserInfo();

        CreateOutput<UserDto, long> Create(CreateInput<UserDto, long> input);

        UpdateOutput<UserDto, long> Update(UpdateInput<UserDto, long> input);

        ChangePasswordOutput ChangePassword(ChangePasswordInput input);

        UpdatePublicUserFieldsOutput UpdatePublicUserFields(UpdatePublicUserFieldsInput input);

        DeleteOutput<long> Delete(DeleteInput<long> input);
    }
}
