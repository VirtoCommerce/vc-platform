using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties;

[JsonConverter(typeof(DynamicPropertyAccessorJsonConverter))]
public class DynamicPropertyAccessor : DynamicObject
{
    private readonly IDictionary<string, object> _properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

    private string _parentObjectType;
    private IHasDynamicProperties _connectedEntity;

    protected string ParentObjectType { get { return _parentObjectType; } }

    public DynamicPropertyAccessor()
    {

    }

    public DynamicPropertyAccessor(IHasDynamicProperties parentEntity)
    {
        ArgumentNullException.ThrowIfNull(parentEntity);

        _parentObjectType = parentEntity.ObjectType;
        _connectedEntity = parentEntity;
    }

    public DynamicPropertyAccessor(IDictionary<string, object> properties)
    {
        ArgumentNullException.ThrowIfNull(properties);

        foreach (var kvp in properties)
        {
            _properties[kvp.Key] = kvp.Value;
        }
    }

    public DynamicPropertyAccessor(string objectType, IDictionary<string, object> properties = null)
    {
        ArgumentNullException.ThrowIfNull(objectType);

        _parentObjectType = objectType;

        if (properties != null)
        {
            foreach (var kvp in properties)
            {
                _properties[kvp.Key] = kvp.Value;
            }
        }
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        return TryGetPropertyValue(binder.Name, out result);
    }

    public bool TryGetPropertyValue(string propertyName, out object result)
    {
        var metaProperty = GetDynamicPropertyByName(propertyName);

        if (metaProperty == null)
        {
            result = null;
            return false;
        }

        result = GetPropertyValue(metaProperty);

        return true;
    }

    protected object GetPropertyValue(DynamicProperty metaProperty)
    {
        object result;

        if (IsConnected())
        {
            result = GetValueFromConnectEntity(metaProperty);
        }
        else
        {
            result = GetValueFromDisconnectedEntity(metaProperty);
        }

        return result;
    }

    protected object GetValueFromDisconnectedEntity(DynamicProperty metaProperty)
    {
        if (!_properties.TryGetValue(metaProperty.Name, out var result))
        {
            result = null;
        }

        return result;
    }

    protected object GetValueFromConnectEntity(DynamicProperty metaProperty)
    {
        object result;
        var prop = GetConnectedEntity().DynamicProperties.FirstOrDefault(p => p.Name.Equals(metaProperty.Name, StringComparison.OrdinalIgnoreCase));

