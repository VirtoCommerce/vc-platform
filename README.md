# Meet the new 3.0 major version of Virto Commerce platform 
Our development efforts were focused on moving to ASP.NET Core, performance, architecture improvements, further enhancements and fixing arhitectural bugs. 

## What were our objective when starting development project on VC platform v3? 

- Change primary technology stack to .NET Core for the platform application and all key modules. 
- Eliminate known technical and architecture design issues of 2.x version (Caching, Overloaded core module, Asynchronous code, Platform Complexity, Extensibility, Performance, Authentication and Authorization) 
- Provide easy and clear migration from 2.x version by preserving complete backward compatibility for API and Database Schema 
- The platform and 18 core modules were planned to be migrated. 

## Release status note 
- *We encourage you to try and investigate the new version of the product and give us your feedback*
- *This is a beta release, which hasn't been verified on a production project yet*
- *We have delivered a simple migration from 2.x version by preserving complete backward compatibility for API and Database Schema. You'll need an additional effort when there are custom changes in 2.x version. Please follow our migration guide during the project migration*
- **_We cannot guarantee the backward compatibility of current beta version with the final 3.X release_**

# What's new
Read [What's new](docs/whats-new.md) article

# Getting started
## Platform from precompiled binary getting started

- Deploy Platform on [Windows](docs/deploy-from-precompiled-binaries-windows.md)
- Deploy Platform on [Linux](docs/deploy-from-precompiled-binaries-linux.md)

## Platform from source code getting started 
  - Get the latest platform source code from [release/3.0.0](https://github.com/VirtoCommerce/vc-platform/tree/release/3.0.0)
  - Set public url for assets `Assets:FileSystem:PublicUrl` with url of your application, this step is needed in order for display images 

```json
"Assets": {
        "Provider": "FileSystem",
        "FileSystem": {
            "RootPath": "~/assets",
            "PublicUrl": "http://localhost:10645/assets/" <-- Set your platform application url with port localhost:10645
        },
     
    },
```
  - Open `VirtoCommerce.Platform.sln` solution in Visual Studion 2019 and press F5 or run via `dotnet` CLI by typing in the console the follow commands

   ```console
    cd src\VirtoCommerce.Platform.Web
   ```
   
   - Install all required npm packages

   ```console
    npm ci
   ```
    
   - Bundle all js scripts and css styles

   ```console
    npm run webpack:build
   ```

   - Run platform by dotnet CLI. 
   
   **Note:** you can add `--no-build` flag to speed up start if you already compile solution.

   ```console
    dotnet run -c Development --no-launch-profile
   ```

   - Open in your browser follow url `http://localhost:10645`.
   - On the first request the application will create and initialize database. After that you should see the sign in page. Use the following credentials: `admin/store` to sign in. 
  
**Note:** Don't forget to change them after first sign in.

## Module from source code getting started
   - Run platform from binary or source code as described in the steps above 
   
   - Run command to change the current directory
   
   ```console
   cd src\VirtoCommerce.Platform.Web\Modules
   ```
      
   - Clone module repository from GitHub into `Modules` foldr
   
   ```console 
   git clone  https://github.com/VirtoCommerce/{module-name.git}  src\VirtoCommerce.Platform.Web\Modules\{module-name}
   ```
   
   ```console
      cd src\VirtoCommerce.Platform.Web\Modules\{module-name}\src\{module-name}.Web
   ```

   - Build module code

   ```console
      dotnet build -c Development
   ```

   - Install all required npm packages

   ```console
      npm ci 
   ```

   - Bundle all js scripts and css styles

   ```console
      npm run webpack:build
   ```

   - Restart the platform to load new module assemblies into the application process

## How to debug module

- Install and run platform as described in steps above.
- Setup module from source code as described above, open a module solution in Visual Studio and attach debugger for one of dotnet.exe processes.

  **Note:** to distinguish between multiple dotnet.exe processes, If you're running in windows, you can use Task Manager. If you add the Command Line column to the Details tab, it will show you which app that dotnet.exe is running.

## How to Run [Storefront](https://github.com/VirtoCommerce/vc-storefront-core) with new platform version

- [Connect Storefront to Platform](docs/connect-storefront-to-platform-v3.md)

## How to migrate your solution from 2.x to 3.0 platform version

- If your solution doesn't have any custom modules and extensions you just need to use the connection string to the old database for the new 3.0 platfrom version and after first run the update scripts will transfer all your data to the new scheme otherwise, you need to convert your models according to this instruction https://github.com/VirtoCommerce/vc-platform/blob/release/3.0.0/docs/Migrate-Extension-module-from-the-Platform-2.0-to-3.0-version.md.

# License
Copyright (c) Virto Solutions LTD.  All rights reserved.

Licensed under the Virto Commerce Open Software License (the "License"); you
may not use this file except in compliance with the License. You may
obtain a copy of the License at

http://virtocommerce.com/opensourcelicense

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
implied.
