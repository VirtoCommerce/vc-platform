using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security;

namespace VirtoCommerce.Platform.Data.SqlServer
{
    public class SqlServerCertificateLoader : ICertificateLoader
    {
        private readonly IConfiguration _configuration;

        public SqlServerCertificateLoader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected virtual string GetConnectionString()
        {
            return _configuration["Auth:ConnectionString"] ??
                _configuration.GetConnectionString("VirtoCommerce");
        }

        /// <summary>
        /// Check database existence
        /// </summary>
        /// <param name="sourceConnectionString"></param>
        /// <returns></returns>
        protected virtual bool CheckDatabaseExist(string sourceConnectionString)
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

        public ServerCertificate Load()
        {
            var result = new ServerCertificate();

            var connectionString = GetConnectionString();

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

                var publicCertPath = _configuration["Auth:PublicCertPath"];
                var privateKeyPath = _configuration["Auth:PrivateKeyPath"];

                // If there is no certificate in DB and certificate is present in files
                // then load from files.
                // Otherwise left it empty with default virto-cert number.
                // Default certificate will be replaced later by self-signed
                if (!result.StoredInDb &&
                    !string.IsNullOrEmpty(publicCertPath) &&
                    !string.IsNullOrEmpty(privateKeyPath) &&
                    File.Exists(publicCertPath) &&
                    File.Exists(privateKeyPath))
                {
                    result.PrivateKeyCertPassword = _configuration["Auth:PrivateKeyPassword"];
                    result.PublicCertBytes = File.ReadAllBytes(publicCertPath);
                    result.PrivateKeyCertBytes = File.ReadAllBytes(privateKeyPath);
                }
            }

            return result;
        }
    }
}
