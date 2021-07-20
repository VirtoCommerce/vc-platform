using System;
using System.IO.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.TransactionFileManager;
using VirtoCommerce.Platform.Core.ZipFile;
using VirtoCommerce.Platform.Data.ChangeLog;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.DynamicProperties;
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
            var connectionString = configuration.GetConnectionString("VirtoCommerce");
            //Do not use PlatformDbContext if db connection string is not set
            if (!string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<PlatformDbContext>(options => options.UseSqlServer(connectionString));
                services.AddTransient<IPlatformRepository, PlatformRepository>();
            }
            else
            {
                //use Stub instead, to be able run platform as lite host for modules without connection to DB
                services.AddTransient<IPlatformRepository, PlatformRepositoryStub>();
            }

            services.AddTransient<Func<IPlatformRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetService<IPlatformRepository>());

            services.AddSettings();

            services.AddDynamicProperties();

            services.AddSingleton<InProcessBus>();
            services.AddSingleton<IHandlerRegistrar>(x => x.GetRequiredService<InProcessBus>());
            services.AddSingleton<IEventPublisher>(x => x.GetRequiredService<InProcessBus>());
            services.AddTransient<IChangeLogService, ChangeLogService>();
            services.AddTransient<ILastModifiedDateTime, ChangeLogService>();
            services.AddTransient<ILastChangesService, LastChangesService>();

            services.AddTransient<IChangeLogSearchService, ChangeLogSearchService>();

            services.AddCaching(configuration);

           
            services.AddSingleton<ITransactionFileManager, TransactionFileManager.TransactionFileManager>();

            services.AddTransient<IEmailSender, DefaultEmailSender>();


            //Register dependencies for translation
            services.AddSingleton<ITranslationDataProvider, PlatformTranslationDataProvider>();
            services.AddSingleton<ITranslationDataProvider, ModulesTranslationDataProvider>();
            services.AddSingleton<ITranslationService, TranslationService>();

            services.AddSingleton<ICountriesService, FileSystemCountriesService>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddTransient<IZipFileWrapper, ZipFileWrapper>();

            return services;
        }
    }
}
