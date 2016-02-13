using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Services.FileGeneratorService.Data
{
    public class GenerateFileInput
    {
        //nothing here yet
    }

    public class PDFInputData
    {
        public string GamePicturePath { get; set; }
        public string GameName { get; set; }
        public IList<string> Keys { get; set; }
        public PDFInputData(string gamePicturePath, string gameName, IList<string> keys)
        {
            GamePicturePath = gamePicturePath;
            GameName = gameName;
            Keys = keys;
        }
    }
}
