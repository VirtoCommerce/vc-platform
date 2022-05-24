using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Data.GenericCrud;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security.Services
{
    public class ServerCertificateService : CrudService<ServerCertificate, ServerCertificateEntity, ServerCertificateChangeEvent, ServerCertificateChangedEvent>
    {

        public ServerCertificateService(Func<ISecurityRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IEventPublisher eventPublisher) : base(repositoryFactory, platformMemoryCache, eventPublisher)
        {
        }

        protected async override Task<IEnumerable<ServerCertificateEntity>> LoadEntities(IRepository repository, IEnumerable<string> ids, string responseGroup)
        {
            return await ((ISecurityRepository)repository).ServerCertificates.Where(x => ids.Contains(x.Id)).ToListAsync();

        }

        public static ServerCertificate CreateSelfSigned()
        {
            using var algorithm = RSA.Create(keySizeInBits: 4096);

            var subject = new X500DistinguishedName("O=Virtocommerce, S=Vilnius, C=LT, CN=virtocommerce.com");
            var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, critical: true));
            request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection() {
                Oid.FromOidValue("1.3.6.1.5.5.7.3.1", OidGroup.EnhancedKeyUsage), // Add Server Authentication usage
                Oid.FromOidValue("1.3.6.1.5.5.7.3.2", OidGroup.EnhancedKeyUsage) // Add Client Authentication usage
            }, true));
            var dnsExtensionBuilder = new SubjectAlternativeNameBuilder();
            dnsExtensionBuilder.AddDnsName("virtocommerce.com");
            request.CertificateExtensions.Add(dnsExtensionBuilder.Build());

            var x509Cert = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(10));

            var newCert = new ServerCertificate()
            {
                PrivateKeyCertPassword = Convert.ToBase64String(BitConverter.GetBytes(Environment.ProcessId)) // Generate password somehow (the way does not matter). We can even use a constant here, but Slint does not allow this
            };
            newCert.PublicCertBytes = x509Cert.Export(X509ContentType.Cert, string.Empty);
            newCert.PrivateKeyCertBytes = x509Cert.Export(X509ContentType.Pfx, newCert.PrivateKeyCertPassword);

            return newCert;
        }
    }
}
