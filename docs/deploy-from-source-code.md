# Deploy from source code

## Platform from source code getting started

* Get the latest platform source code from [release/3.0.0](https://github.com/VirtoCommerce/vc-platform/tree/release/3.0.0)

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
            "PublicUrl": "http://localhost:10645/assets/" <-- Set your platform application url with port localhost:10645
        },
    },
```

* In the **Content** section set public url for content `Content:FileSystem:PublicUrl` with url of your application, this step is needed in order for configure CMS content storage

```json
"Content*": {
        "Provider": "FileSystem",
        "FileSystem": {
            "RootPath": "~/cms-content",
            "PublicUrl": "http://localhost:10645/cms-content/" <-- Set your platform application url with port localhost:10645
        },
    },
```

### Run from Visual Studio

* Open `VirtoCommerce.Platform.sln` solution in Visual Studio 2019 and press F5

### Run via dotnet CLI

* Open console

```console
cd src\VirtoCommerce.Platform.Web
```

* Install all required npm packages

   ```console
    npm ci
   ```

*- Bundle all js scripts and css styles

```console
npm run webpack:build
```

* Run platform by dotnet CLI.

```console
dotnet run -c Development --no-launch-profile
```

**Note:** you can add `--no-build` flag to speed the start, if you have compiled the solution already.

* Open  `http://localhost:10645` in the browser.
* On the first request the application will create and initialize database. After that you should see the sign in page. Use the following credentials: `admin/store` to sign in.

**Note:** Don't forget to change them after the first sign in.

## Module from source code getting started

* Run VC Platform  from precompiled binaries or source code as described in the steps above
* Run command to change the current directory

```console
cd src\VirtoCommerce.Platform.Web\Modules
```

* Clone module repository from GitHub into `Modules` folder

```console
git clone  https://github.com/VirtoCommerce/{module-name.git}  src\VirtoCommerce.Platform.Web\Modules\{module-name}
```

* Build module code

```console
cd src\VirtoCommerce.Platform.Web\Modules\{module-name}\src\{module-name}.Web
dotnet build -c Development
```

* Install all required npm packages

```console
npm ci
```

* Bundle all js scripts and css styles

```console
npm run webpack:build
```

* Restart the Platform to load the new module assemblies into the Platform's application process

## How to debug module

* Install and run platform as described in steps above.
* Setup module from source code as described above, open the module solution in Visual Studio and attach the debugger to the `VirtoCommerce.Platform.Web.exe` process.
![image](https://user-images.githubusercontent.com/7566324/72246321-1d213380-35fb-11ea-9819-c3fdb92d4e42.png)
