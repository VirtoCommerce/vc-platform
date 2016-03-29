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
    /// Used to deserialize from JSON derived SearchCriteria types
    /// </summary>
    public class PolymorphicMemberSearchCriteriaJsonConverter : JsonConverter
    {
        #region JsonConverter Members
        public override bool CanWrite { get { return false; } }

        public override bool CanRead { get { return true; } }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MembersSearchCriteria);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var retVal = CreateSearchCriteria();
            serializer.Populate(obj.CreateReader(), retVal);
            return retVal;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        } 
        #endregion

        /// <summary>
        /// Derived types should override this method to create other criteria type instance
        /// </summary>
        /// <returns></returns>
        protected virtual MembersSearchCriteria CreateSearchCriteria()
        {
            return new MembersSearchCriteria();
        }
    }
}