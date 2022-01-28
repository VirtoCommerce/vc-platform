
# Quick start
Use this guide to deploy, configure and run precompiled Virto Commerce Platform on local machine.

<iframe width="560" height="315" src="https://www.youtube.com/embed/AHPgLG771Uk" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

## Prerequisites
The following prerequisites needs to be installed to deploy and run Virto Commerce.

* [.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Install CLI

`vc-build` is the official CLI for building, deploy releases, create and push packages, and package management for projects based on Virto Commerce.

* Open new command line or terminal window

* Install Virto Commerce CLI tools
```console
dotnet tool install -g VirtoCommerce.GlobalTool
```

* We recommend to use Virto Commerce in HTTPS mode. .NET Core SDK installs the ASP.NET Core HTTPS development certificate, but it's not trusted. To trust the certificate, perform the one-time step to run the dotnet `dev-certs`tool
```console
dotnet dev-certs https --trust
```

## Setup

* Create a clean folder for Virto Commerce for example `C:\vc-platform`
```console
mkdir C:\vc-platform
```

* In the command line go to the `vc-platform` folder
```console
cd C:\vc-platform
```

* Install platform and modules
```console
vc-build install
```

The `vc-build install` command creates a Virto Commerce platform or other artifacts based on a default template.

## Configure

* In the `vc-platform` folder open the **appsettings.json** file in a text editor.
* Find a **ConnectionStrings** section and modify the **VirtoCommerce** node to your SQL server configuration accordingly:
```json
    "ConnectionStrings": {
        "VirtoCommerce" : "Data Source={SQL Server URL};Initial Catalog={Database name};Persist Security Info=True;User ID={User name};Password={User password};MultipleActiveResultSets=True;Connect Timeout=30"
    },
```

## Run

* Run the VirtoCommerce platform using `dotnet` CLI command
```console
dotnet.exe VirtoCommerce.Platform.Web.dll
```
`dotnet.exe` starts Virto Commerce platform, connects to database and creates tables.

    After that Virto Commerce platform is ready to accept API requests from any client.

* To access Virto Commerce administrator portal go to: <a href="https://localhost:5001" target="_blank">https://localhost:5001</a>

* Use the following credentials:
```console
Login: admin
Password: store
```

* I will ask if to prepare the platform with the sample data or leave it empty. Leave it empty. The data preparation will be described in the next guidelines.
* It will also ask to change the password after the first login. Update password to the new one.

