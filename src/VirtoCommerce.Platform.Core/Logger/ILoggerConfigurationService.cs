using Serilog;

namespace VirtoCommerce.Platform.Core.Logger
{
    public interface ILoggerConfigurationService
    {
        void Configure(LoggerConfiguration loggerConfiguration);
    }
}
