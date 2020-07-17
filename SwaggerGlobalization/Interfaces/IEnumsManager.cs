using SwaggerGlobalization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Interfaces
{
    public interface IEnumsManager
    {
        string GetDisplayValue(Enum value);
        string GetDescription(Enum value);
        List<KeyValueIntDto> ToList<T>(bool order = false, List<T> ElementsToRemove = null);
        string GetDisplayValue(Type enumType, string value, bool originalValueIfFails = false);

    }
}
