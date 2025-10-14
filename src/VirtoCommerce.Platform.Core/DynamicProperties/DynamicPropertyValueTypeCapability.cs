namespace VirtoCommerce.Platform.Core.DynamicProperties;
public class DynamicPropertyValueTypeCapability
{
    public DynamicPropertyValueType ValueType { get; set; }

    public bool SupportArray { get; set; }

    public bool SupportDictionary { get; set; }

    public bool SupportLocalization { get; set; }
}
