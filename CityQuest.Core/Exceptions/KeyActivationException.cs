using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Exceptions
{
    /// <summary>
    /// Is used like exception for key activation situation
    /// </summary>
    public class KeyActivationException : UserFriendlyException
    {
        public KeyActivationException(string key)
            : base("Key activation failed!",
            String.Format("Wrong key value: \"{0}\"! Please input correct one.", key)) { }
    }
}
