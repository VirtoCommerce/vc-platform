namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

public class ClientIdMetadataDocumentResult
{
    public ClientIdMetadataDocument Document { get; init; }

    public string Error { get; init; }

    /// <summary>
    /// A timeout or a server error, which may pass.
    /// </summary>
    public bool IsTransient { get; init; }

    public static ClientIdMetadataDocumentResult Success(ClientIdMetadataDocument document) => new() { Document = document };

    public static ClientIdMetadataDocumentResult Fail(string error, bool isTransient = false) => new() { Error = error, IsTransient = isTransient };
}
