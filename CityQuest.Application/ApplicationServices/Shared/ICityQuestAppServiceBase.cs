using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.Shared
{
    public interface ICityQuestAppServiceBase<TEntityDto, TPrimaryKey> : IApplicationService
        where TEntityDto : class, IEntityDto<TPrimaryKey>
    {
        RetrieveAllPagedResultOutput<TEntityDto, TPrimaryKey> RetrieveAllPagedResult(RetrieveAllPagedResultInput input);
        RetrieveAllOutput<TEntityDto, TPrimaryKey> RetrieveAll(RetrieveAllInput input);
        RetrieveOutput<TEntityDto, TPrimaryKey> Retrieve(RetrieveInput input);
        CreateOutput<TEntityDto, TPrimaryKey> Create(CreateInput<TEntityDto, TPrimaryKey> input);
        UpdateOutput<TEntityDto, TPrimaryKey> Update(UpdateInput<TEntityDto, TPrimaryKey> input);
        DeleteOutput<TPrimaryKey> Delete(DeleteInput<TPrimaryKey> input);
    }
}
