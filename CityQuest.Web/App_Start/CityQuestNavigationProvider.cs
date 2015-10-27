using Abp.Application.Navigation;
using Abp.Localization;

namespace CityQuest.Web
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See .cshtml and .js files under App/Main/views/layout/header to know how to render menu.
    /// </summary>
    public class CityQuestNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        "Home",
                        new LocalizableString("HomePage", CityQuestConsts.LocalizationSourceName),
                        url: "#/",
                        icon: "fa fa-home"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Divisions",
                        new LocalizableString("Divisions", CityQuestConsts.LocalizationSourceName),
                        url: "#/divisions",
                        icon: "fa fa-list-alt"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Teams",
                        new LocalizableString("Teams", CityQuestConsts.LocalizationSourceName),
                        url: "#/teams",
                        icon: "fa fa-list-alt"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Locations",
                        new LocalizableString("Locations", CityQuestConsts.LocalizationSourceName),
                        url: "#/locations",
                        icon: "fa fa-list-alt"
                        )
                );
        }
    }
}
