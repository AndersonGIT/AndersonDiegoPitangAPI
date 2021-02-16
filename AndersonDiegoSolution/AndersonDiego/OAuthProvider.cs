using AndersonDiego.Infra.Constants;
using AndersonDiego.Infra.Handlers;
using AndersonDiego.Infra.Repositories;
using AndersonDiego.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AndersonDiego
{
    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        private HandlerLogin _HandlerLogin;
        public OAuthProvider()
        {
            _HandlerLogin = new HandlerLogin(new ResponseError(), new UserRepository());
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext pContext)
        {
            return Task.Run(() => {
                string userName = pContext.UserName;
                string password = pContext.Password;

                object objectLogin = _HandlerLogin.Login(userName, password);

                if (objectLogin is ResponseError loginError)
                {
                    if (loginError.ContainsError)
                        pContext.SetError(loginError.Message);
                }
                else if (objectLogin is User user)
                {
                    List<Claim> claims = new List<Claim>();

                    if(user?.UserId >= 1)
                    {
                        claims.Add(new Claim(Constant.CLAIM_USER_OBJECT, JsonConvert.SerializeObject(user)));

                        ClaimsIdentity OAuthIdentity = new ClaimsIdentity(claims, Startup.OAuthOptions.AuthenticationType);

                        AuthenticationTicket OAuthTicket = new AuthenticationTicket(OAuthIdentity, new AuthenticationProperties());

                        pContext.Validated(OAuthTicket);
                    }
                    else
                        pContext.SetError("Login Error.");
                }
            });
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext pContext)
        {
            if (pContext.ClientId == null)
                pContext.Validated();

            return Task.FromResult<object>(null);
        }
    }
}