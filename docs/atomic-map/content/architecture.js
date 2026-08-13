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

      /* The lifecycle's second half. DevOps above says how a release is built; this says what
         happens when Virto ships the next one — which is the question a customer asks first and
         the deck exists to answer. Source: the release-strategy presentation. */
      {
        kind: 'section',
        title: 'Update process',
        note: 'Virto is **100+ independent modules plus the platform**, each shipping continuously — so there is no annual big-bang upgrade to schedule. **Stable** publishes roughly every three months with full regression, E2E and load testing, and is the CLI default; **Edge** publishes daily with automated testing only, carries the newest features, and may include breaking changes. Run Stable in production and reach for Edge selectively, when one feature genuinely cannot wait.\n\n**The default update is a manifest edit, not a code change.** Most of the time you bump a version — or add a module — in `vc-package.json`, run `vc-build update`, rebuild your own modules, run your tests and deploy. The Virto layers swap in; your work stays on your own modules. That only holds because you **extend rather than modify**: a module is open for extension and closed for modification, so an update flows around your code instead of through it.',
        items: [
          '**When custom code is affected, the size is known up front.** **S** — add a class or an extension point because your customization needs a new feature. **M** — resolve obsolete APIs and apply the documented breaking-change fixes. **L** — follow the published update path when the change comes from Microsoft and .NET.',
          '**Three things make a step genuinely bigger**, and all three are flagged in advance: a **.NET LTS move** roughly every two years, which means a new target framework and refreshed dependencies across the solution; a **yearly refresh** of the third-party libraries Virto builds on; and a **security finding** that occasionally forces a breaking fix. Every breaking change ships with migration notes.',
          '**Cadence is your choice, within limits.** Keep inside the latest two Stable releases — those are the two that receive hotfixes. Updating on every Stable release is the recommendation; every sprint, every month or once a year are all workable, but the longer the gap the more change arrives at once.',
          '**A hotfix is a patch, not a version jump.** Virto issues hotfixes for the two most recent Stable releases: bump the patch version in the manifest and update. No new features, no migration.',
          '**A feature you need before it is Stable can be adopted onto Stable now.** New features land on Edge, harden, then roll into the next Stable bundle — and when they do, your next update folds in the version you were already running, tested.',
          '**Test your customizations, not the platform.** Virto already runs regression, E2E and load tests against the platform and the modules; re-testing them is spent effort. Automate the part only you can — `vc-testing-module` is the open example to copy.',
          '**Even a long drift is recoverable.** The deck cites a 10-year-old VC 2.x solution brought to the latest Stable with a new frontend: 17 modules, one developer, two months, now live.'
        ]
      },

      /* The two ends of the documented sizing range — S and XL — rendered in the same
         swimlane language so they can be read against each other. Everything the production
         column adds over the non-production one is a line item in the upgrade conversation.
         Source: Fundamentals/Scalability/scalability-options (S · M · L · XL) and
         Fundamentals/Scalability/scaling-configuration-on-azure-cloud (the settings). */

      {
        kind: 'topology',
        title: 'Deployment — non-production',
        note: 'The **non-production** configuration — Small in the sizing guide: one frontend instance, one backend instance, and **background jobs inside that same process** — `BackgroundJobs:Mode = Both`, no worker to deploy. The dashed box is the image: REST, XAPI, the Commerce Manager UI and the job engine all restart together, which is exactly why this shape is for proof of concept, demo and developer environments rather than production. **No Redis**: with a single instance there is no other cache to invalidate, no SignalR backplane to share and no lock to take across processes.',
        cols: 4,
        legend: [
          { kind: 'custom', label: 'Your code' },
          { kind: 'virto', label: 'Virto Commerce' },
          { kind: 'infra', label: 'Edge & routing' },
          { kind: 'data', label: 'Service' },
          { dashed: true, label: 'Reached directly — no instance in front of it' }
        ],
        regions: [
          { id: 'cloud', label: 'One environment', outer: true, col: [2, 4], row: [1, 5] },
          /* One image, drawn the way the Composable view draws one: tight to the card, dashed,
             unlabelled. Here it contains every role at once, jobs included. */
          { id: 'platformbox', tight: true, col: [3, 3], row: [1, 1] }
        ],
        nodes: [
          { id: 'customer', name: 'Browser', sub: 'Customer', meta: 'MFA', kind: 'infra', col: 1, row: 1 },
          { id: 'employee', name: 'Browser', sub: 'Employee · Commerce Manager', kind: 'infra', col: 1, row: 3 },
          { id: 'frontend', name: 'Frontend app', sub: 'Static content · `vc-frontend`', meta: '×1', kind: 'custom', col: 2, row: 1 },
          { id: 'platform', name: 'Platform App', sub: 'REST · XAPI · Manager · jobs', meta: '×1 · all roles', kind: 'virto', col: 3, row: 1 },
          { id: 'sql', name: 'SQL database', sub: 'One database, no pool', kind: 'data', col: 4, row: 1 },
          { id: 'search', name: 'Elasticsearch', sub: 'Or Lucene on a developer box', kind: 'data', col: 4, row: 2 },
          { id: 'insights', name: 'Application Insights', sub: 'Telemetry from the first environment', kind: 'data', col: 4, row: 3 },
          { id: 'providers', name: '3rd-party providers', sub: 'Payments · tax · logistics', kind: 'data', col: 4, row: 4 },
          /* The same dependency as production, with a different provider behind it:
             `FileSystemAssets` on a developer box, blob storage once there is more than one
             instance. Drawing it here is what makes that a provider swap rather than a new
             service appearing at the production tier. */
          { id: 'blob', name: 'Blob storage', sub: 'Local disk — `FileSystemAssets`', kind: 'data', col: 4, row: 5 }
        ],
        edges: [
          { from: 'customer', to: 'frontend', label: 'HTTPS' },
          { from: 'frontend', to: 'platform', label: 'GraphQL · XAPI' },
          /* Straight into the backend: nothing is deployed in front of the back office. */
          { from: 'employee', to: 'platform', label: 'REST', bypass: true },
          { from: 'platform', to: 'sql', label: 'EF Core' },
          { from: 'platform', to: 'search', label: 'catalog reads' },
          /* Fanned turns, so the outbound lines do not stack into one stroke. */
          { from: 'platform', to: 'insights', label: 'telemetry', turnOffset: -13 },
          { from: 'platform', to: 'providers', label: 'callbacks', turnOffset: -26 },
          { from: 'platform', to: 'blob', label: 'assets', turnOffset: -39 }
        ]
      },

      {
        kind: 'topology',
        title: 'Deployment — production',
        note: 'The **production** configuration — Extra Large in the sizing guide: the backend is split by workload, so background jobs, content managers and system traffic cannot degrade shoppers. Each environment runs **at least two instances** behind the load balancer, all of them run the **same image**, and all four use the whole shared pool — one line each is drawn to keep the picture readable. The documented topology names three environments; the **integration** instance is the common fourth split, worth making once a system pushes bulk traffic through the API.\n\n**Big is not the same as expensive.** Nothing here is a large machine: the unit is a **small container — roughly 1 vCPU and 512 MB** — and the topology gets its capacity from having several of them rather than from any one being big. .NET reads its container limits and sizes the GC heap to them, so an instance behaves predictably at that size, and the platform is designed so the request path holds no state worth protecting: cache coherence, the SignalR backplane and locks all live in Redis, so an instance can be added, removed or replaced without ceremony. What that buys is cost that tracks load — scale the four environments on their own curves, and the ones that are quiet stay small. What it costs is discipline: work that needs memory in bursts (a reindex, a bulk import) belongs on the jobs environment, sized for it, and not on a 512 MB request host.',
        cols: 5,
        legend: [
          { kind: 'custom', label: 'Your code' },
          { kind: 'virto', label: 'Virto Commerce' },
          { kind: 'infra', label: 'Edge & routing' },
          { kind: 'data', label: 'Service' },
          { dashed: true, label: 'Reached directly — bypasses the platform' }
        ],
        regions: [
          { id: 'cloud', label: 'Cloud environment', outer: true, col: [2, 5], row: [1, 5] },
          { id: 'platformbox', label: 'Same image · 4 roles', col: [4, 4], row: [1, 5] },
          { id: 'sharedbox', label: 'Shared by every role', accent: 'shared', col: [5, 5], row: [1, 5] }
        ],
        nodes: [
          { id: 'cdn', name: 'CDN', sub: 'Static assets at the edge', kind: 'infra', col: 2, row: 1 },
          { id: 'blob', name: 'Blob storage', sub: 'Product images · assets', kind: 'data', col: 5, row: 1 },

          { id: 'customer', name: 'Browser', sub: 'Customer', meta: 'MFA', kind: 'infra', col: 1, row: 2 },
          { id: 'frontend', name: 'Frontend app', sub: 'Static content · `vc-frontend`', kind: 'custom', col: 3, row: 2 },
          { id: 'bff', name: 'Backend 4 Frontend', sub: '`Mode: Producer` · ARR off', meta: '2…n', kind: 'virto', col: 4, row: 2 },
          { id: 'search', name: 'Elasticsearch', sub: 'A cluster, not one node', kind: 'data', col: 5, row: 2 },

          { id: 'lb', name: 'Load balancer', sub: 'Routes by host and path', kind: 'infra', col: 2, row: 3 },
          { id: 'bemp', name: 'Backend 4 Employee', sub: '`RedisBackplane` · ARR on', meta: '2…n', kind: 'virto', col: 4, row: 3 },
          { id: 'redis', name: 'Redis', sub: 'Invalidation · backplane · locks', meta: 'mandatory', kind: 'data', col: 5, row: 3 },

          { id: 'employee', name: 'Browser', sub: 'Employee · Commerce Manager', kind: 'infra', col: 1, row: 4 },
          { id: 'bint', name: 'Integration instance', sub: 'Bulk imports · webhooks', meta: '2…n', kind: 'virto', col: 4, row: 4 },
          { id: 'sql', name: 'SQL DB elastic pool', sub: 'Main · Cart · Order · Catalog · Customer', kind: 'data', col: 5, row: 4 },

          { id: 'systems', name: 'Integration middleware', sub: 'ERP · WMS · CRM', meta: 'REST', kind: 'custom', col: 1, row: 5 },
          { id: 'jobs', name: 'Job workers', sub: 'The only `Mode: Worker`', meta: '2…n', kind: 'virto', col: 4, row: 5 },
          { id: 'insights', name: 'Application Insights', sub: 'One target for every environment', kind: 'data', col: 5, row: 5 }
        ],
        edges: [
          /* The top row is a channel, not a stage: it is the one path that skips the platform,
             so it is drawn clear of everything else. */
          { from: 'customer', to: 'cdn', label: 'assets' },
          { from: 'cdn', to: 'blob', label: 'images · assets', bypass: true },

          { from: 'customer', to: 'lb', label: 'HTTPS' },
          { from: 'employee', to: 'lb', label: 'HTTPS' },
          { from: 'systems', to: 'lb', label: 'REST', turnOffset: -12 },

          { from: 'lb', to: 'frontend', label: 'ARR on' },
          { from: 'frontend', to: 'bff', label: 'GraphQL · XAPI' },
          { from: 'lb', to: 'bemp', label: 'REST · ARR on' },
          { from: 'lb', to: 'bint', label: 'REST · ARR off' },

          { from: 'bff', to: 'search', label: 'catalog reads' },
          { from: 'bemp', to: 'redis', label: 'SignalR backplane' },
          { from: 'bint', to: 'sql', label: 'bulk writes' },
          { from: 'jobs', to: 'insights', label: 'telemetry' },
          /* The queue itself: producers enqueue into the job storage, the worker drains it. */
          { from: 'bff', to: 'sql', label: 'enqueue', turnOffset: -15, labelDy: -22 },
          { from: 'jobs', to: 'sql', label: 'drain queue', turnOffset: -30 }
        ]
      },

      /* Between the two: the deployment views show shapes, this says which dial to turn. Out
         before up, level by level, with the one lever per level that actually moves the number. */
      {
        kind: 'section',
        title: 'Scaling',
        note: 'Two directions, and they are not equal. **Out** — more instances — is the one this platform is built for: the request path is stateless, so capacity is a replica count. **Up** — a bigger instance — is the fallback for the things that cannot be split: a single database writer, one reindex, one large import.\n\nThe order matters. Scale **out** until something refuses to, then scale **up** only that thing. Going up first buys headroom you pay for around the clock and hides the coupling that stopped you scaling out.',
        items: [
          '**Frontend** — out, without limit. Static content plus `vc-frontend` behind a CDN: replicas are interchangeable and the CDN absorbs most of the load before it arrives. Nothing to scale up.',
          '**API and PBC hosts** — out, by replica count, and independently per environment. This is where the platform expects you to spend: `2…n` small containers per role, each on its own curve. Up only when a single request genuinely needs more memory than a small container has.',
          '**Background jobs** — out by worker count, up by machine shape. The one environment where **up** is often the right answer first: a reindex or a bulk import is one large unit of work, and splitting it across workers needs a map/reduce job rather than another replica.',
          '**Cache** — Redis, and it is a scale-out **prerequisite**, not an optimisation. The platform keeps a local memory cache per instance and uses Redis to invalidate the others; without it, two instances serve two different truths. Scale Redis up before out — a cluster changes the semantics of the operations the platform uses.',
          '**Database** — up first, then out sideways. One writer, so vertical is the honest first move, then read replicas where the provider allows, then a **database per module** via `ConnectionStrings:<ModuleId>` for a table set that has earned its own server. On SQL Server, `SqlServer:CompatibilityLevel` and `SqlServer:ParameterTranslationMode` change how EF Core 10 translates queries — cheaper than either kind of scaling.',
          '**Search** — out, as a cluster, and treat it as the read path it is. Catalog browse and filter do not touch the catalog database; if the index is slow, the storefront is slow no matter how large the database is.',
          '**Blob storage** — out by definition, once you stop using local disk. `FileSystemAssets` does not survive a second instance; that swap is a scale-out prerequisite in the same way Redis is.'
        ]
      },

      /* Composability, drawn as the mechanism rather than as a pile of boxes. The thing that
         makes a PBC split safe is that the frontend never learns about it: one GraphQL endpoint,
         one schema, and a router that resolves each operation to the host that owns it. Last of
         the five views on purpose — it only reads once the deployment shapes above it have. */
      {
        kind: 'topology',
        title: 'Composable Architecture',
        note: '**The frontend never learns about the split.** It keeps one endpoint and one GraphQL schema; the load balancer resolves each operation and forwards it to the PBC host that owns it, so `xCatalog` queries land on the catalog host and `xCart` mutations on the purchase host. Every host runs the **same image** with a different module set, and they meet again in the shared stores. The dashed box round each one is that boundary: **one deployable image**, scaled and restarted on its own, which is what isolation means here — a bad release of the catalog image cannot take checkout with it. Everything inside the outer boundary is the cloud environment; only the storefront sits outside it. The pool on the right is **the same one the production topology shares** — every image uses all of it, and one line each is drawn to keep the picture readable.\n\n**When to split by PBC** rather than run one instance, or the same modules split by role: when one capability\'s load curve is unlike the others — catalog reads spike with a campaign while checkout stays flat; when a bad reindex or a bulk import must not be able to reach checkout; when one capability needs its own release cadence, its own region, or a different machine shape; or when a team needs to deploy without coordinating with everyone else.\n\n**When not to.** Split by **role** first — it is the same image, same modules and same database, and it costs one configuration change. Reach for a PBC split only when the role split has stopped being enough, because this one is a second image to build, promote and keep in step. And check the graph closes: `XOrder` requires `XCart`, which requires `XCatalog`, so those three deploy together whatever the diagram wishes.',
        cols: 4,
        legend: [
          { kind: 'custom', label: 'Your code' },
          { kind: 'infra', label: 'Edge & routing' },
          { kind: 'virto', label: 'PBC host · same image, own module set' },
          { kind: 'data', label: 'Shared store' }
        ],
        regions: [
          /* Three levels, because three things are true at once: everything but the storefront
             runs in the cloud environment, each PBC host is its own image, and the stores are
             the one thing all of them share. */
          { id: 'cloud', label: 'Cloud environment', outer: true, col: [2, 4], row: [1, 5] },
          { id: 'img-catalog', tight: true, col: [3, 3], row: [1, 1] },
          { id: 'img-purchase', tight: true, col: [3, 3], row: [2, 2] },
          { id: 'img-order', tight: true, col: [3, 3], row: [3, 3] },
          { id: 'img-profile', tight: true, col: [3, 3], row: [4, 4] },
          { id: 'stores', label: 'Shared by every image', accent: 'shared', col: [4, 4], row: [1, 5] }
        ],
        nodes: [
          { id: 'frontend', name: 'Storefront', sub: 'One endpoint, one schema', meta: 'GraphQL', kind: 'custom', col: 1, row: 2 },
          { id: 'router', name: 'Load balancer', sub: 'Resolves the operation, forwards to its owner', meta: 'by path · by operation', kind: 'infra', col: 2, row: 2 },

          { id: 'catalog', name: 'Digital Catalog', sub: 'xCatalog · Catalog · Search', meta: '2…n', kind: 'virto', col: 3, row: 1 },
          { id: 'purchase', name: 'Purchase', sub: 'xCart · Cart · Pricing · Payment', meta: '2…n', kind: 'virto', col: 3, row: 2 },
          { id: 'order', name: 'Order', sub: 'xOrder · Orders — needs xCart', meta: '2…n', kind: 'virto', col: 3, row: 3 },
          { id: 'profile', name: 'Customer & Company', sub: 'Profile API · Customer', meta: '2…n', kind: 'virto', col: 3, row: 4 },

          /* The same pool the production topology shares — same services, same names. The row
             order differs so every edge below can run straight; the set is what matters. */
          { id: 'search', name: 'Elasticsearch', sub: 'A cluster, not one node', kind: 'data', col: 4, row: 1 },
          { id: 'redis', name: 'Redis', sub: 'Invalidation · backplane · locks', meta: 'mandatory', kind: 'data', col: 4, row: 2 },
          { id: 'sql', name: 'SQL DB elastic pool', sub: 'Main · Cart · Order · Catalog · Customer', kind: 'data', col: 4, row: 3 },
          { id: 'blob', name: 'Blob storage', sub: 'Product images · assets · documents', kind: 'data', col: 4, row: 4 },
          { id: 'insights', name: 'Application Insights', sub: 'One target for every image', kind: 'data', col: 4, row: 5 }
        ],
        edges: [
          { from: 'frontend', to: 'router', label: 'one schema' },
          { from: 'router', to: 'catalog', label: 'catalog queries' },
          { from: 'router', to: 'purchase', label: 'cart mutations' },
          { from: 'router', to: 'order', label: 'order queries', turnOffset: -13 },
          { from: 'router', to: 'profile', label: 'profile', turnOffset: -26 },
          /* One straight edge per row: each host owns the store on its own line, so the picture
             stays legible instead of turning into a mesh nobody reads. */
          { from: 'catalog', to: 'search', label: 'catalog reads' },
          { from: 'purchase', to: 'redis', label: 'cache · locks' },
          { from: 'order', to: 'sql', label: 'reads · writes' },
          { from: 'profile', to: 'blob', label: 'documents' }
        ]
      },
    ],
    matrixTitle: 'Composition shapes — pick the shallowest cut that solves your problem',
    matrix: [
      { name: 'All-in-one · cut nothing',
        desc: 'One host, every module, one database. **Use it until something measured says otherwise** — it is the only shape with no coordination cost, and most solutions never need to leave it. The cost: a bulk import or a reindex competes with checkout, and scaling means scaling all of it.' },
      { name: 'Split by role · cut at configuration',
        desc: 'Same image, same modules, same database — different settings per host: `BackgroundJobs:Mode`, `PushNotifications:ScalabilityMode`, ARR affinity. **The highest value per unit of effort**, and the shape the production topology below already draws. The cost: Redis becomes mandatory, and every host still carries every module.' },
      { name: 'Split by Cell · cut at the module set',
        desc: 'A second package manifest, a second image, a smaller module set — a catalog-read host is the clean example. **This is real decomposition**: independent scaling and a smaller blast radius per business capability. The cost: two images to promote, and the dependency graph has to close around the subset.' },
      { name: 'Split the database · cut at the module',
        desc: 'One configuration line — `ConnectionStrings:<ModuleId>` — moves a module to its own server, schema, backups and all. **Orthogonal to the other three**: you can do it in an all-in-one host. The cost: no transaction spans two module databases, and restore becomes per-module.' },
      { name: 'Not available · a module as a service',
        desc: 'A module is a unit of *code and schema*, not of process. Every module in a host loads into that host. **There is no supported way to run one module as its own service** — the seam is the module set, which is why the Cells tier exists.' }
    ],
    bullets: [
      'How far can you decompose? Rung by rung, on the ladder the Architectural Guidelines define. **Atom** — a primitive, in-process, not a boundary. **Molecule** (a module) — a schema boundary yes, a process boundary no. **Cell** (Digital Catalog, Order Management) — deployable: this is where you cut. **Organism** — the environment topology, drawn below.',
      'The two decomposition levers worth knowing by name: `optional="true"` in a manifest, which is how a module set gets small enough to deploy on its own, and `ConnectionStrings:<ModuleId>`, which is how a module gets its own database. Both are configuration; neither appears in the shipped `appsettings.json`, which is why teams reasonably assume they do not exist.',
      'Modules never hold a foreign key across the boundary — ids and copied values only. That single convention is what makes a per-module database work at all, and it is why an order keeps the price it charged instead of looking one up.',
      'The ratio surprises people: a mature solution is mostly configuration plus a handful of custom modules, sitting on dozens of vendor modules. If you are writing a lot of code, check whether a lower extensibility level would do.',
      'Your custom module has the same three-project shape as a vendor one — `Core` / `Data` / `Web` — and loads through the same manifest and dependency graph. There is no "application project" that is special.',
      'Extend in this order: no-code (dynamic properties, settings, permissions) → API (REST/GraphQL, webhooks, event handlers) → native (`AbstractTypeFactory` override). Each step up costs more at upgrade time.',
      '`vc-package.json` is the boundary between what you own and what you consume. It is the file that makes an environment reproducible, and the first thing to read when two environments behave differently.',
      'The storefront is a separate deliverable with its own build and deploy — a platform release does not update it, and it does not ship inside the container image.'
    ],
    gotchas: [
      'A module is not a service. Splitting by role gives you separate processes over the same modules and the same data; splitting by module set gives you a smaller host. Neither turns one module into an independently deployable unit.',
      'Every dependency a module declares as **required** has to be installed in the package with it — there is no partial install, and a missing one does not degrade the module, it takes the module and everything depending on it out of the host. So when you write a module, **split its dependencies into required and optional**: mark `optional="true"` on everything it can start without, and it becomes installable in several PBCs instead of only in the one that happens to carry the whole list. `VirtoCommerce.Orders`, with 11 required and none optional, is the counter-example.',
      'There is no distributed transaction. The moment two modules are on two databases, a write that spans them is two writes, and you own the reconciliation.',
      'Every host sharing a database runs migrations at startup, serialised by the platform distributed lock — which falls back to a no-op when Redis is not configured. Split hosts without Redis and two of them can migrate at once.',
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
      { label: 'Release strategy for business users (presentation)', href: 'https://virtocommerce.github.io/vc-release-notes/presentations/release-strategy-for-business-users.html' },
      { label: 'Release strategy', page: 'Updating-Virto-Commerce-Based-Project/release-strategy-overview' },
      { label: 'vc-testing-module (GitHub)', href: 'https://github.com/VirtoCommerce/vc-testing-module' },
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
      'XAPI lives in a separate repository (`vc-module-x-api`) — it is a module set, not part of the platform core. `vc-module-experience-api` is the archived predecessor; do not start from it.'
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
    /* The folder layout a developer actually gets, from the official scaffold:
       vc-cli-module-template/templates/vc-module-dba-template. Every line is answered, because
       "which folder does this go in" is the question a new module raises first. */
    diagrams: [
      {
        kind: 'tree',
        title: 'Solution module structure',
        note: 'What `dotnet new vc-module` gives you, from the **vc-cli-module-template** scaffold. `{Namespace}` becomes `VirtoCommerce.MyModule` or `Abc.MyModule`; the folder names are conventions the platform and the build both rely on, so keep them. A custom solution module has exactly this shape — there is no different, lesser layout for "just our code".',
        root: 'vc-module-{kebab-name}/',
        items: [
          { name: '{Namespace}.sln', depth: 0, kind: 'file', desc: 'The solution. Its only members are the projects below — no application project, because the platform is the host.' },
          { name: 'Directory.Build.props', depth: 0, kind: 'file', desc: 'Build settings shared by every project: target framework, `TreatWarningsAsErrors`, version. Change it once, not per project.' },
          { name: 'module.ignore', depth: 0, kind: 'file', desc: 'Paths `vc-build` leaves out of the module package.' },
          { name: 'docs/', depth: 0, desc: 'Module documentation, including the database model diagram the template ships a `.drawio` source for.' },

          { name: 'src/', depth: 0, desc: 'The three-project vertical slice, plus one project per database provider.' },

          { name: '{Namespace}.Core/', depth: 1, kind: 'project', desc: 'Contracts only — no EF Core, no ASP.NET. This is what other modules may depend on, so anything you put here you have to keep.' },
          { name: 'Models/', depth: 2, desc: 'Domain models: the types your services and API speak in.' },
          { name: 'Services/', depth: 2, desc: 'Service **interfaces**. The implementations live in `.Data`, which is what lets a solution swap one out.' },
          { name: 'Events/', depth: 2, desc: 'Domain and integration event classes — the contract other modules react to instead of calling you.' },
          { name: 'Notifications/', depth: 2, desc: 'Notification types this module raises, registered so their templates can be overridden.' },
          { name: 'ModuleConstants.cs', depth: 2, kind: 'file', desc: 'Permissions, settings and literals as constants — `{kebab}:read`, `SettingDescriptor`s, security scopes. The template ships the CRUD five.' },

          { name: '{Namespace}.Data/', depth: 1, kind: 'project', desc: 'Persistence and implementation. Provider-neutral: no SQL Server specifics here, which is what makes the module database-agnostic.' },
          { name: 'Models/', depth: 2, desc: 'EF Core entities. Deliberately **not** the domain models above — the mapping between them is where you control what the database sees.' },
          { name: 'Repositories/', depth: 2, desc: 'The module `DbContext` and its repositories. One context per module; the platform does not own your tables.' },
          { name: 'Services/', depth: 2, desc: 'Implementations of the `.Core` interfaces.' },
          { name: 'Handlers/', depth: 2, desc: 'Event handlers — your own events and other modules\' integration events.' },
          { name: 'Caching/', depth: 2, desc: 'Cache regions for this module, so an eviction can be scoped to it rather than global.' },
          { name: 'ExportImport/', depth: 2, desc: 'Backup and restore support, so this module\'s data travels with an environment.' },

          { name: '{Namespace}.Data.SqlServer/', depth: 1, kind: 'project', desc: 'Migrations for one provider, plus a `DesignTimeDbContextFactory` and an assembly marker. Nothing but migrations belongs here.' },
          { name: '{Namespace}.Data.PostgreSql/', depth: 1, kind: 'project', desc: 'The same for PostgreSQL. A module that skips a provider simply does not run on it — migrations are not generated at runtime.' },
          { name: '{Namespace}.Data.MySql/', depth: 1, kind: 'project', desc: 'The same for MySQL.' },

          { name: '{Namespace}.Web/', depth: 1, kind: 'project', desc: 'The application layer, and the only project that is never a NuGet package — this is the deployable module.' },
          { name: 'Module.cs', depth: 2, kind: 'file', desc: '`IModule`: `Initialize` registers services, `PostInitialize` registers permissions, settings and event handlers, `Uninstall` cleans up.' },
          { name: 'module.manifest', depth: 2, kind: 'file', desc: 'Identity, dependencies and `platformVersion`. No manifest, no module — there is no convention-based fallback.' },
          { name: 'Controllers/Api/', depth: 2, desc: 'REST controllers, one per aggregate, each endpoint carrying its own permission.' },
          { name: 'Scripts/', depth: 2, desc: 'The Admin UI: `module.js` registers the AngularJS module, `blades/` holds the screens, `resources/` the generated API client.' },
          { name: 'Localizations/', depth: 2, desc: '`en.{Namespace}.json` and its siblings. Loaded at runtime, so a missing key shows as the key itself.' },
          { name: 'Content/', depth: 2, desc: 'Static assets served by the module — the manifest icon lives here.' },
          { name: 'package.json · webpack.config.js', depth: 2, kind: 'file', desc: 'The Admin UI build. Output goes to `wwwroot/dist`, which is what the platform serves.' },

          { name: 'tests/', depth: 0, desc: 'One test project per module.' },
          { name: '{Namespace}.Tests/', depth: 1, kind: 'project', desc: 'xUnit, with Moq and FluentAssertions. Fake the repository and `IPlatformMemoryCache`; exercise the service for real.' }
        ]
      }
    ],
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
      { label: 'Extensibility overview', page: 'Extensibility/overview' },
      { label: 'vc-cli-module-template (the scaffold)', href: 'https://github.com/VirtoCommerce/vc-cli-module-template/tree/main/templates/vc-module-dba-template' }
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
    sub: 'How the platform joins an existing business ecosystem. Integration is not one problem — it is a different problem for buyers, for your own team and for your suppliers, and the platform opens a different door for each. Middleware does the translating rather than either side compromising its model.',
    tags: ['xAPI', 'REST', 'middleware', 'UCP/MCP', 'EventBus', 'WebHooks'],
    /* One schema per audience, in the order the Integration Capabilities deck presents them.
       Each is the same shape — caller, the door it knocks on, the platform behind it — because
       the point being made is that the shape repeats and only the door changes. */
    diagrams: [
      {
        kind: 'topology',
        title: 'Buyers — doors on the demand side',
        note: 'Three kinds of buyer, three doors, one set of rules behind them. Whichever door a request arrives through, the platform resolves **who the buyer is, which organization they belong to and what they have negotiated** before it answers — so the price is the same story everywhere.',
        cols: 3,
        legend: [
          { kind: 'custom', label: 'Your code' },
          { kind: 'infra', label: 'Someone else\'s system' },
          { kind: 'virto', label: 'Virto Commerce' }
        ],
        regions: [
          { id: 'doors', label: 'Doors for buyers', col: [2, 2], row: [1, 3] }
        ],
        nodes: [
          { id: 'store', name: 'Storefront · app', sub: 'Your front end, any channel', kind: 'custom', col: 1, row: 1 },
          { id: 'procure', name: 'Procurement system', sub: 'Coupa · Ariba · Jaggaer', kind: 'infra', col: 1, row: 2 },
          { id: 'agent', name: 'AI agent', sub: 'Shopping on the buyer\'s behalf', kind: 'infra', col: 1, row: 3 },

          { id: 'xapi', name: 'xAPI', sub: 'One request per screen', meta: 'GraphQL', kind: 'virto', col: 2, row: 1 },
          { id: 'punch', name: 'Punchout', sub: 'Answers in their language', meta: 'cXML', kind: 'virto', col: 2, row: 2 },
          { id: 'ucp', name: 'UCP', sub: 'Universal Commerce Protocol', meta: 'MCP', kind: 'virto', col: 2, row: 3 },

          { id: 'platform', name: 'Virto Commerce', sub: 'Catalog · pricing · cart · orders', kind: 'virto', col: 3, row: 2 }
        ],
        edges: [
          { from: 'store', to: 'xapi', label: 'GraphQL' },
          { from: 'procure', to: 'punch', label: 'punch out' },
          { from: 'agent', to: 'ucp', label: 'discover' },
          { from: 'xapi', to: 'platform', label: 'priced server-side' },
          { from: 'punch', to: 'platform', label: 'cart back' },
          { from: 'ucp', to: 'platform', label: 'cart · checkout' }
        ]
      },

      {
        kind: 'topology',
        title: 'eCommerce team — doors for the people who run it',
        note: 'Three doors, cheapest first. **Configuration** covers a large share of what looks like integration work and is live immediately. **REST** is the same API the admin screens use, so nothing is trapped in a screen. **Middleware** is where the enterprise-specific work belongs — and for a multi-system landscape it is the normal architecture, not the fallback.',
        cols: 4,
        legend: [
          { kind: 'custom', label: 'Yours to run' },
          { kind: 'infra', label: 'People' },
          { kind: 'virto', label: 'Virto Commerce' },
          { kind: 'data', label: 'Enterprise system' }
        ],
        regions: [
          { id: 'doors', label: 'Doors for your team', col: [2, 2], row: [1, 3] }
        ],
        nodes: [
          { id: 'biz', name: 'Business team', sub: 'Merchandisers, pricing, support', kind: 'infra', col: 1, row: 1 },
          { id: 'dev', name: 'Your developers', sub: 'Internal tools, custom logic', kind: 'infra', col: 1, row: 2 },
          { id: 'intteam', name: 'Integration team', sub: 'Often not the commerce team', kind: 'infra', col: 1, row: 3 },

          { id: 'admin', name: 'Admin UI', sub: 'Configuration, live immediately', kind: 'virto', col: 2, row: 1 },
          { id: 'rest', name: 'REST /api', sub: 'The API the screens use', meta: 'OpenAPI', kind: 'virto', col: 2, row: 2 },
          { id: 'mw', name: 'Integration middleware', sub: 'Functions · Logic Apps · Boomi', kind: 'custom', col: 2, row: 3 },

          { id: 'platform', name: 'Virto Commerce', sub: 'One set of business rules', kind: 'virto', col: 3, row: 2 },
          { id: 'systems', name: 'ERP · CRM · PIM · WMS', sub: 'Prices and stock in, orders out', kind: 'data', col: 4, row: 3 }
        ],
        edges: [
          { from: 'biz', to: 'admin', label: 'no ticket' },
          { from: 'dev', to: 'rest', label: 'generated client' },
          { from: 'intteam', to: 'mw', label: 'owns the flows' },
          { from: 'admin', to: 'platform', label: 'settings' },
          { from: 'rest', to: 'platform', label: 'read · write' },
          { from: 'mw', to: 'platform', label: 'imports the schema' },
          { from: 'mw', to: 'systems', label: 'map · retry' }
        ]
      },

      {
        kind: 'topology',
        title: 'Suppliers — doors for whoever is supplying you',
        note: 'The same capability at three levels of supplier maturity. Everything a supplier submits stays **scoped to their own account and subject to your moderation** — nothing reaches the storefront that you did not allow. Make the portal the default and graduate suppliers upward as their volume justifies it.',
        cols: 3,
        legend: [
          { kind: 'custom', label: 'Yours to run' },
          { kind: 'infra', label: 'The supplier' },
          { kind: 'virto', label: 'Virto Commerce' }
        ],
        regions: [
          { id: 'doors', label: 'Doors for suppliers', col: [2, 2], row: [1, 3] }
        ],
        nodes: [
          { id: 'bigsup', name: 'Supplier with IT', sub: 'Own systems, enough volume', kind: 'infra', col: 1, row: 1 },
          { id: 'tail', name: 'The long tail', sub: 'A spreadsheet, an SFTP drop, an API', kind: 'infra', col: 1, row: 2 },
          { id: 'nosup', name: 'Supplier with no IT', sub: 'Nothing to integrate at all', kind: 'infra', col: 1, row: 3 },

          { id: 'vapi', name: 'Vendor APIs', sub: 'Products, offers, stock, orders', kind: 'virto', col: 2, row: 1 },
          { id: 'vmw', name: 'Vendor middleware', sub: 'One flow, many formats', kind: 'custom', col: 2, row: 2 },
          { id: 'portal', name: 'Vendor Portal', sub: 'A login instead of an integration', kind: 'virto', col: 2, row: 3 },

          { id: 'platform', name: 'Virto Commerce', sub: 'Moderation · approval · publish', kind: 'virto', col: 3, row: 2 }
        ],
        edges: [
          { from: 'bigsup', to: 'vapi', label: 'system to system' },
          { from: 'tail', to: 'vmw', label: 'file · feed · API' },
          { from: 'nosup', to: 'portal', label: 'sign in' },
          /* The middleware does not have its own door: it calls the vendor APIs for the supplier. */
          { from: 'vmw', to: 'vapi', label: 'on their behalf' },
          { from: 'vapi', to: 'platform', label: 'account-scoped' },
          { from: 'portal', to: 'platform', label: 'moderated' }
        ]
      }
    ],
    matrixTitle: 'Which door — match it to whoever is knocking',
    matrix: [
      { name: 'Admin UI · configuration',
        desc: '**Check this door first, every time.** A large share of what looks like integration work is configuration: catalogs and content, price lists and contracts, promotions, organizations and roles, stores, currencies, languages — and the settings of the integrations themselves. If a business person can describe the change in business terms, it is usually a setting rather than a project.' },
      { name: 'xAPI · a person on a screen',
        desc: 'The experience API. A screen asks for exactly the data it needs in one request, and the platform resolves who the buyer is, which organization they belong to and what they have negotiated **before** it answers. Choose it for a storefront, a mobile app, a partner-branded portal or a rep selling on a customer\'s behalf — a new surface is then a new front end, not a new back end.' },
      { name: 'REST · a system you already own',
        desc: 'Every module publishes an open REST API with an OpenAPI schema — **the same API the admin screens use**, so nothing is trapped in a screen. Choose it for logic your business is genuinely built on, for high-volume data movement, or for an internal tool that needs complete control.' },
      { name: 'Integration middleware · the back office',
        desc: 'Azure Function Apps (the default in Virto delivery), Logic Apps, Boomi, MuleSoft, Kafka, or client-owned. **For multi-system landscapes this is the normal architecture, not the fallback**: orchestration, transformation, routing, retries and client-specific business logic belong here, especially when the integration team is not the commerce team. It imports the OpenAPI schema, so connecting SAP is a mapping — prices and stock in, orders out, invoices back.' },
      { name: 'Punchout · the buyer\'s procurement system',
        desc: 'Your largest customers buy from Coupa, Ariba or Jaggaer, because that is where budgets and approvals live. Punchout answers those systems in **cXML**: the buyer clicks your catalog from inside their tool, lands already identified with the right organization and contract prices, shops normally, and the finished cart goes back for approval and a PO. Choose it the moment a customer says they can only buy through their procurement platform.' },
      { name: 'UCP over MCP · an AI agent',
        desc: 'The `VirtoCommerce.UCP` module exposes the **Universal Commerce Protocol** through an MCP endpoint — an open standard for agent-driven commerce, so agents discover products, build a cart and check out against your real catalog, pricing and order rules. One endpoint, every compliant agent, no per-agent integration.' },
      { name: 'Vendor APIs · a supplier with its own IT',
        desc: 'A supplier\'s own systems create products and offers, update price and inventory, and receive and fulfil their orders — scoped strictly to their account and still subject to your moderation. Choose it for strategic suppliers with the volume to justify a system-to-system link.' },
      { name: 'Vendor middleware · the long tail',
        desc: 'Most suppliers cannot meet you at your API: they have a spreadsheet, an SFTP drop, or an API built for something else. One middleware flow collects whatever they can produce, validates and normalizes it, and calls the vendor APIs on their behalf. **The next supplier is a mapping, not a project.**' },
      { name: 'Vendor Portal · a supplier with no IT at all',
        desc: 'A login instead of an integration: register and get approved, create or bulk-import products and offers, keep price and stock current, see and fulfil orders. Make it the default for every new supplier and graduate the high-volume ones to middleware or API when the volume justifies it.' },
      { name: 'Events out · EventBus and WebHooks',
        desc: 'Not a door in, but the way the platform tells other systems something happened. EventBus bridges domain events onto Azure Service Bus, RabbitMQ or Kafka; WebHooks does configurable outbound HTTP per event type. Both are installable modules, not platform core.' }
    ],
    bullets: [
      'Integration is a different problem for each audience — buyers, your own team, your suppliers — and handing all three the same generic API is how budgets disappear. **Match the door to whoever is knocking**; the matrix above is that decision.',
      'The doors are not mutually exclusive. Most projects run several in parallel, and every door reads and writes the same data under the same rules, so nothing drifts apart.',
      'Integration middleware — a translation layer between the platform and ERP / WMS / CRM / PIM. Keeps foreign models out of your domain and lets each side change independently.',
      'Master vs reference data — decide per entity which system owns the truth. This single decision determines the direction of every sync you will build.',
      'The proof is volume, not a demo: **HEINEKEN runs 250 integrations across 25 operating companies** on one Virto Commerce foundation, and hundreds of enterprise-system integrations are delivered across the customer base — ERP, CRM, PIM, WMS, payments, tax, search and marketplaces (as of 2026-06-30).'
    ],
    gotchas: [
      'Integration events are at-least-once in practice. Handlers must be idempotent; assume every message can arrive twice.',
      'EventBus and WebHooks are installable modules, not platform core — the tiles on this poster reflect that.',
      'Point-to-point integration between the platform and each external system is the trap middleware exists to prevent.',
      'The most expensive integration is the one that never needed to exist — a ticket raised because a price list, a promotion or an approval limit could only be changed by a developer. Check the Admin UI door before scoping anything.',
      'Hand-written integration code has a running cost: every field the ERP team renames becomes a ticket, a release and a deployment window. That cost, not the build, is what middleware is bought to avoid.',
      '**Punchout is named in the Integration Capabilities deck but is not in the public module registry** and has no public repository. Treat it as a delivered capability to scope with the Virto team, not a package you can install from `vc-package.json`.'
    ],
    docs: [
      { label: 'Integration Capabilities (presentation)', href: 'https://virtocommerce.github.io/vc-release-notes/presentations/integration-capabilities.html' },
      { label: 'Architecture reference', page: 'Back-End-Architecture/02-conceptual-overview' },
      { label: 'Extending using events', page: 'Fundamentals/Event-Driven-Development/using-domain-events' },
      { label: 'vc-module-ucp (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-ucp' },
      { label: 'Universal Commerce Protocol (ucp.dev)', href: 'https://ucp.dev/' }
    ]
  },

  {
    id: 'infrastructure',
    name: 'Infrastructure',
    hue: 230,
    sub: 'What must exist for the platform to run, what each piece is actually for — and who operates it. Cloud-agnostic: Azure, AWS, GCP or on-premise, self-hosted or managed on Virto Cloud.',
    tags: ['SQL', 'Redis', 'Elasticsearch', 'Blob', 'Kubernetes', 'GitOps'],
    matrixTitle: 'Who runs it — self-hosted or Virto Cloud',
    matrix: [
      { name: 'Required skills',
        desc: '**Self-hosted:** Kubernetes, DevOps, DBA and security capability in-house — a platform team, permanently staffed. **Virto Cloud:** the platform skills come with it, and your team stays on the solution.' },
      { name: 'DevOps process',
        desc: '**Self-hosted:** you design and run CI/CD and GitOps yourself. **Virto Cloud:** starter CI/CD plus **GitOps with Argo**, ready to use — the environment is declared in Git and reconciled from it.' },
      { name: 'New environments',
        desc: '**Self-hosted:** built per project, in weeks. **Virto Cloud:** self-service, in minutes. This is the difference a team feels first.' },
      { name: 'Maintenance',
        desc: '**Self-hosted:** patching, version updates and database routines are yours. **Virto Cloud:** automated and run by Virto, including scheduled backups for every critical component.' },
      { name: 'Rollback strategy',
        desc: '**Self-hosted:** you design it, and — the part that gets skipped — you test it. **Virto Cloud:** blue-green deployment with instant rollback, staged.' },
      { name: 'Performance on the same resources',
        desc: '**Self-hosted:** generic infrastructure. **Virto Cloud:** tuned for this stack, so the same solution runs faster on identical resources. Worth asking to see measured, on your workload.' },
      { name: 'Multi-region and residency',
        desc: '**Self-hosted:** you build each region. **Virto Cloud:** nearest datacenters including **China** — the deck cites a dedicated China deployment with 20k+ SKUs and 50+ modules. Data residency and sovereignty stay your decision either way.' },
      { name: 'Monitoring and telemetry',
        desc: '**Self-hosted:** you set it up and you watch it. **Virto Cloud:** proactive alerts and one dashboard across every solution — and metrics, logs and traces stay open to your team rather than locked away.' },
      { name: 'Support, uptime and compliance',
        desc: '**Self-hosted:** your team, and you certify it. **Virto Cloud:** 24×7 for Priority-1 incidents, follow-the-sun business hours across AMER · EMEA · APAC, a **99.9% availability SLA**, and SOC 2 / GDPR.' }
    ],
    bullets: [
      'Relational database — SQL Server, PostgreSQL or MySQL. One provider per deployment, chosen by connection string and provider package; migrations exist per provider.',
      'Redis — three distinct jobs, worth separating in your head: cache-invalidation bus, distributed lock (RedLock), and SignalR backplane. It is not used as a shared cache store.',
      'Search engine — Elasticsearch (or Lucene / Azure Search via the matching provider module). The catalog read path depends on it; it is not optional at scale.',
      'Blob storage — product images, imports, exports and other assets, behind a file-system or cloud provider.',
      'CDN — static assets and product images, in front of blob storage.',
      '**Kubernetes-native, and portable.** On Virto Cloud the runtime is a Kubernetes-native core following CNCF practice — AKS, SQL, Redis and Elasticsearch as managed services, in a dedicated isolated environment. Azure today; AWS and GCP on request. Portable, not simultaneous.',
      '**Composable scaling is an infrastructure property, not just a code one.** Catalog, pricing and search scale independently, horizontally and automatically; vertical sizing stays manual and on demand. That is the same seam the Composable Architecture view draws — see [[host-composition]] and [[scalability]].',
      '**The environment is a Git artifact.** Declarative infrastructure reconciled by Argo means an environment is reviewable and reproducible, and a rollback is a revert rather than a rebuild. If you self-host, this is the practice worth copying first.'
    ],
    gotchas: [
      'Scaling out to more than one instance makes Redis mandatory: without it, per-instance memory caches drift and module installation has no distributed lock.',
      'Redis holding cache *invalidation messages* rather than cache *values* surprises almost everyone. Losing Redis costs coherence, not the cache itself.',
      'An SLA covers the layer the provider operates. **99.9% on the platform layer is not 99.9% on your solution** — a custom module that throws, or a migration that locks a table, is inside your half of the boundary. Know where the line is before you quote a number to a customer.',
      'Managed does not mean unobservable, and it does not mean unwatched either: telemetry being open to your team is only worth something if someone on it is looking at the dashboard.',
      '"Azure today, AWS and GCP on request" means the stack is portable, not that it runs on all three at once. Treat a second cloud as a project with a date, not a switch.',
      'Everything in the comparison above comes from the Virto Cloud presentation — vendor material, and correct as far as it goes. The numbers worth asking to see measured on your own workload are the performance claim and the achieved uptime, not the target.'
    ],
    docs: [
      { label: 'Scalability', page: 'Fundamentals/Scalability/scalability-options' },
      { label: 'Scale out on Azure', page: 'Fundamentals/Scalability/scaling-configuration-on-azure-cloud' },
      { label: 'Health checks', page: 'Tutorials-and-How-tos/How-tos/health-checks' },
      { label: 'Search', page: 'Fundamentals/Indexed-Search/overview' },
      { label: 'Virto Cloud (presentation)', href: 'https://virtocommerce.github.io/vc-release-notes/presentations/virto-cloud.html' }
    ]
  }
];
