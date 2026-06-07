# Virto Commerce — Module Icon Generation Prompts

Reusable prompts and a shared visual system for generating **module icons** that are consistent
across the platform and let users tell modules apart at a glance by **capability (PBC)**.

- **Style:** **flat, modern, colorful** — a rounded-square tile with a diagonal PBC gradient and a
  bold, multi-color glyph that fills most of the tile, with one **soft drop shadow** for gentle depth.
  No glassmorphism, no photoreal 3D, no glossy/frosted layers. Think clean app-icon/“fluent flat”.
- **Differentiation:** every icon shares the same treatment; only the **PBC tile color** and the
  **glyph** change. Modules in the same PBC share a tile color, so the glyph carries the meaning.
- **Format:** module icons are referenced by `module.manifest` →
  `<iconUrl>Modules/$(<ModuleId>)/Content/logo.svg</iconUrl>` (PNG also supported).
- **Reference set:** `~/Downloads/_icon_set/` contains the full generated set (one `.svg` per module)
  plus `index.html`, a live gallery (size slider, filter, light/dark toggle) — use it as the source
  of truth for the look.

---

## 1. Fixed visual recipe

Applies to every icon:

- **Canvas / viewBox** `0 0 128 128`, 1:1. The whole tile is the icon (no inner inset/border).
- **Tile:** rounded square (squircle) — the root `<rect width="128" height="128" rx="28">`
  (`rx 28` ≈ 22%, a modern app-icon corner).
- **Tile fill:** a **vibrant diagonal linear gradient** of the module’s PBC color, **lighter at
  top-left → deeper at bottom-right** (≈120°: use `x1="0%" y1="0%" x2="92%" y2="100%"`).
- **Glyph:** one clear concept, **filling ~72–86% of the tile** (≈ **14% padding**; the glyph live
  area is roughly `±46` around the center `64,64`). Build it from simple geometric shapes:
  - **White is the primary/base color**, plus **2–3 accent colors** for detail (use the palette
    below). Multi-color is encouraged — the icons should feel lively, not monochrome line-art.
  - A single **soft drop shadow** on the whole glyph group for depth — `feDropShadow dy≈3
    stdDeviation≈3.5`, flood = a dark shade of the tile color at ~0.30 opacity. **No** gloss,
    highlight streaks, frosted panels, or contact shadows.
  - Straight-on perspective. A cylinder/box may show its top ellipse to convey volume (the only
    allowed “3D” cue).
- **No text / no letters** (brand letterforms are the only exception — see §5). Include a `<title>`.
- Must render crisply from **256px down to ~24px**, and hold up on both light and dark backgrounds
  (the colored tile guarantees contrast; keep glyph contrast strong).

### App-icon best practices we follow (Apple HIG + Microsoft)

- One clear focal concept; ≤ 2 metaphors. Strong silhouette, few shapes, generous negative space.
- 1–2 gradient stops only; minimize translucent overlays.
- Use light **and** medium **and** dark values so it reads on any backdrop.
- Bold, fairly uniform stroke weights; avoid thin lines and fussy detail that muddy at small sizes.

---

## 2. PBC color taxonomy (tile gradient: top → base)

White/multi-color glyphs pass contrast on all of these.

| PBC / group | Example modules | Top | Base |
| --- | --- | --- | --- |
| **Core / Platform** | Platform, Core, Backup & Restore, Notifications, Push, Assets, Bulk Actions, Export, File/Frontend API, XAPI | `#5FC8F5` | `#265FC6` |
| **Digital Catalog** | Catalog, Search, Pricing, Inventory, Personalization, Publishing, Store, Associations, XCatalog | `#3FD8B4` | `#0E9E80` |
| **Purchase** (Cart & Checkout) | Cart, Checkout, Payment, Tax, Contracts, XCart | `#FFC24D` | `#ED8A12` |
| **Order Management** | Orders, Shipping, Subscriptions, Returns, XOrder | `#9D86F5` | `#6A45D9` |
| **Customer & Organizations (CRM)** | Customer, Organizations, Customer Reviews | `#F87FAC` | `#DB3E78` |
| **Marketing & Promotions** | Marketing, Promotions | `#F88A8D` | `#E5484D` |
| **Content (CMS)** | Content, Pages, Page Builder, SEO, Sitemaps, Image Tools, XCMS | `#B79DFB` | `#8B5CF6` |
| **PIM** | Profile / PIM experience | `#4FD2EE` | `#0E9BC4` |
| **Identity / Security** | IdP, Security, GDPR | `#6E7E97` | `#3E4C63` |
| **Integration / Connectors** | ERP / payment / shipping / search / SSO connectors | `#5FD080` | `#2FA85A` |
| **Other / default** | uncategorized / extension modules | `#94A6BA` | `#5B6B7E` |

