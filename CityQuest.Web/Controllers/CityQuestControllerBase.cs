using Abp.Web.Mvc.Controllers;
using CityQuest.Runtime.Sessions;

namespace CityQuest.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class CityQuestControllerBase : AbpController
    {
        protected ICityQuestSession CityQuestSession
        {
            get
            {
                return AbpSession as ICityQuestSession;
            }
        }
        protected CityQuestControllerBase()
        {
            LocalizationSourceName = CityQuestConsts.LocalizationSourceName;
        }
    }
}