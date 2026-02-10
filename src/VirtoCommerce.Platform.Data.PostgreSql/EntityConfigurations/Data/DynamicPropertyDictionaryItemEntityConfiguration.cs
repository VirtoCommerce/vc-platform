using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;

namespace VirtoCommerce.Platform.Data.PostgreSql.EntityConfigurations.Data;

public class DynamicPropertyDictionaryItemEntityConfiguration : IEntityTypeConfiguration<DynamicPropertyDictionaryItemEntity>
{
    public void Configure(EntityTypeBuilder<DynamicPropertyDictionaryItemEntity> builder)
    {
        builder.Property(x => x.Name).UseCollation(CaseInsensitive.CollationName);
    }
}
