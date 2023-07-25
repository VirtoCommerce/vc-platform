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

        public virtual ServerCertificate ToModel(ServerCertificate model)
        {
            model.Id = Id;
            model.PublicCertBytes = Convert.FromBase64String(PublicCertBase64);
            model.PrivateKeyCertBytes = Convert.FromBase64String(PrivateKeyCertBase64);
            model.PrivateKeyCertPassword = PrivateKeyCertPassword;

            return model;
        }

        public virtual ServerCertificateEntity FromModel(ServerCertificate model, PrimaryKeyResolvingMap pkMap)
        {
            pkMap.AddPair(model, this);

            Id = model.Id;
            PublicCertBase64 = Convert.ToBase64String(model.PublicCertBytes);
            PrivateKeyCertBase64 = Convert.ToBase64String(model.PrivateKeyCertBytes);
            PrivateKeyCertPassword = model.PrivateKeyCertPassword;

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
