namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Turns an API key into the value the platform stores and looks keys up by, and into the hint it shows.
    /// Replace the registration to hash differently (for example HMAC with a secret kept outside the database);
    /// keys stored with the previous hasher stop matching, so a replacement needs the keys reissued.
    /// </summary>
    public interface IApiKeyHasher
    {
        string Hash(string apiKey);

        string GetHint(string apiKey);
    }
}
