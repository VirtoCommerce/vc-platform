# Build a Virto Commerce Extension-Point Inspector for <FRAMEWORK>

I want a runtime developer tool that highlights every extension point on the page in
the **<FRAMEWORK> codebase at <REPO_PATH>** (e.g. vc-frontend Vue 3 + Vite,
or a React storefront, etc.) and provides one-click copy of the registration snippet
for each. This is the same idea as the AngularJS Platform Manager inspector that
already exists at:

`src/VirtoCommerce.Platform.Web/wwwroot/js/app/developer-tools/extensionPointInspector.js`
in the `vc-platform` repo (Opus built it; reference it for the proven UX and tricky
parts). Replicate the UX, but adapt the detection layer to <FRAMEWORK>'s extension
model.

## Phase 1 — discover the extension points

Before writing any code, search the codebase to enumerate the actual extension
points and how modules register against them. Look for:

- Plugin registry / module init APIs (e.g. `app.use(...)`, `defineModule`,
  `registerSlot`, `registerComponent`, named `<slot>` / `<component :is>` patterns,
  Module Federation `exposes`, etc.)
- Slot / fallthrough patterns in templates (`<slot name="...">`, named outlets)
- Route / view registration
- Composables / Pinia stores that hold registries developers extend
- Any existing internal devtools, MCP server hooks, or feature flags

Produce a short table:

| Extension point | DOM marker | Runtime registry | Snippet template |
|---|---|---|---|
| (e.g. Header slot) | `[data-vc-slot="header"]` | `useSlotRegistry().slots.header` | `registerSlot('header', { component: ... })` |

If the framework's slots have no stable DOM marker, **add one** behind a dev-mode
flag (a `data-vc-slot` attribute applied conditionally), rather than relying on
fragile heuristics.

## Phase 2 — implement the inspector

A self-contained vanilla-JS / TS file (no framework imports — works even when the
host app is in an error state). For Vue 3 + Vite this means a plain `.ts` module
loaded via a small `<script setup>` shim that runs only in `import.meta.env.DEV`.
For other frameworks: load the inspector only in development mode.

### Public console API (must match exactly)

```ts
window.vcExt = {
  help(),                  // print usage + link to extensibility docs
  show(),                  // overlay every detected extension point (idempotent)
  hide(),                  // tear down all overlays + listeners
  toggle(),
  list(),                  // console.table of every registered item
  copy(type, id?)          // copy a snippet template for the named point
};
```

On bundle load, log: `[Virto] Extension-point inspector loaded. Type vcExt.help() to begin.`

### Overlay UX

- One root `<div id="vc-ext-inspector-root" data-vc-ext="1">` appended to
  `document.body`. `position:fixed; pointer-events:none; z-index:2147483600`.
- Per host: **two siblings** under root — a *box* (`pointer-events:none`,
  dashed border, semi-transparent fill) and a *label* (`pointer-events:auto`,
  coloured background, monospace text + Copy button). The label is **not** a
  child of the box. Reason: when a parent has `pointer-events:none` and a
  positioned child sits outside the parent's geometric rect, real mouse
  hit-testing rejects the child — `elementFromPoint` works but actual click
  dispatch does not. Sibling layout avoids this.
- Colour-code per type. Label text format: `[type] :: <name> (N items)`.
  Omit `:: <name>` when name is empty (no `<unknown>` placeholder).
- Cap label `max-width: 400px`; inner text span uses
  `flex:1 1 auto; min-width:0; overflow:hidden; text-overflow:ellipsis;
  white-space:nowrap`. Copy button is `flex:0 0 auto`.
- Label sits 22 px above its box by default. When `rect.top < 22` (host at top
  of viewport), flip the label to `top: 0` *inside* the box so it stays visible.

### Click handling — non-negotiable

- Delegate on the **root**, in **capture phase**, listening to **both
  `mousedown` and `click`**. Mousedown is the load-bearing one: it fires before
  any framework-induced DOM mutation can tear down the button mid-press.
