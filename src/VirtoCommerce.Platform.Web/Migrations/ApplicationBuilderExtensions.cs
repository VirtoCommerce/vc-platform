using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.Platform.Data.Repositories;
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
                var platformDbContext = serviceScope.ServiceProvider.GetService<PlatformDbContext>();
                if (platformDbContext != null)
                {
                    platformDbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName("Platform"));
                    platformDbContext.Database.Migrate();
                }

            }

            return appBuilder;
        }
    }
}
