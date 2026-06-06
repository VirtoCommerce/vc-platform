# Virto Commerce — Module Icon Generation Prompts

Reusable prompts and a shared visual system for generating **module icons** that are consistent
across the platform and let users tell modules apart at a glance by **capability (PBC)**.

- **Style:** Glassmorphism + dimensional 3D + vibrant gradient — frosted/translucent layers, soft
  depth, glossy highlights. Inspiration: the Microsoft Office (Word) 3D icon redesign, glassmorphism
  icon sets, and liquid-gradient logos.
- **Differentiation:** every icon shares the same treatment; only the **PBC color** and the **glyph**
  change.
- **Format:** module icons are referenced by `module.manifest` →
  `<iconUrl>Modules/$(<ModuleId>)/Content/logo.svg</iconUrl>` (PNG also supported).

---

## 1. Fixed visual recipe

Applies to every icon (vector or raster):

- **Canvas** `512×512`, 1:1. **Tile:** rounded square ("squircle") inset `x16 y16`, `480×480`,
  `rx≈112`.
- **Tile fill:** vibrant **diagonal gradient** of the module's PBC color — lighter at top-left,
  deeper at bottom-right — plus a soft top-left corner **sheen**.
- **Frosted glass panel** behind the glyph: translucent white rounded rect (`rx≈72`), a 2px white
  edge at ~0.55 opacity, and a fading **top reflection** highlight.
- **Glyph:** exactly **one** bold, instantly recognizable symbol, white, centered in a ~320×320 live
  area. Give it **volume** (body gradient `#FFFFFF → light tint`, side-shading, glossy top, a soft
  left highlight streak), a **soft drop shadow** (`dy≈14`, `blur≈16`, navy ~0.30), and a blurred
  **contact shadow** beneath it. Optional: an **offset translucent secondary shape** behind the glyph
  for extra layered depth (as in glassmorphism sets).
- Must render crisply at **32px and 512px**, hold up in light/dark themes, include `<title>`, and
  contain **no text**.

---

## 2. PBC color taxonomy (tile gradient: top → base; mid auto-interpolated)

Headline groups first; extended set covers the rest. White glyph passes contrast on all.

| PBC / group | Example modules | Top | Base |
| --- | --- | --- | --- |
| **Core / Platform** | Platform, **Backup & Restore**, Notifications, Settings | `#5FC8F5` | `#265FC6` |
| **Digital Catalog** | Catalog, Search, Pricing, Inventory, Personalization, Store | `#3FD8B4` | `#0E9E80` |
| **Purchase** (Cart & Checkout) | Cart, Checkout, Payment, Quote | `#FFC24D` | `#ED8A12` |
| **Order Management** | Orders, Shipping, Subscriptions | `#9D86F5` | `#6A45D9` |
| **Other / default** | uncategorized / extension modules | `#94A6BA` | `#5B6B7E` |
| Customer & Organizations (CRM) | Customer, Organizations | `#F87FAC` | `#DB3E78` |
| Marketing & Promotions | Marketing, Promotions | `#F88A8D` | `#E5484D` |
| Content (CMS) | Content, Page Builder | `#B79DFB` | `#8B5CF6` |
| PIM | Profile / PIM experience | `#4FD2EE` | `#0E9BC4` |
| Identity / Security | IdP, Security, Azure AD | `#6E7E97` | `#3E4C63` |
| Integration / Connectors | ERP / payment / shipping connectors | `#5FD080` | `#2FA85A` |

**Backup & Restore → Core / Platform** (`#5FC8F5` → `#265FC6`).

---

## 3. Master prompt — SVG via LLM (recommended)

Fill the placeholders `{MODULE_TITLE}`, `{PBC}`, `{COLOR_TOP}`, `{COLOR_BASE}`, `{GLYPH_CONCEPT}`.

