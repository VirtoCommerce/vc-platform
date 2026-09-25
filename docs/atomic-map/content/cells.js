/* Cells — the rung above molecules on the Virto Atomic Architecture ladder.
 *
 * The Architectural Guidelines define the ladder as atom → molecule → cell → organism, where a
 * *molecule* is a module (Catalog, Pricing, Inventory) and a *cell* is "a set of molecules
 * combined into a high-performance functional unit … to solve a complete business scenario".
 * This file uses that vocabulary literally: a composability conversation held in different words
 * than the deck is a conversation nobody can check.
 *
 * SOURCE OF MEMBERSHIP — the module registry, not judgement:
 *
 *   https://github.com/VirtoCommerce/vc-modules/blob/master/modules_v3.json
 *
 * Every cell is anchored on an **experience API module**, and its `modules` / `optional` lists are
 * the dependencies the registry records for that module's latest stable release. `version` is that
 * release, so a stale tile is visible rather than merely wrong. Re-read the registry when you
 * touch this file; do not extend a list by hand.
 *
 * Two rules about membership:
 *   · Only modules with a stable 3.800+ release are in scope — the registry carries 104 of them.
 *   · Infrastructure modules already covered on the **atoms** tier are deliberately absent:
 *     Assets, Search and its providers, BackgroundJobs, BackupRestore, EventBus, WebHooks,
 *     SeqLog, ApplicationInsights. A cell is a business capability, not a service it runs on.
 *
 * `splittable` is the verdict the dependency graph gives, not an opinion:
 *   'own host'     — its required closure is small; it can run as its own deployment
 *   'with catalog' — its manifest requires XCatalog, so the catalog cell deploys with it
 *   'with cart'    — it requires XCart, and so XCatalog too; the three deploy together
 *
 * These tiles are RESERVED, like the molecules shelf: the membership and the verdict are
 * verified, the walk-throughs are not written. The composability explanation they point at lives
 * on the `Your solution` layer, which answers how far a solution can actually be decomposed.
 *
 * Registry modules that would qualify as cells and are not tiled yet: `VirtoCommerce.UCP`
 * (requires XCart · XCatalog · Marketing · Orders · Store) and `VirtoCommerce.XPickup`
 * (requires Cart · Catalog · Inventory · Shipping). Both are real; the shelf holds one row.
 */
