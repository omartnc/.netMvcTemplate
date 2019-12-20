using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using EventManagementSystem.Service;
using Microsoft.Owin.Security.OAuth;

namespace EventManagementSystem.WebApi.App_Start
{
    public class ProviderAuthorization : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //Domainler arası etkileşim ve kaynak paylaşımını sağlayan ve bir domainin bir başka domainin kaynağını kullanmasını sağlayan CORS ayarlarını set ediyoruz.
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });


            //Kullanıcının access_token alabilmesi için gerekli validation işlemlerini yapıyoruz.
            var authUser = new UserService().Get(context.UserName, context.Password);
            if (authUser == null)
            {
                context.SetError("hata", "Kullanıcı adı veya şifre hatalı.");
            }
            else
            {
                ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("UserId", authUser.Id.ToString()));

                context.Validated(identity);
            }
        }
    }
}