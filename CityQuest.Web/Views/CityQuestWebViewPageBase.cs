using Abp.Web.Mvc.Views;

namespace CityQuest.Web.Views
{
    public abstract class CityQuestWebViewPageBase : CityQuestWebViewPageBase<dynamic>
    {

    }

    public abstract class CityQuestWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected CityQuestWebViewPageBase()
        {
            LocalizationSourceName = CityQuestConsts.LocalizationSourceName;
        }
    }
}