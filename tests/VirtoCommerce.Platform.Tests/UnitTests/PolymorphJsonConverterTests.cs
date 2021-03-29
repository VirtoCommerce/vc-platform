using FluentAssertions;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.JsonConverters;
using VirtoCommerce.Platform.Core.Security;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    class PermissionScopeExt1 : PermissionScope
    {
    }

    class PermissionScopeExt2 : PermissionScope
    {
    }

    class PermissionScopeOverridden : PermissionScope
    {
        public string Extension { get; set; }
    }

    public class PolymorphJsonConverterTests
    {
        /// <summary>
        /// Test PolymorphJsonConverter returns proper instances of overridden types
        /// </summary>
        [Fact]        
        public void DeserializeObject_DeserializeOverridden()
        {
            AbstractTypeFactory<PermissionScope>.OverrideType<PermissionScope, PermissionScopeOverridden>();
            var jsonOver = "{'scope':'ScopeOver', 'extension':'ext'}".Replace("'", "\"");
            var scopeOver = JsonConvert.DeserializeObject<PermissionScope>(jsonOver, new PolymorphJsonConverter());
            scopeOver.Should().NotBeNull();
            scopeOver.Should().BeOfType<PermissionScopeOverridden>();
            (scopeOver as PermissionScopeOverridden).Extension.Should().Be("ext");
        }


        /// <summary>
        /// Test PolymorphJsonConverter returns proper instances of discriminator-defined types and defaults
        /// </summary>
        [Fact]
        public void DeserializeObject_DeserializeWithDiscriminator()
        {
            //Arrange
            AbstractTypeFactory<PermissionScope>.RegisterType<PermissionScope>();
            AbstractTypeFactory<PermissionScope>.RegisterType<PermissionScopeExt1>();
            AbstractTypeFactory<PermissionScope>.RegisterType<PermissionScopeExt2>();

            var json0 = "{'scope':'Scope000'}".Replace("'", "\"");
            var json1 = "{'Type':'PermissionScopeExt1','scope':'Scope111'}".Replace("'", "\"");
            var json2 = "{'Type':'PermissionScopeExt2','scope':'Scope123'}".Replace("'", "\"");

            var converter = new PolymorphJsonConverter();
            PolymorphJsonConverter.RegisterTypeForDiscriminator(typeof(PermissionScope), nameof(PermissionScope.Type));

            //Act
            var scope0 = JsonConvert.DeserializeObject<PermissionScope>(json0, converter);

            //Assert
            scope0.Should().NotBeNull();
            scope0.Should().BeOfType<PermissionScope>();
            scope0.Should().NotBeOfType<PermissionScopeExt1>();
            scope0.Should().NotBeOfType<PermissionScopeExt2>();

            //Act
            var scope1 = JsonConvert.DeserializeObject<PermissionScope>(json1, converter);

            //Assert
            scope1.Should().NotBeNull();
            scope1.Should().BeOfType<PermissionScopeExt1>();
            scope1.Should().NotBeOfType<PermissionScope>();
            scope1.Should().NotBeOfType<PermissionScopeExt2>();

            //Act
            var scope2 = JsonConvert.DeserializeObject<PermissionScope>(json2, converter);

            //Assert
            scope2.Should().NotBeNull();
            scope2.Should().BeOfType<PermissionScopeExt2>();
            scope2.Should().NotBeOfType<PermissionScope>();
            scope2.Should().NotBeOfType<PermissionScopeExt1>();
        }
    }
}

