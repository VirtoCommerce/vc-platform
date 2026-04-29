# VC-Shell Implementation Spec — Backoffice Modularity Adapter

> **Audience:** the VC-Shell team. This document specifies what `@vc-shell/mf-host`
> (and its surrounding packages) must ship to align with the platform-side
> Backoffice Modularity Framework already merged in `vc-platform`.
>
> Companion docs:
> - [Backoffice Modularity overview](backoffice-modularity.md) — partner-facing
> - [Framework reference](backoffice-modularity-framework.md) — platform-side details

---

## 1. Goal

VC-Shell currently calls `POST /api/frontend-modules` with a body of `{ appName, provides }` and expects a `{ modules: [...] }` registry. That endpoint **was never officially in `vc-platform`**; production installations have stubbed it out-of-tree, which is why upgrades have been painful.

The platform now ships an official endpoint:

```
GET /api/apps/{appId}/manifest
```

**Migrate `@vc-shell/mf-host` to call this canonical endpoint.** The platform keeps `POST /api/frontend-modules` as a permanent backwards-compatibility alias so you can take the upgrade at your own pace, but new code should use the canonical name.

---

## 2. Current state — what to keep

The following parts of vc-shell stay essentially as-is. Don't disturb them:

- **`packages/mf-config/src/shared-deps.ts`** — the shared singleton catalogue (Vue, Vue Router, Vue I18n, vee-validate, lodash-es, `@vueuse/core`, `@vc-shell/framework` and its subpaths). The platform doesn't care which deps you share; this list stays VC-Shell's call.
- **`packages/mf-module/src/dynamic-module-config.ts`** — the Vite config builder for plugin remotes. Unchanged.
- **`framework/core/plugins/modularity/index.ts`** (`defineAppModule`) — the per-plugin contract. Unchanged.
- **`framework/core/plugins/extension-points/`** — `defineExtensionPoint` / `useExtensionPoint`. Unchanged.
- The shape of `app.use(plugin, { router })` invocation. Unchanged.

What changes: the **bootstrap path** in `packages/mf-host/`.

---

## 3. The new platform contract

### 3.1 Endpoint

```http
GET /api/apps/{appId}/manifest
Cookie: ...                        ← session auth, send credentials
If-None-Match: "<last-etag>"      ← optional; gets 304 on no-change
```

### 3.2 Response (200 OK)

```ts
interface AppManifestResponse {
  /** Echo of {appId} from the URL. */
  appId: string;

  /** Version of the host app — running platform version for `appId == "platform"`,
      otherwise the version of the module declaring the <app> in its manifest. */
  version: string;

  /** Display title from the <app><title>...</title></app> declaration. */
  title: string;

  /** Plugins to load, in topological dependency order of owning modules.
      Already filtered server-side by current user permissions. */
  plugins: PluginEntry[];
}

interface PluginEntry {
  /** Defaults to the owning .NET module id. */
  id: string;
  /** Defaults to the parent module version. */
  version: string;
  /** Always a script. The MF entry of the plugin remote. */
  entry: ContentFile;
  /** Additional preload assets (CSS, occasionally extra scripts). */
  contentFiles: ContentFile[];
  /** MF coordinates. Always present for non-"platform" appIds. */
  remote: { name: string; exposed: string };
}

interface ContentFile {
  type: 'script' | 'style';
  /** Absolute URL served by the platform. The `$(...)` segment is literal. */
  path: string;
  /** Cache-busting hash, append as `?v={hash}`. */
  hash?: string;
}
```

### 3.3 Status codes

| Code | Meaning |
|---|---|
| `200` | Manifest body returned. ETag header set. |
| `304` | `If-None-Match` matched — body is empty, reuse cached response. |
| `401` | Unauthenticated (caller has no session). |
| `403` | The host app declares a `<permission>` and the caller lacks it. |
| `404` | No installed module declares an `<app>` with this `appId`. |

### 3.4 Cache headers

```
Cache-Control: private, must-revalidate
ETag: "<sha1-hex>"
```

The ETag inputs are: `appId`, all installed module ids+versions, the caller's sorted permission claims. It changes whenever any of those change.

### 3.5 Backwards-compatibility alias (transitional)

```http
POST /api/frontend-modules
Content-Type: application/json

{ "appName": "vc-shell-marketplace" }
```

Routes through the same handler. Marked `[Obsolete]` in the OpenAPI spec but never removed. **Don't use this in new mf-host versions** — but rest assured your deployed older clients keep working against the new platform.

---

