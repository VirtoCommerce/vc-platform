/* Reference-integrity check for docs/atomic-map content.
   A broken pointer in a reference map is worse than a missing one, so this must be clean. */
const fs = require('fs');
const path = require('path');

const REPO = path.resolve(__dirname, '../..');
const MAP = __dirname;

global.window = {};
for (const f of ['content/meta.js', 'content/architecture.js', 'content/atoms.js', 'content/cells.js', 'content/molecules.js']) {
  new Function(fs.readFileSync(path.join(MAP, f), 'utf8')).call(global);
}
const { VC_MAP_META: META, VC_MAP_ARCHITECTURE: LAYERS, VC_MAP_FAMILIES: FAMILIES,
        VC_MAP_ATOMS: ATOMS, VC_MAP_CELLS: CELLS, VC_MAP_MOLECULES: MOLECULES } = global.window;

const problems = [];
const add = (kind, id, msg) => problems.push(`${kind} ${id}: ${msg}`);

// ---- api[].file must exist under the repo, unless it is an explicit "(annotation)"
const checkedPaths = new Set();
function checkRepoPath(kind, id, p) {
  if (!p) return;
  if (p.trim().startsWith('(')) return;            // deliberate "not a path" annotation
  const abs = path.join(REPO, p);
  checkedPaths.add(p);
  if (!fs.existsSync(abs)) add(kind, id, `api file does not exist → ${p}`);
}

/* Doc links come in three forms:
     page  — a vc-docs developer-guide page, rendered as an absolute docs.virtocommerce.org URL
     href  — a fully external URL
     path  — an in-repo file, relative to docs/
   `page` values cannot be verified offline; run with --online to check them against the
   vc-docs repo (requires the gh CLI). */
const ONLINE = process.argv.includes('--online');
let VC_DOCS_PAGES = null;

if (ONLINE) {
  try {
    const out = require('child_process').execSync(
      'gh api repos/VirtoCommerce/vc-docs/git/trees/main?recursive=1 --jq ".tree[].path"',
      { encoding: 'utf8', maxBuffer: 32 * 1024 * 1024, stdio: ['ignore', 'pipe', 'ignore'] });
    VC_DOCS_PAGES = new Set(out.split('\n')
      .filter(p => p.startsWith('platform/developer-guide/docs/') && p.endsWith('.md'))
      .map(p => p.replace('platform/developer-guide/docs/', '').replace(/\.md$/, '')));
    console.log(`online: loaded ${VC_DOCS_PAGES.size} vc-docs developer-guide pages`);
  } catch (e) {
    console.log('online: could not reach vc-docs via gh — skipping page existence checks');
  }
}

const docsSeen = new Set();

function checkDoc(kind, id, doc) {
  const forms = ['page', 'href', 'path'].filter(f => doc[f]);
  if (forms.length === 0) { add(kind, id, `doc "${doc.label}" has no page/href/path`); return; }
  if (forms.length > 1) add(kind, id, `doc "${doc.label}" sets more than one of page/href/path`);
  if (!doc.label) add(kind, id, 'doc entry without a label');

  if (doc.page) {
    docsSeen.add(doc.page);
    if (/\.md$/.test(doc.page)) add(kind, id, `doc page must not end in .md → ${doc.page}`);
    if (/^\/|\/$/.test(doc.page)) add(kind, id, `doc page must not start or end with "/" → ${doc.page}`);
    if (/^https?:/.test(doc.page)) add(kind, id, `doc page must be a path, not a URL → ${doc.page}`);
    if (VC_DOCS_PAGES && !VC_DOCS_PAGES.has(doc.page)) {
      add(kind, id, `doc page not found in vc-docs → ${doc.page}`);
    }
  }
  if (doc.path) {
    const abs = path.join(REPO, 'docs', doc.path.split('#')[0]);
    if (!fs.existsSync(abs)) add(kind, id, `doc path does not resolve → docs/${doc.path}`);
  }
}

/* Compare two `1.2.3` platform versions. Lexicographic string compare gets 3.999 > 3.1053 wrong,
   which is exactly the range these versions live in. */
function verCompare(a, b) {
  const pa = String(a).split('.').map(Number), pb = String(b).split('.').map(Number);
  for (let i = 0; i < Math.max(pa.length, pb.length); i++) {
    const d = (pa[i] || 0) - (pb[i] || 0);
    if (d) return d < 0 ? -1 : 1;
  }
  return 0;
}

