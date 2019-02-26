using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Platform.Core.Security
{
    public class ApiAccount : ICloneable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ApiAccountType ApiAccountType { get; set; }
        public bool? IsActive { get; set; }
        public string AppId { get; set; }
        public string SecretKey { get; set; }

        /// <summary>
        /// The flag indicates that SecretKey  must be changed
        /// </summary>
        public bool SecretKeyExpired { get; set; }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
