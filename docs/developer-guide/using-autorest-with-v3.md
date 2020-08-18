# AutoRest clients

## Installing the Autorest

!!! note
    Read the official AutoRest documentation: [Installing AutoRest](https://github.com/Azure/autorest/blob/master/docs/installing-autorest.md)

   - Install [Node.js](https://nodejs.org/en/) (10.15.x LTS preferred. May not function with Node < 10.x Be Wary of 11.x builds as they may introduce instability or breaking changes. )
   - Reinstall or install AutoRest using npm, at the moment using version 3.0.x, because the Platform generate the api as OpenApi 3.0.x. Then reset autorest cache:
```console
npm uninstall -g autorest
npm i -g autorest
autorest --reset
```
   - Have the platform run locally (10645 port by default)
   - Generate autorest client on the Storefront
   - Open Tools > NuGet Package Manager > Package Manager Console
   -   Run the following commands to generate API clients:
```console
$modules = @('Platform', 'Cart', 'Catalog', 'Content', 'Core', 'Customer', 'Inventory', 'Marketing', 'Notifications', 'Orders', 'Payment', 'Pricing', 'Shipping', 'Sitemaps', 'Store', 'Subscription', 'Tax')
$modules.ForEach( { autoRest VirtoCommerce.Storefront\AutoRestClients\array-in-query-fix.yml --version=3.0.6274 --v3 --debug --input-file=http://localhost:10645/docs/VirtoCommerce.$_/swagger.json --output-folder=.\VirtoCommerce.Storefront\AutoRestClients --output-file=$_`ModuleApi.cs --namespace=VirtoCommerce.Storefront.AutoRestClients.$_`ModuleApi --override-client-name=$_`ModuleClient --add-credentials --csharp })
```
   -   Add the api client to [the ServiceColection](https://github.com/VirtoCommerce/vc-storefront-core/blob/master/VirtoCommerce.Storefront/DependencyInjection/ServiceCollectionExtension.cs)
```csharp
    services.AddAutoRestClient((credentials, httpClient, disposeHttpClient, baseUri) => new TaxModuleApi(credentials, httpClient, disposeHttpClient) { BaseUri = baseUri });
    services.AddSingleton<ITaxModule>(sp => new TaxModule(sp.GetRequiredService<TaxModuleApi>()));
```





    