/* `[[atom-id]]` in prose renders as a link, so a reference to an atom that does not exist is a
   dead link on the page. Every string in the content files is swept for them. */
function checkCrossRefs(kind, id, value) {
  if (typeof value === 'string') {
    const refs = value.match(/\[\[([a-z0-9-]+)\]\]/g) || [];
    for (const r of refs) {
      const target = r.slice(2, -2);
      if (!atomIds.has(target)) add(kind, id, `cross-reference [[${target}]] does not name an atom`);
    }
    return;
  }
  if (Array.isArray(value)) { for (const v of value) checkCrossRefs(kind, id, v); return; }
  if (value && typeof value === 'object') { for (const v of Object.values(value)) checkCrossRefs(kind, id, v); }
}

const REQUIRED = ['id','symbol','name','family','adoption','layer','oneLiner','pattern','whenToUse','api'];
const ADOPTIONS = new Set(['platform','module','available','in-flight','legacy']);
const atomIds = new Set(ATOMS.map(a => a.id));
const layerIds = new Set(LAYERS.map(l => l.id));
const familyIds = new Set(FAMILIES.map(f => f.id));
const moleculeIds = new Set(MOLECULES.map(m => m.id));

for (const a of ATOMS) {
  for (const f of REQUIRED) {
    const v = a[f];
    if (v === undefined || v === null || v === '' || (Array.isArray(v) && !v.length)) add('atom', a.id, `missing ${f}`);
  }
  if (!ADOPTIONS.has(a.adoption)) add('atom', a.id, `bad adoption "${a.adoption}"`);
  if (!familyIds.has(a.family)) add('atom', a.id, `unknown family "${a.family}"`);
  if (!layerIds.has(a.layer)) add('atom', a.id, `unknown layer "${a.layer}"`);
  if (!a.verifiedAgainst) add('atom', a.id, 'missing verifiedAgainst');
  for (const ref of a.seeAlso || []) if (!atomIds.has(ref)) add('atom', a.id, `seeAlso → "${ref}" missing`);
  if (a.molecule && !moleculeIds.has(a.molecule)) add('atom', a.id, `molecule → "${a.molecule}" missing`);
  for (const api of a.api || []) checkRepoPath('atom', a.id, api.file);
  for (const d of a.docs || []) checkDoc('atom', a.id, d);
  if (a.snippet && !a.snippet.code) add('atom', a.id, 'snippet without code');
  if (a.seeAlso?.includes(a.id)) add('atom', a.id, 'seeAlso references itself');
  checkCrossRefs('atom', a.id, a);
}

/* Cells are the third rung of the ladder: a set of modules that solves a business scenario.
   They carry the same diagram shapes as a layer, so the diagram rules below are shared. */
const SPLIT_VERDICTS = new Set(['own host', 'with catalog', 'with cart', 'no']);
const cellIds = new Set((CELLS || []).map(c => c.id));

const VIA_KINDS = new Set(['graphql', 'rest', 'trend', 'plain']);
const CONNECTOR_DIRS = new Set(['down', 'up', 'both']);
/* Per-diagram rules, shared by layers and cells — the two things that carry diagrams. One copy
   on purpose: two copies of a validation rule is one rule and one stale rule. */
const KINDS = new Set(['stack', 'flow', 'lanes', 'pipeline', 'topology', 'tree', 'section']);
const NODE_KINDS = new Set(['virto', 'oob', 'custom', 'data', 'infra']);

