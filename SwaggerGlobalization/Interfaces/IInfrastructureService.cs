using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Interfaces
{
    public interface IInfrastructureService
    {
        IConfiguration Configuration { get; }
        IEnumsManager EnumsManager { get; }
        IStringLocalizer<Resources> Localizer { get; }

    }
}
