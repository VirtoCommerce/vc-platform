# Admin UI

The platform is API only. The back office ships as admin UI modules:

| Module | App id | UI |
|---|---|---|
| [VirtoCommerce.AdminAngularJs](https://github.com/VirtoCommerce/vc-module-admin-angular-js) | `platform` | The AngularJS admin (Commerce Manager), formerly part of the platform |
| [VirtoCommerce.AdminShell](https://github.com/VirtoCommerce/vc-module-admin-shell) | `admin-shell` | The Vue admin (vc-shell) |

Any number of admin UIs can be installed. One of them owns the site root (`/`); every one is also reachable at `/apps/{appId}/`.

## Scenarios

| # | Installed | `VirtoCommerce:PlatformUI:RootApp` | `/` | Also |
|---|---|---|---|---|
| 1 | No admin UI | — | A notice: no admin UI is installed, with links to `/docs` and `/health` | REST API, `/connect/*`, `/docs`, `/health`, SignalR, `<apps>` work as before |
| 2 | AngularJS admin | empty | AngularJS admin | `/apps/platform/` |
| 3 | Admin Shell | empty | Admin Shell | `/apps/admin-shell/`. AngularJS menu items say that the AngularJS admin isn't installed |
| 4 | Both | empty | AngularJS admin (priority 100 beats 50) | Admin Shell at `/apps/admin-shell/`, its AngularJS screens embed `/` — as before the split |
| 5 | Both | `admin-shell` | Admin Shell | AngularJS admin at `/apps/platform/`; the shell embeds its screens from there and discovers them for the menu editor |
| 6 | Any | an app id, or `none` | That app, or the notice | See [Another admin UI](#another-admin-ui) |

```json
{
  "VirtoCommerce": {
    "PlatformUI": {
      "RootApp": "admin-shell"
    }
  }
}
```

or the environment variable `VirtoCommerce__PlatformUI__RootApp=admin-shell`.

`RootApp` values:

* empty: the installed admin UI with the highest priority;
* the id of an installed module's `<app>`, with or without `<adminUI>`;
* `none`: no admin UI at the root, for API-only nodes (the admin UIs stay reachable at `/apps/{appId}/`).

An id that no installed module declares is logged as a warning and treated as empty.

## Declaring an admin UI

An admin UI is an `<app>` in `module.manifest` with an `<adminUI>` element. Platforms before 3.1093.0 ignore the element.

```xml
<apps>
  <app id="admin-shell">
    <title>Admin Shell</title>
    <placement>AppMenu</placement>
    <adminUI priority="50" />                       <!-- hosting="App" by default -->
  </app>
</apps>
```

| Attribute | Meaning |
|---|---|
| `priority` | When `RootApp` is empty, the installed admin UI with the highest priority owns the site root. AngularJS admin: 100, Admin Shell: 50 |
| `hosting="App"` | A static single-page app in the app's content folder (`Content/{appId}` or `<contentPath>`), served at `/apps/{appId}/`. As the root admin UI, the platform answers `/` with the app's index page and the address stays `/`. The page must load its files by absolute URLs (`/apps/{appId}/...`, what Vite emits with `base: "/apps/{appId}/"`) and route with the URL hash. |
| `hosting="Endpoint"` | The module maps the page itself: an action at `/` marked with `[AdminUIPage("{appId}")]`, answering GET and HEAD. The platform also serves the UI at `/apps/{appId}/`: requests under that path are handled as if they came to the site root, so the page and every URL relative to it (`api/...`, bundles) work there too. The page puts `PathBase + AdminUIRequestFeature.MountPath + "/"` into its `<base href>`. The AngularJS admin uses it. Unknown `hosting` values are read as `App`. |

## How the platform routes

* `AdminUIMiddleware`, first in the request pipeline (so module hooks, the API no-cache headers, error handling, static files and routing all see the rewritten path):
    * a request under `/apps/{appId}/` of an `Endpoint` admin UI is handled as if it came to the site root (`/apps/platform/api/...` as `/api/...`); the path base is unchanged, so cookies, token issuers and generated URLs stay those of the site root, and `AdminUIRequestFeature` tells the page its mount path. `/apps/{appId}` without the slash redirects to it;
    * `/` goes to the root admin UI: for an `App` one the path becomes `/apps/{appId}/`; otherwise the choice is left to the next step.
* `AdminUIMatcherPolicy`: several pages map `/` — each `Endpoint` admin UI's and the platform's own notice. The policy keeps the page of the admin UI chosen for the request, or the notice when that admin UI has none.
* `GET api/platform/apps` reports each admin UI's link where it opens: `/` for the root one, `/apps/{appId}` for the others.
* The notice page and the AngularJS admin answer `HEAD /`, as load balancers probe the site root with it. The notice ranks last, so any other endpoint a module maps at `/` wins over it.
* `VirtoCommerce:PlatformUI:Enable = false` turns the admin UI off: `/` shows the notice whatever `RootApp` says, and the AngularJS admin refuses its page (also at `/apps/platform/`).

> **Reverse proxies and WAFs:** under `/apps/{appId}/` of an `Endpoint` admin UI the whole platform is reachable (`/apps/platform/api/...`, `/apps/platform/docs`). Authorization is the same as at the root; rules that allow or block paths by prefix need to cover the `/apps/{appId}/` form too.

## What the platform provides to an admin UI

* **`GET api/platform/common/ui/context`** (anonymous): the values a UI needs before sign-in — platform version, license, demo credentials, environment banner, whether modules can be installed, whether push notifications must use WebSockets — and `adminUIs`: each installed admin UI with its `url` (`/` or `/apps/{appId}/`) and `isRoot`. Admin Shell finds the AngularJS admin there. Answers `404` when `VirtoCommerce:PlatformUI:Enable` is `false`.
* **`IAdminUIContextProvider`** and **`IAdminUIAppService`** (`VirtoCommerce.Platform.Core`): the same, for a module that renders its page on the server.
* **`GET api/apps/{appId}/manifest`**: which module plugins to load, in dependency order. `platform` lists the modules' AngularJS bundles. See [Backoffice modularity](./backoffice-modularity.md).
* **`IAdminUIAccessPolicy`**: who may enter the admin UI (`VirtoCommerce:PlatformUI:Access`).

## How VirtoCommerce.AdminAngularJs plugs in

* `IHasHostEnvironment`: in `Initialize` the module adds its `Content/wwwroot` folder to `IWebHostEnvironment.WebRootFileProvider`. The SPA files keep the URLs they had in the platform (`/dist/app.js`, `/images/...`, `/css/...`), `asp-append-version` works, and a file of the platform's own `wwwroot` (`favicon.ico`, `swagger/`) wins over a module file with the same path.
* `IModule.PostInitialize`: maps the SPA sources to `/$(Platform)/Scripts`, where non-bundled templates are loaded from in development builds.
* An MVC controller renders the page at `/` (`[AdminUIPage("platform")]`, GET and HEAD) with the values of `IAdminUIContextProvider`; its `<base href>` adds `AdminUIRequestFeature.MountPath`, so the same page works at `/apps/platform/`.
* Its `Localizations` folder (`platform.*` strings) is read by the platform translation service like any other module's.

The webpack build writes the bundles to `Content/wwwroot/dist`, not to `dist`: the platform loads every module's `dist/app.js` as an AngularJS plugin, and the AngularJS admin itself must not be loaded twice.

## Another admin UI

1. Create a module with an `<app>`: a static SPA (`hosting="App"`) or a page the module renders (`hosting="Endpoint"`, `[AdminUIPage]`).
2. Read `api/platform/common/ui/context` for the values to show before sign-in, sign in with the platform's OAuth endpoints (`/connect/token`), and load module plugins from `GET api/apps/{appId}/manifest`.
3. Declare `<adminUI priority="...">` to take the site root when it is the highest, or set `RootApp` to its app id.

An app that doesn't declare `<adminUI>` can still be made the root with `RootApp`, as an `App` admin UI.
