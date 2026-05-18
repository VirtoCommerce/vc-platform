using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.Platform.BackupRestore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackupRestoreService(this IServiceCollection services)
    {
        services.AddScoped<IPlatformExportImportManager, BackupRestoreManager>();
        services.AddSingleton<IZipBackupArchiveFactory, SharpZipBackupArchiveFactory>();

        return services;
    }
}
