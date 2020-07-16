using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SwaggerGlobalization.Infrastructure.Extensions
{
    public static class StringExtensions
    {
     
        public static string RemoveNewLineAndTrim(this string input)
        {
            if (input == null)
                return null;

            input = Regex.Replace(input, @"\t|\n|\r", "");
            input = input.Trim();

            return input;
        }

      
        public static T CustomCast<T>(this string input)
        {
            T output = default(T);
            if (string.IsNullOrWhiteSpace(input))
                return output;

            input = input.Trim();
            try
            {
                Type typeToCastTo = typeof(T);

                if (typeof(T).IsGenericType)
                    typeToCastTo = typeToCastTo.GenericTypeArguments[0];

                if (typeToCastTo.IsEnum)
                {
                    if (Enum.IsDefined(typeToCastTo, input))
                        return (T)Enum.Parse(typeToCastTo, input);
                    return output;
                }


                object value = Convert.ChangeType(input, typeToCastTo, CultureInfo.InvariantCulture);
                return (value == null) ? output : (T)value;
            }
            catch
            {
                return output;
            }
        }

        public static JToken GetBodyObject(this string body)
        {
            return string.IsNullOrWhiteSpace(body) ? null : JToken.Parse(body);
        }

        public static T GetBodyObject<T>(this string body)
        {
            return string.IsNullOrWhiteSpace(body) ? default(T) : JsonConvert.DeserializeObject<T>(body);
        }

    }
}
