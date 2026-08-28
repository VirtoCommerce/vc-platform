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
                var currentUserNameResolver = appBuilder.ApplicationServices.CreateScope().ServiceProvider.GetService<IUserNameResolver>();
                var currentTime = DateTime.UtcNow;
                var userName = currentUserNameResolver.GetCurrentUserName();

                entry.Entity.CreatedDate = currentTime;
                entry.Entity.ModifiedDate = entry.Entity.CreatedDate;
                entry.Entity.CreatedBy = userName;
                entry.Entity.ModifiedBy = entry.Entity.CreatedBy;
            };

            Triggers<IAuditable>.Updating += entry =>
            {
                var currentUserNameResolver = appBuilder.ApplicationServices.CreateScope().ServiceProvider.GetService<IUserNameResolver>();
                var currentTime = DateTime.UtcNow;
                var userName = currentUserNameResolver.GetCurrentUserName();

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
    }
}
