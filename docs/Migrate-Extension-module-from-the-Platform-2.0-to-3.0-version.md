### Introduction
This article describes how to migrate existing extension module from 2.0 to 3.0 version.

> NOTE: A sample module source code can be found here: https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/samples/VirtoCommerce.CustomerSampleModule.Web

### Migrating extension module to ASP.NET Core
For example, have project with name _Cart2Module.Web.csproj_ for _CartModule_.
* Create a new branch with  name: release/3.0.0 then switch to the branch.
* Then create a new solution for extension module if not exists.
* Then need to migrate the project from ASP.NET to ASP.NET Core.
> please read this article https://docs.microsoft.com/en-us/aspnet/core/migration/proper-to-2x

Need to make changes in the Core/Data/Web extension projects _Cart2Module.XXX.csproj_
* Create a project VirtoCommerce.Cart2Module.Core.csproj
* Change xml signature inside csproj to
```
<Project Sdk="Microsoft.NET.Sdk"> </Project>
```
* Then add _TargetFramework_ as _PropertyGroup_ inside _Project_ 
```
<TargetFramework>netcoreapp2.2</TargetFramework>
```
* Add references NuGet packages(if you have) and dependencies from _module.manifest_ as _ItemGroup_
```
<PackageReference Include="VirtoCommerce.Platform.Core" Version="3.0.0-rc0001" />
<PackageReference Include="VirtoCommerce.CartModule.Core" Version="3.0.0-rc0001" />
``` 
* Move all domain models/events and interfaces to Cart2Module.Core from Cart2Module.Data
> look at example https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/VirtoCommerce.CartModule.Core/VirtoCommerce.CartModule.Core.csproj
#### Some action need to do with existing projects Cart2Module.Data, Cart2Module.Web and Cart2Module.Tests: 
- Change xml signature inside csproj as sample in Core-project
- Remove all files depending with .net framework 4.x (properties, packages, configs)
- Add references

> look at example https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/samples/VirtoCommerce.CustomerSampleModule.Web/VirtoCommerce.CustomerSampleModule.Web.csproj

### Create _Cart2DbContext.cs_
* Need to add _Cart2DbContext_ inheritance from _CartDbContext_
* Add overriding _OnModelCreating_ method
* Then need to map all extended models. 
If there are classes Cart2Entity.cs and _Cart2_ with signature:
```
public class Cart2 : ShoppingCart
{
    public string CartType { get; set; }
}
```
Then need to add mapping to modelBuilder
```
modelBuilder.Entity<Cart2Entity>();
```
> look at example https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/samples/VirtoCommerce.CustomerSampleModule.Web/Repositories/CustomerSampleDbContext.cs

### Create _DesignTimeDbContextFactory.cs_
> NOTE: A factory for creating derived Microsoft.EntityFrameworkCore.DbContext instances.
* Create a class _DesignTimeDbContextFactory.cs_
* Inherite from _IDesignTimeDbContextFactory<Cart2DbContext>_
* Add CreateDbContext method with Cart2DbContext, example code can copy from https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/samples/VirtoCommerce.CustomerSampleModule.Web/Repositories/DesignTimeDbContextFactory.cs#L8
* Change connection string for database in _UseSqlServer_ method. It need for migration.
> look at example: https://github.com/VirtoCommerce/vc-module-customer/tree/release/3.0.0/samples/VirtoCommerce.CustomerSampleModule.Web/Repositories/DesignTimeDbContextFactory.cs

### Change Cart2Repository
* The class have to derive from CartRepository and have constructor with the new Core2DbContext dependency
* Add DbSet _Cart2_ to access data
```
public IQueryable<Cart2Entity> Cart2 => DbContext.Set<Cart2Entity>();
```
* Remove OnModelCreating method
* Add overriding methods, if need to add some logic
> look at example: https://github.com/VirtoCommerce/vc-module-order/tree/release/3.0.0/samples/VirtoCommerce.OrdersModule2.Web/Repositories/OrderRepository2.cs

### Create Migration
* Remove old migrations in the folder Migrations which generated for v. 2.0
* Create a migration with name _InitialCart2_, you could run a command from _Package Manager Console_
```
Add-Migration InitialCart2 -Context VirtoCommerce.Cart2Module.Web.Repositories.Cart2DbContext -StartupProject VirtoCommerce.Cart2Module.Web  -Verbose -OutputDir Migrations
```
* then need to remove the lines which depends CartDbContext(like Tables, FK, PK, Index)
* If the entity Cart2 extend with a new property CartType, then need to add code:
```
migrationBuilder.AddColumn<string>(name: "CartType", table: "Cart", maxLength: 128, nullable: true);
```
and add line:
```
migrationBuilder.AddColumn<string>(name: "Discriminator", table: "Cart", nullable: false, maxLength: 128, defaultValue: "Cart2Entity");
```
> look at https://docs.microsoft.com/en-us/ef/core/modeling/relational/inheritance
> example https://github.com/VirtoCommerce/vc-module-order/tree/release/3.0.0/samples/VirtoCommerce.OrdersModule2.Web/Migrations/20180724064542_InitialOrders2.cs


### Create Migration for backward compatibility v.2.0
* Need to create migration with name UpdateCart2V2 and rename the migration file name to 20000000000000_UpdateCart2V2 
> look at link https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.cs
* Add SQL Insert command to 20000000000000_UpdateCart2V2 
> look at https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.cs#L13 

and rename migration name to _InitialCart2_ in sql-script. 
* Also need to rename UpdateCart2V2.Designer to 20000000000000_ UpdateCart2V2.Designer and rename MigrationAttribute 
> look at example https://github.com/VirtoCommerce/vc-module-core/tree/release/3.0.0/src/VirtoCommerce.CoreModule.Data/Migrations/20000000000000_UpdateCoreV2.Designer.cs#L12

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
