namespace aspnet4_sample1
{
    using System;
    using System.Threading.Tasks;
    using Auth0.AuthenticationApi;
    using Auth0.AuthenticationApi.Models;
    using Auth0.AspNet;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Services;
    using System.Linq;
    using System.Web;
    using System.Security.Claims;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using System.Web.Mvc;
    using Microsoft.Owin.Host.SystemWeb;


    public class LoginCallback : HttpTaskAsyncHandler
    {
        public override async Task ProcessRequestAsync(HttpContext context)
        {
            AuthenticationApiClient client = new AuthenticationApiClient(
                new Uri(string.Format("https://{0}", ConfigurationManager.AppSettings["auth0:Domain"])));

            string contextCode = context.Request.QueryString["code"];
            var token = await client.GetTokenAsync(new AuthorizationCodeTokenRequest
            {
                ClientId = ConfigurationManager.AppSettings["auth0:ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["auth0:ClientSecret"],
                Code = contextCode,
                RedirectUri = context.Request.Url.ToString()
            });

            UserInfo profile = await client.GetUserInfoAsync(token.AccessToken);

            //var user = new List<KeyValuePair<string, object>>
            //{
            //    new KeyValuePair<string, object>("name", profile.FullName ?? profile.PreferredUsername ?? profile.Email),
            //    new KeyValuePair<string, object>("email", profile.Email),
            //    new KeyValuePair<string, object>("family_name", profile.LastName),
            //    new KeyValuePair<string, object>("given_name", profile.FirstName),
            //    new KeyValuePair<string, object>("nickname", profile.NickName),
            //    new KeyValuePair<string, object>("picture", profile.Picture),
            //    new KeyValuePair<string, object>("user_id", profile.UserId),
            //    new KeyValuePair<string, object>("id_token", token.IdToken),
            //    new KeyValuePair<string, object>("access_token", token.AccessToken),
            //    new KeyValuePair<string, object>("refresh_token", token.RefreshToken),
            //    new KeyValuePair<string, object>("email_verified", profile.EmailVerified),
            //    new KeyValuePair<string, object>("gender", profile.Gender)
            //};

            // NOTE: Uncomment the following code in order to include claims from associated identities
            //profile.Identities.ToList().ForEach(i =>
            //{
            //    user.Add(new KeyValuePair<string, object>(i.Connection + ".access_token", i.AccessToken));
            //    user.Add(new KeyValuePair<string, object>(i.Connection + ".provider", i.Provider));
            //    user.Add(new KeyValuePair<string, object>(i.Connection + ".user_id", i.UserId));
            //});

            // NOTE: uncomment this if you send roles
            // user.Add(new KeyValuePair<string, object>(ClaimTypes.Role, profile.ExtraProperties["roles"]));

            // NOTE: this will set a cookie with all the user claims that will be converted 
            //       to a ClaimsPrincipal for each request using the SessionAuthenticationModule HttpModule. 
            //       You can choose your own mechanism to keep the user authenticated (FormsAuthentication, Session, etc.)



            //FederatedAuthentication.SessionAuthenticationModule.CreateSessionCookie(user);


          var ident = new ClaimsIdentity(
           new[]
           {
                new Claim("Certificate", contextCode),

                // populate assigned user's role form your DB
                // and add each one as a claim
                //new Claim(ClaimTypes.Role,"R1"),
                //new Claim(ClaimTypes.Role,"Editor"),
           },
           DefaultAuthenticationTypes.ApplicationCookie);

           if(profile.Gender!=null)
            {
                Claim genderClaim = new Claim(ClaimTypes.Gender, profile.Gender);
                ident.AddClaim(genderClaim);
            }
           if(profile.Email!=null)
            {
                Claim emailClaim = new Claim(ClaimTypes.Email, profile.Email);
                ident.AddClaim(emailClaim);
            }
           if(profile.FullName!=null)
            {
                Claim fullnameCliam = new Claim(ClaimTypes.Name, profile.FullName);
                ident.AddClaim(fullnameCliam);
            }
           if(profile.EmailVerified!=null)
            {
                Claim emailVerifiedClaim = new Claim("EmailVerified", profile.EmailVerified.ToString());
                ident.AddClaim(emailVerifiedClaim);
            }
            else
            {
                Claim emailVerifiedClaim = new Claim("EmailVerified", "false");
                ident.AddClaim(emailVerifiedClaim);
            }
           if(token.AccessToken!=null)
            {
                Claim auth0Token = new Claim("Auth0Token", token.AccessToken);
                ident.AddClaim(auth0Token);
            }
           if(profile.UserId!=null)
            {
                Claim Auth0UserId = new Claim("Auth0UserId", profile.UserId);
                ident.AddClaim(Auth0UserId);
            }

            HttpContext.Current.GetOwinContext().Authentication.SignIn(
              new AuthenticationProperties { IsPersistent = false }, ident);

            var state = context.Request.QueryString["state"];
            //Take user to passed redirect URL
            if (state != null)
            {
                var stateValues = HttpUtility.ParseQueryString(context.Request.QueryString["state"]);
                var redirectUrl = stateValues["ru"];

                // check for open redirection
                if (redirectUrl != null && IsLocalUrl(redirectUrl))
                {
                    context.Response.Redirect(redirectUrl, true);
                }
            }

            context.Response.Redirect("/");
        }

        public bool IsReusable
        {
            get { return false; }
        }

        private bool IsLocalUrl(string url)
        {
            return !String.IsNullOrEmpty(url)
                && url.StartsWith("/")
                && !url.StartsWith("//")
                && !url.StartsWith("/\\");
        }
    }
}