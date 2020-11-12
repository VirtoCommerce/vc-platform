# Extend Database Model

This article provides the steps to take to extend persistent model.

!!! tips
    [VirtoCommerce.OrdersModule2.Web](https://github.com/VirtoCommerce/vc-module-order/tree/master/samples/VirtoCommerce.OrdersModule2.Web) sample module is used as an example throughout the guide. 

## Changes in "_.Core_" project
- Add reference to **"_.Core_"** NuGet package containing the base models, e.g., _VirtoCommerce.OrdersModule.Core_
- Define the new model class by extending the base model in **Models** folder:
- e.g., `CustomerOrder2 : CustomerOrder` 
- Add the additional properties

## Changes in "_.Data_" project
1. Add reference to **"_.Data_"** NuGet package containing the base models, e.g., _VirtoCommerce.OrdersModule.Data_
1. Define the new model class by extending the base model in **Models** folder:

    1. e.g., `CustomerOrder2Entity : CustomerOrderEntity` 
    1. Add the additional properties
    1. Override `ToModel`, `FromModel`, and `Patch` methods

1. Changes in **Repositories** folder:
    1. Define a new DbContext class by extending the parent DbContext; add 1 public constructor; override `OnModelCreating`. Check [the sample](https://github.com/VirtoCommerce/vc-module-order/blob/release/3.0.0/samples/VirtoCommerce.OrdersModule2.Web/Repositories/Order2DbContext.cs) for details.
    1. Create `DesignTimeDbContextFactory` for the _DbContext_, just defined in previous step.
    1. **_Optional:_** Extend the parent repository interface: add `IQueryable<T>` property, add additional methods, if needed.
    1. Extend the parent repository by implementing the interface, just defined in previous step. If new interface wasn't defined, override the base methods as needed. It's important to add the `IQueryable<the new type>`, e.g.:

    ```csharp
    public IQueryable<CustomerOrder2Entity> CustomerOrders2 => DbContext.Set<CustomerOrder2Entity>();
    ```
   
1. Generate code-first DB migration:
    1. Execute "**Set as Startup Project**" on your **"_.Data_"** project in Solution Explorer
    2. Open NuGet **Package Manager Console**
    3. Select your "**.Data**" as "**Default project**" in the console
    4. Run command to add a new migration. Where "_YourNewMigrationName_" is the name for the migration to add. The new migration files should be generated and opened:
```console
Add-Migration YourNewMigrationName
```

    5. Explore the generated code and **remove** the commands, not reflecting your model changes or configurations defined in your _DbContext.OnModelCreating()_. These changes were applied already by migrations in base module. Ensure, that the _Up()_ method defines:

        1. new tables and indices, like:

        ```csharp
        migrationBuilder.CreateTable(
               name: "OrderInvoice",
               ...
        );

        migrationBuilder.CreateIndex(
              name: "IX_OrderInvoice_CustomerOrder2Id",
              table: "OrderInvoice",
              column: "CustomerOrder2Id");
        ```

        1. the new column(s), if existing tables are being altered, like:

        ```csharp
        migrationBuilder.AddColumn<string>(name: "NewField", table: "CustomerOrder", maxLength: 128, nullable: true);
        ```

        1. a `Discriminator` column (if new columns were defined in previous step AND it didn't exist in the original table already):

        ```csharp
        migrationBuilder.AddColumn<string>(name: "Discriminator", table: "CustomerOrder", nullable: false, maxLength: 128, defaultValue: "CustomerOrder");
        ```

        1. any custom SQL scripts, if data update is needed.

        !!! tip
            Read the [EF Core article about inheritance](https://docs.microsoft.com/en-us/ef/core/modeling/inheritance) for more details.

    1. The _Down()_ method should do the opposite of what _Up()_ does. That way you can apply and un-apply your changes quickly by `Update-Database` command in console.

## Changes in "_.Web_" project
The changes required in **_module.manifest_** file:

1. Ensure, that a dependency to appropriate module is added to **_dependencies_** section:
```xml
    <dependency id="VirtoCommerce.Orders" version="3.0.0" />
```

The required changes to **_Module.cs_** regarding the model extension:

1. Changes in **_Initialize()_** method:
    1. Register the new _DbContext_ in DI:
```csharp
serviceCollection.AddDbContext<Order2DbContext>(options => options.UseSqlServer(configuration.GetConnectionString("VirtoCommerce")));
```
    1. Register the new _Repository_ implementation in DI:
```csharp
serviceCollection.AddTransient<IOrderRepository, OrderRepository2>();
```

1. Changes in **_PostInitialize()_** method:
    1. Register type override(s) to AbstractTypeFactory
    1. Register new type(s) to AbstractTypeFactory (as in usual module)
    1. Add code to ensure that the migrations from new DbContext are applied:
```csharp
using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<Order2DbContext>();
    dbContext.Database.EnsureCreated();
    dbContext.Database.Migrate();
}
```

1. Test your changes in  Solution REST API documentation (Swagger) and DB.


> This tutorial provided the specific steps to take while extending any existing module's model and persistency layer. Also check the [How to create a new module](../developer-guide/create-new-module.md) guide for common steps while creating a module.
