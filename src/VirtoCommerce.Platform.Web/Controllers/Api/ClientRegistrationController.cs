using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using static OpenIddict.Abstractions.OpenIddictConstants;
using AuthorizationOptions = VirtoCommerce.Platform.Core.Security.AuthorizationOptions;

namespace VirtoCommerce.Platform.Web.Controllers.Api;

/// <summary>
/// Dynamic client registration (RFC 7591), off unless Authorization:ClientRegistration:DynamicRegistration:Enabled is true.
/// </summary>
[AllowAnonymous]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class ClientRegistrationController : Controller
{
    private const int MaxRequestSize = 16 * 1024;

    private readonly IDynamicClientRegistrationService _registrationService;
    private readonly IOptionsMonitor<AuthorizationOptions> _options;

    public ClientRegistrationController(
        IDynamicClientRegistrationService registrationService,
        IOptionsMonitor<AuthorizationOptions> options)
    {
        _registrationService = registrationService;
        _options = options;
    }

    /// <summary>
    /// Register an OAuth client (RFC 7591)
    /// </summary>
    [HttpPost("~/" + AttachClientRegistrationMetadata.RegistrationEndpointPath)]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [RequestSizeLimit(MaxRequestSize)]
    public async Task<IActionResult> Register()
    {
        if (!_options.CurrentValue.ClientRegistration.DynamicRegistration.Enabled)
        {
            return NotFound();
        }

        // Read as it is: the MVC formatters would bind the metadata with other names and types
        JsonElement metadata;
        try
        {
            using var document = await JsonDocument.ParseAsync(Request.Body, cancellationToken: HttpContext.RequestAborted);
            metadata = document.RootElement.Clone();
        }
        catch (JsonException)
        {
            metadata = default;
        }

        var result = await _registrationService.RegisterAsync(metadata, HttpContext.RequestAborted);
        if (result.Error != null)
        {
            return BadRequest(new Dictionary<string, string>
            {
                [Parameters.Error] = result.Error,
                [Parameters.ErrorDescription] = result.ErrorDescription,
            });
        }

        return StatusCode(StatusCodes.Status201Created, new Dictionary<string, object>
        {
            [ClientMetadata.Names.ClientId] = result.ClientId,
            [ClientMetadata.Names.ClientIdIssuedAt] = result.IssuedAt.ToUnixTimeSeconds(),
            [ClientMetadata.Names.ClientName] = result.ClientName,
            [ClientMetadata.Names.RedirectUris] = result.RedirectUris,
            [ClientMetadata.Names.GrantTypes] = result.GrantTypes,
            [ClientMetadata.Names.ResponseTypes] = result.ResponseTypes,
            [ClientMetadata.Names.TokenEndpointAuthMethod] = ClientAuthenticationMethods.None,
        });
    }
}
