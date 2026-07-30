/* Reference-integrity check for docs/atomic-map content.
   A broken pointer in a reference map is worse than a missing one, so this must be clean. */
const fs = require('fs');
const path = require('path');

const REPO = path.resolve(__dirname, '../..');
const MAP = __dirname;

global.window = {};
for (const f of ['content/meta.js', 'content/architecture.js', 'content/atoms.js', 'content/molecules.js']) {
  new Function(fs.readFileSync(path.join(MAP, f), 'utf8')).call(global);
}
const { VC_MAP_META: META, VC_MAP_ARCHITECTURE: LAYERS,
        VC_MAP_FAMILIES: FAMILIES, VC_MAP_ATOMS: ATOMS, VC_MAP_MOLECULES: MOLECULES } = global.window;

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
}

const VIA_KINDS = new Set(['graphql', 'rest', 'trend', 'plain']);
const CONNECTOR_DIRS = new Set(['down', 'up', 'both']);
for (const l of LAYERS) {
  for (const d of l.docs || []) checkDoc('layer', l.id, d);
  if (!l.hue) add('layer', l.id, 'missing hue');
  if (!l.sub) add('layer', l.id, 'missing sub');
  if (l.schema) {
    const rows = l.schema.rows;
    if (!rows?.length) add('layer', l.id, 'schema has no rows');
    let targets = 0;
    for (const r of rows || []) {
      if (r.connectorDir && !CONNECTOR_DIRS.has(r.connectorDir)) {
        add('layer', l.id, `schema row has unknown connectorDir "${r.connectorDir}"`);
      }
      if (r.connectorDir && !r.connector) add('layer', l.id, 'schema row sets connectorDir but no connector label');
      if (r.target) { targets++; continue; }
      if (!r.title) add('layer', l.id, 'schema row is neither a target nor a titled group');
      if (!r.nodes?.length) add('layer', l.id, `schema row "${r.title}" has no nodes`);
      for (const n of r.nodes || []) {
        if (!n.name) add('layer', l.id, `schema node without a name in "${r.title}"`);
        if (n.viaKind && !VIA_KINDS.has(n.viaKind)) {
          add('layer', l.id, `schema node "${n.name}" has unknown viaKind "${n.viaKind}"`);
        }
        // A coloured chip with no protocol, or a protocol with no chip, is half-authored.
        if (n.viaKind && !n.via) add('layer', l.id, `schema node "${n.name}" sets viaKind but no via label`);
      }
    }
    if (targets !== 1) add('layer', l.id, `schema must have exactly one target row, found ${targets}`);
  }
}
for (const m of MOLECULES) {
  for (const d of m.docs || []) checkDoc('molecule', m.id, d);
  for (const ref of m.atoms || []) if (!atomIds.has(ref)) add('molecule', m.id, `atoms → "${ref}" missing`);
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
  if (a.verifiedAgainst && a.verifiedAgainst !== META.platformVersion) {
    add('atom', a.id, `verifiedAgainst ${a.verifiedAgainst} != meta ${META.platformVersion}`);
  }
}

// ---- report
const byFamily = FAMILIES.map(f => `${f.name}: ${ATOMS.filter(a => a.family === f.id).length}`).join('  |  ');
const byAdoption = [...ADOPTIONS].map(k => `${k}: ${ATOMS.filter(a => a.adoption === k).length}`).join('  |  ');
const noSnippet = ATOMS.filter(a => !a.snippet).map(a => a.id);

console.log(`atoms: ${ATOMS.length}   layers: ${LAYERS.length}   molecules: ${MOLECULES.length}`);
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
