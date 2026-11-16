using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenIddict.Server;
using VirtoCommerce.Platform.Core.Security;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

public class AttachClientRegistrationMetadata : IOpenIddictServerHandler<HandleConfigurationRequestContext>
{
    public const string RegistrationEndpointPath = "connect/register";

    private readonly IOptionsMonitor<AuthorizationOptions> _options;

    public AttachClientRegistrationMetadata(IOptionsMonitor<AuthorizationOptions> options)
    {
        _options = options;
    }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; } = OpenIddictServerHandlerDescriptor
        .CreateBuilder<HandleConfigurationRequestContext>()
        .UseSingletonHandler<AttachClientRegistrationMetadata>()
        .SetOrder(OpenIddictServerHandlers.Discovery.AttachAdditionalMetadata.Descriptor.Order + 1000)
        .SetType(OpenIddictServerHandlerType.Custom)
        .Build();

    public virtual ValueTask HandleAsync(HandleConfigurationRequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var options = _options.CurrentValue.ClientRegistration;

        if (options.ClientIdMetadataDocuments.Enabled)
        {
            context.Metadata["client_id_metadata_document_supported"] = true;
        }

        if (options.DynamicRegistration.Enabled && context.BaseUri != null)
        {
            context.Metadata["registration_endpoint"] = new Uri(context.BaseUri, RegistrationEndpointPath).AbsoluteUri;
        }

        return ValueTask.CompletedTask;
    }
}
