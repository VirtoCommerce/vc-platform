# Migrate a module from the Platfrom 2.0 to 3.0 version.

### Introduction
This article describes how to migrate an existing [CustomerReviews sample](https://github.com/VirtoCommerce/vc-samples/tree/master/CustomerReviews) module from VC Platform version 2.x to 3.0.

> NOTE: A sample module source code can be found here: https://github.com/VirtoCommerce/vc-samples/tree/release/3.0.0/CustomerReviews.


## 1. Make correct structure in solution and projects
1. Ensure that installed latest version v.2 of the platform and all modules. Need to update  for latest version v.2
2. If it exists, delete **_packages_** folder 
2. Delete **Properties** folder from each of the projects' folder 
3. Convert the projects from ASP&#46;NET to ASP&#46;NET Core:
   1. Open each of the projects' **_*.csproj_** files in text editor (e.g., Notepad), clear whole file content and set it to this:
		```xml
		<Project Sdk="Microsoft.NET.Sdk">
			<PropertyGroup>
				<TargetFramework>netcoreapp3.1</TargetFramework>
			</PropertyGroup>
		</Project>
		```
   1. Set current support Target Framework (at this time is netcoreapp3.1)    
   1. Read [this article](https://docs.microsoft.com/en-us/aspnet/core/migration/30-to-31) for more info.
3. Create **_src_** and **_tests_** subfolders in module's root folder (Windows Explorer)
3. Move **_CustomerReviews.Core_**, **_CustomerReviews.Data_**, **_CustomerReviews&#46;Web_** projects to **_src_**
3. Move **_CustomerReviews.Test_** project to **_tests_**
2. Open **_CustomerReviews.sln_** solution in Visual Studio
2. Remove all projects from the solution
3. Add **_src_** and **_tests_** Solution Folders
4. Add the existing **_CustomerReviews.Core_**, **_CustomerReviews.Data_**, **_CustomerReviews&#46;Web_** projects to **_src_** folder
4. Add the existing **_CustomerReviews.Test_** project to **_tests_** folder
5. Remove all files related to .NET Framework 4.x in **every project**:
	* **App.config**
	* **packages.config**
	* **Web.config**, **Web.Debug.config**, **Web.Release.config**, **module.ignore**
5. Add references to projects:
   1. **CustomerReviews.Data**: add reference to CustomerReviews.Core project
   1. **CustomerReviews&#46;Web**: add references to CustomerReviews.Core, CustomerReviews.Data projects
   1. **CustomerReviews.Tests**: add references to CustomerReviews.Core, CustomerReviews.Data, CustomerReviews&#46;Web projects
5. References to NuGet packages:
   1. **CustomerReviews.Core**: add reference to the latest version **_VirtoCommerce.Platform.Core_** package.
   1. **CustomerReviews.Data**: add reference to the latest version **_VirtoCommerce.Platform.Data_** package.
5. References to NuGet packages in **_CustomerReviews&#46;Web_**:
   1. (Double click in Visual Studio to) open _CustomerReviews.Web.csproj_ file for editing;
   1. Add new ItemGroup:
   ```
   <ItemGroup>
     <PackageReference Include="Microsoft.AspNetCore.App" />
   </ItemGroup>
   ```
5. Add other NuGet dependency packages, if any exists in **_module.manifest_**.

## 2. Make changes in CustomerReviews.Core project
1. If missing, add class **_ModuleConstants.cs_** for module constants:
	1. Inside **_ModuleConstants_** add sub-classes **_Security_** and **_Permissions_**
	2. Add sub-classes **_Settings_** and **_General_** containing settings' definitions of type **_SettingDescriptor_**. Move settings definitions from module.manifest to this class
		> Follow the structure as defined in [ModuleConstants.cs in CustomerModule](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Core/ModuleConstants.cs).
	3. Other constants.

2. Update **ICustomerReviewService.cs**:
	* Refactor all methods to **be asynchronous**: return `Task<>`
	* Rename all methods to have suffix `Async`

2. If there is a search service defined:
	1. Move **_CustomerReviewSearchCriteria_** class to **Search** sub-folder;
	1. Ensure, that CustomerReviewSearchCriteria inherits from `SearchCriteriaBase`; 
	1. Create **_CustomerReviewSearchResult_** class in **Search** sub-folder;
	1. Ensure, that CustomerReviewSearchResult inherits from `GenericSearchResult<CustomerReview>`.
	1. Refactor **_ICustomerReviewSearchService_** to use CustomerReviewSearchResult, all methods be asynchronous, and end with **Async**: 
	```cs
	public interface ICustomerReviewSearchService
    {
        Task<CustomerReviewSearchResult> SearchCustomerReviewsAsync(CustomerReviewSearchCriteria criteria);
    }
	```

3. If any model-related changing/changed events were defined in **Events** folder, ensure that each of them derive from the base `GenericChangedEntryEvent` class.

4. If any custom Notifications were added to **Notifications** folder:
	1. Add reference to NuGet **VirtoCommerce.NotificationsModule.Core** package;
	1. Ensure that each defined Notification class inherits from **_EmailNotification_**/**_SmsNotification_** or own class, based on **_Notification_**.

## 3. Make changes in CustomerReviews.Data project
1. **Repositories** folder
	1. Create **CustomerReviewsDbContext.cs**
		* Add new class **_CustomerReviewsDbContext_**
		* Make it public and derive from `DbContextWithTriggers`
		* Add 2 constructors, using [CustomerDbContext](https://github.com/VirtoCommerce/vc-module-customer/blob/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Repositories/CustomerDbContext.cs) as an example.
		* Override _OnModelCreating_ method, add **CustomerReviewEntity** mapping to modelBuilder and set max length to Id:
			```cs
			modelBuilder.Entity<CustomerReviewEntity>().ToTable("CustomerReview").HasKey(x => x.Id);
			modelBuilder.Entity<CustomerReviewEntity>().Property(x => x.Id).HasMaxLength(128);
			```
	2. Create **DesignTimeDbContextFactory.cs**
		> NOTE: a factory for creating derived Microsoft.EntityFrameworkCore.DbContext instances.
		* Add new class **_DesignTimeDbContextFactory_** 
		* Make it public and derive from `IDesignTimeDbContextFactory<CustomerReviewsDbContext>`
		* Implement **_CreateDbContext_** method, using [CustomerModule.Data/Repositories/DesignTimeDbContextFactory.cs](https://github.com/VirtoCommerce/vc-module-customer/blob/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Repositories/DesignTimeDbContextFactory.cs) as an example
		* Ensure that connection string to your development SQL Server in **_UseSqlServer_** method is correct. It would be used while generating code-first migrations. 
	3. Update **ICustomerReviewsRepository.cs**
		* If the module is an extension then derive from derived module's interface repository.
		* Refactor all methods to **be asynchronous**: return `Task<>`
		* Rename all methods to have suffix `Async`
	3. Update **CustomerReviewsRepository.cs**
		* Refactor **_CustomerReviewsRepository_** class to derive from `DbContextRepositoryBase<CustomerReviewsDbContext>` or if the module is an extension then derive from derived module's repository.
		* Refactor the constructors to leave only one, taking the only  **_CustomerReviewsDbContext_** parameter:
		```cs
		public CustomerReviewRepository(CustomerReviewsDbContext dbContext) : base(dbContext)
        {
        }
		```
		* Refactor **_CustomerReviews_** property to access data using DbSet like this:
		```cs
		public IQueryable<CustomerReviewEntity> CustomerReviews => DbContext.Set<CustomerReviewEntity>();
		```
		* Remove **_OnModelCreating_** method

2. **Caching** folder
	1. If missing, create **Caching** folder. This folder is for the cache region classes. Typically, each model should have its own region.
	2.  Derive CacheRegion from generic `CancellableCacheRegion<T>` class:
	```cs
	public class CustomerReviewCacheRegion : CancellableCacheRegion<CustomerReviewCacheRegion>
    {
    }
	```

2. **Services** folder
	* All services: remove inheritance from **ServiceBase**
	* Ensure that the signatures of the methods matches the ones defined in the corresponding interfaces
	* Change response to `Task<CustomerReviewSearchResult>` in **_CustomerReviewSearchService_** service
	* Refactor all methods to **be asynchronous**
	* Add working with cache to all methods
	> check this example for more details [VirtoCommerce.CustomerModule.Data.Services](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Services).

2. **_Migrations_** folder
	1. Create **_InitialCustomerReviews_** migration
		* Delete everything (all migrations and Configuration.cs) from **_Migrations_** folder
		* Open **Package Manager Console**;
		* Select "src\CustomerReviews.**Data**" as "**Default project**";
		* Run command:
			```
			Add-Migration InitialCustomerReviews -Context CustomerReviews.Data.Repositories.CustomerReviewsDbContext -StartupProject CustomerReviews.Data  -Verbose -OutputDir Migrations
			```
		* if there are extensions then need to remove the lines which depends extented entities(like Tables, FK, PK, Index) and then add the Discriminator column like [that](https://github.com/VirtoCommerce/vc-module-order/blob/ce72193f54ad0626c5c3d85b4682c9ee9ba812b1/samples/VirtoCommerce.OrdersModule2.Web/Migrations/20180724064542_InitialOrders2.cs#L11). Please read the [article about inheritance](https://docs.microsoft.com/en-us/ef/core/modeling/relational/inheritance).

	2. Create Migration for backward compatibility with v2.x
		* Add new migration with name **_UpdateCustomerReviewsV2_** and rename the migration **_filename_** to **_20000000000000_UpdateCustomerReviewsV2_**
		* Add SQL command to the migration:
		```cs
		migrationBuilder.Sql(@"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '__MigrationHistory'))
                IF (EXISTS (SELECT * FROM __MigrationHistory WHERE ContextKey = 'CustomerReviews.Data.Migrations.Configuration'))
                    BEGIN
	                    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId],[ProductVersion]) VALUES ('20191129134041_InitialCustomerReviews', '2.2.3-servicing-35854')
                    END");
		```
		> Note: the `ContextKey` value has to be constructed as "{ModuleId}.Data.Migrations.Configuration".
		> Note2: the value for `MigrationId` has to be the name of your new migration, added in previous step. ('20191129134041_InitialCustomerReviews' in our case). Check [20000000000000_UpdateCoreV2.cs migration](https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.cs#L12) as another example.

		* Open **_20000000000000_UpdateCustomerReviewsV2.Designer_** and change **_Migration_** attribute parameter value to the current migration ID ("20000000000000_UpdateCustomerReviewsV2" in this case). Check [20000000000000_UpdateCoreV2.Designer.cs](https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.Designer.cs#L12) as another example.

## 4. Make changes in CustomerReviews&#46;Web project
1. Changes in **_module.manifest_**
	* Versioning - increase module version and add prerelease tag (empty value for a release version):
    ```xml
    <version>3.0.0</version>
    <version-tag></version-tag>
    ```
   * Required minimal version of VC Platform:
    ```xml
    <platformVersion>3.0.0</platformVersion>
    ```
   * Module dependencies - change to actual versions:
    ```xml
    <dependencies>
        <dependency id="VirtoCommerce.Core" version="3.0.0" />
    </dependencies>
    ```
   * Remove **styles**/**scripts** sections from the manifest file
   * Move all localization of permissions/settings to **_Localizations/en.CustomerReviews.json_** file [as this](https://github.com/VirtoCommerce/vc-samples/blob/release/3.0.0/CustomerReviews/src/CustomerReviews.Web/Localizations/en.customerReviews.json#L18-L30)
   * Remove **permissions**/**settings** definitions sections from the manifest file

2. Changes in **_Module.cs_**
	* Change the class inheritance to interface **_IModule_**, then add implementation methods: **_Initialize, PostInitialize, Uninstall_** and property **_ModuleInfo_**. Check [VirtoCommerce.CustomerModule.Web/Module.cs](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Web/Module.cs) for another implementation example.
	* Read the [Dependency injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.0) article for additional info.
	* Register all the needed classes for dependency injection inside **_Initialize_** method, like **_CustomerReviewDbContext, CustomerReviewRepository_**, etc.
	* Register settings using interface **_ISettingsRegistrar_** in PostInitialize method:
	```cs
	var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
	settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
	```
	* Register permissions using interface **_IPermissionsRegistrar_** in PostInitialize method: 
	```cs
	var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
	permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x => new Permission() { GroupName = "CustomerReview", Name = x }).ToArray());
	```
	* Add this code into PostInitialize method, needed to ensure that the migrations would be applied: 
	```cs
	using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
	{
		var dbContext = serviceScope.ServiceProvider.GetRequiredService<CustomerReviewsDbContext>();
		dbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName(ModuleInfo.Id));
		dbContext.Database.EnsureCreated();
		dbContext.Database.Migrate();
	}
	```
	> NOTE: The **MigrateIfNotApplied** extension method is needed for the database backward compatibility with version 2.x. This extension enables to skip generating the initial migration, as there are changes (tables, indexes) in the database already.

3. Changes to all API Controller(s) in **Controllers/Api** folder:
	* Refactor controllers to derive from _Microsoft.AspNetCore.Mvc.**Controller**_.
	* Change _RoutePrefix_ attribute to **_Route_** for all endpoints
	* Remove _ResponseType_ attribute from all endpoints
	* Change _CheckPermission_ attribute to **_Authorize_** for all endpoints
    > If the endpoint should have a restricted access, an **_Authorize_** attribute with the required permission should be added. Use the **_ModuleConstants_** class, which was previously defined in **_CustomerReviews.Core_** project.
	* Refactor all endpoints to **be asynchronous** (return `async Task<>`)
	* Mark each complex type parameter with `[FromBody]` attribute for all endpoints. The attribute for Delete endpoint should be `[FromQuery]`.

	> E.g., SearchCustomerReviews method converted to ASP&#46;NET Core MVC:
    ```cs
	[HttpPost]
	[Route("search")]
	[Authorize(ModuleConstants.Security.Permissions.Read)]
	public async Task<ActionResult<CustomerReviewSearchResult>> SearchCustomerReviews([FromBody]CustomerReviewSearchCriteria criteria)
    ```

4. If there are any JavaScript or stylesheet files in the project:
    1. Copy **package.json** from [sample package.json](https://raw.githubusercontent.com/VirtoCommerce/vc-module-order/release/3.0.0/src/VirtoCommerce.OrdersModule.Web/package.json);
    1. Copy **webpack.config.js** from [sample webpack.config.js](https://raw.githubusercontent.com/VirtoCommerce/vc-module-order/release/3.0.0/src/VirtoCommerce.OrdersModule.Web/webpack.config.js);
    2. Change the namespace on line 36 in webpack.config.js to be equal to module's identifier:
    ```js
    namespace: 'CustomerReviews'
    ```
    4. Open Command prompt and navigate to CustomerReviews&#46;Web folder:
        1. Run `npm install`
        1. Run `npm run webpack:dev`
		> Note: fix any css errors, of the previous command would fail. For CustomerReviews sample also change line 12 in webpack.config.js to:
		```js
		...glob.sync('./Content/css/*.css', { nosort: true })
		```
	5. Add `dist/` line to `.gitignore` file
	5. Add `node_modules/` line to `.gitignore` file.

## 5. Make changes in CustomerReviews.Tests project
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
2. Remove inheritance from **_FunctionalTestBase_**	for each tests class.

2. Add the integration tests under **IntegrationTests** folder. Ensure, that each integration tests class is marked with this **Trait** attribute:
    ```cs
    [Trait("Category", "IntegrationTest")]		

## 6. Create module package
1. please read article about `vc-build` [link](https://github.com/VirtoCommerce/vc-platform/blob/release/3.0.0/build/README.md)