- Deduplicate via a `copyLockUntil` timestamp window (~500 ms).
- Copy must use a **synchronous `<textarea>` + `document.execCommand('copy')`**
  inside the user-gesture handler, with `navigator.clipboard.writeText` fired
  as a best-effort second write. Async `.catch(fallback)` paths lose the
  user-gesture context and silently fail.
- On copy: green toast at viewport bottom-centre (`Snippet copied to clipboard\n[type] name`).
  Red toast `Copy failed — see console` plus `console.warn(snippet)` on failure.
  Button text briefly flips to `Copied!` / `Copy failed`.

### Auto-update — without an infinite rebuild loop

- Watch DOM via `MutationObserver` on `document.body`
  (`childList: true, subtree: true, attributes` filtered to the marker
  attributes you decided in Phase 1 — **do NOT include `class`**, framework
  reactivity will spam it).
- **Tag every inspector-owned element with `data-vc-ext="1"`** (root, both
  boxes/labels, the toast, the copy textarea). The observer must filter out
  mutations whose added/removed nodes are ALL internal — otherwise the
  inspector's own DOM changes trigger more rebuilds, ad infinitum.
- Debounce `scheduleRebuild` via `setTimeout(120)`, not `requestAnimationFrame`.
- If `copyLockUntil` is in flight, defer the rebuild — never tear down a
  button the user is currently pressing.
- Reposition (not rebuild) on `scroll` (capture) and `resize`, throttled with rAF.

### Snippet copy format

Bare framework-idiomatic call, no module-bootstrap wrapper. Examples:

```ts
// Vue 3 Pinia + composable example
useSlotRegistry().register('header', { component: defineAsyncComponent(...) });

// Slot registration via plugin
app.config.globalProperties.$vcSlots.add('header', SomeComponent);
```

Adapt to whatever the framework's actual extension idiom is — match the snippet
to what the developer would *paste into their existing module's setup file*.

## Phase 3 — verify end-to-end

1. Run the dev server (`npm run dev` / `vite` / etc.).
2. Open the app in Chrome, open DevTools console — confirm the load banner.
3. `vcExt.show()` — every extension-point host on the current view is outlined.
4. Click `Copy snippet` on each → toast appears, clipboard contains a valid
   snippet you can paste into a fresh module file and have it actually register.
5. Navigate to a different view / open a modal / mount a child route — overlays
   auto-refresh; copy still works on every click, including immediately after
   the navigation. **Test specifically: open a new view/modal, then click an
   existing overlay's Copy button. It must still work — this caught a critical
   bug in the AngularJS version.**
6. `vcExt.hide()` — overlays gone, observer + scroll/resize listeners detached
   (verify in DevTools' "Event Listeners" panel or via
   `getEventListeners(window)`).

## Constraints

- No new HTTP endpoint, no new permission, no new dependency.
- Bundle delta ≤ 10 KB minified.
- Pure DOM + framework-injector introspection; no framework component tree
  required at module load (all framework lookups deferred until `vcExt.show()`).
- Dev-mode only: gate the script tag (or import) on the framework's dev flag
  so it never ships in production.

## Out of scope (don't build, but mention in code comments)

- Edit / register / unregister from the inspector UI — read-only by design.
- Cross-frame inspection (iframes, micro-frontends loaded via Module Federation
  remotes) — single-origin host only.
- Persisting overlay state across navigation — `show()` is per-session.

## Deliverables

1. The inspector source file (one self-contained module).
2. A 1-line dev-mode loader (e.g. `if (import.meta.env.DEV) import('./extensionPointInspector')`).
3. A short README section under the file location documenting the console API
   and the list of detected extension points (the table from Phase 1).

Reference (proven implementation, copy the structural ideas):
`src/VirtoCommerce.Platform.Web/wwwroot/js/app/developer-tools/extensionPointInspector.js`
in the vc-platform repo.
