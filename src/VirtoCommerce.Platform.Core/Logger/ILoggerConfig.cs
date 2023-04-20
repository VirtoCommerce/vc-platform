using Serilog;

namespace VirtoCommerce.Platform.Core.Logger
{
    public interface ILoggerConfig
    {
        void Configure(LoggerConfiguration loggerConfiguration);
    }
}
