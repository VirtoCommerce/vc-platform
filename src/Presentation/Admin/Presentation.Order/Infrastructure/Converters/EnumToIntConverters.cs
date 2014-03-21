using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Converters;

namespace VirtoCommerce.ManagementClient.Order.Infrastructure.Converters
{
    public class JurisdictionTypesConverter : EnumToIntConverter<JurisdictionTypes> { }
    public class TaxTypesConverter : EnumToIntConverter<TaxTypes> { }
}
