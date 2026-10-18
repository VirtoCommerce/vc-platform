using System;
using EntityFrameworkCore.Triggers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.ChangeLog;

namespace VirtoCommerce.Platform.Data.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDbTriggers(this IApplicationBuilder appBuilder)
        {
            Triggers<IAuditable>.Inserting += entry =>
            {
                var currentTime = DateTime.UtcNow;
                var userName = GetCurrentUserName(appBuilder);

                entry.Entity.CreatedDate = currentTime;
                entry.Entity.ModifiedDate = entry.Entity.CreatedDate;
                entry.Entity.CreatedBy = userName;
                entry.Entity.ModifiedBy = entry.Entity.CreatedBy;
            };

            Triggers<IAuditable>.Updating += entry =>
            {
                var currentTime = DateTime.UtcNow;
                var userName = GetCurrentUserName(appBuilder);

                entry.Entity.CreatedDate = entry.Original.CreatedDate;
                entry.Entity.CreatedBy = entry.Original.CreatedBy;
                entry.Entity.ModifiedDate = currentTime;
                entry.Entity.ModifiedBy = userName;
            };

            // Resolved once: the trigger fires for every saved row.
            var lastChangesNotifier = appBuilder.ApplicationServices.GetRequiredService<ILastChangesNotifier>();

            Triggers<IEntity>.Inserting += entry =>
            {
                lastChangesNotifier.OnEntitySaving(entry.Context, entry.Entity);
            };

            Triggers<IEntity>.Updating += entry =>
            {
                lastChangesNotifier.OnEntitySaving(entry.Context, entry.Entity);
            };

            return appBuilder;
        }

        // IUserNameResolver is scoped; the trigger runs per saved row outside any request scope, so the scope must be released here.
        private static string GetCurrentUserName(IApplicationBuilder appBuilder)
        {
            using var scope = appBuilder.ApplicationServices.CreateScope();

            return scope.ServiceProvider.GetRequiredService<IUserNameResolver>().GetCurrentUserName();
        }
    }
}
