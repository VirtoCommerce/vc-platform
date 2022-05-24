using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Model
{
    public class ServerCertificateEntity : Entity, IDataEntity<ServerCertificateEntity, ServerCertificate>
    {
        public string PublicCertBase64 { get; set; }
        public string PrivateKeyCertBase64 { get; set; }
        public string PrivateKeyCertPassword { get; set; }

        public virtual ServerCertificate ToModel(ServerCertificate certificate)
        {
            certificate.Id = Id;
            certificate.PublicCertBytes = Convert.FromBase64String(PublicCertBase64);
            certificate.PrivateKeyCertBytes = Convert.FromBase64String(PrivateKeyCertBase64);
            certificate.PrivateKeyCertPassword = PrivateKeyCertPassword;
            return certificate;
        }
        public virtual ServerCertificateEntity FromModel(ServerCertificate certificate, PrimaryKeyResolvingMap pkMap)
        {
            Id = certificate.Id;
            PublicCertBase64 = Convert.ToBase64String(certificate.PublicCertBytes);
            PrivateKeyCertBase64 = Convert.ToBase64String(certificate.PrivateKeyCertBytes);
            PrivateKeyCertPassword = certificate.PrivateKeyCertPassword;
            return this;
        }

        public void Patch(ServerCertificateEntity target)
        {
            target.Id = Id;
            target.PublicCertBase64 = PublicCertBase64;
            target.PrivateKeyCertBase64 = PrivateKeyCertBase64;
            target.PrivateKeyCertPassword = PrivateKeyCertPassword;
        }
    }
}
