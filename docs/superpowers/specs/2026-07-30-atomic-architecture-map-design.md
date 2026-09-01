# Atomic Architecture Map — Design Spec

**Status:** Approved for implementation
**Date:** 2026-07-30
**Author:** Oleg Zhuk
**Location:** `docs/atomic-map/`

---

## 1. Goal

Give a developer **one screen** that answers three questions without reading source:

1. *What primitives does Virto Commerce give me?*
2. *Which one do I reach for, and which should I not?*
3. *Where does it sit in the stack?*

The artifact is an interactive HTML poster — a **periodic table of atoms** under a **Solution Architecture
band**, with a reserved shelf for composite topics (**molecules**). Print-material clarity at a glance; a
click opens a memo card with pattern, when-to-use, what-to-avoid, real API names, and a C# snippet taken
from this repository.

It is a **памятка** (cheat-sheet) meant to be continuously updated, so maintainability of the *content* is a
first-class requirement — not an afterthought.

---

## 2. Why not just more prose docs

`docs/fundamentals`, `docs/developer-guide` and `docs/techniques` already explain most of these primitives
well. What they cannot do is show the **shape of the whole set** — which is exactly what a newcomer lacks.
Prose answers "how does caching work"; only a map answers "what are my nine options for doing work
asynchronously, and which three are deprecated or not-yet-real".

The map therefore **links to** the prose rather than duplicating it. Every atom carries `docs[]` pointers.

---

## 3. Grounding — code review changed the content

Reviewing `src/` before authoring surfaced facts that a from-memory map would have gotten wrong. This is the
core justification for the adoption-badge design in §5.

| Finding in `src/` | Consequence |
|---|---|
| `VirtoCommerce.Platform.Hangfire` is **not in `VirtoCommerce.Platform.sln`** — only a stale `.xml` doc file remains on disk | Job atoms must not be labelled "Hangfire" |
| `Platform.Core/Jobs` exposes engine-agnostic `IBackgroundJob`, `IBackgroundJobHandler<TPayload>`, `IRecurringJobService`, `IMapReduceJob`, `IJobProgress`, and `BackgroundJobEngineNotInstalledException` | Job atoms are **△ IN FLIGHT**; engine ships as an installable module (`VirtoCommerce.BackgroundJobs`) |
| `Core/Jobs/BackgroundJob.cs` documents itself as a *migration aid* for code moving off static `Hangfire.BackgroundJob.Enqueue` | Static fire-and-forget is **✕ LEGACY** → inject `IBackgroundJob` |
| No `HybridCache`, no `IDistributedCache` store, no Keyed DI, no `System.Threading.Channels`, no `TimeProvider` call sites, no Minimal APIs anywhere in `src/` | Those .NET atoms are **○ AVAILABLE**, each pointing to the platform-native alternative |
| Redis caching is an **invalidation bus** (`RedisPlatformMemoryCache`, `RedisCachingMessage`), not a shared cache store | Corrects the most common wrong mental model about VC caching |
| `Polly 8.7.0` and `Microsoft.Bcl.TimeProvider` are referenced packages with no matched call sites | Marked ○ pending per-atom verification at authoring time |
| MVC is configured with `Microsoft.AspNetCore.Mvc.NewtonsoftJson` | JSON atom must warn that API serialization is Newtonsoft, not `System.Text.Json` |

**Verified stack:** .NET 10 (`10.0.10` packages), EF Core 10, OpenIddict 7.5, Serilog 4.3,
StackExchange.Redis 3.0 + RedLock.net 2.3, FluentValidation 12.1, Swashbuckle 10.2, platform `3.1053.0`.

---

## 4. Layout

```
┌─ SOLUTION ARCHITECTURE ─────────────────────────────────────┐
│ Channels │ API edge │ Modules │ Platform │ Integration │ Infra │
└─────────────────────────────────────────────────────────────┘
  ATOMS — colour-coded by family, badged by adoption
  ┌──┐┌──┐┌──┐ │ ┌──┐┌──┐ │ ┌──┐┌──┐┌──┐ │ ┌──┐┌──┐
  │Jb││Rj││MR│ │ │MC││Cr│ │ │St││DP││Op│ │ │Ev││Bu│  …
  └──┘└──┘└──┘ │ └──┘└──┘ │ └──┘└──┘└──┘ │ └──┘└──┘
   Execution      Caching       Config       Messaging
 ─────────────────── MOLECULES (reserved) ───────────────────
  [ eCommerce Modules ] [ Dev Process + Roles ] [ Deployment ] …
```

