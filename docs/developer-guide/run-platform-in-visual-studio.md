# Run the platform from source in Visual Studio

Use this setup when you work on the platform code itself. It follows the [ASP.NET Core configuration](https://learn.microsoft.com/aspnet/core/fundamentals/configuration/) model. Settings shared by the team stay in the committed `appsettings*.json` files, and each developer keeps their own connection strings and paths outside the repository.

## Prerequisites

* [Visual Studio 2026](https://visualstudio.microsoft.com/) with the **ASP.NET and web development** workload, which includes the .NET 10 SDK
* [Node.js LTS](https://nodejs.org/) with npm. The first build runs `npm install` and webpack for the admin UI.
* SQL Server. Any of these works:
    * **SQL Server Express LocalDB**: Visual Studio Installer → Individual components
    * SQL Server Developer edition
    * A container:

        ```bash
        docker run -d --name vc-sql -e ACCEPT_EULA=Y -e 'MSSQL_SA_PASSWORD=My0wn!Passw0rd' -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
        ```

* Optional: Redis for distributed cache and Elasticsearch 8 for search. Without them the platform uses non production mode: in-memory cache and Lucene search.

## 1. Get the code and build

```bash
git clone https://github.com/VirtoCommerce/vc-platform.git
```

Open `VirtoCommerce.Platform.sln`, set **VirtoCommerce.Platform.Web** as the startup project and build the solution. The first build restores npm packages and builds the admin UI bundles into `wwwroot/dist`. After changing admin UI scripts, rebuild them with `npm run webpack:build`, or keep `npm run webpack:watch` running in `src/VirtoCommerce.Platform.Web`.

## 2. Configure your machine, not the repository

Don't edit `appsettings.json`, `appsettings.Development.json` or `Properties/launchSettings.json` for your machine. These files are shared, and a changed connection string or password is easy to commit by mistake. Put your values here instead:

| Setting | Where it goes | Why |
|---|---|---|
| Connection strings, passwords, API keys, public URLs, search and cache settings | [User secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) | Stored outside the repository and loaded automatically in `Development` |
| `VirtoCommerce:DiscoveryPath`, `VirtoCommerce:ProbingPath` | User environment variables | Modules are loaded before the host starts, from `appsettings*.json` and environment variables only. User secrets are not read at that point. |

Configuration sources are applied in this order, and later sources win:

```
appsettings.json → appsettings.Development.json → user secrets → environment variables → command-line arguments
```

### User secrets

In Visual Studio, right-click **VirtoCommerce.Platform.Web** → **Manage User Secrets** and add your values:

```json
{
  "ConnectionStrings:VirtoCommerce": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VirtoCommerce3;Integrated Security=True;TrustServerCertificate=True;"
}
```

Or set them from the command line:

```bash
dotnet user-secrets set 'ConnectionStrings:VirtoCommerce' 'Data Source=localhost,1433;Initial Catalog=VirtoCommerce3;User ID=sa;Password=My0wn!Passw0rd;TrustServerCertificate=True;' --project src/VirtoCommerce.Platform.Web
```

The user in the connection string needs permission to create a database.

`appsettings.Development.json` already points the asset and content URLs at `http://localhost:10645`. Override `Assets:FileSystem:PublicUrl` and `Content:FileSystem:PublicUrl` only if you run the platform on another URL:

```json
{
  "Assets:FileSystem:PublicUrl": "http://localhost:5000/assets/",
  "Content:FileSystem:PublicUrl": "http://localhost:5000/cms-content/"
}
```

Optional settings for Redis and Elasticsearch 8 go in the same file:

```json
{
  "ConnectionStrings:RedisConnectionString": "localhost:6379,password=...",
  "Search:Provider": "ElasticSearch8",
  "Search:ElasticSearch8:Server": "http://localhost:9200"
}
```

The file is stored in `%APPDATA%\Microsoft\UserSecrets\VirtoCommerce.Platform.Web\secrets.json` on Windows and `~/.microsoft/usersecrets/VirtoCommerce.Platform.Web/secrets.json` on macOS and Linux. It is shared by every clone and worktree of this repository on your machine. User secrets are plain JSON on your disk and are meant for development only. Never use them in a deployed environment.

!!! note
    Earlier versions used the secrets id `local`. If you have settings in `%APPDATA%\Microsoft\UserSecrets\local\secrets.json`, move the ones for the platform into the new file.

### Module folders

By default the platform discovers modules in `src/VirtoCommerce.Platform.Web/modules`, which is git-ignored, and copies their assemblies to `app_data/modules`. To use modules from another folder, for example one populated by `vc-build install`, set user environment variables with absolute paths:

```powershell
[Environment]::SetEnvironmentVariable("VirtoCommerce__DiscoveryPath", "C:\vc\modules", "User")
[Environment]::SetEnvironmentVariable("VirtoCommerce__ProbingPath", "C:\vc\app_data\modules", "User")
```

Restart Visual Studio afterwards so it picks up the new variables. Environment variable names use `__` for nesting: `VirtoCommerce__DiscoveryPath` sets `VirtoCommerce:DiscoveryPath`.

## 3. Run

Press **F5**. The `VirtoCommerce.Platform.Web` launch profile starts the platform in the `Development` environment on `http://localhost:10645`.

To run from the command line with the same profile:

```bash
cd src/VirtoCommerce.Platform.Web
```

```bash
dotnet run
```

Don't pass `--no-launch-profile`. The platform then starts in the `Production` environment and ignores your user secrets and `appsettings.Development.json`.

On the first start the platform creates and migrates the database. Sign in with `admin` / `store` and change the password straight away. Check `http://localhost:10645/health` to confirm that the database, cache and modules report `Healthy`.

## Troubleshooting

* **Images don't display in the admin UI**: `Assets:FileSystem:PublicUrl` doesn't match the URL the platform runs on. Override it in user secrets.
* **Settings from user secrets are ignored**: the platform isn't running in the `Development` environment. Start it with the `VirtoCommerce.Platform.Web` launch profile.
* **Modules aren't discovered from a custom folder**: set `VirtoCommerce__DiscoveryPath` as an environment variable, not in user secrets, and restart Visual Studio.

## See also

* [Deploy from source code](./deploy-from-source-code.md): building the frontend, debugging and testing
* [Modules development via docker](./modules-development-via-docker.md): run the platform in containers and develop modules on the host
