using AndersonDiego.Infra.Constants;
using AndersonDiego.Infra.Handlers;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndersonDiego
{
    public partial class Startup
    {
        static Startup()
        {
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(5),
                AllowInsecureHttp = true,
                TokenEndpointPath = new Microsoft.Owin.PathString(Constant.Path_Api_GENERATE_TOKEN),
                Provider = new OAuthProvider()
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void Configuration(IAppBuilder pAppBuilder)
        {
            pAppBuilder.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}