using CityQuest.ApplicationServices.GameModule.Divisions.Dtos;
using CityQuest.ApplicationServices.Shared;
using CityQuest.ApplicationServices.Shared.Dtos.Input;
using CityQuest.ApplicationServices.Shared.Dtos.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.ApplicationServices.GameModule.Divisions
{
    public class DivisionAppService : IDivisionAppService
    {
        public RetrieveAllPagedResultOutput<DivisionDto, long> RetrieveAllPagedResult(RetrieveAllPagedResultInput input)
        {
            throw new NotImplementedException();
        }

        public RetrieveAllOutput<DivisionDto, long> RetrieveAll(RetrieveAllInput input)
        {
            throw new NotImplementedException();
        }

        public RetrieveOutput<DivisionDto, long> Retrieve(RetrieveInput input)
        {
            throw new NotImplementedException();
        }

        public CreateOutput<DivisionDto, long> Create(CreateInput<DivisionDto, long> input)
        {
            throw new NotImplementedException();
        }

        public UpdateOutput<DivisionDto, long> Update(UpdateInput<DivisionDto, long> input)
        {
            throw new NotImplementedException();
        }

        public DeleteOutput<long> Delete(DeleteInput<long> input)
        {
            throw new NotImplementedException();
        }
    }
}
