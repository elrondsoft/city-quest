using CityQuest.Services.FileGeneratorService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Services.FileGeneratorService
{
    public class FileGeneratorService<T> : IFileGeneratorService<T>
    {
        public T GenerateFile(GenerateFileInput input)
        {
            throw new NotImplementedException();
        }
    }
}