function checkDiagram(kind, id, d) {
  if (!d.title) add(kind, id, 'diagram without a title');
  if (!KINDS.has(d.kind)) { add(kind, id, `diagram "${d.title}" has unknown kind "${d.kind}"`); return; }

  if (d.kind === 'section') {
    // A section is prose in the diagram sequence: it must say something, or it is a heading alone.
    if (!(d.items || []).length && !d.note) add(kind, id, `section "${d.title}" has neither a note nor items`);
    for (const item of d.items || []) {
      if (typeof item !== 'string' || !item.trim()) add(kind, id, `section "${d.title}" has a blank item`);
    }
    return;
  }

  if (d.kind === 'pipeline') {
    const PP_KINDS = new Set(['src', 'custom', 'virto', 'select', 'image', 'env']);
    if (!(d.lanes || []).length && !(d.rows || []).length) {
      add(kind, id, `pipeline "${d.title}" has neither lanes nor rows`);
    }
    const widths = new Set((d.lanes || []).map(ln => (ln.steps || []).length));
    if (widths.size > 1) {
      // Uneven lanes leave the converging pill misaligned against one of them.
      add(kind, id, `pipeline "${d.title}" lanes have differing step counts (${[...widths].join(', ')})`);
    }
    for (const ln of d.lanes || []) {
      for (const st of ln.steps || []) {
        if (st.connector) continue;
        if (!st.name) add(kind, id, `pipeline "${d.title}" has a step without a name`);
        if (st.kind && !PP_KINDS.has(st.kind)) add(kind, id, `pipeline step "${st.name}" has unknown kind "${st.kind}"`);
      }
    }
    for (const r of d.rows || []) {
      if (!r.name) add(kind, id, `pipeline "${d.title}" has a row without a name`);
      if (r.kind && !PP_KINDS.has(r.kind)) add(kind, id, `pipeline row "${r.name}" has unknown kind "${r.kind}"`);
    }
    return;
  }

  if (d.kind === 'tree') {
    const TREE_KINDS = new Set(['project', 'file']);
    if (!(d.items || []).length) add(kind, id, `tree "${d.title}" has no items`);
    let previous = -1;
    for (const item of d.items || []) {
      if (!item.name) add(kind, id, `tree "${d.title}" has an item without a name`);
      if (!item.desc) add(kind, id, `tree item "${item.name}" has no description — the point of the tree is that every line is answered`);
      const depth = item.depth || 0;
      // A jump of more than one level means a parent is missing from the listing.
      if (depth > previous + 1) {
        add(kind, id, `tree item "${item.name}" jumps from depth ${previous} to ${depth}`);
      }
      previous = depth;
      if (item.kind && !TREE_KINDS.has(item.kind)) {
        add(kind, id, `tree item "${item.name}" has unknown kind "${item.kind}"`);
      }
    }
    return;
  }

  if (d.kind === 'topology') {
    const cols = d.cols || 0;
    if (!cols) add(kind, id, `topology "${d.title}" has no cols`);
    const seen = new Map();
    const ids = new Set();
    for (const n of d.nodes || []) {
      if (!n.id) { add(kind, id, `topology "${d.title}" has a node without an id`); continue; }
      if (!n.name) add(kind, id, `topology node "${n.id}" has no name`);
      if (ids.has(n.id)) add(kind, id, `topology "${d.title}" has duplicate node id "${n.id}"`);
      ids.add(n.id);
      if (n.kind && !NODE_KINDS.has(n.kind)) add(kind, id, `topology node "${n.id}" has unknown kind "${n.kind}"`);
      if (!(n.col >= 1 && n.col <= cols)) {
        add(kind, id, `topology node "${n.id}" sits at col ${n.col}, outside 1..${cols}`);
      }
      if (!(n.row >= 1)) add(kind, id, `topology node "${n.id}" has no valid row`);
      // Two cards in one cell overlap: the grid stacks them and one becomes unreadable.
      const cell = n.col + ':' + n.row;
      if (seen.has(cell)) {
        add(kind, id, `topology "${d.title}" places "${n.id}" and "${seen.get(cell)}" in the same cell (${cell})`);
      }
      seen.set(cell, n.id);
    }
    for (const e of d.edges || []) {
      if (!ids.has(e.from)) add(kind, id, `topology edge from unknown node "${e.from}"`);
      if (!ids.has(e.to)) add(kind, id, `topology edge to unknown node "${e.to}"`);
      if (e.from === e.to) add(kind, id, `topology edge "${e.from}" points at itself`);
    }
    for (const r of d.regions || []) {
      // A `tight` region wraps one card and has no room for a label; every other region needs one.
      if (!r.label && !r.tight) add(kind, id, `topology "${d.title}" has a region without a label`);
      const okCol = Array.isArray(r.col) && r.col.length === 2 && r.col[0] >= 1 && r.col[1] <= cols && r.col[0] <= r.col[1];
      const okRow = Array.isArray(r.row) && r.row.length === 2 && r.row[0] >= 1 && r.row[0] <= r.row[1];
      if (!okCol || !okRow) add(kind, id, `region "${r.label}" has an invalid col/row span`);
    }
    const usedKinds = new Set((d.nodes || []).map(n => n.kind));
    for (const item of d.legend || []) {
      if (!item.label) add(kind, id, `topology "${d.title}" legend entry without a label`);
      if (item.dashed) continue;
      if (!usedKinds.has(item.kind)) add(kind, id, `topology "${d.title}" legend lists unused kind "${item.kind}"`);
    }
    // A dashed legend entry promises at least one bypass edge to explain.
    if ((d.legend || []).some(i => i.dashed) && !(d.edges || []).some(e => e.bypass)) {
      add(kind, id, `topology "${d.title}" legends a dashed line but has no bypass edge`);
    }
    return;
  }

  if (d.kind === 'lanes') {
    const cols = d.columns || [];
    const laned = cols.filter(c => !c.shared);
    if (!cols.length) add(kind, id, `lanes "${d.title}" has no columns`);
    if (!(d.lanes || []).length) add(kind, id, `lanes "${d.title}" has no lanes`);
    // Laned columns must come first, or the grid indices the renderer computes are wrong.
    const firstShared = cols.findIndex(c => c.shared);
    if (firstShared !== -1 && cols.slice(firstShared).some(c => !c.shared)) {
      add(kind, id, `lanes "${d.title}" mixes shared and laned columns — laned ones must come first`);
    }
    // A group must cover a contiguous run, or the single box it draws would swallow
    // columns that are not part of it.
    for (const id of Object.keys(d.groups || {})) {
      const idx = cols.reduce((acc, c, i) => (c.group === id ? acc.concat(i) : acc), []);
      if (!idx.length) { add(kind, id, `group "${id}" is declared but no column uses it`); continue; }
      if (idx[idx.length - 1] - idx[0] + 1 !== idx.length) {
        add(kind, id, `group "${id}" spans non-adjacent columns (${idx.join(', ')})`);
      }
    }
    for (const c of cols) {
      if (c.group && !(d.groups || {})[c.group]) {
        add(kind, id, `column "${c.label}" references undeclared group "${c.group}"`);
      }
      if (!c.label) add(kind, id, `lanes "${d.title}" has a column without a label`);
      if (!c.shared) continue;
      if (!(c.nodes || []).length && !(c.scopes || []).length) {
        add(kind, id, `shared column "${c.label}" has neither nodes nor scopes`);
      }
      for (const sc of c.scopes || []) {
        if (!sc.title) add(kind, id, `scope in "${c.label}" has no title`);
        if (!(sc.modules || []).length) add(kind, id, `scope "${sc.title}" lists no modules`);
      }
    }
    const kindsUsed = new Set();
    for (const lane of d.lanes || []) {
      if (!lane.label) add(kind, id, `lanes "${d.title}" has a lane without a label`);
      if ((lane.cells || []).length !== laned.length) {
        add(kind, id, `lane "${lane.label}" has ${(lane.cells || []).length} cells but there are ${laned.length} laned columns`);
      }
      for (const cell of lane.cells || []) {
        for (const n of (cell || {}).nodes || []) {
          if (!n.name) add(kind, id, `lane "${lane.label}" has a node without a name`);
          if (n.kind && !NODE_KINDS.has(n.kind)) add(kind, id, `lane node "${n.name}" has unknown kind "${n.kind}"`);
          kindsUsed.add(n.kind);
        }
      }
    }
    for (const c of cols) for (const n of c.nodes || []) kindsUsed.add(n.kind);
    for (const item of d.legend || []) {
      if (!item.label) add(kind, id, `lanes "${d.title}" legend entry without a label`);
      if (item.dashed) continue;   // describes a line style, not a node kind
      if (!kindsUsed.has(item.kind)) add(kind, id, `lanes "${d.title}" legend lists unused kind "${item.kind}"`);
    }
    return;
  }

  if (d.kind === 'flow') {
    const tiers = (d.tiers || []).filter(t => !t.arrow);
    if (!tiers.length) add(kind, id, `flow "${d.title}" has no tiers`);
    for (const t of tiers) {
      if (!t.label) add(kind, id, `flow "${d.title}" has a tier without a label`);
      const nodes = [...(t.nodes || []), ...(t.clusters || []).flatMap(c => c.nodes || [])];
      if (!nodes.length) add(kind, id, `flow tier "${t.label}" has no nodes`);
      for (const c of t.clusters || []) {
        if (!c.title) add(kind, id, `flow "${d.title}" has a cluster without a title`);
      }
      for (const n of nodes) {
        if (!n.name) add(kind, id, `flow tier "${t.label}" has a node without a name`);
        if (n.kind && !NODE_KINDS.has(n.kind)) {
          add(kind, id, `flow node "${n.name}" has unknown kind "${n.kind}"`);
        }
      }
    }
    // A legend that names a kind nothing uses is stale.
    const used = new Set((d.tiers || []).flatMap(t =>
      [...(t.nodes || []), ...(t.clusters || []).flatMap(c => c.nodes || [])].map(n => n.kind)));
    for (const item of d.legend || []) {
      if (!used.has(item.kind)) add(kind, id, `flow "${d.title}" legend lists unused kind "${item.kind}"`);
    }
    return;
  }

  const rows = d.rows;
  if (!rows?.length) add(kind, id, `stack "${d.title}" has no rows`);
  let targets = 0;
  for (const r of rows || []) {
    if (r.connectorDir && !CONNECTOR_DIRS.has(r.connectorDir)) {
      add(kind, id, `stack row has unknown connectorDir "${r.connectorDir}"`);
    }
    if (r.connectorDir && !r.connector) add(kind, id, 'stack row sets connectorDir but no connector label');
    if (r.target) { targets++; continue; }
    if (!r.title) add(kind, id, `stack "${d.title}" has a row that is neither a target nor a titled group`);
    if (!r.nodes?.length) add(kind, id, `stack row "${r.title}" has no nodes`);
    for (const n of r.nodes || []) {
      if (!n.name) add(kind, id, `stack node without a name in "${r.title}"`);
      if (n.viaKind && !VIA_KINDS.has(n.viaKind)) {
        add(kind, id, `stack node "${n.name}" has unknown viaKind "${n.viaKind}"`);
      }
      // A coloured chip with no protocol, or a protocol with no chip, is half-authored.
      if (n.viaKind && !n.via) add(kind, id, `stack node "${n.name}" sets viaKind but no via label`);
    }
  }
  if (targets !== 1) add(kind, id, `stack "${d.title}" must have exactly one target row, found ${targets}`);
}

