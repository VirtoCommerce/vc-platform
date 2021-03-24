using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Security.Repositories;
namespace VirtoCommerce.Platform.Web.Migrations
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Apply platform migrations
        /// </summary>
        /// <param name="appBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePlatformMigrations(this IApplicationBuilder appBuilder)
        {
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                var platformDbContext = serviceScope.ServiceProvider.GetRequiredService<PlatformDbContext>();
                platformDbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName("Platform"));
                platformDbContext.Database.Migrate();

                var securityDbContext = serviceScope.ServiceProvider.GetRequiredService<SecurityDbContext>();
                securityDbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName("Security"));
                securityDbContext.Database.Migrate();
            }

            return appBuilder;
        }
    }
}
