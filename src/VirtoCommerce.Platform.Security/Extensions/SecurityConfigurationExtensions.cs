using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace VirtoCommerce.Platform.Security.Extensions;

public static class SecurityConfigurationExtensions
{
    /// <summary>
    /// The name of the connection string of the security (authorization) database.
    /// </summary>
    public const string AuthConnectionStringName = "VirtoCommerce.Auth";

    /// <summary>
    /// The name of the connection string of the main platform database.
    /// </summary>
    public const string PlatformConnectionStringName = "VirtoCommerce";

    /// <summary>
    /// The obsolete location of the security database connection string.
    /// </summary>
    [Obsolete("Use the VirtoCommerce.Auth connection string (ConnectionStrings:VirtoCommerce.Auth) instead.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public const string AuthConnectionStringKey = "Auth:ConnectionString";

    /// <summary>
    /// Returns the connection string of the security (authorization) database.
    /// The value is resolved in the following order:
    /// <list type="number">
    /// <item><description>ConnectionStrings:VirtoCommerce.Auth</description></item>
    /// <item><description>Auth:ConnectionString (obsolete, kept for backward compatibility)</description></item>
    /// <item><description>ConnectionStrings:VirtoCommerce</description></item>
    /// </list>
    /// </summary>
    public static string GetAuthConnectionString(this IConfiguration configuration)
    {
#pragma warning disable VC0016 // Obsolete Auth:ConnectionString setting is read for backward compatibility
        var obsoleteAuthConnectionString = configuration[AuthConnectionStringKey];
#pragma warning restore VC0016

        string[] connectionStrings =
        [
            configuration.GetConnectionString(AuthConnectionStringName),
            obsoleteAuthConnectionString,
            configuration.GetConnectionString(PlatformConnectionStringName),
        ];

        return connectionStrings.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
    }
}
