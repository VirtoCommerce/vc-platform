
If customers have shippable products, they can choose a shipping option during checkout.

[Sample code](https://github.com/VirtoCommerce/vc-module-shipping/blob/master/src/VirtoCommerce.ShippingModule.Data/FixedRateShippingMethod.cs)

## Define new shipping method

* Create new module ([create new module](../../developer-guide/create-new-module.md)
* Create a class derived from `TaxProvider` abstract class and overrides `CalculateRate` method. 
  
```C#
  public class FixedRateShippingMethod : ShippingMethod
    {
        public FixedRateShippingMethod() : base("FixedRate")
        {
        }
		public override IEnumerable<ShippingRate> CalculateRates(IEvaluationContext context)
        { 
			//Implement logic of shipping rates calculation here
		}
	}
```

* Register your module class in the DI container. This must be done in `PostInitialize` method. You can also associate the settings which will be used in your method and can be changed in the management UI. 
  
```C#
public void PostInitialize(IApplicationBuilder applicationBuilder)
{
  ...
     
	 	var settingsRegistrar = applicationBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
        var shippingMethodsRegistrar = applicationBuilder.ApplicationServices.GetRequiredService<IShippingMethodsRegistrar>();
		//Associate the settings with the particular shipping provider
		settingsRegistrar.RegisterSettingsForType(ModuleConstants.Settings.FixedRateShippingMethod.AllSettings, typeof(FixedRateShippingMethod).Name);
        shippingMethodsRegistrar.RegisterShippingMethod<FixedRateShippingMethod>();

  ...
}
```

All settings may have default values that can be used for default methods if not overridden by custom values later.


## Enable and configure shipping method for store

After your module is installed in your target system, all your shipping methods should appear and be available for configuration in every store in your system. Store->Shipping methods widget. You can configure shipping methods for each store individually:
* enable/disable method for current store
* change priority (to determine in what order they will be displayed in the checkout)
* edit all settings and what you define for the shipping method
* use a custom UI for a more detailed shipping method configuration

After you complete the configuration, your shipping method will be appear in the FrontEnd checkout page and the customer may select it as a shipping method.

## Management UI customization

If a standard user interface is not enough, then consider implementing your own UI for managing tax providers through the standard UI extension point (widget container with group ”shippingMethodDetail”). You can read more about extending the existing UI with widgets [here](./widgets.md).
