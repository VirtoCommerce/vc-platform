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
    'module':    { glyph: '◐', label: 'Module',    cls: 'adopt-module',    blurb: 'Shipped by an installable module, not platform core.' },
    'available': { glyph: '○', label: 'Available', cls: 'adopt-available', blurb: '.NET offers it; this platform does not use it.' },
    'in-flight': { glyph: '△', label: 'In flight', cls: 'adopt-inflight',  blurb: 'Changing right now — read the migration note.' },
    'legacy':    { glyph: '✕', label: 'Legacy',    cls: 'adopt-legacy',    blurb: 'Still works; do not build new code on it.' }
  };
  var ADOPTION_ORDER = ['platform', 'module', 'in-flight', 'legacy', 'available'];

  var REQUIRED = ['id', 'symbol', 'name', 'family', 'adoption', 'layer', 'oneLiner', 'pattern', 'whenToUse', 'api'];

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

  /** Renders `backticked` spans as <code>. No HTML is ever parsed. */
  function rich(text) {
    var frag = document.createDocumentFragment();
    String(text).split('`').forEach(function (part, i) {
      if (part === '') return;
      frag.appendChild(i % 2 ? el('code', { text: part }) : document.createTextNode(part));
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

  function block(title, body) {
    if (!body) return null;
    return el('div', { class: 'd-block' }, el('h3', { text: title }), body);
  }

  function list(items, variant) {
    if (!items || !items.length) return null;
    return el('ul', { class: 'd-list' + (variant ? ' ' + variant : '') },
      items.map(function (item) { return el('li', {}, rich(item)); }));
  }

  function snippetBlock(snippet) {
    if (!snippet || !snippet.code) return null;
    var pre = el('pre', {}, el('code', { text: snippet.code }));
    var copy = el('button', {
      type: 'button', class: 'icon-btn snippet-copy',
      onclick: function () {
        copyText(snippet.code).then(function (ok) {
          copy.textContent = ok ? 'copied' : 'select & copy';
          setTimeout(function () { copy.textContent = 'copy'; }, 1400);
        });
      }
    }, 'copy');
    return el('div', { class: 'snippet' }, pre, copy);
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

  function pills(label, items) {
    if (!items || !items.length) return null;
    return block(label, el('div', { class: 'pill-row' }, items));
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

    if (atom._problems.length) {
      append(body, el('div', { class: 'd-note' },
        el('strong', { text: '⚠ Content schema' }),
        ' — this atom is incomplete: ' + atom._problems.join('; ') + '.'));
    }

    append(body, el('p', { class: 'd-lead' }, rich(atom.oneLiner)));

    if (atom.note) {
      append(body, el('div', { class: 'd-note' },
        el('strong', { text: atom.adoption === 'in-flight' ? 'Migration note' : 'Read this first' }),
        ' — ', rich(atom.note)));
    }
    if (atom.useInstead) {
      append(body, el('div', { class: 'd-note' },
        el('strong', { text: 'Use instead' }), ' — ', rich(atom.useInstead)));
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
      block('Snippet', snippetBlock(atom.snippet)),
      block('Gotchas', list(atom.gotchas, 'warn')),
      block('Docs', atom.docs && atom.docs.length
        ? el('div', { class: 'd-links' }, atom.docs.map(function (doc) {
            return el('a', { href: doc.href, text: doc.label });
          }))
        : null),
      pills('See also', (atom.seeAlso || []).map(function (ref) {
        var other = byId(ATOMS, ref);
        if (!other) return null;
        return el('button', { type: 'button', class: 'pill', text: other.name,
          onclick: function () { openHash('atom', other.id, nodes.atoms[other.id]); } });
      }).filter(Boolean)),
      atom.molecule ? pills('Part of', [(function () {
        var mol = byId(MOLECULES, atom.molecule);
        return mol ? el('button', { type: 'button', class: 'pill', text: mol.name + ' →',
          onclick: function () { openHash('molecule', mol.id, nodes.molecules[mol.id]); } }) : null;
      })()].filter(Boolean)) : null,
      el('div', { class: 'd-meta' }, 'Verified against platform ' + (atom.verifiedAgainst || '?') +
        '  ·  id: ' + atom.id)
    ]);
  }

  function renderLayerDrawer(layer) {
    document.getElementById('drawer-eyebrow').textContent = 'Solution architecture';
    document.getElementById('drawer-title').textContent = layer.name;

    var body = document.getElementById('drawer-body');
    body.textContent = '';

    append(body, el('p', { class: 'd-lead' }, rich(layer.sub)));

    append(body, [
      block('What lives here', list(layer.bullets)),
      layer.matrix ? block(layer.matrixTitle || 'Variants',
        el('div', { class: 'api-list' }, layer.matrix.map(function (row) {
          return el('div', { class: 'api-row' },
            el('span', { class: 'api-name', text: row.name }),
            el('span', { class: 'api-file' }, rich(row.desc)));
        }))) : null,
      block('Gotchas', list(layer.gotchas, 'warn')),
      block('Docs', layer.docs && layer.docs.length
        ? el('div', { class: 'd-links' }, layer.docs.map(function (doc) {
            return el('a', { href: doc.href, text: doc.label });
          }))
        : null),
      pills('Atoms in this layer', ATOMS.filter(function (a) { return a.layer === layer.id; })
        .map(function (atom) {
          return el('button', { type: 'button', class: 'pill',
            text: adoptionOf(atom.adoption).glyph + ' ' + atom.name,
            onclick: function () { openHash('atom', atom.id, nodes.atoms[atom.id]); } });
        }))
    ]);
  }

  function renderMoleculeDrawer(molecule) {
    document.getElementById('drawer-eyebrow').textContent = 'Molecule · reserved';
    document.getElementById('drawer-title').textContent = molecule.name;

    var body = document.getElementById('drawer-body');
    body.textContent = '';

    append(body, el('p', { class: 'd-lead' }, rich(molecule.sub || '')));
    append(body, el('div', { class: 'd-note' },
      el('strong', { text: 'Reserved' }),
      ' — this molecule is a placeholder. The tile exists so the shape of the whole picture is visible; the content is not written yet.'));

    append(body, [
      block('Planned contents', list(molecule.planned)),
      block('Material that already exists', molecule.docs && molecule.docs.length
        ? el('div', { class: 'd-links' }, molecule.docs.map(function (doc) {
            return el('a', { href: doc.href, text: doc.label });
          }))
        : null),
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
    scrim.addEventListener('click', function () { closeDrawer(); });

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
