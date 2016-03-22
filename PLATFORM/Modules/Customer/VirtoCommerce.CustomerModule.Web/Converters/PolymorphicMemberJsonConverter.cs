using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Web.Converters
{
    /// <summary>
    /// Used to deserialize from JSON derived Member types
    /// </summary>
    public class PolymorphicMemberJsonConverter : JsonConverter
    {
        private readonly IMembersFactory _membersFactory;
        public PolymorphicMemberJsonConverter(IMembersFactory membersFactory)
        {
            _membersFactory = membersFactory;
        }

        public override bool CanWrite { get { return false; } }

        public override bool CanRead { get { return true; } }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Member);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);   
            var pt = obj["memberType"];
            if (pt == null)
            {
                throw new ArgumentException("Missing memberType", "memberType");
            }

            string memberType = pt.Value<string>();
            var retVal = _membersFactory.GetAllServices().Select(x => x.TryCreateMember(memberType)).Where(x => x != null).FirstOrDefault();
            if(retVal == null)
            {
                throw new NotSupportedException("Unknown memberType: " + memberType);
            }

            serializer.Populate(obj.CreateReader(), retVal);
            return retVal;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}