# Deploy storefront kit

[Virto Commerce Storefront Kit](https://github.com/VirtoCommerce/vc-storefront-core/) is Official online shopping website based on VirtoCommerce Platform written on ASP.NET Core. The website is a client application for VC Platform and uses only public APIs while communicating.

!!! note
    If Platform and Storefront are deployed in the same on-premises environment, Storefront should be deployed on different port then Platform. You can do it by `dotnet run CLI`

## Downloading the precomplied binaries

* Navigate to the [Releases section of Virto Commerce Storefront Kit in GitHub](https://github.com/VirtoCommerce/vc-storefront-core/releases).

* You will find **VirtoCommerce.Storefront.5.x.x.zip** file. In this file the site has already been built and can be run without additional compilation. The source code is not included.

* Unpack this zip to a local directory **C:\vc-storefront**. After that you will have the directory with Storefront precompiled files.

## Setup

### Configure application strings

* Open the **appsettings.json** file in a text editor.
* In the **Endpoint** section change **Url**, **UserName**, **Password** with correct path and credentials for Virto Commerce Platform:

```json
...
 "Endpoint": {
     "Url": "https://localhost:5001",
     "UserName": "admin",
     "Password": "store",
```

### Configure CMS Content storage

Storefront  **appsettings.json** file contains **ContentConnectionString** setting with pointed to the folder with actual themes and pages content
```json
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

1. Build and Copy theme to `wwwroot\cms-content\{StoreName}\{ThemeName}`
1. If you have already have installed  platform with sample data, your platform already contains `~/App_Data/cms-content` folder with themes for sample stores and you need only to make symbolic link to this folder by this command:
    ```console
    mklink /d C:\vc-storefront\VirtoCommerce.Storefront\wwwroot\cms-content C:\vc-platform\VirtoCommerce.Platform.Web\App_Data\cms-content
    ```
On Mac OS and Linux:
    ```console
    ln -s ~/vc-storefront/wwwroot/cms-content ~/vc-platform/wwwroot/cms-content
    ```
1. If you did not install sample data with your platform, you need to create new store in platform manager and download themes as it described in this article: [Theme development](../fundamentals/theme-development.md)

### Running the Storefront only on HTTP schema
 
* In order to run the platform only at HTTP schema in production mode, it's enough to pass only HTTP URLs in `--urls` argument of the `dotnet` command.

```console
  dotnet VirtoCommerce.Storefront.dll --urls=http://localhost:5002
```

### Running the Platform on HTTPS schema

* Install and trust HTTPS certificate

Run to trust the .NET Core SDK HTTPS development certificate:

```console
    dotnet dev-certs https --trust
```

Read more about [enforcing HTTPS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.0&tabs=visual-studio#trust)


```console
    dotnet VirtoCommerce.Storefront.dll --urls=https://localhost:4302/
```


* Trust the .Net Core Development Self-Signed Certificate. More details on trusting the self-signed certificate can be found [here](https://blogs.msdn.microsoft.com/robert_mcmurray/2013/11/15/how-to-trust-the-iis-express-self-signed-certificate/)


## Sample themes

### [Default theme](https://github.com/VirtoCommerce/vc-theme-default)
![electronics](https://user-images.githubusercontent.com/7566324/31821605-f36d17de-b5a5-11e7-9bb5-a71803285d8b.png)

### [B2B theme](https://github.com/VirtoCommerce/vc-theme-b2b)
![img_20102017_174148_0](https://user-images.githubusercontent.com/7566324/31821606-f3974b26-b5a5-11e7-8b52-e3b80d6bdd74.png)

