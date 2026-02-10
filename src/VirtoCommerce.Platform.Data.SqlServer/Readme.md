# Generate Migrations

## Install CLI tools for Entity Framework Core
```cmd
dotnet tool install --global dotnet-ef --version 10.0.1
```

or update

```cmd
dotnet tool update --global dotnet-ef --version 10.0.1
```

## Add Migration
Select Data.<Provider> folder and run one of the following commands, depending on the required database context:

```cmd
dotnet ef migrations add <migration-name> --context PlatformDbContext
```

```cmd
dotnet ef migrations add <migration-name> --context SecurityDBContext
```
