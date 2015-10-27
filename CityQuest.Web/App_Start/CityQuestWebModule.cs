using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using Abp.Web.Authorization;
using CityQuest.Web.Authorization;

namespace CityQuest.Web
{
    [DependsOn(typeof(CityQuestDataModule), typeof(CityQuestApplicationModule), typeof(CityQuestWebApiModule))]
    public class CityQuestWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.IocContainer.Register(Component.For<IAuthorizationScriptManager>().ImplementedBy<CityQuestAuthorizationScriptManager>().LifestyleTransient());


            //Add/remove languages for your application
            Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england", true));
            Configuration.Localization.Languages.Add(new LanguageInfo("ru", "Русский", "famfamfam-flag-ru"));
            Configuration.Localization.Languages.Add(new LanguageInfo("uk", "Українська", "famfamfam-flag-ua"));

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    CityQuestConsts.LocalizationSourceName,
                    new XmlFileLocalizationDictionaryProvider(
                        HttpContext.Current.Server.MapPath("~/Localization/CityQuest")
                        )
                    )
                );

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<CityQuestNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
