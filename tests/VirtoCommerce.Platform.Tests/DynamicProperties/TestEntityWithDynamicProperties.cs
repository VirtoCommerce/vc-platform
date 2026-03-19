using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Tests.DynamicProperties;
public class TestEntityWithDynamicProperties : AuditableEntity, IHasDynamicProperties
{
    public string ObjectType => typeof(TestEntityWithDynamicProperties).FullName;

    public ICollection<DynamicObjectProperty> DynamicProperties { get; set; } = [];


    private DynamicPropertyAccessor _dynamicPropertyAccessor;

    public DynamicPropertyAccessor DynamicPropertyAccessor
    {
        get
        {
            _dynamicPropertyAccessor ??= new DynamicPropertyAccessor(this);
            return _dynamicPropertyAccessor;
        }
        set
        {
            _dynamicPropertyAccessor = value;
            _dynamicPropertyAccessor.ConnectEntity(this);
        }
    }

    public string[] ShortTextField_MultiValue
    {
        get
        {
            dynamic dynamicPropertyAccessor = DynamicPropertyAccessor;
            return dynamicPropertyAccessor.ShortTextField_MultiValue;
        }
        set
        {
            dynamic dynamicPropertyAccessor = DynamicPropertyAccessor;
            dynamicPropertyAccessor.ShortTextField_MultiValue = value;
        }
    }

    public int? IntegerFieldSingleValue
    {
        get
        {
            dynamic dynamicPropertyAccessor = DynamicPropertyAccessor;
            return dynamicPropertyAccessor.IntegerFieldSingleValue;
        }
        set
        {
            dynamic dynamicPropertyAccessor = DynamicPropertyAccessor;
            dynamicPropertyAccessor.IntegerFieldSingleValue = value;
        }
    }
}
