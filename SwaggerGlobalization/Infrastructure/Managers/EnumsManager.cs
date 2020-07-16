using Microsoft.Extensions.Localization;
using SwaggerGlobalization.Infrastructure.Extensions;
using SwaggerGlobalization.Interfaces;
using SwaggerGlobalization.Models;
using SwaggerGlobalization.Models.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Infrastructure.Managers
{
    public class EnumsManager : IEnumsManager
    {
        private readonly IStringLocalizer _localizer;

        public EnumsManager(IStringLocalizer<Resources> localizer)
        {
            _localizer = localizer;
        }
        public string GetDisplayValue(Enum value)
        {
            try
            {
                if (value == null)
                    return null;


                var fieldInfo = value.GetType().GetField(value.ToString());
                var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (descriptionAttributes == null || descriptionAttributes.Length == 0) return value.ToString();

                if (!string.IsNullOrWhiteSpace(descriptionAttributes[0].Name))
                    return _localizer[descriptionAttributes[0].Name];
                else
                    return value.ToString();


            }
            catch
            {
                return value.ToString();
            }
        }

        public string GetDisplayValue(Type enumType, string value, bool originalValueIfFails = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value) || enumType == null)
                    return originalValueIfFails ? value : null;


                var fieldInfo = enumType.GetField(value);
                var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (descriptionAttributes == null || descriptionAttributes.Length == 0) return originalValueIfFails ? value : null;

                if (!string.IsNullOrWhiteSpace(descriptionAttributes[0].Name))
                    return _localizer[descriptionAttributes[0].Name];
                else
                    return originalValueIfFails ? value : null;

            }
            catch
            {
                return originalValueIfFails ? value : null;
            }

        }


        public string GetDescription(Enum value)
        {
            try
            {
                if (value == null)
                    return null;

                var fieldInfo = value.GetType().GetField(value.ToString());
                var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (descriptionAttributes == null || descriptionAttributes.Length == 0) return value.ToString();

                return descriptionAttributes[0].Description;
            }
            catch
            {
                return value.ToString();
            }
        }
        public List<KeyValueDto> ToList<T>(bool order = false, List<T> ElementsToRemove = null)
        {

            Type enumType = typeof(T);
            if (enumType.BaseType != typeof(System.Enum))
            {
                throw new ArgumentException("Type T must inherit from System.Enum. Found: " + enumType.BaseType);
            }

            List<KeyValueDto> items = null;

            if (order)
            {
                var list = EnumExtension.SortEnum<T>();
                items =
                  (from val in list
                   select new KeyValueDto
                   {
                       Value = _localizer[enumType.Name + "_" + val.ToString()],
                       Key = Convert.ToInt32(System.Enum.Parse(enumType, val.ToString()))/*.ToString()*/
                   }).Where(x => (ElementsToRemove == null || !ElementsToRemove.Contains(((int)(x.Key)).ToEnum<T>()))).ToList();
            }
            else
            {
                items =
                  (from string n in System.Enum.GetNames(enumType)
                   select new KeyValueDto
                   {
                       Value = _localizer[enumType.Name + "_" + n.ToString()],
                       Key = Convert.ToInt32(System.Enum.Parse(enumType, n))/*.ToString()*/
                   }).Where(x => (ElementsToRemove == null || !ElementsToRemove.Contains(((int)(x.Key)).ToEnum<T>()))).ToList();
            }


            return items;
        }
    }
}
