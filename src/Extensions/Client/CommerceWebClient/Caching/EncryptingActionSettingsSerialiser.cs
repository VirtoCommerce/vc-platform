using VirtoCommerce.Web.Client.Caching.Interfaces;

namespace VirtoCommerce.Web.Client.Caching
{
    public class EncryptingActionSettingsSerialiser : IEncryptingActionSettingsSerialiser
    {
        private readonly IActionSettingsSerialiser _serialiser;
        private readonly IEncryptor _encryptor;

        public EncryptingActionSettingsSerialiser(IActionSettingsSerialiser serialiser, IEncryptor encryptor)
        {
            _serialiser = serialiser;
            _encryptor = encryptor;
        }

        public string Serialise(ActionSettings actionSettings)
        {
            var serialisedActionSettings = _serialiser.Serialise(actionSettings);

            return _encryptor.Encrypt(serialisedActionSettings);
        }

        public ActionSettings Deserialise(string serialisedActionSettings)
        {
            var decryptedSerialisedActionSettings = _encryptor.Decrypt(serialisedActionSettings);

            return _serialiser.Deserialise(decryptedSerialisedActionSettings);
        }
    }
}
