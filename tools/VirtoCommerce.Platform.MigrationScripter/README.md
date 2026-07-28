# VirtoCommerce Platform Migration Scripter

A companion, ops-time utility that **grabs the EF Core migrations that platform startup would apply**
(platform + security + every installed module) and writes them to `.sql` files — **without applying anything
or starting a web server**.

Use it to review (and optionally hand-apply) schema changes *before* the platform first runs.

## Why this tool exists

The platform applies migrations programmatically at web startup (`Database.Migrate()`); there is no built-in
way to preview that SQL. `dotnet ef migrations script` needs buildable `.csproj` projects, so it cannot see
installed modules in a **deployed binaries folder**. This tool solves that by replicating the platform's
module-loading bootstrap (`ModuleBootstrapper`), registering the platform + security DbContexts and running
every module's `IModule.Initialize` (where module `AddDbContext` registrations happen) into a service
collection, then scripting every registered `DbContext`. It never calls `Migrate()` and never starts a web
server, so nothing is applied.

**Dependencies — data / security / modules only (no `Platform.Web`):** the module loader treats platform
assemblies (`Caching`, `Security`, `Data.*`, `Hangfire`, …) as *Trusted Platform Assemblies* and loads the
host's copy by name. Modules bundle their own (often older) copies; unless those assemblies are in this
process's TPA set, loading a module's bundled copy fails with *"assembly already loaded"*. Referencing the
platform **data + security + modules** projects (`Platform.Data`, `Platform.Data.{SqlServer,PostgreSql,MySql}`,
`Platform.Security`, `Platform.Modules`) puts the full platform assembly closure into the TPA set — the same
coverage a real host has — **without** pulling in the ASP.NET web app (MVC, SignalR, Swagger, OpenIddict-web).
Verified equivalent to a `Platform.Web`-based build on a live 52-module deployment.

## Scope

- ✅ **PlatformDbContext + SecurityDbContext** (registered by `Startup`).
- ✅ **All installed module DbContexts** — full coverage from the deployed folder.

## Build

```bash
dotnet build tools/VirtoCommerce.Platform.MigrationScripter
```

Build from a **platform version close to** the folder you will point it at (the tool acts as a mini platform
host; a 3.10xx build loads 3.10xx modules fine, as with a normal platform upgrade).

## Run

```bash
dotnet VirtoCommerce.Platform.MigrationScripter.dll --platform-path <deployed-folder> --output <out-folder>
```

Options:

| Option | Description |
|---|---|
| `-p`, `--platform-path <path>` | Deployed platform folder (has `appsettings.json`, `./modules`, `./app_data`). Defaults to the current directory. |
| `-o`, `--output <path>` | Output folder for `.sql` files. Defaults to `<platform-path>/migration-scripts`. |
| `--include-empty` | Also write files for contexts with no pending migrations. By default, up-to-date contexts are skipped (no empty "up to date" files). |
| `-h`, `--help` | Show help. |

Provider and connection string are read from the platform's `appsettings.json`. Override them without editing
files via environment variables: `DatabaseProvider`, `ConnectionStrings__VirtoCommerce`.

### PowerShell wrapper

`Grab-Migrations.ps1` (in this folder) sets those env vars for you, invokes the tool, and can zip the output:

```powershell
./Grab-Migrations.ps1 -PlatformPath 'C:\vc\platform' -DatabaseProvider SqlServer `
    -ConnectionString 'Data Source=.;Initial Catalog=VC;Integrated Security=True;TrustServerCertificate=True' `
    -OutputPath 'C:\vc\out' -Zip
```

Handy switches: `-IncludeEmpty` (also write up-to-date contexts), `-Build` (build the tool first), `-Zip`.

## Output

```
<out>/
  _combined.sql        # platform -> security -> modules, in dependency (load) order
  Platform.sql
  Security.sql
  <ModuleId>.sql       # one per module DbContext with pending migrations
```

By default, contexts that are already up to date produce no file (pass `--include-empty` to write them too).

## Script format

- **Pending-only delta** per context: only the migrations not yet applied (computed from `__EFMigrationsHistory`).
  The delta is anchored on the last applied migration that exists in the deployed assembly, so a database that
  is *ahead* of the binaries (an applied migration the assembly doesn't contain) does not break scripting.
- **Idempotent fallback**: if the database is unreachable or has no migration history, an idempotent full script
  (each migration self-guards) is written instead, with a warning.

## Caveats

- **Database reachability:** pending-only needs a reachable DB. Otherwise the idempotent fallback fires.
- **MySql:** the MySql provider auto-detects the server version when building options, so the server must be
  reachable even for the fallback. SqlServer/PostgreSql script fully offline.
- **Nothing is applied.** The tool never runs migrations and never starts Kestrel.
- **Hangfire** schema is created by Hangfire storage (not EF Core) and does not appear in these scripts.
- **Resilience:** if a single context fails to script, it is logged and skipped; the remaining contexts and the
  combined file are still produced.
- **Version:** build/run the tool from a platform version close to the target folder's version.
