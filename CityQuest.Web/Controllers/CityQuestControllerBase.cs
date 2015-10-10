using Abp.Web.Mvc.Controllers;

namespace CityQuest.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class CityQuestControllerBase : AbpController
    {
        protected CityQuestControllerBase()
        {
            LocalizationSourceName = CityQuestConsts.LocalizationSourceName;
        }
    }
}