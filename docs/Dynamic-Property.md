# Introduction
Made changes in working with the Dynamic Properties, namely, have divided values and meta-info.
It means, each model(which has dynamic properties) collect dynamic property values in separate table from meta-info.
This allowed to improved performance and to separate the logic on working with the properties. Allowed to reduce the size of models/data-transfer-objects.

> look at https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Model/MemberDynamicPropertyObjectValueEntity.cs

# How to make changes for extension models
E.g. There is a extension class ShoppingCart2.cs and ShoppingCart2Entity.cs

### Working with ShoppingCart2
Then need to add a class, it has to be called ShoppingCart2DynamicPropertyObjectValueEntity.cs, which inherit from abstract class DynamicPropertyObjectValueEntity

```
public class ShoppingCart2DynamicPropertyObjectValueEntity : DynamicPropertyObjectValueEntity
{
   public virtual ShoppingCart2Entity ShoppingCart { get; set; }
}
```

And then to add collection DynamicPropertyObjectValues to ShoppingCart2Entity

```
public virtual ObservableCollection<ShoppingCart2DynamicPropertyObjectValueEntity> DynamicPropertyObjectValues { get; set; }
            = new NullCollection<ShoppingCart2DynamicPropertyObjectValueEntity>();
```

Then make changes in ShoppingCart2Entity:

* add the code to method ToModel

```
cart.DynamicProperties = DynamicPropertyObjectValues.GroupBy(g => g.PropertyId).Select(x =>
{
	var property = AbstractTypeFactory<DynamicObjectProperty>.TryCreateInstance();
	property.Id = x.Key;
	property.Name = x.FirstOrDefault()?.PropertyName;
	property.Values = x.Select(v => v.ToModel(AbstractTypeFactory<DynamicPropertyObjectValue>.TryCreateInstance())).ToArray();
	return property;
}).ToArray();
```

* add the code to method FromModel

```
if (cart.DynamicProperties != null)
{
	DynamicPropertyObjectValues = new ObservableCollection<ShoppingCart2DynamicPropertyObjectValueEntity>(cart.DynamicProperties.SelectMany(p => p.Values
		.Select(v => AbstractTypeFactory<ShoppingCart2DynamicPropertyObjectValueEntity>.TryCreateInstance().FromModel(v, cart, p))).OfType<Shopping2CartDynamicPropertyObjectValueEntity>());
}
```

* add the code to method Patch

```
if (!DynamicPropertyObjectValues.IsNullCollection())
{
	DynamicPropertyObjectValues.Patch(target.DynamicPropertyObjectValues, (sourceDynamicPropertyObjectValues, targetDynamicPropertyObjectValues) => sourceDynamicPropertyObjectValues.Patch(targetDynamicPropertyObjectValues));
}
```

### Working with Cart2DbContext
Need to make mapping with ShoppingCart2DynamicPropertyObjectValueEntity 

```
modelBuilder.Entity<ShoppingCart2DynamicPropertyObjectValueEntity>().ToTable("Cart2DynamicPropertyObjectValue").HasKey(x => x.Id);
modelBuilder.Entity<ShoppingCart2DynamicPropertyObjectValueEntity>().Property(x => x.Id).HasMaxLength(128);
modelBuilder.Entity<ShoppingCart2DynamicPropertyObjectValueEntity>().Property(x => x.DecimalValue).HasColumnType("decimal(18,5)");
modelBuilder.Entity<ShoppingCart2DynamicPropertyObjectValueEntity>().HasOne(p => p.ShoppingCart)
	.WithMany(s => s.DynamicPropertyObjectValues).HasForeignKey(k => k.ObjectId)
	.OnDelete(DeleteBehavior.Cascade);
modelBuilder.Entity<ShoppingCart2DynamicPropertyObjectValueEntity>().HasIndex(x => new { x.ObjectType, x.ObjectId })
	.IsUnique(false)
	.HasName("IX_ObjectType_ObjectId");
```

### Working with Cart2Repository
Need to add IQueryable<ShoppingCart2DynamicPropertyObjectValueEntity>
```
protected IQueryable<ShoppingCartDynamicPropertyObjectValueEntity> DynamicPropertyObjectValues => DbContext.Set<ShoppingCartDynamicPropertyObjectValueEntity>();
```
Then need to add a logic to GetShoppingCartsByIdsAsync which will collect values from database
```
await DynamicPropertyObjectValues.Where(x => ids.Contains(x.ObjectId)).ToArrayAsync()
```
and if need to add a logic of working with ResponseGroup.

### Migration
Then need to add migration which will create a table Cart2DynamicPropertyObjectValue and collect values to the table.
use the command in the Package Manage Console
```
Add-Migration AddCart2DynamicPropertyObjectValue -Context VirtoCommerce.CartModule.Data.Repositories.CartDbContext -StartupProject VirtoCommerce.CartModule.Data  -Verbose -OutputDir Migrations
```

> look at https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Migrations/20190628091513_AddCustomerDynamicPropertyObjectValue.cs.
 There's custom SQL script added to the migration. It's recommended to combine this script in a single migration: https://github.com/VirtoCommerce/vc-module-customer/blob/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Migrations/20000000000000_UpdateCustomerV2.cs#L61-L71