Target: fits one screen at 1440×900 with no page scroll. The drawer opens over the right side; only the
drawer scrolls.

---

## 5. Adoption badges

Five states, each a **glyph + colour** so meaning never depends on colour alone:

| Badge | Meaning |
|---|---|
| ● PLATFORM | Platform-native. This is the Virto way. |
| ◐ MODULE | Shipped by an installable module, not the platform core. |
| ○ AVAILABLE | .NET/ASP.NET Core offers it; the platform does not use it. Points to the alternative. |
| △ IN FLIGHT | Changing right now. Drawer carries the migration note. |
| ✕ LEGACY | Exists, still works, do not build new code on it. |

The badge is the map's highest-value feature: it converts a catalog into **guidance**.

---

## 6. Packaging

No build step, no npm, no bundler, no CDN, no network at runtime.

```
docs/atomic-map/
  index.html            poster shell, legend, drawer skeleton
  styles.css            grid, tiles, drawer, light/dark, @media print
  app.js                render · search · filter · drawer · hash routing · schema check
  content/
    architecture.js     window.VC_MAP_ARCHITECTURE
    atoms.js            window.VC_MAP_ATOMS
    molecules.js        window.VC_MAP_MOLECULES
  README.md             maintenance contract
```

**Renderer and content are decoupled.** Adding or correcting an atom means editing one object in
`content/atoms.js` — never touching `app.js`. That is what makes continuous updating realistic.

**Constraint:** content loads via classic `<script src>` assigning to `window.*`, **not**
`<script type="module">`. ES modules are blocked by CORS on the `file://` origin, which would break
double-click-to-open — the entire reason for choosing this packaging.

**Publishing:** MkDocs uses the `awesome-pages` plugin with no explicit `nav:` in `mkdocs.yml`, so the new
folder publishes with no config change.

---

## 7. Content schema

```js
{
  id: 'background-jobs',            // deep-link slug → #/atom/background-jobs
  symbol: 'Jb', name: 'Background Jobs',
  family: 'execution',              // colour + column group
  adoption: 'in-flight',            // platform | module | available | in-flight | legacy
  layer: 'platform-core',           // cross-highlights the architecture band
  oneLiner: 'Durable, engine-agnostic queued work.',
  pattern: 'Port/adapter + command message. Platform owns the port; an engine module adapts it.',
  whenToUse: [...], avoid: [...], gotchas: [...],
  api: [{ name: 'IBackgroundJob', file: 'src/VirtoCommerce.Platform.Core/Jobs/IBackgroundJob.cs' }],
  snippet: { lang: 'csharp', code: '...' },
  docs: [{ label: 'Scalability', href: '../fundamentals/scalability.md' }],
  seeAlso: ['recurring-jobs', 'distributed-lock'],
  molecule: 'background-processing-hub',
  verifiedAgainst: '3.1053.0'
}
```

Two anti-staleness mechanisms:

1. **`app.js` validates required fields at render time** and paints a visible `⚠ schema` badge on any
   incomplete tile. Bad content fails loudly instead of rendering blank.
2. **`verifiedAgainst`** records the platform version an atom was last checked against, surfaced in the
   footer. Age becomes visible rather than assumed.

---

## 8. Atom inventory (8 families, ~50 tiles)

| Family | Tiles |
|---|---|
| Execution & Async | Background Jobs △ · Recurring Jobs △ · Map/Reduce △ · Job Progress △ · Fire-and-Forget ✕ · `BackgroundService` ● · CancellationToken ● · `AsyncLock` ● · Channels ○ |
| Caching | `PlatformMemoryCache` ● · Cache Regions ● · Redis invalidation bus ● · Request-scoped cache ● · `CacheDisabler` ● · HybridCache / `IDistributedCache` ○ |
| Config & Metadata | Settings ● · Dynamic Properties ● · Options pattern ● · `ProcessSettings` ● · Localizations ● |
| Messaging & Events | Domain Events ● · `InProcessBus` ● · Commands ● · Push Notifications ● · WebHooks / EventBus ◐ |
| Data & Domain | `AbstractTypeFactory` ● · Generic CRUD ● · Repository + UoW ● · EF Core 10 + db-agnostic ● · Change Log ● · Specifications ● · FluentValidation ● · JSON ● |
| Modularity | `IModule` lifecycle ● · Module catalog & install ● · `IPlatformStartup` ● · DI & override rules ● · Keyed DI ○ · Export/Import ● |
| Security | Permissions ● · Authentication ● · Authorization policies ● · Current user & tenancy ● |
| Infra & Ops | Distributed Lock ● · Logging ● · Swagger/OpenAPI ● · `IHttpClientFactory` ● · Resilience ○ · Developer Tools ● · File/Blob & Zip ● · Health checks ● · `TimeProvider` ○ · Minimal APIs ○ |

