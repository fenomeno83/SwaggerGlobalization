using SwaggerGlobalization.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwaggerGlobalization.Services
{
    public class ServiceBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly IStringLocalizer<Resources> _localizer;
        protected readonly IEnumsManager _enumsManager; 
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceBase(IInfrastructureService infrastructure, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = infrastructure.Configuration;
            _httpContextAccessor = httpContextAccessor;
            _localizer = infrastructure.Localizer;
            _enumsManager = infrastructure.EnumsManager;

        }
    }
}
