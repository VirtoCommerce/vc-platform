# Entity Framework Core Commands
```
dotnet tool install --global dotnet-ef --version 6.0.13
```

## Generate PlatformDbContext Migrations
```
dotnet ef migrations add Update1 --context PlatformDbContext --output-dir Migrations/Data
```

## Generate SecurityDbContext Migrations
```
dotnet ef migrations add Update2 --context SecurityDbContext --output-dir Migrations/Security
```

## Apply Migrations**

```
dotnet ef database update
```
