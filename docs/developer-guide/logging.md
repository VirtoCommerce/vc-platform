# Logging

This documentation explain logging basics and configuration for VC Platform.

## Overview

VirtoCommerce Platform uses Serilog library to capture and store log messages from the application. Serilog also supports a wide range of sinks (logging providers), which are the destinations for log messages. These can include local files, network destinations, cloud-based services, and more. Serilog's configuration system makes it easy to set up and manage these sinks, so you can quickly and easily start logging to the destinations.

By default Platform provides Console and Debug sinks. Azure Application Insights sink is delivered in the separate Commerce Bundle module.  

## Configuring Serilog

The default Development configuration in the `appsettings.json` file will look like this: 

```JSON
{
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console",
      "Serilog.Sinks.Debug"
    ],
    "MinimumLevel": {
      "Default": "Verbose",
      "Override": {
        "System": "Information",
        "Microsoft": "Information",
        "Microsoft.AspNetCore.SignalR": "Verbose",
        "Microsoft.AspNetCore.Http.Connections": "Verbose"
      }
    },
    "WriteTo": [
      "Console",
      "Debug"
    ]
  }
}
```

The configuration is read from the `Serilog` section. The `Using` section contains list of assemblies in which configuration methods (WriteTo.File(), Enrich.WithThreadId()) reside. The `MinimumLevel` configuration property can be set to a single value as in the sample above, or, levels can be overridden per logging source. Sinks are configured using the `WriteTo` configuration property.

## Log levels

Serilog offers several log levels that can be used for both production and development environments. The appropriate log levels will depend on the specific needs of your application and the level of detail you want to capture in your logs. However, here are some general recommendations:

For production environments, it is recommended to use the following log levels:

* **Information**: This log level is suitable for general logging of important events in your application that are not errors or warnings. For example, successful requests or completed transactions.
* **Warning**: This log level is suitable for logging events that are potentially problematic, but not critical. For example, failed login attempts or failed database connections.
* **Error**: This log level is suitable for logging events that indicate an error or problem with your application that needs attention. For example, exceptions, crashes or unhandled errors.

For development environments, it is recommended to use the following log levels:

* **Debug**: This log level is suitable for detailed logging of events during development and debugging. For example, variable values, method calls or control flow.
* **Trace**: This log level is suitable for extremely detailed logging of events that are not normally logged. For example, low-level system events or diagnostic information.

## Extending logging

The Platform reads Serilog configuration from the `appsettings.json` and can also configured from the code. To achieve this Platform provides `ILoggerConfigurationService` interface that allows users writing adding their own sinks or making customizations. 

```cs
UseSerilog((context, services, loggerConfiguration) =>
{
    // read from configuration
    _ = loggerConfiguration.ReadFrom.Configuration(context.Configuration);

    // enrich configuration from external sources
    var configurationServices = services.GetService<IEnumerable<ILoggerConfigurationService>>();
    foreach (var service in configurationServices)
    {
        service.Configure(loggerConfiguration);
    }
})
```

This code shows how Serilog is being initialized in the Platform. First the `loggerConfiguration` objects is initialized from the configuration sections and the passed to a list of external services. To implement your own config services first create a class that inherits `ILoggerConfig` and implement it. For example, this is the implementation for the Azure Application Insights sink:

```cs
public class ApplicationInsightsLoggerConfiguration : ILoggerConfigurationService
{
    private readonly TelemetryConfiguration _configuration;

    public ApplicationInsightsLoggerConfiguration(TelemetryConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration.WriteTo.ApplicationInsights(telemetryConfiguration: _configuration,
        telemetryConverter: TelemetryConverter.Traces,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error);
    }
}
```

Them register the implementation in `module.cs` Initialize method of your external module:

```cs
public void Initialize(IServiceCollection serviceCollection)
{
    serviceCollection.AddTransient<ILoggerConfigurationService, ApplicationInsightsLoggerConfiguration>();
}
```

## References
* [Serilog Library](https://serilog.net/)
* [Serilog ASP .NET Core](https://github.com/serilog/serilog-aspnetcore)
* [Settings Configuration](https://github.com/serilog/serilog-settings-configuration)
* [Provided Sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks)
