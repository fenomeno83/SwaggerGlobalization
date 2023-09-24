using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SwaggerGlobalization.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Services
{
    public static class ServiceExtensions
    {
        public static IStringLocalizer<Resources> GetSwaggerStringLocalizer(this IServiceCollection services)
        {
            return services.BuildServiceProvider().GetService<IStringLocalizer<Resources>>();
        }

        public static void AddBusinessServices(this IServiceCollection services)
        {

            // Business services
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<IInfrastructureService, InfrastructureService>();


        }
    }
}
