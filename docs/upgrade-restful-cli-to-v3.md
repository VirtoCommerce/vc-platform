# How to upgrade RESTful clients to VirtoCommerce v3 in the Storefront 

## Summary
Currently, storefront is compatible to both major versions of the Platform (v2, v3). This document describes how to upgrade Storefront RESTful API clients (for platform and modules) to the latest v3 interfaces. This action allow you to modernize storefront but break compatibility with v2.
## Contents

## Preconditions
* Open solution with storefront source code;
* Please check Autorest version by following:
    - Open Package Manager Console in Visual Studio;
    - Run *autorest --info*, check output:
    
            PM> autorest --info
            AutoRest code generation utility [version: 2.0.4407; node: v10.16.0]
            (C) 2018 Microsoft Corporation.
            https://aka.ms/autorest

        It must be 2.0.4 or above, install/upgrade Autorest if needed (https://github.com/Azure/AutoRest).
* Remember the Autorest command to re-generate Restful interface client code:

        $modules = @('Platform')
        $modules.ForEach( { autoRest --debug --input-file=http://localhost:10645/docs/VirtoCommerce.$_/swagger.json --output-folder=VirtoCommerce.Storefront\AutoRestClients --output-file=$_`ModuleApi.cs --namespace=VirtoCommerce.Storefront.AutoRestClients.$_`ModuleApi --override-client-name=$_`ModuleClient --add-credentials --csharp })
    The *$modules* is a list of modules API clients that will be regenerated. You can put there more than one, but it be harder to understand the changes. It is recommended to upgrade module by module.

    Don't forget to modify endpoint in *--input-file* parameter.

    It's useful to run Autorest from Package Manager Console with selected Virtocommerce.Storefront project.
___
## Upgrading RESTful clients
___
### **Platform**
Run Autorest for *$modules = @('Platform','Notifications')*. Wait while Autorest finishes. *'Notifications'* passed because in v3 notifications module was extracted from platform to separate module.

Try to compile solution. Look at the errors. Resolve errors and update code with recommendations below.

>file: \Domain\Common\NotificationConverter.cs

|Error|Recommendation|
|-|-|
|'TestNotificationRequest' could not be found|Notifications were extracted to a separate module, therefore:<ul><li>add<br>*using VirtoCommerce.Storefront.AutoRestClients.NotificationsModuleApi.Models;*</li><li>remove<br>*using VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models;*</li><li>rewrite *TestNotificationRequest* to *NotificationRequest* (typename was changed).</li></ul>|

>file: \Controllers\AccountController.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'INotifications' could not be found|<ul><li>add<br>*using VirtoCommerce.Storefront.AutoRestClients.NotificationsModuleApi;*</li><li>remove<br>*using VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi;*</li></ul>|
|The type or namespace name 'SendNotificationResult' could not be found|*SendNotificationResult* was renamed to *NotificationSendResult*:<ul><li>add<br>*using VirtoCommerce.Storefront.AutoRestClients.NotificationsModuleApi.Models;*</li><li>change names *SendNotificationResult* to *NotificationSendResult*</li></ul>|
|Errors with parameter type in call *_platformNotificationApi.SendNotificationAsync*|Change this call to a backward compatible method *_platformNotificationApi.SendNotificationByRequestAsync*.|
|'IdentityResult' is an ambiguous reference between 'VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models.IdentityResult' and 'Microsoft.AspNetCore.Identity.IdentityResult'|Remove: *using VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models;*|

>file: \Controllers\Api\ApiAccountController.cs

|Error|Recommendation|
|-|-|
|A lot of errors like:<br>'IdentityResult' is an ambiguous reference between 'VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models.IdentityResult' and 'Microsoft.AspNetCore.Identity.IdentityResult'|Remove:<ul><li>*using VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi;*</li><li>*using VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models;*</li></ul>|
|The type or namespace name 'INotifications' could not be found|Add *using VirtoCommerce.Storefront.AutoRestClients.NotificationsModuleApi;*|
|Errors with parameter type in call *_platformNotificationApi.SendNotificationAsync*|Change this call to a backward compatible method *_platformNotificationApi.SendNotificationByRequestAsync*.|
|The type or namespace name 'ChangePasswordInfo' could not be found|It was renamed to *ChangePasswordRequest*, replace *ChangePasswordInfo* with *AutoRestClients.PlatformModuleApi.Models.ChangePasswordRequest* |

>file: \Domain\Common\SettingConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'Setting' does not exist...|It was renamed to *ObjectSettingEntry*: <ul><li>remove<br>*using platformDto = VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models;*</li><li>add<br>*using platformDto = VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models;*</li><li>change type name *Setting* to *ObjectSettingEntry*.</li><li>Check *ObjectSettingEntry*'s members for nulls before converting to string.</li></ul>|
|Issues inside *ToSettingEntry*|<ul><li>There is no *Description* and *Title* anymore. Not used, just remove assignments;</li><li>Convert *DefaultValue* and *Value* to string;</li><li>Set *IsArray* to **false**. It's not used;</li><li>Remove *ArrayValues* assignment. It's not used;</li><li>Convert each item of *AllowedValues* to string.</li></ul><br>As a result the method should look like:<br>|
```Csharp
    public static SettingEntry ToSettingEntry(this platformDto.ObjectSettingEntry settingDto)
    {
        var retVal = new SettingEntry();
        retVal.DefaultValue = settingDto.DefaultValue.ToString();
        retVal.IsArray = false;
        retVal.Name = settingDto.Name;
        retVal.Value = settingDto.Value.ToString();
        retVal.ValueType = settingDto.ValueType;
        if (settingDto.AllowedValues != null)
        {
            retVal.AllowedValues = settingDto.AllowedValues.Cast<string>().ToList();
        }
        return retVal;
    }
```

>file: \Domain\Security\SecurityConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'SecurityResult' does not exist...|Rewrite *dto.SecurityResult* as *dto.IdentityResult*, it was renamed.|
|The type or namespace name 'ApplicationUserExtended' does not exist|Rewrite *dto.ApplicationUserExtended* as *dto.ApplicationUser*, it was renamed.|
|Cannot implicitly convert type 'VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models.IdentityError' to 'string'|*IdentityResult* now contains errors as *IdentityError*. Use member *Description* (Rewrite *Description = x* as *Description = x.Description*).|

>file: \Domain\Security\UserStoreStub.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'ApplicationUserExtended' does not exist|Rewrite *ApplicationUserExtended* as *ApplicationUser*, it was renamed.|
|'ISecurity' does not contain a definition for 'CreateAsyncAsync'...|*CreateAsyncAsync* was renamed to *CreateAsync*|
|'ISecurity' does not contain a definition for 'DeleteAsyncAsync'...|*DeleteAsyncAsync* was renamed to *DeleteAsync*|
|'ISecurity' does not contain a definition for 'UpdateAsyncAsync'...|*UpdateAsyncAsync* was renamed to *UpdateAsync*|

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommercePlatformRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommercePlatformRESTAPIdocumentation* as *PlatformModuleClient*|
|The type or namespace name 'INotifications' could not be found...|add *using VirtoCommerce.Storefront.AutoRestClients.NotificationsModuleApi;*|

Warning! Don't forget to register in DI AutoRestClient for newcoming Notifications module! Be sure to have in *AddPlatformEndpoint* following lines:
```CSharp
    services.AddAutoRestClient((credentials, httpClient, disposeHttpClient, baseUri) => new NotificationsModuleClient(credentials, httpClient, disposeHttpClient) { BaseUri = baseUri });
    services.AddSingleton<INotifications>(sp => new Notifications(sp.GetRequiredService<NotificationsModuleClient>()));
```

>file: \Domain\Security\PermissionAuthorizationPolicyProvider.cs

|Error|Recommendation|
|-|-|
|'ISecurity' does not contain a definition for 'GetPermissionsAsync'...|Method was renamed to *GetAllRegisteredPermissionsAsync*|

>file: \Domain\Security\PollingApiUserChangeToken.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'UserSearchRequest' could not be found...|*UserSearchRequest* was renamed to *UserSearchCriteria*. Also its fields *SkipCount*->*Skip*, *TakeCount*->*Take*.|
|'UserSearchResult' does not contain a definition for 'TotalCount'|Search result was incapsulated into a *Result* property of *UserSearchResult*. Therefore replace *result.TotalCount* with *result.Result.TotalCount*.|
|'UserSearchResult' does not contain a definition for 'Users'|Search result was incapsulated into a *Result* property of *UserSearchResult*. Therefore replace *result.Users* with *result.Result.Users*.|

>file: \Domain\Cart\CartConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'Setting' does not exist...|*Setting* was renamed to *ObjectSettingEntry*|

>file: \Domain\Stores\StoreConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'Setting' does not exist...|*Setting* was renamed to *ObjectSettingEntry*|

>file: \Domain\Stores\StoreService.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'Setting' does not exist...|*Setting* was renamed to *ObjectSettingEntry*, rewrite as *AutoRestClients.PlatformModuleApi.Models.ObjectSettingEntry*|

>file: \Middleware\CreateStorefrontRolesMiddleware.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'RoleSearchRequest' could not be found...|*RoleSearchRequest* was renamed to *RoleSearchCriteria*. Also field *TakeCount*->*Take*.|
___
### **Catalog module**
Run Autorest for *$modules = @('Catalog')*. Wait while Autorest finishes.

>file: \Domain\Catalog\CatalogConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'ProductSearchCriteria' does not exist|*ProductSearchCriteria* was renamed to *ProductIndexedSearchCriteria*|
|The type or namespace name 'CategorySearchCriteria' does not exist|*CategorySearchCriteria* was renamed to *CategoryIndexedSearchCriteria*|

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceCatalogRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceCatalogRESTAPIdocumentation* as *PlatformModuleClient*|
|The type or namespace name 'ICatalogModuleSearch' could not be found...|*ICatalogModuleSearch* was renamed to *ICatalogModuleIndexedSearch*|
|The type or namespace name 'CatalogModuleSearch' could not be found...|*CatalogModuleSearch* was renamed to *CatalogModuleIndexedSearch*|

>file: \Domain\Catalog\CatalogConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'Product' does not exist|*Product* was renamed to *CatalogProduct*|
Warning! Variations in platform v3 have new type *Variation* itstead of storefront code where variations is a products. Therefore we need to add in this file additional conversion method to convert RESTful *Variation* to the *Product* in storefront. Let's do it:

```Csharp
        public static Product ToProduct(this catalogDto.Variation variationDto, Language currentLanguage, Currency currentCurrency, Store store)
        {
            return variationDto.JsonConvert<catalogDto.CatalogProduct>().ToProduct(currentLanguage, currentCurrency, store);
        }
```

>file: \Domain\Catalog\CatalogService.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'ICatalogModuleSearch' could not be found...|*ICatalogModuleSearch* was renamed to *ICatalogModuleIndexedSearch*|
|The type or namespace name 'ProductSearchResult' could not be found...|*ProductSearchResult* was renamed to *ProductIndexedSearchResult*|
___
### **Cart module**
Run Autorest for *$modules = @('Cart')*. Wait while Autorest finishes.

>file: \Domain\Cart\CartConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'ShipmentItem' could not be found...|*ShipmentItem* was renamed to *CartShipmentItem*|
|The type or namespace name 'Shipment' could not be found...|*Shipment* was renamed to *CartShipment*|
|The type or namespace name 'Address' could not be found...|*Address* was renamed to *CartAddress*|
|The type or namespace name 'LineItem' could not be found...|*LineItem* was renamed to *CartLineItem*|
|'ShippingRate' does not contain a definition for 'OptionDescription'|*OptionDescription* obsolete. Just remove this assignment.|
|'PaymentMethod' does not contain a definition for 'Description'|*Description* obsolete. Just remove this assignment.|
|The property or indexer 'ShoppingCart.ObjectType' cannot be used in this context because the set accessor is inaccessible|Just remove this assignment. It does not matter because it's equal to .NET type name on the server.|
|The property or indexer 'CartLineItem.ObjectType' cannot be used in this context because the set accessor is inaccessible|Just remove this assignment. It does not matter because it's equal to .NET type name on the server.|

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceCartRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceCartRESTAPIdocumentation* as *CartModuleClient*|

>file: \Domain\Cart\CartService.cs

|Error|Recommendation|
|-|-|
|'ICartModule' does not contain a definition for 'UpdateAsync'...|*UpdateAsync* was renamed to *UpdateShoppingCartAsync*|
|'ICartModule' does not contain a definition for 'SearchAsync'...|*SearchAsync* was renamed to *SearchShoppingCartAsync*|
___
### **Content module**
Run Autorest for *$modules = @('Content')*. Wait while Autorest finishes.

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceContentRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceContentRESTAPIdocumentation* as *ContentModuleClient*|

___
### **Core and Tax modules**
Run Autorest for *$modules = @('Core','Tax')*. Wait while Autorest finishes.
We are running for both modules because a tax-linked part of Core module was moved to Tax module.

>file: \Domain\Common\DynamicPropertyConverter.cs

|Error|Recommendation|
|-|-|
|A lot of errors like: The type or namespace name 'DynamicObjectProperty' could not be found...|*DynamicObjectProperty* was moved to platform API. Replace *coreDto.DynamicObjectProperty* with *platformDto.DynamicObjectProperty*|

>file: \Domain\Tax\TaxConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'TaxRate' could not be found...|*TaxRate* was moved to Tax module: <ul><li>Add *using taxDto = VirtoCommerce.Storefront.AutoRestClients.TaxModuleApi.Models;*</li><li>replace *coreDto.TaxRate* with *taxDto.TaxRate*;</li><li>Remove *using coreDto = VirtoCommerce.Storefront.AutoRestClients.CoreModuleApi.Models.*</li></ul>|
|The type or namespace name 'TaxEvaluationContext' could not be found...|*TaxEvaluationContext* was moved to Tax module: replace *coreDto.TaxEvaluationContext* with *taxDto.TaxEvaluationContext*|
|The type or namespace name 'TaxLine' could not be found...|*TaxLine* was moved to Tax module: replace *coreDto.TaxLine* with *taxDto.TaxLine*</li></ul>|
|The type or namespace name 'TaxDetail' could not be found...|*TaxDetail* was moved to Tax module: replace *coreDto.TaxDetail* with *taxDto.TaxDetail*</li></ul>|
|'TaxCustomer' does not contain a definition for 'MemberType'|*MemberType* was removed. Remove assignment.|

>file: \Domain\Tax\TaxEvaluator.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'TaxRate' does not exist...|*TaxRate* was moved to Tax module: <ul><li>add *using taxDto = VirtoCommerce.Storefront.AutoRestClients.TaxModuleApi.Models;*</li><li>replace *coreService.TaxRate* with *taxDto.TaxRate*;</li><li>Remove *using coreService = VirtoCommerce.Storefront.AutoRestClients.CoreModuleApi.Models.*</li></ul>|
|'ICommerce' does not contain a definition for 'EvaluateTaxesAsync'...|*EvaluateTaxesAsync* was moved to Tax module:<ul><li>add *using VirtoCommerce.Storefront.AutoRestClients.TaxModuleApi;*</li><li>replace *ICommerce* with *ITaxModule* to make tax module methods accessible;</li><li>Rename member *_commerceApi* to *_taxApi*;</li><li>remove *using VirtoCommerce.Storefront.AutoRestClients.CoreModuleApi;*</li></ul>|

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceCoreRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceCoreRESTAPIdocumentation* as *CoreModuleClient*|
|'TaxModule' does not contain a constructor that takes 3 arguments|Rewrite *TaxModule* as *CoreModuleClient*.|
|The type or namespace name 'IStorefrontSecurity' could not be found...<br>The type or namespace name 'StorefrontSecurity' could not be found...|It's obsolete. Just remove this registration in DI.|

>file: \Domain\Stores\StoreConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'DynamicObjectProperty' could not be found...|*DynamicObjectProperty* was moved to platform API. Replace *coreDto.DynamicObjectProperty* with *platformDto.DynamicObjectProperty*|

>file: \Domain\Marketing\MarketingConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'DynamicObjectProperty' could not be found...|*DynamicObjectProperty* was moved to platform API. Replace *coreDto.DynamicObjectProperty* with *platformDto.DynamicObjectProperty*. Add appropriate using.|

>file: \Domain\Order\OrderConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'DynamicObjectProperty' could not be found...|*DynamicObjectProperty* was moved to platform API. Replace *coreDto.DynamicObjectProperty* with *platformDto.DynamicObjectProperty*. Add appropriate using.|

>file: \Domain\Cart\CartConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'DynamicObjectProperty' could not be found...|*DynamicObjectProperty* was moved to platform API. Replace *coreDto.DynamicObjectProperty* with *platformDto.DynamicObjectProperty*|
|Cannot implicitly convert type 'VirtoCommerce.Storefront.AutoRestClients.CoreModuleApi.Models.Address' to 'VirtoCommerce.Storefront.AutoRestClients.TaxModuleApi.Models.TaxAddress'|Add conversion to *TaxAddress* thru *JsonConvert*: <br>*``` retVal.Address = taxContext.Address.ToCoreAddressDto().JsonConvert<taxDto.TaxAddress>();```*|

>file: \Domain\Customer\MemberConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'DynamicObjectProperty' could not be found...|*DynamicObjectProperty* was moved to platform API. Replace *coreDto.DynamicObjectProperty* with *platformDto.DynamicObjectProperty*. Add appropriate using.|
|The type or namespace name 'Contact' does not exist in the namespace|*Contact* was moved to Customer module. Start with next topic: convert Customer module.|
___
### **Customer module**
Run Autorest for *$modules = @('Customer')*. Wait while Autorest finishes.

>file: \Domain\Customer\MemberConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'Contact' does not exist in the namespace|*Contact* was moved to Customer module. Replace *coreDto.Contact* with *customerDto.Contact*.|
|The type or namespace name 'Address' could not be found...|*Address* was renamed to *CustomerAddress*.|

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceCustomerRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceCustomerRESTAPIdocumentation* as *CoreModuleClient*|


>file: \Domain\Customer\MemberService.cs

|Error|Recommendation|
|-|-|
|'ICustomerModule' does not contain a definition for 'SearchAsync' and no accessible extension method 'SearchAsync' ...|*SearchAsync* was renamed to *SearchMemberAsync*.|

>file: \Domain\Tax\TaxConverter.cs

|Error|Recommendation|
|-|-|
|Cannot implicitly convert type 'VirtoCommerce.Storefront.AutoRestClients.CustomerModuleApi.Models.Contact' to 'VirtoCommerce.Storefront.AutoRestClients.TaxModuleApi.Models.TaxCustomer'|Add conversion to *TaxCustomer* thru *JsonConvert*: <br>*``` retVal.Customer = taxContext?.Customer?.Contact?.ToCoreContactDto().JsonConvert<taxDto.TaxCustomer>();```*|

>file: \Domain\Quote\QuoteConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'DynamicObjectProperty' does not exist...|*DynamicObjectProperty* was moved to platform API. Replace *coreDto.DynamicObjectProperty* with *platformDto.DynamicObjectProperty*. Add appropriate using of platform models:<br>*using platformDto = VirtoCommerce.Storefront.AutoRestClients.PlatformModuleApi.Models;*|
___
### **Inventory module**
Run Autorest for *$modules = @('Inventory')*. Wait while Autorest finishes.

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceInventoryRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceInventoryRESTAPIdocumentation* as *InventoryModuleClient*|
___
### **Marketing module**
Run Autorest for *$modules = @('Marketing')*. Wait while Autorest finishes.

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceMarketingRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceMarketingRESTAPIdocumentation* as *InventoryModuleClient*|

___
### **Orders module**
Run Autorest for *$modules = @('Orders')*. Wait while Autorest finishes.

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceOrdersRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceOrdersRESTAPIdocumentation* as *OrdersModuleClient*|

>file: \Controllers\Api\ApiCartController.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'ProcessPaymentResult' does not exist|*ProcessPaymentResult* was renamed to *ProcessPaymentRequestResult*|

>file: \Domain\Order\OrderConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'ProcessPaymentResult' does not exist|*ProcessPaymentResult* was renamed to *ProcessPaymentRequestResult*|
|The type or namespace name 'Address' does not exist|*Address* was renamed to *OrderAddress*.|
|The type or namespace name 'ShipmentItem' does not exist|*ShipmentItem* was renamed to *OrderShipmentItem*.|
|The type or namespace name 'Shipment' does not exist|*Shipment* was renamed to *OrderShipmentItem*.|
|The type or namespace name 'LineItem' does not exist|*LineItem* was renamed to *OrderLineItem*.|
|'ProcessPaymentRequestResult' does not contain a definition for 'Error'|*Error* was renamed to *ErrorMessage*|

>file: \Controllers\Api\ApiOrderController.cs

|Error|Recommendation|
|-|-|
|IOrderModule' does not contain a definition for 'SearchAsync' and no accessible extension method 'SearchAsync' |*SearchAsync* was renamed to *SearchCustomerOrderAsync*|
|IOrderModule' does not contain a definition for 'UpdateAsync' and no accessible extension method 'UpdateAsync' |*UpdateAsync* was renamed to *UpdateOrderAsync*|
|A lot of errors: There is no argument given that corresponds to the required formal parameter 'respGroup' of 'OrderModuleExtensions.GetByNumberAsync|Write *string.Empty* to *respGroup* parameter.|
|There is no argument given that corresponds to the required formal parameter 'respGroup' of 'OrderModuleExtensions.GetByIdAsync|Write *string.Empty* to *respGroup* parameter.|

>file: \Domain\Order\CustomerOrderService.cs

|Error|Recommendation|
|-|-|
|There is no argument given that corresponds to the required formal parameter 'respGroup' of 'OrderModuleExtensions.GetByNumberAsync|Write *string.Empty* to *respGroup* parameter.|
|There is no argument given that corresponds to the required formal parameter 'respGroup' of 'OrderModuleExtensions.GetByIdAsync|Write *string.Empty* to *respGroup* parameter.|
|IOrderModule' does not contain a definition for 'SearchAsync' and no accessible extension method 'SearchAsync' |*SearchAsync* was renamed to *SearchCustomerOrderAsync*|
___
### **Pricing module**
Run Autorest for *$modules = @('Pricing')*. Wait while Autorest finishes.

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommercePricingRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommercePricingRESTAPIdocumentation* as *PricingModuleClient*|
___
### **Shipping module**
Run Autorest for *$modules = @('Shipping')*. Wait while Autorest finishes.

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|'ShippingModule' does not contain a constructor that takes 3 arguments<br>|Rewrite *ShippingModule* as *ShippingModuleClient*.|
___
### **Payment module**
Run Autorest for *$modules = @('Payment')*. Wait while Autorest finishes.

|Error|Recommendation|
|-|-|
|'PaymentModule' does not contain a constructor that takes 3 arguments<br>|Rewrite *PaymentModule* as *PaymentModuleClient*.|
___
### **Sitemaps module**
Run Autorest for *$modules = @('Sitemaps')*. Wait while Autorest finishes.

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceSitemapsRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceSitemapsRESTAPIdocumentation* as *SitemapsModuleClient*|
___
### **Store module**
Run Autorest for *$modules = @('Store')*. Wait while Autorest finishes.

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceStoreRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceStoreRESTAPIdocumentation* as *SitemapsModuleClient*|

>file: \Domain\Stores\StoreService.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'PaymentMethod' does not exist<br> The type or namespace name 'TaxProvider' does not exist|Just remove these usings|
|'Store' does not contain a definition for 'PaymentMethods'...<br>'Store' does not contain a definition for 'TaxProviders'...|*storeDto* does not have such properties. We should always ask *_paymentModule* service for valid store payment methods and *_taxModule* for tax providers. Rewrite *ConvertStoreAndLoadDependeciesAsync* with this mean (look at the code after table).|
|The type or namespace name 'StorePaymentMethod' could not be found|Just remove *JsonConvert\<StorePaymentMethod>()* it doesn't need anymore.|

```CSharp 
        protected virtual async Task<Store> ConvertStoreAndLoadDependeciesAsync(StoreApi.Store storeDto, Currency currency = null)
        {
            var result = storeDto.ToStore();

            if (currency != null)
            {
                var paymentSearchCriteria = new PaymentMethodsSearchCriteria { StoreId = result.Id };
                var paymentsSearchResult = await _paymentModule.SearchPaymentMethodsAsync(paymentSearchCriteria);

                result.PaymentMethods = paymentsSearchResult.Results
                    .Where(pm => pm.IsActive != null && pm.IsActive.Value)
                    .Select(pm =>
                    {
                        var paymentMethod = pm.ToStorePaymentMethod(currency);
                        paymentMethod.Name = pm.TypeName;
                        return paymentMethod;
                    }).ToArray();
            }
            var taxSearchCriteria = new TaxProviderSearchCriteria { StoreId = result.Id };
            var taxProviderSearchResult = await _taxModule.SearchTaxProvidersAsync(taxSearchCriteria);
            result.FixedTaxRate = GetFixedTaxRate(taxProviderSearchResult.Results.Select(xp => xp.JsonConvert<TaxProvider>()).ToArray());

            //use url for stores from configuration file with hight priority than store url defined in manager
            result.Url = _storefrontOptions.StoreUrls[result.Id] ?? result.Url;

            return result;
        }
```

>file: \Domain\Stores\StoreConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'PaymentMethod' does not exist...<br>The type or namespace name 'TaxDetail' does not exist...|They both were moved to payment module. Therefore add appropriate using *using paymentDto = VirtoCommerce.Storefront.AutoRestClients.PaymentModuleApi.Models;* then rewrite *storeDto.PaymentMethod* as *paymentDto.PaymentMethod*, *storeDto.TaxDetail*, as *paymentDto.TaxDetail*|

>file: \Domain\Order\OrderConverter.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'PaymentMethod' does not exist...|There meaning PaymentMethod from store module, that was moved in v3 to payment module. Therefore it is need to add appropriate using *using paymentDto = VirtoCommerce.Storefront.AutoRestClients.PaymentModuleApi.Models;* then rewrite *storeDto.PaymentMethod* as *paymentDto.PaymentMethod*|

___
### **Subscription module**
Run Autorest for *$modules = @('Subscription')*. Wait while Autorest finishes.

>file: \DependencyInjection\ServiceCollectionExtension.cs

|Error|Recommendation|
|-|-|
|The type or namespace name 'VirtoCommerceSubscriptionRESTAPIdocumentation' could not be found...|Rewrite *VirtoCommerceSubscriptionRESTAPIdocumentation* as *SitemapsModuleClient*|
