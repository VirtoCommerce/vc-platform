---
date: '2019-10-16'
title: 'Deploy Platform V3 from precompiled binaries on Linux'
layout: docs
---
## Summary

Use this guide to <a class="crosslink" href="https://virtocommerce.com/ecommerce-hosting" target="_blank">deploy</a> and configure precompiled Virto Commerce Platform V3.

### Prerequisites

[Prerequisites for .NET Core on Linux](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites)

[.NET Core SDK on Linux](https://dotnet.microsoft.com/download/linux-package-manager/ubuntu19-04/sdk-current)

[Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2017-editions)

## Downloading the precomplied binaries

* Navigate to the <a href="https://github.com/VirtoCommerce/vc-platform/releases">Releases section of Virto Commerce Platform in GitHub.</a>

* You will find **VirtoCommerce.Platform.3.x.x.zip** file. In this file the site has already been built and can be run without additional compilation. It does not includes all the source code. Run command to download binaries:

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

* In the **Assets** section set public url for assets `Assets:FileSystem:PublicUrl` with url of your application, this step is needed in order for display images

```json
"Assets": {
        "Provider": "FileSystem",
        "FileSystem": {
            "RootPath": "~/assets",
            "PublicUrl": "https://localhost:5001/assets/" <-- Set your platform application url with port localhost:5001
        },
    },
```

### Running the Platform by CLI "dotnet"

* Install and trust HTTPS certificate

To trust the .NET Core SDK HTTPS development certificate on Linux run steps described in the [article](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.0&tabs=visual-studio#trust-https-certificate-from-windows-subsystem-for-linux)

Read more about [enforcing HTTPS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.0&tabs=visual-studio#trust)

* Run the Platform by follow command

```console
export ASPNETCORE_URLS=http://+:5000
dotnet.exe /vc-platform-3/VirtoCommerce.Platform.Web.dll
```

The output in the console will say something like:

```console
Now listening on: http://localhost:5000
Now listening on: https://localhost:5001
```

### First sign in

* Open in your browser follow url `https://localhost:5001` in the warning for not private connections that appears click advanced and continue work. How to remove this error and use a trusted self-signed certificate please read in this article [Trust the ASP.NET Core HTTPS development certificate](https://www.hanselman.com/blog/DevelopingLocallyWithASPNETCoreUnderHTTPSSSLAndSelfSignedCerts.aspx)
* On the first request the application will create and initialize database. After that you should see the sign in page. Use the following credentials:
  * Login: **admin**
  * Password: **store**

## License

Copyright (c) Virto Solutions LTD.  All rights reserved.

Licensed under the Virto Commerce Open Software License (the "License"); you
may not use this file except in compliance with the License. You may
obtain a copy of the License at

http://virtocommerce.com/opensourcelicense

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
implied.