for (const l of LAYERS) {
  checkCrossRefs('layer', l.id, l);
  for (const d of l.docs || []) checkDoc('layer', l.id, d);
  if (!l.hue) add('layer', l.id, 'missing hue');
  if (!l.sub) add('layer', l.id, 'missing sub');
  if (l.schema) add('layer', l.id, 'uses the old `schema` field — migrate to `diagrams: [{ kind: "stack", … }]`');

  for (const d of l.diagrams || []) checkDiagram('layer', l.id, d);
}
for (const c of CELLS || []) {
  if (!c.id) { add('cell', '(no id)', 'missing id'); continue; }
  checkCrossRefs('cell', c.id, c);
  if (!c.name) add('cell', c.id, 'missing name');
  if (!c.sub) add('cell', c.id, 'missing sub');
  if (!SPLIT_VERDICTS.has(c.splittable)) {
    add('cell', c.id, `splittable "${c.splittable}" is not one of: ${[...SPLIT_VERDICTS].join(', ')}`);
  }
  if (!c.anchor) add('cell', c.id, 'missing anchor — a cell is anchored on the experience API module whose manifest decides its membership');
  // The registry release the membership was read from. Without it a stale list is invisible.
  if (!/^\d+\.\d+\.\d+$/.test(c.version || '')) add('cell', c.id, `version "${c.version}" is not a module release the membership can be checked against`);
  if (!(c.planned || []).length) add('cell', c.id, 'missing planned — these tiles are reserved, so they must say what is coming');
  if (!(c.modules || []).length) add('cell', c.id, 'lists no modules — a cell is a set of modules');
  for (const m of c.modules || []) {
    if (typeof m !== 'string' || !m.trim()) add('cell', c.id, 'has a blank module name');
  }
  // An optional module the cell does not contain is a contradiction, and would render a chip
  // for something that is not there.
  for (const o of c.optional || []) {
    if (!(c.modules || []).includes(o)) add('cell', c.id, `optional "${o}" is not in its modules list`);
  }
  for (const ref of c.atoms || []) {
    if (!atomIds.has(ref)) add('cell', c.id, `atoms → "${ref}" does not exist`);
  }
  for (const d of c.docs || []) checkDoc('cell', c.id, d);
  for (const d of c.diagrams || []) checkDiagram('cell', c.id, d);
}

