using System;
using System.Security.Cryptography.X509Certificates;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security;

public sealed class ServerCertificate : Entity, ICloneable
{
    public const string SerialNumberOfVirtoPredefined = "482e53df1e594c89d5905c6a64719424566055f9";
    private byte[] _PublicCertBytes;
    public byte[] PublicCertBytes
    {
        get
        {
            return _PublicCertBytes;
        }
        set
        {
            _PublicCertBytes = value;
            X509Certificate = new X509Certificate2(_PublicCertBytes);
            SerialNumber = X509Certificate.SerialNumber;
        }

    }
    public byte[] PrivateKeyCertBytes { get; set; }
    public string PrivateKeyCertPassword { get; set; }
    public bool StoredInDb { get; set; }
    public string SerialNumber { get; private set; } = SerialNumberOfVirtoPredefined;
    public X509Certificate2 X509Certificate { get; private set; }
    public bool Expired
    {
        get
        {
            var now = DateTime.UtcNow;
            return !(now >= X509Certificate.NotBefore.ToUniversalTime() && now < X509Certificate.NotAfter.ToUniversalTime());
        }
    }
    public object Clone()
    {
        return MemberwiseClone() as ServerCertificate;
    }
}
