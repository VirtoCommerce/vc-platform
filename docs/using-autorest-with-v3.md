# How to generate AutoRest-generated clients with the Platform v.3

## Installing the Autorest

> NOTE: look at the documentation: [Installing AutoRest](https://github.com/Azure/autorest/blob/master/docs/installing-autorest.md)

   1. Install [Node.js](https://nodejs.org/en/) (10.15.x LTS preferred. May not function with Node < 10.x Be Wary of 11.x builds as they may introduce instability or breaking changes. )
   2. Install AutoRest using npm, at the moment using version 3.0.x, because the Platform generate the api as OpenApi 3.0.x
   ```cmd
    npm install -g @autorest/core 
   ```
   3. Generate autorest client on the Storefront
        1. Open Tools > NuGet Package Manager > Package Manager Console
        2. Run the following commands to generate API clients:
```cmd
$modules = @('Cache','Cart','Catalog','Content','Core','Customer','Inventory','Marketing','Orders','Platform','Pricing','Quote','Sitemaps','Store','Subscription')
$modules.ForEach( { autorest-core --debug --input-file=http://localhost:10645/docs/VirtoCommerce.$_/swagger.json --output-folder=VirtoCommerce.Storefront\AutoRestClients --output-file=$_`ModuleApi.cs --namespace=VirtoCommerce.Storefront.AutoRestClients.$_`ModuleApi --override-client-name=$_`ModuleApi --add-credentials --csharp })
```
   4. Add the api client to [the ServiceColection](https://github.com/VirtoCommerce/vc-storefront-core/blob/master/VirtoCommerce.Storefront/DependencyInjection/ServiceCollectionExtension.cs)

```cs
    services.AddAutoRestClient((credentials, httpClient, disposeHttpClient, baseUri) => new TaxModuleApi(credentials, httpClient, disposeHttpClient) { BaseUri = baseUri });
    services.AddSingleton<ITaxModule>(sp => new TaxModule(sp.GetRequiredService<TaxModuleApi>()));
```





    
