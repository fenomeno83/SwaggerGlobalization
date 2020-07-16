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
