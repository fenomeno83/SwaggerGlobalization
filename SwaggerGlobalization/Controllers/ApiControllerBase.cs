using SwaggerGlobalization.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly IStringLocalizer _localizer;
        protected readonly IEnumsManager _enumsManager;

        public ApiControllerBase(IInfrastructureService infrastructure)
        {
            _configuration = infrastructure.Configuration;
            _localizer = infrastructure.Localizer;
            _enumsManager = infrastructure.EnumsManager;

        }
    }
}
