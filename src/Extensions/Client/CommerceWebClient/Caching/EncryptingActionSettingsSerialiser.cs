using VirtoCommerce.Web.Client.Caching.Interfaces;
using IActionSettingsSerialiser = VirtoCommerce.Web.Client.Caching.Interfaces.IActionSettingsSerialiser;
using IEncryptor = VirtoCommerce.Web.Client.Caching.Interfaces.IEncryptor;

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

        public string Serialise(VirtoCommerce.Web.Client.Caching.ActionSettings actionSettings)
        {
            var serialisedActionSettings = _serialiser.Serialise(actionSettings);

            return _encryptor.Encrypt(serialisedActionSettings);
        }

        public VirtoCommerce.Web.Client.Caching.ActionSettings Deserialise(string serialisedActionSettings)
        {
            var decryptedSerialisedActionSettings = _encryptor.Decrypt(serialisedActionSettings);

            return _serialiser.Deserialise(decryptedSerialisedActionSettings);
        }
    }
}
