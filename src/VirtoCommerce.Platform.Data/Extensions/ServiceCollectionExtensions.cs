using System;
using System.IO.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.TransactionFileManager;
using VirtoCommerce.Platform.Core.ZipFile;
using VirtoCommerce.Platform.Data.ChangeLog;
using VirtoCommerce.Platform.Data.DynamicProperties;
using VirtoCommerce.Platform.Data.ExportImport;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Localizations;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings;
using VirtoCommerce.Platform.Data.ZipFile;

namespace VirtoCommerce.Platform.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddPlatformServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PlatformDbContext>(options => options.UseDatabaseProviderSwitcher(configuration).SetConnectionName(configuration, "VirtoCommerce"));
            services.AddTransient<IPlatformRepository, PlatformRepository>();
            services.AddTransient<Func<IPlatformRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetService<IPlatformRepository>());

            services.AddSettings();

            services.AddDynamicProperties();

            var inProcessBus = new InProcessBus();
            services.AddSingleton<IHandlerRegistrar>(inProcessBus);
            services.AddSingleton<IEventPublisher>(inProcessBus);
            services.AddTransient<IChangeLogService, ChangeLogService>();
            services.AddTransient<ILastModifiedDateTime, ChangeLogService>();
            services.AddTransient<IChangeLogSearchService, ChangeLogSearchService>();

            services.AddCaching(configuration);

            services.AddScoped<IPlatformExportImportManager, PlatformExportImportManager>();
            services.AddSingleton<ITransactionFileManager, TransactionFileManager.TransactionFileManager>();

            services.AddTransient<IEmailSender, DefaultEmailSender>();


            //Register dependencies for translation
            services.AddSingleton<ITranslationDataProvider, PlatformTranslationDataProvider>();
            services.AddSingleton<ITranslationDataProvider, ModulesTranslationDataProvider>();
            services.AddSingleton<ITranslationService, TranslationService>();

            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddTransient<IZipFileWrapper, ZipFileWrapper>();

            return services;

        }
    }
}
