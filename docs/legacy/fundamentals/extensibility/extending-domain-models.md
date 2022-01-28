
VirtoCommerce supports extension of managed code domain types. This article shows how to use the various techniques to extend exist domain type without direct code modification.


[View or download sample code](https://github.com/VirtoCommerce/vc-module-order/tree/dev/samples/VirtoCommerce.OrdersModule2.Web)

## Extending with type inheritance

Common domain classes have a fixed structure and are defined in modules. This means that you cannot add additional properties to existing domain types without direct code modification. On of possible way to do this is extend an entity class and add additional properties in the subclass.

Let’s demonstrate how the domain model extension works, extending the `CustomerOrder` type that defined in the **OrderModule** with new properties.

> However, this approach does not work anymore when one domain entity type should be extended from different modules. The extension domain model is based on class inheritance and the .NET does not support multiple class inheritance and, in the result, only one the last registered extension wins. You should consider this limitation.

First step what we should do is define new subclass `CustomerOrder2` derived from original `CustomerOrder` class.

*`VirtoCommerce.OrdersModule2.Web/Model/CustomerOrder2.cs`*
```C#
    public class CustomerOrder2 : CustomerOrder
    {
        public CustomerOrder2()
        {
            Invoices = new List<Invoice>();
        }
        public string NewField { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }
```

Now, we need to register the newly defined `CustomerOrder2` type in the `AbstractFactory<>` in order to tell the system that `CustomerOrder2` is now overlying (replace) the original `CustomerOrder` class and will be used everywhere instead of it.

*`VirtoCommerce.OrdersModule2.Web/Module.cs`*
```C#
    public class Module : IModule
    {
        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            ...
             AbstractTypeFactory<CustomerOrder>.OverrideType<CustomerOrder, CustomerOrder2>()
                                            .WithFactory(() => new CustomerOrder2 { OperationType = "CustomerOrder" }); //need to preserve original order  discriminator value
            ...
        }
    }
```

> **AbstractTypeFactory<>** is the key element in the Virto **extension concept** that response for instantiate a concrete type instance based on internal types mapping table.

Each code which should support domain types extensions must use   

`AbstractTypeFactory<BaseType>.TryCreateInstance()` instead of `new BaseType()`  statement, when you need to override any base type with another derived type you must call   `AbstractTypeFactory<BaseType>.OverrideType<BaseType, DerivedType>()`  after this, each  calls  `AbstractTypeFactory< BaseType>.TryCreateInstance()`  will return your `DerivedType` object instance instead of `BaseType`. 

That’s how the magic with types extension works!

## The schema of  persistent layer extensions.

You just saw how to extend the exists `CustomerOrder`  in class  with a new class `CustomerOrder2` with new  properties. But, how can you actually change the current DB schema and persist these new types into the database through Entity Framework (EF) Core? To solve this task we can also  use the inheritance technics here, and define and derive  the new   `Order2DbContext`  from original `OrderDbContext` along with `OrderRepository2` derived from `OrderRepository`.

*`VirtoCommerce.OrdersModule2.Web/Repositories/Order2DbContext.cs`*
```C#
    //Derive custom DB context from the OrderDbContext
    public class Order2DbContext : OrderDbContext
    {
         public Order2DbContext(DbContextOptions<Order2DbContext> builderOptions) : base(builderOptions)
        {
        }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //the method code
        }
    }
```

*`VirtoCommerce.OrdersModule2.Web/Repositories/OrderRepository2.cs`*
```C#
 public class OrderRepository2 : OrderRepository
    {
        public OrderRepository2(Order2DbContext dbContext, IUnitOfWork unitOfWork = null) : base(dbContext, unitOfWork)
        {
        }
    }
```

In Virto for persistence logic we use **[Data Mapper](https://www.martinfowler.com/eaaCatalog/dataMapper.html)** pattern, it helps to completely isolate your domain from the persistence layer. Usages of this pattern give us more benefits with keeping domain contracts in a more stable state and allows to us change persistence schema without affecting a domain that usually play the role of public contracts.

The each domain type has its own representation in the database, it is the special `DataEntitity` classes that are defined in EF Core `DbContext` through fluent syntax and contains a three methods:

* `ToModel` and `FromModel` – for map object of domain types into persistent and vice versa
* `Patch` for apply changes to only  specified columns. This method is crucial  for implementation of partial update logic.

Now let’s define the new persistence `CustomerOrder2Entity` type that will represent the persistence schema model of the new `CustomerOrder2` class.

*`VirtoCommerce.OrdersModule2.Web/Model/CustomerOrder2Entity.cs`*
```C#
    public class CustomerOrder2Entity : CustomerOrderEntity
    {
        public override OrderOperation ToModel(OrderOperation operation)
        {
           //the method code
        }
        public override OperationEntity FromModel(OrderOperation operation, PrimaryKeyResolvingMap pkMap)
        {
           //the method code
        }
    }

```

The next step we need to generate the new Db Migration for our new extended `Order2DbContext`.

We can do that by run this command in *Nuget* package version console in *Visual Studio.*

```Console 
Add-Migration InitialOrder2 -Context VirtoCommerce.OrdersModule2.Web.Repositories. Order2DbContext   -Verbose -OutputDir Migrations -Project VirtoCommerce.OrdersModule2.Web -StartupProject VirtoCommerce.OrdersModule2.Web -Debug
```

The result of this command execution will be `Migrations/XXXXXX_InitialOrder2.cs` file that will also contains the original (extendable) order module DB schema along with a new one. Thus, you need manually edit the resulting `InitialOrder2.cs` file and left only DB schema changes that relevant to your extension. 


*`VirtoCommerce.OrdersModule2.Web/Migrations/20200324130250_InitialOrders2.cs`*
```C#
    public partial class InitialOrders2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           //the method code
        }
    }
```

>Note: In order to avoid complex process of editing the resulting migration that will contain an original and custom schema, we recommend initially create an empty initial migration first for derived DbContext class has no changes, then you can just cleanup the resulting  migration cs file and leave the Up and Down methods empty. 
Then you can make changes to the custom "derived" DbContext and generate a new migration  that contains only your custom changes, so you can avoid the complex manual editing of the initial migration.

And the final step is to register our derived `OrderRepository2` and `Order2DbContext` in DI container. By registration the new `OrderRepository2` in DI we override the base `OrderRepository`  that is defined in `CustomerOrder.Module`.

*`VirtoCommerce.OrdersModule2.Web/Module.cs`*
```C#
    public class Module : IModule
    {
        public void Initialize(IServiceCollection serviceCollection)
        {
            var snapshot = serviceCollection.BuildServiceProvider();
            var configuration = snapshot.GetService<IConfiguration>();
            serviceCollection.AddDbContext<Order2DbContext>(options => options.UseSqlServer(configuration.GetConnectionString("VirtoCommerce")));
            serviceCollection.AddTransient<IOrderRepository, OrderRepository2>();

        }
    }
```

Also is so important to register our new persistent schema representation `CustomerOrder2Entity` in `AbstractTypeFactory<>` and override the base `CustomerOrderEntity` with new type.

*`VirtoCommerce.OrdersModule2.Web/Module.cs`*
```C#
    public class Module : IModule
    {
        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            ...

            AbstractTypeFactory<IOperation>.OverrideType<CustomerOrder, CustomerOrder2>();
            AbstractTypeFactory<CustomerOrderEntity>.OverrideType<CustomerOrderEntity, CustomerOrder2Entity>();

            ...
        }
    }
```

## How the API understands/deserializes the derived domain types 

In the previous paragraphs, we have considered extending domain types and persistent layers, but, in some cases, it is not enough. Especially when your domain types are used as DTOs (Data Transfer Objects) in public API contracts and can be used as a result or parameter in the API endpoints. 

There is a technical task of an instantiation of the right “effective” type instance from incoming JSON data (deserialization).

There are two ways to force ASP.NET Core API Json serializer to understand our domain extensions:
1. Use platform-defined `PolymorphJsonConverter`. It's preferable to use in most cases. The `PolymorphJsonConverter` transparently deserializes extended domain types with no developer effort.
2. Custom JSON converter passed to MvcNewtonsoftJsonOptions. Consider using it, if `PolymorphJsonConverter` is not suitable for your specific case.
