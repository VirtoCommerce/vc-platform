---
date: '2018-08-30'
title: 'Deploy Storefront from precompiled binaries'
layout: docs
---
## Summary

Use this guide to <a class="crosslink" href="https://virtocommerce.com/ecommerce-hosting" target="_blank">deploy</a> and configure precompiled Virto Commerce Storefront.


### Prerequisites
[Prerequisites for .NET Core on Windows](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites)

[Microsoft Visual C++ 2015 Redistributable](https://www.microsoft.com/en-us/download/details.aspx?id=53840) (required for SCSS engine)

## Downloading the precomplied binaries:

Navigate to the <a href="https://github.com/VirtoCommerce/vc-storefront-core/releases">Releases section of Virto Commerce Storefront in GitHub.</a>

You will find **VirtoCommerce.Storefront.3.x.x.zip** file. In this file the site has already been built and can be run without additional compilation. It does not includes all the source code.

Unpack this zip to a local directory **C:\vc-storefront-core**. After that you will have the directory with storefront precompiled files.


## Setup

### Configuring VirtoCommerce Platform Endpoint
Set actual platform endpoint values in the C:\vc-storefront-core\appsettings.json.
Read more about how to generate API keys [here](https://virtocommerce.com/docs/vc2devguide/development-scenarios/working-with-platform-api)

```
 ...
  "VirtoCommerce": {
    "Endpoint": {
	   //Virto Commerce platform manager url
      "Url": "http://localhost/admin",
	   //HMAC authentification user credentials on whose behalf the API calls will be made.
      "AppId": "Enter your AppId here"
      "SecretKey": "Enter your SecretKey here",
    }
	...
```
### Configure themes
Storefront  **appsettings.json** file contains **ContentConnectionString** setting with pointed to the folder with actual themes and pages content
```
...
"ConnectionStrings": {
    //For themes stored in local file system
    "ContentConnectionString": "provider=LocalStorage;rootPath=~/cms-content"
	//For themes stored in azure blob storage
    //"ContentConnectionString" connectionString="provider=AzureBlobStorage;rootPath=cms-content;DefaultEndpointsProtocol=https;AccountName=yourAccountName;AccountKey=yourAccountKey"
  },
...
```
You can set this connection string in one of the following ways:
1. If you have already have installed  platform with sample data, your platform already contains `~/App_Data/cms-content` folder with themes for sample stores and you need only to make symbolic link to this folder by this command:
```
mklink /d C:\vc-storefront-core\wwwroot\cms-content C:\vc-platform\VirtoCommerce.Platform.Web\App_Data\cms-content
```
2. If you did not install sample data with your platform, you need to create new store in platform manager and download themes as it described in this article
[Theme development](https://virtocommerce.com/docs/vc2devguide/working-with-storefront/theme-development)

### Running the storefont by CLI "dotnet run"
Run the storefront by  follow command
``` 
dotnet.exe C:\vc-storefront-core\VirtoCommerce.Storefront.dll
```
The output in the console will say something like:
```
Now listening on: http://localhost:5000
```
So if you then browse to http://localhost:5000, you'll see your storefront application (and the console will show logging info about your visit)

### Host on Windows with IIS
VirtoCommerce.Storefront project already include the **web.config** file with all necessary settings for runing in IIS.
How to configure IIS application to host ASP.NET Core site please learn more in the official Microsoft ASP.NET Core documentation
[Host ASP.NET Core on Windows with IIS](https://docs.microsoft.com/en-us/aspnet/core/publishing/iis)

Open the VirtoCommerce Storefront application in the browser.
