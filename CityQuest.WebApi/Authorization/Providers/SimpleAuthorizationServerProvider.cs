using Abp.Dependency;
using Abp.Domain.Uow;
using Castle.Core.Logging;
using CityQuest.Entities.MainModule.Authorization.UserServices;
using CityQuest.Entities.MainModule.Users;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Authorization.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider, ISingletonDependency
    {
        UserManager _userManager;
        IUnitOfWorkManager _uowManager;
        public ILogger Logger { get; set; }
        public SimpleAuthorizationServerProvider(UserManager userManager, IUnitOfWorkManager uowManager)
        {
            this._userManager = userManager;
            this._uowManager = uowManager;
        }

        public async override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            User client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return;
            }

            long digitClientId;
            if (long.TryParse(context.ClientId, out digitClientId))
            {
                context.SetError("invalid_clientId", string.Format("Client id '{0}' must be digital", context.ClientId));
                Logger.Warn(string.Format("ValidateClientAuthentication Error: {0} Description: {1}", context.Error, context.ErrorDescription));
                return;
            }

            try
            {
                using (var uow = _uowManager.Begin())
                {
                    //_uowManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                    client = await _userManager.FindByIdAsync(digitClientId);
                    //_uowManager.Current.EnableFilter(AbpDataFilters.MayHaveTenant);
                }
            }
            catch (Exception ex)
            {
                context.SetError("Getting user exception", ex.Message);
                Logger.Error(string.Format("ValidateClientAuthentication Error: {0} Description: {1}", context.Error, context.ErrorDescription));
                return;
            }

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                Logger.Warn(string.Format("ValidateClientAuthentication Error: {0} Description: {1}", context.Error, context.ErrorDescription));
                return;
            }

            //context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            //context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
            context.Validated();
            return;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            User user;
            try
            {
                using (var uow = _uowManager.Begin())
                {
                    //_uowManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
                    user = await _userManager.FindAsync(context.UserName, context.Password);
                    //_uowManager.Current.EnableFilter(AbpDataFilters.MayHaveTenant);
                }
            }
            catch (Exception ex)
            {
                context.SetError("Getting user exception", ex.Message);
                Logger.Error(string.Format("GrantResourceOwnerCredentials Error: {0} Description: {1}", context.Error, context.ErrorDescription), ex);
                return;
            }
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                context.Response.Headers.Add(Consts.TempAuthorizationErrorReasonHeader, new[] { context.Error });//would be intercept by owin midlevare
                Logger.Warn(string.Format("GrantResourceOwnerCredentials Error: {0} Description: {1}", context.Error, context.ErrorDescription));
                return;
            }
            var identity = await _userManager.CreateIdentityAsync(user, context.Options.AuthenticationType);
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { 
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    { 
                        "userName", context.UserName
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                Logger.Warn(string.Format("GrantRefreshToken Error: {0} Description: {1}", context.Error, context.ErrorDescription));
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}