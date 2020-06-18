---
aliases:
  - docs/vc2devguide/deployment/storefront-deployment/storefront-source-code-getting-started
date: '2018-08-30'
layout: docs
title: 'Deploy Storefront from source code'

---
## Summary

Use this guide to deploy and configure Virto Commerce Storefront from source code and setup development environment.


## Source code getting started

### Prerequisites
[Prerequisites for .NET Core on Windows](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites)

[Microsoft Visual C++ 2015 Redistributable](https://www.microsoft.com/en-us/download/details.aspx?id=53840) (required for SCSS engine)

### Downloading source code

Fork your own copy of VirtoCommerce Storefront to your account on GitHub:

1. Open VirtoCommerce Storefront in GitHub and click Fork in the upper right corner.
If you are a member of an organization on GitHub, select the target for the fork.
2. Clone the forked repository to local machine:
```
git clone https://github.com/<<your GitHub user name>>/vc-storefront-core.git C:\vc-storefront-core
```
3. Switch to the cloned directory:

```cd C:\vc-storefront-core```

4. Add a reference to the original repository:

```git remote add upstream https://github.com/VirtoCommerce/vc-storefront-core.git```

In result you should get the C:\vc-storefront-core folder which contains full storefront source code. To retrieve changes from original Virto Commerce Storefront repository, merge upstream/master branch.

### Configuring VirtoCommerce Platform Endpoint
Set actual platform endpoint values in the C:\vc-storefront-core\VirtoCommerce.Storefront\appsettings.json.
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
ASP.NET Core represents a new tools a **Secret Manager tool**, which allows in development to keep secrets out of your code.
You can find more about them [here](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?tabs=visual-studio)

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
mklink /d C:\vc-storefront-core\VirtoCommerce.Storefront\wwwroot\cms-content C:\vc-platform\VirtoCommerce.Platform.Web\App_Data\cms-content
```
2. If you did not install sample data with your platform, you need to create new store in platform manager and download themes as it described in this article
[Theme development](https://virtocommerce.com/docs/vc2devguide/working-with-storefront/theme-development)

### Host on Windows with IIS
VirtoCommerce.Storefront project already include the **web.config** file with all necessary settings for runing in IIS.
How to configure IIS application to host ASP.NET Core site please learn more in the official Microsoft ASP.NET Core documentation
[Host ASP.NET Core on Windows with IIS](https://docs.microsoft.com/en-us/aspnet/core/publishing/iis)