## 4. Required changes in `@vc-shell/mf-host`

### 4.1 Files

```
packages/mf-host/src/
├── register-remote-modules.ts          ← MODIFY (the single change site)
└── types/
    └── manifest.ts                      ← NEW (TS types for the response)
```

### 4.2 `register-remote-modules.ts` — change set

Locate the existing constant:

```ts
const REGISTRY_URL = "/api/frontend-modules";
```

and the existing fetch call:

```ts
const response = await fetch(REGISTRY_URL, {
  method: "POST",
  credentials: "same-origin",
  headers: { "Content-Type": "application/json" },
  body: JSON.stringify({ appName: options.appName, provides: versionDict }),
});
```

Replace with:

```ts
import type { AppManifestResponse, PluginEntry } from './types/manifest';

const MANIFEST_URL = (appName: string) => `/api/apps/${encodeURIComponent(appName)}/manifest`;

const response = await fetch(MANIFEST_URL(options.appName), {
  method: "GET",
  credentials: "same-origin",
  headers: { Accept: "application/json" },
});

if (!response.ok) {
  // 304 doesn't apply on first load. 401/403 mean the user can't open this app
  // at all — let the host's auth flow handle that. 404 means appId isn't
  // declared by any installed module — surface as a console warning, render
  // an empty plugin set, and continue.
  console.warn(`[mf-host] manifest endpoint returned HTTP ${response.status}`);
  return;
}

const manifest = (await response.json()) as AppManifestResponse;
```

### 4.3 Map the new shape to the existing `ModuleRegistryEntry`

The internal `ModuleRegistryEntry` you already use to feed `createInstance({ remotes: ... })` can stay. Map fields:

```ts
const entries: ModuleRegistryEntry[] = manifest.plugins.map((p) => ({
  id: p.id,
  // The platform stamps the cache-busting hash via ?v=… when entry.hash is set.
  entry: p.entry.hash ? `${p.entry.path}?v=${encodeURIComponent(p.entry.hash)}` : p.entry.path,
  version: p.version,
  // Map p.remote.name + p.remote.exposed onto whatever your loadRemote() call expects.
  // Default convention: p.remote.exposed === "./Module" → loadRemote(`${p.remote.name}/Module`)
  remoteName: p.remote.name,
  exposedKey: stripLeadingDotSlash(p.remote.exposed),
  // Compatibility filter: drop the `compatibleWith` shim. The platform already
  // refused to install incompatible modules at .NET module-load time — by the
  // time you receive a PluginEntry the dependency graph has been validated.
}));

function stripLeadingDotSlash(s: string) {
  return s.replace(/^\.\//, '');
}
```

### 4.4 Drop the `provides` payload

`provides: { "@vc-shell/framework": Framework.version, ... }` was the npm-semver
compatibility filter you were sending. **Remove it.** The platform doesn't speak
npm semver and doesn't need to — it relies on the existing
`<dependency id="..." version="...">` validator at module-install time.

