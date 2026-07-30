/* Molecules — composite topics built out of many atoms.
 *
 * These are deliberately reserved placeholders. The tiles exist so the shape of the whole
 * picture is visible and the gaps are honest; the content is written later, one molecule at
 * a time. `atoms` lists the atom ids a molecule will compose — unresolved ids are simply
 * skipped, so it is safe to list an atom before it is authored.
 */
window.VC_MAP_MOLECULES = [
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
      { label: 'Architecture reference', href: '../fundamentals/architecture-reference.md' },
      { label: 'Modularity', href: '../modularity.md' }
    ],
    atoms: ['module-lifecycle', 'generic-crud', 'domain-events']
  },
  {
    id: 'background-processing-hub',
    name: 'Background processing hub',
    sub: 'The in-flight extraction of the job engine behind one port.',
    planned: [
      'Why Hangfire was removed as a hard platform dependency',
      'The `IJobEngine` port and its adapters (Hangfire as reference engine, RabbitMQ as the push/scale-to-zero proof)',
      'What happens when no engine is installed, and why the platform still boots',
      'The migration path for modules calling Hangfire directly',
      'Idempotency, dedup keys, claim-check for large payloads, and one OpenTelemetry model across engines'
    ],
    docs: [
      { label: 'Design spec (2026-06-06)', href: '../superpowers/specs/2026-06-06-background-processing-hub-design.md' }
    ],
    atoms: ['background-jobs', 'recurring-jobs', 'map-reduce-jobs', 'job-progress', 'fire-and-forget', 'distributed-lock']
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
      { label: 'Search', href: '../fundamentals/search.md' }
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
      { label: 'New payment method', href: '../fundamentals/extensibility/new-payment-method-registration.md' },
      { label: 'New shipping method', href: '../fundamentals/extensibility/new-shipping-method-registration.md' },
      { label: 'New tax provider', href: '../fundamentals/extensibility/new-tax-provider-registration.md' }
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
      { label: 'Extensibility overview', href: '../fundamentals/extensibility/overview.md' },
      { label: 'Extending domain models', href: '../fundamentals/extensibility/extending-domain-models.md' }
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
      { label: 'Create a new module', href: '../developer-guide/create-new-module.md' },
      { label: 'Global tools', href: '../developer-guide/global-tools.md' }
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
      { label: 'Scale out on Azure', href: '../techniques/how-scale-out-platform-on-azure.md' },
      { label: 'Sharing bearer tokens across instances', href: '../techniques/sharing-bearer-tokens-across-platform-instances.md' },
      { label: 'Deploy from source code', href: '../developer-guide/deploy-from-source-code.md' }
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
    docs: [
      { label: 'B2B multi-regional', href: '../architecture-center/B2B-multiregional.md' }
    ],
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
      { label: 'Logging', href: '../developer-guide/logging.md' },
      { label: 'Health checks', href: '../techniques/healthchecks.md' },
      { label: 'Debugging without source code', href: '../techniques/debugging-without-source-code.md' }
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
      { label: 'Secure Web API', href: '../fundamentals/make-secure-webapi.md' },
      { label: 'Extending authorization policies', href: '../fundamentals/extensibility/extending-authorization-policies.md' },
      { label: 'Add an SSO provider', href: '../techniques/add-new-sso-provider.md' }
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
  {
    id: 'performance',
    name: 'Performance & scale-out',
    sub: 'The playbook when it gets slow, in the order worth trying.',
    planned: [
      'The usual suspects: missing cache tokens, N+1 over EF Core, per-call index lookups, chatty XAPI resolvers',
      'Reading the caching layer as a whole: region tokens, request-scoped cache, Redis coherence',
      'Scaling out: what must be shared, what must be locked, what must not be cached',
      'Where to measure first, and which numbers actually predict user-visible latency'
    ],
    docs: [
      { label: 'Scalability', href: '../fundamentals/scalability.md' },
      { label: 'Essential caching', href: '../fundamentals/essential-caching.md' }
    ],
    atoms: ['platform-memory-cache', 'cache-regions', 'request-scoped-cache', 'ef-core']
  }
];