**Accent palette** (pick 2–3 per glyph; they recur across the set so icons feel related):
white `#FFFFFF`, amber `#FFC24D`, sky `#5FC8F5`, teal `#3FD8B4`, green `#2FA85A`, red `#E5484D`,
purple `#9D86F5` / `#6A45D9`, pink `#F87FAC`, neutral line `#9FB6CC` / `#CBD8E6`, navy `#2C3A4E`.

---

## 3. SVG skeleton (copy-paste starting point)

Every icon uses this structure. Fill in `{MODULE_TITLE}`, the gradient stops `{TOP}`/`{BASE}`, the
shadow tint `{SHADOW}` (a dark version of the tile color), and the glyph.

```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 128 128" width="128" height="128"
     role="img" aria-label="{MODULE_TITLE}">
  <title>{MODULE_TITLE}</title>
  <defs>
    <linearGradient id="bg" x1="0%" y1="0%" x2="92%" y2="100%">
      <stop offset="0%"   stop-color="{TOP}"/>
      <stop offset="100%" stop-color="{BASE}"/>
    </linearGradient>
    <filter id="lift" x="-30%" y="-30%" width="160%" height="160%">
      <feDropShadow dx="0" dy="3" stdDeviation="3.5" flood-color="{SHADOW}" flood-opacity="0.30"/>
    </filter>
  </defs>

  <!-- Tile -->
  <rect width="128" height="128" rx="28" fill="url(#bg)"/>

  <!-- Glyph: centered, fills ~72-86%, white base + 2-3 accents -->
  <g transform="translate(64,64)" filter="url(#lift)">
    <!-- ...build the glyph here from rects / circles / paths... -->
  </g>
</svg>
```

Shadow tints used per PBC (a deep shade of the tile): Core `#0B3F66`, Catalog `#064F40`,
Purchase `#7A3D00`, Order `#2C1670`, CRM `#7A1A40`, Marketing `#7A1417`, Content `#3A1D7A`,
PIM `#064E63`, Identity `#10161F`, Integration `#0B4F2A`.

---

## 4. Master prompt — SVG via LLM (recommended)

Fill the placeholders `{MODULE_TITLE}`, `{PBC}`, `{TOP}`, `{BASE}`, `{SHADOW}`, `{GLYPH_CONCEPT}`,
`{ACCENTS}`.

> Generate a Virto Commerce module icon as a **single, self-contained, valid SVG** — output **only**
> the SVG markup, nothing else.
>
> **Style (identical for every module — flat, modern, colorful):**
> - `viewBox="0 0 128 128"`. The tile is the whole icon: `<rect width="128" height="128" rx="28">`
>   filled with a **diagonal linear gradient** from `{TOP}` (top-left) to `{BASE}` (bottom-right)
>   (`x1="0%" y1="0%" x2="92%" y2="100%"`).
> - **One** clear glyph for **{GLYPH_CONCEPT}**, centered via `transform="translate(64,64)"`, filling
>   ~72–86% of the tile (≈14% padding, live area about `±46`). Build it from simple geometric shapes.
> - **White is the base color; add 2–3 accents** from: {ACCENTS}. Make it colorful, not line-art.
> - Apply **one soft drop shadow** to the glyph group via a filter:
>   `feDropShadow dy="3" stdDeviation="3.5" flood-color="{SHADOW}" flood-opacity="0.30"`.
>   No gloss, no highlight streaks, no frosted panels, no contact shadow, no skeuomorphism.
> - Straight-on perspective; a cylinder/box may show a top ellipse for volume. **No text / letters.**
> - Must render crisply 256px→24px and on light/dark backgrounds. Include `<title>{MODULE_TITLE}</title>`.
>
> **This module:** {MODULE_TITLE} — {PBC} PBC. Tile gradient `{TOP}` → `{BASE}`, shadow `{SHADOW}`.
> Glyph: {GLYPH_CONCEPT}.

---

## 5. Connectors & third-party brands

Connector / provider modules use the **Integration green** tile (`#5FD080` → `#2FA85A`).

- **Well-known brands** (Azure AD/Entra, Azure Storage, Azure/AI Search, Google Analytics, Google
  SSO, Elastic, etc.): place the vendor’s **real logo mark in its brand colors** as the glyph on the
  green tile. Prefer the **official vector** when it’s freely available (scale & center it inside the
  glyph group); otherwise redraw the recognizable mark faithfully (e.g. Avalara’s orange “A” with the
  blue check).
- **Contrast helpers** when a mark clashes with green:
  - Mark has its own white background built in (e.g. Elastic) → use as-is.
  - A green-ish/low-contrast mark → keep it direct (default) **or** put it on a small **white rounded
    chip**, or nest the brand’s **original background** as an inset rounded chip behind the mark.
  - A small **badge / accent ring** in white can separate a same-hue glyph from the tile.
