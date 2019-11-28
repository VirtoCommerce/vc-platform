### Introduction
This article describes how to migrate a existing module from 2.0 to 3.0 version.

> NOTE: A sample module source code can be found here: https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/samples/VirtoCommerce.CustomerSampleModule.Web

## 1. Make correct structure in solution and projects
1. For example, have the module [CustomerReviews](https://github.com/VirtoCommerce/vc-samples/tree/master/CustomerReviews).
2. Create a new branch with  name: **_release/3.0.0_** then switch to the branch.
3. Add **_src_** and **_tests_** Solution Folders (using Visual Studio)
4. **_CustomerReviews.Core_**, **_CustomerReviews.Data_**, **_CustomerReviews.Web_** projects move to **_src_** 
5. **_CustomerReviews.Test_** project move to **_tests_**
6. Then the projects add to solution **_CustomerReviews.sln_** 
7. Then need to migrate the projects from ASP.NET to ASP.NET Core. Read this [article](https://docs.microsoft.com/en-us/aspnet/core/migration/proper-to-2x).
	1. Remove all files depending with .net framework 4.x (properties, packages, configs)
	2. Need to make changes in **_*.csproj_** files
		* Clear whole the file
		* Change xml signature inside csproj to
		```xml
		<Project Sdk="Microsoft.NET.Sdk"> </Project>
		```
		* Then add **_TargetFramework_** as **_PropertyGroup_** inside Project
		```xml
		<PropertyGroup>
			<TargetFramework>netcoreapp2.2</TargetFramework>
		<PropertyGroup>
		```
		* Add project and package references packages(if you have) and dependencies from **_module.manifest_** as **_ItemGroup_**, looks like this:
		```xml
		<ItemGroup>
			<PackageReference Include="VirtoCommerce.Platform.Core" Version="3.0.0-rc0001" />
			...
		</ItemGroup>
		<ItemGroup>
			<ProjectReference Include="..\CustomerReviews.Core\CustomerReviews.Core.csproj" />
			...
		</ItemGroup>
		``` 

## 2. Make changes in CustomerReviews.Core project
1. If don't have class **_ModuleConstants.cs_** Add class **_ModuleConstants.cs_** for module constants
	1. Inside **_ModuleConstants_** add sub-classes **_Security_** and **_Permissions_**
	2. Add sub-classes **_Settings_** and **_General_** containing settings' definitions of type **_SettingDescriptor_**
		> look at [example](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Core/ModuleConstants.cs) in **_CustomerModule_**
	3. Other constants.	
2. If there is search service, then need to move **_SearchCriteria_** and **_SearchResult_** classes to **Search** sub-folder. And inherite from **_SearchCriteriaBase_** and **_GenericSearchResult<CustomerReview>_** accordingly.
3. **Events** folder: add model-related changing/changed events. Derive from the base `GenericChangedEntryEvent` class.

4. **Notifications** folder: define new types of **_Notifications_**, that your module would expose. Each class should inherit from **_EmailNotification_**/**_SmsNotification_** or own class, based on **_Notification_**.

## 3. Make changes in CustomerReviews.Data project
1. **Repositories** folder: 
	1. Create **_CustomerReviewsDbContext.cs_**
		* Need to add **_CustomerReviewsDbContext_** inheritance from **_DbContextWithTriggers_**
		* Add overriding _OnModelCreating_ method
		* Then need to add mapping to modelBuilder
			```cs
			modelBuilder.Entity<CustomerReviewEntity>().ToTable("CustomerReview").HasKey(x => x.Id);
			```
		* Then set max length to Id
			```cs
			modelBuilder.Entity<CustomerReviewEntity>().Property(x => x.Id).HasMaxLength(128);
			```
		> look at [CustomerDbContext](https://github.com/VirtoCommerce/vc-module-customer/blob/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Repositories/CustomerDbContext.cs)

	2. Create **_DesignTimeDbContextFactory.cs_**
		> NOTE: A factory for creating derived Microsoft.EntityFrameworkCore.DbContext instances.
		* Create a class **_DesignTimeDbContextFactory.cs_** and inherite from **_IDesignTimeDbContextFactory_**
		* Need to implement **_CreateDbContext_** method with **_CustomerReviewsDbContext_**
		* Change connection string for database in **_UseSqlServer_** method. It need for migration.
		> look at [example](https://github.com/VirtoCommerce/vc-module-customer/blob/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Repositories/DesignTimeDbContextFactory.cs)
	3. Change CustomerReviewsRepository
		* The class have to derive from **_DbContextRepositoryBase_** and have constructor with the new **_CustomerReviewsDbContext_** dependency
		* Add DbSet **_CustomerReviews_** to access data
		```cs
		public IQueryable<CustomerReviewEntity> CustomerReviews => DbContext.Set<CustomerReviewEntity>();
		```
		* Remove **_OnModelCreating_** method
		* Add asynchronously to all methods

2. **_Migrations_** folder:
	1. Create **_Migration_**
		* Remove old migrations in the folder **_Migrations_** which generated for v. 2.0
		* Open **Package Manager Console**;
		* Select "src\CustomerReviews.**Data**" as "**Default project**";
		* Run command:
			```
			Add-Migration InitialCustomerReviews -Context CustomerReviews.Data.Repositories.CustomerReviewsDbContext -StartupProject CustomerReviews.Data  -Verbose -OutputDir Migrations
			```
	2. Create Migration for backward compatibility v.2.0
		* Need to create migration with name **_UpdateCustomerReviewsV2_** and rename the migration file name to **_20000000000000_UpdateCustomerReviewsV2_** , look at [link](https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.cs)
		* Add SQL Insert command to **_20000000000000_UpdateCustomerReviewsV2_** , look at [line](https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.cs#L13) 

		and rename migration name to **_InitialCustomerReviews_** in sql-script. 
		* Also need to rename **_UpdateCustomerReviewsV2.Designer_** to **_20000000000000_ UpdateCustomerReviewsV2.Designer_** and rename **_MigrationAttribute_** , look at [example](https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.Designer.cs#L12)

3. **Caching** folder: add it, if data caching should be used. This folder is for the cache region classes. Typically, each model should have its own region. Derive CacheRegion from generic `CancellableCacheRegion<T>` class e.g., `public class CustomerReviewsCacheRegion : CancellableCacheRegion<CustomerReviewsCacheRegion>`.

4. **Services** folder:
	* Make separate services for Search and CRUD methods
	* Remove inheritance from ServiceBase
	* Change responses to CustomerReviewSearchResult in the Search Service
	* Check that there are methods like GetByIds, SaveChanges, Delete
	* Add asynchronously to all methods
	* Add working with cache to all methods
	> look at [VirtoCommerce.CustomerModule.Data.Services](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Data/Services)

6. **ExportImport** folder: add class for data export/import. It should be called from **_Module.cs_** and contain implementation for module data export and import.
7. **Handlers** folder: add handlers for the _domain events_, which were defined under **.Core/Events**.

## 4. Make changes in CustomerReviews.Web project

1. Make changes in **_modules.manifest_**.
	* Versioning - prerelease version tag of a module:
    ```xml
    <version-tag>v1</version-tag>
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
   * Move all localization of permossions/settings to **_Localizaton_** file, look at [example](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Web/Localizations/en.VirtoCommerce.Customer.json)
   * Remove groups permissions/settings

2. Change signature in **_Module.cs_**
	* Need to change inheritance to interface **_IModule_** , then add implementation methods: **_Initialize, PostInitialize, Uninstall_** and property **_ModuleInfo_**
	* Add all dependency injections to **_Initialize_** method, like as **_CustomerReviewDbContext, CustomerReviewRepository_**
	> look at [sample](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Web/Module.cs)
	* Register settings using interface **_ISettingsRegistrar_** in PostInitialize method
	```cs
	var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
	settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
	```
	* Register permissions using interface **_IPermissionsRegistrar_** in PostInitialize method 
	```cs
	var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
	permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x => new Permission() { GroupName = "CustomerReview", Name = x }).ToArray());
	```
	* Add three methods **_MigrateIfNotApplied, EnsureCreated, Migrate_** into PostInitialize method, need for insure creating migration 
	```cs
	using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
	{
		var dbContext = serviceScope.ServiceProvider.GetRequiredService<CustomerReviewsDbContext>();
		dbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName(ModuleInfo.Id));
		dbContext.Database.EnsureCreated();
		dbContext.Database.Migrate();
	}
	```
	> NOTE: Extension MigrateIfNotApplied need for backward compatibility v.2.0. The extension allows don't generate initial migration, because there are already tables in the DataBase.
	Look at [example](https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Web/Module.cs#L70).

3. Authorization Requirement
	If need to use authorization requirement then 
	* create a class **_CustomerReviewAuthorizationRequirement_** in **_CustomerReviews.Data.Authorization_** and derived from **_PermissionAuthorizationRequirement_**, look at [example](https://github.com/VirtoCommerce/vc-module-store/blob/release/3.0.0/src/VirtoCommerce.StoreModule.Data/Authorization/StoreAuthorizationRequirement.cs)
	* Add **_IAuthorizationService_** to **_CustomerReviewModuleController_** 
	* Call method **_AuthorizeAsync_** in an action where to need, look at [example](https://github.com/VirtoCommerce/vc-module-store/blob/arelease/3.0.0/src/VirtoCommerce.StoreModule.Web/Controllers/Api/StoreModuleController.cs#L51)
	* Then add condition for result 
	```cs
	if (!authorizationResult.Succeeded)
	{
		return Unauthorized();
	}
	```

4. If there are any JavaScript or stylesheet files in the project:
    1. Copy **package.json** from [sample package.json](https://github.com/VirtoCommerce/vc-module-order/blob/release/3.0.0/src/VirtoCommerce.OrdersModule.Web/package.json);
    1. Copy **webpack.config.js** from [sample webpack.config.js](https://github.com/VirtoCommerce/vc-module-order/blob/release/3.0.0/src/VirtoCommerce.OrdersModule.Web/webpack.config.js);
    2. Change the namespace on line 36 in webpack.config.js to be equal to module's identifier:
    ```js
    namespace: 'CustomerReviews'
    ```
    4. Open command prompt and navigate to DummyModule&#46;Web folder:
        1. Run `npm install`
        1. Run `npm run webpack:dev`
	5. Then need to add `dist/` line to `.gitignore` file.

## 5. Fill CustomerReviews.Tests project
1. **UnitTests** folder: add unit tests here.
2. **IntegrationTests** folder: add integration tests here. Ensure, that each integration tests class is marked with **Trait** attribute:
    ```cs
    [Trait("Category", "IntegrationTest")]		

## 6. Create module package
1. Open command prompt
1. If _VirtoCommerce.GlobalTool_ isn't installed, run:
    ```
    dotnet tool install VirtoCommerce.GlobalTool -g --version 3.0.0-beta0006
    ```
   Reopen the command prompt after installing.
1. Navigate to the module's root folder (**/CustomerReviews**) in the command prompt
2. Create **.nuke** file and set your module's solution filename as its content: `CustomerReviews.sln`
5. Run `vc-build compress`
5. In order to install the module to VC Platform, navigate to **artifacts** folder and take _VirtoCommerce.Dummy_1.0.0-v1.zip_ package file.
