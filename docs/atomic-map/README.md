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
| `snippet` | | `{ lang, code }`; taken from or written against the cited file |
| `note` | | shown prominently — use for migration notes on `in-flight` atoms |
| `useInstead` | | the alternative; expected on `available` and `legacy` atoms |
| `docs` | | `[{ label, href }]`; `href` is relative to this folder |
| `seeAlso` | | array of atom ids |
| `molecule` | | a molecule id this atom belongs to |
| `keystone` | | `true` draws a heavier border (currently only `AbstractTypeFactory`) |
| `tags` | | extra search terms that are not in the prose |

Backticks inside any text field render as inline `<code>`.

`app.js` validates the required fields at render time and paints a red `⚠` on any incomplete tile,
with the specific problems listed in its drawer. Broken content fails loudly rather than rendering
blank.

---

## Adoption badges — the part that carries the value

The badge turns a catalog into guidance. Assign it from evidence, not impression.

| Badge | Meaning | How to justify it |
|---|---|---|
| ● `platform` | Platform-native. The Virto way. | at least one real call site in `src/` |
| ◐ `module` | Ships in an installable module, not platform core. | name the module |
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
