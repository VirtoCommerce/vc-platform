using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VirtoCommerce.Platform.Data.PostgreSql.Extensions;

public static class PropertyBuilderExtension
{
    public static void UseCaseInsensitiveCollation<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
    {
        propertyBuilder.UseCollation(CollationNames.CaseInsensitive);
    }
}
