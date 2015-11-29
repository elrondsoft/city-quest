using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Exceptions
{
    /// <summary>
    /// Is used like exception for SafeGuidGenerationService
    /// </summary>
    public class SafeGuidGenerationServiceException : UserFriendlyException
    {
        public SafeGuidGenerationServiceException()
            : base("Guid generation service failed!", 
            "Problems with generating new Guids. Please try again or contact your system administrator.") { }
    }
}
