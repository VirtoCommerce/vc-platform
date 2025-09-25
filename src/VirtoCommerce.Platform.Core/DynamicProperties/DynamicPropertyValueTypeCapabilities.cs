using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.DynamicProperties;
public static class DynamicPropertyValueTypeCapabilities
{
    private static Dictionary<DynamicPropertyValueType, DynamicPropertyValueTypeCapability> Capabilities { get; set; } = new Dictionary<DynamicPropertyValueType, DynamicPropertyValueTypeCapability>();

    static DynamicPropertyValueTypeCapabilities()
    {
        RegisterCapability(new DynamicPropertyValueTypeCapability
        {
            ValueType = DynamicPropertyValueType.Boolean,
            SupportArray = false,
            SupportDictionary = false,
            SupportLocalization = false
        });

        RegisterCapability(new DynamicPropertyValueTypeCapability
        {
            ValueType = DynamicPropertyValueType.DateTime,
            SupportArray = false,
            SupportDictionary = false,
            SupportLocalization = false
        });

        RegisterCapability(new DynamicPropertyValueTypeCapability
        {
            ValueType = DynamicPropertyValueType.Decimal,
            SupportArray = true,
            SupportDictionary = false,
            SupportLocalization = false
        });

        RegisterCapability(new DynamicPropertyValueTypeCapability
        {
            ValueType = DynamicPropertyValueType.Html,
            SupportArray = false,
            SupportDictionary = false,
            SupportLocalization = true
        });

        RegisterCapability(new DynamicPropertyValueTypeCapability
        {
            ValueType = DynamicPropertyValueType.Image,
            SupportArray = false,
            SupportDictionary = false,
            SupportLocalization = false
        });

        RegisterCapability(new DynamicPropertyValueTypeCapability
        {
            ValueType = DynamicPropertyValueType.Integer,
            SupportArray = true,
            SupportDictionary = false,
            SupportLocalization = false
        });

        RegisterCapability(new DynamicPropertyValueTypeCapability
        {
            ValueType = DynamicPropertyValueType.LongText,
            SupportArray = true,
            SupportDictionary = false,
            SupportLocalization = true
        });

        RegisterCapability(new DynamicPropertyValueTypeCapability
        {
            ValueType = DynamicPropertyValueType.ShortText,
            SupportArray = true,
            SupportDictionary = true,
            SupportLocalization = true
        });
    }

    public static DynamicPropertyValueTypeCapability GetCapability(DynamicPropertyValueType valueType)
    {
        if (Capabilities == null)
        {
            throw new InvalidOperationException("DynamicPropertyValueTypeCapabilities not initialized.");
        }
        if (!Capabilities.TryGetValue(valueType, out var capability))
        {
            throw new ArgumentException($"No capabilities defined for value type: {valueType}");
        }
        return capability;
    }

    public static void RegisterCapability(DynamicPropertyValueTypeCapability capability)
    {
        if (Capabilities.ContainsKey(capability.ValueType))
        {
            throw new ArgumentException($"Capability for value type {capability.ValueType} is already registered.");
        }
        Capabilities[capability.ValueType] = capability;
    }

}
