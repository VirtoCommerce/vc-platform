You are creating a single, self-contained HTML presentation in the \*\*Virto Commerce deck style\*\*.

Output ONE complete `.html` file (inline CSS + inline JS, no build step, no external JS).



\# TOPIC

{{ONE-LINE TOPIC, e.g. "Migrating Acme from Magento to Virto Commerce 3.x"}}



\# AUDIENCE

{{e.g. "Client — business stakeholders + solution architects"}}



\# PRESENTER (optional)

{{Name · Role · email}}   |   LOGO (optional): {{logo SVG url or inline}}



\# CONTENT / SECTIONS

Organize into 5–7 sections. Provide the narrative; I'll list candidate sections:

{{e.g. Intro → Today → Target → Capability mapping → Way of working → Plan/Value → Closing}}

Add an "Executive summary" as slide 2 and a "Thank you/CTA" as the final slide.



\# ─────────────────────────────────────────────

\# DESIGN SYSTEM (use EXACTLY — Virto brand)

\# ─────────────────────────────────────────────

Dark, presentation-grade theme on Virto deep-indigo. CSS :root tokens:

&#x20; --bg-dark:#1D164D;  --bg-darker:#130E33;  --bg-card:#2A2161;  --bg-card-hover:#352873;

&#x20; --border:#403A78;   --text-primary:#FAF8F6; --text-secondary:#B8B2D6; --text-muted:#807AA3;

&#x20; --accent:#E57F44 (Virto orange);  --accent-soft:rgba(229,127,68,0.14);  --accent-glow:rgba(229,127,68,0.40);

&#x20; --cyan:#5B8DEF (blue); --green:#22C55E; --amber:#FFC96B (gold); --orange:#FF932E; --purple:#9B86D9; --magenta:#F472B6;

Accent gradient (titles): linear-gradient(135deg, var(--accent) 0%, #FFC96B 100%) with -webkit-background-clip:text.

Fonts (Google): headings/body 'CircularXX','Manrope','Inter',sans-serif; mono 'JetBrains Mono'.

Load: https://fonts.googleapis.com/css2?family=Manrope:wght@400;500;600;700;800\&family=Inter:wght@400;500;600;700\&family=JetBrains+Mono:wght@400;500;600\&display=swap



\# LAYOUT ENGINE (replicate exactly)

\- A fixed `.stage` centers a `.slide` that is \*\*1200×720px\*\*, border-radius 24px, padding 56px 64px, with box-shadow.

\- Each slide is a `<section class="slide" data-section="GroupName" data-title="Slide title">`. Only `.active` is visible (opacity/visibility toggle).

\- Slides auto-scale to viewport via a JS `--fit-scale` (min((vw-32)/1200,(vh-32)/720,1)).

\- Bottom-center pill nav (prev/counter/next), top-left hamburger TOC button, top-right kbd hint.

\- Left TOC panel auto-built from slides, grouped by `data-section`, with the section label inserted whenever `data-section` changes; clicking an item jumps to it.

\- Keyboard: ←/→ + Space/PageUp/PageDown navigate, Home/End jump, T toggles TOC, F fullscreen, Esc closes TOC.

\- JS: build TOC, show(i), goTo/next/prev, keydown handler, openToc/closeToc, fit(), init show(0). Counter sets totIdx = slides.length.



\# SLIDE TYPES (use the right one per slide)

\- `.slide.cover` — radial-gradient bg; optional inline logo (`.cover-logo` \~38px), `.cover-tag` pill, big `.cover-title` (72px, .accent span), `.cover-subtitle`, `.cover-chips` (chip with <strong> accent), `.cover-meta` footer (presenter left).

\- Executive summary — `.grid-3` of three `.psf-card` (.problem red / .solution orange / .features cyan), each: `.psf-label` (num badge), h3, bulleted `ul`.

\- `.slide.section` — divider: `.section-num` ("SECTION 01"), big `.section-title`, `.section-blurb`. Add `data-toc-divider="true"`.

\- Content slide — `.breadcrumb` (uppercase accent), `h1.slide-title` (44px, .accent span), `.slide-lead`, then body.

\- `.slide.cta` — centered closing; `.cta-title` (80px), subtitle, `.cta-links`, presenter footer.



\# COMPONENTS (reuse these classes)

\- `.grid-2/.grid-3/.grid-4` layout grids.

\- `.card` (h3 with `.icon-dot`, p, `ul`); `.card.muted` for callouts.

\- `table.compare` (uppercase th, row borders, `td.code` mono-cyan) — ideal for "legacy → new" mappings.

\- `.pill.green/.amber/.cyan/.purple/.orange/.red` status tags.

\- `pre.snippet` with `.kw/.str/.com/.key/.num` syntax spans.

\- `.schema` container holding inline \*\*SVG diagrams\*\* (architecture, flows, sequences). SVG helpers: `rect.box / .box-accent / .box-cyan / .box-purple / .box-green`, `text.lbl / .lbl-sm / .mono-svg`, `line.arrow / .arrow-accent`, and `<marker>` arrowheads (give each marker a UNIQUE id if a diagram is duplicated). Keep SVGs inside the 1200×720 frame; use viewBox.

\- Timeline/phase cards (`.phase`), and optional accents like a "workshop band" or "cadence equation" if the topic needs a process/roadmap slide.



\# PDF / PRINT (include this so the deck exports cleanly)

Add a `@media print` block: `@page{size:12.5in 7.5in;margin:0}`; hide nav/TOC/kbd; make `.stage` static and every `.slide` `position:relative;opacity:1;visibility:visible;transform:none;page-break-after:always` (last slide auto); set `print-color-adjust:exact` on body so dark backgrounds render. (Export via headless Chrome/Edge `--headless=new --no-pdf-header-footer --print-to-pdf=out.pdf url`, or Ctrl+P → Save as PDF, landscape, margins None, Background graphics on.)



\# RULES

\- Keep every slide within the 1200×720 frame — NO vertical overflow (verify `scrollHeight === clientHeight`). Prefer fewer words, cards, tables, and diagrams over paragraphs.

\- \~15–24 slides. Lead with an executive summary; use section dividers between groups.

\- Use SVG schema diagrams for architecture/flows rather than bullet lists where it adds clarity.

\- Brand-accurate: orange `--accent` for emphasis, gold for gradients, blue/purple/green/amber as secondary accents only.

\- Output the full HTML file only.

