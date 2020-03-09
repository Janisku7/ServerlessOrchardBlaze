using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using BlazeOrchardShared;
using OrchardCore.Users.Models;
using ServerlessOrchard.Auth;
using OrchardCore.Lucene.QueryProviders;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Polly;

namespace ServerlessOrchard
{
    public  class UserControlFunc
    {
        public readonly UserInfo LoggedOutUser;
        private  readonly User _user;
        private  HttpContext context;

        public UserControlFunc()
        {
            LoggedOutUser = new UserInfo { IsAuthenticated = false };
            
        }

        [FunctionName(nameof(SignIn))]
        public  async Task SignIn(
            [HttpTrigger(AuthorizationLevel.User, "get", Route = "user/signin")] HttpRequest req, string redirectUrl,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (string.IsNullOrEmpty(redirectUrl)) redirectUrl = "/";

            await context.ChallengeAsync(TwitterDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = redirectUrl });

            
        }
        [FunctionName(nameof(GetUser))]
        public async Task<UserInfo> GetUser([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user")] HttpRequest req, User user, ILogger log)
        {
            return context.User.Identity.IsAuthenticated? new UserInfo { Name = user.UserName, IsAuthenticated = user.IsEnabled } : LoggedOutUser;
        }
        [FunctionName(nameof(SignOut))]
        public  async Task<IActionResult> SignOut([HttpTrigger(AuthorizationLevel.User,"get",Route = "user/signout")]HttpRequest req, ILogger logger)
        {

            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new RedirectResult("~/");
        } 
         async Task<bool> ValidateUser ( HttpRequest req)
        {
            var authInfo = await AuthInfoExtensions.GetAuthInfoAsync(req);

            if (authInfo.ProviderName.ToLowerInvariant() != "twitter")
                return false;
            
            var user = authInfo.UserId.ToLowerInvariant();
            return user == _user.UserName;
        }
    }
}
