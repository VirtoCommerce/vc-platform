/* Molecules — the modules, and the composite topics written across them.
 *
 * The Architectural Guidelines put molecules between atoms and cells: "a group of two or more
 * atoms built with a single responsibility principle to handle a specific job … Examples:
 * Catalog, Pricing, Inventory, Search, Event Bus." A molecule is therefore a **module**, and the
 * shelf carries two kinds of tile:
 *
 *   kind: 'module'  — a real Virto Commerce module. Id, release and dependencies come from the
 *                     registry, https://github.com/VirtoCommerce/vc-modules/blob/master/modules_v3.json,
 *                     filtered to a stable 3.800+ release. Never edit these by hand: re-read the
 *                     registry, because a hand-maintained inventory is wrong within a month.
 *   (no kind)       — a composite topic: a guide written across several modules. Reserved, and
 *                     referenced by atoms through `atom.molecule`, which is why they stay.
 *
 * Excluded from the module tiles on purpose, because the atoms tier already covers them or they
 * implement a provider contract rather than a business capability: Search and its engines, Assets
 * and its stores, BackgroundJobs, BackupRestore, EventBus, WebHooks, SeqLog, ApplicationInsights,
 * import/export tooling, and the payment · tax · shipping · SSO · CMS adapters.
 *
 * Both kinds are placeholders: the tiles exist so the shape of the whole picture is visible and
 * the gaps are honest. For a module tile the identity and the dependency graph are verified; the
 * write-up is not. `atoms` lists the atom ids a molecule will compose — unresolved ids are simply
 * skipped, so it is safe to list an atom before it is authored.
 */
