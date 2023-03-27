# AutoRest clients

The AutoRest tool generates client libraries for accessing Virto Commerce API.

## Installing the Autorest

!!! note
    Read the official AutoRest documentation: [Installing AutoRest](https://github.com/Azure/autorest/blob/main/docs/readme.md)

* Install [Node.js](https://nodejs.org/en/)
* Install AutoRest using npm, at the moment using version 3.0.x, because the Platform generate the api as OpenApi 3.0.x. 
    ```console
    npm install -g autorest
    ```
* Then reset autorest cache:
    ```console
    autorest --reset
    ```

## Generating C# Clients with AutoRest
* Have the platform run locally (10645 port by default)
* Generate autorest client on the Storefront
* Open Tools > NuGet Package Manager > Package Manager Console
* Run the following commands to generate API clients:
    ```console
    $modules = @('Platform', 'Cart', 'Catalog', 'Content', 'Core', 'Customer', 'Inventory', 'Marketing', 'Notifications', 'Orders', 'Payment', 'Pricing', 'Shipping', 'Sitemaps', 'Store', 'Subscription', 'Tax')
    $modules.ForEach( { autorest VirtoCommerce.Storefront\AutoRestClients\array-in-query-fix.yml --version=3.0.6274 --v3 --debug --input-file=http://localhost:10645/docs/VirtoCommerce.$_/swagger.json --output-folder=.\VirtoCommerce.Storefront\AutoRestClients --output-file=$_`ModuleApi.cs --namespace=VirtoCommerce.Storefront.AutoRestClients.$_`ModuleApi --override-client-name=$_`ModuleClient --add-credentials --csharp })
    ```
* Add the api client to [the ServiceColection](https://github.com/VirtoCommerce/vc-storefront-core/blob/master/VirtoCommerce.Storefront/DependencyInjection/ServiceCollectionExtension.cs):
    ```cs
    services.AddAutoRestClient((credentials, httpClient, disposeHttpClient, baseUri) => new TaxModuleApi(credentials, httpClient, disposeHttpClient) { BaseUri = baseUri });
    services.AddSingleton<ITaxModule>(sp => new TaxModule(sp.GetRequiredService<TaxModuleApi>()));
    ```





    
