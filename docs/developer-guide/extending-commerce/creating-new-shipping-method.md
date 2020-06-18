---
title: Creating new shipping method
description: The article will explain to you the required steps for creating a module and extending the system with a new shipping method
layout: docs
date: 2015-12-28T10:30:00.003Z
priority: 3
---
If customers have shippable products, they can choose a shipping option during checkout. The shipping rate computation methods (such as UPS, USPS, FedEx, etc) are implemented as modules in VCF 2.0. We recommend you readВ [creating new module](docs/vc2devguide/working-with-platform-manager/extending-functionality/creating-new-module)В before you start implementing a new shipping method. The article will explain to you the required steps for creating a module and extending the system with a new shipping method.

## Define new shipping method

* Create new module ([creating new module](docs/vc2devguide/working-with-platform-manager/extending-functionality/creating-new-module)).
* Create a class which should be derived from ShippingMethod abstract class and override CalculateRate method.В When VCF 2.0 calculates shipping rates the CalculateRate methods of your class will be called.
* Register in system and define settings.
* Register your module class in the IoC container. This must be done in PostInitialize method in your module class. You can also define all needed settings which will be used in your method and can be changed in the management UI.В You may define shipping method settings as module settings in the module manifest file ([manage module setting](docs/vc2devguide/working-with-platform-manager/extending-functionality/managing-module-settings)) and you can pass the module settings to a shipping method constructor in registration.  
  
```
var settingManager = _container.Resolve<ISettingsManager>();
var shippingService = _container.Resolve<IShippingMethodsService>();
shippingService.RegisterShippingMethod(() => new FixedRateShippingMethod(settingManager.GetModuleSettings("MyShippingMethodModule"))
	{
		Description = "Fixed rate shipping method",
		LogoUrl = "http://somelogo.com/logo.png"
	});
```

All settings may have default values that can be used for default methods if not overridden by custom values later.

You may see examples on how to define and register new shipping methods in the current solution: (VirtoCommerce.CoreModule - FixedRateShippingMethod)

## Enable and configure shipping method for store

After your module is installed in your target system, all your shipping methods should appear and be available for configuration in every store in your system. Store->Shipping methods widget. You can configure shipping methods for each store individually:
* enable/disable method for current store
* change priority (to determine in what order they will be displayed in the checkout)
* cВЃhange description text or logo
* edit all settings and what you define for the shipping method
* use a custom UI for a more detailed shipping method configuration

After you complete the configuration, your shipping method will be appear in the FrontEnd checkout page and the customer may select it as a shipping method.

## Management UI customization

If a standard user interface is not enough, then consider implementing your own UI for managing shipping methods through the standard UI extension point (widget container with group вЂќshippingMethodDetailвЂќ).В You can read more about extending the existing UI with widgetsВ [here](docs/vc2devguide/working-with-platform-manager/basic-functions/widgets).

## Related articles

Related articles appear here based on the labels you select. Click to edit the macro and add or change labels.
