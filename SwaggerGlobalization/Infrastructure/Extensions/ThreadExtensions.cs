using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Infrastructure.Extensions
{
    public static class ThreadExtensions
    {
        public static void SetCulture(this Thread thread, string culture)
        {
            if (string.IsNullOrWhiteSpace(culture))
                return;

            SetCulture(thread, new CultureInfo(culture.Trim()));
        }

        public static void SetCulture(this Thread thread, CultureInfo culture)
        {
            if (culture == null)
                return;

            thread.CurrentCulture = culture;
            thread.CurrentUICulture = culture;
        }
    }
}
