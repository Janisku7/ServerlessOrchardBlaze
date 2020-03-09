using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore;
using Dynamitey;

[assembly: FunctionsStartup(typeof(ServerlessOrchard.Startup))]
namespace ServerlessOrchard
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.AddAuthenticationCore();
            builder.Services.AddOrchardCore();
            builder.Services.AddHttpClient();
            builder.Services.AddDataProtection();
            
        }
        
    }
}
