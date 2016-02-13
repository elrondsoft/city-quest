using CityQuest.Services.FileGeneratorService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Services.FileGeneratorService
{
    public interface IFileGeneratorService<T>
    {
        T GenerateFile(GenerateFileInput input);
    }
}
