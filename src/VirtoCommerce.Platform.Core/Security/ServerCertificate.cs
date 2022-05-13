using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class ServerCertificate : Entity, ICloneable
    {
        public const string SerialNumberOfVirtoPredefined = "482e53df1e594c89d5905c6a64719424566055f9";
        public byte[] PublicCertBytes { get; set; }
        public byte[] PrivateKeyCertBytes { get; set; }
        public string PrivateKeyCertPassword { get; set; }
        public bool StoredInDb { get; set; }

        public object Clone()
        {
            return MemberwiseClone() as ServerCertificate;
        }
    }
}
