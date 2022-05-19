using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        private static string GetConnectionString(IConfiguration configuration)
        {
            return configuration["Auth:ConnectionString"] ?? configuration.GetConnectionString("VirtoCommerce");
        }

        /// <summary>
        /// Check database existence
        /// </summary>
        /// <param name="sourceConnectionString"></param>
        /// <returns></returns>
        private static bool CheckDatabaseExist(string sourceConnectionString)
        {
            var builder = new SqlConnectionStringBuilder(sourceConnectionString);
            var dbName = builder.InitialCatalog; // Catch database name to search from the connection string
            builder.Remove("Initial Catalog"); // Initial catalog should be removed from connection string, otherwise the connection could not be opened
            const string cmdCheckDb =
                @"select 1 from [sys].[databases] where name=@dbname";
            var connectionString = builder.ConnectionString;

            using var conn = new SqlConnection(connectionString);
            using var commandCheckDb = conn.CreateCommand();
            commandCheckDb.CommandText = cmdCheckDb;
            var parameterDbName = commandCheckDb.CreateParameter();
            parameterDbName.ParameterName = "dbName";
            parameterDbName.Value = dbName;
            commandCheckDb.Parameters.Add(parameterDbName);
            conn.Open();
            using var readerCheckDb = commandCheckDb.ExecuteReader();

            return readerCheckDb.HasRows;
        }

        /// <summary>
        /// Get stored server certificate
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ServerCertificate LoadCurrentlySet(IConfiguration configuration)
        {
            var result = new ServerCertificate();

            var connectionString = GetConnectionString(configuration);

            if (CheckDatabaseExist(connectionString))
            {
                const string cmdCheckMigration =
                    @"select 1 from [sys].[tables] where name='ServerCertificate'";

                const string cmdServerCert =
                    @"SELECT TOP (1) [Id]
                  ,[PublicCertBase64]
                  ,[PrivateKeyCertBase64]
                  ,[PrivateKeyCertPassword]
                FROM [ServerCertificate]";

                const int ixId = 0;
                const int ixPublicCertBase64 = 1;
                const int ixPrivateKeyCertBase64 = 2;
                const int ixPrivateKeyCertPassword = 3;

                using var conn = new SqlConnection(connectionString);
                using var commandCheckMigration = conn.CreateCommand();
                commandCheckMigration.CommandText = cmdCheckMigration;
                using var commandServerCert = conn.CreateCommand();
                commandServerCert.CommandText = cmdServerCert;

                // If the table ServerCertificate not found, still no migrations applied, this is the first run, return an empty ServerCertificate instance
                conn.Open();
                using var readerCheckMigration = commandCheckMigration.ExecuteReader();
                if (readerCheckMigration.HasRows)
                {
                    conn.Close();
                    conn.Open();
                    // If the table ServerCertificate is empty, therefore no certificates stored yet, return an empty ServerCertificate instance
                    using var readerServerCert = commandServerCert.ExecuteReader();
                    if (readerServerCert.HasRows)
                    {   //Read certificate from DB
                        readerServerCert.Read();
                        result.Id = readerServerCert.GetString(ixId);
                        result.PublicCertBytes = Convert.FromBase64String(readerServerCert.GetString(ixPublicCertBase64));
                        result.PrivateKeyCertBytes = Convert.FromBase64String(readerServerCert.GetString(ixPrivateKeyCertBase64));
                        result.PrivateKeyCertPassword = readerServerCert.GetString(ixPrivateKeyCertPassword);
                        result.StoredInDb = true;
                    }
                }

                var publicCertPath = configuration["Auth:PublicCertPath"];
                var privateKeyPath = configuration["Auth:PrivateKeyPath"];

                // If there is no certificate in DB and certificate is present in files
                // then load from files.
                // Otherwise left it empty with default virto-cert number.
                // Default certificate will be replaced later by self-signed
                if (!result.StoredInDb &&
                    File.Exists(publicCertPath ?? string.Empty) &&
                    File.Exists(privateKeyPath ?? string.Empty))
                {
                    result.PrivateKeyCertPassword = configuration["Auth:PrivateKeyPassword"];
                    result.PublicCertBytes = File.ReadAllBytes(publicCertPath);
                    result.PrivateKeyCertBytes = File.ReadAllBytes(privateKeyPath);
                }
            }

            return result;
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
                PrivateKeyCertPassword = "virto"
            };
            newCert.PublicCertBytes = x509Cert.Export(X509ContentType.Cert, string.Empty);
            newCert.PrivateKeyCertBytes = x509Cert.Export(X509ContentType.Pfx, newCert.PrivateKeyCertPassword);

            return newCert;
        }
    }
}
