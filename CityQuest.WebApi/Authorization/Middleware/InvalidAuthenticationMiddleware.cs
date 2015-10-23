using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityQuest.Authorization.Middleware
{
    public class InvalidAuthenticationMiddleware : OwinMiddleware
    {
        public InvalidAuthenticationMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            if (context.Response.StatusCode == 400 && context.Response.Headers.ContainsKey(Consts.TempAuthorizationErrorReasonHeader))
            {
                var reason = context.Response.Headers.Get(Consts.TempAuthorizationErrorReasonHeader);
                if (reason == "invalid_grant")
                {
                    context.Response.Headers.Remove(Consts.TempAuthorizationErrorReasonHeader);
                    context.Response.StatusCode = 401;
                }
            }
        }
    }
}