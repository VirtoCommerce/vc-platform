/* Solution architecture band — the top strip of the poster.
 *
 * `id` values are referenced by every atom's `layer` field, which drives the
 * hover-spotlight and the "atoms in this layer" list. Keep them stable.
 */
window.VC_MAP_ARCHITECTURE = [
  {
    id: 'solution',
    name: 'Your solution',
    hue: 90,
    sub: 'What a custom Virto project actually consists of: a little code you own, a lot of vendor artifacts you consume, and one file that pins the two together. You never fork the platform — that is the whole upgrade story.',
    tags: ['custom modules', 'vc-package.json', 'environments'],
    /* Three ordered views of the same solution: what it is made of, how it is built and
       shipped, and where it runs. Generalised from a real customer solution — structure
       only, no customer-specific modules or naming. */
    diagrams: [

      {
        kind: 'lanes',
        title: 'Solution architecture',
        legend: [
          { kind: 'custom', label: 'Your code' },
          { kind: 'virto', label: 'Virto Commerce' },
          { kind: 'infra', label: 'Edge & routing' },
          { kind: 'data', label: 'Service' },
          { dashed: true, label: 'Reached directly — outside the API path' }
        ],
        /* Two isolated paths run as parallel rails and only converge at the modules. The
           shopper reaches XAPI only; the employee reaches REST only, from behind a firewall.
           Columns marked `shared` span both lanes — that is the join. */
        /* API + Modules are one thing from the outside: the platform. Grouping them stops
           the diagram reading as five equally-independent stages. */
        groups: { platform: 'Virto Commerce' },
        columns: [
          { label: 'Presentation' },
          { label: 'Edge & routing' },
          { label: 'API', group: 'platform' },
          {
            label: 'Modules',
            group: 'platform',
            shared: true,
            scopes: [
              {
                title: 'Virto Commerce Modules',
                chip: '≈80% standard base',
                accent: 'virto',
                role: 'Released artifacts you consume, never fork — a sample below',
                count: '100+ modules',
                modules: ['Catalog', 'Pricing', 'Inventory', 'Cart', 'Order', 'Payment', 'Tax',
                          'Shipping', 'Customer', 'Marketing', 'Notifications', 'Assets',
                          'Search', 'Backup & restore', 'Security']
              },
              {
                title: 'Your Solution Modules',
                chip: '≈20% tailored',
                accent: 'custom',
                role: 'New modules, Virto Commerce modules extended via the Extensibility Framework, and integration adapters',
                modules: ['OrderExtension', 'Helpdesk', 'Trading calendar', 'ERP adapter',
                          'WMS adapter', 'PIM adapter']
              }
            ]
          },
          {
            label: 'Services',
            shared: true,
            nodes: [
              /* Reached straight from the load balancer's static route, in parallel with the
                 platform — it is not behind the API or the modules, which is exactly why the
                 storefront stays fast and deploys independently. */
              { name: 'Content storage', kind: 'infra', bypass: true, badge: '← direct from edge',
                role: 'Theme, site configuration, pages. Served in **parallel** with the platform, never through it.' },
              { name: 'Database', kind: 'data', role: 'SQL Server · PostgreSQL · MySQL', meta: 'EF Core' },
              { name: 'Search', kind: 'data', role: 'Elasticsearch · OpenSearch · Azure AI Search' },
              { name: 'Distributed cache', kind: 'data', role: 'Redis — invalidation, locks, SignalR backplane' },
              { name: 'Background jobs', kind: 'data', role: 'Hangfire (default) or RabbitMQ, behind `IBackgroundJob`' },
              { name: 'Custom', kind: 'data', role: 'Whatever this solution adds — payment gateway, tax service, ERP, email/SMS provider' }
            ]
          }
        ],
        lanes: [
          {
            label: 'Customer',
            chip: 'public',
            accent: 'shopper',
            cells: [
              { nodes: [
                { name: 'Storefront SPA', kind: 'custom', role: 'Vue 3 · Vite. Talks to **XAPI only** — no REST, no server-side middleware.', meta: 'GraphQL' }
              ]},
              { nodes: [
                { name: 'CDN', kind: 'infra', role: 'TLS, caching, static assets' },
                { name: 'SSR / prerender', kind: 'infra', role: 'Optional — crawlers and first paint' },
                { name: 'Load balancer', kind: 'infra', role: 'Splits by path — **static content** straight to Content storage, **API** to the platform. Both in parallel.' }
              ]},
              { nodes: [
                { name: 'Business Logic (XAPI)', kind: 'virto', role: 'GraphQL queries **and mutations** — `xCatalog` · `xCart` · `xOrder` · `xCMS` · `xProfile`' }
              ]}
            ]
          },
          {
            label: 'Employee',
            chip: 'internal',
            accent: 'employee',
            cells: [
              { nodes: [
                { name: 'Admin UI SPA', kind: 'virto', role: 'Commerce Manager · VC-Shell app. Talks to **REST API only**.', meta: 'REST' }
              ]},
              { nodes: [
                { name: 'Firewall', kind: 'infra', role: 'The back office is not published to the internet — REST reaches the platform from inside only' }
              ]},
              { nodes: [
                { name: 'REST /api', kind: 'virto', role: 'Full CRUD, per-endpoint permissions, Swagger-documented' }
              ]}
            ]
          }
        ]
      },

      {
        kind: 'pipeline',
        title: 'DevOps',
        note: '**The challenge** — for a solution architect, what is the real CI/CD path from source to a running environment? **In plain terms** — Git holds your custom source. CI builds your custom modules and publishes them to your Custom Modules artifact storage; the Virto modules already live in the Virto Commerce artifact storage. Your `vc-package.json` picks modules and versions from both. CD assembles them into one container image, which then deploys to each environment. **What this means for you** — `vc-package.json` is the control point: it chooses which artifacts, Virto and custom, go into the image, and one image then promotes across Dev → Stage → Prod.',
        /* The compact CI/CD view: two artifact sources built and published in parallel,
           converging on vc-package.json, which is the control point — it decides which
           artifacts go into the image that is then promoted across environments. */
        lanes: [
          {
            label: 'Yours',
            accent: 'custom',
            steps: [
              { name: 'Your Git', kind: 'custom', sub: 'custom source' },
              { connector: 'CI build' },
              { name: 'Custom modules', kind: 'custom', sub: 'Artifact storage' }
            ]
          },
          {
            label: 'Vendor',
            accent: 'virto',
            steps: [
              { name: 'Virto Commerce GitHub', kind: 'virto', sub: 'platform + modules source' },
              { connector: 'publish' },
              { name: 'Virto Commerce', kind: 'virto', sub: 'Artifact storage' }
            ]
          }
        ],
        rows: [
          {
            connector: 'both storages feed →',
            name: '`vc-package.json` — selects modules + versions',
            kind: 'select',
            sub: 'the control point'
          },
          {
            connector: 'CD · restore & assemble',
            name: 'Container image',
            kind: 'image',
            sub: 'one immutable artifact'
          },
          {
            connector: 'deploy',
            name: 'Environment — Dev · Stage · Prod',
            kind: 'env',
            sub: 'same image, different configuration'
          }
        ]
      },

      /* The two ends of the documented sizing range — S and XL — rendered in the same
         swimlane language so they can be read against each other. Everything the production
         column adds over the non-production one is a line item in the upgrade conversation.
         Source: Fundamentals/Scalability/scalability-options (S · M · L · XL) and
         Fundamentals/Scalability/scaling-configuration-on-azure-cloud (the settings). */

      {
        kind: 'lanes',
        title: 'Deployment — non-production (S)',
        note: 'The **Small** configuration: one frontend instance, one backend instance, background jobs in-process. Proof of concept, demo and developer environments. **No Redis**: with a single instance there is no other cache to invalidate, no SignalR backplane to share and no lock to take across processes.',
        groups: { platform: 'Same image · all roles' },
        legend: [
          { kind: 'custom', label: 'Your code' },
          { kind: 'virto', label: 'Virto Commerce' },
          { kind: 'infra', label: 'Edge & routing' },
          { kind: 'data', label: 'Service' },
          { dashed: true, label: 'Reached directly — no instance in front of it' }
        ],
        columns: [
          { label: 'Client' },
          { label: 'Frontend env' },
          {
            label: 'Backend env',
            group: 'platform',
            shared: true,
            nodes: [
              { name: 'Virto Commerce Platform App', kind: 'virto', meta: '×1',
                role: 'REST, XAPI and the Commerce Manager UI in **one instance**' },
              { name: 'Background process', kind: 'virto',
                role: 'In-process — `BackgroundJobs:Mode = Both`. No separate worker to deploy.' }
            ]
          },
          {
            label: 'Services',
            shared: true,
            nodes: [
              { name: 'SQL database', kind: 'data', meta: 'single db',
                role: 'One database, no elastic pool' },
              { name: 'Elasticsearch', kind: 'data',
                role: 'Or Lucene on a developer box — filesystem-bound, so single instance only' },
              { name: 'Application Insights', kind: 'infra',
                role: 'Telemetry from the first environment, not something added later' },
              { name: '3rd-party providers', kind: 'data',
                role: 'Payments, tax, logistics — called straight from the backend, sandbox credentials' }
            ]
          }
        ],
        lanes: [
          {
            label: 'Customer',
            chip: 'public',
            accent: 'shopper',
            cells: [
              { nodes: [
                { name: 'Browser', kind: 'infra', role: 'Multifactor authentication at sign-in' }
              ]},
              { nodes: [
                { name: 'Frontend app', kind: 'custom', meta: '×1',
                  role: 'Static content folder plus `vc-frontend`' }
              ]}
            ]
          },
          {
            label: 'Employee',
            chip: 'internal',
            accent: 'employee',
            cells: [
              { nodes: [
                { name: 'Browser', kind: 'infra', bypass: true, badge: '→ straight to the backend',
                  role: 'The Commerce Manager UI is served by the platform app itself — nothing sits in front of it' }
              ]},
              {}   /* deliberately empty: the employee path has no frontend instance */
            ]
          }
        ]
      },

      {
        kind: 'lanes',
        title: 'Deployment — production',
        note: 'The **Extra Large** configuration: the backend is split by workload, so background jobs, content managers and system traffic cannot degrade shoppers. Each environment runs **at least two instances** behind its own load balancer, all of them run the **same image** and share one resource pool. The published table stops at L (300 requests/sec, 15 000 orders/day); XL is beyond it. The documented topology names three environments — the **integration** lane is the common fourth split, worth making once a system pushes bulk traffic through the API.',
        groups: { platform: 'Same image · 4 roles' },
        legend: [
          { kind: 'custom', label: 'Your code' },
          { kind: 'virto', label: 'Virto Commerce' },
          { kind: 'infra', label: 'Edge & routing' },
          { kind: 'data', label: 'Service' },
          { dashed: true, label: 'Reached directly — bypasses the platform' }
        ],
        columns: [
          { label: 'Client' },
          { label: 'Edge & routing' },
          { label: 'Frontend' },
          { label: 'Backend · 2…n', group: 'platform' },
          {
            label: 'Shared resources',
            shared: true,
            nodes: [
              { name: 'SQL DB elastic pool', kind: 'data',
                role: 'Modules segmented across databases — Main, Cart, Order, Catalog, Customer' },
              { name: 'Redis', kind: 'data', meta: 'mandatory',
                role: '`RedisConnectionString` — cache invalidation backplane, SignalR backplane, distributed locks' },
              { name: 'Elasticsearch', kind: 'data',
                role: 'A cluster, not a single node — this is the catalogue read path' },
              { name: 'Azure Blob Storage', kind: 'data',
                role: 'Product images and assets. Filesystem assets do not survive more than one instance.' },
              { name: 'Application Insights', kind: 'infra',
                role: 'One telemetry target for all three environments' }
            ]
          }
        ],
        lanes: [
          {
            label: 'Customer',
            chip: 'public',
            accent: 'shopper',
            cells: [
              { nodes: [
                { name: 'Browser', kind: 'infra', role: 'Multifactor authentication at sign-in' }
              ]},
              { nodes: [
                { name: 'Azure CDN', kind: 'infra', bypass: true, badge: '→ direct to blob',
                  role: 'Product images and assets, served **from blob storage** rather than through the platform' },
                { name: 'Load balancer', kind: 'infra',
                  role: 'ARR affinity **on** for the frontend app' }
              ]},
              { nodes: [
                { name: 'Frontend app', kind: 'custom',
                  role: 'Static content folder plus `vc-frontend` — content, not a scaling lever' }
              ]},
              { nodes: [
                { name: 'Backend 4 Frontend', kind: 'virto', meta: '2…n',
                  role: 'Backend for frontend. `Mode: Producer` — enqueues jobs, runs none. ARR affinity **off**, `ScalabilityMode: None`.' }
              ]}
            ]
          },
          {
            label: 'Employee',
            chip: 'internal',
            accent: 'employee',
            cells: [
              { nodes: [
                { name: 'Browser', kind: 'infra', role: 'Commerce Manager UI' }
              ]},
              { nodes: [
                { name: 'Load balancer', kind: 'infra',
                  role: 'ARR affinity **on** — SignalR push notifications need sticky sessions' }
              ]},
              {},   /* the back office is served by the platform, not by a frontend instance */
              { nodes: [
                { name: 'Backend 4 Employee', kind: 'virto', meta: '2…n',
                  role: 'Commerce Manager and integrations. `Mode: Producer`, `ScalabilityMode: RedisBackplane`.' }
              ]}
            ]
          },
          {
            label: 'Integration',
            chip: 'system',
            accent: 'integration',
            cells: [
              { nodes: [
                { name: 'Integration middleware', kind: 'custom', meta: 'REST',
                  role: 'ERP, WMS and CRM traffic — machine-to-machine, no browser and no session' }
              ]},
              { nodes: [
                { name: 'Load balancer', kind: 'infra',
                  role: 'ARR affinity **off** — stateless calls, and no push notifications to keep pinned' }
              ]},
              {},   /* system traffic never touches the storefront */
              { nodes: [
                { name: 'Integration instance', kind: 'virto', meta: '2…n',
                  role: 'Bulk imports, webhook delivery and polling, kept off the shopper path. `Mode: Producer`.' }
              ]}
            ]
          },
          {
            label: 'Background jobs',
            chip: 'no traffic',
            accent: 'jobs',
            cells: [
              {},   /* nothing calls this environment — it drains the queue */
              {},
              {},
              { nodes: [
                { name: 'Job workers', kind: 'virto', meta: '2…n',
                  role: 'The only environment running the engine: `Mode: Worker`. Sized for CPU and scaled on queue depth.' }
              ]}
            ]
          }
        ]
      },
    ],
    bullets: [
      'The ratio surprises people: a mature solution is mostly configuration plus a handful of custom modules, sitting on dozens of vendor modules. If you are writing a lot of code, check whether a lower extensibility level would do.',
      'Your custom module has the same three-project shape as a vendor one — `Core` / `Data` / `Web` — and loads through the same manifest and dependency graph. There is no "application project" that is special.',
      'Extend in this order: no-code (dynamic properties, settings, permissions) → API (REST/GraphQL, webhooks, event handlers) → native (`AbstractTypeFactory` override). Each step up costs more at upgrade time.',
      '`vc-package.json` is the boundary between what you own and what you consume. It is the file that makes an environment reproducible, and the first thing to read when two environments behave differently.',
      'The storefront is a separate deliverable with its own build and deploy — a platform release does not update it, and it does not ship inside the container image.'
    ],
    gotchas: [
      'Never edit vendor module source, even locally to "just test something". The moment you do, you own that module forever and upgrades become merges.',
      'Manifest dependencies are caret SemVer ranges, so an unpinned range can resolve differently on two machines. `vc-package.json` is what makes the result repeatable — commit it.',
      'Prefix anything you add to a vendor type with your own abbreviation (`AbcStatus`). A future vendor property with the same name is a collision you cannot rename your way out of.',
      'A custom module owning schema needs a migration per database provider it must support — not just the one you develop against.',
      'Dev and prod differ by provider modules as much as by configuration: Lucene and filesystem assets work on one instance and quietly break on several.'
    ],
    docs: [
      { label: 'Package management (vc-package.json)', page: 'CLI-tools/package-management' },
      { label: 'Create a module from scratch', page: 'Tutorials-and-How-tos/Tutorials/create-new-module-from-scratch' },
      { label: 'Extensibility overview', page: 'Extensibility/overview' },
      { label: 'Release strategy', page: 'Updating-Virto-Commerce-Based-Project/release-strategy-overview' },
      { label: 'Scalability options (S · M · L · XL)', page: 'Fundamentals/Scalability/scalability-options' },
      { label: 'Scaling configuration on Azure', page: 'Fundamentals/Scalability/scaling-configuration-on-azure-cloud' },
      { label: 'Key extensibility points', page: 'Extensibility/key-extensibility-points' },
      { label: 'Storefront architecture (docs site)', href: 'https://docs.virtocommerce.org/storefront/developer-guide/latest/architecture/' }
    ]
  },

  {
    id: 'channels',
    name: 'Channels',
    hue: 275,
    sub: 'Every surface that talks to the platform, and the protocol each one speaks. Presentation is fully separated from business logic, so every channel — including the back office — is just an API client.',
    tags: ['vc-frontend', 'GraphQL', 'Admin UI', 'AI agents'],
    /* Vertical by design: sales channels on top calling down into the platform, back office
       and integrations below calling up into it. The arrow on each connector shows who calls
       whom — everything points at the platform, because nothing else holds business logic. */
    diagrams: [{
      kind: 'stack',
      title: 'Channels → platform',
      rows: [
        {
          title: 'Sales channels',
          hint: 'customer-facing',
          nodes: [
            { name: 'Virto Commerce Frontend', sub: '`vc-frontend` — Vue 3 · Vite · TS · Tailwind. Storefront-less, no middleware.', via: 'GraphQL', viaKind: 'graphql' },
            { name: 'Custom storefront', sub: 'Your own SPA or native app against the same schema', via: 'GraphQL', viaKind: 'graphql' },
            { name: 'Mobile app · kiosk · chatbot', sub: 'First-class API clients, not special cases', via: 'GraphQL', viaKind: 'graphql' },
            { name: 'Marketplaces & partner feeds', sub: 'Catalogue and inventory pushed outward', via: 'REST', viaKind: 'rest' },
            { name: 'AI agents', sub: 'Emerging — capabilities as `MCP` tools, `UCP` as the commerce layer above', via: 'MCP → UCP', viaKind: 'trend', trend: true }
          ]
        },
        {
          connector: 'XAPI GraphQL · REST',
          connectorDir: 'down',
          target: 'Virto Commerce Platform',
          sub: 'XAPI — a business API of GraphQL queries and mutations · REST `/api` for full CRUD · modules behind both'
        },
        {
          connector: 'REST /api',
          connectorDir: 'up',
          title: 'Admin UI & back office',
          hint: 'internal · full CRUD',
          nodes: [
            { name: 'Admin UI', sub: 'Commerce Manager — AngularJS 1.8 blades, ships inside the platform', via: 'REST', viaKind: 'rest' },
            { name: 'Vendor Portal', sub: 'VC-Shell — Vue 3 + Module Federation, used by Marketplace and vertical apps', via: 'REST', viaKind: 'rest' },
            { name: 'Custom UI', sub: 'Standalone SPA served through the platform `<apps>` mechanism', via: 'REST', viaKind: 'rest' }
          ]
        },
        {
          connector: 'REST in · webhooks out',
          connectorDir: 'both',
          title: 'Integrations',
          hint: 'no human in the loop',
          nodes: [
            { name: 'Integration middleware', sub: 'ERP · WMS · CRM · PIM. Translates between models so neither side compromises.', via: 'REST', viaKind: 'rest' },
            { name: 'EventBus & outbound webhooks', sub: 'The platform calling you, instead of you polling it', via: 'HTTP', viaKind: 'plain' }
          ]
        }
      ]
    }],
    bullets: [
      '`vc-frontend` — Vue 3 · TypeScript · Vite · TailwindCSS · Yarn 4. Storefront-less: it talks straight to XAPI over GraphQL, with no ASP.NET middleware in between. Follows Atomic Design, which is where the atom/molecule vocabulary on this poster comes from.',
      'Admin UI ("Commerce Manager") — AngularJS 1.8.3 + Webpack 5 + SASS, using the blades navigation pattern. Ships inside `Platform.Web/wwwroot`; each module contributes its own scripts, templates and localizations.',
      'VC-Shell — Vue 3 + Module Federation, the host behind the Vendor Portal and other vertical apps. Loads plugin remotes from the canonical `GET /api/apps/{appId}/manifest`.',
      'Custom standalone SPAs — served via the platform `<apps>` mechanism, and now extensible through the same manifest contract as the other two hosts.',
      'AI agents are a trend worth designing for, not a shipped feature. `MCP` exposes capabilities as tools an agent can call; `UCP` (Universal Commerce Protocol — Google with Shopify, announced January 2026) defines the commerce conversation above it, and UCP capabilities map 1:1 to MCP tools. So an MCP server wrapping your commerce capabilities is the practical first step.'
    ],
    gotchas: [
      'The Admin UI is back-office only. It is never the customer-facing surface, and its AngularJS age says nothing about the storefront stack.',
      'Admin UI assets are built into the source `wwwroot/dist`, but a running platform serves them from its publish folder — a webpack build alone does not update a running instance.',
      'All three back-office hosts converge on one contract: `GET /api/apps/{appId}/manifest`, driven by the same `module.manifest` dependency graph as .NET install order. `POST /api/frontend-modules` survives only as a deprecated alias.',
      'Nothing in the platform implements MCP or UCP today — the AI-agent node is dashed for that reason. Treat any agent as an untrusted client and put authorization in front of every capability you expose.'
    ],
    docs: [
      { label: 'Architecture reference', page: 'Back-End-Architecture/02-conceptual-overview' },
      { label: 'Back-office modularity', page: 'Fundamentals/Modularity/07-backoffice-app-modularity' },
      { label: 'VC-Shell implementation spec', page: 'Fundamentals/Modularity/07-backoffice-app-modularity' },
      { label: 'Blades and navigation', page: 'Platform-Manager/Extensibility-Points/blades-and-navigation' },
      { label: 'Universal Commerce Protocol (ucp.dev)', href: 'https://ucp.dev/' }
    ]
  },

  {
    id: 'api-edge',
    name: 'API edge',
    hue: 205,
    sub: 'Where the outside world meets the platform. Two shapes with two jobs: GraphQL as the business API a channel talks to — queries and mutations both — and REST for full CRUD and the back office.',
    tags: ['XAPI /graphql', 'REST /api', 'Swagger', 'SignalR'],
    bullets: [
      'XAPI (Experience API) — a BFF built on GraphQL, shipped as modules: `xCatalog`, `xCart`, `xOrder`, `xCMS`, `xProfile` over the shared `VirtoCommerce.xApi` core. It is a full business API — queries *and* mutations, so a storefront never needs REST — and one round trip returns exactly the shape a screen needs.',
      'REST — the complete surface. Every module exposes controllers under `/api/…`; this is what the Admin SPA and integration middleware use.',
      'Swagger / OpenAPI — generated per module plus a combined document, so a module\'s API is browsable and client-generatable the moment it loads.',
      'SignalR — server-to-client push for long-running work (`/pushNotificationHub`), with a Redis or Azure SignalR backplane when scaled out.',
      'Outbound WebHooks — the platform calling you, rather than you polling it.'
    ],
    matrixTitle: 'Choosing between them',
    matrix: [
      { name: 'Storefront', desc: 'XAPI / GraphQL. A business API: purpose-shaped queries and mutations, batched, one round trip per screen.' },
      { name: 'Back office & admin', desc: 'REST. Full CRUD, per-endpoint permissions, Swagger-documented.' },
      { name: 'Integration & ETL', desc: 'REST + WebHooks, usually through integration middleware rather than point-to-point.' },
      { name: 'Progress & live updates', desc: 'SignalR push notifications — never poll a job endpoint in a loop.' }
    ],
    gotchas: [
      'MVC is configured with `AddNewtonsoftJson`, so REST serialization is Newtonsoft — not `System.Text.Json`. Custom converters must be written against Newtonsoft.',
      'The platform uses MVC controllers throughout; there are no Minimal API endpoints to copy as a pattern.',
      'XAPI lives in a separate repository (`vc-module-experience-api`) — it is a module set, not part of the platform core.'
    ],
    docs: [
      { label: 'Swagger endpoints', page: 'Tutorials-and-How-tos/How-tos/swagger-api' },
      { label: 'Secure Web API', page: 'Fundamentals/Security/authorization/overview' },
      { label: 'Polymorphic types in Swagger', page: 'Tutorials-and-How-tos/How-tos/type-inheritance-support-in-swagger' }
    ]
  },

  {
    id: 'modules',
    name: 'Modules',
    hue: 155,
    sub: 'The unit of everything. A modular monolith of vertical slices: one bounded context per module, split into three projects, discovered and loaded at runtime.',
    tags: ['Core', 'Data', 'Web', 'module.manifest'],
    bullets: [
      '`Module.Core` — domain models, service interfaces, events, `ModuleConstants` (permissions, settings, literals). Distributable as a NuGet package, so other modules can depend on your contracts without your implementation.',
      '`Module.Data` — EF Core entities, migrations, repositories, service implementations, event handlers, cache regions. Also NuGet-distributable.',
      '`Module.Web` — REST controllers, Admin SPA assets, localizations, `Module.cs`, `module.manifest`. Application layer only, never a package.',
      'Relationships between modules: **Uses** (calls another module\'s API), **Extends** (overrides a type via `AbstractTypeFactory`), **Reacts to** (handles integration events). Prefer *reacts to* — it is the only one that does not create a compile-time dependency.',
      'Namespacing is mechanical: `VirtoCommerce.{Module}.{Core|Data|Web}`.'
    ],
    matrixTitle: 'Types of module you will actually build',
    matrix: [
      { name: 'All-in-one', desc: 'XAPI + services + CRUD + Admin UI in one bounded context. The default for a real feature — Catalog, Order and Pricing are all this shape.' },
      { name: 'CRUD module', desc: 'Domain + EF Core + REST + Admin UI, no GraphQL. Back-office-only concerns: reference data, internal registries.' },
      { name: 'XAPI module', desc: 'GraphQL schema, queries, mutations and types over services that already exist. Adds a storefront experience without touching the domain.' },
      { name: 'Provider module', desc: 'Implements a registration contract — payment, shipping or tax provider. Small, focused, swappable per store.' },
      { name: 'Engine module', desc: 'Adapts one port to one technology, selected in configuration: search provider (Elasticsearch / Lucene / Azure Search), or background-job engine.' },
      { name: 'Integration module', desc: 'Talks to an external system (ERP, WMS, CRM), usually driven by event handlers and background jobs rather than by requests.' }
    ],
    gotchas: [
      'A module without a valid `module.manifest` will not load — there is no convention-based fallback.',
      'Never edit vendor module source. Derive a type and register the override, handle an event, or use a dynamic property; anything else you cannot upgrade.',
      'When you add a property to a vendor type, prefix it with your solution abbreviation so a future vendor property cannot collide with it.',
      'Manifest dependencies use caret SemVer ranges (`^1.2.3` means `>=1.2.3 <2.0.0`), and `platformVersion` must be pinned.'
    ],
    docs: [
      { label: 'Modularity', page: 'Fundamentals/Modularity/01-overview' },
      { label: 'Essential modularity', page: 'Fundamentals/Modularity/01-overview' },
      { label: 'Create a new module', page: 'Tutorials-and-How-tos/Tutorials/create-new-module-from-scratch' },
      { label: 'Extensibility overview', page: 'Extensibility/overview' }
    ]
  },

  {
    id: 'platform',
    name: 'Platform',
    hue: 20,
    sub: 'The shared substrate every module builds on. Most atoms on this poster live here — this is the layer worth knowing by heart.',
    tags: ['Core', 'Data', 'Caching', 'Security', 'Modules', 'Web'],
    bullets: [
      '`Platform.Core` — contracts and primitives only: caching, settings, dynamic properties, events, jobs, CRUD abstractions, `AbstractTypeFactory`, security contracts. Almost every atom in this map is declared here.',
      '`Platform.Data` — EF Core infrastructure, the platform `DbContext` and repositories, plus one project per database provider (`.SqlServer`, `.PostgreSql`, `.MySql`).',
      '`Platform.Caching` — `PlatformMemoryCache`, cache regions, and the Redis-backed variant that keeps per-instance memory caches coherent.',
      '`Platform.Security` — ASP.NET Core Identity, OpenIddict, permissions, API keys, external sign-in, admin-UI access policy.',
      '`Platform.Modules` — discovery, probing folder, dependency resolution, install/uninstall.',
      '`Platform.DistributedLock` — Redis (RedLock) or a no-op fallback, so single-instance development needs no Redis.',
      '`Platform.Web` — the host: startup wiring, platform REST controllers, the Admin SPA, health checks at `/health`.'
    ],
    gotchas: [
      'Do not modify platform source in a solution. Customisation belongs in modules — that is the whole seamless-upgrade story.',
      'A DI registration made later wins. That is the extension mechanism for services, and also the way you accidentally replace something you did not mean to.'
    ],
    docs: [
      { label: 'Architecture reference', page: 'Back-End-Architecture/02-conceptual-overview' },
      { label: 'Database agnostic', page: 'Fundamentals/Persistence/DB-Agnostic/overview' },
      { label: 'Essential caching', page: 'Fundamentals/Caching/01-overview' }
    ]
  },

  {
    id: 'integration',
    name: 'Integration',
    hue: 320,
    sub: 'How the platform joins an existing business ecosystem. API-first plus events, with middleware doing the translating rather than either side compromising its model.',
    tags: ['EventBus', 'WebHooks', 'middleware', 'ERP/WMS/CRM'],
    bullets: [
      'Integration middleware — a translation layer between the platform and ERP / WMS / CRM / PIM. Keeps foreign models out of your domain and lets each side change independently.',
      'EventBus module — bridges in-process domain events onto external transports (Azure Service Bus, RabbitMQ, Kafka), so other systems can subscribe without calling you.',
      'WebHooks module — configurable outbound HTTP callbacks per event type.',
      'Master vs reference data — decide per entity which system owns the truth. This single decision determines the direction of every sync you will build.'
    ],
    gotchas: [
      'Integration events are at-least-once in practice. Handlers must be idempotent; assume every message can arrive twice.',
      'EventBus and WebHooks are installable modules, not platform core — the tiles on this poster reflect that.',
      'Point-to-point integration between the platform and each external system is the trap middleware exists to prevent.'
    ],
    docs: [
      { label: 'Architecture reference', page: 'Back-End-Architecture/02-conceptual-overview' },
      { label: 'Extending using events', page: 'Fundamentals/Event-Driven-Development/using-domain-events' }
      
    ]
  },

  {
    id: 'infrastructure',
    name: 'Infrastructure',
    hue: 230,
    sub: 'What must exist for the platform to run, and what each piece is actually for. Cloud-agnostic: Azure, AWS, GCP or on-premise.',
    tags: ['SQL', 'Redis', 'Elasticsearch', 'Blob', 'SignalR'],
    bullets: [
      'Relational database — SQL Server, PostgreSQL or MySQL. One provider per deployment, chosen by connection string and provider package; migrations exist per provider.',
      'Redis — three distinct jobs, worth separating in your head: cache-invalidation bus, distributed lock (RedLock), and SignalR backplane. It is not used as a shared cache store.',
      'Search engine — Elasticsearch (or Lucene / Azure Search via the matching provider module). The catalog read path depends on it; it is not optional at scale.',
      'Blob storage — product images, imports, exports and other assets, behind a file-system or cloud provider.',
      'CDN — static assets and product images, in front of blob storage.'
    ],
    gotchas: [
      'Scaling out to more than one instance makes Redis mandatory: without it, per-instance memory caches drift and module installation has no distributed lock.',
      'Redis holding cache *invalidation messages* rather than cache *values* surprises almost everyone. Losing Redis costs coherence, not the cache itself.'
    ],
    docs: [
      { label: 'Scalability', page: 'Fundamentals/Scalability/scalability-options' },
      { label: 'Scale out on Azure', page: 'Fundamentals/Scalability/scaling-configuration-on-azure-cloud' },
      { label: 'Health checks', page: 'Tutorials-and-How-tos/How-tos/health-checks' },
      { label: 'Search', page: 'Fundamentals/Indexed-Search/overview' }
    ]
  }
];
