# Entity Framework Core Commands
```
dotnet tool install --global dotnet-ef --version 6.0.13
```

## Generate Migrations**

```
dotnet ef migrations add Initial 
dotnet ef migrations add Update1 
dotnet ef migrations add Update2 
```

etc..

## Apply Migrations**

```
dotnet ef database update
```
