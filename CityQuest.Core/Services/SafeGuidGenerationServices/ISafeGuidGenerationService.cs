using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Services.SafeGuidGenerationServices
{
    /// <summary>
    /// Is used to generate unic guids for CityQuest's Entities
    /// </summary>
    public interface ISafeGuidGenerationService
    {
        /// <summary>
        /// Is used to generate unic guids for CityQuest's Entities
        /// </summary>
        /// <param name="count">count of new Guids</param>
        /// <returns>new Guids like list of strings</returns>
        IList<string> Generate(int count);
    }
}