If this raises an internal alarm bell ("but how do we know plugin X is
compatible with my framework version?"), the answer is: plugin authors declare
the host module version they need in their `module.manifest`. The platform
refuses to load a module whose dependency declarations aren't satisfied. That
check happens once, at install/restart, not per request.

### 4.5 Loader simplification

Currently the inline logic in `register-remote-modules.ts` mixes fetch +
loader + install. Recommended (but not required) refactor: split into
`loaders/` per-kind files. The platform supports two loaders — `module-federation`
and `legacy` — but VC-Shell's host only ever sees `module-federation`
(the legacy AngularJS path is platform-internal and never reaches you).
Keeping it as one file is fine.

### 4.6 Bump major version

`@vc-shell/mf-host` to **3.0.0**. This is a breaking change at the network
layer (different verb, different URL, different response shape) and consumers
need to know. Even though their app code (`registerRemoteModules(app, { appName, router })`)
is unchanged, the platform contract under it isn't.

Update [`MIGRATION_GUIDE.md`](https://github.com/VirtoCommerce/vc-shell/blob/master/MIGRATION_GUIDE.md)
with a single paragraph:

> ### 3.0.0 — Backoffice Modularity manifest endpoint
> `@vc-shell/mf-host` now consumes the canonical platform endpoint
> `GET /api/apps/{appId}/manifest` instead of the legacy
> `POST /api/frontend-modules`. **No app-code change required** — the
> `registerRemoteModules(app, { appName, router })` API is unchanged.
> Platforms running the modularity framework (`vc-platform` 3.X.Y or
> later) serve both endpoints; older platforms with the legacy stub
> need to upgrade. See [`backoffice-modularity.md`](https://github.com/VirtoCommerce/vc-platform/blob/master/docs/developer-guide/backoffice-modularity.md).

---

## 5. New file: `packages/mf-host/src/types/manifest.ts`

```ts
// Mirror of the platform DTOs in
// vc-platform/src/VirtoCommerce.Platform.Web/Model/Modularity/.
// Keep in sync — or generate from OpenAPI in a follow-up.

export interface AppManifestResponse {
  appId: string;
  version: string;
  title: string;
  plugins: PluginEntry[];
}

export interface PluginEntry {
  id: string;
  version: string;
  entry: ContentFile;
  contentFiles: ContentFile[];
  remote: PluginRemote;
}

export interface ContentFile {
  type: 'script' | 'style';
  path: string;
  hash?: string;
}

export interface PluginRemote {
  name: string;
  exposed: string;
}
```

A follow-up improvement: generate this from the platform's OpenAPI spec instead of
hand-maintaining. Not blocking.

---

## 6. Plugin-side changes

The plugin authoring contract is **unchanged**. A plugin remote built with
`@vc-shell/mf-module`'s `getDynamicModuleConfiguration()` continues to expose
`./module` (or `./Module`) and default-export an object with `install(app, opts)`
or whatever `defineAppModule(...)` produces.

The only thing plugin authors need to verify: their build output lands at
`{moduleRoot}/plugins/{appId}/remoteEntry.js`. Today's `@vc-shell/mf-module`
default `outDir: 'dist/mf'` does **not** match — it produces
`{moduleRoot}/dist/mf/remoteEntry.js`, which is **not** where the platform's
manifest endpoint probes.

**Two ways to bridge the gap.** Pick one:

### Option A (recommended): change `@vc-shell/mf-module`'s default `outDir`

In `packages/mf-module/src/dynamic-module-config.ts`:

```diff
- build: { target: 'esnext', outDir: 'dist/mf' },
+ build: { target: 'esnext', outDir: '../plugins/<appId>' },
```

The challenge: `outDir` is per-vite-config and `<appId>` varies per plugin/host. The
`getDynamicModuleConfiguration()` factory currently doesn't take an `appId`.
Either:

- Add an `appId` option to `DynamicModuleOptions`:
  ```ts
  export interface DynamicModuleOptions extends UserConfig {
    /** Discovery folder name. Defaults to <pkg.name> for back-compat. */
    appId?: string;
    /* ...existing... */
  }
  ```
  When provided, point `outDir` at `../plugins/<appId>` (relative to the
  subpackage); otherwise fall back to `dist/mf` for back-compat.

- Or: deprecate `getDynamicModuleConfiguration()` and document the explicit
  Vite config plugins should write themselves (matches the
  `@module-federation/vite` ecosystem norms).

### Option B: leave `@vc-shell/mf-module` alone, plugin authors override `outDir`

In each plugin's `vite.config.mts`:

```ts
import { getDynamicModuleConfiguration } from "@vc-shell/mf-module";
import { mergeConfig } from "vite";

export default mergeConfig(getDynamicModuleConfiguration(), {
  build: {
    outDir: '../plugins/vc-shell-marketplace',
    emptyOutDir: true,
  },
});
```

Less elegant; punt the burden to every plugin author. **Prefer Option A.**

---

## 7. Migration plan

### 7.1 Phase order

| # | What | Owner |
|---|---|---|
| 1 | Land platform changes (`vc-platform`) | Platform team — ✅ done |
| 2 | Ship `@vc-shell/mf-host` 3.0 with the new endpoint | VC-Shell team — **this doc** |
| 3 | Ship `@vc-shell/mf-module` with new default `outDir` (Option A) | VC-Shell team |
| 4 | Update one consumer app (e.g. marketplace) end-to-end | Marketplace team |
| 5 | Deprecate `POST /api/frontend-modules` in OpenAPI but keep the alias forever | Platform team |

Phases 2 and 3 can ship in the same release. Phase 4 is the canary; once
proven, broader rollout to other vc-shell-based apps follows their own cadence.

### 7.2 Compatibility matrix during rollout

| `@vc-shell/mf-host` version | Platform endpoint used | Works with platform |
|---|---|---|
| 2.x (current) | `POST /api/frontend-modules` | Old (with stub) and new (alias) |
| 3.0+ (this work) | `GET /api/apps/{appId}/manifest` | New only |

A consumer running mf-host 2.x against the new platform: works (alias).
A consumer running mf-host 3.0 against an old platform without the
modularity framework: fails — they get a 404 on the manifest endpoint
and the loader logs a warning. There's no graceful fallback to the alias
in 3.0 because we want consumers to see the upgrade boundary clearly.

### 7.3 Test matrix

For the 3.0 release, verify against:

1. **New platform, new mf-host, new plugin** (`outDir: plugins/{appId}/`) — full happy path.
2. **New platform, new mf-host, old-style plugin** (`outDir: dist/mf/`) — should fail with a clear error in the loader (tells the plugin author to migrate).
3. **New platform, old mf-host (2.x)** — works via alias. Smoke test.
4. **Old platform (no modularity), new mf-host** — loader logs warning, app boots without plugins.

---

## 8. Verification

### 8.1 Unit tests

In `packages/mf-host/`:

- **`register-remote-modules.test.ts`** — extend / replace existing tests with:
  - Mock `fetch` returning a 200 + new shape; assert correct `loadRemote` calls.
  - Mock 304 on second call; assert no double-load.
  - Mock 404; assert graceful no-op + console warning.
  - Mock 200 with empty `plugins[]`; assert no remotes registered.

### 8.2 E2E

1. Spin up `vc-platform` with the modularity framework merged.
2. Install one MF plugin module that ships
   `{moduleRoot}/plugins/vc-shell-marketplace/remoteEntry.js`.
3. Build a vc-shell consumer app at `mf-host` 3.0.0.
4. Open in browser:
   - DevTools Network: exactly one `GET /api/apps/vc-shell-marketplace/manifest` →
     200 with one plugin.
   - On reload (with same module set + same user): the same request returns
     `304 Not Modified`.
   - The plugin's blade renders.
5. Reinstall the module after rebuilding the plugin — observe the manifest
   endpoint returns 200 with a new ETag and a new `?v={hash}` on the entry.
   Confirm the browser refetches the new bundle.

### 8.3 Curl-level smoke

```bash
# 200 + JSON
curl -i -b session.cookies https://platform/api/apps/vc-shell-marketplace/manifest

# 304 with same ETag
curl -i -b session.cookies \
  -H 'If-None-Match: "<copied from previous response>"' \
  https://platform/api/apps/vc-shell-marketplace/manifest

# 404 for unknown app
curl -i -b session.cookies https://platform/api/apps/does-not-exist/manifest

# Legacy POST alias still works
curl -i -b session.cookies \
  -H 'Content-Type: application/json' \
  -d '{"appName":"vc-shell-marketplace"}' \
  https://platform/api/frontend-modules
```

---

## 9. Open questions

1. **Should `@vc-shell/mf-host` accept an explicit `manifestUrl` option?** Lets
   ops point at a non-default endpoint for debugging. Default stays
   `/api/apps/${appName}/manifest`. **Recommended: yes**, low cost.

2. **Capabilities filter / feature flags.** The platform's manifest endpoint
   doesn't currently accept a request body. If VC-Shell wants per-call
   capability filtering, we'd extend the controller to accept
   `?capabilities=foo,bar` query params (or fall back to `POST` with a body).
   **Out of scope for v1**; raise later if a use case materialises.

3. **Deprecation timeline for the legacy `POST /api/frontend-modules` alias.**
   Plan as written: kept forever. Reconsider only if it becomes operationally
   painful (e.g. masks bugs because old clients silently work).

4. **Should `@vc-shell/mf-module` add an `appId` option?** Yes — see §6
   Option A. Confirm naming (`appId` vs `discoveryFolder` vs `pluginsFolder`).

---

## 10. Reference

- Backoffice Modularity overview: [`backoffice-modularity.md`](backoffice-modularity.md)
- Framework reference (platform-side internals): [`backoffice-modularity-framework.md`](backoffice-modularity-framework.md)
- VC-Shell repo: <https://github.com/VirtoCommerce/vc-shell>
- The file to modify (today): [`packages/mf-host/src/register-remote-modules.ts`](https://github.com/VirtoCommerce/vc-shell/blob/master/packages/mf-host/src/register-remote-modules.ts)
- The shared deps catalogue (untouched): [`packages/mf-config/src/shared-deps.ts`](https://github.com/VirtoCommerce/vc-shell/blob/master/packages/mf-config/src/shared-deps.ts)
- The dynamic plugin Vite config (touch in §6): [`packages/mf-module/src/dynamic-module-config.ts`](https://github.com/VirtoCommerce/vc-shell/blob/master/packages/mf-module/src/dynamic-module-config.ts)
