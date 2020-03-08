using System;

namespace BlazeOrchard
{
    public static class Constants
    {
        public static string BlazorWebsiteURL => "http://localhost:5001/"; //"https://<client side blazor site url>";
        public static string AzureFunctionAuthURL => "http://localhost:7071"; // deployed "https://<your Azure function name>.azurewebsites.net";
        public static string AuthMeEndpoint => "/.auth/me";
        public static string LogOutEndpoint => "/.auth/logout";
        public static string TwitterEndpoint => "/.auth/login/twitter";
        public static string PostLoginRedirect => "?post_login_redirect_url=";
        public static string LoginMode => "&session_mode=token";
    }
}
