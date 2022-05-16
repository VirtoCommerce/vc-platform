using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Exceptions;
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

        /// <summary>
        /// Get stored server certificate
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static ServerCertificate LoadCurrentlySet(IConfiguration configuration)
        {
            var connectionString = configuration["Auth:ConnectionString"] ?? configuration.GetConnectionString("VirtoCommerce");

            const string cmdCheckMigration =
                @"select 1 from [sys].[tables] where name='ServerCertificate'";

            const string cmdServerCert =
                @"SELECT TOP (1) [Id]
                  ,[PublicCertBase64]
                  ,[PrivateKeyCertBase64]
                  ,[PrivateKeyCertPassword]
                  ,[StoredInDb]
                FROM [ServerCertificate]";

            const int ixId = 0;
            const int ixPublicCertBase64 = 1;
            const int ixPrivateKeyCertBase64 = 2;
            const int ixPrivateKeyCertPassword = 3;
            const int ixStoredInDb = 4;

            var result = new ServerCertificate();
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
                    result.StoredInDb = readerServerCert.GetBoolean(ixStoredInDb);
                }
            }

            if (!result.StoredInDb)
            { // Read certificate bytes from the files
                var publicCertPath = configuration["Auth:PublicCertPath"];
                if (string.IsNullOrEmpty(publicCertPath)) throw new PlatformException("The path to the public cert should be provided ('Auth:PublicCertPath' key in the settings)");
                var privateKeyPath = configuration["Auth:PrivateKeyPath"];
                if (string.IsNullOrEmpty(privateKeyPath)) throw new PlatformException("The path to the private key pfx should be provided ('Auth:PrivateKeyPath' key in the settings)");
                result.PrivateKeyCertPassword = configuration["Auth:PrivateKeyPassword"];
                result.PublicCertBytes = File.ReadAllBytes(publicCertPath);
                result.PrivateKeyCertBytes = File.ReadAllBytes(privateKeyPath);
            }

            return result;
        }
        
    }
}
