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

// ---- docs[].href resolves relative to docs/atomic-map/
function checkDocHref(kind, id, href) {
  if (!href) return;
  if (/^https?:/.test(href)) return;
  const abs = path.join(MAP, href.split('#')[0]);
  if (!fs.existsSync(abs)) add(kind, id, `doc href does not resolve → ${href}`);
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
  for (const d of a.docs || []) checkDocHref('atom', a.id, d.href);
  if (a.snippet && !a.snippet.code) add('atom', a.id, 'snippet without code');
  if (a.seeAlso?.includes(a.id)) add('atom', a.id, 'seeAlso references itself');
}

const VIA_KINDS = new Set(['graphql', 'rest', 'trend', 'plain']);
for (const l of LAYERS) {
  for (const d of l.docs || []) checkDocHref('layer', l.id, d.href);
  if (!l.hue) add('layer', l.id, 'missing hue');
  if (!l.sub) add('layer', l.id, 'missing sub');
  if (l.schema) {
    if (!l.schema.groups?.length) add('layer', l.id, 'schema has no groups');
    for (const g of l.schema.groups || []) {
      if (!g.title) add('layer', l.id, 'schema group without a title');
      if (!g.nodes?.length) add('layer', l.id, `schema group "${g.title}" has no nodes`);
      for (const n of g.nodes || []) {
        if (!n.name) add('layer', l.id, `schema node without a name in "${g.title}"`);
        if (n.viaKind && !VIA_KINDS.has(n.viaKind)) {
          add('layer', l.id, `schema node "${n.name}" has unknown viaKind "${n.viaKind}"`);
        }
        // A coloured chip with no protocol, or a protocol with no chip, is half-authored.
        if (n.viaKind && !n.via) add('layer', l.id, `schema node "${n.name}" sets viaKind but no via label`);
      }
    }
    if (!l.schema.target?.name) add('layer', l.id, 'schema has no target');
  }
}
for (const m of MOLECULES) {
  for (const d of m.docs || []) checkDocHref('molecule', m.id, d.href);
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
console.log(`repo paths checked: ${checkedPaths.size}`);
console.log(`atoms without a snippet (${noSnippet.length}): ${noSnippet.join(', ') || 'none'}`);
console.log('');
if (problems.length) {
  console.log(`FAIL — ${problems.length} problem(s):`);
  problems.forEach(p => console.log('  - ' + p));
  process.exitCode = 1;
} else {
  console.log('PASS — every reference resolves, every atom complete.');
}
