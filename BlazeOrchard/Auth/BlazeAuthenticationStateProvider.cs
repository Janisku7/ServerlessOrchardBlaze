using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Web;
using System.Text.RegularExpressions;

namespace BlazeOrchard
{
    public class BlazeAuthenticationStateProvider : AuthenticationStateProvider 
    {
        private readonly NavigationManager NavigationManager;
        private readonly HttpClient HttpClient;
        private readonly IJSRuntime JSRuntime;

        public BlazeAuthenticationStateProvider(HttpClient httpClient, IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            HttpClient = httpClient;
            JSRuntime = jsRuntime;
            NavigationManager = navigationManager;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthToken token = await GetAuthToken();
            if (token?.AuthenticationToken != null)
            {
                HttpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token.AuthenticationToken);
                try
                {
                    var authResponse = await HttpClient.GetStringAsync(Constants.AzureFunctionAuthURL + Constants.AuthMeEndpoint);
                    await LocalStorage.SetAsync(JSRuntime, "authtoken", token);
                    var authInfo = JsonSerializer.Deserialize<List<AuthInfo>>(authResponse);
                    switch (authInfo[0].ProviderName)
                    {
                        case "twitter":
                            return await GetTwitterClaims(authInfo[0]);
                        default:
                            break;
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Unable to authenticate " + e.Message);
                    HttpClient.DefaultRequestHeaders.Remove("X-ZUMO-AUTH");
                }
            }
            await LocalStorage.DeleteAsync(JSRuntime, nameof(AuthToken));
            var identity = new ClaimsIdentity();
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        private Task<AuthenticationState> GetTwitterClaims(AuthInfo authInfo)
        {
            List<Claim> userClaims = new List<Claim>();
            foreach (AuthUserClaim userClaim in authInfo.UserClaims)
            {
                userClaims.Add(new Claim(userClaim.Type, userClaim.Value));
            }
            var identity = new ClaimsIdentity(userClaims, "EasyAuth");
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        private async Task<AuthToken> GetAuthToken()
        {
            var authTokenFragment = HttpUtility.UrlDecode(new Uri(NavigationManager.Uri).Fragment);
            if (string.IsNullOrEmpty(authTokenFragment))
            {
                return await LocalStorage.GetAsync<AuthToken>(JSRuntime, "authtoken");
            }
            var getJsonRegEx = new Regex(@"\{(.|\s)*\}");
            var matches = getJsonRegEx.Matches(authTokenFragment);
            if (matches.Count == 1)
            {
                AuthToken authToken;
                try
                {
                    authToken = JsonSerializer.Deserialize<AuthToken>(matches[0].Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error in authentication token: {e.Message}");
                    return new AuthToken();
                }
                await JSRuntime.InvokeAsync<string>("EasyAuthDemoUtilities.updateURLwithoutReload", Constants.BlazorWebsiteURL);
                return authToken;
            }
            return new AuthToken();
        }
        public async Task Logout()
        {
            var authresponse = await HttpClient.GetAsync(Constants.AzureFunctionAuthURL + Constants.LogOutEndpoint);
            HttpClient.DefaultRequestHeaders.Remove("X-ZUMO-AUTH");
            await LocalStorage.DeleteAsync(JSRuntime, "authtoken");
            if (authresponse.IsSuccessStatusCode)
            {
                NotifyAuthenticationStateChanged();
            }
        }
        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