- **Generic connectors** (no strong brand: file system, Lucene, etc.): use a generic concept glyph
  (hard drive, search index, …) on the green tile.

**Experience API (GraphQL) family** — `FileExperienceApi`, `ProfileExperienceApi`, `XAPI`, `XCart`,
`XCatalog`, `XCMS`, `XFrontend`, `XOrder`: take the **domain glyph** in its own PBC tile color and add
a small **GraphQL node-graph badge** (a triangle of 3 white dots joined by edges, on a purple
`#6A45D9` circle) in the bottom-right corner. `XAPI` itself uses the node-graph as the hero glyph.
On purple tiles, give the badge a 2px white ring so it separates.

---

## 6. Per-module glyph concepts (the shipped set)

| Module | PBC | GLYPH_CONCEPT |
| --- | --- | --- |
| Backup & Restore | Core | database cylinder + circular restore arrow |
| Application Insights | Core | dashboard panel with multi-color bars + green trend line |
| Assets | Core | open folder with stored documents (file storage) |
| Bulk Actions | Core | stack of item cards + gold lightning “action” badge |
| Core | Core | central hub with multi-color module nodes on spokes |
| Export | Core | container box with an up-&-out arrow |
| Notifications | Core | bell + clapper + red badge dot |
| Push Messages | Core | paper plane with motion trail |
| File Experience API | Core | file + GraphQL node-graph badge |
| XAPI | Core | GraphQL node-graph (hero) |
| XFrontend | Core | storefront browser window + API badge |
| Catalog | Catalog | open box revealing colorful products |
| Catalog CSV Import | Catalog | CSV/grid sheet + import arrow into a tray |
| Catalog Personalization | Catalog | avatar disc + “personalization” sparkles |
| Catalog Publishing | Catalog | product card + green “go-live” up-arrow badge |
| Dynamic Associations | Catalog | central product linked to related product nodes |
| Inventory | Catalog | stacked warehouse crates (with tape) |
| Pricing | Catalog | price tag with “%” |
| Search | Catalog | magnifier revealing a 2×2 grid of result tiles |
| Store | Catalog | storefront with striped awning |
| XCatalog | Catalog | open product box + API badge |
| Cart / Checkout | Purchase | shopping cart with colorful products |
| Payment | Purchase | credit card (chip + contactless) |
| Tax | Purchase | receipt with line items + green “%” badge |
| Contracts | Purchase | document with signature + red wax seal |
| XCart | Purchase | cart + API badge |
| Orders | Order Mgmt | clipboard checklist (check + line items) |
| Return | Order Mgmt | package with a return-loop arrow |
| Shipping | Order Mgmt | delivery truck with motion lines |
| Subscription | Order Mgmt | calendar + recurring-cycle badge |
| XOrder | Order Mgmt | order clipboard + API badge |
| Customer | CRM | two overlapping people (colored shirts + collars) |
| Customer Reviews | CRM | review speech bubble + gold star rating |
| Marketing | Marketing | megaphone with sound waves |
| Content | Content | page with hero + content section blocks |
| Pages | Content | stacked pages with a folded corner |
| Page Builder | Content | layout blocks + “add” (＋) badge |
| SEO | Content | magnifier with a rising ranking trend |
| Sitemaps | Content | sitemap hierarchy tree of nodes |
| Image Tools | Content | photo + image-adjustment sliders badge |
| XCMS | Content | CMS page + API badge |
| Profile Experience API | PIM | profile card (avatar) + API badge |
| GDPR | Identity | shield + padlock + EU-style stars |
| Connectors (Azure*, Google*, Elastic, Avalara, Authorize.Net, WebHooks, FS/Lucene) | Integration | brand mark or connector concept (see §5) |

---

## 7. How to apply

1. Pick the **PBC tile color** (and shadow tint) from §2, and a **glyph concept** (§6 or new).
2. Generate the SVG from the §3 skeleton / §4 prompt. Keep it small and self-contained.
3. Save to `src/<Module>.Web/Content/logo.svg`.
4. Point the manifest at it: `<iconUrl>Modules/$(<ModuleId>)/Content/logo.svg</iconUrl>`.
5. **Verify** at large size **and** at ~32–40px (module-list size): the glyph stays legible, the tile
   color matches the PBC, corners read as a squircle, the shadow reads as gentle depth (not noise),
   and it works on light **and** dark backgrounds. The `_icon_set/index.html` gallery is handy for
   eyeballing all sizes/themes at once.

> Reference implementations: the full set in `~/Downloads/_icon_set/*.svg`, and
> `vc-module-backup-restore/src/VirtoCommerce.BackupRestore.Web/Content/logo.svg`.