/* Two kinds of molecule, two contracts. A module tile is verified identity plus a dependency
   graph read from the registry, so it needs no prose; a composite topic is prose that has not been
   written, so it must at least say what is coming. */
const MODULE_GROUPS = new Set(['commerce', 'extension']);
for (const m of MOLECULES) {
  for (const d of m.docs || []) checkDoc('molecule', m.id, d);
  for (const ref of m.atoms || []) if (!atomIds.has(ref)) add('molecule', m.id, `atoms → "${ref}" missing`);
  if (!m.name) add('molecule', m.id, 'missing name');

  if (m.kind === 'module') {
    if (!/^VirtoCommerce\.[A-Za-z0-9.]+$/.test(m.moduleId || '')) {
      add('molecule', m.id, `moduleId "${m.moduleId}" is not a module id from the registry`);
    }
    // The release the identity and the dependency list were read at. Without it a stale tile is
    // invisible, and a hand-edited dependency list is exactly what this tier must not become.
    if (!/^\d+\.\d+\.\d+$/.test(m.version || '')) {
      add('molecule', m.id, `version "${m.version}" is not a module release`);
    }
    if (!MODULE_GROUPS.has(m.group)) add('molecule', m.id, `group "${m.group}" is not commerce or extension`);
    if (!Array.isArray(m.dependsOn)) add('molecule', m.id, 'dependsOn must be an array, even when empty');
    for (const dep of [...(m.dependsOn || []), ...(m.optional || [])]) {
      if (typeof dep !== 'string' || !dep.trim()) add('molecule', m.id, 'has a blank dependency name');
    }
    // An optional dependency is also a dependency; listing it twice would render it twice.
    for (const o of m.optional || []) {
      if ((m.dependsOn || []).includes(o)) add('molecule', m.id, `"${o}" is in both dependsOn and optional`);
    }
    if (m.repo && !/^https:\/\/github\.com\//.test(m.repo)) add('molecule', m.id, `repo "${m.repo}" is not a GitHub URL`);
    if (m.planned) add('molecule', m.id, 'a module tile carries its dependency graph, not a planned list');
    continue;
  }

  if (!m.planned?.length) add('molecule', m.id, 'no planned contents');
}

// duplicate ids / symbols
const seen = {};
for (const a of ATOMS) {
  if (seen[a.id]) add('atom', a.id, 'duplicate id');
  seen[a.id] = true;
}
const symbols = {};
for (const a of ATOMS) {
  if (symbols[a.symbol]) add('atom', a.id, `duplicate symbol "${a.symbol}" (also ${symbols[a.symbol]})`);
  symbols[a.symbol] = a.id;
}

// version consistency
for (const a of ATOMS) {
  /* Behind the sweep is honest — it means nobody has re-read that source since. Ahead of it is
     not: it would claim a check against a version this content set has never been swept at. */
  if (a.verifiedAgainst && verCompare(a.verifiedAgainst, META.platformVersion) > 0) {
    add('atom', a.id, `verifiedAgainst ${a.verifiedAgainst} is newer than the sweep in meta.js (${META.platformVersion})`);
  }
}

// ---- report
const byFamily = FAMILIES.map(f => `${f.name}: ${ATOMS.filter(a => a.family === f.id).length}`).join('  |  ');
const byAdoption = [...ADOPTIONS].map(k => `${k}: ${ATOMS.filter(a => a.adoption === k).length}`).join('  |  ');
const noSnippet = ATOMS.filter(a => !a.snippet).map(a => a.id);

const behind = ATOMS.filter(a => a.verifiedAgainst && verCompare(a.verifiedAgainst, META.platformVersion) < 0);
if (behind.length) {
  console.log(`due for a re-check (verified before ${META.platformVersion}): ${behind.length} of ${ATOMS.length} atoms` +
    (behind.length <= 8 ? ' → ' + behind.map(a => a.id).join(', ') : ''));
}

const modMol = MOLECULES.filter(m => m.kind === 'module');
console.log(`atoms: ${ATOMS.length}   layers: ${LAYERS.length}   cells: ${CELLS.length}   molecules: ${MOLECULES.length} (${modMol.length} modules from the registry, ${MOLECULES.length - modMol.length} topics)`);
console.log('cells → ' + CELLS.map(c => `${c.name} (${c.splittable}, ${c.modules.length} modules)`).join('  |  '));
console.log(`families → ${byFamily}`);
console.log(`adoption → ${byAdoption}`);
console.log(`repo paths checked: ${checkedPaths.size}   distinct vc-docs pages linked: ${docsSeen.size}` +
            (VC_DOCS_PAGES ? '  (verified online)' : '  (run --online to verify)'));
console.log(`atoms without a snippet (${noSnippet.length}): ${noSnippet.join(', ') || 'none'}`);
console.log('');
if (problems.length) {
  console.log(`FAIL — ${problems.length} problem(s):`);
  problems.forEach(p => console.log('  - ' + p));
  process.exitCode = 1;
} else {
  console.log('PASS — every reference resolves, every atom complete.');
}
