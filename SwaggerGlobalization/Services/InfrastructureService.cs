using SwaggerGlobalization.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace SwaggerGlobalization.Services
{
    public class InfrastructureService : IInfrastructureService
    {
        public IConfiguration Configuration { get; }
        public IStringLocalizer<Resources> Localizer { get; }
        public IEnumsManager EnumsManager { get; }


        public InfrastructureService(
            IStringLocalizer<Resources> localizer,
            IConfiguration configuration, IEnumsManager enumsManager)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            EnumsManager = enumsManager ?? throw new ArgumentNullException(nameof(enumsManager));



        }
    }
}
