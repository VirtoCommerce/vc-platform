How to create a new Dummy module
==========

How to create a new module from scratch? Follow the Dummy module example.

## 1. Create solution and projects in correct structure
1. Create empty **_DummyModule_** solution: https://docs.microsoft.com/en-us/visualstudio/get-started/tutorial-projects-solutions?view=vs-2019##create-a-solution
2. Add **_src_** and **_tests_** Solution Folders (using Visual Studio)
3. In **_src_** add project using "_Class library (.NET Core)_" template:
   1. Project name: **_DummyModule.Core_** 
   2. Location: ...\\_DummyModule\\**src**_ folder (create the missing **src** folder).
3. Add **_DummyModule.Data_** and **_DummyModule.Web_** projects in the same way, just ensure they are located under **src** folder.
3. Delete the auto-generated Class1.cs from all projects.
3. In **_tests_** folder add a project using "_xUnit Test Project (.NET Core)_" template:
   1. Project name: **_DummyModule.Tests_** 
   2. Location: ...\\_DummyModule\\**tests**_ folder (create the missing **tests** folder).
3. Set "Target framework" to ".NET Core 3.1" for all 4 projects.
5. References to projects:
   1. **DummyModule.Data**: add reference to DummyModule.Core project
   1. **DummyModule.Web**: add references to DummyModule.Core, DummyModule.Data projects
   1. **DummyModule.Tests**: add references to DummyModule.Core, DummyModule.Data, DummyModule.Web projects
5. References to NuGet packages:
   1. **DummyModule.Core**: add reference to the latest version **_VirtoCommerce.Platform.Core_** package.
   1. **DummyModule.Data**: add reference to the latest version **_VirtoCommerce.Platform.Data_** package.
5. References to NuGet packages in **_VirtoCommerce.Platform.Web_**:
   1. (Double click in Visual Studio to) open _DummyModule.Web.csproj_ file for editing;
   1. Add new ItemGroup:
   ```
   <ItemGroup>
     <PackageReference Include="Microsoft.AspNetCore.App" />
   </ItemGroup>
   ```
6. Compile the solution (there should be no warnings). 

