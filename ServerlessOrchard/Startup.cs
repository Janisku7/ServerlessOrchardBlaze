﻿using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
[assembly: FunctionsStartup(typeof(ServerlessOrchard.Startup))]
namespace ServerlessOrchard
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOrchardCms();
            
        }
    }
}