        if (metaProperty.IsArray)
        {
            if (metaProperty.IsMultilingual)
            {
                return null; // Multilingual arrays are not supported
            }
            else
            {
                // Simplified array creation and assignment
                var values = prop?.Values?.Select(v => v.Value).ToArray() ?? [];

                var clrType = DynamicPropertyValueTypeToClrType(metaProperty);
                var array = Array.CreateInstance(clrType, values.Length);

                if (clrType == typeof(decimal) || clrType == typeof(int))
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        if (values[i] == null)
                        {
                            array.SetValue(null, i);
                            continue;
                        }

                        // Convert.ChangeType handles int, float, double -> decimal automatically
                        var converted = Convert.ChangeType(values[i], clrType);
                        array.SetValue(converted, i);
                    }
                }
                else
                {
                    Array.Copy(values, array, values.Length);
                }
                result = array;
            }
        }
        else
        {
            if (metaProperty.IsMultilingual)
            {
                var localizedString = new LocalizedString();

                foreach (var propValue in prop?.Values ?? [])
                {
                    localizedString.SetValue(propValue.Locale, (string)propValue.Value);
                }

                result = localizedString;
            }
            else
            {
                result = prop?.Values?.FirstOrDefault()?.Value;
            }
        }

        return result;
    }

    private DynamicProperty GetDynamicPropertyByName(string propertyName)
    {
        var metaProperties = DynamicPropertyMetadata.GetProperties(this.ParentObjectType).GetAwaiter().GetResult();
        var metaProperty = metaProperties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
        return metaProperty;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        return TrySetPropertyValue(binder.Name, value);
    }

    public bool TrySetPropertyValue(string propertyName, object value)
    {
        var metaProperty = GetDynamicPropertyByName(propertyName);

        if (metaProperty == null)
        {
            return false;
        }

        // Validate value type consistency
        if (!IsValueCompatible(metaProperty, value))
        {
            // Value type mismatch
            return false;
        }

        SetPropertyValue(metaProperty, value);

        return true;
    }

    protected void SetPropertyValue(DynamicProperty metaProperty, object value)
    {
        if (IsConnected())
        {
            SetPropertyValueToConnectedEntity(metaProperty, value);
        }
        else
        {
            SetPropertyValueToDisconnectedEntity(metaProperty, value);
        }
    }

    protected void SetPropertyValueToDisconnectedEntity(DynamicProperty metaProperty, object value)
    {
        _properties[metaProperty.Name] = value;
    }

    protected void SetPropertyValueToConnectedEntity(DynamicProperty metaProperty, object value)
    {
        // Find or create the property in the entity
        var dynamicProperties = GetConnectedEntity().DynamicProperties;
        var prop = dynamicProperties.FirstOrDefault(p => p.Name.Equals(metaProperty.Name, StringComparison.OrdinalIgnoreCase));

        if (prop == null)
        {
            prop = new DynamicObjectProperty();
            dynamicProperties.Add(prop);
        }

        // Refresh Metadata
        prop.Id = metaProperty.Id;
        prop.Name = metaProperty.Name;
        prop.ObjectType = _connectedEntity.ObjectType;
        prop.ValueType = metaProperty.ValueType;

        // Create a new list of values
        var values = new List<DynamicPropertyObjectValue>();

        if (metaProperty.IsArray)
        {
            if (value is System.Collections.IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    values.Add(new DynamicPropertyObjectValue
                    {
                        ObjectType = _connectedEntity.ObjectType,
                        ObjectId = _connectedEntity.Id,
                        PropertyId = prop.Id,
                        PropertyName = metaProperty.Name,
                        ValueType = metaProperty.ValueType,
                        Locale = null,
                        Value = item,
                    });
                }
            }
        }
        else
        {
            if (metaProperty.IsMultilingual)
            {
                var localizedString = value as LocalizedString;
                if (localizedString != null)
                {
                    foreach (var localizedItem in localizedString.Values)
                    {
                        values.Add(new DynamicPropertyObjectValue
                        {
                            ObjectType = _connectedEntity.ObjectType,
                            ObjectId = _connectedEntity.Id,
                            PropertyId = prop.Id,
                            PropertyName = metaProperty.Name,
                            Locale = localizedItem.Key,
                            Value = localizedItem.Value,
                        });
                    }
                }
            }
            else
            {
                values.Add(new DynamicPropertyObjectValue
                {
                    ObjectType = _connectedEntity.ObjectType,
                    ObjectId = _connectedEntity.Id,
                    PropertyId = prop.Id,
                    PropertyName = metaProperty.Name,
                    Locale = null,
                    Value = value,
                });
            }
        }

        prop.Values = values;
    }

    protected virtual bool IsValueCompatible(DynamicProperty dynamicProperty, object value)
    {
        if (value == null)
        {
            return true;
        }

        if (dynamicProperty.IsArray)
        {
            if (DynamicPropertyValueTypeCapabilities.GetCapability(dynamicProperty.ValueType).SupportArray
                    && value is System.Collections.IEnumerable enumerable
                    && value is not string)
            {
                foreach (var item in enumerable)
                {
                    if (!IsSingleValueCompatible(dynamicProperty, item))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        else
        {
            return IsSingleValueCompatible(dynamicProperty, value);
        }
    }

    protected virtual bool IsSingleValueCompatible(DynamicProperty dynamicProperty, object val)
    {
        if (dynamicProperty.ValueType == DynamicPropertyValueType.Decimal && val is decimal or double or float or int or long)
        {
            return true;
        }

        if (dynamicProperty.ValueType == DynamicPropertyValueType.Integer && val is int or long)
        {
            return true;
        }

        var clrType = DynamicPropertyValueTypeToClrType(dynamicProperty);

        return clrType.IsInstanceOfType(val);
    }

    protected virtual Type DynamicPropertyValueTypeToClrType(DynamicProperty dynamicProperty)
    {
        if (dynamicProperty.IsMultilingual)
        {
            return typeof(LocalizedString);
        }

        return dynamicProperty.ValueType switch
        {
            DynamicPropertyValueType.ShortText => typeof(string),
            DynamicPropertyValueType.LongText => typeof(string),
            DynamicPropertyValueType.Integer => typeof(int),
            DynamicPropertyValueType.Decimal => typeof(decimal),
            DynamicPropertyValueType.Boolean => typeof(bool),
            DynamicPropertyValueType.DateTime => typeof(DateTime),
            DynamicPropertyValueType.Html => typeof(string),
            DynamicPropertyValueType.Image => typeof(string),
            _ => typeof(object)
        };
    }

    public bool IsConnected()
    {
        return GetConnectedEntity() != null;
    }

    public IHasDynamicProperties GetConnectedEntity()
    {
        return _connectedEntity;
    }

    public void ConnectEntity(IHasDynamicProperties parentEntity, bool copyProperties = false)
    {
        ArgumentNullException.ThrowIfNull(parentEntity);

        _parentObjectType = parentEntity.ObjectType;
        _connectedEntity = parentEntity;

        if (copyProperties)
        {
            foreach (var propName in _properties.Keys)
            {
                TrySetPropertyValue(propName, _properties[propName]);
            }
        }

        _properties.Clear();
    }

}
