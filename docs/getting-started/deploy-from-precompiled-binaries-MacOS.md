# Deploy on Mac OS
Use this guide to <a class="crosslink" href="https://virtocommerce.com/ecommerce-hosting" target="_blank">deploy</a> and configure precompiled Virto Commerce Platform V3.

## Prerequisites

* [.NET Core SDK on Mac OS](https://docs.microsoft.com/en-us/dotnet/core/install/macos)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2017-editions)

## Installation .NET Core SDK

* Navigate to the <a href="https://dotnet.microsoft.com/download/dotnet-core">Download .NET Core SDK</a>

* Install the LTS version, it will include components for build and launch runtime application.

## Installation LibSass

* Download the nuget package <a href="https://www.nuget.org/packages/LibSassHost.Native.osx-x64">LibSass Host Native for for OS X (x64)</a>
* You need to unzip the package and move the library to dotnet location path. You can find the location of dotnet using CLI **dotnet --info**  

```console
unzip libsasshost.native.osx-x64.1.x.x.nupkg -d libsass
```

```console
sudo cp libsass/runtimes/osx-x64/native/libsass.dylib /usr/local/share/dotnet/shared/Microsoft.NETCore.App/3.x.x/
```

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

