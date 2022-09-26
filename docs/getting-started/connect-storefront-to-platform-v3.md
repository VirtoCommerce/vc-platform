# Deploy Storefront

Official Storefront Application for Virto Commerce based on [Virto Commerce Storefront](https://github.com/VirtoCommerce/vc-storefront) and 
[Vue B2B Theme](https://github.com/VirtoCommerce/vc-theme-b2b-vue).

!!! note
	If Platform and Storefront are deployed in the same on-premises environment, Storefront should be deployed on a different port than Platform. 

## Prerequisites
* Install vc-platform 3.x the latest version. [Deploy On Window](deploy-from-precompiled-binaries-windows.md) or  [Deploy On Linux](deploy-from-precompiled-binaries-linux.md)
* Install ecommerce bundle
* Configure stores from scratch or Install sample data
* Go to Security, Create a new frontend user as Administrator, keep login and password

## Install Storefront

* Navigate to the [Releases section of Virto Commerce Storefront in GitHub](https://github.com/VirtoCommerce/vc-storefront/releases).
* You will find **VirtoCommerce.Storefront.6.x.x.zip** file. In this file the storefront has already been built and can be run without additional compilation.
* Unpack this zip to a local directory **C:\vc-storefront**. After thatm you will have the directory with Storefront precompiled files.
* Open the **appsettings.json** file in a text editor.
* In the **Endpoint** section change **Url**, **UserName** and **Password** with correct path and frontend user credentials for Virto Commerce Platform:

```json
...
 "Endpoint": {
	 "Url": "https://localhost:5001",
	 "UserName": "admin",
	 "Password": "store",
```

* In the **VirtoCommerce** section change **DefaultStore** to **B2B-Store**.
```json
...
 "VirtoCommerce": {
    "DefaultStore": "B2B-Store",
```


## Setup Theme

* Navigate to the [Releases section of Vue B2B Theme in GitHub](https://github.com/VirtoCommerce/vc-theme-b2b-vue/releases).
* You will find **vc-theme-b2b-vue-1.x.x.zip** file. In this file the theme has already been built and can be run without additional compilation.
* Unpack this zip and Copy **default** theme to `C:\vc-storefront\wwwroot\cms-content\Themes\B2B-Store\`. So, complete path to theme is `C:\vc-storefront\wwwroot\cms-content\Themes\B2B-Store\default`.

!!! note
    Storefront resolves theme content by paths in CMS Content `Themes\{StoreCode}\{ThemeName}`. It provides support for multi-store and multi-theme functionality.

## Run Storefront

* Run the VirtoCommerce platform using dotnet CLI command

```console
	dotnet VirtoCommerce.Storefront.dll
```

dotnet.exe starts Virto Storefront, loads theme and connects to Virto Commerce Platform via API.

After that Virto Storefront is ready to open in the browser.

![vc-storefront-b2b-store](../media/vc-storefront-b2b-store.png)


## FAQ

### Configure Storefront CMS Content

Based on your deployment schema, you can configure Content Storage. Storefront  **appsettings.json** file contains **ContentConnectionString** setting with pointed to the folder with actual themes and pages content:

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

### Running the Storefront only on HTTP schema

- In order to run the platform only at HTTP schema in production mode, it's enough to pass only HTTP URLs in `--urls` argument of the `dotnet` command.

```console
  dotnet VirtoCommerce.Storefront.dll --urls=http://localhost:5002
```

### Running the Platform on HTTPS schema

- Install and trust HTTPS certificate

Run to trust the .NET Core SDK HTTPS development certificate:

```console
    dotnet dev-certs https --trust
```

Read more about [enforcing HTTPS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.0&tabs=visual-studio#trust)

```console
    dotnet VirtoCommerce.Storefront.dll --urls=https://localhost:4302/
```

- Trust the .Net Core Development Self-Signed Certificate. More details on trusting the self-signed certificate can be found [here](https://blogs.msdn.microsoft.com/robert_mcmurray/2013/11/15/how-to-trust-the-iis-express-self-signed-certificate/)

### Forward the scheme for Linux and non-IIS reverse proxies

Apps that call UseHttpsRedirection and UseHsts put a site into an infinite loop if deployed to an Azure Linux App Service, Azure Linux virtual machine (VM), Linux container or behind any other reverse proxy besides IIS. TLS is terminated by the reverse proxy, and Kestrel isn't made aware of the correct request scheme. OAuth and OIDC also fail in this configuration because they generate incorrect redirects. UseIISIntegration adds and configures Forwarded Headers Middleware when running behind IIS, but there's no matching automatic configuration for Linux (Apache or Nginx integration).

To forward the scheme from the proxy in non-IIS scenarios, set `ASPNETCORE_FORWARDEDHEADERS_ENABLED` environment variable to `true`.

For more details on how it works, see the Microsoft [documentation](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-5.0#forward-the-scheme-for-linux-and-non-iis-reverse-proxies).
