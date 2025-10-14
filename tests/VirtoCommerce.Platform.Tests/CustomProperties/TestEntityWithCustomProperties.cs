using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Tests.DynamicProperties;

namespace VirtoCommerce.Platform.Tests.DynamicProperties2;
public class TestEntityWithCustomProperties : AuditableEntity, IHasDynamicProperties
{
    public string ObjectType => typeof(TestEntityWithDynamicProperties).FullName;
    public ICollection<DynamicObjectProperty> DynamicProperties { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public TestEntityWithCustomProperties()
    {

    }

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

}
