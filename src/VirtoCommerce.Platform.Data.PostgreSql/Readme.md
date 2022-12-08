
## Package manager 
Add-Migration Initial -Context VirtoCommerce.Platform.Data.Repositories.PlatformDbContext  -Verbose -OutputDir Migrations\Data -Project VirtoCommerce.Platform.Data.PostgreSql -StartupProject VirtoCommerce.Platform.Data.PostgreSql  -Debug
Add-Migration Initial -Context VirtoCommerce.Platform.Security.Repositories.SecurityDbContext  -Verbose -OutputDir Migrations\Security -Project VirtoCommerce.Platform.Data.PostgreSql -StartupProject VirtoCommerce.Platform.Data.PostgreSql  -Debug



### Entity Framework Core Commands
```
dotnet tool install --global dotnet-ef --version 6.*
```

**Generate Migrations**

```
dotnet ef migrations add Initial -- "{connection string}"
dotnet ef migrations add Update1 -- "{connection string}"
dotnet ef migrations add Update2 -- "{connection string}"
```

etc..

**Apply Migrations**

`dotnet ef database update -- "{connection string}"`
