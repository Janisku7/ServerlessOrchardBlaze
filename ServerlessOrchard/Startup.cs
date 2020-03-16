using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore;
using Dynamitey;
using Microsoft.Extensions.Azure;
using Azure.Storage.Blobs;
using Azure.Core;
using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.WebJobs.Host.Executors;
using Azure.Core.Extensions;
using Azure.Storage;
using Jint;

[assembly: FunctionsStartup(typeof(ServerlessOrchard.Startup))]
namespace ServerlessOrchard
{
    public class Startup : FunctionsStartup
    {

        private BlazeBlobSettings blazeBlobOptions;
        
        
        public override void Configure(IFunctionsHostBuilder builder)
        {
            
            //builder.AddBlobServiceClient(blazeBlobOptions.ConnectionString,blazeBlobOptions.SharedKeyCredential);
            builder.Services.AddOptions();
            builder.Services.AddAuthenticationCore();
            builder.Services.AddOrchardCore();
            builder.Services.AddHttpClient();
            builder.Services.AddDataProtection();
            
        }
        
    }
}
