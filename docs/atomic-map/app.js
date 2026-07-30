/* Virto Commerce — Atomic Architecture Map · renderer
 *
 * Deliberately dependency-free and content-agnostic: everything it draws comes from
 * content/*.js. Adding or correcting an atom never means touching this file.
 *
 * DOM is built with the el() helper rather than innerHTML — C# snippets are full of
 * generics like IBackgroundJobHandler<TPayload>, and string interpolation into innerHTML
 * would either mangle them or open an injection hole.
 */
(function () {
  'use strict';

  // ---------------------------------------------------------------- content

  var META = window.VC_MAP_META || {};
  var LAYERS = window.VC_MAP_ARCHITECTURE || [];
  var FAMILIES = window.VC_MAP_FAMILIES || [];
  var ATOMS = window.VC_MAP_ATOMS || [];
  var MOLECULES = window.VC_MAP_MOLECULES || [];

  var ADOPTION = {
    'platform':  { glyph: '●', label: 'Platform',  cls: 'adopt-platform',  blurb: 'Platform-native. This is the Virto way.' },
    'module':    { glyph: '◐', label: 'Module',    cls: 'adopt-module',    blurb: 'Ships outside platform core — install the module or tool.' },
    'available': { glyph: '○', label: 'Available', cls: 'adopt-available', blurb: '.NET offers it; this platform does not use it.' },
    'in-flight': { glyph: '△', label: 'In flight', cls: 'adopt-inflight',  blurb: 'Changing right now — read the migration note.' },
    'legacy':    { glyph: '✕', label: 'Legacy',    cls: 'adopt-legacy',    blurb: 'Still works; do not build new code on it.' }
  };
  var ADOPTION_ORDER = ['platform', 'module', 'in-flight', 'legacy', 'available'];

  var REQUIRED = ['id', 'symbol', 'name', 'family', 'adoption', 'layer', 'oneLiner', 'pattern', 'whenToUse', 'api'];

  /* Documentation lives on the public docs site, which is built from the vc-docs repo
     (github.com/VirtoCommerce/vc-docs). Content stores the page path only —
     `Fundamentals/Caching/01-overview` — and the URL is derived here, so the base and the
     version segment are defined in exactly one place. */
  var DOCS_BASE = 'https://docs.virtocommerce.org/platform/developer-guide/latest/';

  function docHref(doc) {
    if (doc.href) return doc.href;                       // fully external (GitHub, ucp.dev)
    if (doc.page) return DOCS_BASE + doc.page + '/';     // vc-docs page
    if (doc.path) return '../' + doc.path;               // in-repo file, e.g. a design spec
    return null;
  }

  function docLinks(docs) {
    var usable = (docs || []).filter(function (doc) { return docHref(doc); });
    if (!usable.length) return null;
    return el('div', { class: 'd-links' }, usable.map(function (doc) {
      var href = docHref(doc);
      return el('a', {
        href: href,
        text: doc.label,
        // Off-site links open in a new tab so the map is not navigated away from.
        target: /^https?:/.test(href) ? '_blank' : null,
        rel: /^https?:/.test(href) ? 'noopener' : null
      });
    }));
  }

  // ---------------------------------------------------------------- helpers

  function el(tag, props) {
    var node = document.createElement(tag);
    if (props) {
      Object.keys(props).forEach(function (key) {
        var value = props[key];
        if (value === null || value === undefined || value === false) return;
        if (key === 'class') node.className = value;
        else if (key === 'text') node.textContent = value;
        else if (key === 'dataset') Object.keys(value).forEach(function (d) { node.dataset[d] = value[d]; });
        else if (key.slice(0, 2) === 'on') node.addEventListener(key.slice(2).toLowerCase(), value);
        else node.setAttribute(key, value === true ? '' : value);
      });
    }
    for (var i = 2; i < arguments.length; i++) append(node, arguments[i]);
    return node;
  }

  function append(node, child) {
    if (child === null || child === undefined || child === false) return;
    if (Array.isArray(child)) { child.forEach(function (c) { append(node, c); }); return; }
    node.appendChild(child.nodeType ? child : document.createTextNode(String(child)));
  }

  /** Renders `backticked` spans as <code> and **double-starred** spans as <strong>.
   *  No HTML is ever parsed — every case builds elements and sets text via textContent.
   *
   *  Bold is the OUTER split so that **`Something`** works. Splitting backticks first
   *  separates the two ** markers into different segments, which breaks the pairing and
   *  silently bolds everything up to the next marker instead of the intended phrase.
   *  Caveat: an odd number of ** in one string bolds the tail — pair your markers. */
  function rich(text) {
    var frag = document.createDocumentFragment();
    String(text).split('**').forEach(function (part, i) {
      if (part === '') return;
      var bold = i % 2 === 1;
      var host = bold ? el('strong', {}) : frag;
      part.split('`').forEach(function (chunk, j) {
        if (chunk === '') return;
        host.appendChild(j % 2 ? el('code', { text: chunk }) : document.createTextNode(chunk));
      });
      if (bold) frag.appendChild(host);
    });
    return frag;
  }

  function byId(list, id) {
    for (var i = 0; i < list.length; i++) if (list[i].id === id) return list[i];
    return null;
  }

  function hueOf(familyId) {
    var family = byId(FAMILIES, familyId);
    return family ? family.hue : 220;
  }

  function adoptionOf(id) {
    return ADOPTION[id] || { glyph: '⚠', label: 'Unknown', cls: 'adopt-schema', blurb: 'Unrecognised adoption state.' };
  }

  // ---------------------------------------------------------------- schema check

  /* Content that is missing required fields must fail loudly. A silently blank tile in a
     reference map is worse than no tile: it teaches the reader something false by omission. */
  function schemaProblems(atom) {
    var missing = REQUIRED.filter(function (field) {
      var value = atom[field];
      return value === undefined || value === null || value === '' ||
             (Array.isArray(value) && value.length === 0);
    });
    if (!ADOPTION[atom.adoption]) missing.push('adoption (unknown value "' + atom.adoption + '")');
    if (atom.family && !byId(FAMILIES, atom.family)) missing.push('family (unknown "' + atom.family + '")');
    if (atom.layer && LAYERS.length && !byId(LAYERS, atom.layer)) missing.push('layer (unknown "' + atom.layer + '")');
    (atom.seeAlso || []).forEach(function (ref) {
      if (!byId(ATOMS, ref)) missing.push('seeAlso → "' + ref + '" does not exist');
    });
    return missing;
  }

  ATOMS.forEach(function (atom) { atom._problems = schemaProblems(atom); });

  // ---------------------------------------------------------------- search index

  ATOMS.forEach(function (atom) {
    atom._haystack = [
      atom.id, atom.symbol, atom.name, atom.oneLiner, atom.pattern,
      (atom.tags || []).join(' '),
      (atom.api || []).map(function (a) { return a.name + ' ' + (a.file || ''); }).join(' '),
      (atom.whenToUse || []).join(' '),
      (atom.avoid || []).join(' '),
      (atom.gotchas || []).join(' '),
      atom.useInstead || '', atom.note || ''
    ].join(' ').toLowerCase();
  });

  // ---------------------------------------------------------------- state

  var state = { query: '', adoptions: {}, spotlight: null, open: null };
  var nodes = { atoms: {}, layers: {}, molecules: {} };
  var lastTrigger = null;

  function queryTokens() {
    return state.query.toLowerCase().split(/\s+/).filter(Boolean);
  }

  function anyAdoptionFilter() {
    return Object.keys(state.adoptions).some(function (k) { return state.adoptions[k]; });
  }

  function atomMatches(atom) {
    var tokens = queryTokens();
    for (var i = 0; i < tokens.length; i++) {
      if (atom._haystack.indexOf(tokens[i]) === -1) return false;
    }
    if (anyAdoptionFilter() && !state.adoptions[atom.adoption]) return false;
    if (state.spotlight && atom.layer !== state.spotlight) return false;
    return true;
  }

  // ---------------------------------------------------------------- topbar / legend

  function renderBrandAndFooter() {
    var sub = document.getElementById('brand-sub');
    sub.textContent = 'verified against platform ' + (META.platformVersion || '?') +
      (META.updated ? ' · updated ' + META.updated : '');

    var counts = {};
    ATOMS.forEach(function (a) { counts[a.adoption] = (counts[a.adoption] || 0) + 1; });

    var footer = document.getElementById('footer');
    footer.textContent = '';
    append(footer, el('span', {}, ATOMS.length + ' atoms · ' + FAMILIES.length + ' families · ' +
      MOLECULES.length + ' molecules reserved'));
    append(footer, el('span', {}, ADOPTION_ORDER.filter(function (k) { return counts[k]; }).map(function (k) {
      return el('span', {}, el('span', { class: adoptionOf(k).cls, text: adoptionOf(k).glyph + ' ' }),
        counts[k] + ' ' + adoptionOf(k).label.toLowerCase() + '  ');
    })));
    append(footer, el('span', {}, rich('Source of truth: this repository. Each atom records the platform version it was last checked against — see `README.md` to add or refresh one.')));

    var problems = ATOMS.filter(function (a) { return a._problems.length; });
    if (problems.length) {
      append(footer, el('span', { class: 'adopt-schema' },
        '⚠ ' + problems.length + ' atom(s) fail the content schema — open them for details.'));
    }
  }

  function renderFilters() {
    var host = document.getElementById('filters');
    host.textContent = '';
    var counts = {};
    ATOMS.forEach(function (a) { counts[a.adoption] = (counts[a.adoption] || 0) + 1; });

    ADOPTION_ORDER.forEach(function (key) {
      if (!counts[key]) return;
      var meta = adoptionOf(key);
      var chip = el('button', {
        type: 'button', class: 'chip', 'aria-pressed': 'false',
        title: meta.blurb,
        onclick: function () {
          state.adoptions[key] = !state.adoptions[key];
          chip.setAttribute('aria-pressed', state.adoptions[key] ? 'true' : 'false');
          applyFilter();
        }
      },
        el('span', { class: 'glyph ' + meta.cls, text: meta.glyph, 'aria-hidden': 'true' }),
        meta.label,
        el('span', { class: 'chip-n', text: counts[key] }));
      host.appendChild(chip);
    });
  }

  function renderLegend() {
    var host = document.getElementById('legend');
    host.textContent = '';

    var adoptionList = el('ul', { class: 'legend-list' }, ADOPTION_ORDER.map(function (key) {
      var meta = adoptionOf(key);
      return el('li', {},
        el('span', { class: 'glyph ' + meta.cls, text: meta.glyph, 'aria-hidden': 'true' }),
        el('b', { text: meta.label }),
        el('span', { text: '— ' + meta.blurb }));
    }));

    var familyList = el('ul', { class: 'legend-list' }, FAMILIES.map(function (family) {
      var n = ATOMS.filter(function (a) { return a.family === family.id; }).length;
      return el('li', {},
        el('span', { class: 'legend-swatch', style: '--h:' + family.hue, 'aria-hidden': 'true' }),
        el('b', { text: family.name }),
        el('span', { text: '— ' + n + ' atoms' }));
    }));

    append(host, el('div', { class: 'legend-cols' },
      el('div', {}, el('h3', { text: 'Adoption state — read this first' }), adoptionList),
      el('div', {}, el('h3', { text: 'Families' }), familyList)));

    append(host, el('div', { class: 'legend-keys' },
      el('span', {}, el('kbd', { text: '/' }), ' search'),
      el('span', {}, el('kbd', { text: '←→↑↓' }), ' move between tiles'),
      el('span', {}, el('kbd', { text: 'Enter' }), ' open'),
      el('span', {}, el('kbd', { text: 'Esc' }), ' close / clear'),
      el('span', {}, el('kbd', { text: 'f' }), ' full-screen panel'),
      el('span', {}, el('kbd', { text: '?' }), ' this legend'),
      el('span', {}, 'Deep-link any tile: the address bar tracks what you opened.')));
  }

  // ---------------------------------------------------------------- architecture band

  function renderArchitecture() {
    var host = document.getElementById('arch');
    host.textContent = '';

    LAYERS.forEach(function (layer) {
      var node = el('button', {
        type: 'button', class: 'layer', style: '--h:' + (layer.hue || 220),
        'aria-label': layer.name + ' layer — ' + layer.sub,
        dataset: { id: layer.id },
        onclick: function () { openHash('layer', layer.id, node); },
        onmouseenter: function () { setSpotlight(layer.id); },
        onmouseleave: function () { setSpotlight(null); },
        onfocus: function () { setSpotlight(layer.id); },
        onblur: function () { setSpotlight(null); }
      },
        el('span', { class: 'layer-name', text: layer.name }),
        el('span', { class: 'layer-sub' }, rich(layer.sub)),
        el('span', { class: 'layer-tags' }, (layer.tags || []).map(function (tag) {
          return el('span', { class: 'layer-tag', text: tag });
        })));
      nodes.layers[layer.id] = node;
      host.appendChild(node);
    });
  }

  function setSpotlight(layerId) {
    if (state.spotlight === layerId) return;
    state.spotlight = layerId;
    applyFilter();
  }

  // ---------------------------------------------------------------- atom grid

  function renderAtoms() {
    var host = document.getElementById('atoms');
    host.textContent = '';

    FAMILIES.forEach(function (family) {
      var members = ATOMS.filter(function (a) { return a.family === family.id; });
      if (!members.length) return;

      var tiles = el('div', { class: 'family-tiles' }, members.map(function (atom) {
        var meta = adoptionOf(atom.adoption);
        var broken = atom._problems.length > 0;
        var tile = el('button', {
          type: 'button',
          class: 'tile' + (atom.keystone ? ' is-keystone' : ''),
          style: '--h:' + family.hue,
          'aria-label': atom.name + ' — ' + (broken ? 'incomplete content' : meta.label) + '. ' + (atom.oneLiner || ''),
          title: atom.name + ' · ' + meta.label,
          dataset: { id: atom.id },
          onclick: function () { openHash('atom', atom.id, tile); }
        },
          el('span', { class: 'tile-badge ' + (broken ? 'adopt-schema' : meta.cls), 'aria-hidden': 'true',
                       text: broken ? '⚠' : meta.glyph }),
          el('span', { class: 'tile-symbol', text: atom.symbol || '??' }),
          el('span', { class: 'tile-name', text: atom.name }));
        nodes.atoms[atom.id] = tile;
        return tile;
      }));

      host.appendChild(el('div', { class: 'family', style: '--h:' + family.hue },
        el('div', { class: 'family-label' },
          el('span', { text: family.name }),
          el('span', { class: 'family-n', text: members.length })),
        tiles));
    });
  }

  // ---------------------------------------------------------------- molecules

  function renderMolecules() {
    var host = document.getElementById('molecules');
    host.textContent = '';
    MOLECULES.forEach(function (molecule) {
      var node = el('button', {
        type: 'button', class: 'molecule',
        'aria-label': molecule.name + ' — reserved molecule. ' + (molecule.sub || ''),
        dataset: { id: molecule.id },
        onclick: function () { openHash('molecule', molecule.id, node); }
      },
        el('span', { class: 'mol-name', text: molecule.name }),
        el('span', { class: 'mol-sub', text: molecule.sub || '' }));
      nodes.molecules[molecule.id] = node;
      host.appendChild(node);
    });
  }

  // ---------------------------------------------------------------- filtering

  function applyFilter() {
    var shown = 0;
    ATOMS.forEach(function (atom) {
      var node = nodes.atoms[atom.id];
      if (!node) return;
      var ok = atomMatches(atom);
      node.classList.toggle('is-dim', !ok);
      if (ok) shown++;
    });

    var tokens = queryTokens();
    LAYERS.forEach(function (layer) {
      var node = nodes.layers[layer.id];
      var dim = state.spotlight ? state.spotlight !== layer.id
        : tokens.length ? !layerHaystack(layer).match(tokensRegexSafe(tokens)) : false;
      node.classList.toggle('is-dim', !!dim);
    });

    MOLECULES.forEach(function (molecule) {
      var node = nodes.molecules[molecule.id];
      var dim = tokens.length ? !tokens.every(function (t) { return moleculeHaystack(molecule).indexOf(t) !== -1; })
        : false;
      node.classList.toggle('is-dim', !!dim);
    });

    var label = document.getElementById('atoms-count');
    label.textContent = shown === ATOMS.length
      ? 'click any tile · ' + ATOMS.length + ' building blocks'
      : shown + ' of ' + ATOMS.length + ' shown';

    document.getElementById('search-clear').hidden = state.query === '';
  }

  function layerHaystack(layer) {
    if (!layer._haystack) {
      layer._haystack = [layer.name, layer.sub, (layer.tags || []).join(' '),
        (layer.bullets || []).join(' ')].join(' ').toLowerCase();
    }
    return layer._haystack;
  }

  function moleculeHaystack(molecule) {
    if (!molecule._haystack) {
      molecule._haystack = [molecule.name, molecule.sub, (molecule.planned || []).join(' ')]
        .join(' ').toLowerCase();
    }
    return molecule._haystack;
  }

  /* Layers dim on a plain substring test too; kept as a helper so the regex is built once. */
  function tokensRegexSafe(tokens) {
    return new RegExp(tokens.map(function (t) {
      return '(?=.*' + t.replace(/[.*+?^${}()|[\]\\]/g, '\\$&') + ')';
    }).join(''));
  }

  // ---------------------------------------------------------------- drawer

  var drawer = document.getElementById('drawer');
  var scrim = document.getElementById('scrim');

  /* `span` controls how much of the expanded-mode grid a block claims:
       true / 'is-wide' — every column (code and long prose both read badly narrow)
       'is-half'        — two of four columns, for prose-heavy blocks like Gotchas
       omitted          — one column
     In docked mode the body is a plain block flow and these are inert. */
  function block(title, body, span) {
    if (!body) return null;
    var cls = 'd-block' + (span === true ? ' is-wide' : span ? ' ' + span : '');
    return el('div', { class: cls }, el('h3', { text: title }), body);
  }

  function list(items, variant) {
    if (!items || !items.length) return null;
    return el('ul', { class: 'd-list' + (variant ? ' ' + variant : '') },
      items.map(function (item) { return el('li', {}, rich(item)); }));
  }

  /* ---------- syntax highlighting ----------
   * A deliberately small tokenizer. Every token becomes a span whose text is set
   * via textContent, so it cannot corrupt the code or inject markup — the worst a
   * mis-tokenization can do is colour something oddly.
   *
   * Ordering inside each regex matters: comments and strings come first, so a `//`
   * inside a string literal is consumed as part of the string, and a quote inside a
   * comment is consumed as part of the comment.
   */

  var CS_KEYWORDS = {};
  ('abstract as async await base bool break byte case catch char checked class const continue decimal default ' +
   'delegate do double else enum event explicit extern false finally fixed float for foreach get global goto if ' +
   'implicit in init int interface internal is lock long nameof namespace new null object operator out override ' +
   'params private protected public readonly record ref return sbyte sealed set short sizeof stackalloc static ' +
   'string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using var virtual void ' +
   'volatile when where while yield'
  ).split(' ').forEach(function (word) { CS_KEYWORDS[word] = true; });

  var SYNTAX = {
    csharp: {
      re: /(\/\/[^\n]*|\/\*[\s\S]*?\*\/)|(@?\$?"(?:[^"\\]|\\.|"")*"|'(?:[^'\\]|\\.)*')|(\b\d[\d_]*(?:\.\d+)?[a-zA-Z]?\b)|([A-Za-z_]\w*)/g,
      classify: function (m) {
        if (m[1]) return 'comment';
        if (m[2]) return 'string';
        if (m[3]) return 'number';
        if (m[4]) return CS_KEYWORDS[m[4]] ? 'keyword' : (/^[A-Z]/.test(m[4]) ? 'type' : null);
        return null;
      }
    },
    json: {
      re: /(\/\/[^\n]*)|("(?:[^"\\]|\\.)*")|(\b(?:true|false|null)\b)|(-?\b\d[\d.]*\b)/g,
      classify: function (m, code) {
        if (m[1]) return 'comment';
        // A string followed by a colon is a property name, not a value.
        if (m[2]) return /^\s*:/.test(code.slice(m.index + m[0].length)) ? 'type' : 'string';
        if (m[3]) return 'keyword';
        if (m[4]) return 'number';
        return null;
      }
    },
    bash: {
      re: /(#[^\n]*)|("(?:[^"\\]|\\.)*"|'(?:[^'\\]|\\.)*')/g,
      classify: function (m) { return m[1] ? 'comment' : m[2] ? 'string' : null; }
    }
  };

  var LANG_LABELS = { csharp: 'C#', json: 'JSON', bash: 'Shell' };

  function highlight(code, lang) {
    var frag = document.createDocumentFragment();
    var spec = SYNTAX[lang];
    if (!spec) { frag.appendChild(document.createTextNode(code)); return frag; }

    var re = spec.re;
    re.lastIndex = 0;
    var last = 0;
    var match;

    while ((match = re.exec(code)) !== null) {
      if (!match[0].length) { re.lastIndex++; continue; }   // never loop on a zero-length match
      if (match.index > last) frag.appendChild(document.createTextNode(code.slice(last, match.index)));

      var cls = spec.classify(match, code);
      frag.appendChild(cls ? el('span', { class: 'syn-' + cls, text: match[0] })
                           : document.createTextNode(match[0]));
      last = match.index + match[0].length;
    }
    if (last < code.length) frag.appendChild(document.createTextNode(code.slice(last)));
    return frag;
  }

  function snippetBlock(snippet) {
    if (!snippet || !snippet.code) return null;

    var copy = el('button', {
      type: 'button', class: 'icon-btn snippet-copy',
      'aria-label': 'Copy snippet to clipboard',
      onclick: function () {
        copyText(snippet.code).then(function (ok) {
          copy.textContent = ok ? 'copied' : 'select & copy';
          setTimeout(function () { copy.textContent = 'copy'; }, 1400);
        });
      }
    }, 'copy');

    return el('div', { class: 'snippet' },
      el('div', { class: 'snippet-bar' },
        el('span', { class: 'snippet-lang', text: LANG_LABELS[snippet.lang] || snippet.lang || 'code' }),
        copy),
      el('pre', {}, el('code', {}, highlight(snippet.code, snippet.lang))));
  }

  function copyText(text) {
    if (navigator.clipboard && navigator.clipboard.writeText) {
      return navigator.clipboard.writeText(text).then(function () { return true; },
        function () { return legacyCopy(text); });
    }
    return Promise.resolve(legacyCopy(text));
  }

  function legacyCopy(text) {
    try {
      var area = el('textarea', { style: 'position:fixed;opacity:0' });
      area.value = text;
      document.body.appendChild(area);
      area.select();
      var ok = document.execCommand('copy');
      document.body.removeChild(area);
      return ok;
    } catch (e) { return false; }
  }

  function pills(label, items, span) {
    if (!items || !items.length) return null;
    return block(label, el('div', { class: 'pill-row' }, items), span);
  }

  /* Static, non-interactive chips. `tags` were previously only visible on the poster
     tile; in the drawer they fill the column beside the lead paragraph, which would
     otherwise be empty on a wide screen. */
  function tagsBlock(label, tags) {
    if (!tags || !tags.length) return null;
    return block(label, el('div', { class: 'tag-row' }, tags.map(function (tag) {
      return el('span', { class: 'tag-chip', text: tag });
    })), 'is-half');
  }

  /* The lead only claims half the width when something sits beside it. */
  function leadPara(text, hasNeighbour) {
    return el('p', { class: 'd-lead' + (hasNeighbour ? ' is-half' : '') }, rich(text));
  }

  function renderAtomDrawer(atom) {
    var meta = adoptionOf(atom.adoption);
    var family = byId(FAMILIES, atom.family);
    var layer = byId(LAYERS, atom.layer);

    document.getElementById('drawer-eyebrow').textContent = '';
    append(document.getElementById('drawer-eyebrow'), [
      el('span', { class: 'adopt ' + meta.cls }, meta.glyph + ' ' + meta.label),
      family ? el('span', { text: '· ' + family.name }) : null,
      layer ? el('span', { text: '· ' + layer.name }) : null
    ]);
    document.getElementById('drawer-title').textContent = atom.name;

    var body = document.getElementById('drawer-body');
    body.textContent = '';

    var seeAlsoBlock = pills('See also', (atom.seeAlso || []).map(function (ref) {
      var other = byId(ATOMS, ref);
      if (!other) return null;
      return el('button', { type: 'button', class: 'pill', text: other.name,
        onclick: function () { openHash('atom', other.id, nodes.atoms[other.id]); } });
    }).filter(Boolean));

    var partOfBlock = atom.molecule ? pills('Part of', [(function () {
      var mol = byId(MOLECULES, atom.molecule);
      return mol ? el('button', { type: 'button', class: 'pill', text: mol.name + ' →',
        onclick: function () { openHash('molecule', mol.id, nodes.molecules[mol.id]); } }) : null;
    })()].filter(Boolean)) : null;

    if (atom._problems.length) {
      append(body, el('div', { class: 'd-note' },
        el('strong', { class: 'd-note-label', text: '⚠ Content schema' }),
        ' — this atom is incomplete: ' + atom._problems.join('; ') + '.'));
    }

    var alsoKnownAs = tagsBlock('Also known as', atom.tags);
    append(body, [leadPara(atom.oneLiner, !!alsoKnownAs), alsoKnownAs]);

    if (atom.note) {
      append(body, el('div', { class: 'd-note' },
        el('strong', { class: 'd-note-label', text: atom.adoption === 'in-flight' ? 'Migration note' : 'Read this first' }),
        ' — ', rich(atom.note)));
    }
    if (atom.useInstead) {
      append(body, el('div', { class: 'd-note' },
        el('strong', { class: 'd-note-label', text: 'Use instead' }), ' — ', rich(atom.useInstead)));
    }

    append(body, [
      block('Pattern', el('p', { style: 'margin:0' }, rich(atom.pattern))),
      block('When to use', list(atom.whenToUse, 'good')),
      block('Avoid', list(atom.avoid, 'bad')),
      block('API', atom.api && atom.api.length
        ? el('div', { class: 'api-list' }, atom.api.map(function (api) {
            return el('div', { class: 'api-row' },
              el('span', { class: 'api-name', text: api.name }),
              api.file ? el('span', { class: 'api-file', text: api.file }) : null);
          }))
        : null),
      block('Snippet', snippetBlock(atom.snippet), true),
      /* Bottom row of the expanded grid: Gotchas 50% (prose, needs the width),
         Docs 25%, and the two short pill blocks stacked together in the last 25%
         rather than each claiming a column of mostly white space. */
      block('Gotchas', list(atom.gotchas, 'warn'), 'is-half'),
      block('Docs', docLinks(atom.docs)),
      // Only claim a column when there is actually something to put in it.
      (seeAlsoBlock || partOfBlock) ? el('div', { class: 'd-col' }, seeAlsoBlock, partOfBlock) : null,
      el('div', { class: 'd-meta' }, 'Verified against platform ' + (atom.verifiedAgainst || '?') +
        '  ·  id: ' + atom.id)
    ]);
  }

  /* A vertical layered schema: an ordered stack of rows joined by connector pills.
     A row is either a group of consumer nodes or the target itself, so the target can sit
     anywhere in the stack — Channels puts the platform in the middle, with sales channels
     calling down into it and back office / integrations calling up. Declared in content as
     `schema`, so a layer gains a diagram without any change here. */
  var CONNECTOR_DIRS = { down: '↓', up: '↑', both: '↕' };

  function schemaNode(node) {
    return el('div', { class: 'sch-node' + (node.trend ? ' is-trend' : '') },
      el('span', { class: 'sch-node-name' }, rich(node.name)),
      node.sub ? el('span', { class: 'sch-node-sub' }, rich(node.sub)) : null,
      node.via ? el('span', { class: 'sch-via sch-via-' + (node.viaKind || 'plain'), text: node.via }) : null);
  }

  /* A name/description table — the module-type matrix, the API-shape comparison.
     Deliberately NOT the .api-list styling it used to borrow: that is monospace,
     which made prose descriptions read like file paths. */
  function matrixBlock(rows) {
    if (!rows || !rows.length) return null;
    return el('div', { class: 'd-matrix' }, rows.map(function (row) {
      return el('div', { class: 'd-matrix-row' },
        el('span', { class: 'd-matrix-name', text: row.name }),
        el('span', { class: 'd-matrix-desc' }, rich(row.desc)));
    }));
  }

  function schemaBlock(schema) {
    if (!schema || !schema.rows || !schema.rows.length) return null;

    var parts = [];
    schema.rows.forEach(function (row) {
      if (row.connector) {
        parts.push(el('div', { class: 'sch-conn' },
          el('span', { class: 'sch-conn-pill' },
            el('span', { class: 'sch-conn-dir', 'aria-hidden': 'true',
                         text: CONNECTOR_DIRS[row.connectorDir] || CONNECTOR_DIRS.down }),
            row.connector)));
      }
      if (row.target) {
        parts.push(el('div', { class: 'sch-row sch-target' },
          el('span', { class: 'sch-target-name', text: row.target }),
          row.sub ? el('span', { class: 'sch-target-sub' }, rich(row.sub)) : null));
        return;
      }
      parts.push(el('div', { class: 'sch-row sch-group' },
        el('div', { class: 'sch-group-head' },
          el('span', { class: 'sch-group-title', text: row.title }),
          row.hint ? el('span', { class: 'sch-group-hint', text: row.hint }) : null),
        el('div', { class: 'sch-nodes' }, (row.nodes || []).map(schemaNode))));
    });
    return el('div', { class: 'schema' }, parts);
  }

  function renderLayerDrawer(layer) {
    document.getElementById('drawer-eyebrow').textContent = 'Solution architecture';
    document.getElementById('drawer-title').textContent = layer.name;

    var body = document.getElementById('drawer-body');
    body.textContent = '';

    var keyPieces = tagsBlock('Key pieces', layer.tags);
    append(body, [leadPara(layer.sub, !!keyPieces), keyPieces]);

    var atomPills = pills('Atoms in this layer', ATOMS.filter(function (a) { return a.layer === layer.id; })
      .map(function (atom) {
        return el('button', { type: 'button', class: 'pill',
          text: adoptionOf(atom.adoption).glyph + ' ' + atom.name,
          onclick: function () { openHash('atom', atom.id, nodes.atoms[atom.id]); } });
      }));

    append(body, [
      block(layer.schemaTitle || 'Schema', schemaBlock(layer.schema), true),
      /* Full width: these are long bullets, and one narrow track left three empty
         beside it. The list flows into columns so lines keep a readable measure. */
      block('What lives here', list(layer.bullets), true),
      layer.matrix ? block(layer.matrixTitle || 'Variants', matrixBlock(layer.matrix), true) : null,
      block('Gotchas', list(layer.gotchas, 'warn'), 'is-half'),
      /* Some layers own no atoms — Channels is all consumers, not primitives. Without
         that third block the bottom row would stop at 69%, so Docs takes the slack. */
      block('Docs', docLinks(layer.docs), atomPills ? null : 'is-half'),
      atomPills
    ]);
  }

  function renderMoleculeDrawer(molecule) {
    document.getElementById('drawer-eyebrow').textContent = 'Molecule · reserved';
    document.getElementById('drawer-title').textContent = molecule.name;

    var body = document.getElementById('drawer-body');
    body.textContent = '';

    append(body, el('p', { class: 'd-lead' }, rich(molecule.sub || '')));
    append(body, el('div', { class: 'd-note' },
      el('strong', { class: 'd-note-label', text: 'Reserved' }),
      ' — this molecule is a placeholder. The tile exists so the shape of the whole picture is visible; the content is not written yet.'));

    append(body, [
      block('Planned contents', list(molecule.planned)),
      block('Material that already exists', docLinks(molecule.docs)),
      pills('Atoms it will compose', (molecule.atoms || []).map(function (ref) {
        var atom = byId(ATOMS, ref);
        return atom ? el('button', { type: 'button', class: 'pill', text: atom.name,
          onclick: function () { openHash('atom', atom.id, nodes.atoms[atom.id]); } }) : null;
      }).filter(Boolean))
    ]);
  }

  // ---------------------------------------------------------------- open / close

  function openHash(kind, id, trigger) {
    lastTrigger = trigger || lastTrigger;
    var next = '#/' + kind + '/' + id;
    if (location.hash === next) openFromHash();
    else location.hash = next;
  }

  function clearActive() {
    [nodes.atoms, nodes.layers, nodes.molecules].forEach(function (group) {
      Object.keys(group).forEach(function (key) { group[key].classList.remove('is-active'); });
    });
  }

  function openFromHash() {
    var match = /^#\/(atom|layer|molecule)\/(.+)$/.exec(location.hash || '');
    if (!match) { closeDrawer(true); return; }

    var kind = match[1];
    var id = decodeURIComponent(match[2]);
    var item = kind === 'atom' ? byId(ATOMS, id) : kind === 'layer' ? byId(LAYERS, id) : byId(MOLECULES, id);

    if (!item) {
      document.getElementById('drawer-eyebrow').textContent = 'Not found';
      document.getElementById('drawer-title').textContent = id;
      var body = document.getElementById('drawer-body');
      body.textContent = '';
      append(body, el('p', { class: 'empty' }, 'No ' + kind + ' with id "' + id + '" exists in the content files.'));
      showDrawer();
      return;
    }

    clearActive();
    if (kind === 'atom') {
      renderAtomDrawer(item);
      if (nodes.atoms[id]) nodes.atoms[id].classList.add('is-active');
      if (item.layer && nodes.layers[item.layer]) nodes.layers[item.layer].classList.add('is-active');
    } else if (kind === 'layer') {
      renderLayerDrawer(item);
      if (nodes.layers[id]) nodes.layers[id].classList.add('is-active');
    } else {
      renderMoleculeDrawer(item);
      if (nodes.molecules[id]) nodes.molecules[id].classList.add('is-active');
    }

    state.open = { kind: kind, id: id };
    showDrawer();
  }

  function showDrawer() {
    drawer.hidden = false;
    scrim.hidden = false;
    drawer.querySelector('.drawer-body').scrollTop = 0;
    var title = document.getElementById('drawer-title');
    title.setAttribute('tabindex', '-1');
    title.focus({ preventScroll: true });
  }

  /* Expanded mode is sticky: someone who wants the room usually wants it for the
     next atom too, so it survives closing the drawer and reloading the page. */
  function setExpanded(expanded) {
    drawer.classList.toggle('is-full', expanded);
    // Full-screen covers the poster entirely, so it genuinely is modal there.
    drawer.setAttribute('aria-modal', expanded ? 'true' : 'false');

    var button = document.getElementById('drawer-expand');
    button.textContent = expanded ? '⤡' : '⤢';
    button.setAttribute('aria-pressed', expanded ? 'true' : 'false');
    button.title = (expanded ? 'Collapse to side panel' : 'Expand to full screen') + ' ( f )';
    button.setAttribute('aria-label', expanded
      ? 'Collapse details panel back to the side'
      : 'Expand details panel to full screen');

    try { localStorage.setItem('vc-atomic-map-drawer-full', expanded ? '1' : '0'); } catch (e) { /* ignore */ }
  }

  function toggleExpanded() {
    setExpanded(!drawer.classList.contains('is-full'));
  }

  function closeDrawer(silent) {
    drawer.hidden = true;
    scrim.hidden = true;
    clearActive();
    state.open = null;
    if (!silent && location.hash) location.hash = '';
    if (lastTrigger && document.body.contains(lastTrigger)) lastTrigger.focus();
  }

  // ---------------------------------------------------------------- keyboard

  function visibleTiles() {
    return Array.prototype.filter.call(document.querySelectorAll('.tile'), function (tile) {
      return !tile.classList.contains('is-dim');
    });
  }

  function moveTileFocus(delta, absolute) {
    var tiles = visibleTiles();
    if (!tiles.length) return;
    var index = tiles.indexOf(document.activeElement);
    var next;
    if (absolute === 'first') next = 0;
    else if (absolute === 'last') next = tiles.length - 1;
    else if (index === -1) next = 0;
    else next = Math.min(tiles.length - 1, Math.max(0, index + delta));
    tiles[next].focus();
  }

  document.addEventListener('keydown', function (event) {
    var target = event.target;
    var typing = target && (target.tagName === 'INPUT' || target.tagName === 'TEXTAREA');

    if (event.key === 'Escape') {
      if (!drawer.hidden) { closeDrawer(); event.preventDefault(); }
      else if (state.query) { setQuery(''); document.getElementById('search').focus(); event.preventDefault(); }
      return;
    }
    if (typing) return;
    if (event.ctrlKey || event.metaKey || event.altKey) return;

    if (event.key === '/') { document.getElementById('search').focus(); event.preventDefault(); return; }
    if (event.key === '?') { toggleLegend(); event.preventDefault(); return; }
    if ((event.key === 'f' || event.key === 'F') && !drawer.hidden) {
      toggleExpanded(); event.preventDefault(); return;
    }

    if (target && target.classList && target.classList.contains('tile')) {
      if (event.key === 'ArrowRight' || event.key === 'ArrowDown') { moveTileFocus(1); event.preventDefault(); }
      else if (event.key === 'ArrowLeft' || event.key === 'ArrowUp') { moveTileFocus(-1); event.preventDefault(); }
      else if (event.key === 'Home') { moveTileFocus(0, 'first'); event.preventDefault(); }
      else if (event.key === 'End') { moveTileFocus(0, 'last'); event.preventDefault(); }
    }
  });

  // ---------------------------------------------------------------- wiring

  function setQuery(value) {
    state.query = value;
    var input = document.getElementById('search');
    if (input.value !== value) input.value = value;
    applyFilter();
  }

  function toggleLegend() {
    var legend = document.getElementById('legend');
    var button = document.getElementById('legend-toggle');
    var show = legend.hidden;
    legend.hidden = !show;
    button.setAttribute('aria-expanded', show ? 'true' : 'false');
  }

  var THEMES = { auto: { next: 'light', glyph: '◐', label: 'Theme: follows system' },
                 light: { next: 'dark', glyph: '☀', label: 'Theme: light' },
                 dark: { next: 'auto', glyph: '☾', label: 'Theme: dark' } };

  function setTheme(name) {
    var theme = THEMES[name] ? name : 'auto';
    document.documentElement.setAttribute('data-theme', theme);
    var button = document.getElementById('theme-toggle');
    button.textContent = THEMES[theme].glyph;
    button.title = THEMES[theme].label + ' — click for ' + THEMES[theme].next;
    button.setAttribute('aria-label', THEMES[theme].label + '. Activate for ' + THEMES[theme].next + ' theme.');
    try { localStorage.setItem('vc-atomic-map-theme', theme); } catch (e) { /* file:// or blocked storage */ }
  }

  function init() {
    renderLegend();
    renderFilters();
    renderArchitecture();
    renderAtoms();
    renderMolecules();
    renderBrandAndFooter();
    applyFilter();

    document.getElementById('search').addEventListener('input', function (event) { setQuery(event.target.value); });
    document.getElementById('search-clear').addEventListener('click', function () {
      setQuery('');
      document.getElementById('search').focus();
    });
    document.getElementById('legend-toggle').addEventListener('click', toggleLegend);
    document.getElementById('drawer-close').addEventListener('click', function () { closeDrawer(); });
    document.getElementById('drawer-expand').addEventListener('click', toggleExpanded);
    scrim.addEventListener('click', function () { closeDrawer(); });

    var wasFull = '0';
    try { wasFull = localStorage.getItem('vc-atomic-map-drawer-full') || '0'; } catch (e) { /* ignore */ }
    setExpanded(wasFull === '1');

    var stored = 'auto';
    try { stored = localStorage.getItem('vc-atomic-map-theme') || 'auto'; } catch (e) { /* ignore */ }
    setTheme(stored);
    document.getElementById('theme-toggle').addEventListener('click', function () {
      setTheme(THEMES[document.documentElement.getAttribute('data-theme')] ?
        THEMES[document.documentElement.getAttribute('data-theme')].next : 'light');
    });

    window.addEventListener('hashchange', openFromHash);
    if (location.hash) openFromHash();
  }

  if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
  else init();
})();
