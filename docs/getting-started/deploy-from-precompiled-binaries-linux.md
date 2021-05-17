# Deploy on Linux
Use this guide to <a class="crosslink" href="https://virtocommerce.com/ecommerce-hosting" target="_blank">deploy</a> and configure precompiled Virto Commerce Platform V3.

## Prerequisites

* [Prerequisites for .NET Core on Linux](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites)
* [.NET Core SDK on Linux](https://dotnet.microsoft.com/download/linux-package-manager/ubuntu19-04/sdk-current)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2017-editions)



## You have two options for installing the platform 

* Manual downloading the precomplied binaries
* Using CLI (beta)
  

## Downloading the precomplied binaries

* Navigate to the <a href="https://github.com/VirtoCommerce/vc-platform/releases">Releases section of Virto Commerce Platform in GitHub.</a>

* You will find **VirtoCommerce.Platform.3.x.x.zip** file. In this file the site has already been built and can be run without additional compilation. The source code is not included. Run command to download binaries:

```console
wget "https://github.com/VirtoCommerce/vc-platform/releases/download/3.x.x/VirtoCommerce.Platform.3.x.x.zip"
```

* Unpack this zip to a local directory **/vc-platform-3**. After that you will have the directory with Platform precompiled files.

```console
unzip VirtoCommerce.Platform.3.x.x.zip -d vc-platform-3
```

## Using `vc-build` CLI (beta).

* Install vc-build 
```console
dotnet tool install -g VirtoCommerce.GlobalTool
```
* Install platform and modules
```console
vc-build install
```
Also you can specify the platform version:
```console
vc-build install -version 3.55.0
```
Check out [vc-build for packages management](https://github.com/VirtoCommerce/vc-platform/tree/dev/build#packages-management)  for more info.


## Setup

### Configure application strings

* Open the **appsettings.json** file in a text editor.
* In the **ConnectionStrings** section change **VirtoCommerce** node (provided user should have permission to create new database):

```json
    "ConnectionStrings": {
        "VirtoCommerce" : "Data Source={SQL Server URL};Initial Catalog={Database name};Persist Security Info=True;User ID={User name};Password={User password};MultipleActiveResultSets=True;Connect Timeout=30"
    },

```

* In the **Assets** section set public url for assets `Assets:FileSystem:PublicUrl` with url of your application, this step is needed in order to display images

```json
"Assets": {
        "Provider": "FileSystem",
        "FileSystem": {
            "RootPath": "~/assets",
            "PublicUrl": "https://localhost:5001/assets/" <-- Set your platform application url with port localhost:5001
        },
    },
```

* In the **Content** section set public url for content `Content:FileSystem:PublicUrl` with url of your application, this step is needed in order for configure CMS content storage

```json
"Content*": {
        "Provider": "FileSystem",
        "FileSystem": {
            "RootPath": "~/cms-content",
            "PublicUrl": "https://localhost:5001/cms-content/" <-- Set your platform application url with port localhost:5001
        },
    },
```

### Running the Platform by CLI "dotnet"

* Install and trust HTTPS certificate

Run steps described in [this article](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.0&tabs=visual-studio#trust-https-certificate-from-windows-subsystem-for-linux) to trust the .NET Core SDK HTTPS development certificate on Linux.

Read more about [enforcing HTTPS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.0&tabs=visual-studio#trust)

* Run the Platform by following command:

```console
export ASPNETCORE_URLS="http://+:5000;https://+:5001"
cd vc-platform-3
dotnet VirtoCommerce.Platform.Web.dll
```

The output in the console will say something like:

```console
Now listening on: http://[::]:5000
Now listening on: https://[::]:5001
```

### First run and sign in

* Open `https://localhost:5001` url in your browser. "Your connection is not private" might appear. Click "Advanced" and "Proceed to ...".
Read more on removing this error and using a self-signed certificate: [Trust the ASP.NET Core HTTPS development certificate](https://www.hanselman.com/blog/DevelopingLocallyWithASPNETCoreUnderHTTPSSSLAndSelfSignedCerts.aspx)
* The application will create and initialize database on the first request. After that you should see the sign in page. Use the following credentials:
  * Login: **admin**
  * Password: **store**

