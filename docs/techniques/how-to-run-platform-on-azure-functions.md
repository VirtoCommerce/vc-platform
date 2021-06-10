Azure Functions lets you run your code in a serverless environment without having to create a virtual machine (VM) or publish a web application first. 
In this article you use command-line tools to deploy and run VC Platform on a serverless environment of Azure Functions.


# How it works
We use [Azure Functions custom handlers](https://docs.microsoft.com/en-us/azure/azure-functions/functions-custom-handlers) for running `vc platform` on a serverless environment of Azure Functions. Custom handlers are best suited for the situations where we want to host and run an existing ASP.NET Core application that is not intended to run on Azure functions. This way we receive a good option to run as a serverless application without any code changes.


# Prerequisites
Verify these prerequisites before you begin:
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) must be installed
- The [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local) version 3.x.
- Instance of on-premises running [VC Platform](https://github.com/VirtoCommerce/vc-platform/blob/master/docs/getting-started/deploy-from-precompiled-binaries-windows.md) with a set of modules installed
- [Create supporting Azure resources for your function](https://docs.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Cbrowser#create-supporting-azure-resources-for-your-function)

In this article you use `command-line` tools to deploy the platform app to the serverless environment of Azure Functions. There is also an article how to get started with Azure Functions [Getting started with Azure Functions using command line](https://docs.microsoft.com/en-us/azure/azure-functions/functions-get-started?pivots=programming-language-csharp), or alternative [Create a C# function in Azure using Visual Studio Code](https://docs.microsoft.com/en-us/azure/azure-functions/create-first-function-vs-code-csharp)

The platform folder already includes all the files that are required to run platform app as a custom handler. You don't need to do anything extra with this:

A `/host.json` file at the root of platform app. Informs the Functions host where to send requests by pointing to a web server, capable of processing HTTP events.

A `/local.settings.json` file at the root of platform app. Defines application settings used when running the function app locally.

A `/azure-func/function.json` function metadata in a folder `azure-func`. Will forward all request payloads to the platform app.


# Run the platform as function locally
Run platform function by starting the local Azure Functions runtime host from the platform's installed folder:

```console
cd C:\vc-platform-3\
func start
```
Toward the end of the output, the following lines should appear:

```console

Functions:

        azure-func: [GET,POST,PUT,DELETE] http://localhost:7071/{*paths}

For detailed output, run func with --verbose flag.
```

Then you can execute platform API locally by sending http requests to http://localhost:7071/{*paths}. 
```console
curl http://localhost:7071/api/currencies?api_key=<API KEY>
```

> Note: currently the platform that is hosted as function doesn't accept `Authorization: Bearer` header, and you must use simple key authorization in query string `?api_key=<api key>` instead (TBD: article how to use api_key authorization).

# Deploy the function project to Azure

Before you can deploy your function code to Azure, you need to create three resources:

- A resource group, which is a logical container for related resources. [Create resource groups](https://docs.microsoft.com/en-us/azure/azure-resource-manager/management/manage-resource-groups-portal#create-resource-groups)  
- A Storage account, which is used to maintain state and other information about your functions. [Create a storage account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create)
- A function app, which provides the environment for executing platform app as a function. 


Use the following commands to create these items.

1. If you haven't done so already, sign in to Azure:
```console
az login
```
2. Create a resource group named `vc-platform-serverless` in the `westeurope` region or in a region near you:
```console
az group create --name vc-platform-serverless --location westeurope
```
3. Create a general-purpose storage account in your resource group and region:
```console
az storage account create --name vcplatformserverlessstorage --location westeurope --resource-group vc-platform-serverless --sku Standard_LRS
```
4. Create the function app:
```console
az functionapp create --resource-group vc-platform-serverless --consumption-plan-location westeurope --runtime custom --functions-version 3 --name vc-platform-serverless-app --storage-account vcplatformserverlessstorage --os-type Windows --subscription <your azure subscription id>
```


Then deploy the platform application to Azure:
```console
cd C:\vc-platform-3\
func azure functionapp publish vc-platform-serverless-app --subscription <your azure subscription id>
```   

> Important note! 
> 
> To reduce the launch time of the platform app, the next step after a successful publication to Azure, please set the `RefreshProbingFolderOnStart` setting to `false` for the Azure Function App configuration in Azure portal.

![image](../media/how-to-run-platform-on-azure-functions-1.png) 


# Known limitations
- Platform runs as Azure Functions only on Windows OS type (Linux OS: WIP)
- Bearer token Authorization doesn't work. You must use simple key authorization (api_key) instead. (WIP)
