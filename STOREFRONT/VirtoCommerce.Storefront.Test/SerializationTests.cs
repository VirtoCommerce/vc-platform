using Newtonsoft.Json;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Quote;
using Xunit;

namespace VirtoCommerce.Storefront.Test
{
    public class SerializationTests
    {
        private static readonly Language _language = new Language("en-US");
        private static readonly Currency _currency = new Currency(_language, "USD");
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        [Fact]
        public void Money()
        {
            var originalObject = new Money(_currency);
            var expectedJson = JsonConvert.SerializeObject(originalObject, _settings);

            var deserializedObject = JsonConvert.DeserializeObject<Money>(expectedJson, _settings);
            var actualJson = JsonConvert.SerializeObject(deserializedObject, _settings);

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void QuoteRequest()
        {
            var originalObject = new QuoteRequest(_currency, _language);
            var expectedJson = JsonConvert.SerializeObject(originalObject, _settings);

            var deserializedObject = JsonConvert.DeserializeObject<QuoteRequest>(expectedJson, _settings);
            var actualJson = JsonConvert.SerializeObject(deserializedObject, _settings);

            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void MutablePagedList()
        {
            var items = new[] { "1", "2", "3" };
            var originalObject = new MutablePagedList<string>(items);
            var expectedJson = JsonConvert.SerializeObject(originalObject, _settings);

            var deserializedObject = JsonConvert.DeserializeObject<MutablePagedList<string>>(expectedJson, _settings);
            var actualJson = JsonConvert.SerializeObject(deserializedObject, _settings);

            Assert.Equal(expectedJson, actualJson);
        }
    }
}