window.VC_MAP_CELLS = [
  {
    id: 'digital-catalog',
    name: 'Digital Catalog',
    sub: 'Product data and the read path in front of it.',
    anchor: 'VirtoCommerce.XCatalog',
    version: '3.1015.0',
    splittable: 'own host',
    modules: ['Catalog', 'Xapi', 'Pricing', 'Inventory', 'Marketing'],
    optional: ['Pricing', 'Inventory', 'Marketing'],
    planned: [
      'Where a price comes from: pricelist evaluation, the projection into the search index, and why an order price is a copy rather than a lookup',
      'The catalog-read host in practice — the module set, its configuration, and what it cannot serve',
      'The reindex consistency window, and how blue-green indexing narrows it',
      'What breaks first when the index and the database disagree'
    ],
    docs: [
      { label: 'Indexed search overview', page: 'Fundamentals/Indexed-Search/overview' },
      { label: 'Blue-green indexing', page: 'Fundamentals/Indexed-Search/indexing/blue-green-indexing' },
      { label: 'vc-module-x-catalog (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-x-catalog' }
    ],
    atoms: ['module-database', 'cross-module-references', 'optional-dependency', 'host-composition', 'search']
  },

  {
    id: 'purchase',
    name: 'Purchase',
    sub: 'Cart, promotions, payment and shipping options.',
    anchor: 'VirtoCommerce.XCart',
    version: '3.1028.0',
    splittable: 'with catalog',
    modules: ['Cart', 'Catalog', 'Pricing', 'Inventory', 'Marketing', 'Payment', 'Shipping',
              'FileExperienceApi', 'Xapi', 'XCatalog'],
    optional: [],
    planned: [
      'Why the cart evaluates prices live while the catalog reads them from the index',
      'Ten required dependencies and no optional ones — which of them are the real obstacles to a smaller host',
      'Where a cart becomes an order, and what is copied at that moment'
    ],
    docs: [
      { label: 'New payment method', page: 'Fundamentals/Payments/new-payment-method-registration' },
      { label: 'vc-module-x-cart (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-x-cart' }
    ],
    atoms: ['cross-module-references', 'module-database', 'host-composition']
  },

  {
    id: 'order',
    name: 'Order',
    sub: 'Orders as the storefront sees them.',
    anchor: 'VirtoCommerce.XOrder',
    version: '3.1008.0',
    splittable: 'with cart',
    modules: ['Orders', 'Payment', 'FileExperienceApi', 'Xapi', 'XCart', 'XCatalog'],
    optional: [],
    planned: [
      'Order pricing as a snapshot: no dependency on Pricing, and a totals calculator that only sums stored values',
      'Requiring xCart and xCatalog means this cell never deploys alone — what that costs and what it buys',
      'The order database on its own server, and the retention rules that usually motivate it'
    ],
    docs: [
      { label: 'vc-module-x-order (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-x-order' },
      { label: 'vc-module-order (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-order' }
    ],
    atoms: ['cross-module-references', 'module-database', 'host-composition']
  },

  {
    id: 'customer-company',
    name: 'Customer & Company',
    sub: 'Accounts, B2B organisations and their contacts.',
    anchor: 'VirtoCommerce.ProfileExperienceApiModule',
    version: '3.1014.0',
    splittable: 'own host',
    modules: ['Customer', 'Notifications', 'Xapi', 'Marketing', 'Pricing', 'Tax', 'XOrder'],
    optional: ['Marketing', 'Pricing', 'Tax', 'XOrder'],
    planned: [
      'The B2B hierarchy: organisations, contacts, and the permissions that follow them',
      'Why xOrder is optional here, and what a profile host loses without it',
      'Identity storage on its own server — the platform already does this with `Auth:ConnectionString`',
      'Sharing bearer tokens before splitting anything that authenticates'
    ],
    docs: [
      { label: 'Sharing bearer tokens', page: 'Tutorials-and-How-tos/How-tos/sharing-bearer-tokens' },
      { label: 'Authorization overview', page: 'Fundamentals/Security/authorization/overview' },
      { label: 'vc-module-profile-experience-api (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-profile-experience-api' }
    ],
    atoms: ['authentication', 'current-user', 'module-database', 'optional-dependency']
  },

  {
    id: 'content',
    name: 'Content',
    sub: 'Pages, menus and the CMS behind the storefront.',
    anchor: 'VirtoCommerce.XCMS',
    version: '3.1004.0',
    splittable: 'own host',
    modules: ['Content', 'Customer', 'Xapi', 'Pages', 'PageBuilderModule'],
    optional: ['Pages', 'PageBuilderModule'],
    planned: [
      'Why content is served in parallel with the platform rather than through it',
      'The page builder as an optional member — what a content host looks like without it',
      'Where an external CMS fits, and what the adapter modules actually do'
    ],
    docs: [
      { label: 'vc-module-x-cms (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-x-cms' }
    ],
    atoms: ['optional-dependency', 'host-composition', 'assets']
  },

  {
    id: 'recommendations',
    name: 'Recommendations',
    sub: 'Product recommendations over the catalog read path.',
    anchor: 'VirtoCommerce.XRecommend',
    version: '3.1002.0',
    splittable: 'with catalog',
    modules: ['XCatalog', 'Xapi'],
    optional: [],
    planned: [
      'The smallest cell in the registry — two required dependencies and nothing else',
      'Why it deploys with the catalog cell rather than beside it',
      'Where the AI modules join this path'
    ],
    docs: [
      { label: 'vc-module-x-recommend (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-x-recommend' }
    ],
    atoms: ['optional-dependency', 'host-composition', 'search']
  }
];
