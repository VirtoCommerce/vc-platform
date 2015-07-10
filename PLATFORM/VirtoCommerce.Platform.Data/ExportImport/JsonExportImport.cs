using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.Platform.Data.ExportImport
{
    public abstract class JsonExportImport
    {
        private JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            return serializer;
        }

        public void Save(Stream backupStream, object obj, Action<ExportImportProgressInfo> progressCallback, ExportImportProgressInfo prodgressInfo)
        {

            var serializer = GetSerializer();

            using (var streamWriter = new StreamWriter(backupStream, Encoding.UTF8, 1024, true) { AutoFlush = true })
            {
                //Notification
                prodgressInfo.Description = "Saving ...";
                progressCallback(prodgressInfo);

                using (JsonWriter writer = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(writer, obj);
                }
                
                prodgressInfo.Description = "Done";
                progressCallback(prodgressInfo);
            }
        }

        public T Load<T>(Stream backupStream, Action<ExportImportProgressInfo> progressCallback, ExportImportProgressInfo prodgressInfo)
        {
            var serializer = GetSerializer();
            using (var streamReader = new StreamReader(backupStream, Encoding.UTF8))
            {
                //Notification
                prodgressInfo.Description = "Read data ...";
                progressCallback(prodgressInfo);
                
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    prodgressInfo.Description = "Transform data ...";
                    progressCallback(prodgressInfo);
                    var result = serializer.Deserialize<T>(reader);
                    prodgressInfo.Description = "Saving ...";
                    progressCallback(prodgressInfo);
                    return result;
                }
            }
        }
    }
}