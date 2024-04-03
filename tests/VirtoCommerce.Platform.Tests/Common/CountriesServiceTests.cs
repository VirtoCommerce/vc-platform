using System;
using System.Threading.Tasks;
using Moq;
using Nager.Country;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Common
{
    public class CountriesServiceTests
    {
        [Fact]
        public async Task CanGetCountries()
        {
            var filesystemCountryService = new Mock<ICountriesService>();
            var service = new CountriesService(filesystemCountryService.Object as FileSystemCountriesService);

            var countries = await service.GetCountriesAsync();

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
        [InlineData("--")]
        [InlineData("---")]
        public void CanThrowOnIncorrectCode(string code)
        {
            var filesystemCountryService = new Mock<ICountriesService>();
            var service = new CountriesService(filesystemCountryService.Object as FileSystemCountriesService);
            Assert.Throws<UnknownCountryException>(() => service.GetByCode(code));
        }

        [Theory]
        [InlineData("-")]
        public void CanThrowOnIncorrectCode2(string code)
        {
            var filesystemCountryService = new Mock<ICountriesService>();
            var service = new CountriesService(filesystemCountryService.Object as FileSystemCountriesService);
            Assert.Throws<ArgumentException>(() => service.GetByCode(code));
        }
    }
}
