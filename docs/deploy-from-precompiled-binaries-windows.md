---
date: '2019-10-16'
title: 'Deploy Platform V3 from precompiled binaries to Windows'
layout: docs
---
## Summary

Use this guide to <a class="crosslink" href="https://virtocommerce.com/ecommerce-hosting" target="_blank">deploy</a> and configure precompiled Virto Commerce Platform V3.

### Prerequisites

[Prerequisites for .NET Core on Windows](https://docs.microsoft.com/en-us/dotnet/core/windows-prerequisites)

[.NET Core SDK](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-3.0.100-windows-x64-installer)

[Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-2017-editions)

## Downloading the precomplied binaries

* Navigate to the <a href="https://github.com/VirtoCommerce/vc-platform/releases">Releases section of Virto Commerce Platform in GitHub.</a>

* You will find **VirtoCommerce.Platform.3.x.x.zip** file. In this file the site has already been built and can be run without additional compilation. It does not includes all the source code.

* Unpack this zip to a local directory **C:\vc-platform-3**. After that you will have the directory with Platform precompiled files.

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

### Running the Platform by CLI "dotnet run"

Run the Platform by follow command

```console
dotnet.exe C:\vc-platform-3\VirtoCommerce.Platform.Web.dll
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

### Host on Windows with IIS

VirtoCommerce.Platform project already include the **web.config** file with all necessary settings for running in IIS.
How to configure IIS application to host ASP.NET Core site please learn more in the official Microsoft ASP.NET Core documentation
[Host ASP.NET Core on Windows with IIS](https://docs.microsoft.com/en-us/aspnet/core/publishing/iis)

Open the VirtoCommerce Platform application in the browser.

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
