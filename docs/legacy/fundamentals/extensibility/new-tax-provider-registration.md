
In order to calculate taxes in Virto Commerce you must register at least one `TaxProvider` implementation. 

[Sample code](https://github.com/VirtoCommerce/vc-module-tax/blob/master/src/VirtoCommerce.TaxModule.Data/Provider/FixedRateTaxProvider.cs)

## Define new tax provider

* Create new module ([creating new module](../../developer-guide/create-new-module.md)
* Create a class derived from `TaxProvider` abstract class and override `CalculateRate` method. 
  
```C#
 public class FixedRateTaxProvider : TaxProvider
    {
        public FixedRateTaxProvider()
        {
            Code = "FixedRate";
        }
        public override IEnumerable<TaxRate> CalculateRates(IEvaluationContext context)
        {
          //Implement logic of tax calculation here
        }
    }

```

* Register your module class in the DI container. This must be done in `PostInitialize` method in your module class. You can also associate the settings which will be used in your method and can be changed in the management UI. 
  
```C#
public void PostInitialize(IApplicationBuilder applicationBuilder)
{
  ...
            var settingsRegistrar = applicationBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            var taxProviderRegistrar = applicationBuilder.ApplicationServices.GetRequiredService<ITaxProviderRegistrar>();
            taxProviderRegistrar.RegisterTaxProvider<FixedRateTaxProvider>();
            //Associate the settings with the particular tax provider
            settingsRegistrar.RegisterSettingsForType(Core.ModuleConstants.Settings.FixedTaxProviderSettings.AllSettings, typeof(FixedRateTaxProvider).Name);
  ...
}
```

All settings may have default values that can be used for default methods if not overridden by custom values later.

## Enable and configure tax provider for store

After your module is installed in your target system, all your tax providers should appear and be available for configuration in every store in your system under Store->Tax providers widget. You can configure tax provider settings for each store individually:
* enable/disable method for current store
* edit all settings and what you define for the tax calculation provider
* use a custom UI for a more detailed tax provider configuration

After you complete the configuration, your tax provider will be used for tax calculation of orders in the store.

## Management UI customization

If a standard user interface is not enough, then consider implementing your own UI for managing tax providers through the standard UI extension point (widget container with group ”taxProviderDetail”). You can read more about extending the existing UI with widgets [here](./widgets.md).
