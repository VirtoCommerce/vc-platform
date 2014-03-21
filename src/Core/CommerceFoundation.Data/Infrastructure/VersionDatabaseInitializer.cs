using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Transactions;
using VirtoCommerce.Foundation.Data.Common;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Logging;

namespace VirtoCommerce.Foundation.Data
{
    public abstract class VersionDatabaseInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        private const string _splitByGoRegex = @"(?m)^\s*GO\s*\d*\s*$";

        protected string[] SplitScriptByGo(string script)
        {
            return Regex.Split(script + Environment.NewLine, _splitByGoRegex,
                               RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        private static bool Exists(SchemaVersionContext context)
        {
            bool flag;
            using (SqlDbConfiguration.ExecutionStrategySuspension)
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                flag = context.Database.Exists();
            }
            if (flag)
            {
                try
                {
                    context.Version.Count();
                    return true;
                }
                catch (EntityException)
                {
                }
            }
            return false;
        }

        private static bool Exists(OperationLogContext context)
        {
            bool flag;

            using (SqlDbConfiguration.ExecutionStrategySuspension)
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                flag = context.Database.Exists();
            }

            if (flag)
            {
                try
                {
                    context.OperationLogs.Count();
                    return true;
                }
                catch (EntityException)
                {
                }
            }
            return false;
        }

        protected void CreateSchemaVersionTables(DbContext context)
        {
            using (var ctx = new SchemaVersionContext(context.Database.Connection.ConnectionString))
            {
                //using (new TransactionScope(TransactionScopeOption.Suppress))
                {
                    var objectContext = ((IObjectContextAdapter) ctx).ObjectContext;
                    objectContext.Connection.Open();
                    context.Database.ExecuteSqlCommand(objectContext.CreateDatabaseScript());
                    var entity = new SchemaVersionRow();
                    //entity.MigrationId = MigrationAssembly.CreateMigrationId(Strings.InitialCreate);
                    entity.CreatedOn = DateTime.UtcNow;
                    entity.ModelId = ModelId;
                    entity.VersionId = Assembly.GetExecutingAssembly().GetFileVersion();
                    ctx.Version.Add(entity);
                    ctx.SaveChanges();
                    objectContext.Connection.Close();
                }
            }
        }

        protected void CreateOperationLogTables(DbContext context)
        {
            using (var ctx = new OperationLogContext(context.Database.Connection.ConnectionString))
            {
                //using (new TransactionScope(TransactionScopeOption.Suppress))
                {
                    var objectContext = ((IObjectContextAdapter) ctx).ObjectContext;
                    //objectContext.Connection.Open();
                    var script = objectContext.CreateDatabaseScript();
                    //string script = context.CreateDatabaseScript();
                    ctx.Database.ExecuteSqlCommand(script);
                    ctx.Database.CreateIndex<OperationLog>(x => x.ObjectType);
                    ctx.Database.CreateIndex<OperationLog>(x => x.OperationType);
                    ctx.Database.CreateIndex<OperationLog>(x => x.LastModified);
                    //objectContext.Connection.Close();
                }
            }
        }

        protected void SaveSchemaToDatabase(DbContext context)
        {
            using (var ctx = new SchemaVersionContext(context.Database.Connection))
            {
                var entity = new SchemaVersionRow();
                //entity.MigrationId = MigrationAssembly.CreateMigrationId(Strings.InitialCreate);
                entity.CreatedOn = DateTime.UtcNow;
                entity.ModelId = ModelId;
                entity.VersionId = Assembly.GetExecutingAssembly().GetFileVersion();
                ctx.Version.Add(entity);
                ctx.SaveChanges();
            }
        }

        public bool HasOperationLog(DbContext context)
        {
            bool ret = false;
            using (var ctx = new OperationLogContext(context.Database.Connection.ConnectionString))
            {
                if (Exists(ctx))
                {
                    ret = true;
                }
            }

            return ret;
        }

        public bool CompatibleWithModel(DbContext context, bool throwIfNoMetadata, string id)
        {
            bool ret = false;
            using (var ctx = new SchemaVersionContext(context.Database.Connection))
            {
                if (!Exists(ctx))
                {
                    ret = false;
                }
                else
                {
                    SchemaVersionRow version =
                        (from v in ctx.Version where v.ModelId.Equals(id, StringComparison.OrdinalIgnoreCase) select v)
                            .SingleOrDefault();
                    if (version == null)
                    {
                        ret = false;
                    }
                    else
                    {
                        ret = true;
                    }
                }
            }

            if (throwIfNoMetadata)
                throw new ApplicationException("no schema for " + id);

            return ret;
            /*
			string hash = EdmMetadata.TryGetModelHash(context);
			if (hash == null)
			{
				if (throwIfNoMetadata)
				{
					throw new ApplicationException("no schema hash");
				}
				return true;
			}

			if (((IObjectContextAdapter)context).ObjectContext.GetEntitySet(typeof(SchemaMetadata), false) == null)
			{
				if (throwIfNoMetadata)
				{
					throw new ApplicationException("no schema mapping");
				}
				return true;
			}

			string a = context.QueryForModelHash(id);
			if (a != null)
			{
				return string.Equals(a, hash, StringComparison.Ordinal);
			}
			if (throwIfNoMetadata)
			{
				throw new ApplicationException("no schema hash2");
			}

			return false;
			 * */
        }

        #region IDatabaseInitializer<Context> Members

        public virtual void InitializeDatabase(T context)
        {
            bool dbExists;

            //int MaxRetries = 10;
            //int DelayMS = 100;

            //RetryPolicy policy = new RetryPolicy<SqlAzureTransientErrorDetectionStrategy>(MaxRetries, TimeSpan.FromMilliseconds(DelayMS));

            //TransactionOptions tso = new TransactionOptions();
            //tso.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            //policy.ExecuteAction(() =>
            {
                dbExists = context.Database.Exists();
                if (!dbExists)
                    context.Database.Create();

                //using (new TransactionScope(TransactionScopeOption.Suppress))
                {
                    var objectContext = ((IObjectContextAdapter) context).ObjectContext;
                    //objectContext.Connection.Open();
                    //context.Database.Connection.Open();
                    //dbExists = context.Database.Exists();

                    if (dbExists)
                    {
                        var throwIfNoMetadata = false;

                        if (!HasOperationLog(context))
                            CreateOperationLogTables(context);

                        if (CompatibleWithModel(context, throwIfNoMetadata, ModelId))
                        {
                            //return;
                        }
                        else
                        {
                            // create all tables
                            //using (new TransactionScope(TransactionScopeOption.Suppress))
                            {
                                //objectContext.Connection.Open();
                                var dbCreationScript = objectContext.CreateDatabaseScript();
                                context.Database.ExecuteSqlCommand(dbCreationScript);
                                Seed(context);
                                //objectContext.Connection.Close();
                            }

                            SaveSchemaToDatabase(context);
                        }
                    }
                    else
                    {
                        //context.Database.Create();
                        CreateOperationLogTables(context);
                        CreateSchemaVersionTables(context);
                        Seed(context);
                    }

                    //objectContext.Connection.Close();
                    //context.Database.Connection.Close();
                }
                //});
            }
        }

        #endregion

        #region Methods

        protected abstract string ModelId { get; }

        protected virtual void Seed(T context)
        {
            // TODO: put here your seed creation
        }

        #endregion
    }
}