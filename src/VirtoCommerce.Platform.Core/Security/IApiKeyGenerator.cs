namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Makes new API keys. Replace the registration to change the key format.
    /// </summary>
    public interface IApiKeyGenerator
    {
        string Generate();
    }
}
