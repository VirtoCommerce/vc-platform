using System;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Common
{
    public class CountriesServiceTests
    {
        [Fact]
        public void CanGetCountries()
        {
            var filesystemCountryService = new Mock<ICountriesService>();
            var service = new CountriesService(filesystemCountryService.Object as FileSystemCountriesService);

            var countries = service.GetCountriesAsync().GetAwaiter().GetResult();

            Assert.Equal(249, countries.Count);
        }

        [Theory]
        [InlineData("RU", "RUS", "Russian Federation")]
        [InlineData("US", "USA", "United States of America")]
        public void CanGetCountryByCode(string iso2, string iso3, string expectedName)
        {
            var filesystemCountryService = new Mock<ICountriesService>();
            var service = new CountriesService(filesystemCountryService.Object as FileSystemCountriesService);

            var country = service.GetByCode(iso2);

            Assert.Equal(expectedName, country.Name);
            Assert.Equal(iso3, country.Id);

            country = service.GetByCode(iso3);

            Assert.Equal(expectedName, country.Name);
            Assert.Equal(iso3, country.Id);
        }

        [Theory]
        [InlineData("-")]
        [InlineData("--")]
        public void CanThrowOnIncorrectCode(string code)
        {
            var filesystemCountryService = new Mock<ICountriesService>();
            var service = new CountriesService(filesystemCountryService.Object as FileSystemCountriesService);
            Assert.Throws<ArgumentException>(() => service.GetByCode(code));
        }
    }
}
