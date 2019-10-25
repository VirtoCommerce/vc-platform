# Deploy from source code

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

- Clone module repository from GitHub into `Modules` folder

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
