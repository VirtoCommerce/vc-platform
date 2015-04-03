using DotLiquid;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class LoginProvider : Drop
    {
        [DataMember]
        public string AuthenticationType { get; set; }

        [DataMember]
        public string Caption { get; set; }

        [DataMember]
        public IDictionary<string, object> Properties { get; set; }
    }
}