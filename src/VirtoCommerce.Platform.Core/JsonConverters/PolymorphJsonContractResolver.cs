using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VirtoCommerce.Platform.Core.JsonConverters
{

    /// <summary>
    /// Used for represent derived (overridden) types in resulting swagger API docs.
    /// This converter gets derived types from AbstractTypeFactory
    /// </summary>
    public class PolymorphJsonContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            //Do not handle abstract types
            if (!type.IsAbstract)
            {
                var abstractTypeFactory = typeof(Common.AbstractTypeFactory<>).MakeGenericType(type);
                var pi = abstractTypeFactory.GetProperty("AllTypeInfos");
                var values = pi?.GetValue(null) as IList;
                //Handle only types which have any registered derived type
                if (values?.Count > 0)
                {
                    type = abstractTypeFactory.GetMethods().First(x => x.Name == "TryCreateInstance").Invoke(null, null)?.GetType();
                }
            }
            return base.CreateProperties(type, memberSerialization);
        }
    }

}