## 2. Fill DummyModule.Core project
1. Add class **_ModuleConstants.cs_** for module constants:
   1. Inside **_ModuleConstants_** add sub-classes **_Security_** and **_Permissions_** like this:
    ```cs
            public static class Security
            {
                public static class Permissions
                {
                    public const string Read =   "dummy:read";
                    public const string Create = "dummy:create";
                    public const string Access = "dummy:access";
                    public const string Update = "dummy:update";
                    public const string Delete = "dummy:delete";

                    public static string[] AllPermissions = { Read, Create, Access, Update, Delete };
                }
            }
    ```

    2. Add sub-classes **_Settings_** and **_General_** containing settings' definitions of type **_SettingDescriptor_**, e.g.,
    ```cs
            public static SettingDescriptor DummyInteger = new SettingDescriptor
            {
                Name = "Dummy.Integer",
                GroupName = "Dummy|General",
                ValueType = SettingValueType.Integer,
                DefaultValue = 50
            };
    ```
    Check sample [ModuleConstants.cs](https://github.com/VirtoCommerce/vc-samples/.....) for complete code.

    3. Other constants.

1. If there is any domain models in your module, add **Models** folder. All domain models should be defined in Models folder.
   1. If there is any need for search services, add **Search** sub-folder. Add all **_SearchCriteria_** and **_SearchResult_** classes there.

1. **Services** folder: in order to use the created models, create **interfaces** of the required services. Typically, interfaces containing CRUD and search operations are defined.

1. **Events** folder: add model-related changing/changed events. Derive from the base `GenericChangedEntryEvent` class.

1. **Notifications** folder: define new types of **_Notifications_**, that your module would expose. Each class should inherit from **_EmailNotification_**/**_SmsNotification_** or own class, based on **_Notification_**.

## 3. Fill DummyModule.Data project
1. If there is any (domain) data to be persisted, add **Models** folder. All persistency related models should be defined in Models folder. Typically, each of the classes under this folder would have a corresponding class in DummyModule.**Core**/Models. The class should:
   1. define the DB table structure and restrictions;
   1. be able to convert from/to the corresponding Core model class.
2. Add **Repositories** folder. Under it:
	 1. Add class **_DummyDbContext_**, deriving from `DbContextWithTriggers`. Check and copy class contents from [sample DummyDbContext.cs](https://github.com/VirtoCommerce/vc-samples/.....).
	 2. Add class **_DesignTimeDbContextFactory_**. Check and copy class contents from [sample DesignTimeDbContextFactory.cs](https://github.com/VirtoCommerce/vc-samples/.....). Double-check that connection to your development SQL server is correct.
	 3. Add data repository abstraction (interface), deriving from **_IRepository_** for the defined persistency model.
   4. Add the repository implementation class for the previously defined interface and deriving from `DbContextRepositoryBase<DummyDbContext>`.
3. Generate code-first migrations:
   1. Open NuGet **Package Manager Console**;
   1. Select "src\DummyModule.**Data**" as "**Default project**";
   1. Run command:
    ```
    Add-Migration Initial -Context DummyModule.Data.Repositories.DummyDbContext -StartupProject DummyModule.Data -Verbose -OutputDir Migrations
    ```
    A new migration gets generated.
4. **Caching** folder: add it, if data caching should be used. This folder is for the cache region classes. Typically, each model should have its own region. Derive CacheRegion from generic `CancellableCacheRegion<T>` class e.g., `public class StoreCacheRegion : CancellableCacheRegion<StoreCacheRegion>`.
5. **Services** folder: add implementations of the interfaces that were defined in the **.Core** project.
6. **ExportImport** folder: add class for data export/import. It should be called from **_Module.cs_** and contain implementation for module data export and import.
7. **Handlers** folder: add handlers for the _domain events_, which were defined under **.Core/Events**.

## 4. Fill DummyModule&#46;Web project
Typical structure of **.Web** project is:
* Controllers/Api - API controllers;
* Localizations - translation resources for UI;
* Scripts - AngularJS module for extending Platform Manager UI;
* Module.cs - the entry point for module managed code;
* module.manifest - the main file of a module, the module definition.

**Project items to add:**
1. **_module.manifest_** (xml file)
   * Identifier - a module identifier, each modules' identifier should be unique:
    ```xml
    <id>VirtoCommerce.Dummy</id>
    ```
   * Versioning - actual version and (optional) prerelease version tag of a module:
    ```xml
    <version>1.0.0</version>
    <version-tag>v1</version-tag>
    ```
   * Required minimal version of VC Platform:
    ```xml
    <platformVersion>3.0.0</platformVersion>
    ```
   * Module dependencies - list of modules (with versions), functions of which will be used in the new module:
    ```xml
    <dependencies>
        <dependency id="VirtoCommerce.Core" version="3.0.0" />
    </dependencies>
    ```
   * Title and description - name and description of a new module that will be displayed in the Platform Manager and Swagger UI:
    ```xml
    <title>Dummy module</title>
    <description>Sample module for training purposes.</description>
    ```
   * Authors - list of the developers, who wrote the module:
    ```xml
    <authors>
        <author>Andrei Kostyrin</author>
        <author>Egidijus Mazeika</author>
    </authors>
    ```
   * Owners - list of the module owners:
    ```xml
    <owners>
        <owner>Virto Commerce</owner>
    </owners>
    ```
   * Module groups - optional, logical grouping of the modules:
    ```xml
    <groups>
        <group>samples</group>
    </groups>
    ```
   * ProjectUrl - optional URL to get more details on the module:
    ```xml
    <projectUrl>https://github.com/VirtoCommerce/vc-samples</projectUrl>
    ```
   * IconUrl - optional icon for the module to display in Platform's module management UI:
    ```xml
    <iconUrl></iconUrl>
    ```
   * ReleaseNotes, copyright, tags - optional, additional information for the module:
    ```xml
    <releaseNotes>First version.</releaseNotes>
    <copyright>Copyright © 2011-2019 Virto Commerce. All rights reserved</copyright>
    <tags>sample</tags>
    ```
   * AssemblyFile and ModuleType - information for the module's managed code library loading into VC Platform:
    ```xml
    <assemblyFile>DummyModule.Web.dll</assemblyFile>
    <moduleType>DummyModule.Web.Module, DummyModule.Web</moduleType>
    ```

2. **JsonConverters** folder: add it, if polymorphic data should be accepted by the API methods and deserialized correctly. This folder should contain converter(s) for populating the JSON values onto the target object.

3. **_Module.cs_** class: the main entry point for module managed code, containing database initialization, registration of new repositories, services, model types and overrides:
    1. Implement [_VirtoCommerce.Platform.Core.Modularity.IModule_](https://github.com/VirtoCommerce/vc-platform/blob/release/3.0.0/src/VirtoCommerce.Platform.Core/Modularity/IModule.cs) interface like this:
    ```cs
    public class Module : IModule
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // database initialization
            var configuration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("VirtoCommerce.Dummy") ?? configuration.GetConnectionString("VirtoCommerce");
            serviceCollection.AddDbContext<DummyDbContext>(options => options.UseSqlServer(connectionString));
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            // register settings
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

            // register permissions
            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x =>
                new Permission() { GroupName = "Dummy", Name = x }).ToArray());

        }

        public void Uninstall()
        {
            // do nothing in here
        }
    }
    ```
   2. If the module should support data export/import, implement [_IExportSupport_](https://github.com/VirtoCommerce/vc-platform/blob/release/3.0.0/src/VirtoCommerce.Platform.Core/ExportImport/IExportSupport.cs) and [_IImportSupport_](https://github.com/VirtoCommerce/vc-platform/blob/release/3.0.0/src/VirtoCommerce.Platform.Core/ExportImport/IImportSupport.cs) interfaces accordingly. Call the methods of the class(es), that were defined in **.Data/ExportImport** folder.
    
4. Add **Controllers/Api** folder, if any API endpoints need to be exposed. Add API Controller(s), derived from _Microsoft.AspNetCore.Mvc.Controller_. Sample ASP.NET MVC endpoint:
    ```cs
        [HttpGet]
        [Route("getnew")]
        [Authorize(ModuleConstants.Security.Permissions.Create)]
        public ActionResult<DummyModel> GetNewDummyModel()
    ```
    If the endpoint should have a restricted access, an **_Authorize_** attribute with the required permission should be provided. Use the **_ModuleConstants_** class, which was previously defined in **_Dummy.Core_** project.

5. Optional **Content** folder: add module icon, stylesheets or any other optional content.

6. Optional **Scripts** folder: add **_module.js_** and any required subfolders: blades, resources, widgets, etc. Sample **_module.js_** file:
    ```js
    //Call this to register our module to main application
    var moduleName = "virtoCommerce.dummy";

    if (AppDependencies !== undefined) {
        AppDependencies.push(moduleName);
    }

    angular.module(moduleName, [])
        ;
    ```

7. If there are any JavaScript or stylesheet files in the project:
    1. Copy **package.json** from [sample package.json](https://github.com/VirtoCommerce/vc-module-order/blob/release/3.0.0/src/VirtoCommerce.OrdersModule.Web/package.json);
    1. Copy **webpack.config.js** from [sample webpack.config.js](https://github.com/VirtoCommerce/vc-module-order/blob/release/3.0.0/src/VirtoCommerce.OrdersModule.Web/webpack.config.js);
    2. Change the namespace on line 36 in webpack.config.js to be equal to module's identifier:
    ```js
    namespace: 'VirtoCommerce.Dummy'
    ```
    4. Open command prompt and navigate to DummyModule&#46;Web folder:
        1. Run `npm install`
        1. Run `npm run webpack:dev`


8. **Localizations** folder. Add **_en.VirtoCommerce.Dummy.json_** file:
    ```json
    {
        "permissions": {
            "dummy:access": "Open Dummy menu",
            "dummy:create": "Create Dummy related data",
            "dummy:read": "View Dummy related data",
            "dummy:update": "Update Dummy related data",
            "dummy:delete": "Delete Dummy related data"
        },
        "settings": {
            "Dummy": {
                "ShortText": {
                    "title": "Dummy dictionary",
                    "description": "ShortText dummy dictionary setting"
                },
                "Integer": {
                    "title": "Dummy integer",
                    "description": "Dummy integer setting"
                },
                "DateTime": {
                    "title": "Dummy DateTime",
                    "description": "Dummy DateTime setting"
                }
            }
        }
    }
    ```

## 5. Fill DummyModule.Tests project
1. **UnitTests** folder: add unit tests here.
2. **IntegrationTests** folder: add integration tests here. Ensure, that each integration tests class is marked with **Trait** attribute:
    ```cs
    [Trait("Category", "IntegrationTest")]
    ```

## 6. Create module package
1. please read the [article](https://github.com/VirtoCommerce/vc-platform/blob/release/3.0.0/build/README.md)
