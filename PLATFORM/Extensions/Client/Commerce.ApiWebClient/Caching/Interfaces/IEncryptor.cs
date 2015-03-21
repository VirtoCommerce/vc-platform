namespace VirtoCommerce.ApiWebClient.Caching.Interfaces
{
    public interface IEncryptor
    {
        /// <summary>
        /// Implementations should encrypt the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>An encrypted representation of <see cref="plainText"/></returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Implementations should Decrypt the specified encrypted text.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <returns>The original text</returns>
        string Decrypt(string encryptedText);
    }
}
