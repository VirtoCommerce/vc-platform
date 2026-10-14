# Admin UI module

The platform is API only. The back office (Commerce Manager, the AngularJS 1.8 SPA) ships as the
[VirtoCommerce.AdminUI](https://github.com/VirtoCommerce/vc-module-admin-ui) module, so the platform can run without any UI,
and other admin UIs can be built and installed the same way.

## With and without the module

| | Platform only | Platform + `VirtoCommerce.AdminUI` |
|---|---|---|
| `/` | Short notice with links to `/docs` and `/health` | The admin SPA |
| REST API, `/connect/*`, `/docs`, `/health`, SignalR hub | Yes | Yes |
| `<apps>` (System Operations and others) at `/apps/{id}` | Yes | Yes |
| Module UI bundles (`modules/{id}/dist/app.js`) | Listed by `/api/apps/platform/manifest`, nobody loads them | Loaded by the SPA |
| `platform.*` UI strings (`*.VirtoCommerce.Platform.json`, `*.VirtoCommerce.Common.json`) | Not present | From the module's `Localizations` folder |

## What the platform provides to an admin UI

* **`IAdminUIContextProvider`** (`VirtoCommerce.Platform.Core`): the values the UI needs before sign-in —
  platform version, license, demo credentials and reset time, environment banner, whether modules can be installed,
  whether push notifications must use WebSockets. A UI module renders them into its page.
* **`GET api/platform/common/ui/context`** (anonymous): the same values as JSON, for UIs that are plain static files.
  Answers `404` when `VirtoCommerce:PlatformUI:Enable` is `false`.
* **`GET api/apps/{appId}/manifest`**: which module bundles to load, in dependency order. See [Backoffice modularity](./backoffice-modularity.md).
* **`IAdminUIAccessPolicy`**: who may enter the admin UI (`VirtoCommerce:PlatformUI:Access`).
* **A lowest-priority endpoint at `/`**: any module controller action mapped to `/` (for example `[HttpGet("/")]`) replaces the platform notice.

## How VirtoCommerce.AdminUI plugs in

The module uses the standard extension points only:

* `IHasHostEnvironment`: in `Initialize` the module adds its `Content/wwwroot` folder to `IWebHostEnvironment.WebRootFileProvider`.
  The SPA files keep the URLs they had in the platform (`/dist/app.js`, `/images/...`, `/css/...`), `asp-append-version` works,
  and a file of the platform's own `wwwroot` (`favicon.ico`, `swagger/`) wins over a module file with the same path.
* `IModule.PostInitialize`: maps the SPA sources to `/$(Platform)/Scripts`, where non-bundled templates are loaded from in development builds.
* An MVC controller mapped to `/` renders the page with the values of `IAdminUIContextProvider`.
* Its `Localizations` folder is read by the platform translation service like any other module's.

The webpack build writes the bundles to `Content/wwwroot/dist`, not to `dist`: the platform loads every module's
`dist/app.js` as an AngularJS plugin, and the shell itself must not be loaded twice.

## Building another admin UI

1. Create a module. Map the UI page at `/` with a controller, or serve a static page and read `api/platform/common/ui/context`.
2. Serve its assets from the module: through `/modules/$({moduleId})/...` (served for every module), or add a folder to the web root as described above.
3. Load module plugins from `GET api/apps/{appId}/manifest`.
4. Install either this module or `VirtoCommerce.AdminUI` — both map `/`, so only one admin UI module can own the site root.
