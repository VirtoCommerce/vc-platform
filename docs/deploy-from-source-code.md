# Deploy from source code

## Platform from source code getting started

- Get the latest platform source code from [release/3.0.0](https://github.com/VirtoCommerce/vc-platform/tree/release/3.0.0)
- Set public URL for assets `Assets:FileSystem:PublicUrl` with url of your application. This step is needed in order to display images on the Commerce Manager app:

```json
"Assets": {
        "Provider": "FileSystem",
        "FileSystem": {
            "RootPath": "~/assets",
            "PublicUrl": "http://localhost:10645/assets/" <-- Set your platform application url with port localhost:10645
        },
     
    },
```

### Run from Visual Studio

- Open `VirtoCommerce.Platform.sln` solution in Visual Studio 2019 and press F5

### Run via dotnet CLI

- Open console

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

```console
dotnet run -c Development --no-launch-profile
```

**Note:** you can add `--no-build` flag to speed the start, if you have compiled the solution already.

- Open  `http://localhost:10645` in the browser.
- On the first request the application will create and initialize database. After that you should see the sign in page. Use the following credentials: `admin/store` to sign in.

**Note:** Don't forget to change them after the first sign in.

## Module from source code getting started

- Run VC Platform  from precompiled binaries or source code as described in the steps above
- Run command to change the current directory

```console
cd src\VirtoCommerce.Platform.Web\Modules
```

- Clone module repository from GitHub into `Modules` folder

```console
git clone  https://github.com/VirtoCommerce/{module-name.git}  src\VirtoCommerce.Platform.Web\Modules\{module-name}
```

- Build module code

```console
cd src\VirtoCommerce.Platform.Web\Modules\{module-name}\src\{module-name}.Web
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

- Restart the Platform to load the new module assemblies into the Platform's application process

## How to debug module

- Install and run platform as described in steps above.
- Setup module from source code as described above, open the module solution in Visual Studio and attach the debugger to the dotnet.exe process, running VC Platform.

**Note:** to distinguish between multiple dotnet.exe processes, If running in Windows, use Task Manager to distinguish between multiple dotnet.exe processes: add "Command line" column to Details tab. This would show which app each dotnet.exe is running..