> Generate a Virto Commerce module icon as a **single, self-contained, valid SVG** — output **only**
> the SVG markup, nothing else.
>
> **Style (identical for every module — glassmorphism + dimensional 3D):**
> - `viewBox="0 0 512 512"`. A **squircle tile** inset at `x16 y16`, `480×480`, `rx=112`.
> - Tile fill = a **vibrant diagonal linear gradient** from `{COLOR_TOP}` (top-left) to `{COLOR_BASE}`
>   (bottom-right), plus a soft top-left radial white **sheen**.
> - A **frosted glass panel** behind the glyph: translucent white rounded rect (`rx≈72`) with a 2px
>   white edge (~0.55 opacity) and a fading white **top reflection**.
> - **One** bold, instantly recognizable glyph for **{GLYPH_CONCEPT}**, centered in a ~320×320 live
>   area, rendered with **volume**: body gradient `white → light tint`, subtle right-side shading, a
>   glossy top, and a soft left highlight streak. Add a **soft drop shadow** (`dy≈14`, `blur≈16`,
>   navy, ~0.30) and a blurred **contact shadow** beneath it.
> - Use `linearGradient`/`radialGradient` for volume, `feDropShadow`/`feGaussianBlur` for soft
>   shadows, and translucent white fills + thin light borders for the glass. No text, no photoreal
>   raster effects, no skeuomorphism.
> - Must render crisply at 32px and 512px, hold up in light/dark themes, and include
>   `<title>{MODULE_TITLE}</title>`.
>
> **This module:** {MODULE_TITLE} — {PBC} PBC. Tile gradient `{COLOR_TOP}` → `{COLOR_BASE}`.
> Glyph: {GLYPH_CONCEPT}.

---

## 4. Master prompt — Raster 3D (Midjourney / DALL·E / SDXL)

Use when you want the full photoreal 3D-render look; export a 1:1 PNG and place it in `Content/`.

> A modern **3D app icon** for the "{MODULE_TITLE}" software module. Rounded-square (squircle) tile
> with a **vibrant {PBC_COLOR_NAME} glassmorphism gradient** (`{COLOR_TOP}` → `{COLOR_BASE}`), frosted
> translucency, soft inner light and glossy reflections. Centered glyph: **{GLYPH_CONCEPT}**, glossy
> white with smooth 3D volume, layered depth, soft drop shadows and highlights — premium fluid 3D
> style like the Microsoft Office icon redesign and glassmorphism icon sets. 1:1 square, centered,
> app-icon composition, clean neutral background, high detail. **No text.**
>
> Negative/avoid: text, letters, watermark, busy background, flat clipart, harsh edges.

---

## 5. Per-module glyph concepts (example fills)

| Module | PBC (top → base) | GLYPH_CONCEPT |
| --- | --- | --- |
| **Backup & Restore** | Core (`#5FC8F5`→`#265FC6`) | glossy 3D database cylinder with two recessed bands, wrapped by a bold circular restore/refresh arrow with a clean arrowhead |
| Catalog | Digital Catalog (`#3FD8B4`→`#0E9E80`) | open box revealing a grid of products / stacked product tags |
| Pricing | Digital Catalog (`#3FD8B4`→`#0E9E80`) | price tag with a "%" or a coin |
| Inventory | Digital Catalog (`#3FD8B4`→`#0E9E80`) | stacked warehouse crates / boxes |
| Search | Digital Catalog (`#3FD8B4`→`#0E9E80`) | magnifier over a list |
| Orders (OMS) | Order Management (`#9D86F5`→`#6A45D9`) | clipboard / checklist with a checkmark |
| Shipping | Order Management (`#9D86F5`→`#6A45D9`) | delivery truck or box with motion lines |
| Cart & Checkout | Purchase (`#FFC24D`→`#ED8A12`) | shopping cart |
| Payment | Purchase (`#FFC24D`→`#ED8A12`) | credit card with a chip |
| Customer / Organizations | CRM (`#F87FAC`→`#DB3E78`) | two overlapping person silhouettes / org nodes |
| Marketing & Promotions | Marketing (`#F88A8D`→`#E5484D`) | megaphone or tag with a star |
| Content / Page Builder | Content (`#B79DFB`→`#8B5CF6`) | layout blocks / document with sections |
| Notifications | Core (`#5FC8F5`→`#265FC6`) | bell |

---

## 6. How to apply

1. Generate the icon (SVG via §3, or PNG via §4).
2. Save it to `src/<Module>.Web/Content/logo.svg` (or `logo.png`).
3. Ensure the manifest points at it:
   `<iconUrl>Modules/$(<ModuleId>)/Content/logo.svg</iconUrl>`.
4. **Verify** by opening the file in a browser at large size **and** scaled to ~32px (module-list
   size): the glyph stays legible, the tile color matches the PBC, corners look like a squircle, and
   the gloss/shadows read as depth (not noise).

> Reference implementation: `vc-module-backup-restore/src/VirtoCommerce.BackupRestore.Web/Content/logo.svg`.
