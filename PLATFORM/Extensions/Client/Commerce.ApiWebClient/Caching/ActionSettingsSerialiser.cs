using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Routing;
using VirtoCommerce.ApiWebClient.Caching.Interfaces;

namespace VirtoCommerce.ApiWebClient.Caching
{
    public class ActionSettingsSerialiser : IActionSettingsSerialiser
    {
        private readonly DataContractSerializer _serialiser;

        public ActionSettingsSerialiser()
        {
            _serialiser = new DataContractSerializer(typeof(ActionSettings), new[] { typeof(RouteValueDictionary) });
        }

        public string Serialise(ActionSettings actionSettings)
        {
            using (var memoryStream = new MemoryStream())
            {
                _serialiser.WriteObject(memoryStream, actionSettings);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public ActionSettings Deserialise(string serialisedActionSettings)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(serialisedActionSettings)))
            {
                return (ActionSettings)_serialiser.ReadObject(memoryStream);
            }
        }
    }
}