window.VC_MAP_MOLECULES = [

  // ============================================================ MODULES (from the registry)

  {
    id: 'mod-cart',
    kind: 'module',
    name: 'Cart',
    moduleId: 'VirtoCommerce.Cart',
    version: '3.1007.0',
    sub: 'Shopping Cart',
    group: 'commerce',
    dependsOn: ['Assets', 'Core', 'Customer', 'Notifications', 'Payment', 'Shipping', 'Store'],
    optional: ['Search'],
    repo: 'https://github.com/VirtoCommerce/vc-module-cart',
  },

  {
    id: 'mod-catalog',
    kind: 'module',
    name: 'Catalog',
    moduleId: 'VirtoCommerce.Catalog',
    version: '3.1040.0',
    sub: 'Catalog',
    group: 'commerce',
    dependsOn: ['Assets', 'Core', 'Search', 'Seo', 'Store'],
    optional: ['BulkActionsModule', 'Export'],
    repo: 'https://github.com/VirtoCommerce/vc-module-catalog',
  },

  {
    id: 'mod-catalog-personalization',
    kind: 'module',
    name: 'CatalogPersonalization',
    moduleId: 'VirtoCommerce.CatalogPersonalization',
    version: '3.1003.0',
    sub: 'Catalog Personalization',
    group: 'commerce',
    dependsOn: ['Catalog', 'Core', 'Search'],
    repo: 'https://github.com/VirtoCommerce/vc-module-catalog-personalization',
  },

  {
    id: 'mod-catalog-publishing',
    kind: 'module',
    name: 'CatalogPublishing',
    moduleId: 'VirtoCommerce.CatalogPublishing',
    version: '3.1005.0',
    sub: 'Catalog Publishing',
    group: 'commerce',
    dependsOn: ['Catalog', 'Pricing'],
    repo: 'https://github.com/VirtoCommerce/vc-module-catalog-publishing',
  },

  {
    id: 'mod-content',
    kind: 'module',
    name: 'Content',
    moduleId: 'VirtoCommerce.Content',
    version: '3.1003.0',
    sub: 'Content',
    group: 'commerce',
    dependsOn: ['Assets', 'AzureBlobAssets', 'FileSystemAssets', 'Search', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-content',
  },

  {
    id: 'mod-core',
    kind: 'module',
    name: 'Core',
    moduleId: 'VirtoCommerce.Core',
    version: '3.1007.0',
    sub: 'Commerce core module',
    group: 'commerce',
    dependsOn: [],
    repo: 'https://github.com/VirtoCommerce/vc-module-core',
  },

  {
    id: 'mod-customer',
    kind: 'module',
    name: 'Customer',
    moduleId: 'VirtoCommerce.Customer',
    version: '3.1021.0',
    sub: 'Companies and Contacts',
    group: 'commerce',
    dependsOn: ['Core', 'Notifications', 'Search', 'Seo', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-customer',
  },

  {
    id: 'mod-file-experience-api',
    kind: 'module',
    name: 'FileExperienceApi',
    moduleId: 'VirtoCommerce.FileExperienceApi',
    version: '3.1004.0',
    sub: 'File Experience API',
    group: 'commerce',
    dependsOn: ['Assets', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-file-experience-api',
  },

  {
    id: 'mod-inventory',
    kind: 'module',
    name: 'Inventory',
    moduleId: 'VirtoCommerce.Inventory',
    version: '3.1004.0',
    sub: 'Inventory',
    group: 'commerce',
    dependsOn: ['Catalog', 'Core', 'Search'],
    repo: 'https://github.com/VirtoCommerce/vc-module-inventory',
  },

  {
    id: 'mod-marketing',
    kind: 'module',
    name: 'Marketing',
    moduleId: 'VirtoCommerce.Marketing',
    version: '3.1006.0',
    sub: 'Marketing',
    group: 'commerce',
    dependsOn: ['Assets', 'Catalog', 'Core', 'Payment', 'Shipping', 'Store'],
    optional: ['Customer', 'Orders'],
    repo: 'https://github.com/VirtoCommerce/vc-module-marketing',
  },

  {
    id: 'mod-notifications',
    kind: 'module',
    name: 'Notifications',
    moduleId: 'VirtoCommerce.Notifications',
    version: '3.1013.0',
    sub: 'Notifications',
    group: 'commerce',
    dependsOn: ['Assets'],
    repo: 'https://github.com/VirtoCommerce/vc-module-notification',
  },

  {
    id: 'mod-orders',
    kind: 'module',
    name: 'Orders',
    moduleId: 'VirtoCommerce.Orders',
    version: '3.1013.0',
    sub: 'Order Management',
    group: 'commerce',
    dependsOn: ['Assets', 'Cart', 'Catalog', 'Core', 'Customer', 'Inventory', 'Notifications', 'Payment', 'Search', 'Shipping', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-order',
  },

  {
    id: 'mod-page-builder-module',
    kind: 'module',
    name: 'PageBuilderModule',
    moduleId: 'VirtoCommerce.PageBuilderModule',
    version: '3.1020.0',
    sub: 'CMS Page Builder',
    group: 'commerce',
    dependsOn: ['Assets', 'Content', 'Core', 'Customer', 'Search', 'Store'],
    optional: ['Pages'],
    repo: 'https://github.com/VirtoCommerce/vc-module-pagebuilder',
  },

  {
    id: 'mod-payment',
    kind: 'module',
    name: 'Payment',
    moduleId: 'VirtoCommerce.Payment',
    version: '3.1006.0',
    sub: 'Payment module',
    group: 'commerce',
    dependsOn: ['Core', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-payment',
  },

  {
    id: 'mod-pricing',
    kind: 'module',
    name: 'Pricing',
    moduleId: 'VirtoCommerce.Pricing',
    version: '3.1005.0',
    sub: 'Pricing',
    group: 'commerce',
    dependsOn: ['Assets', 'Catalog', 'Core', 'Search'],
    optional: ['Export'],
    repo: 'https://github.com/VirtoCommerce/vc-module-pricing',
  },

  {
    id: 'mod-profile-experience-api-module',
    kind: 'module',
    name: 'ProfileExperienceApiModule',
    moduleId: 'VirtoCommerce.ProfileExperienceApiModule',
    version: '3.1014.0',
    sub: 'Commerce Profile Experience API',
    group: 'commerce',
    dependsOn: ['Customer', 'Notifications', 'Xapi'],
    optional: ['Marketing', 'Pricing', 'Tax', 'XOrder'],
    repo: 'https://github.com/VirtoCommerce/vc-module-profile-experience-api',
  },

  {
    id: 'mod-seo',
    kind: 'module',
    name: 'Seo',
    moduleId: 'VirtoCommerce.Seo',
    version: '3.1004.0',
    sub: 'SEO',
    group: 'commerce',
    dependsOn: [],
    repo: 'https://github.com/VirtoCommerce/vc-module-seo',
  },

  {
    id: 'mod-shipping',
    kind: 'module',
    name: 'Shipping',
    moduleId: 'VirtoCommerce.Shipping',
    version: '3.1007.0',
    sub: 'Shipping',
    group: 'commerce',
    dependsOn: ['Core', 'Search', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-shipping',
  },

  {
    id: 'mod-sitemaps',
    kind: 'module',
    name: 'Sitemaps',
    moduleId: 'VirtoCommerce.Sitemaps',
    version: '3.1003.0',
    sub: 'Sitemap Generator',
    group: 'commerce',
    dependsOn: ['Assets', 'Catalog', 'Content', 'Core', 'Customer', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-sitemaps',
  },

  {
    id: 'mod-store',
    kind: 'module',
    name: 'Store',
    moduleId: 'VirtoCommerce.Store',
    version: '3.1006.0',
    sub: 'Store',
    group: 'commerce',
    dependsOn: ['Core', 'Notifications', 'Seo'],
    repo: 'https://github.com/VirtoCommerce/vc-module-store',
  },

  {
    id: 'mod-subscription',
    kind: 'module',
    name: 'Subscription',
    moduleId: 'VirtoCommerce.Subscription',
    version: '3.1002.0',
    sub: 'Subscriptions',
    group: 'commerce',
    dependsOn: ['Core', 'Customer', 'Notifications', 'Orders', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-subscription',
  },

  {
    id: 'mod-tax',
    kind: 'module',
    name: 'Tax',
    moduleId: 'VirtoCommerce.Tax',
    version: '3.1005.0',
    sub: 'Tax Core',
    group: 'commerce',
    dependsOn: ['Core', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-tax',
  },

  {
    id: 'mod-u-c-p',
    kind: 'module',
    name: 'UCP',
    moduleId: 'VirtoCommerce.UCP',
    version: '3.1004.0',
    sub: 'Universal Commerce Protocol',
    group: 'commerce',
    dependsOn: ['Marketing', 'Orders', 'Store', 'XCart', 'XCatalog', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-u-c-p',
  },

  {
    id: 'mod-xapi',
    kind: 'module',
    name: 'Xapi',
    moduleId: 'VirtoCommerce.Xapi',
    version: '3.1016.0',
    sub: 'Core Experience API',
    group: 'commerce',
    dependsOn: ['Customer', 'Search', 'Seo', 'Store'],
    optional: ['ApplicationInsights', 'Tax'],
    repo: 'https://github.com/VirtoCommerce/vc-module-x-api',
  },

  {
    id: 'mod-x-cart',
    kind: 'module',
    name: 'XCart',
    moduleId: 'VirtoCommerce.XCart',
    version: '3.1028.0',
    sub: 'Cart Experience API',
    group: 'commerce',
    dependsOn: ['Cart', 'Catalog', 'FileExperienceApi', 'Inventory', 'Marketing', 'Payment', 'Pricing', 'Shipping', 'XCatalog', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-x-cart',
  },

  {
    id: 'mod-x-catalog',
    kind: 'module',
    name: 'XCatalog',
    moduleId: 'VirtoCommerce.XCatalog',
    version: '3.1015.0',
    sub: 'Catalog Experience API',
    group: 'commerce',
    dependsOn: ['Catalog', 'Xapi'],
    optional: ['Inventory', 'Marketing', 'Pricing'],
    repo: 'https://github.com/VirtoCommerce/vc-module-x-catalog',
  },

  {
    id: 'mod-x-c-m-s',
    kind: 'module',
    name: 'XCMS',
    moduleId: 'VirtoCommerce.XCMS',
    version: '3.1004.0',
    sub: 'CMS Experience API',
    group: 'commerce',
    dependsOn: ['Content', 'Customer', 'Xapi'],
    optional: ['PageBuilderModule', 'Pages'],
    repo: 'https://github.com/VirtoCommerce/vc-module-x-cms',
  },

  {
    id: 'mod-x-frontend',
    kind: 'module',
    name: 'XFrontend',
    moduleId: 'VirtoCommerce.XFrontend',
    version: '3.1005.0',
    sub: 'Frontend Experience API',
    group: 'commerce',
    dependsOn: ['ProfileExperienceApiModule', 'Xapi'],
    optional: ['WhiteLabeling'],
    repo: 'https://github.com/VirtoCommerce/vc-module-x-frontend',
  },

  {
    id: 'mod-x-order',
    kind: 'module',
    name: 'XOrder',
    moduleId: 'VirtoCommerce.XOrder',
    version: '3.1008.0',
    sub: 'Order Experience API',
    group: 'commerce',
    dependsOn: ['FileExperienceApi', 'Orders', 'Payment', 'XCart', 'XCatalog', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-x-order',
  },

  {
    id: 'mod-a-i',
    kind: 'module',
    name: 'AI',
    moduleId: 'VirtoCommerce.AI',
    version: '3.1001.0',
    sub: 'VirtoCommerce AI',
    group: 'extension',
    dependsOn: [],
    repo: 'https://github.com/VirtoCommerce/vc-module-ai',
  },

  {
    id: 'mod-a-i-document-processing',
    kind: 'module',
    name: 'AIDocumentProcessing',
    moduleId: 'VirtoCommerce.AIDocumentProcessing',
    version: '3.901.0',
    sub: 'VirtoCommerce AIDocumentProcessing module',
    group: 'extension',
    dependsOn: ['Core', 'FileExperienceApi', 'ProfileExperienceApiModule', 'Quote', 'XCart', 'Xapi'],
    optional: ['ApplicationInsights'],
    repo: 'https://github.com/VirtoCommerce/vc-module-ai-document-processing',
  },

  {
    id: 'mod-back-in-stock',
    kind: 'module',
    name: 'BackInStock',
    moduleId: 'VirtoCommerce.BackInStock',
    version: '3.1002.0',
    sub: 'Back In Stock',
    group: 'extension',
    dependsOn: ['Catalog', 'Customer', 'Inventory', 'Notifications', 'Store', 'XCatalog', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-back-in-stock',
  },

  {
    id: 'mod-contracts',
    kind: 'module',
    name: 'Contracts',
    moduleId: 'VirtoCommerce.Contracts',
    version: '3.1004.0',
    sub: 'Contracts',
    group: 'extension',
    dependsOn: ['Customer', 'Pricing', 'Store', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-contract',
  },

  {
    id: 'mod-customer-reviews',
    kind: 'module',
    name: 'CustomerReviews',
    moduleId: 'VirtoCommerce.CustomerReviews',
    version: '3.1005.0',
    sub: 'Rating and Reviews',
    group: 'extension',
    dependsOn: ['Assets', 'Catalog', 'Customer', 'FileExperienceApi', 'Notifications', 'Orders', 'ProfileExperienceApiModule', 'Store', 'XCatalog', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-customer-review',
  },

  {
    id: 'mod-dynamic-associations-module',
    kind: 'module',
    name: 'DynamicAssociationsModule',
    moduleId: 'VirtoCommerce.DynamicAssociationsModule',
    version: '3.1002.0',
    sub: 'Dynamic Associations',
    group: 'extension',
    dependsOn: ['Catalog', 'Core', 'Marketing', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-dynamic-associations',
  },

  {
    id: 'mod-g-d-p-r',
    kind: 'module',
    name: 'GDPR',
    moduleId: 'VirtoCommerce.GDPR',
    version: '3.1001.0',
    sub: 'GDPR',
    group: 'extension',
    dependsOn: ['Customer', 'Orders'],
    repo: 'https://github.com/VirtoCommerce/vc-module-gdpr',
  },

  {
    id: 'mod-loyalty',
    kind: 'module',
    name: 'Loyalty',
    moduleId: 'VirtoCommerce.Loyalty',
    version: '3.1004.0',
    sub: 'Loyalty',
    group: 'extension',
    dependsOn: ['Catalog', 'Core', 'Orders', 'XCart', 'XCatalog'],
    repo: 'https://github.com/VirtoCommerce/vc-module-loyalty',
  },

  {
    id: 'mod-marketing-experience-api',
    kind: 'module',
    name: 'MarketingExperienceApi',
    moduleId: 'VirtoCommerce.MarketingExperienceApi',
    version: '3.1004.0',
    sub: 'Marketing Experience API',
    group: 'extension',
    dependsOn: ['Customer', 'Marketing', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-marketing-experience-api',
  },

  {
    id: 'mod-news',
    kind: 'module',
    name: 'News',
    moduleId: 'VirtoCommerce.News',
    version: '3.1005.0',
    sub: 'News',
    group: 'extension',
    dependsOn: ['Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-news',
  },

  {
    id: 'mod-order-management',
    kind: 'module',
    name: 'OrderManagement',
    moduleId: 'VirtoCommerce.OrderManagement',
    version: '3.1002.0',
    sub: 'Order Management (Business Rules)',
    group: 'extension',
    dependsOn: ['Catalog', 'Orders', 'Store'],
    optional: ['XCatalog'],
    repo: 'https://github.com/VirtoCommerce/vc-module-order-management',
  },

  {
    id: 'mod-pages',
    kind: 'module',
    name: 'Pages',
    moduleId: 'VirtoCommerce.Pages',
    version: '3.1008.0',
    sub: 'Virto Pages',
    group: 'extension',
    dependsOn: ['Customer', 'Search', 'Seo', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-pages',
  },

  {
    id: 'mod-product-snapshot',
    kind: 'module',
    name: 'ProductSnapshot',
    moduleId: 'VirtoCommerce.ProductSnapshot',
    version: '3.1003.0',
    sub: 'Product Snapshot',
    group: 'extension',
    dependsOn: ['Catalog', 'Orders', 'XOrder'],
    repo: 'https://github.com/VirtoCommerce/vc-module-product-snapshot',
  },

  {
    id: 'mod-push-messages',
    kind: 'module',
    name: 'PushMessages',
    moduleId: 'VirtoCommerce.PushMessages',
    version: '3.1005.0',
    sub: 'Push Messages',
    group: 'extension',
    dependsOn: ['Customer', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-push-messages',
  },

  {
    id: 'mod-quote',
    kind: 'module',
    name: 'Quote',
    moduleId: 'VirtoCommerce.Quote',
    version: '3.1002.0',
    sub: 'Quotes',
    group: 'extension',
    dependsOn: ['Assets', 'Cart', 'Catalog', 'Core', 'Customer', 'FileExperienceApi', 'Orders', 'Shipping', 'Store', 'Tax', 'XCart', 'XCatalog', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-quote',
  },

  {
    id: 'mod-return',
    kind: 'module',
    name: 'Return',
    moduleId: 'VirtoCommerce.Return',
    version: '3.1001.0',
    sub: 'Returns',
    group: 'extension',
    dependsOn: ['Orders', 'Store'],
    repo: 'https://github.com/VirtoCommerce/vc-module-return',
  },

  {
    id: 'mod-sales-rep',
    kind: 'module',
    name: 'SalesRep',
    moduleId: 'VirtoCommerce.SalesRep',
    version: '3.1000.0',
    sub: 'Virto Commerce Sales Rep',
    group: 'extension',
    dependsOn: ['Cart', 'Catalog', 'Core', 'Customer', 'Notifications', 'Orders', 'PushMessages', 'Store', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-sales-rep',
  },

  {
    id: 'mod-state-machine',
    kind: 'module',
    name: 'StateMachine',
    moduleId: 'VirtoCommerce.StateMachine',
    version: '3.1002.0',
    sub: 'StateMachine module',
    group: 'extension',
    dependsOn: ['Core'],
    repo: 'https://github.com/VirtoCommerce/vc-module-state-machine',
  },

  {
    id: 'mod-task-management',
    kind: 'module',
    name: 'TaskManagement',
    moduleId: 'VirtoCommerce.TaskManagement',
    version: '3.1005.0',
    sub: 'Task Management',
    group: 'extension',
    dependsOn: ['Customer', 'Notifications', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-task-management',
  },

  {
    id: 'mod-white-labeling',
    kind: 'module',
    name: 'WhiteLabeling',
    moduleId: 'VirtoCommerce.WhiteLabeling',
    version: '3.1003.0',
    sub: 'White Labeling',
    group: 'extension',
    dependsOn: ['Customer', 'FileExperienceApi', 'ImageTools', 'XCMS', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-white-labeling',
  },

  {
    id: 'mod-x-pickup',
    kind: 'module',
    name: 'XPickup',
    moduleId: 'VirtoCommerce.XPickup',
    version: '3.1003.0',
    sub: 'Buy online pickup in store (BOPIS) Experience API',
    group: 'extension',
    dependsOn: ['Cart', 'Catalog', 'Inventory', 'Shipping', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-x-pickup',
  },

  {
    id: 'mod-x-recommend',
    kind: 'module',
    name: 'XRecommend',
    moduleId: 'VirtoCommerce.XRecommend',
    version: '3.1002.0',
    sub: 'Product Recommendations',
    group: 'extension',
    dependsOn: ['XCatalog', 'Xapi'],
    repo: 'https://github.com/VirtoCommerce/vc-module-x-recommend',
  },

  // ============================================================ COMPOSITE TOPICS
  {
    id: 'ecommerce-modules',
    name: 'eCommerce modules',
    sub: 'What ships in the box and how the bounded contexts relate.',
    planned: [
      'The commerce module map: Catalog, Pricing, Inventory, Cart, Order, Customer, Marketing, Store, Content, Search, Notifications, Payment, Shipping, Tax',
      'Which module owns which entity, and where the seams between them are',
      'The dependency direction between them, and which pairs are safe to couple',
      'Where each one keeps its data, and which ones depend on the search index to be usable'
    ],
    docs: [
      { label: 'Architecture reference', page: 'Back-End-Architecture/02-conceptual-overview' },
      { label: 'Modularity', page: 'Fundamentals/Modularity/01-overview' }
    ],
    atoms: ['module-lifecycle', 'generic-crud', 'domain-events']
  },
  {
    id: 'background-processing-hub',
    name: 'Background processing hub',
    sub: 'The in-flight extraction of the job engine behind one port.',
    planned: [
      'Why Hangfire was removed as a hard platform dependency',
      'The engine port and its adapters, selected by `VirtoCommerce:BackgroundJobs:Provider` — Hangfire (default), RabbitMQ, in-memory for development',
      '`Mode` as a deployment lever: Producer, Worker, or Both, and what a producer-only cluster looks like',
      'What happens when no engine is installed, and why the platform still boots',
      'The two-step migration for modules calling Hangfire directly, and the `EnableLegacyHangfire` escape hatch',
      'Idempotency, dedup keys, claim-check for large payloads, and one OpenTelemetry model across engines'
    ],
    docs: [
      { label: 'vc-module-background-jobs (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-background-jobs' },
      { label: 'Design spec (2026-06-06)', href: 'https://github.com/VirtoCommerce/vc-platform/blob/dev/docs/superpowers/specs/2026-06-06-background-processing-hub-design.md' }
    ],
    atoms: ['background-jobs', 'recurring-jobs', 'map-reduce-jobs', 'job-progress', 'fire-and-forget', 'hangfire', 'distributed-lock']
  },
  {
    id: 'search-and-indexing',
    name: 'Search & indexing',
    sub: 'The read path the catalog actually depends on.',
    planned: [
      'Index documents, builders and the indexing pipeline',
      'Provider choice: Elasticsearch, Lucene, Azure Search — and what changes with each',
      'Full vs incremental reindex, and what triggers each',
      'Why catalog reads go to the index rather than the database, and the consistency window that creates'
    ],
    docs: [
      { label: 'Search', page: 'Fundamentals/Indexed-Search/overview' }
    ],
    atoms: ['background-jobs', 'domain-events']
  },
  {
    id: 'notifications',
    name: 'Notifications',
    sub: 'Transactional email and messages: templates, layouts, sending.',
    planned: [
      'Notification types, templates and layouts',
      'Registering a notification and overriding a built-in one',
      'Sending providers and what to do when one fails',
      'Where notifications sit relative to domain events'
    ],
    docs: [],
    atoms: ['domain-events', 'localizations', 'background-jobs']
  },
  {
    id: 'provider-registration',
    name: 'Payment · shipping · tax',
    sub: 'The provider pattern: small, focused, swappable per store.',
    planned: [
      'The registration contract for each provider kind',
      'Per-store configuration through settings, and per-provider setting scoping',
      'The request/response flow through a payment provider, including callbacks',
      'Testing a provider without a live gateway'
    ],
    docs: [
      { label: 'New payment method', page: 'Fundamentals/Payments/new-payment-method-registration' },
      { label: 'New shipping method', page: 'Fundamentals/Shipments/new-shipping-method-registration' },
      { label: 'New tax provider', page: 'Fundamentals/Taxes/new-tax-provider-registration' }
    ],
    atoms: ['settings', 'abstract-type-factory', 'dependency-injection']
  },
  {
    id: 'xapi',
    name: 'XAPI deep-dive',
    sub: 'GraphQL experience API: schema, resolvers, extension points.',
    planned: [
      'How `xCatalog` / `xCart` / `xOrder` / `xCMS` / `xProfile` compose one schema',
      'Adding a field to an existing type without forking the module',
      'Query batching, dataloaders and the N+1 traps specific to GraphQL',
      'Authorization inside resolvers, and how it differs from `[Authorize]` on a controller'
    ],
    docs: [],
    atoms: ['generic-crud', 'authorization']
  },
  {
    id: 'extensibility-decision-tree',
    name: 'Extensibility decision tree',
    sub: 'No-code → API → native. Pick the lowest level that works.',
    planned: [
      'Level 1 no-code: dynamic properties, statuses, permissions, settings',
      'Level 2 API: REST/GraphQL, webhooks, event bus, integration middleware',
      'Level 3 native: derive a type and register the override via `AbstractTypeFactory`',
      'The cost of each level at upgrade time — the real reason the order matters'
    ],
    docs: [
      { label: 'Extensibility overview', page: 'Extensibility/overview' },
      { label: 'Extending domain models', page: 'Tutorials-and-How-tos/Tutorials/extending-domain-models' }
    ],
    atoms: ['abstract-type-factory', 'dynamic-properties', 'domain-events', 'dependency-injection']
  },
  {
    id: 'dev-process',
    name: 'Development process + roles',
    sub: 'Who does what, and the path from ticket to production.',
    planned: [
      'Roles: solution architect, backend, frontend, QA, DevOps, business analyst',
      'Branching, PR review and the quality gate (`TreatWarningsAsErrors`, SonarCloud)',
      'Where a change belongs: platform vs vendor module vs solution module',
      'The vc-build CLI in the loop: Install, Update, Compress, Pack',
      'Definition of done for a module change, including migrations and permissions'
    ],
    docs: [
      { label: 'Create a new module', page: 'Tutorials-and-How-tos/Tutorials/create-new-module-from-scratch' },
      { label: 'Global tools', page: 'CLI-tools/getting-started' }
    ],
    atoms: ['module-lifecycle', 'ef-core']
  },
  {
    id: 'deployment',
    name: 'Deployment schema',
    sub: 'From one container to a scaled-out, multi-instance cluster.',
    planned: [
      'The minimum viable deployment, and each piece you add as load grows',
      'What becomes mandatory when instance count goes above one (Redis, shared blob, bearer-token sharing)',
      'Module delivery: pre-installed vs installed at runtime, and the probing folder',
      'Zero-downtime concerns: migrations, module install locks, cache warm-up',
      'Docker Compose for development vs cloud topology for production'
    ],
    docs: [
      { label: 'Scale out on Azure', page: 'Fundamentals/Scalability/scaling-configuration-on-azure-cloud' },
      { label: 'Sharing bearer tokens across instances', page: 'Tutorials-and-How-tos/How-tos/sharing-bearer-tokens' },
      { label: 'Deploy from source code', page: 'CLI-tools/install-and-update-platform-and-modules' }
    ],
    atoms: ['distributed-lock', 'redis-cache-bus', 'module-catalog', 'health-checks']
  },
  {
    id: 'multi-store',
    name: 'Multi-store & multi-region',
    sub: 'One environment, many brands, many regions.',
    planned: [
      'Store as the unit of configuration, catalog scope and currency',
      'Per-store settings, themes and languages',
      'Multi-region catalogs, pricing and inventory',
      'Where tenancy is real in the platform and where it is convention'
    ],
    docs: [],
    atoms: ['settings', 'current-user', 'localizations']
  },
  {
    id: 'observability',
    name: 'Observability',
    sub: 'Knowing what production is doing before a customer tells you.',
    planned: [
      'Structured logging with Serilog, and what belongs in a log versus a metric',
      'Health checks as a deployment gate, not just an endpoint',
      'Tracing a request across XAPI, modules and background jobs',
      'The developer tools registry as an in-product diagnostics surface'
    ],
    docs: [
      { label: 'Logging', page: 'Fundamentals/Logging/overview' },
      { label: 'Health checks', page: 'Tutorials-and-How-tos/How-tos/health-checks' },
      { label: 'Debugging without source code', page: 'Tutorials-and-How-tos/How-tos/debugging' }
    ],
    atoms: ['logging', 'health-checks', 'developer-tools', 'job-progress']
  },
  {
    id: 'security-compliance',
    name: 'Security & compliance',
    sub: 'Identity, permissions and the surfaces that need locking down.',
    planned: [
      'The permission model end to end: declaration, role assignment, endpoint enforcement',
      'Authentication flows: password, API key, external SSO, service-to-service',
      'Restricting the Admin UI surface, and why the gate belongs server-side',
      'Scoped permissions for row-level restrictions',
      'Secrets, certificates and token sharing across instances'
    ],
    docs: [
      { label: 'Secure Web API', page: 'Fundamentals/Security/authorization/overview' },
      { label: 'Extending authorization policies', page: 'Fundamentals/Security/authorization/overview' },
      { label: 'Add an SSO provider', page: 'Fundamentals/Security/extensions/adding-azure-as-sso-provider' }
    ],
    atoms: ['permissions', 'authentication', 'authorization', 'current-user']
  },
  {
    id: 'testing',
    name: 'Testing strategy',
    sub: 'What to test at which level, with the conventions this repo uses.',
    planned: [
      'Unit tests with xUnit + Moq + FluentAssertions + AutoFixture, and the `//Arrange //Act //Assert` convention',
      'What to fake: repositories and `IPlatformMemoryCache` versus what to exercise for real',
      'Integration tests against a real database provider',
      'Testing event handlers and background job handlers in isolation',
      'The `VirtoCommerce.Testing` helpers'
    ],
    docs: [],
    atoms: ['generic-crud', 'domain-events', 'platform-memory-cache']
  },
];
