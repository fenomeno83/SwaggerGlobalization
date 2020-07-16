using Microsoft.Extensions.DependencyInjection;
using SwaggerGlobalization.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Services
{
    public static class ServiceExtensions
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {

            // Business services
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IInfrastructureService, InfrastructureService>();


        }
    }
}
