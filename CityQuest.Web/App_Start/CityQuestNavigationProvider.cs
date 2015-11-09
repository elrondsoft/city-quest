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
                ).AddItem(
                    new MenuItemDefinition(
                        "LocationsMenuItem",
                        new LocalizableString("LocationsMenuItem", CityQuestConsts.LocalizationSourceName),
                        url: "#/locations",
                        icon: "fa fa-list-alt"
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        "AdministrationMenuItem",
                        new LocalizableString("AdministrationMenuItem", CityQuestConsts.LocalizationSourceName),
                        icon: "fa fa-wrench"
                        ).AddItem(new MenuItemDefinition(
                            "UsersMenuItem",
                            new LocalizableString("UsersMenuItem", CityQuestConsts.LocalizationSourceName),
                            url: "#/users",
                            icon: "fa fa-users"
                        )).AddItem(new MenuItemDefinition(
                            "RolesMenuItem",
                            new LocalizableString("RolesMenuItem", CityQuestConsts.LocalizationSourceName),
                            url: "#/roles",
                            icon: "fa fa-gavel"
                        )).AddItem(new MenuItemDefinition(
                            "DivisionsMenuItem",
                            new LocalizableString("DivisionsMenuItem", CityQuestConsts.LocalizationSourceName),
                            url: "#/divisions",
                            icon: "fa fa-list-alt"
                        )).AddItem(new MenuItemDefinition(
                            "TeamsMenuItem",
                            new LocalizableString("TeamsMenuItem", CityQuestConsts.LocalizationSourceName),
                            url: "#/teams",
                            icon: "fa fa-list-alt"
                        )).AddItem(new MenuItemDefinition(
                            "GamesMenuItem",
                            new LocalizableString("GamesMenuItem", CityQuestConsts.LocalizationSourceName),
                            url: "#/games",
                            icon: "fa fa-list-alt"
                        ))
                );
        }
    }
}
