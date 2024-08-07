using Microsoft.Extensions.Configuration;
using Npgsql;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security;

namespace VirtoCommerce.Platform.Data.PostgreSql
{
    public class PostgreSqlCertificateLoader : ICertificateLoader
    {
        private readonly IConfiguration _configuration;

        public PostgreSqlCertificateLoader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected virtual string GetConnectionString()
        {
            return _configuration["Auth:ConnectionString"] ??
                _configuration.GetConnectionString("VirtoCommerce") ??
                string.Empty;
        }

        /// <summary>
        /// Check database existence
        /// </summary>
        /// <param name="sourceConnectionString"></param>
        /// <returns></returns>
        protected virtual bool CheckDatabaseExist(string sourceConnectionString)
        {
            using (var connection = new NpgsqlConnection(sourceConnectionString))
            {
                try
                {
                    connection.Open();
                    return true; // Database exists
                }
                catch
                {
                    return false; // Database doesn't exist or connection failed
                }
            }
        }

        public ServerCertificate Load()
        {
            var result = new ServerCertificate();

            var connectionString = GetConnectionString();

            if (CheckDatabaseExist(connectionString))
            {
                var builder = new NpgsqlConnectionStringBuilder(connectionString);

                const string cmdCheckMigration =
                    @"SELECT 1 FROM pg_tables
                      WHERE
                         schemaname = 'public' AND
                         tablename  = 'ServerCertificate'
                    ;";

                const string cmdServerCert =
                    "SELECT \"Id\",\"PublicCertBase64\",\"PrivateKeyCertBase64\",\"PrivateKeyCertPassword\" FROM \"ServerCertificate\" LIMIT 1";

                const int ixId = 0;
                const int ixPublicCertBase64 = 1;
                const int ixPrivateKeyCertBase64 = 2;
                const int ixPrivateKeyCertPassword = 3;

                using var conn = new NpgsqlConnection(builder.ConnectionString);
                using var commandCheckMigration = conn.CreateCommand();
                commandCheckMigration.CommandText = cmdCheckMigration;
                var parameterDbName = commandCheckMigration.CreateParameter();
                parameterDbName.ParameterName = "dbName";
                parameterDbName.Value = builder.Database;
                commandCheckMigration.Parameters.Add(parameterDbName);

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
