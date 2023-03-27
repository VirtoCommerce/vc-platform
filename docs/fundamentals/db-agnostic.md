# DB Agnostic

## Overview
Virto Commerce built with DB agnostic architecture and out of the box supports the following databases:
* Microsoft SQL Server 2019 or above
* MySql Server 5.7 or above
* PostgreSQL 12 or above

At same time, Virto Commerce Architectire allows you add custom database provider on top of Entity Framework for solution as well as for a specific module.


## Principles
We designed DB Agnostic Architecture with focus on few architecture principles:

1. No Braking Changes.
2. Built with Entity Framework.
3. Database Provider Isolation on Project Level.
4. Support Customization and Specific.


First, No Braking Changes. You can aplply updates without breaking changes in your solution.

Secondly, building a DB Agnostic system with well-known Entity Framework can provide many benefits for your dev team. 

Thirdly, the principle of Provider Isolation. We isolate database specific code as a specific project. This approach allows for each project to utilize the database provider that best suits its specific needs, without affecting core project.

Finally, We added Support for customization by specific Database providers need. By allowing for customization at the provider level, you can take advantage of the unique features and capabilities of each database technology, while still benefiting from the DB agnostic approach.

## How Setup DB Provider
You can easily setup DB provider for your solution. Just follow the steps below:
1. Install and Configure Database. Create a new database and user with access to it.
1. Open appsettings.json in your preferred text editor.
1. Find the **DatabaseProvider** propery.
1. Set the value to the name of the database provider you want to use. For example, "SqlServer", "PostgreSQL", "MySql", etc.
1. Locate the "ConnectionString/VirtoCommerce" property in the appsettings.json file.
1. Change the value of the "ConnectionString" property to match the connection details for the database.

Here sample of connection string for differnt DB providers:

Microsoft SQL Server
```json
    "DatabaseProvider": "SqlServer",
    "ConnectionStrings": {
    "VirtoCommerce": "Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;Connect Timeout=30;TrustServerCertificate=True;"
    },
```

MySql Server
```json
    "DatabaseProvider": "MySql",
    "ConnectionStrings": {
    "VirtoCommerce": "server=localhost;user=root;password=virto;database=VirtoCommerce3;"
    },
```

PostgreSQL
```json
    "DatabaseProvider": "PostgreSql",
    "ConnectionStrings": {
    "VirtoCommerce": "User ID=postgres;Password=password;Host=localhost;Port=5432;Database=virtocommerce3;"
    },
```

After these steps, you can run Virto Commerce.

## Create a Custom Module with DB Agnostic Approach

This is a useful technique if you want to develop a module that can work with different database systems without having to rewrite your code for each one.

Virto Commerce Module Templates for Dotnet is a tool that helps you generate the basic structure and files for a new module based on some parameters. You can find it on GitHub: https://github.com/VirtoCommerce/vc-cli-module-template

To create a new module with a database agnostic approach, you need to run the following command:

```cmd
dotnet new vc-module-dba-template --ModuleName CustomerReviews --Author "Jon Doe" --CompanyName VirtoCommerce
```

This will create a folder named CustomerReviews containing all the required projects and files for your module. You can then open solution in Visual Studio.

The key points to understand about this template are:
* It contains four projects related to data access: Data and Data.[Provider] projects.
* The Data.[Provider] projects have a specific structure and configuration for each database system: MySql, PostgreSql, and SqlServer.
* The Data project contains the common data models and interfaces that are shared by all database systems.
* The Module.Initialize method registers the DbContext service using the AddDbContext extension method from Virto Commerce Platform Core library.
* The OnModelCreating extension method allows you to customize the entity type configuration for different database systems using conditional compilation symbols.

Let's take a closer look at each of these points.

### Data and Data.[Provider] projects

The template generates four projects related to data access:

- CustomerReviews.Data
- CustomerReviews.Data.MySql
- CustomerReviews.Data.PostgreSql
- CustomerReviews.Data.SqlServer

These projects allow your module to be database agnostic, meaning it can be easily adapted to work with different database systems without significant code changes.

Each project provides a specific implementation for working with a particular database system, while the CustomerReviews.Data project serves as the common base for all database-related functionality.

This architecture was designed to promote code reusability, maintainability, and scalability.

### Data.[Provider] projects structure

Each Data.[Provider] project has the following structure:

* Migrations folder - contains the migration files for the database system.
* DbContextOptionsBuilderExtensions class - contains the helper method for configuring the DbContextOptionsBuilder for specific database provider.
* [Provider]DbContextFactory class - contains implementation of IDesignTimeDbContextFactory for  for specific database provider.  
* Readme.md file - contains the instructions for configuring and migration creation the specific database provider.

### [Provider] Model Customization
One of the features of Virto Commerce is that it allows you to configure various aspects of the entity type required
for a specific database provider needs. For example, you can specify the properties, keys, indexes, relationships,
etc. of your entities using a fluent API.

To use this feature, you need to implement the IEntityTypeConfiguration<TEntity> interface in your entity configuration classes.
This interface defines a method called Configure that takes an EntityTypeBuilder<TEntity> parameter and
configures various aspects of the entity type.

The following code snippet shows an example of how to implement this interface for a CurrencyEntity entity:

```cs
    public class CurrencyEntityConfiguration : IEntityTypeConfiguration<CurrencyEntity>
    {
        public void Configure(EntityTypeBuilder<CurrencyEntity> builder)
        {
            builder.Property(x => x.ExchangeRate).HasColumnType("decimal").HasPrecision(18, 4);
        }
    }
```

## Summary
By choosing the ideal database engine for your unique scenario, you can cut operational costs and experience significant savings.
