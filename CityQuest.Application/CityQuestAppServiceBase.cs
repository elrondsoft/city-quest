using Abp.Application.Services;

namespace CityQuest
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class CityQuestAppServiceBase : ApplicationService
    {
        protected CityQuestAppServiceBase()
        {
            LocalizationSourceName = CityQuestConsts.LocalizationSourceName;
        }
    }
}