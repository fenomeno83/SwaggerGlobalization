using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace SwaggerGlobalization.Infrastructure.Extensions
{
    public static class HttpContextExtensions
    {
     
     
        public static string GetBodyString(this HttpContext HttpContext)
        {
            string bodyText;
            try
            {
                var bodyStream = new StreamReader(HttpContext.Request.Body);
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                bodyText = bodyStream.BaseStream.Length == 0 ? null : bodyStream.ReadToEnd();

            }
            catch
            {
                bodyText = null;
            }

            return bodyText;
        }

        public static string GetHasCatchError(this HttpContext context)
        {
            return context.GetContextItem(ApiConstants.HasCatchError);

        }

        public static void SetHasCatchError(this HttpContext context, string catchError)
        {
            context.SetContextItem(ApiConstants.HasCatchError, catchError);
        }

        public static void SetContextItem(this HttpContext context, string itemName, string value)
        {
            if (!string.IsNullOrWhiteSpace(itemName) && !string.IsNullOrWhiteSpace(value))
                context.Items[itemName] = value;
        }

        public static void RemoveContextItem(this HttpContext context, string itemName)
        {
            if (!string.IsNullOrWhiteSpace(itemName) && context.Items != null)
                context.Items.Remove(itemName);
        }

        public static string GetContextItem(this HttpContext context, string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                return null;

            context.Items.TryGetValue(itemName, out var value);

            if (value == null)
                return null;
            else
                return value.ToString();

        }

        public static JToken GetBodyObject(this HttpContext HttpContext)
        {
            string body = HttpContext.GetBodyString();
            return string.IsNullOrWhiteSpace(body) ? null : JToken.Parse(body);
        }

        public static T GetBodyObject<T>(this HttpContext HttpContext)
        {

            string body = HttpContext.GetBodyString();

            return string.IsNullOrWhiteSpace(body) ? default(T) : JsonConvert.DeserializeObject<T>(body);

        }

    }

}
