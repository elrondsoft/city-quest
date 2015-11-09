using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Exceptions
{
    /// <summary>
    /// Is used like exception that were generated if canould not find item (entity)
    /// </summary>
    public class CityQuestItemNotFoundException : UserFriendlyException
    {
        #region ctors

        /// <summary>
        /// Is used to initialize CityQuest's item not found exception (user friendly)
        /// </summary>
        public CityQuestItemNotFoundException()
            : base(Localize(CityQuestConsts.CityQuestItemNotFoundExceptionMessageHeader)) { }

        /// <summary>
        /// Is used to initialize CityQuest's item not found exception (user friendly)
        /// </summary>
        /// <param name="message">message (would be localized)</param>
        public CityQuestItemNotFoundException(String message)
            : base(Localize(CityQuestConsts.CityQuestPolicyExceptionMessageHeader), Localize(message)) { }

        /// <summary>
        /// Is used to initialize CityQuest's item not found exception (user friendly)
        /// </summary>
        /// <param name="message">message for String.Format</param>
        /// <param name="parameter">parameter for message</param>
        public CityQuestItemNotFoundException(String message, String parameter)
            : base(Localize(CityQuestConsts.CityQuestPolicyExceptionMessageHeader), BuildMessage(message, parameter)) { }

        #endregion

        #region Helpers

        protected static string Localize(string str)
        {
            //TODO: implement this
            return str;
        }

        protected static string BuildMessage(string message, string parameter)
        {
            return String.Format(Localize(message), parameter);
        }

        #endregion
    }
}