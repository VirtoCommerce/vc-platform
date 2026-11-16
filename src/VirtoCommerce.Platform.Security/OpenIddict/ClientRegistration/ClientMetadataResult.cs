namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

public class ClientMetadataResult
{
    public ClientMetadata Metadata { get; init; }

    public string Error { get; init; }

    public string ErrorDescription { get; init; }

    public static ClientMetadataResult Success(ClientMetadata metadata) => new() { Metadata = metadata };

    public static ClientMetadataResult Fail(string error, string errorDescription) => new() { Error = error, ErrorDescription = errorDescription };
}
