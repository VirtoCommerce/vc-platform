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
		* Then add _TargetFramework_ as _PropertyGroup_ inside _Project_ 
		```xml
		<PropertyGroup>
			<TargetFramework>netcoreapp2.2</TargetFramework>
		<PropertyGroup>
		```
		* Add project and package references packages(if you have) and dependencies from _module.manifest_ as _ItemGroup_
		```xml
		<ItemGroup>
			<ProjectReference Include="..\CustomerReviews.Core\CustomerReviews.Core.csproj" />
			<PackageReference Include="VirtoCommerce.Platform.Core" Version="3.0.0-rc0001" />
		</ItemGroup>
		``` 

## 2. Make changes in CustomerReviews.Core project
1. If don't have class **_ModuleConstants.cs_** Add class **_ModuleConstants.cs_** for module constants
	1. Inside **_ModuleConstants_** add sub-classes **_Security_** and **_Permissions_**
	2. Add sub-classes **_Settings_** and **_General_** containing settings' definitions of type **_SettingDescriptor_**
		> look at [example](https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Core/ModuleConstants.cs) in **_CustomerModule_**
	3. Other constants.	
2. If there is search service, then need to move **_SearchCriteria_** and **_SearchResult_** classes to **Search** sub-folder. And inherite from **_SearchCriteriaBase_** and **_GenericSearchResult<CustomerReview>_** accordingly.

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
		* Add asynchrony to all methods

2. **_Migrations_** folder:
	1. Create **_Migration_**
		* Remove old migrations in the folder **_Migrations_** which generated for v. 2.0
		* Open **Package Manager Console**;
		* Select "src\CustomerReviews.**Data**" as "**Default project**";
		* Run command from **_Package Manager Console_**
			```
			Add-Migration InitialCustomerReviews -Context CustomerReviews.Data.Repositories.Cart2DbContext -StartupProject CustomerReviews.Data  -Verbose -OutputDir Migrations
			```
	2. Create Migration for backward compatibility v.2.0
		* Need to create migration with name **_UpdateCustomerReviewsV2_** and rename the migration file name to **_20000000000000_UpdateCustomerReviewsV2_** 
		> look at link https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.cs
		* Add SQL Insert command to **_20000000000000_UpdateCustomerReviewsV2_** 
		> look at https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.cs#L13 

		and rename migration name to **_InitialCustomerReviews_** in sql-script. 
		* Also need to rename **_UpdateCustomerReviewsV2.Designer_** to **_20000000000000_ UpdateCustomerReviewsV2.Designer_** and rename **_MigrationAttribute_** 
		> look at example https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.Designer.cs#L12

2. **Caching** folder: add it, if data caching should be used. This folder is for the cache region classes. Typically, each model should have its own region. Derive CacheRegion from generic `CancellableCacheRegion<T>` class e.g., `public class CustomerReviewsCacheRegion : CancellableCacheRegion<CustomerReviewsCacheRegion>`.

> TODO: !!!

### Change _modules.manifest_
If there are extension settings/permission/localizations in module.manifest, need to do:
* Create a class _ModuleConstants.cs_ which contains settings, permissions in _VirtoCommerce.Cart2Module.Web_ project
* Settings move to _ModuleConstants_, for example have Cart2.Search.Setting and the code will be: 
```
public static class Settings
{
	public static class General
	{
		public static SettingDescriptor Cart2Setting = new SettingDescriptor
		{
			Name = "VirtoCommerce.Cart2.Search.Cart2Setting",
			GroupName = "Cart2|Search",
			ValueType = SettingValueType.ShortText,
			IsDictionary = true,
		};

		
		public static IEnumerable<SettingDescriptor> AllSettings
		{
			get
			{
				yield return Cart2Setting;                        
			}
		}
	}

	public static IEnumerable<SettingDescriptor> AllSettings
	{
		get
		{
			return General.AllSettings;
		}
	}
}
```
and localization of name and description need to add localization file (read down)
> look at https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Core/ModuleConstants.cs
* Permissions also move to _ModuleConstants_, have permission for searching in Cart, then the code will be:
```
public static class Security
{
	public static class Permissions
	{
		public const string Cart2SearchAccess = "cart2:search:access";
		
		public static string[] AllPermissions = { Cart2SearchAccess };
	}
}
```
> look at https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Core/ModuleConstants.cs
* Localization of settings and permissions move to VirtoCommerce.Cart2Module.Web/Localizations/en.VirtoCommerce.Cart2.json
> example: https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Web/Localizations/en.VirtoCommerce.Customer.json

### Change signature in _Module.cs_
* Need to change inheritance to interface _IModule_ , then add implementation methods: _Initialize, PostInitialize, Uninstall_ and property _ModuleInfo_
* Add all dependency injections to _Initialize_ method, like as _Cart2DbContext, Cart2RepositoryImpl_
> look at https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/samples/VirtoCommerce.CustomerSampleModule.Web/Module.cs
* Register settings using interface _ISettingsRegistrar_ in PostInitialize method
```
var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
```
* Register permissions using interface _IPermissionsRegistrar_ in PostInitialize method 
```
var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x => new Permission() { GroupName = "Cart2", Name = x }).ToArray());
```
* Override type Cart2 in AbstractFactory, like this:
```
AbstractTypeFactory<Cart>.OverrideType<Cart, Cart2>().MapToType<Cart2Entity>();
AbstractTypeFactory<CartEntity>.OverrideType<CartEntity, Cart2Entity>();
```
* Add three methods MigrateIfNotApplied, EnsureCreated, Migrate into PostInitialize method, need for insure creating migration 
```
using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<CartDbContext>();
    dbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName(ModuleInfo.Id));
    dbContext.Database.EnsureCreated();
    dbContext.Database.Migrate();
}
```
> look at https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Web/Module.cs#L70 

> NOTE: Extension MigrateIfNotApplied need for backward compatibility v.2.0. The extension allows don't generate initial migration, because there are already tables in the DataBase.

### Authorization Requirement
If need to use authorization requirement then 
* create a class Cart2AuthorizationRequirement in VirtoCommerce.Cart2Module.Data.Authorization and derived from PermissionAuthorizationRequirement
> look at https://github.com/VirtoCommerce/vc-module-store/blob/release/3.0.0/src/VirtoCommerce.StoreModule.Data/Authorization/StoreAuthorizationRequirement.cs
* Add IAuthorizationService to Cart2ModuleController 
* Call method AuthorizeAsync in an action where to need
> look at https://github.com/VirtoCommerce/vc-module-store/blob/a7f39ce2fa41762f9c658bfa2453263932a67c17/src/VirtoCommerce.StoreModule.Web/Controllers/Api/StoreModuleController.cs#L51
* Then add condition for result 
```
if (!authorizationResult.Succeeded)
{
    return Unauthorized();
}
```

### Build js-scripts
If have extension scripts, then need to do:  
* add webpack packages (package.json, webpack.config.js)  to VirtoCommerce.Cart2Module.Web
> these files: https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Web/package.json
https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/src/VirtoCommerce.CustomerModule.Web/webpack.config.js
* change namespace in webpack.config.js (line 15) 
* build and pack js scripts and css for Cart2Module 
```
npm run webpack:dev 
```

### Building the project
* Check what all your extensions works with the new platform without exceptions and as expected with initial empty data


## If you find some errors, you can make a issue in https://github.com/VirtoCommerce/vc-platform-core/issues. Enjoy! :)
