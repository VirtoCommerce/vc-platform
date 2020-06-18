---
title: Extending domain models
description: The developer guide for extending domain models
layout: docs
date: 2017-03-16T15:25:00Z
priority: 5
---
## Introduction

This article describes how to add new fields to VC domain model without changing the platform code.

The VC platform and VC modules expose many models which you can use as is, but most likely you will want to change them.

Since VC is an open source platform you could make changes directly to the platform or modules, but this could cause serious problems in the future when upgrading the platform or modules.

The more reliable way to customize the VC model is to inherit your custom model from the VC model and tell the platform to use the new model instead of the standard one.

> We strongly recommend to avoid changing the VC platform and VC modules and make all changes in your own modules.

## Creating a model extension module

In this sample we will add a new field CartType to the shopping cart.

> A sample module source code can be found here: [https://github.com/VirtoCommerce/vc-samples/tree/master/CartModule2](https://github.com/VirtoCommerce/vc-samples/tree/master/CartModule2)

### Create new module

Create new **managed** module first: [creating new module](docs/vc2devguide/working-with-platform-manager/extending-functionality/creating-new-module).

### Add dependencies

The shopping cart model is defined in [VirtoCommerce.Domain](https://github.com/VirtoCommerce/vc-module-core/tree/master/VirtoCommerce.Domain) project which is a part of the **VirtoCommerce.Core** module. So we should add the [VirtoCommerce.Domain](https://www.nuget.org/packages/VirtoCommerce.Domain) NuGet package to the project and add a dependency on **VirtoCommerce.Core** module to our [module.manifest](https://github.com/VirtoCommerce/vc-samples/blob/master/CartModule2/module.manifest).

### Define custom model

We inherit a new class from ShoppingCart and add a new property CartType which can be either Regular or Wishlist.

```
public enum Cart2Type
{
    Regular,
    Whishlist
}

public class Cart2 : ShoppingCart
{
    public Cart2Type CartType { get; set; }
}
```

### Register custom model

In the [Module.cs](https://github.com/VirtoCommerce/vc-samples/blob/master/CartModule2/Module.cs) file we override the **PostInitialize** method and tell the platform to use our Cart2 model instead of the original ShoppingCart.

```
public class Module : ModuleBase
{
    public override void PostInitialize()
    {
        AbstractTypeFactory<ShoppingCart>.OverrideType<ShoppingCart, Cart2>();
    }
}
```

### Check REST API

After installing our extension module to the platform all existing APIs will use the new shopping cart model automatically.

Open the Swagger JSON for the **VirtoCommerce.Cart** module by navigating to [http://localhost/admin/docs/VirtoCommerce.Cart/v1](http://localhost/admin/docs/VirtoCommerce.Cart/v1)

Here is a small fragment of this document where you can see that the standard **ShoppingCart** has a new enum property **cartType** with two values: **Regular** and **Whishlist**.

```
{
    "definitions": {
        "ShoppingCart": {
            "type": "object",
            "properties": {
                "cartType": {
                    "enum": [
                        "Regular",
                        "Whishlist"
                    ],
                    "type": "string"
                }
            }
        }
    }
}
```

### Generate new API client

In order to use our new shopping cart on the storefront we have to generate a new API client.

Open the storefront solution and run the following command in the Package Manager Console:

```
AutoRest.exe -Input http://localhost/admin/docs/VirtoCommerce.Cart/v1 -OutputFileName CartModuleApi.cs -Namespace VirtoCommerce.Storefront.AutoRestClients.CartModuleApi -ClientName CartModuleApiClient -OutputDirectory VirtoCommerce.Storefront\AutoRestClients -AddCredentials true -UseDateTimeOffset false
```

After that the generated ShoppingCart class will have a new property:

```
public partial class ShoppingCart
{
    ...
    /// <summary>
    /// Gets or sets possible values include: 'Regular', 'Whishlist'
    /// </summary>
    [Newtonsoft.Json.JsonProperty(PropertyName = "cartType")]
    public string CartType { get; set; }
    ...
}
```
