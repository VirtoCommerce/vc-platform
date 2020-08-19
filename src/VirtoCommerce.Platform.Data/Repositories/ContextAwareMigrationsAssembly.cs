using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System;
using System.Reflection;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public class ContextAwareMigrationsAssembly : MigrationsAssembly
    {
        private readonly DbContext context;

        public ContextAwareMigrationsAssembly(
            ICurrentDbContext currentContext,
            IDbContextOptions options,
            IMigrationsIdGenerator idGenerator,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger) : base(currentContext, options, idGenerator, logger)
        {
            context = currentContext.Context;
        }

        /// <summary>
        /// Modified from http://weblogs.thinktecture.com/pawel/2018/06/entity-framework-core-changing-db-migration-schema-at-runtime.html
        /// </summary>
        /// <param name="migrationClass"></param>
        /// <param name="activeProvider"></param>
        /// <returns></returns>
        public override Migration CreateMigration(TypeInfo migrationClass, string activeProvider)
        {
            var hasCtorWithDbContext = migrationClass
                    .GetConstructor(new[] { typeof(DbContext) }) != null;

            if (hasCtorWithDbContext)
            {
                var instance = (Migration)Activator.CreateInstance(migrationClass.AsType(), context);
                instance.ActiveProvider = activeProvider;
                return instance;
            }

            return base.CreateMigration(migrationClass, activeProvider);
        }
    }
}
