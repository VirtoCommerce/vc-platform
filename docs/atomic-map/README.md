# Atomic Architecture Map

An interactive one-screen map of the building blocks a Virto Commerce developer works with: a
**periodic table of atoms** under a **solution architecture band**, with a reserved shelf for
composite topics (**molecules**).

It answers three questions without reading source:

1. What primitives does the platform give me?
2. Which one should I reach for — and which should I not?
3. Where does it sit in the stack?

**Open it:** double-click `index.html`. No build, no npm, no server, no network access. It also
publishes with the docs site (MkDocs `awesome-pages` picks the folder up with no `mkdocs.yml`
change).

Design rationale: [`../superpowers/specs/2026-07-30-atomic-architecture-map-design.md`](../superpowers/specs/2026-07-30-atomic-architecture-map-design.md).

---

## Layout

```
index.html            poster shell, legend, drawer skeleton
styles.css            grid, tiles, drawer, light/dark, @media print
app.js                render · search · filter · drawer · hash routing · schema check
check-content.js      reference-integrity checker (node check-content.js)
content/
  meta.js             platform version + last sweep date
  architecture.js     the six architecture layers
  atoms.js            families + every atom — this is where you will spend your time
  molecules.js        reserved composite topics
```

**Renderer and content are decoupled on purpose.** Adding or correcting an atom means editing one
object in `content/atoms.js`. You should never need to touch `app.js`.

---

## Adding or updating an atom

1. Add an object to `window.VC_MAP_ATOMS` in `content/atoms.js`.
2. **Read the source file you are describing.** Every `api[].file` path and every member named in a
   snippet must exist. This map is only worth having if it is true.
3. Run the checker:

```bash
node docs/atomic-map/check-content.js
```

4. Open `index.html` and click the tile.

### Field contract

| Field | Required | Notes |
|---|---|---|
| `id` | ✔ | kebab-case slug; becomes the deep link `#/atom/<id>` |
| `symbol` | ✔ | 2–3 characters, unique across all atoms |
| `name` | ✔ | short label on the tile |
| `family` | ✔ | must match a `VC_MAP_FAMILIES` id |
| `adoption` | ✔ | `platform` · `module` · `available` · `in-flight` · `legacy` |
| `layer` | ✔ | must match a `VC_MAP_ARCHITECTURE` id; drives the hover spotlight |
| `oneLiner` | ✔ | one sentence; the lead line in the drawer |
| `pattern` | ✔ | the architectural pattern, named |
| `whenToUse` | ✔ | array; concrete situations, not restatements of the name |
| `api` | ✔ | `[{ name, file }]`; `file` is repo-relative, or a `(parenthesised annotation)` when there deliberately is no file |
| `verifiedAgainst` | ✔ | platform version you checked against; must equal `meta.js` |
| `avoid` | | array; where this is the wrong tool |
| `gotchas` | | array; the things that cost someone a day |
| `snippet` | | `{ lang, code }`; taken from or written against the cited file. `lang` is `csharp`, `json`, `bash` or `xml` — it selects the highlighter and the label, and an unknown value renders as plain text rather than failing |
| `note` | | shown prominently — use for migration notes on `in-flight` atoms |
| `useInstead` | | the alternative; expected on `available` and `legacy` atoms |
| `docs` | | `[{ label, page }]` — see **Documentation links** below |
| `seeAlso` | | array of atom ids |
| `molecule` | | a molecule id this atom belongs to |
| `keystone` | | `true` draws a heavier border (currently only `AbstractTypeFactory`) |
| `tags` | | extra search terms that are not in the prose |

In any text field, backticks render as inline `<code>` and `**double stars**` as `<strong>`. Bold is
the outer split, so ``**`Something`**`` works. Pair your markers — an odd number of `**` in one
string bolds the tail.

`app.js` validates the required fields at render time and paints a red `⚠` on any incomplete tile,
with the specific problems listed in its drawer. Broken content fails loudly rather than rendering
blank.

---

## Architecture-band layers

Layers live in `content/architecture.js` and use `name`, `hue`, `sub`, `tags`, `bullets`,
`gotchas`, `docs`, plus two optional structures:

- **`matrix`** (with `matrixTitle`) — a name/description table, e.g. the module-type list.
- **`diagrams`** — an ordered array of diagrams; `kind` picks the renderer. `Your solution`
  carries three: Solution architecture (`lanes`), DevOps (`pipeline`), Deployment schema
  (`flow`). Channels carries one (`stack`).

**`kind: 'stack'`** — a vertical stack of rows joined by connector pills. A row is either a
group of nodes or the `target`, so the target can sit anywhere: Channels puts the platform in
the middle, with sales channels calling down into it and back office calling up.