`AbstractTypeFactory` carries extra visual weight — it is the keystone of the extension model.

---

## 9. Architecture band

Clickable layers, each opening the same drawer component:

- **Channels** — VC-Frontend (Vue 3 · Vite · TS) · Admin SPA (AngularJS 1.8 blades) · 3rd-party & mobile
- **API edge** — XAPI `/xapi/graphql` (xCatalog · xCart · xOrder · xCMS · xProfile) · REST `/api` · Swagger · outbound WebHooks
- **Modules** — vertical slice `Module.Core` / `Module.Data` / `Module.Web`, plus a **module-type matrix**:
  XAPI-only · CRUD-only · All-in-one (XAPI + Services + CRUD + Admin UI) · Provider (payment/shipping/tax) ·
  Engine (job engine, search provider)
- **Platform** — `Platform.Core` · `.Data` (+3 DB providers) · `.Modules` · `.Caching` · `.Security` · `.DistributedLock` · `.Web`
- **Integration** — EventBus · Integration Middleware · ERP / WMS / CRM
- **Infrastructure** — SQL Server / PostgreSQL / MySQL · Redis · Elasticsearch · Blob · SignalR

---

## 10. Molecules shelf

Reserved, outlined-not-filled tiles so the poster reads complete while the gap stays honest.

Requested: **eCommerce Modules** · **Development Process + Roles** · **Deployment Schema**.

Proposed, each already backed by material in this repo: **Background Processing Hub**
(`2026-06-06-background-processing-hub-design.md`) · **Search & Indexing** (`fundamentals/search.md`) ·
**Notifications** · **Payment / Shipping / Tax providers** (`fundamentals/extensibility/new-*-registration.md`) ·
**XAPI deep-dive** · **Extensibility decision tree** (no-code → API → native) · **Scale-out playbook**
(`fundamentals/scalability.md`, `techniques/how-scale-out-platform-on-azure.md`) · **Multi-store / multi-region**
(`architecture-center/B2B-multiregional.md`) · **Security & Compliance** · **Testing strategy** ·
**Observability**.

---

## 11. Interaction design

- **Search** — `/` focuses; fuzzy match over name, symbol, API type names, tags. Non-matching tiles **dim**
  rather than disappear, so the poster's shape never jumps.
- **Filters** — chips per family and per badge. "Show only ● PLATFORM" answers *"what's the Virto way?"*
- **Cross-highlight** — hovering an architecture layer dims atoms outside it; opening an atom highlights its
  layer. This is what makes it a *map* rather than a list.
- **Drawer** — right side, ~480px, fixed section order (What · Pattern · When · Avoid · API · Snippet ·
  Gotchas · Docs · See also). Fixed order means developers learn where to look once.
- **Deep links** — `#/atom/<id>`, shareable into a PR or chat.
- **Keyboard** — `/` search, arrows move focus, Enter opens, Esc closes, `?` toggles legend.
- **Print** — `@media print` puts the poster on A3/A4 landscape with the legend, drawer hidden.
- **Theme** — `prefers-color-scheme` plus manual toggle.
- **Accessibility** — real `<button>` tiles, `aria-label`s, visible focus ring, AA contrast, glyph+colour badges.

---

## 12. Out of scope

- Commerce-module atoms (Search/Indexing, Notifications, Payment/Shipping/Tax internals) — they live in other
  repos and cannot be code-verified from here. They are molecule tiles for now.
- Any framework or build tooling.
- Generating the map from source. Tempting, but the value here is **curated judgement** — when to use, what to
  avoid, which alternative — and that is exactly what codegen cannot produce.

---

## 13. Verification

- Zero console errors; zero non-local network requests (proves the no-network claim).
- Every `api[].file` exists under `src/`; every `docs[].href` resolves under `docs/`.
- Every snippet's cited types/members grep-confirmed present in the file it references.
- Every ○ AVAILABLE badge re-confirmed by call-site grep; every ● PLATFORM badge backed by ≥1 call site.
- Layout at 1280×800 / 1440×900 / 1920×1080 / mobile; no horizontal body scroll.
- Print preview at A4 landscape: full poster, legend present, nothing clipped.
- Keyboard-only pass.
