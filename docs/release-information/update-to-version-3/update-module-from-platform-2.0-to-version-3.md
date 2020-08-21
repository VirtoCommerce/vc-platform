# Update module

## Introduction

This article describes how to update an existing [CustomerReviews sample](https://github.com/VirtoCommerce/vc-samples/tree/master/CustomerReviews) module from VC Platform version 2.x to 3.0.

!!! note
    A sample module source code can be found here: https://github.com/VirtoCommerce/vc-samples/tree/release/3.0.0/CustomerReviews.

## 0. Before the update
1. Only update from the latest VC v2.x versions is supported. Ensure that the latest v2 versions of the Platform and all modules are installed.
1. Usually, you have more than one custom VC module. Create a **dependency map** between your and the VC modules **before the update**. That helps to update the modules smoothly.
1. Ensure that all your **unit tests are current** and passing
1. In case of any question or issue, submit a new topic to [Virto Commerce Community](https://community.virtocommerce.com/c/bug/11)
1. Please read the [The list of code breaking changes included in 3.0](https://github.com/VirtoCommerce/vc-platform/blob/release/3.0.0/docs/code-breaking-changes-included-in-v3.md)

## 1. Prerequisites
* Visual Studio 2019 (v16.4 or later)

## 2. Make correct structure in solution and projects
1. Open the folder of selected v2 module in file manager (e.g., Windows Explorer).
2. If exists, delete **_packages_** folder.
3. Delete **Properties** folder from each of the projects' folder.
4. Convert the projects from ASP&#46;NET to ASP&#46;NET Core:
    1. Open **_*.csproj_** file of **_CustomerReviews&#46;Web_** project in text editor (e.g., Notepad), clear whole file content and set it to this:
    ```xml
    <Project Sdk="Microsoft.NET.Sdk.Web">
        <PropertyGroup>
            <TargetFramework>netcoreapp3.1</TargetFramework>
            <OutputType>Library</OutputType>
        </PropertyGroup>
        <ItemGroup>
            <Compile Remove="dist\**" />
            <EmbeddedResource Remove="dist\**" />
            <None Remove="dist\**" />
        </ItemGroup>
    </Project>
    ```
    2. Replace all other **_*.csproj_** files' content with this:
    ```xml
    <Project Sdk="Microsoft.NET.Sdk">
        <PropertyGroup>
            <TargetFramework>netcoreapp3.1</TargetFramework>
        </PropertyGroup>
    </Project>
    ```
    3. Read [this article](https://docs.microsoft.com/en-us/aspnet/core/migration/30-to-31) for more info.

5. Create **_src_** and **_tests_** subfolders in module's root folder (Windows Explorer)
6. Move **_CustomerReviews.Core_**, **_CustomerReviews.Data_**, **_CustomerReviews&#46;Web_** projects to **_src_**
7. Move **_CustomerReviews.Test_** project to **_tests_**
7. If exists, move **module.ignore** from _CustomerReviews**&#46;Web**_ project up to the same folder as _CustomerReviews**.sln**_ is.
8. Open **_CustomerReviews.sln_** solution in Visual Studio
9. Remove all projects from the solution
10. Add **_src_** and **_tests_** Solution Folders
11. Add the existing **_CustomerReviews.Core_**, **_CustomerReviews.Data_**, **_CustomerReviews&#46;Web_** projects to **_src_** folder
12. Add the existing **_CustomerReviews.Test_** project to **_tests_** folder
13. Remove all files related to .NET Framework 4.x in **every project**:
    * **App.config**
    * **packages.config**
    * **Web.config**, **Web.Debug.config**, **Web.Release.config**
    * if exists, delete **CommonAssemblyInfo.cs** 
    * if exists, delete all ***.nuspec** files. (NuGet packages are now released without using this file.)
14. Add references to projects:
    1. **CustomerReviews.Data**: add reference to CustomerReviews.Core project
    1. **CustomerReviews&#46;Web**: add references to CustomerReviews.Core, CustomerReviews.Data projects
    1. **CustomerReviews.Tests**: add references to CustomerReviews.Core, CustomerReviews.Data, CustomerReviews&#46;Web projects
15. References to NuGet packages:
    1. **CustomerReviews.Core**: add reference to **_VirtoCommerce.Platform.Core_** package (latest version).
    1. **CustomerReviews.Data**: 
        1. add reference to **_VirtoCommerce.Platform.Data_** package (latest version).
        1. add reference to **_Microsoft.EntityFrameworkCore.Tools_** package (same version as referenced in _VirtoCommerce.Platform.Data_).
16. Add other NuGet dependency packages, if any exists in **_module.manifest_**.

## 3. Make changes in CustomerReviews.Core project

1. If missing, add class **_ModuleConstants.cs_** for module constants:
    1. Inside **_ModuleConstants_** add sub-classes **_Security_** and **_Permissions_**
    2. Add sub-classes **_Settings_** and **_General_** containing settings' definitions of type **_SettingDescriptor_**. Move settings definitions from module.manifest to this class
        > Follow the structure as defined in [ModuleConstants.cs in CustomerModule](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Core/ModuleConstants.cs).
    3. Other constants.

2. Update **ICustomerReviewService.cs**:
    * Refactor all methods to **be asynchronous**: return `Task<>`
    * Rename all methods to have suffix `Async`

3. If there is a search service defined:
    1. Move **_CustomerReviewSearchCriteria_** class to **Search** sub-folder;
    1. Ensure, that CustomerReviewSearchCriteria inherits from `SearchCriteriaBase`; 
    1. Create **_CustomerReviewSearchResult_** class in **Search** sub-folder;
    1. Ensure, that CustomerReviewSearchResult inherits from `GenericSearchResult<CustomerReview>`.
    1. Refactor **_ICustomerReviewSearchService_** to use CustomerReviewSearchResult, all methods be asynchronous, and end with **Async**: 
    ```csharp
    public interface ICustomerReviewSearchService
    {
        Task<CustomerReviewSearchResult> SearchCustomerReviewsAsync(CustomerReviewSearchCriteria criteria);
    }
    ```

4. If any model-related changing/changed events were defined in **Events** folder, ensure that each of them derive from the base `GenericChangedEntryEvent` class.

5. If any custom Notifications were added to **Notifications** folder:
    1. Add reference to NuGet **VirtoCommerce.NotificationsModule.Core** package;
    1. Ensure that each defined Notification class inherits from **_EmailNotification_**/**_SmsNotification_** or own class, based on **_Notification_**.

## 4. Make changes in CustomerReviews.Data project

1. **Repositories** folder
    1. Create **CustomerReviewsDbContext.cs**
        * Add new class **_CustomerReviewsDbContext_**
        * Make it public and derive from `DbContextWithTriggers`
        * Add 2 constructors, using [CustomerDbContext](https://github.com/VirtoCommerce/vc-module-customer/blob/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Repositories/CustomerDbContext.cs) as an example.
         > Note: 1 constructor is **public** and another is **protected**.
        * Override _OnModelCreating_ method, add **CustomerReviewEntity** mapping to modelBuilder and set max length to Id:
        ```csharp
        modelBuilder.Entity<CustomerReviewEntity>().ToTable("CustomerReview").HasKey(x => x.Id);
        modelBuilder.Entity<CustomerReviewEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
        ```
        > Note: a factory for creating derived Microsoft.EntityFrameworkCore.DbContext instances.

    1. Create **DesignTimeDbContextFactory.cs**
        * Add new class **_DesignTimeDbContextFactory_** 
        * Make it public and derive from `IDesignTimeDbContextFactory<CustomerReviewsDbContext>`
        * Implement **_CreateDbContext_** method, using [CustomerModule.Data/Repositories/DesignTimeDbContextFactory.cs](https://github.com/VirtoCommerce/vc-module-customer/blob/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Repositories/DesignTimeDbContextFactory.cs) as an example
        * Ensure that connection string to your development SQL Server in **_UseSqlServer_** method is correct. It would be used while generating code-first migrations. 
    1. Update **ICustomerReviewsRepository.cs**
        * If the module is an extension then derive from derived module's interface repository.
        * Refactor all methods to **be asynchronous**: return `Task<>`
        * Rename all methods to have suffix `Async`
    1. Update **CustomerReviewsRepository.cs**
        * Refactor **_CustomerReviewsRepository_** class to derive from `DbContextRepositoryBase<CustomerReviewsDbContext>` or if the module is an extension then derive from derived module's repository.
        * Refactor the constructors to leave only one, taking the only  **_CustomerReviewsDbContext_** parameter:
        ```csharp
        public CustomerReviewRepository(CustomerReviewsDbContext dbContext) : base(dbContext)
        {
        }
        ```
        * Refactor **_CustomerReviews_** property to access data using DbSet like this:
        ```csharp
        public IQueryable<CustomerReviewEntity> CustomerReviews => DbContext.Set<CustomerReviewEntity>();
        ```
        * Remove **_OnModelCreating_** method

2. **Caching** folder
    1. If missing, create **Caching** folder. This folder is for the cache region classes. Typically, each model should have its own region.
    2.  Derive CacheRegion from generic `CancellableCacheRegion<T>` class:
    ```csharp
    public class CustomerReviewCacheRegion : CancellableCacheRegion<CustomerReviewCacheRegion>
    {
    }
    ```

3. **Services** folder
    1. All services: remove inheritance from **ServiceBase**
    2. Ensure that the signatures of the methods matches the ones defined in the corresponding interfaces
    3. Change response to `Task<CustomerReviewSearchResult>` in **_CustomerReviewSearchService_** service
    4. Refactor all methods to **be asynchronous**
    5. Add working with cache to all methods
    > check this example for more details [VirtoCommerce.CustomerModule.Data.Services](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Services).

4. **Migrations** folder
    1. Create **_InitialCustomerReviews_** migration
        1. Open Configuration.cs and copy the namespace to your notes. (Will need it in the next section.)
        1. Delete everything (all migrations and Configuration.cs) from **_Migrations_** folder
        1. Execute "**Unload Project**" on CustomerReviews.**Web** project in Solution Explorer (or the solution would fail to build in this step due to the errors in this project).
        1. Execute "**Set as Startup Project**" on CustomerReviews.**Data** project in Solution Explorer
        2. Open NuGet **Package Manager Console**
        3. Select "src\CustomerReviews.**Data**" as "**Default project**"
        4. Run command:
        ```console
        Add-Migration InitialCustomerReviews -Verbose
        ```
        5. In case of any existing module's extension is developed, study and follow the steps from [How to extend the DB model of VC module](../../techniques/extend-DB-model.md) guide.

    2. Create Migration for backward compatibility with v2.x
        1. Add new migration with name **_UpdateCustomerReviewsV2_** and rename the migration **_filename_** to **_20000000000000_UpdateCustomerReviewsV2_**. Mind the name format: _"20000000000000_Update{ModuleName}V2"_.
        2. Add SQL command to the migration:
        ```csharp
        migrationBuilder.Sql(@"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__MigrationHistory'))
                IF (EXISTS (SELECT * FROM __MigrationHistory WHERE ContextKey = 'CustomerReviews.Data.Migrations.Configuration'))
                    BEGIN
                        INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES ('20191129134041_InitialCustomerReviews', '2.2.3-servicing-35854')
                    END");
        ```
          * the `ContextKey` value is the V2 migration Configuration name, including namespace. Retrieve the namespace from your notes, as you put it there in the previous section. Typically, the value is "{ModuleId}.Data.Migrations.Configuration".
          * the value for `MigrationId` has to be the name of your new migration, added in previous step. ('20191129134041_InitialCustomerReviews' in our case). Check [20000000000000_UpdateCoreV2.cs migration](https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.cs#L12) as another example.
          * value for `ProductVersion` should be taken from **_20000000000000_UpdateCustomerReviewsV2.Designer_** line 19:
        ```csharp
        .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
        ```
        3. Open **_20000000000000_UpdateCustomerReviewsV2.Designer_** and change **_Migration_** attribute parameter value to the current migration ID ("20000000000000_UpdateCustomerReviewsV2" in this case). Check [20000000000000_UpdateCoreV2.Designer.cs](https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.Designer.cs#L12) as another example.

5. If Dynamic Properties are used in the module, follow the steps in [Update Dynamic Property](dynamic-property.md).
5. If multiple databases are used by the solution, follow the steps in [Prepare distributed databases for VC v3](prepare-distibuted-databases-for-v3.md).

## 5. Make changes in CustomerReviews&#46;Web project
1. Execute "**Reload Project**" on CustomerReviews.**Web** project in Solution Explorer (as it was unloaded earlier).
1. Changes in **_module.manifest_**
    1. Versioning - increase major module version and add prerelease tag (empty value for a release version):
    ```xml
    <version>3.0.0</version>
    <version-tag></version-tag>
    ```
    2. Required minimal version of VC Platform:
    ```xml
    <platformVersion>3.0.0</platformVersion>
    ```
    3. Module dependencies - change to actual versions:
    ```xml
    <dependencies>
        <dependency id="VirtoCommerce.Core" version="3.1.0" />
    </dependencies>
    ```
    4. Remove **styles**, **scripts** sections from the manifest file.
2. Add localizations for permissions/settings to **_Localizations/en.CustomerReviews.json_** file [as this](https://github.com/VirtoCommerce/vc-samples/blob/release/3.0.0/CustomerReviews/src/CustomerReviews.Web/Localizations/en.customerReviews.json#L18-L30).
   Sample resulting keys: `"permissions.customerReview:read"`, `"settings.CustomerReviews.CustomerReviewsEnabled.title"`. 
2. Remove **permissions**, **settings** definitions sections from the manifest file.

2. Changes in **_Module.cs_**

    1. Change the class inheritance to interface **_IModule_**, then add implementation methods: **_Initialize, PostInitialize, Uninstall_** and property **_ModuleInfo_**. Check [VirtoCommerce.CustomerModule.Web/Module.cs](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Web/Module.cs) for another implementation example.
    2. Read the [Dependency injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.0) article for additional info.
    3. Register all the needed classes for dependency injection inside **_Initialize_** method, like **_CustomerReviewDbContext, CustomerReviewRepository_**, etc.
    4. Register settings using interface **_ISettingsRegistrar_** in PostInitialize method:
    ```csharp
    var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
    settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
    ```
    5. Register permissions using interface **_IPermissionsRegistrar_** in PostInitialize method: 
    ```csharp
    var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
    permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x => new Permission() { GroupName = "CustomerReview", Name = x }).ToArray());
    ```
    6. Add this code into PostInitialize method, needed to ensure that the migrations would be applied: 
    ```csharp
    using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<CustomerReviewsDbContext>();
        dbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationNameByOwnerName(ModuleInfo.Id, <<your company prefix in moduleId>>));
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
    }
    ```

    !!! note
        The **MigrateIfNotApplied** extension method is needed for the database backward compatibility with version 2.x. This extension enables to skip generating the initial migration, as there are changes (tables, indexes) in the database already.

3. Changes to all API Controllers in **Controllers/Api** folder:
    1. Refactor controllers to derive from _Microsoft.AspNetCore.Mvc.**Controller**_.
    2. Change _RoutePrefix_ attribute to **_Route_** for all endpoints
    3. Remove _ResponseType_ attribute from all endpoints
    4. Change _CheckPermission_ attribute to **_Authorize_** for all endpoints
      > If the endpoint should have a restricted access, an **_Authorize_** attribute with the required permission should be added. Use the **_ModuleConstants_** class, which was previously defined in **_CustomerReviews.Core_** project.

    5. Review the exposed endpoints and refactor to **be asynchronous** (return `async Task<>`), if needed
    6. Mark each complex type parameter with `[FromBody]` attribute for all endpoints. The attribute for Delete endpoint should be `[FromQuery]`.
    E.g., SearchCustomerReviews method converted to ASP&#46;NET Core MVC:
    ```csharp
    [HttpPost]
    [Route("search")]
    [Authorize(ModuleConstants.Security.Permissions.Read)]
    public async Task<ActionResult<CustomerReviewSearchResult>> SearchCustomerReviews([FromBody]CustomerReviewSearchCriteria criteria)
    ```
    Read [Controller action return types in ASP.NET Core web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1) for more info.

4. If there are any JavaScript or stylesheet files in the project:
    1. Copy **package.json** from [sample package.json](https://raw.githubusercontent.com/VirtoCommerce/vc-module-order/release/3.0.0/src/VirtoCommerce.OrdersModule.Web/package.json);
    2. Copy **webpack.config.js** from [sample webpack.config.js](https://raw.githubusercontent.com/VirtoCommerce/vc-module-order/release/3.0.0/src/VirtoCommerce.OrdersModule.Web/webpack.config.js);
    3. Change the namespace on line 36 in webpack.config.js to be equal to module's identifier:
    ```js
    namespace: 'CustomerReviews'
    ```
    4. Open Command prompt and navigate to CustomerReviews&#46;Web folder. Run:
        ```
        npm install
        ```

        !!! note
            fix any css errors, if the previous command would fail. For CustomerReviews sample also change line 12 in webpack.config.js to:
            ```js
            ...glob.sync('./Content/css/*.css', { nosort: true })
            ```

    5. Add `dist/` line to `.gitignore` file;
    6. Add `node_modules/` line to `.gitignore` file;
    6. Install [WebPack Task Runner](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.WebPackTaskRunner) extension to Visual Studio (restart required);
    6. Build the scripts:
        1. Locate and right-click on **webpack.config.js** in Solution Explorer
        1. Execute "Task Runner Explorer"
        1. Double-click "Run - Development" in "Task Runner Explorer"

        !!! note
            The resulting file(s) (*app.js*, *style.css*) were generated to ***.Web/dist** folder.

## 6. Make changes in CustomerReviews.Tests project

1. Reference the required NuGet packages by adding this `ItemGroup` to project file:
```xml
    <ItemGroup>
        <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.4.0" />
        <PackageReference Include="Moq" Version="4.13.1" />
        <PackageReference Include="MSTest.TestAdapter" Version="2.0.0" />
        <PackageReference Include="MSTest.TestFramework" Version="2.0.0" />
        <PackageReference Include="xunit" Version="2.4.1" />
        <PackageReference Include="xunit.runner.console" Version="2.4.1">
            <PrivateAssets>all</PrivateAssets>
            <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
        </PackageReference>
        <PackageReference Include="xunit.runner.visualstudio" Version="2.4.1">
            <PrivateAssets>all</PrivateAssets>
            <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
        </PackageReference>
        <DotNetCliToolReference Include="dotnet-xunit" Version="2.3.1" />
    </ItemGroup>
```
2. Add **_TestAsyncQueryProvider_** class under Common folder. Paste the class implementation [from here](https://raw.githubusercontent.com/VirtoCommerce/vc-samples/release/3.0.0/CustomerReviews/tests/CustomerReviews.Test/Common/TestAsyncQueryProvider.cs).
3. Remove inheritance from **_FunctionalTestBase_**    for each tests class.
4. Add the integration tests under **IntegrationTests** folder. Ensure, that each integration tests class is marked with this **Trait** attribute:
```csharp
[Trait("Category", "IntegrationTest")]
```

## 7. Create module package

1. Please, read the article about [VirtoCommerce.GlobalTool](https://github.com/VirtoCommerce/vc-platform/blob/release/3.0.0/build/README.md).
1. Add **.nuke** file to be able to use VirtoCommerce.GlobalTool. It should contain the solution filename, e.g., [.nuke in vc-module-customer](https://github.com/VirtoCommerce/vc-module-customer/blob/release/3.0.0/.nuke)
1. Add **Directory.Build.props** file to be able to configure package release versions. Check [Directory.Build.props in vc-module-customer](https://github.com/VirtoCommerce/vc-module-customer/blob/release/3.0.0/Directory.Build.Props) for details.