```js
{ kind: 'stack', title: 'Channels → platform', rows: [
    { title: 'Sales channels', hint: 'read-heavy', nodes: [
        { name: 'Storefront', sub: 'Vue 3', via: 'GraphQL', viaKind: 'graphql' },
        { name: 'AI agents', sub: 'Emerging', via: 'MCP → UCP', viaKind: 'trend', trend: true } ]},
    { connector: 'XAPI GraphQL', connectorDir: 'down', target: 'Platform', sub: '…' } ]}
```

- `viaKind` is `graphql` · `rest` · `trend` · `plain` and colours the protocol chip.
- `connectorDir` is `down` · `up` · `both` (↓ ↑ ↕). Point it at whoever is *called*, so the
  diagram shows dependency direction rather than adjacency.
- `trend: true` draws the node dashed — the "not shipped yet" language of the molecule tiles.
- Protocol chips pin to the bottom of their card so they align on one baseline per row.

**`kind: 'flow'`** — left-to-right tiers of cards, optionally grouped into dashed clusters with
a badge, arrows between tiers, and an optional legend. Structure follows
[vc-module-solution-architecture-map](https://github.com/VirtoCommerce/vc-module-solution-architecture-map)'s
tier/node/cluster vocabulary.

```js
{ kind: 'flow', title: 'Solution architecture',
  legend: [{ kind: 'oob', label: 'Out-of-the-box module' }],
  tiers: [
    { label: 'Presentation', nodes: [{ name: 'Storefront SPA', kind: 'custom', role: '…', meta: 'CDN' }] },
    { arrow: true },
    { label: 'Modules', clusters: [
        { title: 'Out of the box', chip: '≈80% standard base', nodes: [ /* … */ ] } ]}
  ]}
```

- Node `kind` is `virto` · `oob` · `custom` · `data` · `infra` and colours the left border —
  that border *is* the legend, so keep the legend in step (the checker rejects a legend entry
  no node uses).
- A flow scrolls horizontally inside its own box rather than wrapping; a wrapped tier flow
  stops reading as a flow.
- **No status dots.** They mean live health in the module this borrows from; an all-green
  static poster would imply monitoring this page does not do.

The checker rejects unknown `kind`, `viaKind` or `connectorDir` values, tiers or rows without
nodes, a stack without exactly one target, a stale legend entry, and the old `schema` field.

**`kind: 'lanes'`** — swimlanes for isolated paths: columns are stages, rows are paths. A
column marked `shared` spans every lane, which is where the paths converge. A `scope` renders
like a Cell in the Atomic Architecture diagram — a bounded box whose members are named modules.

```js
{ kind: 'lanes', title: 'Solution architecture',
  columns: [
    { label: 'Presentation' },
    { label: 'Modules', shared: true, scopes: [
        { title: 'Virto Commerce Modules', chip: '≈80%', accent: 'virto',
          count: '100+ modules', modules: ['Catalog', 'Pricing'] } ]}
  ],
  lanes: [
    { label: 'Customer', chip: 'public', accent: 'shopper',
      cells: [ { nodes: [ /* one entry per laned column */ ] } ] }
  ]}
```

- Laned columns must come **before** shared ones, and each lane needs one cell per laned
  column — the checker enforces both, since either mistake silently misplaces grid cells.
- `count` overrides the chip tally when the chips are only a sample of a larger set.
- Cards are a fixed size (196×84) so the diagram reads as a grid rather than a collage.

**`kind: 'pipeline'`** — the compact CI/CD view: parallel source lanes converging into
full-width steps, joined by small uppercase pills. Follows the release-strategy deck's
`fl-lane` / `fl-conn` / `fl-lbl` shape.

```js
{ kind: 'pipeline', title: 'DevOps',
  lanes: [
    { label: 'Yours', steps: [
        { name: 'Your Git', kind: 'src', sub: 'custom source' },
        { connector: 'CI build' },
        { name: 'Custom modules', kind: 'custom', sub: 'Artifact storage' } ]}
  ],
  rows: [
    { connector: 'both storages feed →', name: '`vc-package.json`', kind: 'select' },
    { connector: 'deploy', name: 'Environment', kind: 'env' }
  ]}
```

Node `kind` is `src` · `custom` · `virto` · `select` · `image` · `env`. Lanes must have equal
step counts — uneven ones leave the converging pill misaligned against one lane, so the checker
rejects them.

## Documentation links

Documentation lives on **https://docs.virtocommerce.org**, which is built from the
[vc-docs](https://github.com/VirtoCommerce/vc-docs) repo — *not* from this repo's `docs/` folder.
So a doc link stores the vc-docs page path and the renderer derives the URL:

```js
docs: [
  { label: 'Essential caching', page: 'Fundamentals/Caching/01-overview' },   // → docs site
  { label: 'vc-module-background-jobs', href: 'https://github.com/...' },     // → external
  { label: 'Design spec', path: 'superpowers/specs/2026-06-06-....md' }       // → in-repo file
]
```

- **`page`** — path under `platform/developer-guide/docs/` in vc-docs, without `.md`. The base URL
  and the `latest` version segment live in one constant (`DOCS_BASE` in `app.js`).
- **`href`** — a fully external URL.
- **`path`** — a file in this repo, relative to `docs/`. Only for in-repo material such as a design
  spec; user documentation belongs on the docs site.

Exactly one of the three per entry. Do not hand-write `docs.virtocommerce.org` URLs in content —
use `page`, so a site restructure is a one-line change.

**Verify the pages exist:**

```bash
node docs/atomic-map/check-content.js --online
```

`--online` pulls the vc-docs file list via the `gh` CLI and fails on any `page` that is not a real
file. Without it the checker validates only the shape. Run it after adding links.

## Adoption badges — the part that carries the value

The badge turns a catalog into guidance. Assign it from evidence, not impression.

| Badge | Meaning | How to justify it |
|---|---|---|
| ● `platform` | Platform-native. The Virto way. | at least one real call site in `src/` |
| ◐ `module` | Ships outside platform core — install the module or tool. | name the module id or package, and check it in the [vc-modules registry](https://raw.githubusercontent.com/VirtoCommerce/vc-modules/master/modules_v3.json) |
| ○ `available` | .NET offers it; this platform does not use it. | a grep that finds no call sites — say so in `note`, and set `useInstead` |
| △ `in-flight` | Changing right now. | link the design spec in `docs`, explain in `note` |
| ✕ `legacy` | Works, but do not build on it. | the `[Obsolete]` attribute or the source comment that says so |

An ○ or ✕ badge with no `useInstead` is an incomplete atom: you have told the reader to stop without
telling them where to go.

---

## Keeping it honest over time

The real failure mode of a living cheat-sheet is silent staleness, so age is made visible rather
than assumed:

- **`verifiedAgainst`** on each atom records the platform version it was last checked against, and
  the footer surfaces it.
- When you re-check an atom, bump its `verifiedAgainst`.
- When you sweep the whole file, bump `platformVersion` and `updated` in `content/meta.js`.
- `check-content.js` fails if any atom's `verifiedAgainst` disagrees with `meta.js`, which forces the
  two to be reconciled deliberately instead of drifting.

**Check the branch you are describing.** During authoring, several atoms initially cited
`AdminUIAccess*` types that exist only on an unmerged feature branch — `check-content.js` caught it.
Content should describe `dev` unless it is explicitly marked `in-flight`.

---

## Molecules

Reserved placeholders, rendered outlined rather than filled so the shape of the whole picture is
visible while the gaps stay honest. To promote one, write its content and give it a real page; the
atoms it composes are already listed in its `atoms` array.

---

## Constraints worth knowing before you edit

- **Classic scripts, not ES modules.** Content loads via `<script src>` assigning to `window.*`.
  ES modules are CORS-blocked on the `file://` origin, which would break double-click-to-open.
- **No network at runtime.** No CDN, no webfonts, no `fetch`. Keep it that way; it is what makes the
  page work from a filesystem, an air-gapped machine and the docs site alike.
- **DOM is built with an `el()` helper, not `innerHTML`.** C# snippets are full of generics like
  `IBackgroundJobHandler<TPayload>`; string interpolation into `innerHTML` would mangle them.
- **Syntax highlighting is a ~40-line tokenizer in `app.js`**, not a library. Every token becomes a
  span whose text is set via `textContent`, so it cannot corrupt code or inject markup — a
  mis-tokenization can only colour something oddly. It must stay **lossless**: the rendered text of
  every snippet is byte-identical to its source. Worth re-checking if you touch it.
- **Don't put `white-space` on the bare `code` selector.** Inline chips need `normal` and `<pre>`
  code needs `pre`; an unscoped rule collapses every snippet onto one line. The inline rules are
  scoped `:not(pre) > code` for exactly this reason.
- **Colour comes from one `hue` number per family.** Adding a family means adding a hue in
  `content/atoms.js` — no CSS change.

## The details panel

Clicking a tile opens a side panel. The `⤢` button in its header (or `f`) expands it to full
screen, which reflows the sections into columns — useful for atoms with long snippets or several
lists. Snippets, lead text and notes always span the full width; only the short lists column up.
The choice sticks across atoms and reloads, and the button is hidden on narrow screens where the
panel already fills the viewport.

## Keyboard

`/` search · `←→↑↓` move between tiles · `Enter` open · `Esc` close or clear · `f` full-screen
panel · `?` legend

`Ctrl`/`Cmd`+`P` prints the poster in landscape with the legend and without the drawer — the paper
version to pin above a desk. It fits **one page on A3**; on A4 it runs to two, breaking between
tiles rather than through them. No paper size is hard-coded, so the layout follows whatever is
loaded.
