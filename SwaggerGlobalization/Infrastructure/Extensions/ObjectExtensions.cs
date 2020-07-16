using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SwaggerGlobalization.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
       
        public static T CloneObject<T>(this object objSource)
        {
            if (objSource == null)
                return default(T);

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, objSource);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T CastObject<T>(this object objSource)
        {
            if (objSource == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(objSource));
        }
    }
}
