using Abp.Dependency;
using CityQuest.Authorization.Middleware;
using CityQuest.Authorization.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

[assembly: OwinStartup(typeof(CityQuest.Web.App_Start.Startup))]

namespace CityQuest.Web.App_Start
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            //config.Filters.Remove(config.Filters.First(r => r.Instance.GetType() == typeof(Abp.WebApi.Controllers.Filters.AbpExceptionFilterAttribute)).Instance);
            //config.Filters.Add(IocManager.Instance.Resolve<TestAbpExceptionFilterAttribute>());
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            ConfigureOAuth(app);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(7),
                Provider = IocManager.Instance.Resolve<SimpleAuthorizationServerProvider>(),
            };
            // Token Generation
            app.Use<InvalidAuthenticationMiddleware>();
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
    public class TestAbpExceptionFilterAttribute : Abp.WebApi.Controllers.Filters.AbpExceptionFilterAttribute
    {
        public TestAbpExceptionFilterAttribute()
            : base()
        {

        }
        public override void OnException(System.Web.Http.Filters.HttpActionExecutedContext context)
        {
            base.OnException(context);
        }
    }
}
