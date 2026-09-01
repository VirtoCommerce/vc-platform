/* Atom families — array order is left-to-right order on the poster.
   `hue` is the only colour input: styles.css derives tile, label and swatch colours from it. */
window.VC_MAP_FAMILIES = [
  { id: 'execution',  hue: 35,  name: 'Execution & async' },
  { id: 'caching',    hue: 190, name: 'Caching' },
  { id: 'config',     hue: 275, name: 'Config & metadata' },
  { id: 'messaging',  hue: 335, name: 'Messaging & events' },
  { id: 'data',       hue: 150, name: 'Data & domain' },
  { id: 'modularity', hue: 225, name: 'Modularity' },
  { id: 'security',   hue: 0,   name: 'Security' },
  { id: 'ops',        hue: 95,  name: 'Infra & ops' }
];

/* One object per atom. See README.md for the field contract.
   Every snippet is taken from, or written directly against, the source file named in `api`. */
window.VC_MAP_ATOMS = [

  // ================================================================ EXECUTION & ASYNC

  {
    id: 'background-jobs',
    symbol: 'Jb',
    name: 'Background jobs',
    family: 'execution',
    adoption: 'in-flight',
    layer: 'platform',
    tags: ['hangfire', 'queue', 'enqueue', 'worker', 'async', 'durable'],
    oneLiner: 'Durable queued work that outlives the request, behind an engine-agnostic facade.',
    pattern: 'Port and adapter, with work crossing the boundary as a serialized message rather than a delegate. The platform owns the port (`IBackgroundJob`); an installed engine module adapts it to Hangfire, RabbitMQ or another transport. You implement `IBackgroundJobHandler<TPayload>` and enqueue a payload.',
    whenToUse: [
      'Work that must survive an app restart or a deploy',
      'Anything slower than a request should be: imports, indexing, bulk updates, report generation',
      'Work needing retries, or at-least-once delivery guarantees',
      'Fan-out where you want the engine to handle concurrency and queueing'
    ],
    avoid: [
      'Anything needing `HttpContext` — the job runs outside the request scope',
      'Sub-second latency work; enqueue plus dispatch is not free',
      'Passing large data in the payload — store the blob and pass a reference (claim-check)',
      'Assuming exactly-once. Make the handler idempotent instead'
    ],
    api: [
      { name: 'IBackgroundJob', file: 'src/VirtoCommerce.Platform.Core/Jobs/IBackgroundJob.cs' },
      { name: 'IBackgroundJobHandler<TPayload>', file: 'src/VirtoCommerce.Platform.Core/Jobs/IBackgroundJobHandler.cs' },
      { name: 'EnqueueOptions', file: 'src/VirtoCommerce.Platform.Core/Jobs/EnqueueOptions.cs' },
      { name: 'IJobExecutionContext', file: 'src/VirtoCommerce.Platform.Core/Jobs/IJobExecutionContext.cs' },
      { name: 'AddBackgroundJob<THandler, TPayload>()', file: 'src/VirtoCommerce.Platform.Core/Jobs/BackgroundJobsServiceCollectionExtensions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Module.Core — the payload. Create it via AbstractTypeFactory so partners can override it.\n' +
'public class ReindexPayload\n' +
'{\n' +
'    public string StoreId { get; set; }\n' +
'}\n' +
'\n' +
'// Module.Data — the handler. Resolved from DI; last registration wins, so it stays overridable.\n' +
'public class ReindexHandler(IIndexingService indexing) : IBackgroundJobHandler<ReindexPayload>\n' +
'{\n' +
'    public async Task Execute(ReindexPayload payload, IJobExecutionContext context,\n' +
'        CancellationToken cancellationToken = default)\n' +
'    {\n' +
'        await context.Progress.Report(\n' +
'            new JobProgressInfo { Message = $"Reindexing {payload.StoreId}" }, cancellationToken);\n' +
'\n' +
'        await indexing.IndexAsync(payload.StoreId, cancellationToken);\n' +
'    }\n' +
'}\n' +
'\n' +
'// Module.Web — Module.Initialize. Note the order: handler first, payload second.\n' +
'services.AddBackgroundJob<ReindexHandler, ReindexPayload>();\n' +
'\n' +
'// Anywhere — enqueue. Naming the handler makes the call self-documenting.\n' +
'await _backgroundJob.Enqueue<ReindexHandler>(\n' +
'    new ReindexPayload { StoreId = storeId },\n' +
'    new EnqueueOptions\n' +
'    {\n' +
'        Title = "Reindex catalog",\n' +
'        ReportProgress = true,\n' +
'        UniqueKey = $"reindex:{storeId}",   // dedup: one in flight per store\n' +
'        MaxRetryAttempts = 3\n' +
'    });'
    },
    note: 'Hangfire is no longer a platform dependency. `VirtoCommerce.Platform.Hangfire` has been removed from `VirtoCommerce.Platform.sln`, and the platform now depends only on the contracts in `Platform.Core/Jobs`. Execution comes from the installable **`VirtoCommerce.BackgroundJobs`** module, which picks one engine — Hangfire (default), RabbitMQ, or in-memory for development — from `VirtoCommerce:BackgroundJobs:Provider`. Your code never names the engine.',
    gotchas: [
      'With no engine module installed, `IBackgroundJob` throws `BackgroundJobEngineNotInstalledException` — with installation instructions in the message. The platform still boots.',
      'The payload is validated against the handler at enqueue time: if `THandler` does not implement `IBackgroundJobHandler<>` for the payload\'s runtime type, `Enqueue` throws immediately rather than failing later on a worker.',
      'There is a shorter registration overload, `AddBackgroundJob<THandler>()`, which infers the payload type from the handler\'s interface. It requires the handler to implement exactly one `IBackgroundJobHandler<>`; name both types when it implements several.',
      '`UniqueKey` is how you stop a user double-clicking their way into two concurrent imports.',
      'Handlers get a fresh DI scope per job — not the enqueuing request\'s scope.'
    ],
    docs: [
      { label: 'vc-module-background-jobs (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-background-jobs' },
      { label: 'Background Processing Hub — design spec', href: 'https://github.com/VirtoCommerce/vc-platform/blob/dev/docs/superpowers/specs/2026-06-06-background-processing-hub-design.md' },
      { label: 'Scalability', page: 'Fundamentals/Scalability/scalability-options' }
    ],
    seeAlso: ['recurring-jobs', 'map-reduce-jobs', 'job-progress', 'fire-and-forget', 'hangfire', 'push-notifications', 'distributed-lock'],
    molecule: 'background-processing-hub',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'recurring-jobs',
    symbol: 'Rj',
    name: 'Recurring jobs',
    family: 'execution',
    adoption: 'in-flight',
    layer: 'platform',
    tags: ['cron', 'schedule', 'timer', 'nightly', 'watchjobsetting'],
    oneLiner: 'Cron-scheduled work whose schedule an operator can change without a deploy.',
    pattern: 'Declarative schedule registered at startup, resolved by the engine\'s scheduler. Either a fixed cron or — the Virto-specific move — a **setting-driven** schedule: an on/off setting plus a cron setting, re-evaluated automatically whenever either setting changes.',
    whenToUse: [
      'Nightly or periodic maintenance: pruning, digests, reconciliation, reindexing',
      'Any schedule an operator should be able to tune or switch off from the Admin UI',
      'Work that should be skipped entirely in some environments, via a deployment flag'
    ],
    avoid: [
      'Sub-minute intervals — that is a queue consumer or a hosted service, not a cron job',
      'Long chains where each step depends on the last; enqueue explicit follow-on jobs instead',
      'Assuming only one instance fires it — take a distributed lock if the work must be exclusive'
    ],
    api: [
      { name: 'IRecurringJobService', file: 'src/VirtoCommerce.Platform.Core/Jobs/IRecurringJobService.cs' },
      { name: 'IRecurringJobScheduleBuilder', file: 'src/VirtoCommerce.Platform.Core/Jobs/IRecurringJobScheduleBuilder.cs' },
      { name: 'AddRecurringJob<THandler, TPayload>()', file: 'src/VirtoCommerce.Platform.Core/Jobs/BackgroundJobsServiceCollectionExtensions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Setting-driven: the operator owns the schedule, and changing the setting re-registers the job.\n' +
'services.AddRecurringJob<SendDigestJob, SendDigestPayload>(\n' +
'    new SendDigestPayload { Top = 10 },\n' +
'    schedule => schedule\n' +
'        .WithId("MyModule.SendDigest")            // stable id — required\n' +
'        .FromSettings(\n' +
'            ModuleConstants.Settings.DigestEnabled,\n' +
'            ModuleConstants.Settings.DigestCronExpression));\n' +
'\n' +
'// Fixed cron, switched off by a deployment flag. WithEnabled(false) also REMOVES an\n' +
'// already-scheduled job from engine storage, rather than silently leaving it behind.\n' +
'services.AddRecurringJob<PruneTokensJob>(\n' +
'    schedule => schedule\n' +
'        .WithId("MyModule.PruneTokens")\n' +
'        .WithCron("0 3 * * *")\n' +
'        .WithTimeZone(TimeZoneInfo.Utc)\n' +
'        .WithEnabled(_options.PruneEnabled));'
    },
    note: 'Same extraction as background jobs: `Platform.Core/Jobs/IRecurringJobService` is the new engine-free contract, and scheduling is executed by the `VirtoCommerce.BackgroundJobs` module. The older `VirtoCommerce.Platform.Hangfire.IRecurringJobService` is a *distinct type* kept for binary compatibility — check which one you are injecting, because both compile.',
    gotchas: [
      'With no engine installed, recurring registration is a logged no-op rather than an exception: the platform boots and the job simply never runs. Silence here is the failure mode to watch for.',
      '`WithCron` and `FromSettings` are mutually exclusive — supply exactly one.',
      'The payload instance passed to the non-factory overload is serialized identically on every occurrence. Use the `Func<TPayload>` overload when a run needs a fresh value such as a timestamp.',
      'Cron is evaluated in UTC unless you pass `WithTimeZone` — a nightly job can fire at the wrong local hour after a DST change.'
    ],
    docs: [
      { label: 'vc-module-background-jobs (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-background-jobs' },
      { label: 'Background Processing Hub — design spec', href: 'https://github.com/VirtoCommerce/vc-platform/blob/dev/docs/superpowers/specs/2026-06-06-background-processing-hub-design.md' }
    ],
    seeAlso: ['background-jobs', 'hangfire', 'settings', 'distributed-lock', 'hosted-service'],
    molecule: 'background-processing-hub',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'map-reduce-jobs',
    symbol: 'MR',
    name: 'Map / reduce jobs',
    family: 'execution',
    adoption: 'in-flight',
    layer: 'platform',
    tags: ['fanout', 'batch', 'parallel', 'aggregate', 'bulk'],
    oneLiner: 'Fan a large batch out across workers, then aggregate the results in one place.',
    pattern: 'Map/reduce over the job engine. `IMapReduceJob.Enqueue<TMap, TReduce>` splits a collection into per-item map jobs, then runs a single reduce handler over the collected `MapResult`s once every map finishes.',
    whenToUse: [
      'Bulk operations over thousands of entities where each item is independent',
      'Work where you need a summary at the end: counts, failures, a report',
      'Any place you were about to write your own "enqueue N jobs and poll for completion" loop'
    ],
    avoid: [
      'Small batches — the coordination overhead outweighs the parallelism',
      'Items that depend on each other, or that must be processed in order',
      'Map handlers with side effects that are not idempotent; individual maps can be retried'
    ],
    api: [
      { name: 'IMapReduceJob', file: 'src/VirtoCommerce.Platform.Core/Jobs/IMapReduceJob.cs' },
      { name: 'IMapJobHandler<TItem, TState, TResult>', file: 'src/VirtoCommerce.Platform.Core/Jobs/IMapJobHandler.cs' },
      { name: 'IReduceJobHandler', file: 'src/VirtoCommerce.Platform.Core/Jobs/IReduceJobHandler.cs' },
      { name: 'MapReduceOptions', file: 'src/VirtoCommerce.Platform.Core/Jobs/MapReduceOptions.cs' },
      { name: 'AddMapReduce()', file: 'src/VirtoCommerce.Platform.Core/Jobs/MapReduceServiceCollectionExtensions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// items are mapped in parallel by TMap; TReduce runs once over all MapResults.\n' +
'var jobId = await _mapReduceJob.Enqueue<PriceRecalcMap, PriceRecalcReduce>(\n' +
'    items: productIds,\n' +
'    state: new PriceRecalcState { PricelistId = pricelistId },\n' +
'    options: new MapReduceOptions { /* batching, queue, retries */ });'
    },
    note: 'Part of the same background-processing extraction as the other job atoms. Read the Hub spec before designing around it — the contract is still settling.',
    gotchas: [
      'The reduce step only runs once every map job has reached a terminal state, so one stuck map job stalls the aggregate.',
      '`state` is serialized once and handed to every map job — it is shared read-only input, not a place to accumulate results.',
      'Requires an installed job engine, exactly like plain background jobs.'
    ],
    docs: [
      { label: 'Background Processing Hub — design spec', href: 'https://github.com/VirtoCommerce/vc-platform/blob/dev/docs/superpowers/specs/2026-06-06-background-processing-hub-design.md' }
    ],
    seeAlso: ['background-jobs', 'hangfire', 'job-progress', 'backup-restore'],
    molecule: 'background-processing-hub',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'job-progress',
    symbol: 'Jp',
    name: 'Job progress',
    family: 'execution',
    adoption: 'in-flight',
    layer: 'platform',
    tags: ['progress', 'reporting', 'percent', 'status', 'ui'],
    oneLiner: 'Reporting how far a long job has got, so the UI can show it instead of spinning.',
    pattern: 'Ambient reporter handed to the running handler. `IJobExecutionContext.Progress` accepts a `JobProgressInfo` (message plus processed and total counts) and is a **no-op** when the job was enqueued without `ReportProgress`, so handlers never branch on whether anyone is watching.',
    whenToUse: [
      'Imports, exports, reindexing — anything where the operator needs to see movement',
      'Jobs where a stalled step should be visibly distinguishable from a slow one',
      'Any job a user can trigger from the Admin UI'
    ],
    avoid: [
      'Reporting per item in a tight loop — batch it, or you make progress reporting the bottleneck',
      'Using progress messages as a log. They are a UI surface, not a diagnostic record'
    ],
    api: [
      { name: 'IJobProgress', file: 'src/VirtoCommerce.Platform.Core/Jobs/IJobProgress.cs' },
      { name: 'JobProgressInfo', file: 'src/VirtoCommerce.Platform.Core/Jobs/JobProgressInfo.cs' },
      { name: 'IJobExecutionContext.Progress', file: 'src/VirtoCommerce.Platform.Core/Jobs/IJobExecutionContext.cs' },
      { name: 'EnqueueOptions.ReportProgress', file: 'src/VirtoCommerce.Platform.Core/Jobs/EnqueueOptions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'public async Task Execute(ImportPayload payload, IJobExecutionContext context,\n' +
'    CancellationToken cancellationToken = default)\n' +
'{\n' +
'    var total = rows.Count;\n' +
'    for (var i = 0; i < total; i++)\n' +
'    {\n' +
'        cancellationToken.ThrowIfCancellationRequested();\n' +
'        await ImportRow(rows[i]);\n' +
'\n' +
'        if (i % 100 == 0)   // batch the reporting, not every row\n' +
'        {\n' +
'            await context.Progress.Report(new JobProgressInfo\n' +
'            {\n' +
'                Message = "Importing rows",\n' +
'                ProcessedCount = i,\n' +
'                TotalCount = total\n' +
'            }, cancellationToken);\n' +
'        }\n' +
'    }\n' +
'}'
    },
    gotchas: [
      'Progress is a no-op unless the job was enqueued with `ReportProgress = true` — a handler that "reports nothing" is usually an enqueue-side omission.',
      '`ProcessedCount` and `TotalCount` are nullable: send a message-only update when you genuinely do not know the total yet, rather than guessing.',
      'Progress reaches the browser over SignalR push notifications, so scaled-out deployments need the backplane configured or updates land on the wrong instance.'
    ],
    docs: [
      { label: 'Background Processing Hub — design spec', href: 'https://github.com/VirtoCommerce/vc-platform/blob/dev/docs/superpowers/specs/2026-06-06-background-processing-hub-design.md' }
    ],
    seeAlso: ['background-jobs', 'hangfire', 'push-notifications', 'backup-restore'],
    molecule: 'background-processing-hub',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'fire-and-forget',
    symbol: 'FF',
    name: 'Fire & forget',
    family: 'execution',
    adoption: 'platform',
    layer: 'platform',
    tags: ['task.run', 'async', 'unawaited', 'non-blocking', 'enqueue'],
    oneLiner: 'Start work and move on without waiting for the result — durably, so it survives the request that started it.',
    pattern: 'Enqueue-and-continue. You await the *enqueue*, not the work: `IBackgroundJob.Enqueue` hands the payload to the engine and returns a job id immediately, so the caller is not blocked by however long the work takes. The durability is what separates this from simply not awaiting a `Task`.',
    whenToUse: [
      'Work the caller genuinely should not wait for: sending a confirmation email, warming a cache, notifying a downstream system',
      'Keeping a request fast when the slow part does not affect the response',
      'Reacting to an event where the handler must not extend the publisher\'s transaction'
    ],
    avoid: [
      '`Task.Run` or an un-awaited `Task` for real work — the request ends, the DI scope is disposed, and unobserved exceptions vanish silently',
      'Anything whose result the caller needs. Fire-and-forget means you have given up the answer, not just the wait',
      'Assuming ordering between two things you fired. There is none',
      'Firing something and never surfacing its failure — give it a notification, a log, or a status the operator can see'
    ],
    api: [
      { name: 'IBackgroundJob.Enqueue', file: 'src/VirtoCommerce.Platform.Core/Jobs/IBackgroundJob.cs' },
      { name: 'BackgroundJob (static facade)', file: 'src/VirtoCommerce.Platform.Core/Jobs/BackgroundJob.cs' },
      { name: 'EnqueueOptions', file: 'src/VirtoCommerce.Platform.Core/Jobs/EnqueueOptions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// ✓ Durable fire-and-forget: await the enqueue, not the work.\n' +
'public class OrderService(IBackgroundJob backgroundJob)\n' +
'{\n' +
'    public async Task Place(CustomerOrder order)\n' +
'    {\n' +
'        await Save(order);\n' +
'\n' +
'        // Returns as soon as the job is queued. The email send happens on a worker,\n' +
'        // survives a restart, and retries on failure.\n' +
'        await backgroundJob.Enqueue<SendOrderEmailJob>(\n' +
'            new SendOrderEmailPayload { OrderId = order.Id });\n' +
'    }\n' +
'}\n' +
'\n' +
'// ✕ Not fire-and-forget so much as fire-and-lose: the scope dies with the request,\n' +
'//   nothing retries, and an exception here is never observed by anyone.\n' +
'_ = Task.Run(() => _service.SendEmail(order.Id));\n' +
'\n' +
'// ~ Static facade — works, but ambient and awkward to test. It is a migration aid\n' +
'//   for code moving off Hangfire\'s static API; prefer injecting IBackgroundJob.\n' +
'await BackgroundJob.Enqueue<SendOrderEmailJob>(payload);'
    },
    gotchas: [
      '`Task.Run` inside a request competes for the same thread pool that serves requests, so it degrades latency precisely when you are busiest.',
      'The static `BackgroundJob` facade is not deprecated, but it is documented in its own source as a migration aid rather than an API to build on. It only works once an engine module has called `BackgroundJob.Initialize`, and it opens a fresh DI scope per call rather than inheriting the caller\'s.',
      'The enqueuing user flows into the job\'s scope via `IHttpContextAccessor`, so audit fields survive the hand-off — one of the few pieces of ambient context that does.',
      'Fire-and-forget hides failures by construction. Pair it with a push notification or a visible job status, or nobody learns the email never went.'
    ],
    docs: [],
    seeAlso: ['background-jobs', 'hangfire', 'hosted-service', 'push-notifications'],
    molecule: 'background-processing-hub',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'hangfire',
    symbol: 'Hf',
    name: 'Hangfire',
    family: 'execution',
    adoption: 'legacy',
    layer: 'platform',
    tags: ['hangfire', 'dashboard', 'recurringjob', 'engine', 'provider', 'rabbitmq'],
    oneLiner: 'Referencing Hangfire directly is the legacy path. It is now one selectable engine behind the background-jobs port, not the platform\'s job system.',
    pattern: 'Provider behind a port, chosen by configuration — the same shape as search providers. `VirtoCommerce.BackgroundJobs` owns the engine contract; Hangfire is its default reference engine, with RabbitMQ and an in-memory engine as alternatives. Application code talks to `IBackgroundJob` and never names the engine.',
    whenToUse: [
      'Reading or maintaining existing code that calls `Hangfire.BackgroundJob.Enqueue` or the old `VirtoCommerce.Platform.Hangfire.IRecurringJobService`',
      'Choosing Hangfire deliberately as your engine — it is still the default and a perfectly good one',
      'Reaching the Hangfire dashboard to inspect jobs in a running environment'
    ],
    avoid: [
      'Referencing `Hangfire.*` types from new module code — that is the coupling the extraction removed',
      'Assuming the platform provides job execution on its own. Without an engine module installed, `IBackgroundJob` throws',
      'Depending on the dead `VirtoCommerce.Platform.Hangfire` package rather than `VirtoCommerce.BackgroundJobs.Hangfire`',
      'Treating `InMemory` as a production engine — it is documented for development and testing only'
    ],
    api: [
      { name: 'VirtoCommerce.BackgroundJobs (module)', file: '(separate repository — vc-module-background-jobs)' },
      { name: 'VirtoCommerce.BackgroundJobs.Hangfire (engine)', file: '(engine package, replaces VirtoCommerce.Platform.Hangfire)' },
      { name: 'IBackgroundJob (the port to use instead)', file: 'src/VirtoCommerce.Platform.Core/Jobs/IBackgroundJob.cs' },
      { name: 'IRecurringJobService (engine-free contract)', file: 'src/VirtoCommerce.Platform.Core/Jobs/IRecurringJobService.cs' },
      { name: 'BackgroundJobEngineNotInstalledException', file: 'src/VirtoCommerce.Platform.Core/Jobs/BackgroundJobEngineNotInstalledException.cs' }
    ],
    snippet: {
      lang: 'json',
      code:
'// appsettings.json — the engine is a configuration choice, not a code dependency.\n' +
'{\n' +
'  "VirtoCommerce": {\n' +
'    "BackgroundJobs": {\n' +
'      "Provider": "Hangfire",        // Hangfire (default) | RabbitMQ | InMemory\n' +
'      "Mode": "Both",                // Producer | Worker | Both\n' +
'      "EnableLegacyHangfire": true,  // keeps Hangfire running alongside another engine\n' +
'      "DefaultQueue": "default",\n' +
'      "MaxRetryAttempts": 3\n' +
'    },\n' +
'    "Hangfire": { "WorkerCount": 10, "JobStorageType": "SqlServer" },\n' +
'    "RabbitMQ": { "HostName": "localhost", "Port": 5672, "PrefetchCount": 10 }\n' +
'  }\n' +
'}'
    },
    useInstead: 'Install `VirtoCommerce.BackgroundJobs`, then define a payload plus an `IBackgroundJobHandler<TPayload>` and enqueue through `IBackgroundJob` — see the Background jobs atom. Migration is two steps: swap the dead `VirtoCommerce.Platform.Hangfire` package reference for `VirtoCommerce.BackgroundJobs.Hangfire`, then move off `Hangfire.BackgroundJob.Enqueue()` to `IBackgroundJob.Enqueue<THandler>()`.',
    note: 'The badge is about **direct Hangfire coupling**, not about Hangfire itself — Hangfire remains the default engine and is fine to run. What is legacy is code that names `Hangfire.*` types, and the `VirtoCommerce.Platform.Hangfire` package, which is now a type-forwarding shim kept only so existing assemblies still load.',
    gotchas: [
      '`EnableLegacyHangfire` defaults to **true**, so Hangfire may still be running even when you have selected another provider. That is deliberate backward compatibility, and it surprises people debugging which engine actually ran a job.',
      '`VirtoCommerce.Platform.Hangfire.dll` still exists as a type-forwarding shim. A reference to it compiles and appears to work, which is exactly why stale references survive unnoticed.',
      '`Mode` splits producer from worker: an instance set to `Producer` enqueues but never executes. A queue that fills up while nothing drains it is usually this.',
      'Engine choice changes delivery semantics — RabbitMQ is push-based and scale-to-zero friendly, Hangfire polls its storage. Handlers must be idempotent either way.'
    ],
    docs: [
      { label: 'vc-module-background-jobs (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-background-jobs' },
      { label: 'Background Processing Hub — design spec', href: 'https://github.com/VirtoCommerce/vc-platform/blob/dev/docs/superpowers/specs/2026-06-06-background-processing-hub-design.md' }
    ],
    seeAlso: ['background-jobs', 'recurring-jobs', 'fire-and-forget', 'map-reduce-jobs'],
    molecule: 'background-processing-hub',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'hosted-service',
    symbol: 'Hs',
    name: 'Hosted service',
    family: 'execution',
    adoption: 'platform',
    layer: 'platform',
    tags: ['backgroundservice', 'ihostedservice', 'long-running', 'startup', 'dotnet'],
    oneLiner: 'A .NET long-running loop tied to application lifetime — for in-process work no queue should own.',
    pattern: 'Hosted service. Derive from `BackgroundService`, override `ExecuteAsync`, and let the host start and stop it with the application. The platform uses exactly this for periodic in-memory synchronisation work.',
    whenToUse: [
      'Continuous in-process work: polling a channel, syncing in-memory state, keeping a connection warm',
      'Startup work that must run once per instance rather than once per cluster',
      'Intervals too short to be a cron job'
    ],
    avoid: [
      'Work that must not be lost — a hosted service dies with its instance and has no retry',
      'Work that should run once cluster-wide; every instance runs its own copy',
      'Anything an operator needs to trigger, monitor or cancel — that is a background job'
    ],
    api: [
      { name: 'BackgroundService', file: 'src/VirtoCommerce.Platform.Web/PushNotifications/Scalability/PushNotificationSynchronizerTask.cs' },
      { name: 'services.AddHostedService<T>()', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// The platform\'s own example: src/VirtoCommerce.Platform.Web/PushNotifications/\n' +
'//   Scalability/PushNotificationSynchronizerTask.cs\n' +
'public class MySynchronizerTask(IMyService service) : BackgroundService\n' +
'{\n' +
'    protected override async Task ExecuteAsync(CancellationToken stoppingToken)\n' +
'    {\n' +
'        while (!stoppingToken.IsCancellationRequested)\n' +
'        {\n' +
'            try\n' +
'            {\n' +
'                await service.Synchronize(stoppingToken);\n' +
'            }\n' +
'            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)\n' +
'            {\n' +
'                // Never let one iteration kill the loop for the rest of the process lifetime.\n' +
'                _logger.LogError(ex, "Synchronisation iteration failed");\n' +
'            }\n' +
'\n' +
'            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);\n' +
'        }\n' +
'    }\n' +
'}'
    },
    gotchas: [
      'An unhandled exception in `ExecuteAsync` stops the service silently for the rest of the process lifetime. Wrap the body of the loop, not just the outside.',
      '`BackgroundService` is a singleton — resolve scoped services by creating a scope per iteration, never by injecting them into the constructor.',
      'Shutdown gives you a limited grace period. Honour `stoppingToken` or deploys become forced kills.',
      'Every instance runs it, so N instances mean N copies. Add a distributed lock if that is not acceptable.'
    ],
    docs: [],
    seeAlso: ['background-jobs', 'cancellation', 'distributed-lock', 'push-notifications'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'cancellation',
    symbol: 'Ct',
    name: 'Cancellation',
    family: 'execution',
    adoption: 'platform',
    layer: 'platform',
    tags: ['cancellationtoken', 'timeout', 'abort', 'icancellationtoken', 'obsolete'],
    oneLiner: 'Threading a cancellation signal all the way down — and the obsolete shim you will still meet in older code.',
    pattern: 'Cooperative cancellation. A `CancellationToken` is passed down the call chain and checked at safe points; nothing is aborted forcibly. The platform threads it through jobs, event handlers, repositories and export/import.',
    whenToUse: [
      'Every async method that does real work — accept a token and pass it on',
      'Long loops: check `ThrowIfCancellationRequested()` once per iteration or batch',
      'Any call to a database, HTTP endpoint or search engine'
    ],
    avoid: [
      'Swallowing `OperationCanceledException` — it is the success path of a cancelled operation, not an error to log as one',
      'Accepting a token and then never passing it to the calls you make; that is worse than not accepting it',
      'The obsolete `ICancellationToken` interface in new code'
    ],
    api: [
      { name: 'ICancellationToken (obsolete, VC0014)', file: 'src/VirtoCommerce.Platform.Core/Common/ICancellationToken.cs' },
      { name: 'CancellationTokenWrapper', file: 'src/VirtoCommerce.Platform.Core/Common/CancellationTokenWrapper.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// ✕ Obsolete: ICancellationToken is a Hangfire compatibility shim for legacy queue items.\n' +
'//   Marked [Obsolete] with diagnostic id VC0014.\n' +
'Task ExportAsync(Stream stream, ExportImportOptions options,\n' +
'    Action<ExportImportProgressInfo> progress, ICancellationToken token);\n' +
'\n' +
'// ✓ The current shape. Note the platform bridges old implementers automatically\n' +
'//   via CancellationTokenWrapper, so both overloads exist on IExportSupport.\n' +
'Task ExportAsync(Stream stream, ExportImportOptions options,\n' +
'    Action<ExportImportProgressInfo> progress, CancellationToken cancellationToken);'
    },
    gotchas: [
      '`ICancellationToken` is `[Obsolete]` with diagnostic id `VC0014`. Because the solution builds with `TreatWarningsAsErrors`, using it will fail your build unless suppressed — which is the intended nudge.',
      '`IExportSupport` and `IImportSupport` carry both overloads: the obsolete one throws `NotImplementedException` by default, and the modern one bridges to it via `CancellationTokenWrapper` for old implementers.',
      'A token that is never checked is decoration. Cancellation only happens where you look for it.'
    ],
    docs: [],
    seeAlso: ['background-jobs', 'hosted-service', 'backup-restore'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'async-lock',
    symbol: 'Al',
    name: 'Async lock',
    family: 'execution',
    adoption: 'platform',
    layer: 'platform',
    tags: ['semaphore', 'mutex', 'in-process', 'concurrency', 'lockbykey'],
    oneLiner: 'An awaitable in-process lock keyed by string — one process only, not a cluster.',
    pattern: 'Keyed semaphore with reference counting. `AsyncLock.GetLockByKey(key)` returns a per-key lock; `await LockAsync()` yields an `IDisposable` you release with `using`. Locks are reference-counted so unused keys do not accumulate.',
    whenToUse: [
      'Serialising concurrent work on the same key inside one process — cache loading, lazy initialisation',
      'Protecting a critical section where `lock` would be wrong because the body awaits',
      'Single-instance deployments where a distributed lock would be overkill'
    ],
    avoid: [
      'Multi-instance coordination — this lock is invisible to other processes. Use the distributed lock',
      'Holding it across a network call you do not control; you are serialising every caller on someone else\'s latency',
      'Using `lock` around `await` instead. It does not compile, and the workarounds are worse'
    ],
    api: [
      { name: 'AsyncLock.GetLockByKey', file: 'src/VirtoCommerce.Platform.Core/Common/AsyncLock.cs' },
      { name: 'AsyncLock.LockAsync', file: 'src/VirtoCommerce.Platform.Core/Common/AsyncLock.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Serialise per-key work inside this process.\n' +
'using (await AsyncLock.GetLockByKey($"rebuild:{storeId}").LockAsync())\n' +
'{\n' +
'    // Only one caller per storeId gets in here at a time, within this instance.\n' +
'    await Rebuild(storeId);\n' +
'}'
    },
    gotchas: [
      'In-process only. On two instances, two callers hold "the same" lock simultaneously — this is the single most common misuse.',
      'Forgetting the `using` leaks the lock for the process lifetime and deadlocks every later caller on that key.',
      'For cache loading you usually do not need this: `GetOrCreateExclusiveAsync` already locks per key.'
    ],
    docs: [],
    seeAlso: ['distributed-lock', 'platform-memory-cache', 'hosted-service'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'channels',
    symbol: 'Ch',
    name: 'Channels',
    family: 'execution',
    adoption: 'available',
    layer: 'platform',
    tags: ['system.threading.channels', 'producer', 'consumer', 'queue', 'dotnet'],
    oneLiner: 'In-memory producer/consumer queues from .NET — available, but nothing in the platform uses them.',
    pattern: 'Bounded or unbounded in-memory channel with async readers and writers, usually drained by a hosted service. A good fit for in-process pipelines; no persistence and no cross-instance delivery.',
    whenToUse: [
      'Smoothing bursts of in-process work where losing the backlog on restart is genuinely acceptable',
      'Decoupling a fast producer from a slower consumer inside one instance',
      'Applying backpressure with a bounded channel rather than unbounded memory growth'
    ],
    avoid: [
      'Anything that must survive a restart or reach another instance — use background jobs',
      'Reaching for it before you have a measured in-process bottleneck; the platform has no precedent to copy',
      'Unbounded channels in request paths: they turn a load spike into an out-of-memory failure'
    ],
    api: [
      { name: 'System.Threading.Channels.Channel<T>', file: '(.NET base class library — no platform usage)' }
    ],
    useInstead: 'For durable or cross-instance work use `IBackgroundJob`. For a continuous in-process loop use a `BackgroundService`.',
    note: 'A grep across `src/` finds no `System.Threading.Channels` usage. Nothing stops you using it, but you will be establishing the pattern rather than following one — and there is no platform code to copy conventions from.',
    gotchas: [
      'A channel is not a queue in the durability sense. Process exit discards everything buffered.',
      'You still need something to drain it — normally a hosted service, with all of that atom\'s caveats.'
    ],
    docs: [],
    seeAlso: ['background-jobs', 'hosted-service'],
    verifiedAgainst: '3.1053.0'
  },

  // ================================================================ CACHING

  {
    id: 'platform-memory-cache',
    symbol: 'MC',
    name: 'Cache',
    family: 'caching',
    adoption: 'platform',
    layer: 'platform',
    tags: ['imemorycache', 'getorcreateexclusive', 'cachekey', 'performance'],
    oneLiner: 'The one caching primitive you will use constantly: in-process memory, keyed, with coordinated invalidation.',
    pattern: 'Cache-aside with an exclusive loader. `IPlatformMemoryCache` extends `IMemoryCache`; `GetOrCreateExclusiveAsync` guarantees one loader runs per key under concurrency (no cache stampede), and the entry is tied to a **cache region** token so a write elsewhere can invalidate it.',
    whenToUse: [
      'Any read that is expensive and repeated — the default for service read paths',
      'Search results, keyed by the criteria\'s own cache key',
      'Reference data: countries, dictionaries, settings, metadata'
    ],
    avoid: [
      'Caching per-user or per-request data here — use the request-scoped cache instead',
      'Caching without an expiration token: an entry nothing can invalidate is a bug with a long fuse',
      'Treating it as shared state across instances. It is per-process; only invalidation is coordinated'
    ],
    api: [
      { name: 'IPlatformMemoryCache', file: 'src/VirtoCommerce.Platform.Core/Caching/IPlatformMemoryCache.cs' },
      { name: 'MemoryCacheExtensions.GetOrCreateExclusiveAsync', file: 'src/VirtoCommerce.Platform.Core/Caching/MemoryCacheExtensions.cs' },
      { name: 'MemoryCacheExtensions.GetOrLoadByIdsAsync', file: 'src/VirtoCommerce.Platform.Core/Caching/MemoryCacheExtensions.cs' },
      { name: 'CacheKey.With', file: 'src/VirtoCommerce.Platform.Core/Caching/CacheKey.cs' },
      { name: 'PlatformMemoryCache', file: 'src/VirtoCommerce.Platform.Caching/PlatformMemoryCache.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// src/VirtoCommerce.Platform.Data/ChangeLog/ChangeLogSearchService.cs — the canonical shape.\n' +
'public virtual async Task<ChangeLogSearchResult> SearchAsync(ChangeLogSearchCriteria criteria)\n' +
'{\n' +
'    // Key includes the owning type, the method, and the criteria\'s own cache key.\n' +
'    var cacheKey = CacheKey.With(GetType(), nameof(SearchAsync), criteria.GetCacheKey());\n' +
'\n' +
'    return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>\n' +
'    {\n' +
'        // Without this token the entry can never be invalidated by a write.\n' +
'        cacheEntry.AddExpirationToken(ChangeLogCacheRegion.CreateChangeToken());\n' +
'\n' +
'        var result = AbstractTypeFactory<ChangeLogSearchResult>.TryCreateInstance();\n' +
'        using var repository = _repositoryFactory();\n' +
'        repository.DisableChangesTracking();\n' +
'        // ... query, project to models ...\n' +
'        return result;\n' +
'    });\n' +
'}'
    },
    gotchas: [
      'Forgetting `AddExpirationToken` is the most common caching bug in Virto code: it works in development and serves stale data in production.',
      '`GetOrCreateExclusiveAsync` locks per key, so a slow loader blocks only its own key — but it does block. Keep loaders free of unrelated work.',
      'Null results are cached by default (`cacheNullValue: true`) to avoid a hammering miss — which also means "not found" sticks around.',
      'Cache keys are normalized, so do not rely on exact string identity when debugging a key.',
      'For batches of entities by id, `GetOrLoadByIdsAsync` loads only the misses instead of all-or-nothing.'
    ],
    docs: [
      { label: 'Essential caching', page: 'Fundamentals/Caching/01-overview' }
    ],
    seeAlso: ['cache-regions', 'redis-cache-bus', 'request-scoped-cache', 'cache-disabler', 'hybrid-cache'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'cache-regions',
    symbol: 'Cr',
    name: 'Cache regions',
    family: 'caching',
    adoption: 'platform',
    layer: 'platform',
    tags: ['cancellationtoken', 'changetoken', 'invalidation', 'expire', 'region'],
    oneLiner: 'Named groups of cache entries you can invalidate together — how a write clears the reads that depend on it.',
    pattern: 'Change token per region, backed by a `CancellationTokenSource`. Entries opt into a region with `AddExpirationToken(Region.CreateChangeToken())`; a write calls `ExpireRegion()` or `ExpireTokenForKey(id)`, cancelling the token and evicting every entry that registered it. Regions are strongly typed: `CancellableCacheRegion<T>` derives its name from `T`.',
    whenToUse: [
      'Every cached read in a service — pair each entity type with its own region',
      'Invalidating "everything derived from this entity" after a save, without tracking individual keys',
      'Per-id invalidation when a region-wide flush would be too blunt'
    ],
    avoid: [
      'Inventing a region per method. Region granularity should follow the entity, not the call site',
      'Expiring a global region on every write — correct, but it throws away unrelated cached work',
      'Assuming an expire on one instance clears the others; that is what the Redis bus is for'
    ],
    api: [
      { name: 'CancellableCacheRegion<T>', file: 'src/VirtoCommerce.Platform.Core/Caching/CancellableCacheRegion.cs' },
      { name: 'GenericCachingRegion<T>', file: 'src/VirtoCommerce.Platform.Caching/GenericCachingRegion.cs' },
      { name: 'GenericSearchCachingRegion<T>', file: 'src/VirtoCommerce.Platform.Caching/GenericSearchCachingRegion.cs' },
      { name: 'GlobalCacheRegion', file: 'src/VirtoCommerce.Platform.Caching/GlobalCacheRegion.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Module.Data — one region per cached concern.\n' +
'public class MyEntityCacheRegion : CancellableCacheRegion<MyEntityCacheRegion>\n' +
'{\n' +
'}\n' +
'\n' +
'// Read side: enrol the entry in the region.\n' +
'cacheEntry.AddExpirationToken(MyEntityCacheRegion.CreateChangeToken());\n' +
'\n' +
'// Write side: after saving, clear what depends on it.\n' +
'MyEntityCacheRegion.ExpireRegion();                 // everything in the region\n' +
'MyEntityCacheRegion.ExpireTokenForKey(entity.Id);   // just one entity\'s entries'
    },
    gotchas: [
      'A region cancels its token and replaces it, so entries registered *after* the expire are unaffected — expire after the write commits, not before.',
      'Search results usually need their own region separate from entity reads; otherwise every entity save also throws away every cached search.',
      'Expiring a region is cheap; recomputing everything it held may not be. Watch for expire-per-item loops during bulk saves — expire once at the end.'
    ],
    docs: [
      { label: 'Essential caching', page: 'Fundamentals/Caching/01-overview' }
    ],
    seeAlso: ['platform-memory-cache', 'redis-cache-bus', 'domain-events'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'redis-cache-bus',
    symbol: 'Rc',
    name: 'Redis cache bus',
    family: 'caching',
    adoption: 'platform',
    layer: 'platform',
    tags: ['redis', 'scale-out', 'invalidation', 'pubsub', 'coherence'],
    oneLiner: 'Keeps per-instance memory caches coherent across a cluster. It stores invalidation messages, not cached values.',
    pattern: 'Publish/subscribe invalidation. `RedisPlatformMemoryCache` wraps the in-memory cache and publishes a `RedisCachingMessage` whenever a region or key is expired; every other instance receives it and expires the same region locally. Each instance keeps its own copy of the data.',
    whenToUse: [
      'Any deployment with more than one instance — at that point this stops being optional',
      'When users report "the change shows on some page loads but not others" — the classic symptom of missing coherence',
      'Blue/green and rolling deploys, where old and new instances run side by side'
    ],
    avoid: [
      'Expecting it to be a shared cache. Every instance still computes and stores its own values',
      'Relying on it for correctness-critical consistency; propagation is fast but not instant',
      'Chatty per-item invalidation during bulk work — every message fans out to every instance'
    ],
    api: [
      { name: 'RedisPlatformMemoryCache', file: 'src/VirtoCommerce.Platform.Caching/Redis/RedisPlatformMemoryCache.cs' },
      { name: 'RedisCachingMessage', file: 'src/VirtoCommerce.Platform.Caching/Redis/RedisCachingMessage.cs' },
      { name: 'RedisCachingOptions', file: 'src/VirtoCommerce.Platform.Caching/Redis/RedisCachingOptions.cs' }
    ],
    snippet: {
      lang: 'json',
      code:
'// appsettings.json — the connection string is what switches coherence on.\n' +
'{\n' +
'  "ConnectionStrings": {\n' +
'    "RedisConnectionString": "localhost:6379,ssl=False,abortConnect=False"\n' +
'  },\n' +
'  "Caching": {\n' +
'    "CacheEnabled": true,\n' +
'    "CacheSlidingExpiration": "0:15:0"\n' +
'  }\n' +
'}'
    },
    gotchas: [
      'This is the single most misunderstood piece of Virto caching: Redis here carries *invalidation messages*, not cache entries. Losing Redis costs coherence, not the cache.',
      'Without it, a save on instance A leaves instance B serving stale data until its own entry expires — intermittent staleness that is miserable to reproduce.',
      'Redis does triple duty in a Virto deployment: this bus, the distributed lock, and the SignalR backplane. Keep them straight when diagnosing.',
      'Cache memory is per instance, so cluster memory scales with instance count rather than being pooled.'
    ],
    docs: [
      { label: 'Essential caching', page: 'Fundamentals/Caching/01-overview' },
      { label: 'Scale out on Azure', page: 'Fundamentals/Scalability/scaling-configuration-on-azure-cloud' }
    ],
    seeAlso: ['cache-regions', 'platform-memory-cache', 'distributed-lock', 'push-notifications'],
    molecule: 'deployment',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'request-scoped-cache',
    symbol: 'Rs',
    name: 'Request-scoped cache',
    family: 'caching',
    adoption: 'platform',
    layer: 'platform',
    tags: ['per-request', 'httpcontext', 'n+1', 'dataloader', 'graphql'],
    oneLiner: 'A cache that lives exactly as long as one request — the cheap fix for repeated loads inside a single call.',
    pattern: 'Per-request memoisation. `IRequestScopedCache` stores values for the lifetime of the current request via `HttpRequestScopedCacheAccessor`, and `GetOrLoadMapByIdsAsync` loads only the ids not already present — the dataloader pattern, minus the ceremony.',
    whenToUse: [
      'The same entity loaded several times while serving one request',
      'GraphQL resolvers, where the N+1 problem is structural rather than accidental',
      'Per-user or per-request data that must never leak into the shared memory cache'
    ],
    avoid: [
      'Data that should outlive the request — that is the memory cache',
      'Background jobs and hosted services: there is no request, so there is no scope',
      'Using it as a way to avoid passing state through your own call chain'
    ],
    api: [
      { name: 'IRequestScopedCache', file: 'src/VirtoCommerce.Platform.Core/Caching/IRequestScopedCache.cs' },
      { name: 'IRequestScopedCacheAccessor', file: 'src/VirtoCommerce.Platform.Core/Caching/IRequestScopedCacheAccessor.cs' },
      { name: 'HttpRequestScopedCacheAccessor', file: 'src/VirtoCommerce.Platform.Caching/HttpRequestScopedCacheAccessor.cs' },
      { name: 'RequestScopedCacheExtensions', file: 'src/VirtoCommerce.Platform.Core/Caching/RequestScopedCacheExtensions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Simple memoisation for the current request.\n' +
'var store = await _requestCache.GetOrAddAsync(\n' +
'    $"store:{storeId}",\n' +
'    () => _storeService.GetByIdAsync(storeId));\n' +
'\n' +
'// Batch by ids — only the misses reach loadMissing. This is the N+1 fix.\n' +
'var map = await _requestCache.GetOrLoadMapByIdsAsync(\n' +
'    keyPrefix: "product",\n' +
'    ids: productIds,\n' +
'    idSelector: x => x.Id,\n' +
'    loadMissing: missing => _productService.GetAsync(missing));'
    },
    gotchas: [
      'Outside a request there is no scope to store into — check behaviour before reusing request-scoped code inside a job.',
      'It is not a substitute for the memory cache: nothing survives the response, so a hot read still hits the database once per request.',
      'Per-user data belongs here precisely because it cannot leak; putting it in the shared cache is a data-disclosure bug, not just a design smell.'
    ],
    docs: [
      { label: 'Essential caching', page: 'Fundamentals/Caching/01-overview' }
    ],
    seeAlso: ['platform-memory-cache', 'current-user'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'cache-disabler',
    symbol: 'Cd',
    name: 'Cache disabler',
    family: 'caching',
    adoption: 'platform',
    layer: 'platform',
    tags: ['debug', 'diagnostics', 'bypass', 'cacheenabled', 'import'],
    oneLiner: 'Turn caching off for a scope or for the whole app — a diagnostic tool and an import-time necessity.',
    pattern: 'Ambient scoped switch, plus global configuration. `CacheDisabler.DisableCache()` returns an `IDisposable`: inside that scope reads bypass the cache. `CachingOptions.CacheEnabled` does the same globally from configuration.',
    whenToUse: [
      'Confirming whether a bug is a stale cache entry or genuinely wrong data — the fastest way to split that question',
      'Bulk import or migration code that must read what it just wrote',
      'Locally reproducing a problem that only appears on a cold cache'
    ],
    avoid: [
      'Leaving it disabled globally in production. Performance falls off a cliff, and the platform assumes caching is on',
      'Using it to paper over a missing expiration token — fix the invalidation instead',
      'Wrapping large call trees; you disable far more than you meant to'
    ],
    api: [
      { name: 'CacheDisabler.DisableCache', file: 'src/VirtoCommerce.Platform.Core/Caching/CacheDisabler.cs' },
      { name: 'CacheDisabler.CacheDisabled', file: 'src/VirtoCommerce.Platform.Core/Caching/CacheDisabler.cs' },
      { name: 'CachingOptions.CacheEnabled', file: 'src/VirtoCommerce.Platform.Caching/CachingOptions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Read-after-write inside a scope, without touching global configuration.\n' +
'using (CacheDisabler.DisableCache())\n' +
'{\n' +
'    var fresh = await _service.GetByIdAsync(id);   // bypasses the cache\n' +
'}\n' +
'\n' +
'// Globally, from appsettings.json:  "Caching": { "CacheEnabled": false }'
    },
    gotchas: [
      'The flag is ambient (an async-local), so it follows the async flow — including into code you did not intend to affect.',
      'If disabling the cache fixes your bug, the real defect is a missing or wrong expiration token. Do not ship the disabler as the fix.',
      'Global `CacheEnabled: false` is a legitimate debugging setting and a terrible production one.'
    ],
    docs: [
      { label: 'Essential caching', page: 'Fundamentals/Caching/01-overview' },
      { label: 'Debugging without source code', page: 'Tutorials-and-How-tos/How-tos/debugging' }
    ],
    seeAlso: ['platform-memory-cache', 'cache-regions', 'developer-tools'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'hybrid-cache',
    symbol: 'HC',
    name: 'Hybrid Cache',
    family: 'caching',
    adoption: 'available',
    layer: 'platform',
    tags: ['dotnet', 'distributed', 'redis', 'stampede', 'idistributedcache'],
    oneLiner: '.NET\'s own two-tier and distributed caching APIs. Neither is used here — reach for the platform cache instead.',
    pattern: 'Two-tier (memory plus out-of-process backing store) with built-in stampede protection, in `HybridCache`; raw byte-array distributed storage in `IDistributedCache`. Conceptually attractive, but they do not participate in Virto\'s region-based invalidation.',
    whenToUse: [
      'Practically never inside a Virto module. The platform cache already gives you exclusive loading and cluster-wide invalidation',
      'Only if you need a genuinely shared cache store — and then only after confirming a memory cache plus the Redis bus will not do'
    ],
    avoid: [
      'Introducing it alongside `IPlatformMemoryCache`. You end up with two caches, one invalidation mechanism, and stale reads nobody can explain',
      'Assuming Virto\'s Redis integration is an `IDistributedCache`. It is not — it carries invalidation messages, not values'
    ],
    api: [
      { name: 'Microsoft.Extensions.Caching.Hybrid.HybridCache', file: '(.NET — not referenced by the platform)' },
      { name: 'IDistributedCache', file: '(.NET — no store registered by the platform)' }
    ],
    useInstead: '`IPlatformMemoryCache` with `GetOrCreateExclusiveAsync` and a cache region token — see [[platform-memory-cache]].',
    note: 'Verified by grep across `src/`: there is no `HybridCache` package reference and no `IDistributedCache` store registration. The platform\'s answer to the same problems is per-instance memory plus the Redis invalidation bus.',
    gotchas: [
      '`HybridCache` solves cache stampede, which `GetOrCreateExclusiveAsync` already solves here — the reason to adopt it is usually already met.',
      'Two caching systems in one process is a correctness problem, not a redundancy: only one of them hears a region expire.'
    ],
    docs: [
      { label: 'Essential caching', page: 'Fundamentals/Caching/01-overview' }
    ],
    seeAlso: ['platform-memory-cache', 'redis-cache-bus', 'cache-regions'],
    verifiedAgainst: '3.1053.0'
  },

  // ================================================================ CONFIG & METADATA

  {
    id: 'settings',
    symbol: 'St',
    name: 'Settings',
    family: 'config',
    adoption: 'platform',
    layer: 'platform',
    tags: ['isettingsmanager', 'settingdescriptor', 'configuration', 'module.manifest', 'store'],
    oneLiner: 'Operator-editable configuration, declared in code and overridable per object — reach for this before appsettings.',
    pattern: 'Declared registry plus a key-value store. A module declares `SettingDescriptor`s as static members of `ModuleConstants`, registers them at startup, and reads them through `ISettingsManager`. Values resolve per object (store, provider) with the declared default as fallback, and the Admin UI renders the editor from the descriptor.',
    whenToUse: [
      'Anything an operator should change without a deploy: thresholds, toggles, cron expressions, provider credentials',
      'Configuration that differs per store or per provider instance',
      'Feature flags scoped to a module'
    ],
    avoid: [
      'Infrastructure configuration — connection strings and hosting concerns belong in `appsettings.json` and the options pattern',
      'Secrets operators should not see; use configuration and a secret store',
      'Reading a setting inside a tight loop without hoisting the value out'
    ],
    api: [
      { name: 'ISettingsManager', file: 'src/VirtoCommerce.Platform.Core/Settings/ISettingsManager.cs' },
      { name: 'SettingDescriptor', file: 'src/VirtoCommerce.Platform.Core/Settings/SettingDescriptor.cs' },
      { name: 'ISettingsRegistrar.RegisterSettings', file: 'src/VirtoCommerce.Platform.Core/Settings/ISettingsRegistrar.cs' },
      { name: 'SettingsExtension.GetValue<T>', file: 'src/VirtoCommerce.Platform.Core/Settings/SettingsExtension.cs' },
      { name: 'ObjectSettingEntry', file: 'src/VirtoCommerce.Platform.Core/Settings/ObjectSettingEntry.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Module.Core/ModuleConstants.cs — declare once, reference everywhere.\n' +
'public static class Settings\n' +
'{\n' +
'    public static SettingDescriptor ExportPageSize { get; } = new()\n' +
'    {\n' +
'        Name = "MyModule.Export.PageSize",\n' +
'        GroupName = "MyModule|Export",\n' +
'        ValueType = SettingValueType.Integer,\n' +
'        DefaultValue = 50,\n' +
'        IsPublic = false\n' +
'    };\n' +
'\n' +
'    public static IEnumerable<SettingDescriptor> AllSettings => [ExportPageSize];\n' +
'}\n' +
'\n' +
'// Module.Web/Module.cs — PostInitialize\n' +
'var registrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();\n' +
'registrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);\n' +
'\n' +
'// Reading: pass the descriptor, so the name and default cannot drift apart.\n' +
'var pageSize = await _settingsManager.GetValueAsync<int>(ModuleConstants.Settings.ExportPageSize);\n' +
'\n' +
'// Per-object override (e.g. per store).\n' +
'var setting = await _settingsManager.GetObjectSettingAsync(\n' +
'    ModuleConstants.Settings.ExportPageSize.Name, nameof(Store), storeId);'
    },
    gotchas: [
      'Pass the `SettingDescriptor` to `GetValue`, not a bare string name — the descriptor carries the default, so a typo becomes a compile error instead of a silent zero.',
      '`RestartRequired = true` tells the Admin UI to warn the operator; it does not restart anything for you.',
      'A setting declared but never registered simply does not exist at runtime: no error, no editor, and the default is all you ever read.',
      'Settings can also be declared in `module.manifest`; both paths converge on the same registry, so pick one per setting and stay consistent.',
      '`IsPublic = true` exposes the value to the storefront — check this before putting anything sensitive in a setting.'
    ],
    docs: [
      { label: 'Settings V2', page: 'Fundamentals/Modularity/06-module-manifest-file' },
      { label: 'Manifest settings', page: 'Fundamentals/Modularity/06-module-manifest-file' }
    ],
    seeAlso: ['dynamic-properties', 'options-pattern', 'recurring-jobs', 'localizations'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'dynamic-properties',
    symbol: 'DP',
    name: 'Dynamic properties',
    family: 'config',
    adoption: 'platform',
    layer: 'platform',
    tags: ['custom field', 'metadata', 'no-code', 'eav', 'extend'],
    oneLiner: 'Add a field to an existing entity without a migration, a code change or a fork. The first thing to try when extending.',
    pattern: 'Entity-attribute-value with a typed registry. Any entity implementing `IHasDynamicProperties` exposes an `ObjectType` and a `DynamicProperties` collection; properties are declared at runtime (or registered at startup) and stored generically, with optional dictionary-backed value lists.',
    whenToUse: [
      'A customer-specific field on a vendor entity — the canonical no-code extension',
      'Fields an operator should be able to add themselves',
      'Anything you would otherwise add by deriving a type and writing a migration, where the field is only ever data'
    ],
    avoid: [
      'Fields you filter or sort large result sets by — EAV storage makes that expensive',
      'Core domain fields with real business behaviour; those deserve a proper typed property',
      'Dozens of properties on a hot entity — every read carries them'
    ],
    api: [
      { name: 'IHasDynamicProperties', file: 'src/VirtoCommerce.Platform.Core/DynamicProperties/IHasDynamicProperties.cs' },
      { name: 'IDynamicPropertyService', file: 'src/VirtoCommerce.Platform.Core/DynamicProperties/IDynamicPropertyService.cs' },
      { name: 'IDynamicPropertyRegistrar', file: 'src/VirtoCommerce.Platform.Core/DynamicProperties/IDynamicPropertyRegistrar.cs' },
      { name: 'DynamicPropertiesExtensions', file: 'src/VirtoCommerce.Platform.Core/DynamicProperties/DynamicPropertiesExtensions.cs' },
      { name: 'IDynamicPropertyMetaDataResolver', file: 'src/VirtoCommerce.Platform.Core/DynamicProperties/IDynamicPropertyMetaDataResolver.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Make your own entity extensible the same way vendor entities are.\n' +
'public class MyEntity : AuditableEntity, IHasDynamicProperties\n' +
'{\n' +
'    public string ObjectType => GetType().FullName;\n' +
'    public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }\n' +
'}\n' +
'\n' +
'// Reading — the default is a required argument, and the value is coerced\n' +
'// with Convert.ChangeType, so a bad cast throws here rather than at the call site.\n' +
'var priority = order.GetDynamicPropertyValue("AbcPriority", defaultValue: "Normal");\n' +
'\n' +
'// Writing — there is no Set… helper. You construct the property and its value.\n' +
'order.DynamicProperties =\n' +
'[\n' +
'    new DynamicObjectProperty\n' +
'    {\n' +
'        Name = "AbcPriority",\n' +
'        Values =\n' +
'        [\n' +
'            new DynamicPropertyObjectValue\n' +
'            {\n' +
'                PropertyName = "AbcPriority",\n' +
'                ValueType = DynamicPropertyValueType.ShortText,\n' +
'                Value = "High"\n' +
'            }\n' +
'        ]\n' +
'    }\n' +
'];'
    },
    gotchas: [
      'Prefix your property names with a solution abbreviation (`AbcPriority`). A future vendor property with the same name is otherwise a collision you cannot rename your way out of.',
      'The API is asymmetric: `GetDynamicPropertyValue` exists, a matching setter does not. Writing means building a `DynamicObjectProperty` with its `Values` collection by hand.',
      'The getter takes only the *first* value across all matching properties, so a multi-value property silently gives you one of them.',
      'Dictionary-backed properties resolve to the dictionary item\'s `Name`, not the raw stored value — the getter unwraps `DynamicPropertyDictionaryItem` (including from a `JObject`) for you, which is convenient until you compare against the wrong thing.',
      'Dynamic property values load separately from the entity, so check they are actually populated before reading rather than assuming.',
      'This is level 1 of the extensibility model. If you need behaviour rather than data, go to `AbstractTypeFactory` instead.'
    ],
    docs: [
      { label: 'Using dynamic properties', page: 'Fundamentals/Dynamic-Properties/using-DynamicPropertyAccessor' },
      { label: 'Extending domain models', page: 'Tutorials-and-How-tos/Tutorials/extending-domain-models' }
    ],
    seeAlso: ['abstract-type-factory', 'settings', 'ef-core'],
    molecule: 'extensibility-decision-tree',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'options-pattern',
    symbol: 'Op',
    name: 'Options pattern',
    family: 'config',
    adoption: 'platform',
    layer: 'platform',
    tags: ['ioptions', 'appsettings', 'configuration', 'ioptionsmonitor', 'dotnet'],
    oneLiner: 'Strongly typed `appsettings.json` binding — for infrastructure configuration, as opposed to operator settings.',
    pattern: 'Bind a configuration section to a POCO and inject it. `IOptions<T>` for values fixed at startup, `IOptionsMonitor<T>` when the value should follow a configuration reload. The platform uses both, `PlatformOptions` being the central example.',
    whenToUse: [
      'Connection strings, paths, feature switches that belong to the deployment rather than the operator',
      'Anything read at startup to decide how services are wired',
      'Values that must be present before the database is reachable'
    ],
    avoid: [
      'Configuration an operator should change from the Admin UI — that is a platform setting',
      'Injecting `IConfiguration` and reading raw string keys; you lose validation, defaults and discoverability',
      '`IOptions<T>` for something that must react to a config change — it is captured once'
    ],
    api: [
      { name: 'PlatformOptions', file: 'src/VirtoCommerce.Platform.Core/PlatformOptions.cs' },
      { name: 'CachingOptions', file: 'src/VirtoCommerce.Platform.Caching/CachingOptions.cs' },
      { name: 'AuthorizationOptions', file: 'src/VirtoCommerce.Platform.Core/Security/AuthorizationOptions.cs' },
      { name: 'IOptionsMonitor<T> usage', file: 'src/VirtoCommerce.Platform.Web/Security/Authentication/ApiKeyAuthenticationHandler.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Module.Web — bind a section from appsettings.json.\n' +
'services.AddOptions<MyModuleOptions>()\n' +
'    .Bind(Configuration.GetSection("MyModule"))\n' +
'    .ValidateDataAnnotations()      // fail at startup, not at first use\n' +
'    .ValidateOnStart();\n' +
'\n' +
'// Fixed at startup.\n' +
'public class MyService(IOptions<MyModuleOptions> options) { }\n' +
'\n' +
'// Follows configuration reloads — see ApiKeyAuthenticationHandler for a real example.\n' +
'public class MyHandler(IOptionsMonitor<MyModuleOptions> options)\n' +
'{\n' +
'    private MyModuleOptions Current => options.CurrentValue;\n' +
'}'
    },
    gotchas: [
      '`IOptions<T>` is a singleton snapshot. If you edit `appsettings.json` expecting a live change, you need `IOptionsMonitor<T>`.',
      '`ValidateOnStart()` turns a typo in configuration into a startup failure instead of a null-reference hours later.',
      'The line between an option and a setting is ownership: the deployment owns options, the operator owns settings. Getting it wrong means an operator files a ticket to change a number.'
    ],
    docs: [],
    seeAlso: ['settings', 'dependency-injection', 'platform-startup'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'external-processes',
    symbol: 'Xp',
    name: 'External tool processes',
    family: 'config',
    adoption: 'platform',
    layer: 'platform',
    tags: ['process', 'cli', 'exe', 'toolpath', 'pdf', 'shell'],
    oneLiner: 'Launching an external command-line tool from the platform, portably across Windows and Linux.',
    pattern: 'Descriptor plus fluent builder. `ProcessSettings` captures the tool name, path, working directory, environment variables and arguments; `GetFullPathTool()` resolves the executable per OS, so the same registration works on Windows and Linux containers.',
    whenToUse: [
      'Wrapping a real command-line tool you must shell out to — PDF renderers, image processors, converters',
      'Tools shipped alongside the app under the configured processes path',
      'Anywhere you were about to hand-roll `Process.Start` with a hardcoded `.exe`'
    ],
    avoid: [
      'Work a library could do in-process; a subprocess is a deployment dependency and a security surface',
      'Passing unvalidated user input as arguments — that is command injection',
      'Long-running tools in a request. Shell out from a background job instead'
    ],
    api: [
      { name: 'ProcessSettings', file: 'src/VirtoCommerce.Platform.Core/ProcessSettings/ProcessSettings.cs' },
      { name: 'ProcessSettingsExtensions', file: 'src/VirtoCommerce.Platform.Core/ProcessSettings/ProcessSettingsExtensions.cs' },
      { name: 'PlatformOptions.ProcessesPath', file: 'src/VirtoCommerce.Platform.Core/PlatformOptions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Fluent configuration; ToolPath defaults from PlatformOptions.ProcessesPath.\n' +
'var settings = myProcessSettings\n' +
'    .SetToolPath("tools/wkhtmltopdf")\n' +
'    .SetWorkingDirectory(workDir)\n' +
'    .SetArguments(["--quiet", inputPath, outputPath]);\n' +
'\n' +
'// GetFullPathTool() resolves the executable for the current OS.\n' +
'var executable = settings.GetFullPathTool();'
    },
    gotchas: [
      'Despite the name, this has nothing to do with application configuration — it configures *external processes*. Do not confuse it with settings or options.',
      'The tool must actually be present in the container image; a working local build proves nothing about deployment.',
      'Argument arrays avoid one class of quoting bug but not injection. Validate anything user-supplied.'
    ],
    docs: [
      { label: 'Generate a PDF file', page: 'Tutorials-and-How-tos/How-tos/generating-pdfs' }
    ],
    seeAlso: ['options-pattern', 'file-operations', 'background-jobs'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'localizations',
    symbol: 'Lz',
    name: 'Localizations',
    family: 'config',
    adoption: 'platform',
    layer: 'platform',
    tags: ['i18n', 'translation', 'language', 'locale', 'admin ui'],
    oneLiner: 'Translated strings for the Admin UI and localizable entity values, contributed per module.',
    pattern: 'Provider aggregation. Each module ships JSON localization files; `ITranslationService` merges every provider\'s data per language and reports the installed languages. Separately, `LocalizableSetting` and `ILocalizedItemService` handle per-language *data* values rather than UI strings.',
    whenToUse: [
      'Any Admin UI string a module contributes — never hardcode display text',
      'Entity values that differ per language: status names, category labels, dictionary items',
      'Adding a new language to an existing deployment'
    ],
    avoid: [
      'Concatenating translated fragments; grammar differs between languages and the result is unlocalizable',
      'Using UI translations for data that belongs to an entity — those are different mechanisms',
      'Assuming a key exists in every language. Missing keys fall back, and the fallback is visible'
    ],
    api: [
      { name: 'ITranslationService', file: 'src/VirtoCommerce.Platform.Core/Localizations/ITranslationService.cs' },
      { name: 'ITranslationDataProvider', file: 'src/VirtoCommerce.Platform.Core/Localizations/ITranslationDataProvider.cs' },
      { name: 'ILocalizedItemService', file: 'src/VirtoCommerce.Platform.Core/Localizations/ILocalizedItemService.cs' },
      { name: 'LocalizableSetting', file: 'src/VirtoCommerce.Platform.Core/Settings/LocalizableSetting.cs' },
      { name: 'TranslationOptions', file: 'src/VirtoCommerce.Platform.Core/Localizations/TranslationOptions.cs' }
    ],
    snippet: {
      lang: 'json',
      code:
'// Module.Web/Localizations/en.MyModule.json — merged with every other module\'s file.\n' +
'{\n' +
'  "MyModule": {\n' +
'    "blades": {\n' +
'      "list": { "title": "My entities" }\n' +
'    },\n' +
'    "settings": {\n' +
'      "MyModule.Export.PageSize": {\n' +
'        "title": "Export page size",\n' +
'        "description": "Rows per export batch"\n' +
'      }\n' +
'    }\n' +
'  }\n' +
'}'
    },
    gotchas: [
      'Localization files load at runtime from the module\'s deployed folder, so editing them in the source tree does not change a running instance until the assets are deployed.',
      'Two modules using the same top-level key merge into each other. Namespace by module id.',
      'Setting descriptors are localized by their `Name` — rename a setting and every translation for it silently detaches.',
      '`ILocalizedItemService` (entity data) and `ITranslationService` (UI strings) are unrelated despite sounding alike.'
    ],
    docs: [],
    seeAlso: ['settings', 'module-lifecycle'],
    molecule: 'multi-store',
    verifiedAgainst: '3.1053.0'
  },

  // ================================================================ MESSAGING & EVENTS

  {
    id: 'domain-events',
    symbol: 'Ev',
    name: 'Domain events',
    family: 'messaging',
    adoption: 'platform',
    layer: 'platform',
    tags: ['ieventpublisher', 'ieventhandler', 'changing', 'changed', 'decouple'],
    oneLiner: 'The preferred way for modules to react to each other without depending on each other.',
    pattern: 'In-process publish/subscribe with a changing/changed pair. A service publishes a `…ChangingEvent` before committing and a `…ChangedEvent` after; handlers implement `IEventHandler<T>`. `GenericChangedEntry<T>` carries old and new state plus an `EntryState`, so a handler can tell an add from an update.',
    whenToUse: [
      'Reacting to another module\'s data changes — the "reacts to" relationship, and the one to prefer',
      'Cache invalidation after a write',
      'Validation or veto before a write, using the changing event',
      'Anything you would otherwise achieve by calling into a module that should not know about you'
    ],
    avoid: [
      'Long or blocking work in a handler; publishing is in-process and synchronous, so you are extending the caller\'s transaction time',
      'Assuming ordering between handlers of the same event',
      'Relying on a changed event for durability. It is in-process — a crash loses it. Enqueue a job for work that must happen'
    ],
    api: [
      { name: 'IEventPublisher', file: 'src/VirtoCommerce.Platform.Core/Events/IEventPublisher.cs' },
      { name: 'IEventHandler<T>', file: 'src/VirtoCommerce.Platform.Core/Events/IEventHandler.cs' },
      { name: 'GenericChangedEntryEvent<T>', file: 'src/VirtoCommerce.Platform.Core/Events/GenericChangedEntryEvent.cs' },
      { name: 'GenericChangedEntry<T>', file: 'src/VirtoCommerce.Platform.Core/Events/GenericChangedEntry.cs' },
      { name: 'EventSuppressor', file: 'src/VirtoCommerce.Platform.Core/Events/EventSuppressor.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Publish: changing before the commit, changed after it.\n' +
'var changes = models.Select(x => new GenericChangedEntry<MyEntity>(x, EntryState.Modified)).ToList();\n' +
'\n' +
'await _eventPublisher.Publish(new MyEntityChangingEvent(changes));\n' +
'await repository.UnitOfWork.CommitAsync();\n' +
'await _eventPublisher.Publish(new MyEntityChangedEvent(changes));\n' +
'\n' +
'// Handle: registered in DI, resolved per event.\n' +
'public class InvalidateCacheHandler : IEventHandler<MyEntityChangedEvent>\n' +
'{\n' +
'    public Task Handle(MyEntityChangedEvent message)\n' +
'    {\n' +
'        foreach (var entry in message.ChangedEntries)\n' +
'        {\n' +
'            MyEntityCacheRegion.ExpireTokenForKey(entry.NewEntry.Id);\n' +
'        }\n' +
'        return Task.CompletedTask;\n' +
'    }\n' +
'}\n' +
'\n' +
'// Module.Web: services.AddTransient<IEventHandler<MyEntityChangedEvent>, InvalidateCacheHandler>();'
    },
    gotchas: [
      'A throwing handler propagates to the publisher. One module\'s bad handler can fail another module\'s save — wrap non-critical work.',
      '`EventSuppressor.SuppressEvents()` exists for bulk imports where handler-per-row would be ruinous. It is ambient, so it suppresses more than the line you are looking at.',
      'Publishing the changed event *before* the commit is a real and subtle bug: handlers see data that may still roll back.',
      'For cross-process reactions you need the EventBus module. Domain events never leave the process on their own.'
    ],
    docs: [
      { label: 'Extending using events', page: 'Fundamentals/Event-Driven-Development/using-domain-events' }
    ],
    seeAlso: ['inprocess-bus', 'cache-regions', 'eventbus-webhooks', 'generic-crud', 'change-log'],
    molecule: 'extensibility-decision-tree',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'inprocess-bus',
    symbol: 'Bu',
    name: 'In-process bus',
    family: 'messaging',
    adoption: 'platform',
    layer: 'platform',
    tags: ['inprocessbus', 'ihandler', 'registrar', 'dispatch', 'internals'],
    oneLiner: 'The machinery under domain events: how a published event finds its handlers.',
    pattern: 'Handler registry with wrapper-based dispatch. `InProcessBus` resolves every `IHandler<T>` registered for a message type and invokes them; `IEventHandlerRegistrar` lets code register handlers dynamically rather than only through DI.',
    whenToUse: [
      'Registering a handler at runtime instead of at startup — conditional wiring, plug-in scenarios',
      'Understanding why a handler is or is not being called',
      'Building a message-driven mechanism of your own on top of the same dispatch'
    ],
    avoid: [
      'Using it directly for ordinary event handling — register an `IEventHandler<T>` in DI and let the platform wire it',
      'Expecting durability, ordering guarantees or cross-process delivery',
      'Registering handlers per request; registrations are process-wide and will accumulate'
    ],
    api: [
      { name: 'InProcessBus', file: 'src/VirtoCommerce.Platform.Core/Bus/InProcessBus.cs' },
      { name: 'HandlerWrapper', file: 'src/VirtoCommerce.Platform.Core/Bus/HandlerWrapper.cs' },
      { name: 'IEventHandlerRegistrar', file: 'src/VirtoCommerce.Platform.Core/Events/IEventHandlerRegistrar.cs' },
      { name: 'IHandler<T>', file: 'src/VirtoCommerce.Platform.Core/Messages/IHandler.cs' },
      { name: 'ICancellableEventHandler', file: 'src/VirtoCommerce.Platform.Core/Events/ICancellableEventHandler.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// The whole contract handlers implement, from Core/Messages/IHandler.cs:\n' +
'public interface IHandler<in T> where T : IMessage\n' +
'{\n' +
'    Task Handle(T message);\n' +
'}\n' +
'\n' +
'// IEventHandler<T> is just IHandler<T> constrained to IEvent — which is why\n' +
'// "register a handler" and "register a message handler" are the same act here.'
    },
    gotchas: [
      'Handler resolution is by exact message type. A handler for a base event type does not receive derived events unless it is registered for them.',
      'Dynamic registration bypasses DI lifetime management — you own the handler instance\'s lifetime.',
      'This is platform internals. If you are here to fix a handler that does not fire, the answer is almost always a missing DI registration.'
    ],
    docs: [
      { label: 'Extending using events', page: 'Fundamentals/Event-Driven-Development/using-domain-events' }
    ],
    seeAlso: ['domain-events', 'commands', 'dependency-injection'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'commands',
    symbol: 'Cm',
    name: 'Commands',
    family: 'messaging',
    adoption: 'platform',
    layer: 'platform',
    tags: ['icommand', 'icommandsender', 'cqrs', 'mediator'],
    oneLiner: 'A thin command/handler abstraction alongside events — present, but far less used than events.',
    pattern: 'Command dispatch, one handler per command. `ICommandSender.Send` routes a command to its `ICommandHandler`. Where an event says "this happened, react if you like", a command says "do this" and expects exactly one thing to act.',
    whenToUse: [
      'Modelling an explicit imperative operation you want dispatched rather than called directly',
      'CQRS-shaped code where write operations are commands — XAPI modules use this shape heavily',
      'Decoupling a caller from a single implementation without inventing an interface per operation'
    ],
    avoid: [
      'Using a command where an event fits — if several modules may want to react, publish an event',
      'Wrapping every service call in a command; the indirection has to buy something',
      'Expecting multiple handlers. A command has one'
    ],
    api: [
      { name: 'ICommand', file: 'src/VirtoCommerce.Platform.Core/Commands/ICommand.cs' },
      { name: 'ICommandSender', file: 'src/VirtoCommerce.Platform.Core/Commands/ICommandSender.cs' },
      { name: 'ICommandHandler<T>', file: 'src/VirtoCommerce.Platform.Core/Commands/ICommandHandler.cs' },
      { name: 'CommandBase', file: 'src/VirtoCommerce.Platform.Core/Commands/CommandBase.cs' },
      { name: 'ICancellableCommandHandler', file: 'src/VirtoCommerce.Platform.Core/Commands/ICancellableCommandHandler.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Send a command; exactly one handler is expected to act on it.\n' +
'await _commandSender.Send(new RecalculatePricesCommand { PricelistId = id }, cancellationToken);\n' +
'\n' +
'// Contrast with an event, where zero or many modules may react:\n' +
'await _eventPublisher.Publish(new PricesRecalculatedEvent(changes));'
    },
    gotchas: [
      'The platform itself barely uses this; commands are much more prominent in the XAPI modules. Do not assume platform conventions exist for it.',
      'Commands and events share the same underlying dispatch, so a missing registration fails the same quiet way.',
      'One handler per command is a design constraint, not an enforced invariant — registering two is a mistake the compiler will not catch.'
    ],
    docs: [],
    seeAlso: ['domain-events', 'inprocess-bus'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'push-notifications',
    symbol: 'Pn',
    name: 'Push notifications',
    family: 'messaging',
    adoption: 'platform',
    layer: 'platform',
    tags: ['signalr', 'progress', 'realtime', 'backplane', 'admin ui'],
    oneLiner: 'Server-to-browser push over SignalR — how long-running work reports back to the Admin UI.',
    pattern: 'Hub plus persisted notification store, with a backplane for scale-out. `IPushNotificationManager.SendAsync` pushes a `PushNotification` to a user\'s connected clients and stores it so it survives a page reload. A synchroniser task keeps instances in step when scaled out.',
    whenToUse: [
      'Progress and completion for imports, exports, reindexing, module installation',
      'Anything where the Admin UI would otherwise poll an endpoint',
      'Per-user notifications that should still be there after a refresh'
    ],
    avoid: [
      'High-frequency updates — batch them, or you flood both the hub and the UI',
      'Using it as an event bus between server components; it is a UI transport',
      'Assuming delivery. A user with no open browser gets nothing live, only the stored record'
    ],
    api: [
      { name: 'IPushNotificationManager', file: 'src/VirtoCommerce.Platform.Core/PushNotifications/IPushNotificationManager.cs' },
      { name: 'PushNotification', file: 'src/VirtoCommerce.Platform.Core/PushNotifications/PushNotification.cs' },
      { name: 'IPushNotificationStorage', file: 'src/VirtoCommerce.Platform.Core/PushNotifications/IPushNotificationStorage.cs' },
      { name: 'PushNotificationSynchronizerTask', file: 'src/VirtoCommerce.Platform.Web/PushNotifications/Scalability/PushNotificationSynchronizerTask.cs' },
      { name: 'AddSignalR().AddPushNotifications()', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Send a notification to the user who started the work.\n' +
'await _pushNotificationManager.SendAsync(new MyImportNotification(_userNameResolver.GetCurrentUserName())\n' +
'{\n' +
'    Title = "Import products",\n' +
'    Description = "Processed 500 of 2000 rows"\n' +
'});\n' +
'\n' +
'// Scale-out wiring, from Startup.cs — the backplane is what makes this work on N instances:\n' +
'// services.AddSignalR().AddPushNotifications(Configuration);'
    },
    gotchas: [
      'Scaled out without a Redis or Azure SignalR backplane, notifications reach only the instance that sent them — so progress appears to stall at random for half your users.',
      'Notifications are keyed to a user. Getting the current user wrong means the update goes to nobody visible.',
      'The stored history is what the UI shows after a reload; a notification that was only pushed live effectively disappears.'
    ],
    docs: [
      { label: 'Scale out on Azure', page: 'Fundamentals/Scalability/scaling-configuration-on-azure-cloud' }
    ],
    seeAlso: ['job-progress', 'redis-cache-bus', 'current-user', 'hosted-service'],
    molecule: 'observability',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'eventbus-webhooks',
    symbol: 'Wh',
    name: 'EventBus & WebHooks',
    family: 'messaging',
    adoption: 'module',
    layer: 'integration',
    tags: ['azure service bus', 'rabbitmq', 'kafka', 'callback', 'external', 'integration'],
    oneLiner: 'Getting events out of the process: to a message broker, or as an HTTP callback to someone else\'s system.',
    pattern: 'Bridge from in-process domain events to external transports. The EventBus module forwards selected domain events to a broker; the WebHooks module turns them into configurable outbound HTTP calls. Both are installable modules, not platform core.',
    whenToUse: [
      'Letting an external system react to commerce changes without polling your API',
      'Integration middleware consuming a stream of changes rather than diffing snapshots',
      'Partner or customer systems that want callbacks'
    ],
    avoid: [
      'Assuming these exist in a bare platform — they must be installed',
      'Sending sensitive payloads to a webhook endpoint you do not control',
      'Treating outbound delivery as guaranteed and ordered. It is neither'
    ],
    api: [
      { name: 'VirtoCommerce.EventBus module', file: '(separate repository — vc-module-event-bus)' },
      { name: 'VirtoCommerce.WebHooks module', file: '(separate repository — vc-module-webhooks)' },
      { name: 'IEventPublisher (the in-process source)', file: 'src/VirtoCommerce.Platform.Core/Events/IEventPublisher.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Nothing special is required of the publisher: a normal domain event is what\n' +
'// EventBus and WebHooks subscribe to. Publish once, and both can forward it.\n' +
'await _eventPublisher.Publish(new MyEntityChangedEvent(changes));\n' +
'\n' +
'// Which events go out, and where, is configuration in the respective module —\n' +
'// not a code change in yours. That is the point of the split.'
    },
    gotchas: [
      'Outbound delivery is at-least-once at best. Receivers must be idempotent, and so must any loop that comes back into the platform.',
      'A slow or failing webhook endpoint should not slow your write path — check how the installed module handles retries before relying on it.',
      'Forwarding a high-volume event (every price change, say) can generate far more traffic than anyone expected. Filter deliberately.'
    ],
    docs: [
      { label: 'Extending using events', page: 'Fundamentals/Event-Driven-Development/using-domain-events' }
    ],
    seeAlso: ['domain-events', 'module-catalog'],
    verifiedAgainst: '3.1053.0'
  },

  // ================================================================ DATA & DOMAIN

  {
    id: 'abstract-type-factory',
    symbol: 'Af',
    name: 'AbstractTypeFactory',
    family: 'data',
    adoption: 'platform',
    layer: 'platform',
    keystone: true,
    tags: ['override', 'extend', 'polymorphism', 'trycreateinstance', 'keystone'],
    oneLiner: 'The keystone of the extension model: replace any platform or vendor type with your own derived type.',
    pattern: 'Type registry with factory creation. Every extensible type is constructed through `AbstractTypeFactory<T>.TryCreateInstance()` rather than `new`. Register an override once at startup and every construction site — including inside vendor modules you cannot edit — produces your derived type instead.',
    whenToUse: [
      'Adding a property or behaviour to a vendor domain type, when a dynamic property is not enough',
      'Replacing a vendor implementation with your own subclass',
      'Making your own types extensible for whoever builds on your module — always construct through the factory'
    ],
    avoid: [
      '`new` for any extensible domain type. It works, and it silently breaks everyone\'s overrides',
      'Overriding a type in two places; the last registration wins and the conflict is invisible',
      'Reaching for this before checking whether a dynamic property or an event handler solves the problem more cheaply'
    ],
    api: [
      { name: 'AbstractTypeFactory<T>.TryCreateInstance', file: 'src/VirtoCommerce.Platform.Core/Domain/AbstractTypeFactory.cs' },
      { name: 'AbstractTypeFactory<T>.OverrideType<Old, New>', file: 'src/VirtoCommerce.Platform.Core/Domain/AbstractTypeFactory.cs' },
      { name: 'AbstractTypeFactory<T>.RegisterType<T>', file: 'src/VirtoCommerce.Platform.Core/Domain/AbstractTypeFactory.cs' },
      { name: 'IFactory<T>', file: 'src/VirtoCommerce.Platform.Core/Domain/IFactory.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// 1. Derive. Prefix new members so a future vendor property cannot collide.\n' +
'public class AbcCustomerOrder : CustomerOrder\n' +
'{\n' +
'    public string AbcExternalOrderId { get; set; }\n' +
'}\n' +
'\n' +
'// 2. Register the override once, in Module.Initialize.\n' +
'AbstractTypeFactory<CustomerOrder>.OverrideType<CustomerOrder, AbcCustomerOrder>();\n' +
'\n' +
'// 3. Vendor code that never heard of you now builds your type:\n' +
'var order = AbstractTypeFactory<CustomerOrder>.TryCreateInstance();   // → AbcCustomerOrder\n' +
'\n' +
'// Which is why your own extensible types must also be created this way:\n' +
'var mine = AbstractTypeFactory<MyEntity>.TryCreateInstance();         // not: new MyEntity()'
    },
    gotchas: [
      'Register overrides in `Initialize`, before anything constructs the type. An override registered too late applies only to later constructions.',
      'The EF Core entity and the domain model are separate types. Overriding the domain model does not persist your new property — you also need the entity, the mapping and a migration.',
      'Two modules overriding the same type is a genuine conflict with no error: the load order decides, and load order is not something you control.',
      'Serialization sees the derived type, so a polymorphic payload can arrive as a base type unless the factory registration is present on both ends.'
    ],
    docs: [
      { label: 'Extending domain models', page: 'Tutorials-and-How-tos/Tutorials/extending-domain-models' },
      { label: 'Extensibility overview', page: 'Extensibility/overview' },
      { label: 'Extend the DB model', page: 'Tutorials-and-How-tos/Tutorials/extending-database-model' }
    ],
    seeAlso: ['dynamic-properties', 'ef-core', 'dependency-injection', 'module-lifecycle', 'json-serialization'],
    molecule: 'extensibility-decision-tree',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'generic-crud',
    symbol: 'Cs',
    name: 'Generic CRUD',
    family: 'data',
    adoption: 'platform',
    layer: 'platform',
    tags: ['crudservice', 'searchservice', 'icrudservice', 'isearchservice', 'responsegroup', 'template', 'vc-crud', 'scaffold'],
    oneLiner: 'Two abstract base classes with one abstract method each — you supply the query, not the plumbing.',
    pattern: 'Template Method over repository plus cache plus events. `CrudService` and `SearchService` implement the whole read/write path — batched get, save, delete, response groups, cloning, cache regions, changing and changed events — and leave exactly one hole each for the part only your module knows.',
    whenToUse: [
      '**Every entity your module owns.** Inheriting is the default; writing a service from scratch is the thing to justify',
      'When you want the standard behaviour for free: `IPlatformMemoryCache` regions wired up, `GenericChangedEntryEvent` published before and after a save, soft delete, response groups',
      'When you want your service replaceable — the base implements the `ICrudService<TModel>` / `ISearchService<...>` interfaces that other modules and XAPI resolve',
      '**Starting a new entity: let the CLI write it.** `dotnet new vc-crud` emits the whole set and you edit rather than assemble'
    ],
    avoid: [
      'Writing `GetAsync` / `SaveChangesAsync` / `DeleteAsync` yourself. If you are, you have re-implemented the base class and you now own its caching and event bugs',
      'Putting query logic in `LoadEntities` — that one loads by id. Criteria belong in `BuildQuery` on the search service',
      'Skipping the events. Other modules and integrations subscribe to `*ChangedEvent`; a hand-rolled save that does not publish is invisible to them',
      'Injecting `IRepository` directly instead of `Func<IRepository>` — the service is longer-lived than a request, which is exactly why the base takes a factory'
    ],
    api: [
      { name: 'CrudService<TModel, TEntity, TChangingEvent, TChangedEvent> — one abstract member: LoadEntities', file: 'src/VirtoCommerce.Platform.Data/GenericCrud/CrudService.cs' },
      { name: 'SearchService<TCriteria, TResult, TModel, TEntity> — one abstract member: BuildQuery', file: 'src/VirtoCommerce.Platform.Data/GenericCrud/SearchService.cs' },
      { name: 'ICrudService<TModel>', file: 'src/VirtoCommerce.Platform.Core/GenericCrud/ICrudService.cs' },
      { name: 'ISearchService<TCriteria, TResult, TModel>', file: 'src/VirtoCommerce.Platform.Core/GenericCrud/ISearchService.cs' },
      { name: 'CrudOptions', file: 'src/VirtoCommerce.Platform.Core/GenericCrud/CrudOptions.cs' },
      { name: 'CrudServiceExtensions / SearchServiceExtensions — the single-item helpers', file: 'src/VirtoCommerce.Platform.Core/Extensions/CrudServiceExtensions.cs' },
      { name: 'vc-crud — "Virto Commerce 3.x CRUD Template", shortName vc-crud', file: '(vc-cli-module-template/templates/vc-crud-template)' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Two base classes, one abstract method each. Everything else is inherited:\n' +
'// batched Get, Save, Delete, response groups, cloning, cache regions and the\n' +
'// changing/changed events - none of which you write.\n' +
'\n' +
'public class AbcThingService(\n' +
'        Func<IRepository> repositoryFactory,        // a factory: the service outlives a request\n' +
'        IPlatformMemoryCache cache,\n' +
'        IEventPublisher eventPublisher)\n' +
'    : CrudService<AbcThing, AbcThingEntity, AbcThingChangingEvent, AbcThingChangedEvent>(\n' +
'        repositoryFactory, cache, eventPublisher), IAbcThingService\n' +
'{\n' +
'    // The one thing the base class cannot know: how to load your entities.\n' +
'    protected override Task<IList<AbcThingEntity>> LoadEntities(\n' +
'        IRepository repository, IList<string> ids, string responseGroup)\n' +
'        => ((IAbcRepository)repository).GetThingsByIdsAsync(ids, responseGroup);\n' +
'}\n' +
'\n' +
'public class AbcThingSearchService(/* ... same three, plus your options ... */)\n' +
'    : SearchService<AbcThingSearchCriteria, AbcThingSearchResult, AbcThing, AbcThingEntity>(\n' +
'        /* ... */), IAbcThingSearchService\n' +
'{\n' +
'    // And here: how to turn criteria into a query. Paging and sorting are inherited.\n' +
'    protected override IQueryable<AbcThingEntity> BuildQuery(\n' +
'        IRepository repository, AbcThingSearchCriteria criteria)\n' +
'    {\n' +
'        var query = ((IAbcRepository)repository).Things;\n' +
'        if (!string.IsNullOrEmpty(criteria.Keyword))\n' +
'            query = query.Where(x => x.Name.Contains(criteria.Keyword));\n' +
'        return query;\n' +
'    }\n' +
'}\n' +
'\n' +
'// Or do not write any of it. The CLI template emits the whole set:\n' +
'//\n' +
'//   dotnet new vc-crud -n MyCompany.Catalog --EntityName Thing\n' +
'//\n' +
'// model, search criteria and result, changing/changed events, the EF entity,\n' +
'// DbContext and repository, both services, an API controller and Module.cs.'
    },
    gotchas: [
      '**`LoadEntities` is the only abstract member on `CrudService`** and `BuildQuery` the only one on `SearchService`. If a code review shows you overriding much more than that, the question is why — usually the answer is a `responseGroup` that should have been handled in the loader.',
      'The base constructor takes `Func<IRepository>`, not `IRepository`. That is deliberate: the service is registered longer-lived than a request, so it opens a scope per call — see [[dependency-injection]].',
      'Saves publish a **changing** event before and a **changed** event after. The changing one can be cancelled, which means a save can be refused by a handler in another module — read the event, not just the return value.',
      '`clone: true` is the default on reads, and it is what stops a caller mutating the cached instance. Passing `false` is a performance choice with a correctness price; know which one you are making.',
      'Cache invalidation is by region, so a save evicts more than the one entity. That is the trade the base class makes for you — see [[cache-regions]].',
      'The `vc-crud` template takes an `EntityName` symbol and generates against it, so pick the entity name before you run it; renaming afterwards means touching every generated file.'
    ],
    docs: [
      { label: 'Creating a custom module (DB-agnostic)', page: 'Fundamentals/Persistence/DB-Agnostic/creating-custom-module' },
      { label: 'Create a module from scratch', page: 'Tutorials-and-How-tos/Tutorials/create-new-module-from-scratch' },
      { label: 'vc-cli-module-template (GitHub)', href: 'https://github.com/VirtoCommerce/vc-cli-module-template' },
      { label: 'vc-crud template source', href: 'https://github.com/VirtoCommerce/vc-cli-module-template/tree/main/templates/vc-crud-template' }
    ],
    seeAlso: ['repository-uow', 'ef-core', 'domain-events', 'cache-regions', 'dependency-injection', 'specifications'],
    molecule: 'ecommerce-modules',
    verifiedAgainst: '3.1059.0'
  },
  {
    id: 'repository-uow',
    symbol: 'Rp',
    name: 'Repository & unit of work',
    family: 'data',
    adoption: 'platform',
    layer: 'platform',
    tags: ['irepository', 'iunitofwork', 'transaction', 'commit', 'ef'],
    oneLiner: 'The data-access boundary: a repository per module, one commit per unit of work.',
    pattern: 'Repository over a `DbContext`, with an explicit unit of work. `IRepository` exposes `Add` / `Update` / `Remove` / `Attach` and a `UnitOfWork` whose `CommitAsync()` is the single place a transaction lands. Services take a `Func<IRepository>` factory rather than a repository, so each operation gets a fresh context.',
    whenToUse: [
      'All data access inside a module — one repository interface per module',
      'When several changes must commit together',
      'Read-only queries, with change tracking disabled for speed'
    ],
    avoid: [
      'Injecting a repository directly instead of a factory; a long-lived `DbContext` accumulates tracked entities and leaks memory',
      'Committing inside a loop. Batch and commit once',
      'Leaking `IQueryable` out of the repository — the context is disposed before the query runs, and you get a confusing failure far from the cause'
    ],
    api: [
      { name: 'IRepository', file: 'src/VirtoCommerce.Platform.Core/Domain/IRepository.cs' },
      { name: 'IUnitOfWork', file: 'src/VirtoCommerce.Platform.Core/Domain/IUnitOfWork.cs' },
      { name: 'Entity / AuditableEntity', file: 'src/VirtoCommerce.Platform.Core/Domain/AuditableEntity.cs' },
      { name: 'PrimaryKeyResolvingMap', file: 'src/VirtoCommerce.Platform.Core/Common/PrimaryKeyResolvingMap.cs' },
      { name: 'IDataEntity', file: 'src/VirtoCommerce.Platform.Core/Domain/IDataEntity.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// A factory, not a repository — each call gets a fresh, short-lived context.\n' +
'public class MyService(Func<IMyRepository> repositoryFactory)\n' +
'{\n' +
'    public async Task SaveChangesAsync(IList<MyEntity> models)\n' +
'    {\n' +
'        using var repository = repositoryFactory();\n' +
'\n' +
'        var pkMap = new PrimaryKeyResolvingMap();\n' +
'        foreach (var model in models)\n' +
'        {\n' +
'            var entity = AbstractTypeFactory<MyEntityEntity>.TryCreateInstance().FromModel(model, pkMap);\n' +
'            repository.Add(entity);\n' +
'        }\n' +
'\n' +
'        await repository.UnitOfWork.CommitAsync();   // one commit, one transaction\n' +
'        pkMap.ResolvePrimaryKeys();                  // new ids flow back into the models\n' +
'    }\n' +
'}'
    },
    gotchas: [
      '`PrimaryKeyResolvingMap` is how database-generated ids get back onto your domain models after a commit. Skip it and new entities look like they have no id.',
      'Read paths should call `DisableChangesTracking()`. Tracking thousands of entities you never modify is pure overhead.',
      'A `Func<IRepository>` looks like ceremony until you hit a memory leak from a repository injected into a singleton.',
      'The domain model and the EF entity are deliberately different types, mapped by `ToModel` / `FromModel`. Merging them looks tempting and defeats extensibility.'
    ],
    docs: [
      { label: 'Extend the DB model', page: 'Tutorials-and-How-tos/Tutorials/extending-database-model' },
      { label: 'Database agnostic', page: 'Fundamentals/Persistence/DB-Agnostic/overview' }
    ],
    seeAlso: ['ef-core', 'generic-crud', 'abstract-type-factory', 'change-log'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'ef-core',
    symbol: 'Ef',
    name: 'EF Core & migrations',
    family: 'data',
    adoption: 'platform',
    layer: 'platform',
    tags: ['entity framework', 'migration', 'sqlserver', 'postgresql', 'mysql', 'dbcontext'],
    oneLiner: 'EF Core 10 across three database providers — which is why a migration is never written just once.',
    pattern: 'Provider-agnostic core plus per-provider projects. `Platform.Data` holds the context and entity configuration; `Platform.Data.SqlServer`, `.PostgreSql` and `.MySql` each carry provider-specific migrations. A module follows the same split for its own schema.',
    whenToUse: [
      'Any persistent schema a module owns',
      'Adding a column to support a type you overrode via `AbstractTypeFactory`',
      'Query work where you need real SQL translation rather than in-memory filtering'
    ],
    avoid: [
      'Provider-specific SQL or types in shared code — it compiles and then breaks on somebody else\'s database',
      'Generating a migration for one provider and assuming the others follow',
      'Client-side evaluation by accident: an unsupported expression silently pulls the table into memory'
    ],
    api: [
      { name: 'Platform DbContext', file: 'src/VirtoCommerce.Platform.Data/Repositories/PlatformDbContext.cs' },
      { name: 'SQL Server migrations', file: 'src/VirtoCommerce.Platform.Data.SqlServer/' },
      { name: 'PostgreSQL migrations', file: 'src/VirtoCommerce.Platform.Data.PostgreSql/' },
      { name: 'MySQL migrations', file: 'src/VirtoCommerce.Platform.Data.MySql/' },
      { name: 'MigrationName', file: 'src/VirtoCommerce.Platform.Core/Common/MigrationName.cs' }
    ],
    snippet: {
      lang: 'bash',
      code:
'# One migration per provider. All three, or the module only works on one database.\n' +
'dotnet ef migrations add AddAbcExternalOrderId \\\n' +
'  --project src/MyModule.Data.SqlServer \\\n' +
'  --startup-project src/MyModule.Web \\\n' +
'  --context MyModuleDbContext\n' +
'\n' +
'# Then repeat for .PostgreSql and .MySql. Diff the generated SQL between them —\n' +
'# differences are where provider-specific assumptions leaked into your model.'
    },
    gotchas: [
      'Three providers means three migration sets. Forgetting one turns into a runtime failure on a customer\'s database, not a build error.',
      'The platform pins EF Core 10 and builds with `TreatWarningsAsErrors` — an EF analyzer warning will fail your build.',
      'Entity configuration lives with the entity, not the context, so a module can extend the schema without touching platform code.',
      '`EntityFrameworkCore.Triggers` is in use, so some behaviour happens at save time that is not visible in your service code.'
    ],
    docs: [
      { label: 'Database agnostic', page: 'Fundamentals/Persistence/DB-Agnostic/overview' },
      { label: 'Extend the DB model', page: 'Tutorials-and-How-tos/Tutorials/extending-database-model' }
    ],
    seeAlso: ['repository-uow', 'abstract-type-factory', 'dynamic-properties', 'change-log'],
    molecule: 'dev-process',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'change-log',
    symbol: 'Cl',
    name: 'Change log & audit',
    family: 'data',
    adoption: 'platform',
    layer: 'platform',
    tags: ['audit', 'operationlog', 'history', 'lastmodified', 'trail'],
    oneLiner: 'Who changed what, when — recorded automatically for entities that opt in.',
    pattern: 'Operation log written on save. Entities implementing `IHasChangesHistory` get `OperationLog` records; `IChangeLogSearchService` queries them, and `ILastChangesService` answers "what changed since" without scanning history.',
    whenToUse: [
      'Entities with compliance or dispute-resolution requirements: orders, prices, customer data',
      'Answering "who changed this and when" without adding your own audit columns',
      'Incremental sync — "what changed since this timestamp" for an integration'
    ],
    avoid: [
      'Enabling it on high-write entities without thinking about volume; the log grows faster than the data',
      'Using it as an event stream. It is a record for humans, not a delivery mechanism',
      'Relying on it for field-level diffs unless you have checked what is actually captured'
    ],
    api: [
      { name: 'IChangeLogService', file: 'src/VirtoCommerce.Platform.Core/ChangeLog/IChangeLogService.cs' },
      { name: 'IChangeLogSearchService', file: 'src/VirtoCommerce.Platform.Core/ChangeLog/IChangeLogSearchService.cs' },
      { name: 'IHasChangesHistory', file: 'src/VirtoCommerce.Platform.Core/ChangeLog/IHasChangesHistory.cs' },
      { name: 'OperationLog', file: 'src/VirtoCommerce.Platform.Core/ChangeLog/OperationLog.cs' },
      { name: 'ILastChangesService', file: 'src/VirtoCommerce.Platform.Core/ChangeLog/ILastChangesService.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Query the audit trail for one object.\n' +
'var result = await _changeLogSearchService.SearchAsync(new ChangeLogSearchCriteria\n' +
'{\n' +
'    ObjectIds = [orderId],\n' +
'    ObjectTypes = [nameof(CustomerOrder)],\n' +
'    Take = 50\n' +
'});\n' +
'\n' +
'// AuditableEntity already carries CreatedBy/CreatedDate/ModifiedBy/ModifiedDate —\n' +
'// the change log is the history on top of that current-state snapshot.'
    },
    gotchas: [
      'The operation log table is one of the fastest-growing in a busy deployment. Retention is an operational decision somebody has to make.',
      '`AuditableEntity` fields tell you the last change; the change log tells you all of them. Reaching for the wrong one gives a confidently wrong answer.',
      'Audit fields are populated from the resolved current user, so work in a background job records whoever the job thinks it is running as.'
    ],
    docs: [],
    seeAlso: ['repository-uow', 'current-user', 'domain-events'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'specifications',
    symbol: 'Sp',
    name: 'Specifications',
    family: 'data',
    adoption: 'platform',
    layer: 'platform',
    tags: ['ispecification', 'predicate', 'rules', 'issatisfiedby', 'composable'],
    oneLiner: 'A one-method interface for a reusable, composable business rule.',
    pattern: 'Specification pattern, deliberately minimal: `ISpecification<T>` is a single `bool IsSatisfiedBy(T obj)`. Rules become objects you can name, test in isolation and combine, instead of predicates duplicated across services.',
    whenToUse: [
      'A business rule stated in more than one place — eligibility, applicability, validity',
      'Rules a module should be able to replace, by registering a different specification',
      'Anything you want to unit test without constructing the service that uses it'
    ],
    avoid: [
      'Wrapping a one-line predicate used once; a lambda is clearer',
      'Expecting it to translate to SQL. This evaluates in memory, unlike an EF predicate',
      'Building deep composition trees nobody can debug'
    ],
    api: [
      { name: 'ISpecification<T>', file: 'src/VirtoCommerce.Platform.Core/Specifications/ISpecification.cs' },
      { name: 'PredicateBuilder', file: 'src/VirtoCommerce.Platform.Core/Common/PredicateBuilder.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// The entire contract, from Core/Specifications/ISpecification.cs:\n' +
'public interface ISpecification<in T>\n' +
'{\n' +
'    bool IsSatisfiedBy(T obj);\n' +
'}\n' +
'\n' +
'// A named, testable, replaceable rule:\n' +
'public class OrderIsRefundable : ISpecification<CustomerOrder>\n' +
'{\n' +
'    public bool IsSatisfiedBy(CustomerOrder order) =>\n' +
'        order.Status == "Completed" && order.CreatedDate > DateTime.UtcNow.AddDays(-30);\n' +
'}\n' +
'\n' +
'// For database-side filtering use PredicateBuilder with IQueryable instead —\n' +
'// specifications run in memory.'
    },
    gotchas: [
      'In-memory evaluation only. Applying a specification to a large table means loading the table.',
      'For composable *query* predicates use `PredicateBuilder`, which builds expression trees EF can translate.',
      'Registering a specification in DI makes it overridable by a solution module — which is usually the reason to write one.'
    ],
    docs: [],
    seeAlso: ['validation', 'dependency-injection', 'ef-core'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'validation',
    symbol: 'Vl',
    name: 'Validation',
    family: 'data',
    adoption: 'platform',
    layer: 'platform',
    tags: ['fluentvalidation', 'abstractvalidator', 'validator', 'rules', 'modelstate'],
    oneLiner: 'FluentValidation for domain and input rules — declared as a validator class, not scattered through services.',
    pattern: 'Validator per type. Derive from `AbstractValidator<T>`, declare rules in the constructor, register in DI, and validate before persisting. The platform validates dynamic property types and setting entries exactly this way.',
    whenToUse: [
      'Validating a domain model before save, with messages an operator will read',
      'Complex or conditional rules where data annotations run out',
      'Rules a solution module should be able to extend or replace'
    ],
    avoid: [
      'Duplicating validation in the controller and the service; pick the layer that owns the invariant',
      'Database lookups inside a validator without care — validation runs on every save',
      'Relying on validation for security. Authorization is a separate concern'
    ],
    api: [
      { name: 'AbstractValidator<T> (FluentValidation 12.1)', file: 'src/VirtoCommerce.Platform.Data/Validators/ObjectSettingEntryValidator.cs' },
      { name: 'DynamicPropertyTypeValidator', file: 'src/VirtoCommerce.Platform.Data/Validators/DynamicPropertyTypeValidator.cs' },
      { name: 'Validator registration', file: 'src/VirtoCommerce.Platform.Data/DynamicProperties/ServiceCollectionExtenions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Module.Data/Validators — see ObjectSettingEntryValidator for the platform\'s own example.\n' +
'public class MyEntityValidator : AbstractValidator<MyEntity>\n' +
'{\n' +
'    public MyEntityValidator()\n' +
'    {\n' +
'        RuleFor(x => x.Name).NotEmpty().MaximumLength(128);\n' +
'        RuleFor(x => x.Code).NotEmpty().Matches("^[A-Z0-9-]+$")\n' +
'            .WithMessage("Code may contain only capitals, digits and hyphens");\n' +
'    }\n' +
'}\n' +
'\n' +
'// Validate before persisting, and fail with the collected messages.\n' +
'await _validator.ValidateAndThrowAsync(entity);'
    },
    gotchas: [
      'A validator registered but never invoked is dead code that looks like a safeguard. Make sure something actually calls it.',
      'Validation messages surface in the Admin UI, so they should be localizable and written for the operator rather than the developer.',
      'FluentValidation 12 tightened some behaviour versus older majors; do not copy rule syntax from ancient examples without checking.'
    ],
    docs: [],
    seeAlso: ['specifications', 'generic-crud', 'localizations'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'json-serialization',
    symbol: 'Js',
    name: 'JSON serialization',
    family: 'data',
    adoption: 'platform',
    layer: 'api-edge',
    tags: ['newtonsoft', 'system.text.json', 'converter', 'polymorphic', 'mvc'],
    oneLiner: 'The platform\'s REST API serializes with Newtonsoft, not System.Text.Json. Custom converters must match.',
    pattern: 'Newtonsoft-based MVC serialization with polymorphic type support. `AddNewtonsoftJson` is configured in `Startup`, partly so derived types registered through `AbstractTypeFactory` are represented correctly in Swagger and on the wire.',
    whenToUse: [
      'Writing a converter for a type crossing the REST boundary — use Newtonsoft\'s `JsonConverter`',
      'Exposing a polymorphic type, where the concrete type must survive the round trip',
      'Controlling how a domain type appears in the API without changing the domain type'
    ],
    avoid: [
      'Assuming `System.Text.Json` attributes apply to REST responses. They do not — MVC is on Newtonsoft',
      'Two competing converters for one type; the one that wins depends on registration order',
      'Serializing EF entities directly. Serialize the domain model'
    ],
    api: [
      { name: 'AddNewtonsoftJson configuration', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' },
      { name: 'Platform JSON converters', file: 'src/VirtoCommerce.Platform.Core/JsonConverters/' },
      { name: 'DynamicPropertyAccessorJsonConverter', file: 'src/VirtoCommerce.Platform.Core/DynamicProperties/DynamicPropertyAccessorJsonConverter.cs' },
      { name: 'ExportImport JsonSerializerExtensions', file: 'src/VirtoCommerce.Platform.Core/ExportImport/JsonSerializerExtensions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Startup.cs configures MVC with Newtonsoft — note the comment in the source:\n' +
'// "Next line needs to represent custom derived types in the resulting swagger doc\n' +
'//  definitions. Because default SwaggerProvider used global JSON serialization settings"\n' +
'services.AddControllers().AddNewtonsoftJson(options => { /* ... */ });\n' +
'\n' +
'// So a converter for the REST boundary is a Newtonsoft one:\n' +
'public class MyTypeConverter : Newtonsoft.Json.JsonConverter   // not System.Text.Json\n' +
'{\n' +
'    // ...\n' +
'}'
    },
    gotchas: [
      'This trips up developers arriving from modern ASP.NET Core, where `System.Text.Json` is the default. Here it is not, and attributes from the wrong library are silently ignored.',
      'Swagger schema generation reads the same global serializer settings, so a serialization change can quietly alter your published API contract.',
      'Types created through `AbstractTypeFactory` need the override registered on both sides of a round trip, or your derived properties are dropped on deserialization.'
    ],
    docs: [
      { label: 'Polymorphic types in Swagger', page: 'Tutorials-and-How-tos/How-tos/type-inheritance-support-in-swagger' }
    ],
    seeAlso: ['abstract-type-factory', 'swagger', 'backup-restore'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'search',
    symbol: 'Se',
    name: 'Indexed search',
    family: 'data',
    adoption: 'module',
    layer: 'infrastructure',
    tags: ['elasticsearch', 'lucene', 'index', 'catalog', 'facet', 'aggregation', 'opensearch'],
    oneLiner: 'The read path the catalogue actually depends on — and one more engine to keep consistent with the database.',
    pattern: 'Port and adapter over an index, with documents built by contributors. `VirtoCommerce.Search` owns `ISearchProvider` and `IIndexingManager`; an engine module implements the port — Elasticsearch 8/9, OpenSearch, Lucene, Azure Search, Elastic App Search or Algolia. Modules contribute `IIndexDocumentBuilder`s, so one document is assembled from several sources rather than owned by one.',
    whenToUse: [
      'Catalogue browse, search and faceting — at any real catalogue size this is the only viable read path',
      'Filtering or aggregating over fields that would be expensive in SQL',
      'Adding your own field to an existing document, by contributing a builder instead of forking the owning module'
    ],
    avoid: [
      'Treating the index as the source of truth. It is a projection, and it can be rebuilt',
      'Reading from the index immediately after a write and expecting your change — indexing is asynchronous',
      'Lucene in a scaled-out deployment; it is a local index, so every instance would hold its own copy',
      'Reindexing everything on every change when `IndexChangesAsync` exists'
    ],
    api: [
      { name: 'ISearchProvider — SearchAsync / IndexAsync / RemoveAsync / DeleteIndexAsync', file: '(vc-module-search — VirtoCommerce.SearchModule.Core.Services)' },
      { name: 'IIndexingManager — IndexAllDocumentsAsync / IndexChangesAsync / GetIndexStateAsync', file: '(vc-module-search — orchestrates full and incremental indexing)' },
      { name: 'IIndexDocumentBuilder', file: '(vc-module-search — how a module contributes fields to a document)' },
      { name: 'IIndexingJobService', file: '(vc-module-search — scheduled and on-demand indexing)' },
      { name: 'VirtoCommerce.Search + ElasticSearch8 | ElasticSearch9 | LuceneSearch | AzureSearch | OpenSearch', file: '(module ids from the vc-modules registry)' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Contribute fields to a document type instead of forking the module that owns it.\n' +
'public class MyProductFieldsBuilder(IMyService service) : IIndexDocumentBuilder\n' +
'{\n' +
'    public async Task<IList<IndexDocument>> GetDocumentsAsync(IList<string> documentIds)\n' +
'    {\n' +
'        var extras = await service.GetAsync(documentIds);\n' +
'\n' +
'        return extras.Select(x =>\n' +
'        {\n' +
'            var doc = new IndexDocument(x.ProductId);\n' +
'            doc.AddFilterableString("abcVendorCode", x.VendorCode);\n' +
'            return doc;\n' +
'        }).ToList();\n' +
'    }\n' +
'}\n' +
'\n' +
'// Incremental beats full: index what changed, not the whole catalogue.\n' +
'await _indexingManager.IndexChangesAsync(options, progress => { }, cancellationToken);'
    },
    note: 'Search is a module, not platform core, and the engine behind it is a further module chosen per deployment — the same port-and-adapter shape as background-job engines: one active provider, selected in configuration.',
    gotchas: [
      'There is a consistency window between a write and the index reflecting it. Every "the change is saved but the list still shows the old value" report starts here.',
      'A document is assembled from every registered builder for its type, so one slow builder slows indexing for everyone contributing to that document.',
      'Engine versions are separate modules (`ElasticSearch8` vs `ElasticSearch9`) — check which matches your server before installing.',
      '`IIndexingManager.IndexDocumentsAsync` has `[Obsolete]` overloads without a `CancellationToken` (VC0014); use the cancellation-aware ones.'
    ],
    docs: [
      { label: 'Indexed search overview', page: 'Fundamentals/Indexed-Search/overview' },
      { label: 'Indexing overview', page: 'Fundamentals/Indexed-Search/indexing/overview' },
      { label: 'Blue-green indexing', page: 'Fundamentals/Indexed-Search/indexing/blue-green-indexing' },
      { label: 'Configuring Elasticsearch', page: 'Fundamentals/Indexed-Search/integration/configuring-elasticsearch' },
      { label: 'vc-module-search (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-search' }
    ],
    seeAlso: ['assets', 'ef-core', 'background-jobs', 'domain-events'],
    molecule: 'search-and-indexing',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'module-database',
    symbol: 'Db',
    name: 'Database per module',
    family: 'data',
    adoption: 'platform',
    layer: 'modules',
    tags: ['connectionstrings', 'dbcontext', 'migrations', 'composability', 'decomposition', 'sharding'],
    oneLiner: 'Every module owns its schema, and can own its physical database on one line of configuration.',
    pattern: 'Database per service, applied at the module boundary. Each module registers its own `DbContext` and resolves its connection string by **module id** first, falling back to the shared one — so moving a module to its own server is configuration, not code.',
    whenToUse: [
      'A hot table set that needs its own compute or its own backup schedule — orders, or the catalog',
      'A compliance boundary: identity on a separate server is the platform\'s own precedent (`Auth:ConnectionString`)',
      'Scaling one module\'s storage independently of the rest',
      'Answering "is it really separated?" — the schema always is; the server is a config line'
    ],
    avoid: [
      'Anything needing one transaction across two modules — there is no distributed transaction, and none is coming',
      'Splitting before you have a measured reason. One database is simpler to run, back up and restore',
      'Assuming a split module is now a service — it still loads in the same host process'
    ],
    api: [
      { name: 'Auth:ConnectionString — the platform\'s own per-context override', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' },
      { name: 'ConnectionStrings:VirtoCommerce — the shared fallback', file: 'src/VirtoCommerce.Platform.Web/appsettings.json' },
      { name: 'GetConnectionString(ModuleInfo.Id) ?? GetConnectionString("VirtoCommerce")', file: '(module-side pattern — Module.cs in every vc-module-* repo and in vc-cli-module-template)' }
    ],
    snippet: {
      lang: 'json',
      code: '// appsettings.json — the shipped file shows only the shared entry, but every module\n// looks for its own module id FIRST. Adding a key is the whole migration.\n{\n  "ConnectionStrings": {\n    "VirtoCommerce": "Data Source=sql-main;Initial Catalog=VirtoCommerce3;...",\n\n    // Pricing now lives on its own server. No code change, no redeploy of other modules.\n    "VirtoCommerce.Pricing": "Data Source=sql-pricing;Initial Catalog=Pricing;...",\n\n    // Orders too — its own instance, its own backup window.\n    "VirtoCommerce.Orders": "Data Source=sql-orders;Initial Catalog=Orders;..."\n  },\n\n  // Identity is the platform\'s own example of the same idea, under a different key.\n  "Auth": {\n    "ConnectionString": "Data Source=sql-identity;Initial Catalog=Security;..."\n  }\n}'
    },
    gotchas: [
      'The key is the **module id** from `module.manifest` — `VirtoCommerce.Pricing`, not `Pricing` and not the assembly name.',
      'It works only because modules never hold a foreign key across the boundary. Read [[cross-module-references]] before you split anything.',
      'Each module migrates its own context at startup, inside the platform\'s startup distributed lock. Without Redis that lock degrades to a no-op, so two instances starting at once can both migrate.',
      'A per-module database is invisible in the shipped `appsettings.json`, which is the main reason teams believe it does not exist.',
      'Backup and restore become per-module: a point-in-time restore of one database leaves the others ahead of it.'
    ],
    docs: [
      { label: 'Configuring environments', page: 'Tutorials-and-How-tos/How-tos/configuring-environments' },
      { label: 'appsettings.json reference', page: 'Configuration-Reference/appsettingsjson' },
      { label: 'DB-agnostic persistence', page: 'Fundamentals/Persistence/DB-Agnostic/overview' }
    ],
    seeAlso: ['cross-module-references', 'ef-core', 'module-manifest', 'host-composition', 'distributed-lock'],
    molecule: 'deployment',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'cross-module-references',
    symbol: 'Xr',
    name: 'Ids, not foreign keys',
    family: 'data',
    adoption: 'platform',
    layer: 'modules',
    tags: ['boundary', 'denormalization', 'snapshot', 'foreign key', 'composability', 'pricing'],
    oneLiner: 'A module references another module\'s data by id and copies what it needs — which is what makes separate databases possible.',
    pattern: 'Bounded context with denormalised boundary data. Across a module boundary you keep an **id** for traceability and a **copy** of the values you must not lose. No navigation property, no foreign key, no join.',
    whenToUse: [
      'Any time your entity needs data owned by another module',
      'Recording *what was true at the time* — a price, a name, a tax rate on an order',
      'Making a module independently deployable and independently storable',
      'Reporting that must not change when the source record changes'
    ],
    avoid: [
      'A foreign key to another module\'s table. It compiles, it even works in one database, and it makes the boundary a lie',
      'Re-reading the source module to display a historical document — that is how an invoice silently changes',
      'Copying data that is not part of the decision being recorded; a copy is a maintenance cost'
    ],
    api: [
      { name: 'LineItemEntity — ProductId, CatalogId, PriceId, Price, Sku, Name as plain values', file: '(vc-module-order/src/VirtoCommerce.OrdersModule.Data/Model/LineItemEntity.cs)' },
      { name: 'DefaultCustomerOrderTotalsCalculator — sums stored values, never re-prices', file: '(vc-module-order/src/VirtoCommerce.OrdersModule.Data/Services/DefaultCustomerOrderTotalsCalculator.cs)' },
      { name: 'AuditableEntity — the base every module entity derives from', file: 'src/VirtoCommerce.Platform.Core/Domain/AuditableEntity.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code: '// vc-module-order — the order line item, trimmed to the boundary fields.\n//\n// Note what is NOT here: no Product navigation property, no Price navigation\n// property, no [ForeignKey] to another module. Ids for traceability, values for\n// truth. This is the entire reason Orders can live in its own database.\npublic class LineItemEntity : AuditableEntity\n{\n    // Owned by the Catalog module — an id, nothing more.\n    public string ProductId { get; set; }\n    public string CatalogId { get; set; }\n\n    // Owned by the Pricing module — which price was used, recorded for audit.\n    public string PriceId { get; set; }\n\n    // Copied at checkout. The order keeps what was charged, not what the\n    // pricelist says today.\n    [Column(TypeName = "Money")]\n    public decimal Price { get; set; }\n    public string Sku { get; set; }\n    public string Name { get; set; }\n\n    // Every navigation property stays inside OrderDbContext.\n    public virtual CustomerOrderEntity CustomerOrder { get; set; }\n}'
    },
    gotchas: [
      'The Order module has **no** backend dependency on Pricing — the only mention of pricing in the whole repository is in the Admin UI JavaScript. An order never re-prices itself.',
      'A copied value can go stale by design. That is the point: a product renamed today must not rename itself on last year\'s invoice.',
      'Without a foreign key nothing stops a dangling id. Deleting a product does not clean up orders that reference it, and should not.',
      'You cannot join across modules in SQL. Cross-module reporting belongs in the search index or a warehouse, not in a query.'
    ],
    docs: [
      { label: 'Architecture reference', page: 'Back-End-Architecture/02-conceptual-overview' },
      { label: 'Modularity overview', page: 'Fundamentals/Modularity/01-overview' }
    ],
    seeAlso: ['module-database', 'ef-core', 'repository-uow', 'domain-events', 'search'],
    molecule: 'ecommerce-modules',
    verifiedAgainst: '3.1053.0'
  },

  // ================================================================ MODULARITY

  {
    id: 'module-federation',
    symbol: 'Fe',
    name: 'Module federation',
    family: 'modularity',
    adoption: 'platform',
    layer: 'modules',
    tags: ['frontend', 'vc-shell', 'plugin', 'remoteentry', 'app manifest', 'micro-frontend', 'etag'],
    oneLiner: 'The same modularity, on the front end: a module ships a built plugin and the shell discovers it at runtime.',
    pattern: 'Micro-frontend host with runtime discovery. A module declares an `<app>` it hosts; any module can drop a `plugin.json` into its plugins folder, and the platform composes the list per app and per user into one manifest the shell fetches at startup. Webpack Module Federation loads the remotes.',
    whenToUse: [
      'Adding a screen to the Commerce Manager, the Vendor Portal or any VC-Shell app without forking the shell',
      'Shipping back-office UI **with** the module that owns the domain, so one artifact carries both halves of the feature',
      'Standing up your own vertical app — declare it in `module.manifest` and let other modules extend it',
      'Gating UI by permission: a plugin declares one, and it simply does not reach a user who lacks it'
    ],
    avoid: [
      'Editing the shell to add your screen. That is the same mistake as editing a vendor module, one layer up',
      'Treating the manifest as static. It is per app and per user, and the ETag is what makes it cheap',
      'Shipping a plugin whose `remoteEntry.js` is built against a different shared-dependency version than the host — federation resolves shared deps at runtime, and a mismatch fails there rather than at build'
    ],
    api: [
      { name: 'GET api/apps/{appId}/manifest — the shell\'s entry point', file: 'src/VirtoCommerce.Platform.Web/Controllers/Api/AppManifestController.cs' },
      { name: 'AppManifestService — probes every module for plugin.json', file: 'src/VirtoCommerce.Platform.Modules/AppManifestService.cs' },
      { name: 'AppManifestDescriptor.Hash — the ETag, and what it covers', file: 'src/VirtoCommerce.Platform.Core/Modularity/AppManifestDescriptor.cs' },
      { name: 'PluginDescriptor — Entry, ContentFiles, Remote, Permission', file: 'src/VirtoCommerce.Platform.Core/Modularity/PluginDescriptor.cs' },
      { name: 'ManifestApp — the <app> element in module.manifest', file: 'src/VirtoCommerce.Platform.Core/Modularity/ManifestApp.cs' },
      { name: 'AppManifestCacheRegion — explicit invalidation', file: 'src/VirtoCommerce.Platform.Modules/AppManifestCacheRegion.cs' }
    ],
    snippet: {
      lang: 'xml',
      code:
'<!-- 1. A module declares an app it hosts. The platform serves it at /apps/{id}. -->\n' +
'<module>\n' +
'  <id>MyCompany.VendorPortal</id>\n' +
'  <apps>\n' +
'    <app id="vendor-portal">\n' +
'      <title>Vendor Portal</title>\n' +
'      <permission>vendor-portal:access</permission>\n' +
'      <contentPath>Content/vendor-portal</contentPath>\n' +
'    </app>\n' +
'  </apps>\n' +
'</module>\n' +
'\n' +
'<!-- 2. Any module can contribute a federated plugin INTO that app, by dropping a\n' +
'     plugin.json in its plugins folder. No change to the host, no rebuild of it. -->\n' +
'<!--\n' +
'plugins/abc-dashboard/plugin.json\n' +
'{\n' +
'  "id": "abc-dashboard",\n' +
'  "version": "1.2.0",\n' +
'  "permission": "abc:dashboard:access",\n' +
'  "entry":  { "path": "remoteEntry.js" },\n' +
'  "remote": { "name": "abcDashboard", "module": "./Dashboard" }\n' +
'}\n' +
'-->\n' +
'\n' +
'<!-- 3. The shell asks the platform what it is made of, and caches on the ETag:\n' +
'\n' +
'     GET  api/apps/vendor-portal/manifest\n' +
'     If-None-Match: "<hash>"     ->  304, or the plugin list this user may see\n' +
'\n' +
'     The hash covers the app version, every plugin entry and content file, the\n' +
'     federation coordinates, and the permission-filtered subset - so a changed\n' +
'     permission produces a changed manifest, not a stale one. -->'
    },
    gotchas: [
      'The manifest is **cached per app id** in `IPlatformMemoryCache` — and the cache is **bypassed in Development**, so a `yarn build` that produces a new `remoteEntry.js` shows up on the next fetch without restarting the platform. In production it is stable until restart or `AppManifestCacheRegion.ExpireRegion()`.',
      'Plugin order follows the **module dependency graph**, not the folder listing — the service walks the topologically sorted module list. If your plugin must load after another module\'s, declare the module dependency.',
      '`platform` is a reserved app id: it belongs to the legacy AngularJS admin shell. Do not use it for your own app.',
      'The descriptor `Version` is the running **platform** version for the `platform` app, and the version of the module that declares `<app>` for everything else. Two different meanings behind one field name.',
      'The hash covers the permission-filtered plugin subset, so two users can legitimately receive two different manifests for the same app — do not cache it at a CDN or shared proxy keyed on the URL alone.',
      'Two registry modules carry the same title, **Frontend Modules Registry** — `VirtoCommerce.ModuleFederation` and `VirtoCommerce.FrontendModules`, both from the `frontend-modules` repository, both described as replacing the static `apps.json`. Check which one your solution actually installs before pinning a version.'
    ],
    docs: [
      { label: 'Back-office app modularity', page: 'Fundamentals/Modularity/07-backoffice-app-modularity' },
      { label: 'Modularity overview', page: 'Fundamentals/Modularity/01-overview' },
      { label: 'frontend-modules (GitHub)', href: 'https://github.com/VirtoCommerce/frontend-modules' },
      { label: 'Module Federation (webpack)', href: 'https://webpack.js.org/concepts/module-federation/' }
    ],
    seeAlso: ['module-manifest', 'module-lifecycle', 'permissions', 'platform-memory-cache', 'cache-regions'],
    molecule: 'ecommerce-modules',
    verifiedAgainst: '3.1059.0'
  },

  {
    id: 'module-lifecycle',
    symbol: 'Mo',
    name: 'Module lifecycle',
    family: 'modularity',
    adoption: 'platform',
    layer: 'modules',
    tags: ['imodule', 'initialize', 'postinitialize', 'module.manifest', 'startup'],
    oneLiner: 'The two methods every module implements, and why the difference between them matters.',
    pattern: 'Two-phase initialization over a dependency-sorted module graph. `Initialize` registers services and type overrides while the container is still being built; `PostInitialize` runs once everything is registered and may resolve services — which is where registrars (settings, permissions) are called.',
    whenToUse: [
      '`Initialize` — DI registrations, `AbstractTypeFactory` overrides, options binding, EF context registration',
      '`PostInitialize` — registering settings and permissions, seeding, anything that needs to resolve a service',
      'Anywhere you are deciding "why is my registration too late?"'
    ],
    avoid: [
      'Resolving services in `Initialize`. The container is not finished, and you will capture the wrong thing or fail outright',
      'Registering an `AbstractTypeFactory` override in `PostInitialize` — by then something may already have constructed the base type',
      'Long-running or network work in either method; it blocks startup for the whole application'
    ],
    api: [
      { name: 'IModule', file: 'src/VirtoCommerce.Platform.Core/Modularity/IModule.cs' },
      { name: 'ManifestModuleInfo', file: 'src/VirtoCommerce.Platform.Core/Modularity/ManifestModuleInfo.cs' },
      { name: 'ModuleManifest', file: 'src/VirtoCommerce.Platform.Core/Modularity/ModuleManifest.cs' },
      { name: 'ModuleDependencySolver', file: 'src/VirtoCommerce.Platform.Core/Modularity/ModuleDependencySolver.cs' },
      { name: 'IHasConfiguration / IHasLogger', file: 'src/VirtoCommerce.Platform.Core/Modularity/IHasConfiguration.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'public class Module : IModule, IHasConfiguration\n' +
'{\n' +
'    public ManifestModuleInfo ModuleInfo { get; set; }\n' +
'    public IConfiguration Configuration { get; set; }\n' +
'\n' +
'    public void Initialize(IServiceCollection services)\n' +
'    {\n' +
'        // Registrations only — the container is still being built.\n' +
'        services.AddDbContext<MyModuleDbContext>(/* ... */);\n' +
'        services.AddTransient<IMyService, MyService>();\n' +
'        AbstractTypeFactory<CustomerOrder>.OverrideType<CustomerOrder, AbcCustomerOrder>();\n' +
'    }\n' +
'\n' +
'    public void PostInitialize(IApplicationBuilder appBuilder)\n' +
'    {\n' +
'        // Now it is safe to resolve things.\n' +
'        var services = appBuilder.ApplicationServices;\n' +
'        services.GetRequiredService<ISettingsRegistrar>()\n' +
'            .RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);\n' +
'        services.GetRequiredService<IPermissionsRegistrar>()\n' +
'            .RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions);\n' +
'    }\n' +
'\n' +
'    public void Uninstall() { }\n' +
'}'
    },
    gotchas: [
      'A module without a valid `module.manifest` never loads, and the failure is easy to miss among startup logs.',
      'How the platform finds this class: it scans the assembly named by `assemblyFile` for non-abstract `IModule` implementations. One match is used directly and the manifest\'s `moduleType` is ignored; several require `moduleType` to disambiguate; none throws `ModuleInitializeException`.',
      'The class is instantiated with `Activator.CreateInstance`, so it needs a public parameterless constructor. Constructor injection does not work here — take `IConfiguration`, `ILogger` and `IHostEnvironment` through the marker interfaces instead, and resolve everything else in `PostInitialize`.',
      'A module with no `assemblyFile` has no `IModule` at all and skips this lifecycle entirely — see the `module.manifest` atom.',
      'Load order comes from the manifest dependency graph, topologically sorted. If your module needs another\'s registration, declare the dependency — do not rely on luck.',
      '`AbstractTypeFactory` overrides belong in `Initialize` precisely because `PostInitialize` can be too late.',
      'Optional dependencies exist (`IOptionalDependency`), so a module can adapt to another being absent rather than failing.'
    ],
    docs: [
      { label: 'Create a new module', page: 'Tutorials-and-How-tos/Tutorials/create-new-module-from-scratch' },
      { label: 'Create a new module (advanced)', page: 'Fundamentals/Modularity/02-folder-structure' },
      { label: 'Essential modularity', page: 'Fundamentals/Modularity/01-overview' }
    ],
    seeAlso: ['module-catalog', 'platform-startup', 'dependency-injection', 'abstract-type-factory', 'settings'],
    molecule: 'ecommerce-modules',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'module-manifest',
    symbol: 'Mf',
    name: 'module.manifest',
    family: 'modularity',
    adoption: 'platform',
    layer: 'modules',
    tags: ['manifest', 'moduletype', 'startuptype', 'assemblyfile', 'apps', 'web-only', 'frontend-only'],
    oneLiner: 'The file without which a module does not exist — and the file that decides whether it needs a .NET assembly at all.',
    pattern: 'Declarative descriptor read before any code runs. `ManifestReader` deserializes `module.manifest` into `ModuleManifest`, the catalog turns that into `ManifestModuleInfo` and resolves the dependency graph from it, and only then is an assembly loaded — if the manifest names one.',
    whenToUse: [
      'Every module. There is no convention-based fallback: no manifest, no module',
      'Declaring what you need — `platformVersion` gates compatibility and `dependencies` drives load order',
      'Shipping a **frontend-only module**: omit `assemblyFile` and you need no C# at all',
      'Contributing a standalone app to the admin shell through `<apps>`',
      'Declaring settings without an assembly, via `<settings>`'
    ],
    avoid: [
      'Assuming `moduleType` is how the platform finds your `IModule` — it is only a tie-breaker',
      'A floating `platformVersion`; it is a compatibility gate, not documentation',
      'Declaring the same setting both in the manifest and in code — pick one per setting',
      '`startupType` in a module with no `assemblyFile`; it is resolved from the assembly, so it is silently meaningless'
    ],
    api: [
      { name: 'ModuleManifest — the module.manifest schema', file: 'src/VirtoCommerce.Platform.Core/Modularity/ModuleManifest.cs' },
      { name: 'ManifestReader.Read', file: 'src/VirtoCommerce.Platform.Core/Modularity/ManifestReader.cs' },
      { name: 'ManifestModuleInfo.LoadFromManifest', file: 'src/VirtoCommerce.Platform.Core/Modularity/ManifestModuleInfo.cs' },
      { name: 'ManifestApp / AppPlacement', file: 'src/VirtoCommerce.Platform.Core/Modularity/ManifestApp.cs' },
      { name: 'ManifestSetting', file: 'src/VirtoCommerce.Platform.Core/Modularity/ManifestSetting.cs' }
    ],
    snippet: {
      lang: 'xml',
      code:
'<!-- A .NET module. assemblyFile is what makes it code. -->\n' +
'<module>\n' +
'  <id>MyCompany.MyModule</id>\n' +
'  <version>1.0.0</version>\n' +
'  <platformVersion>3.1053.0</platformVersion>\n' +
'  <dependencies>\n' +
'    <dependency id="VirtoCommerce.Core" version="3.800.0" />\n' +
'  </dependencies>\n' +
'\n' +
'  <assemblyFile>MyCompany.MyModule.Web.dll</assemblyFile>\n' +
'  <!-- Only consulted when that assembly contains MORE THAN ONE IModule.\n' +
'       With exactly one, this element is ignored entirely. -->\n' +
'  <moduleType>MyCompany.MyModule.Web.Module</moduleType>\n' +
'  <!-- Optional IPlatformStartup contributor, resolved from the same assembly. -->\n' +
'  <startupType>MyCompany.MyModule.Web.Startup</startupType>\n' +
'</module>\n' +
'\n' +
'<!-- A frontend-only module: no assemblyFile, therefore no C# and no IModule.\n' +
'     The catalog marks it Initialized straight away and never loads an assembly. -->\n' +
'<module>\n' +
'  <id>MyCompany.MyPortal</id>\n' +
'  <version>1.0.0</version>\n' +
'  <platformVersion>3.1053.0</platformVersion>\n' +
'\n' +
'  <apps>\n' +
'    <app id="my-portal">\n' +
'      <title>My portal</title>\n' +
'      <permission>myPortal:access</permission>\n' +
'      <contentPath>dist</contentPath>\n' +
'      <placement>MainMenu</placement>\n' +
'    </app>\n' +
'  </apps>\n' +
'\n' +
'  <!-- Settings with no assembly: registered through ISettingsRegistrar at startup,\n' +
'       exactly as if IModule.Initialize had registered them. -->\n' +
'  <settings>\n' +
'    <setting>\n' +
'      <name>MyPortal.PageSize</name>\n' +
'      <valueType>Integer</valueType>\n' +
'      <defaultValue>20</defaultValue>\n' +
'    </setting>\n' +
'  </settings>\n' +
'</module>'
    },
    gotchas: [
      'The platform does **not** read `moduleType` to find your module class. It scans the assembly for non-abstract `IModule` implementations: with exactly one it uses that and ignores `moduleType` completely — which is why a stale value can sit in a manifest for years and never break. Only when an assembly holds several does `moduleType` matter, matched by assembly-qualified-name prefix; no match throws `ModuleInitializeException`.',
      'Naming an `assemblyFile` whose assembly has *no* `IModule` implementation fails initialization with "No IModule implementation found in assembly …". A module with code must have a module class.',
      'The module class is created with `Activator.CreateInstance`, so it needs a public parameterless constructor. There is no constructor injection — that is precisely why the marker interfaces (`IHasConfiguration`, `IHasLogger`, `IHasHostEnvironment`) exist.',
      'Omitting `assemblyFile` is a supported shape, not an oversight: the catalog comments say "modules without assembly file don\'t need initialization" and set state to `Initialized` without loading anything.',
      '`<app>` takes `id` as an XML *attribute* while everything inside it is an element — an easy thing to get wrong by symmetry.'
    ],
    docs: [
      { label: 'module.manifest file', page: 'Fundamentals/Modularity/06-module-manifest-file' },
      { label: 'Loading modules into the app process', page: 'Fundamentals/Modularity/04-loading-modules-into-app-process' },
      { label: 'Modularity overview', page: 'Fundamentals/Modularity/01-overview' }
    ],
    seeAlso: ['module-lifecycle', 'module-catalog', 'platform-startup', 'settings', 'vc-build'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'module-catalog',
    symbol: 'Mc',
    name: 'Module catalog & install',
    family: 'modularity',
    adoption: 'platform',
    layer: 'modules',
    tags: ['discovery', 'probing', 'install', 'uninstall', 'version', 'semver'],
    oneLiner: 'How modules are found, chosen, copied and loaded — including at runtime, from a gallery.',
    pattern: 'Discovery then probing then load. The local catalog scans the modules folder, the external catalog reads a remote manifest feed, the highest matching version wins, files are copied to a probing folder under a distributed lock, then assemblies load in dependency order — for the modules that have one.',
    whenToUse: [
      'Diagnosing why a module did not load, or why an old version is still running',
      'Understanding what module install does on a multi-instance deployment',
      'Reasoning about the two kinds of module the catalog handles: **.NET modules** with an `assemblyFile` and a C# `IModule`, and **frontend-only modules** that carry only static content, apps and manifest settings',
      'Building tooling or deployment automation around modules'
    ],
    avoid: [
      'Editing the probing folder by hand and expecting it to survive; it is derived state',
      'Installing modules at runtime as a deployment strategy for production — prefer a baked image',
      'Assuming install is instant. It is a background operation, historically an actual background job'
    ],
    api: [
      { name: 'ILocalModuleCatalog', file: 'src/VirtoCommerce.Platform.Core/Modularity/ILocalModuleCatalog.cs' },
      { name: 'IExternalModuleCatalog', file: 'src/VirtoCommerce.Platform.Core/Modularity/IExternalModuleCatalog.cs' },
      { name: 'IModuleInstaller', file: 'src/VirtoCommerce.Platform.Core/Modularity/IModuleInstaller.cs' },
      { name: 'IModuleManagementService', file: 'src/VirtoCommerce.Platform.Core/Modularity/IModuleManagementService.cs' },
      { name: 'SemanticVersion', file: 'src/VirtoCommerce.Platform.Core/Common/SemanticVersion.cs' }
    ],
    snippet: {
      lang: 'json',
      code:
'// module.manifest — the file without which nothing loads.\n' +
'// Caret ranges: ^3.800.0 means >=3.800.0 <4.0.0\n' +
'{\n' +
'  "id": "MyCompany.MyModule",\n' +
'  "version": "1.0.0",\n' +
'  "platformVersion": "3.1053.0",\n' +
'  "dependencies": [\n' +
'    { "id": "VirtoCommerce.Core", "version": "3.800.0" }\n' +
'  ]\n' +
'}'
    },
    gotchas: [
      'Two versions of a module present means the highest version wins — which is how a stale copy left in the modules folder can quietly shadow your build.',
      'A module with no `assemblyFile` is not broken: the catalog marks it `Initialized` on the spot and never loads an assembly for it. That is how a frontend-only module — admin app, static content, manifest-declared settings — ships with no C# at all.',
      'Which means "my module loaded but none of my code ran" has a boring likely cause: no `assemblyFile` in the manifest, so there was no code to run.',
      'Multi-instance installs coordinate through a Redis distributed lock. Without Redis, concurrent installs can race over the probing folder.',
      'The platform\'s own module management has historically run as a background job, which is why job engine availability and module install are entangled.',
      '`platformVersion` is a compatibility gate, not documentation: get it wrong and the module is skipped.'
    ],
    docs: [
      { label: 'Modularity', page: 'Fundamentals/Modularity/01-overview' },
      { label: 'Deploy a module from source', page: 'Tutorials-and-How-tos/Tutorials/deploy-module-from-source-code' }
    ],
    seeAlso: ['module-lifecycle', 'distributed-lock', 'platform-startup', 'background-jobs'],
    molecule: 'deployment',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'platform-startup',
    symbol: 'Pi',
    name: 'Platform startup hooks',
    family: 'modularity',
    adoption: 'platform',
    layer: 'modules',
    tags: ['iplatformstartup', 'middleware', 'pipeline', 'configure', 'bootstrap'],
    oneLiner: 'How a module participates in application startup beyond its own DI registrations.',
    pattern: 'Startup contributor. A module can implement `IPlatformStartup` to take part in host configuration, and marker interfaces (`IHasConfiguration`, `IHasLogger`, `IHasHostEnvironment`, `IHasModuleCatalog`) declare what the platform should inject into the module class itself.',
    whenToUse: [
      'Adding middleware to the request pipeline from a module',
      'Needing `IConfiguration`, `ILogger` or `IHostEnvironment` inside `Module.cs`',
      'Startup work that must happen outside `Initialize` / `PostInitialize`'
    ],
    avoid: [
      'Adding middleware whose order matters without checking where you land in the pipeline',
      'Doing in a startup hook what a plain DI registration would do',
      'Assuming your hook runs before another module\'s — order follows the dependency graph'
    ],
    api: [
      { name: 'IPlatformStartup', file: 'src/VirtoCommerce.Platform.Core/Modularity/IPlatformStartup.cs' },
      { name: 'IHasConfiguration', file: 'src/VirtoCommerce.Platform.Core/Modularity/IHasConfiguration.cs' },
      { name: 'IHasHostEnvironment', file: 'src/VirtoCommerce.Platform.Core/Modularity/IHasHostEnvironment.cs' },
      { name: 'IHasLogger', file: 'src/VirtoCommerce.Platform.Core/Modularity/IHasLogger.cs' },
      { name: 'IPlatformRestarter', file: 'src/VirtoCommerce.Platform.Core/Modularity/IPlatformRestarter.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Marker interfaces are how the module class gets platform services injected.\n' +
'public class Module : IModule, IHasConfiguration, IHasLogger, IHasHostEnvironment\n' +
'{\n' +
'    public IConfiguration Configuration { get; set; }\n' +
'    public ILogger Logger { get; set; }\n' +
'    public IHostEnvironment HostEnvironment { get; set; }\n' +
'\n' +
'    public void Initialize(IServiceCollection services)\n' +
'    {\n' +
'        if (HostEnvironment.IsDevelopment())\n' +
'        {\n' +
'            Logger.LogInformation("Registering development-only services");\n' +
'        }\n' +
'    }\n' +
'}'
    },
    gotchas: [
      'These marker interfaces are the supported way to reach configuration and logging in `Module.cs` — reaching for a static or a service locator instead is the usual mistake.',
      'Middleware added from a module lands relative to the platform\'s own pipeline. Verify the position rather than assuming.',
      '`IPlatformRestarter` exists because some changes genuinely require a restart. Triggering one is an operational act, not a code path to take lightly.'
    ],
    docs: [
      { label: 'Create a new module (advanced)', page: 'Fundamentals/Modularity/02-folder-structure' }
    ],
    seeAlso: ['module-lifecycle', 'options-pattern', 'logging', 'module-catalog'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'dependency-injection',
    symbol: 'Di',
    name: 'Dependency injection',
    family: 'modularity',
    adoption: 'platform',
    layer: 'platform',
    tags: ['ioc', 'container', 'override', 'lifetime', 'scoped', 'singleton'],
    oneLiner: 'Standard .NET DI — pick the lifetime first — plus the Virto convention that makes it an extension point: the last registration wins.',
    pattern: 'Constructor injection over `IServiceCollection`, with override-by-later-registration. Because modules initialize in dependency order, a module loaded later can replace a service registered earlier simply by registering its own implementation.',
    whenToUse: [
      '**`AddTransient` — a new instance per resolution. Start here.** Stateless services, domain services, handlers: cheap to construct, nothing shared, no thread-safety question to answer. Microsoft says the same — do not reach for singleton just because a service holds no state.',
      '**`AddScoped` — one instance per request.** Anything that must see one consistent view of the data for the length of a request: `DbContext` (which `AddDbContext` registers as scoped for you), repositories and units of work built on it. A scoped service may only be used **inside** a scope — the implicit per-request one, or an explicit `IServiceScopeFactory.CreateScope()`.',
      '**`AddSingleton` — one instance for the process.** State that is expensive to build or genuinely global, plus connections and clients: the memory cache, the in-process event bus, `IHttpClientFactory`. Two obligations come with it — it must be thread-safe, and it must hold nothing scoped.',
      'Work with no request to borrow a scope from — a background job, a hosted service, a startup task — opens its own: `using var scope = provider.CreateScope();`. The platform does exactly this in `BackgroundJob.Enqueue`.',
      'Replacing a vendor service with your own implementation — register yours in a module that depends on theirs',
      'Making your own services replaceable by registering against an interface'
    ],
    avoid: [
      'Injecting a scoped service into a singleton — a **captive dependency**. The scoped service is promoted to the process lifetime, and the symptom is stale data on a later request rather than an exception',
      'Resolving a scoped service straight from the root provider. Same outcome: only the root container disposes it, so it lives until shutdown',
      'Singleton for a service with no state of its own. It buys nothing and costs thread-safety, coupling between requests, harder tests and no configuration reload',
      'Service locator patterns (`GetRequiredService` in business code) outside `PostInitialize`',
      'Calling `BuildServiceProvider` while configuring services — take the factory overload that hands you an `IServiceProvider` instead',
      'Registering a `DbContext` or repository as a singleton — see [[repository-uow]] for why'
    ],
    api: [
      { name: 'IServiceCollection (module Initialize)', file: 'src/VirtoCommerce.Platform.Core/Modularity/IModule.cs' },
      { name: 'Platform service registrations', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' },
      { name: 'AddPlatformServices — transient repository, Func<> factory, singleton bus', file: 'src/VirtoCommerce.Platform.Data/Extensions/ServiceCollectionExtensions.cs' },
      { name: 'IPlatformMemoryCache — registered as a singleton', file: 'src/VirtoCommerce.Platform.Caching/ServiceCollectionExtensions.cs' },
      { name: 'BackgroundJob.Enqueue — CreateScope outside a request', file: 'src/VirtoCommerce.Platform.Core/Jobs/BackgroundJob.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// ------------------------------------------------------- 1. pick the lifetime\n' +
'//\n' +
'// Transient - a new one per resolution. The default: stateless, cheap, nothing shared.\n' +
'services.AddTransient<IPriceService, PriceService>();\n' +
'\n' +
'// Scoped - one per request, for anything needing one consistent view of the data.\n' +
'// AddDbContext already does this, so the DbContext is scoped whether you say so or not.\n' +
'services.AddDbContext<AbcDbContext>(options => { /* ... */ });\n' +
'services.AddScoped<IAbcRepository, AbcRepository>();\n' +
'\n' +
'// Singleton - one per process. Expensive or genuinely global state, connections, clients.\n' +
'// Must be thread-safe, and must hold nothing scoped.\n' +
'services.AddSingleton<IAbcPriceIndex, AbcPriceIndex>();\n' +
'\n' +
'// One instance behind several interfaces - register the class once and alias the rest,\n' +
'// or you get one object per interface. This is how the platform exposes InProcessBus.\n' +
'services.AddSingleton<AbcBus>();\n' +
'services.AddSingleton<IAbcPublisher>(sp => sp.GetRequiredService<AbcBus>());\n' +
'\n' +
'// --------------------------------------------- 2. reach a scope from outside one\n' +
'//\n' +
'// A job, hosted service or startup task has no request, so it opens its own scope.\n' +
'// Injecting the scoped service here would capture it for the life of the process.\n' +
'public class AbcNightlyJob(IServiceProvider provider)\n' +
'{\n' +
'    public async Task RunAsync()\n' +
'    {\n' +
'        using var scope = provider.CreateScope();\n' +
'        var repository = scope.ServiceProvider.GetRequiredService<IAbcRepository>();\n' +
'        // ... one unit of work, and the scope disposes the DbContext with it\n' +
'    }\n' +
'}\n' +
'\n' +
'// The platform\'s idiom for the same need: a factory registered beside the repository,\n' +
'// so a longer-lived service asks for a fresh one instead of holding one.\n' +
'//   services.AddTransient<Func<IPlatformRepository>>(provider =>\n' +
'//       () => provider.CreateScope().ServiceProvider.GetService<IPlatformRepository>());\n' +
'\n' +
'// ------------------------------------------------ 3. override, do not edit\n' +
'//\n' +
'// Your module depends on theirs, so it initializes later and simply registers again.\n' +
'services.AddTransient<IPriceService, AbcPriceService>();\n' +
'\n' +
'// Decorating instead of replacing: keep the original as a concrete registration.\n' +
'services.AddTransient<PriceService>();\n' +
'services.AddTransient<IPriceService>(sp =>\n' +
'    new AbcCachingPriceService(sp.GetRequiredService<PriceService>()));'
    },
    gotchas: [
      'Override-by-last-registration is both the extension mechanism and the accident: two modules replacing the same service means load order silently decides, with no warning.',
      'Registration order across modules follows the manifest dependency graph. If you must load after someone, declare the dependency.',
      'Keyed services exist in .NET but are unused here — see the Keyed services atom before introducing them.',
      'Captive dependencies (scoped inside singleton) are the most common lifetime bug and often surface as stale data rather than an exception.',
      '**Scope validation only runs in Development.** The host verifies that no scoped service is resolved from the root provider or injected into a singleton — and that check is off in production, which is exactly where the same mistake becomes stale data instead of a startup exception.',
      'The container disposes what it creates, and **when** depends on the lifetime: transient and scoped at the end of their scope, singleton at shutdown. A disposable transient resolved from the root provider is therefore held until shutdown — a leak wearing a lifetime label.',
      'This platform leans singleton: on `dev` the mix is roughly **76 singleton, 26 transient, 13 scoped**. Almost all of those singletons are caches, buses, registrars and factories — read the ratio as infrastructure, not as a licence for your own services.',
      '`IPlatformMemoryCache` and `InProcessBus` are singletons, so anything handed to them lives as long as the process. Pass values, not scoped services.',
      'The platform registers `IPlatformRepository` as **transient** and puts a `Func<IPlatformRepository>` next to it that opens its own scope. Injecting the repository into something longer-lived than a request is what that factory exists to prevent.'
    ],
    docs: [
      { label: 'Extensibility overview', page: 'Extensibility/overview' },
      { label: 'Service lifetimes (Microsoft)', href: 'https://learn.microsoft.com/dotnet/core/extensions/dependency-injection/service-lifetimes' },
      { label: 'DI guidelines (Microsoft)', href: 'https://learn.microsoft.com/dotnet/core/extensions/dependency-injection/guidelines' },
      { label: 'DI in ASP.NET Core (Microsoft)', href: 'https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection' },
      { label: 'Scope validation (Microsoft)', href: 'https://learn.microsoft.com/dotnet/core/extensions/dependency-injection/overview#scope-validation' }
    ],
    seeAlso: ['module-lifecycle', 'keyed-services', 'abstract-type-factory', 'options-pattern', 'repository-uow', 'platform-memory-cache'],
    molecule: 'extensibility-decision-tree',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'keyed-services',
    symbol: 'Kd',
    name: 'Keyed services',
    family: 'modularity',
    adoption: 'available',
    layer: 'platform',
    tags: ['dotnet', 'addkeyedscoped', 'fromkeyedservices', 'named', 'provider'],
    oneLiner: '.NET keyed DI registrations — available in the framework, but the platform resolves variants its own way.',
    pattern: 'Register several implementations of one interface under distinct keys and resolve by key. The Virto equivalent for provider selection is a registry or a factory chosen by configuration, as search providers and job engines do.',
    whenToUse: [
      'Rarely, inside a single module that genuinely needs several implementations of one interface side by side',
      'Only after checking how the surrounding code selects providers, so you stay consistent with it'
    ],
    avoid: [
      'Using keys where the platform would use a registry — you break the pattern other developers expect',
      'Keying a service other modules should be able to override; keyed registrations do not participate in override-by-last-registration',
      'Introducing it into a shared contract; consumers then need to know your keys'
    ],
    api: [
      { name: 'AddKeyedScoped / AddKeyedSingleton', file: '(.NET — no platform usage)' },
      { name: '[FromKeyedServices]', file: '(.NET — no platform usage)' }
    ],
    useInstead: 'Follow the provider-registry pattern: one active implementation selected by configuration, exactly as search providers and background-job engines are chosen.',
    note: 'Verified by grep across `src/`: no `AddKeyedScoped`, `AddKeyedSingleton` or `FromKeyedServices` usage anywhere in the platform. Using it is legal but unidiomatic here, and it interacts badly with the override convention modules rely on.',
    gotchas: [
      'A keyed registration is not replaced by a later unkeyed one, so a module trying to override your service will appear to succeed and change nothing.',
      'Keys are strings or objects with no compile-time relationship to the consumer — the failure mode is a resolution exception at runtime.'
    ],
    docs: [],
    seeAlso: ['dependency-injection', 'module-lifecycle'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'backup-restore',
    symbol: 'Br',
    name: 'Backup & restore',
    family: 'modularity',
    adoption: 'platform',
    layer: 'platform',
    tags: ['backup', 'restore', 'export', 'import', 'migration', 'sample data', 'iexportsupport', 'iimportsupport'],
    oneLiner: 'The platform-wide data transfer mechanism a module opts into — backup, restore, seeding and environment moves.',
    pattern: 'Per-module export and import contributors coordinated by a platform manager. A module implements `IExportSupport` and `IImportSupport`, writing to and reading from a stream while reporting progress and honouring cancellation. The manifest records what a package contains.',
    whenToUse: [
      'Making your module\'s data part of platform export and import',
      'Shipping sample data for a demo or a new environment',
      'Moving configuration and data between environments'
    ],
    avoid: [
      'Loading everything into memory before writing. Stream it, or large tenants fail',
      'Ignoring the progress callback — an import with no feedback looks hung',
      'Implementing only the obsolete `ICancellationToken` overload in new code'
    ],
    note: 'The platform owns the contracts; the platform-wide backup and restore experience ships as the **`VirtoCommerce.BackupRestore`** module (`vc-module-backup-restore`) today. Per-domain export/import is a different thing again — catalogue CSV, prices and customers each have their own module.',
    api: [
      { name: 'IExportSupport', file: 'src/VirtoCommerce.Platform.Core/ExportImport/IExportSupport.cs' },
      { name: 'IImportSupport', file: 'src/VirtoCommerce.Platform.Core/ExportImport/IImportSupport.cs' },
      { name: 'IPlatformExportImportManager', file: 'src/VirtoCommerce.Platform.Core/ExportImport/IPlatformExportImportManager.cs' },
      { name: 'ExportImportProgressInfo', file: 'src/VirtoCommerce.Platform.Core/ExportImport/ExportImportProgressInfo.cs' },
      { name: 'PlatformExportManifest', file: 'src/VirtoCommerce.Platform.Core/ExportImport/PlatformExportManifest.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Implement the CancellationToken overload. The obsolete ICancellationToken one\n' +
'// (VC0014) exists only so older modules keep compiling.\n' +
'public class MyModuleExportImport : IExportSupport, IImportSupport\n' +
'{\n' +
'    public async Task ExportAsync(Stream outStream, ExportImportOptions options,\n' +
'        Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)\n' +
'    {\n' +
'        progressCallback(new ExportImportProgressInfo { Description = "Exporting my entities" });\n' +
'\n' +
'        // Stream in batches — never materialise the whole table.\n' +
'        await foreach (var batch in ReadBatches(cancellationToken))\n' +
'        {\n' +
'            await WriteBatch(outStream, batch, cancellationToken);\n' +
'        }\n' +
'    }\n' +
'}'
    },
    gotchas: [
      'Both interfaces carry two overloads; the obsolete `ICancellationToken` one throws `NotImplementedException` by default and the modern one bridges to it via `CancellationTokenWrapper`. Implement the modern one.',
      'Import order across modules matters when data references other modules\' data — the platform manager sequences it, so do not assume you can import standalone.',
      'Export runs against live data. On a large tenant it is a background job with real duration, not a request.'
    ],
    docs: [
      { label: 'vc-module-backup-restore (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-backup-restore' }
    ],
    seeAlso: ['cancellation', 'job-progress', 'json-serialization', 'background-jobs'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'vc-build',
    symbol: 'Cb',
    name: 'vc-build CLI',
    family: 'modularity',
    adoption: 'module',
    layer: 'modules',
    tags: ['cli', 'globaltool', 'nuke', 'vc-package.json', 'ci', 'release', 'devops'],
    oneLiner: 'The official CLI for building, packaging, installing and releasing a Virto solution — and for making installs reproducible.',
    pattern: 'Target-based build automation over nuke.build, plus a lockfile. `vc-package.json` records the platform and module versions a solution needs, so the same command produces the same environment on a laptop and on a build server. `Install` and `Update` maintain that file for you.',
    whenToUse: [
      'Standing up or updating an environment: `Install`, `Update`, `InstallPlatform`, `InstallModules`',
      'Packaging for deployment: `Compress`, `Pack`, `Publish`',
      'Release and hotfix flow: `StartRelease` / `CompleteRelease`, `StartHotfix` / `CompleteHotfix`',
      'CI pipelines — the same targets you run locally, so the build is not a second implementation'
    ],
    avoid: [
      'Hand-copying module folders between environments; that is what `vc-package.json` and `Install` exist to replace',
      'Editing `vc-package.json` by hand when a command maintains it',
      'Letting local and CI use different build steps — both should call the same targets'
    ],
    api: [
      { name: 'dotnet tool install VirtoCommerce.GlobalTool -g', file: '(NuGet package id — installs the vc-build global tool)' },
      { name: 'Install · Update · Uninstall · InstallPlatform · InstallModules', file: '(package-management targets)' },
      { name: 'Clean · Restore · Compile · Test · Publish · Compress · Pack · WebPackBuild', file: '(build targets)' },
      { name: 'StartRelease · CompleteRelease · QuickRelease · StartHotfix · CompleteHotfix', file: '(release targets)' },
      { name: 'vc-package.json', file: '(records platform + module versions for reproducible installs)' }
    ],
    snippet: {
      lang: 'bash',
      code:
'# Install once, globally.\n' +
'dotnet tool install VirtoCommerce.GlobalTool -g\n' +
'\n' +
'# Stand up an environment from the lockfile — same result on a laptop and in CI.\n' +
'vc-build Install\n' +
'\n' +
'# Or pin pieces explicitly; both commands update vc-package.json for you.\n' +
'vc-build Install -platform\n' +
'vc-build Update\n' +
'\n' +
'# Package a module for deployment.\n' +
'vc-build Compress\n' +
'\n' +
'# Release flow, rather than hand-tagging.\n' +
'vc-build StartRelease\n' +
'vc-build CompleteRelease'
    },
    note: 'Not a platform module but a .NET global tool (`VirtoCommerce.GlobalTool`), powered by nuke.build. It carries the install-separately badge for the same reason a module does: it is not in the platform, and nothing happens until you add it.',
    gotchas: [
      '`vc-package.json` is the reproducibility contract. Commit it, or "works on my machine" turns into a version-drift hunt.',
      'Module versions resolve through caret SemVer ranges, so an unpinned range can move under you between two runs of the same command.',
      'The tool is versioned independently of the platform; an old global tool against a new platform is a common source of confusing install failures.',
      'Targets compose, so a CI job can call `Compile` then `Test` then `Compress` rather than scripting each step itself.'
    ],
    docs: [
      { label: 'CLI overview', page: 'CLI-tools/overview' },
      { label: 'Getting started', page: 'CLI-tools/getting-started' },
      { label: 'Package management', page: 'CLI-tools/package-management' },
      { label: 'Install & update platform and modules', page: 'CLI-tools/install-and-update-platform-and-modules' },
      { label: 'Build automation', page: 'CLI-tools/build-automation' },
      { label: 'vc-build (GitHub)', href: 'https://github.com/VirtoCommerce/vc-build' }
    ],
    seeAlso: ['module-catalog', 'module-lifecycle', 'ef-core'],
    molecule: 'dev-process',
    verifiedAgainst: '3.1053.0'
  },

  // ================================================================ SECURITY

  {
    id: 'permissions',
    symbol: 'Pm',
    name: 'Permissions',
    family: 'security',
    adoption: 'platform',
    layer: 'platform',
    tags: ['authorize', 'role', 'scope', 'ipermissionsregistrar', 'moduleconstants'],
    oneLiner: 'Declared per module, assigned to roles, enforced per endpoint — with optional row-level scopes.',
    pattern: 'Declare then register then enforce. Permissions are declared as constants in `ModuleConstants.Security.Permissions`, registered via `IPermissionsRegistrar` in `PostInitialize`, and enforced with `[Authorize(ModuleConstants.Security.Permissions.Read)]`. `PermissionScope` narrows a granted permission to a subset of data.',
    whenToUse: [
      'Every REST endpoint a module exposes — no exceptions',
      'Any Admin UI action that should depend on the user\'s role',
      'Row-level restrictions, using scopes rather than a bespoke filter'
    ],
    avoid: [
      'Checking role names in code. Roles are operator-assignable; permissions are the contract',
      'A single coarse permission for a whole module; operators cannot delegate what you did not separate',
      'Enforcing only in the UI. The API is the security boundary'
    ],
    api: [
      { name: 'IPermissionsRegistrar', file: 'src/VirtoCommerce.Platform.Core/Security/IPermissionsRegistrar.cs' },
      { name: 'Permission', file: 'src/VirtoCommerce.Platform.Core/Security/Permission.cs' },
      { name: 'PermissionScope', file: 'src/VirtoCommerce.Platform.Core/Security/PermissionScope.cs' },
      { name: 'ISupportSecurityScopes', file: 'src/VirtoCommerce.Platform.Core/Security/ISupportSecurityScopes.cs' },
      { name: 'IPermissionsRegistrarExtensions', file: 'src/VirtoCommerce.Platform.Core/Security/IPermissionsRegistrarExtensions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Module.Core/ModuleConstants.cs\n' +
'public static class Security\n' +
'{\n' +
'    public static class Permissions\n' +
'    {\n' +
'        public const string Read   = "myModule:read";\n' +
'        public const string Create = "myModule:create";\n' +
'        public const string Update = "myModule:update";\n' +
'        public const string Delete = "myModule:delete";\n' +
'\n' +
'        public static string[] AllPermissions => [Read, Create, Update, Delete];\n' +
'    }\n' +
'}\n' +
'\n' +
'// Module.Web/Module.cs — PostInitialize\n' +
'appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>()\n' +
'    .RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions\n' +
'        .Select(x => new Permission { GroupName = "MyModule", Name = x }).ToArray());\n' +
'\n' +
'// Controller — enforcement lives here, not in the UI.\n' +
'[Authorize(ModuleConstants.Security.Permissions.Read)]\n' +
'public async Task<ActionResult<MyEntity[]>> Get() { /* ... */ }'
    },
    gotchas: [
      'An unregistered permission cannot be granted to a role, so an endpoint using it is effectively closed to everyone but administrators — and the diagnostic is a bare 403.',
      'Separate read from write from delete. Merging them means an operator must grant deletion to allow viewing.',
      'Scopes are enforced only where a service actually consults them; adding a scope does not retroactively filter existing queries.'
    ],
    docs: [
      { label: 'Secure Web API', page: 'Fundamentals/Security/authorization/overview' },
      { label: 'User manager', page: 'Fundamentals/Security/authorization/global-permissions' }
    ],
    seeAlso: ['authorization', 'authentication', 'current-user', 'module-lifecycle'],
    molecule: 'security-compliance',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'authentication',
    symbol: 'Au',
    name: 'Authentication',
    family: 'security',
    adoption: 'platform',
    layer: 'platform',
    tags: ['openiddict', 'jwt', 'api key', 'sso', 'identity', 'oauth'],
    oneLiner: 'Who the caller is: OpenIddict-issued tokens, API keys, basic auth, and external identity providers.',
    pattern: 'ASP.NET Core Identity plus OpenIddict as the authorization server, with additional handlers for machine callers. The platform ships password login, bearer tokens, API key and basic authentication handlers, and external sign-in for SSO providers.',
    whenToUse: [
      'Storefront and Admin UI sign-in — bearer tokens from OpenIddict',
      'Machine-to-machine and integration callers — API keys',
      'Corporate SSO — an external provider such as Azure AD, via the external sign-in pipeline'
    ],
    avoid: [
      'Rolling your own token validation. The handlers already exist and are wired',
      'Basic authentication over anything but TLS, and preferably not at all',
      'Long-lived API keys with broad permissions; scope them and rotate them'
    ],
    api: [
      { name: 'OpenIddict 7.5 configuration', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' },
      { name: 'ApiKeyAuthenticationHandler', file: 'src/VirtoCommerce.Platform.Web/Security/Authentication/ApiKeyAuthenticationHandler.cs' },
      { name: 'BasicAuthenticationHandler', file: 'src/VirtoCommerce.Platform.Web/Security/Authentication/BasicAuthenticationHandler.cs' },
      { name: 'IUserApiKeyService', file: 'src/VirtoCommerce.Platform.Core/Security/IUserApiKeyService.cs' },
      { name: 'ExternalSignIn', file: 'src/VirtoCommerce.Platform.Core/Security/ExternalSignIn/' },
      { name: 'ApplicationUser', file: 'src/VirtoCommerce.Platform.Core/Security/ApplicationUser.cs' }
    ],
    snippet: {
      lang: 'bash',
      code:
'# Bearer token from the OpenIddict token endpoint.\n' +
'curl -X POST https://localhost:5001/connect/token \\\n' +
'  -d "grant_type=password&username=admin&password=***&scope=offline_access"\n' +
'\n' +
'# Then call the API with it:\n' +
'curl https://localhost:5001/api/platform/modules \\\n' +
'  -H "Authorization: Bearer <access_token>"\n' +
'\n' +
'# Or, for an integration caller, an API key instead of a token:\n' +
'curl https://localhost:5001/api/platform/modules -H "api_key: <key>"'
    },
    gotchas: [
      'Scaled out, bearer tokens must be validated by every instance — that needs shared data-protection keys, or tokens issued by one instance are rejected by another.',
      'External sign-in maps an external identity onto an `ApplicationUser`. Getting that mapping wrong silently creates duplicate users.',
      'API keys authenticate as a user, so they carry that user\'s permissions. A key for a highly privileged account is a highly privileged key.',
      'OpenIddict 7.x differs substantially from older majors; do not follow tutorials written for earlier versions.'
    ],
    docs: [
      { label: 'Secure Web API', page: 'Fundamentals/Security/authorization/overview' },
      { label: 'Add an SSO provider', page: 'Fundamentals/Security/extensions/adding-azure-as-sso-provider' },
      { label: 'Authentication with Azure AD', page: 'Fundamentals/Security/extensions/adding-azure-as-sso-provider' },
      { label: 'Sharing bearer tokens across instances', page: 'Tutorials-and-How-tos/How-tos/sharing-bearer-tokens' }
    ],
    seeAlso: ['authorization', 'permissions', 'current-user'],
    molecule: 'security-compliance',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'authorization',
    symbol: 'Az',
    name: 'Authorization policies',
    family: 'security',
    adoption: 'platform',
    layer: 'platform',
    tags: ['policy', 'requirement', 'handler', 'adminui', 'iauthorizationhandler'],
    oneLiner: 'What the caller may do: policies and requirements for rules that a permission attribute cannot express.',
    pattern: 'Requirement plus handler. Where `[Authorize("permission")]` reads as a static check, it is actually a policy underneath: `PermissionAuthorizationRequirement` carries the permission name and `PermissionAuthorizationHandlerBase` decides whether the user satisfies it. Write your own requirement and handler when the rule depends on the resource — ownership, tenancy, or entity state.',
    whenToUse: [
      'Rules depending on the resource, not just the user: "may edit their own orders only"',
      'Tenant or store scoping enforced centrally rather than per query',
      'Extending or replacing an existing platform policy'
    ],
    avoid: [
      'Putting business logic in a handler that belongs in the domain',
      'Client-side-only gates. A UI check is a usability feature, not a security control',
      'Duplicating a permission check that the attribute already performs'
    ],
    api: [
      { name: 'PermissionAuthorizationHandlerBase<TRequirement>', file: 'src/VirtoCommerce.Platform.Security/Authorization/PermissionAuthorizationHandlerBase.cs' },
      { name: 'PermissionAuthorizationRequirement', file: 'src/VirtoCommerce.Platform.Security/Authorization/PermissionAuthorizationRequirement.cs' },
      { name: 'DefaultPermissionAuthorizationHandler', file: 'src/VirtoCommerce.Platform.Security/Authorization/DefaultPermissionAuthorizationHandler.cs' },
      { name: 'AuthorizationOptions', file: 'src/VirtoCommerce.Platform.Core/Security/AuthorizationOptions.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// A requirement plus its handler — the resource-aware alternative to a permission attribute.\n' +
'public class OwnOrderRequirement : IAuthorizationRequirement { }\n' +
'\n' +
'public class OwnOrderHandler : AuthorizationHandler<OwnOrderRequirement, CustomerOrder>\n' +
'{\n' +
'    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,\n' +
'        OwnOrderRequirement requirement, CustomerOrder resource)\n' +
'    {\n' +
'        if (resource.CustomerId == context.User.GetUserId())\n' +
'        {\n' +
'            context.Succeed(requirement);\n' +
'        }\n' +
'        return Task.CompletedTask;\n' +
'    }\n' +
'}\n' +
'\n' +
'// Module.Web — register the handler; policies compose from requirements.\n' +
'services.AddSingleton<IAuthorizationHandler, OwnOrderHandler>();'
    },
    gotchas: [
      'Handlers must call `Succeed` to allow; doing nothing denies. A handler that silently does nothing looks like a mysterious 403.',
      'Several handlers for one requirement means any one succeeding is enough — that is deliberate, and easy to misread as "all must agree".',
      'There is a second, less obvious permission path: a `limited_permissions` claim. When present, it *caps* what the identity may do — only permissions listed in the claim are granted, whatever the user\'s roles say. When absent, no limit applies. It exists so cookie-authenticated non-AJAX GETs and third-party dashboards can be given a narrow slice of access, and it will confuse you if you debug a 403 by looking only at roles.',
      'Gates that exist only in the SPA are bypassable by calling the API directly. Enforce server-side, then mirror it in the UI.'
    ],
    docs: [
      { label: 'Extending authorization policies', page: 'Fundamentals/Security/authorization/overview' },
      { label: 'Authorization with JWT', page: 'Tutorials-and-How-tos/How-tos/authorization-using-jwt' }
    ],
    seeAlso: ['permissions', 'authentication', 'current-user'],
    molecule: 'security-compliance',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'current-user',
    symbol: 'Cu',
    name: 'Current user & tenancy',
    family: 'security',
    adoption: 'platform',
    layer: 'platform',
    tags: ['iusernameresolver', 'claimsprincipal', 'audit', 'tenant', 'identity'],
    oneLiner: 'Who is acting right now — for audit fields, notifications and scoping — and how to get it outside a request.',
    pattern: 'Resolver over an ambient principal. `IUserNameResolver.GetCurrentUserName()` works inside a request from `HttpContext` and can be set explicitly where no request exists — which is how a background job records a meaningful actor. `ClaimsPrincipalExtensions` reads ids and claims; `TenantIdentity` pairs an object type with an id for scoping.',
    whenToUse: [
      'Audit fields and change-log entries',
      'Addressing a push notification to the user who started the work',
      'Background jobs that must act, and be recorded, as a specific user'
    ],
    avoid: [
      'Injecting `IHttpContextAccessor` into business services; depend on the resolver instead',
      'Assuming a current user exists. In a job, a hosted service or at startup there may be none',
      'Trusting a user id from the request body over the authenticated principal'
    ],
    api: [
      { name: 'IUserNameResolver', file: 'src/VirtoCommerce.Platform.Core/Security/IUserNameResolver.cs' },
      { name: 'ICurrentUser', file: 'src/VirtoCommerce.Platform.Core/Security/ICurrentUser.cs' },
      { name: 'ClaimsPrincipalExtensions', file: 'src/VirtoCommerce.Platform.Core/Security/ClaimsPrincipalExtensions.cs' },
      { name: 'TenantIdentity', file: 'src/VirtoCommerce.Platform.Core/Common/TenantIdentity.cs' },
      { name: 'IUserSessionsService', file: 'src/VirtoCommerce.Platform.Core/Security/IUserSessionsService.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Works inside a request; also settable where there is no request.\n' +
'public class MyService(IUserNameResolver userNameResolver)\n' +
'{\n' +
'    public Task Save(MyEntity entity)\n' +
'    {\n' +
'        entity.ModifiedBy = userNameResolver.GetCurrentUserName();\n' +
'        return _repository.SaveAsync(entity);\n' +
'    }\n' +
'}\n' +
'\n' +
'// In a background job there is no HttpContext, so make the actor explicit\n' +
'// rather than letting audit fields record something meaningless.\n' +
'userNameResolver.SetCurrentUserName(payload.InitiatedBy);'
    },
    gotchas: [
      '`ICurrentUser` carries only `UserName`. For ids and claims use `ClaimsPrincipalExtensions` — expecting a rich user object here leads to a confusing dead end.',
      'A background job with no user set records whatever default applies, which makes an audit trail that looks complete and tells you nothing.',
      'The enqueuing user flows into a job\'s DI scope via `IHttpContextAccessor`, so audit fields often survive an enqueue — but do not rely on it without checking.'
    ],
    docs: [
      { label: 'User manager', page: 'Fundamentals/Security/authorization/global-permissions' }
    ],
    seeAlso: ['authentication', 'permissions', 'change-log', 'push-notifications'],
    molecule: 'security-compliance',
    verifiedAgainst: '3.1053.0'
  },

  // ================================================================ INFRA & OPS

  {
    id: 'distributed-lock',
    symbol: 'Dl',
    name: 'Distributed lock',
    family: 'ops',
    adoption: 'platform',
    layer: 'platform',
    tags: ['redlock', 'redis', 'mutex', 'cluster', 'exclusive', 'scale-out'],
    oneLiner: 'Cluster-wide mutual exclusion via Redis — with a no-op fallback so development needs no Redis.',
    pattern: 'Distributed lock over RedLock, wrapped in an execute-with-lock helper. `IDistributedLockService.ExecuteAsync(resourceKey, resolver, …)` acquires, runs and releases; when no Redis is configured a `NoLockService` satisfies the same interface so single-instance development just works.',
    whenToUse: [
      'Work that must happen once cluster-wide: module install, schema migration, a singleton scheduled task',
      'Guarding a resource two instances could otherwise corrupt',
      'Making a recurring job exclusive across instances'
    ],
    avoid: [
      'Long critical sections. Lock expiry versus work duration is where distributed locks go wrong',
      'Assuming the lock is a transaction. It orders access; it does not roll anything back',
      'Relying on it in development without Redis — the fallback grants everything, so the code path is untested'
    ],
    api: [
      { name: 'IDistributedLockService', file: 'src/VirtoCommerce.Platform.Core/DistributedLock/IDistributedLockService.cs' },
      { name: 'DistributedLockService (Redis / RedLock)', file: 'src/VirtoCommerce.Platform.DistributedLock/Redis/DistributedLockService.cs' },
      { name: 'NoLockService (fallback)', file: 'src/VirtoCommerce.Platform.DistributedLock/NoLock/NoLockService.cs' },
      { name: 'DistributedLockOptions', file: 'src/VirtoCommerce.Platform.DistributedLock/DistributedLockOptions.cs' },
      { name: 'DistributedLockCondition', file: 'src/VirtoCommerce.Platform.DistributedLock/DistributedLockCondition.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Acquire, run, release — with explicit timeouts rather than defaults you did not choose.\n' +
'var result = await _distributedLockService.ExecuteAsync(\n' +
'    resourceKey: $"reindex:{storeId}",\n' +
'    resolver: async () =>\n' +
'    {\n' +
'        await Reindex(storeId);\n' +
'        return true;\n' +
'    },\n' +
'    lockTimeout: TimeSpan.FromMinutes(5),    // must exceed the work, or the lock expires mid-flight\n' +
'    tryLockTimeout: TimeSpan.FromSeconds(10),\n' +
'    retryInterval: TimeSpan.FromSeconds(1));'
    },
    gotchas: [
      'Without a Redis connection string the no-op implementation is registered and every acquisition succeeds. Code that looks locked in development is unlocked in production if Redis is missing there too.',
      'If `lockTimeout` is shorter than the work, the lock expires while you are still running and a second instance joins you. Size it against worst-case duration, not typical.',
      '`AsyncLock` and this are different tools: in-process versus cluster-wide. Choosing the wrong one produces a bug that only appears when you scale out.'
    ],
    docs: [
      { label: 'Scale out on Azure', page: 'Fundamentals/Scalability/scaling-configuration-on-azure-cloud' },
      { label: 'Scalability', page: 'Fundamentals/Scalability/scalability-options' }
    ],
    seeAlso: ['async-lock', 'redis-cache-bus', 'module-catalog', 'recurring-jobs'],
    molecule: 'deployment',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'logging',
    symbol: 'Lg',
    name: 'Logging',
    family: 'ops',
    adoption: 'platform',
    layer: 'platform',
    tags: ['serilog', 'ilogger', 'structured', 'sink', 'appsettings'],
    oneLiner: 'Serilog behind `ILogger<T>`, configured from `appsettings.json`, with a bootstrap logger for early module loading.',
    pattern: 'Structured logging through the standard abstraction. Inject `ILogger<T>`, log message templates with named properties rather than interpolated strings, and let configuration decide the sinks. A bootstrap logger exists because module loading happens before DI is ready.',
    whenToUse: [
      'Anything you would want to see in production when something goes wrong',
      'Structured properties you will later filter on: entity ids, store ids, job ids',
      'Warnings for recoverable oddities, errors for genuine failures'
    ],
    avoid: [
      'String interpolation in log messages — you lose every structured property',
      'Logging personal data, secrets or full request bodies',
      'Logging and rethrowing at every level; one exception becomes five confusing entries'
    ],
    api: [
      { name: 'Serilog configuration', file: 'src/VirtoCommerce.Platform.Web/Program.cs' },
      { name: 'ILogger<T> (Microsoft.Extensions.Logging)', file: 'src/VirtoCommerce.Platform.Core/Logger/' },
      { name: 'IHasLogger (module logger injection)', file: 'src/VirtoCommerce.Platform.Core/Modularity/IHasLogger.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// ✓ Structured: storeId and count stay queryable properties.\n' +
'_logger.LogInformation("Reindexed store {StoreId} with {Count} documents", storeId, count);\n' +
'\n' +
'// ✕ Interpolated: one opaque string, nothing to filter on.\n' +
'_logger.LogInformation($"Reindexed store {storeId} with {count} documents");\n' +
'\n' +
'// Exceptions go in the first parameter, not into the message.\n' +
'_logger.LogError(ex, "Reindex failed for store {StoreId}", storeId);'
    },
    gotchas: [
      'Sinks and levels come from the `Serilog` section of `appsettings.json`, so "logging is broken" is usually configuration rather than code.',
      'A bootstrap logger handles the window before DI exists — early module-loading failures land there, not in your configured sinks, which is why they can seem to vanish.',
      'Log level is a production cost. Debug logging left on in a hot path can outweigh the work being logged.'
    ],
    docs: [
      { label: 'Logging', page: 'Fundamentals/Logging/overview' }
    ],
    seeAlso: ['health-checks', 'developer-tools', 'platform-startup'],
    molecule: 'observability',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'swagger',
    symbol: 'Sw',
    name: 'Swagger / OpenAPI',
    family: 'ops',
    adoption: 'platform',
    layer: 'api-edge',
    tags: ['swashbuckle', 'openapi', 'docs', 'client', 'autorest'],
    oneLiner: 'Generated API documentation per module and combined — the browsable contract, and the basis for generated clients.',
    pattern: 'Reflection-driven document generation with per-module grouping. `AddSwagger` builds a document per module plus a combined one, so a module\'s API is discoverable as soon as it loads. Polymorphic types need explicit help to appear correctly.',
    whenToUse: [
      'Exploring an unfamiliar module\'s API — start here rather than in the controller source',
      'Generating a typed client (AutoRest and similar) from the document',
      'Confirming that a change to your controller did what you meant to the published contract'
    ],
    avoid: [
      'Leaving the UI publicly reachable in production without considering what it discloses',
      'Assuming a derived type appears correctly. Polymorphism needs configuration',
      'Treating the generated document as documentation of intent; it describes shape, not meaning'
    ],
    api: [
      { name: 'services.AddSwagger', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' },
      { name: 'app.UseSwagger', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' },
      { name: 'Platform Swagger helpers', file: 'src/VirtoCommerce.Platform.Core/Swagger/' },
      { name: 'PlatformOptions.UseAllOfToExtendReferenceSchemas', file: 'src/VirtoCommerce.Platform.Core/PlatformOptions.cs' }
    ],
    snippet: {
      lang: 'bash',
      code:
'# Combined document and per-module documents are both served.\n' +
'#   /docs/index.html                     — the UI\n' +
'#   /docs/PlatformModule/swagger.json    — one module\n' +
'#   /docs/VirtoCommerce.Platform/swagger.json\n' +
'\n' +
'# Swashbuckle 10.2 with Microsoft.OpenApi 2.x. Because MVC serializes with\n' +
'# Newtonsoft, schema generation follows those settings — a serialization\n' +
'# change can alter your published contract.'
    },
    gotchas: [
      'Schema generation uses the global JSON serializer settings, which are Newtonsoft here. That coupling means serializer changes are contract changes.',
      'Derived types registered through `AbstractTypeFactory` need explicit handling to show up — see the polymorphic-types technique doc.',
      'Per-module documents keep the UI usable; the combined document on a large install is enormous and slow to render.'
    ],
    docs: [
      { label: 'Swagger endpoints', page: 'Tutorials-and-How-tos/How-tos/swagger-api' },
      { label: 'Polymorphic types in Swagger', page: 'Tutorials-and-How-tos/How-tos/type-inheritance-support-in-swagger' }
      
    ],
    seeAlso: ['json-serialization', 'abstract-type-factory', 'developer-tools'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'http-client',
    symbol: 'Ht',
    name: 'HTTP client',
    family: 'ops',
    adoption: 'platform',
    layer: 'platform',
    tags: ['ihttpclientfactory', 'addhttpclient', 'outbound', 'integration', 'socket'],
    oneLiner: 'Outbound HTTP the correct way — factory-managed clients, never a `new HttpClient` per call.',
    pattern: 'Factory-pooled handlers. `services.AddHttpClient()` registers `IHttpClientFactory`, which pools and recycles the underlying handlers. The platform uses it for external module catalogue access and licensing calls.',
    whenToUse: [
      'Any call to an external API: payment gateways, ERP, licensing, webhooks',
      'When you want per-client configuration — base address, default headers, timeout',
      'As the injection point for resilience policies, if you add them'
    ],
    avoid: [
      '`new HttpClient()` per request. It exhausts sockets under load, and the failure looks like a network problem',
      'A static `HttpClient` that never refreshes DNS — the opposite failure',
      'No timeout. The default is long enough to turn a slow dependency into an outage'
    ],
    api: [
      { name: 'services.AddHttpClient()', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' },
      { name: 'ExternalModulesClient', file: 'src/VirtoCommerce.Platform.Modules/External/ExternalModulesClient.cs' },
      { name: 'LicensingController usage', file: 'src/VirtoCommerce.Platform.Web/Controllers/Api/LicensingController.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// A named or typed client, configured once.\n' +
'services.AddHttpClient<IErpClient, ErpClient>(client =>\n' +
'{\n' +
'    client.BaseAddress = new Uri(options.ErpBaseUrl);\n' +
'    client.Timeout = TimeSpan.FromSeconds(30);   // always set one\n' +
'});\n' +
'\n' +
'// Inject the typed client; the factory owns handler lifetime.\n' +
'public class ErpClient(HttpClient httpClient) : IErpClient\n' +
'{\n' +
'    public Task<HttpResponseMessage> GetOrder(string id) =>\n' +
'        httpClient.GetAsync($"orders/{id}");\n' +
'}'
    },
    gotchas: [
      'The platform calls the parameterless `AddHttpClient()`, so there are no pre-configured named clients to inherit from — configure your own.',
      'A typed client is transient while its handler is pooled. Do not keep per-call state on the client class.',
      'An outbound call with no timeout inside a request handler is how one slow third party takes down your thread pool.'
    ],
    docs: [],
    seeAlso: ['resilience', 'options-pattern', 'eventbus-webhooks', 'logging'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'resilience',
    symbol: 'Re',
    name: 'Resilience (Polly)',
    family: 'ops',
    adoption: 'available',
    layer: 'platform',
    tags: ['polly', 'retry', 'circuit breaker', 'timeout', 'transient'],
    oneLiner: 'Retry, circuit-breaker and timeout policies. The package is referenced but the platform has no call sites.',
    pattern: 'Resilience pipeline around an outbound call, usually attached to a typed `HttpClient`. Polly 8 composes retry, circuit-breaker, timeout and hedging strategies into one pipeline.',
    whenToUse: [
      'Calls to a flaky external dependency where a retry genuinely helps',
      'Protecting yourself from a failing dependency with a circuit breaker rather than piling up requests',
      'Enforcing a hard timeout the remote side does not honour'
    ],
    avoid: [
      'Retrying non-idempotent operations — a retried payment is worse than a failed one',
      'Retrying inside a request handler without a budget; you multiply load on an already-struggling dependency',
      'Assuming a platform convention exists. There is none to follow yet'
    ],
    api: [
      { name: 'Polly 8.7.0 (package reference)', file: 'src/VirtoCommerce.Platform.Caching/VirtoCommerce.Platform.Caching.csproj' },
      { name: 'AddResilienceHandler', file: '(.NET — no platform usage)' }
    ],
    note: 'Grep across `src/` finds no `Polly`, `ResiliencePipeline` or `AddResilienceHandler` usage in code — the only trace is a `PackageReference` in `VirtoCommerce.Platform.Caching.csproj`. Treat it as available rather than adopted, and do not assume behaviour you did not configure yourself.',
    gotchas: [
      'Retry plus a long timeout multiplies worst-case latency. Budget the whole pipeline, not each step.',
      'A circuit breaker changes failure semantics: callers start failing fast, which is correct and surprising if nobody expected it.',
      'Because there is no platform precedent, whatever you configure becomes your solution\'s convention. Document it.'
    ],
    docs: [],
    seeAlso: ['http-client', 'health-checks', 'background-jobs'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'developer-tools',
    symbol: 'Dt',
    name: 'Developer tools',
    family: 'ops',
    adoption: 'platform',
    layer: 'platform',
    tags: ['diagnostics', 'admin ui', 'registrar', 'dashboard', 'introspection'],
    oneLiner: 'A registry of in-product diagnostic links, surfaced in the Admin UI and filtered by permission.',
    pattern: 'Descriptor registry with per-user filtering. A module registers a `DeveloperToolDescriptor` pointing at a diagnostic page; `GetRegisteredTools(claimsPrincipal)` returns only what the current user may see, so tools are discoverable without being exposed.',
    whenToUse: [
      'Giving your module a diagnostics page reachable from the Admin UI',
      'Surfacing an existing dashboard (a job engine UI, a cache inspector) in one predictable place',
      'Anything an operator or support engineer needs, and should not have to be told the URL of'
    ],
    avoid: [
      'Registering a tool without a permission check; the registry filters, but the page itself must also be protected',
      'Exposing anything that mutates state without an explicit confirmation and authorization',
      'Using it as a substitute for logs or health checks'
    ],
    api: [
      { name: 'IDeveloperToolRegistrar', file: 'src/VirtoCommerce.Platform.Core/DeveloperTools/IDeveloperToolsRegistrar.cs' },
      { name: 'DeveloperToolDescriptor', file: 'src/VirtoCommerce.Platform.Core/DeveloperTools/DeveloperToolDescriptor.cs' },
      { name: 'Admin UI blade', file: 'src/VirtoCommerce.Platform.Web/wwwroot/js/app/developer-tools/blades/developer-tools-main.html' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// Module.Web/Module.cs — PostInitialize\n' +
'appBuilder.ApplicationServices.GetRequiredService<IDeveloperToolRegistrar>()\n' +
'    .RegisterDeveloperTool(new DeveloperToolDescriptor\n' +
'    {\n' +
'        Name = "My module diagnostics",\n' +
'        Url = "/my-module/diagnostics"\n' +
'    });\n' +
'\n' +
'// The registry filters by ClaimsPrincipal, but the page behind the URL still\n' +
'// needs its own [Authorize] — the registry hides the link, not the endpoint.'
    },
    gotchas: [
      'Hiding a link is not access control. The tool\'s own endpoint must enforce authorization independently.',
      'This is where the job engine dashboard has historically been registered, which is why it disappears when no engine module is installed.',
      'Tool pages tend to accumulate privileged capability over time. Review what yours exposes as it grows.'
    ],
    docs: [
      { label: 'Debugging without source code', page: 'Tutorials-and-How-tos/How-tos/debugging' }
    ],
    seeAlso: ['health-checks', 'logging', 'permissions', 'cache-disabler'],
    molecule: 'observability',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'file-operations',
    symbol: 'Fo',
    name: 'File operations',
    family: 'ops',
    adoption: 'platform',
    layer: 'platform',
    tags: ['transactional', 'rollback', 'zip', 'filesystem', 'assets'],
    oneLiner: 'Rollbackable file-system operations and zip handling. Blob and asset storage is a module, not platform core.',
    pattern: 'Transactional file operations with rollback. `ITransactionFileManager` records operations (`CreateDirectory`, `Delete`, `SafeDelete`) as `IRollbackableOperation`s so a failed multi-step file change can be undone; `IZipFileWrapper` abstracts archive reading and extraction.',
    whenToUse: [
      'Multi-step file changes that must not leave a half-written state — module install being the platform\'s own case',
      'Reading or extracting archives (module packages, import bundles)',
      'Deleting files where a missing file should not be an exception (`SafeDelete`)'
    ],
    avoid: [
      'Expecting blob or asset storage here. There is no blob abstraction in the platform — that is the Assets module',
      'Local file paths as durable storage in a scaled-out deployment; instances do not share a disk',
      'Direct `System.IO` calls for multi-step changes you would need to unwind'
    ],
    api: [
      { name: 'ITransactionFileManager', file: 'src/VirtoCommerce.Platform.Core/TransactionFileManager/ITransactionFileManager.cs' },
      { name: 'IRollbackableOperation', file: 'src/VirtoCommerce.Platform.Core/TransactionFileManager/IRollbackableOperation.cs' },
      { name: 'IZipFileWrapper', file: 'src/VirtoCommerce.Platform.Core/ZipFile/IZipFileWrapper.cs' },
      { name: 'IPathMapper', file: 'src/VirtoCommerce.Platform.Core/Common/IPathMapper.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// The full transactional surface, from Core/TransactionFileManager:\n' +
'public interface ITransactionFileManager\n' +
'{\n' +
'    void CreateDirectory(string path);\n' +
'    void Delete(string path);\n' +
'    void SafeDelete(string path);   // absent file is not an error\n' +
'}\n' +
'\n' +
'// Archives — used by module install to unpack a package.\n' +
'using var archive = _zipFileWrapper.OpenRead(packagePath);\n' +
'_zipFileWrapper.Extract(packagePath, targetDirectory);'
    },
    gotchas: [
      'There is no `IBlobStorageProvider` in the platform — verified by grep. Product images, imports and exports live behind the Assets module, so do not look for it here.',
      'Local disk is per instance. A file written on one instance is invisible to the others, which turns into "works on my node" bugs.',
      'Rollback only covers operations you performed through the manager. Mixing in direct `System.IO` calls leaves those unwound.'
    ],
    docs: [],
    seeAlso: ['backup-restore', 'module-catalog', 'external-processes'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'health-checks',
    symbol: 'Hc',
    name: 'Health checks',
    family: 'ops',
    adoption: 'platform',
    layer: 'platform',
    tags: ['health', 'readiness', 'liveness', 'probe', 'kubernetes', 'monitoring'],
    oneLiner: 'A `/health` endpoint that already checks modules, cache, Redis and the database — extend it, do not reinvent it.',
    pattern: 'Composed health checks with tags and failure severities. The platform registers checks for modules, cache, Redis and the configured database provider, mapping them to `/health`. A module adds its own by registering another check.',
    whenToUse: [
      'Container orchestration probes — readiness and liveness',
      'Load-balancer health decisions',
      'Adding a check for a dependency your module cannot work without'
    ],
    avoid: [
      'Expensive checks. A probe runs constantly and must not become load',
      'Marking a soft dependency `Unhealthy`; that takes the whole instance out of rotation',
      'Leaking internal details in the response body if the endpoint is publicly reachable'
    ],
    api: [
      { name: 'services.AddHealthChecks()', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' },
      { name: 'ModulesHealthChecker', file: 'src/VirtoCommerce.Platform.Web/Infrastructure/HealthCheck/ModulesHealthChecker.cs' },
      { name: 'CacheHealthChecker', file: 'src/VirtoCommerce.Platform.Web/Infrastructure/HealthCheck/CacheHealthChecker.cs' },
      { name: 'RedisHealthCheck', file: 'src/VirtoCommerce.Platform.Web/Infrastructure/HealthCheck/RedisHealthCheck.cs' },
      { name: 'MapHealthChecks("/health")', file: 'src/VirtoCommerce.Platform.Web/Startup.cs' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// What the platform already registers (Startup.cs) — note the deliberate severities:\n' +
'services.AddHealthChecks()\n' +
'    .AddCheck<ModulesHealthChecker>("Modules health",\n' +
'        failureStatus: HealthStatus.Unhealthy, tags: ["Modules"])\n' +
'    .AddCheck<CacheHealthChecker>("Cache health",\n' +
'        failureStatus: HealthStatus.Degraded,  tags: ["Cache"])   // degraded, not unhealthy\n' +
'    .AddCheck<RedisHealthCheck>("Redis health",\n' +
'        failureStatus: HealthStatus.Unhealthy, tags: ["Cache"]);\n' +
'\n' +
'// Plus a per-provider database check, and the endpoint:\n' +
'// endpoints.MapHealthChecks("/health", new HealthCheckOptions { ... });'
    },
    gotchas: [
      'Severity is a deployment decision encoded in code: `Degraded` keeps the instance in rotation, `Unhealthy` removes it. The platform marks cache degraded and Redis unhealthy — understand why before copying either.',
      'The database check is registered per configured provider, so a check that works on SQL Server has an equivalent for PostgreSQL and MySQL.',
      'A probe that touches the database on every call becomes a load source of its own at orchestration frequency.'
    ],
    docs: [
      { label: 'Health checks', page: 'Tutorials-and-How-tos/How-tos/health-checks' }
    ],
    seeAlso: ['logging', 'developer-tools', 'redis-cache-bus', 'ef-core'],
    molecule: 'observability',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'time-provider',
    symbol: 'Tp',
    name: 'TimeProvider',
    /* Execution rather than ops: it is the clock async code schedules against, which puts it with
       cancellation and AsyncLock. It also keeps Infra & ops at twelve atoms — six grid rows — and
       the tallest family is what decides whether the poster still fits one screen. */
    family: 'execution',
    adoption: 'available',
    layer: 'platform',
    tags: ['dotnet', 'clock', 'testable', 'datetime', 'utcnow'],
    oneLiner: '.NET\'s abstract clock, for testable time. The package is referenced; nothing in the platform calls it.',
    pattern: 'Inject an abstract clock instead of calling `DateTime.UtcNow`, then substitute a fake in tests. The platform uses `DateTime.UtcNow` directly, so time-dependent logic here is tested by other means.',
    whenToUse: [
      'New code with time-dependent behaviour you want to unit test without waiting or sleeping',
      'Expiry, scheduling and windowing logic where a fake clock makes the test deterministic'
    ],
    avoid: [
      'Retrofitting it across existing platform-facing code for its own sake',
      'Mixing `TimeProvider` and `DateTime.UtcNow` in the same logic; only half of it becomes testable',
      'Assuming platform APIs accept a clock. They do not'
    ],
    api: [
      { name: 'TimeProvider', file: '(Microsoft.Bcl.TimeProvider — referenced by Platform.Web, no call sites)' }
    ],
    note: 'Verified by grep: `Microsoft.Bcl.TimeProvider 10.0.10` appears as a `PackageReference` in `VirtoCommerce.Platform.Web.csproj`, but there are no `TimeProvider` usages in platform source. It is available to your module; there is no platform pattern to follow.',
    gotchas: [
      'Because nothing uses it, adopting it is a local decision within your own module rather than following a convention.',
      'The value shows up in tests, not production. If your time logic is not tested, this buys you nothing.'
    ],
    docs: [],
    seeAlso: ['recurring-jobs', 'change-log'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'minimal-apis',
    symbol: 'Ma',
    name: 'Minimal APIs',
    family: 'ops',
    adoption: 'available',
    layer: 'api-edge',
    tags: ['dotnet', 'mapget', 'endpoints', 'controllers', 'mvc'],
    oneLiner: 'The .NET endpoint style the platform does not use. Everything here is MVC controllers.',
    pattern: 'Route handlers registered directly on the app rather than on controller classes. Idiomatic modern ASP.NET Core, and a poor fit here: the platform\'s conventions for permissions, Swagger grouping per module and polymorphic serialization are all built around controllers.',
    whenToUse: [
      'Practically never inside a Virto module. Follow the controller convention',
      'At most, a tiny internal endpoint with no permission, documentation or serialization requirements'
    ],
    avoid: [
      'Mixing endpoint styles inside one module; reviewers and tooling both expect controllers',
      'Assuming permission attributes, Swagger grouping and Newtonsoft settings apply automatically — they are wired for MVC',
      'Copying modern ASP.NET Core tutorials wholesale into a module'
    ],
    api: [
      { name: 'MapGet / MapPost', file: '(.NET — no platform usage; grep finds none in src/)' },
      { name: 'Platform controllers (the convention)', file: 'src/VirtoCommerce.Platform.Web/Controllers/Api/' }
    ],
    useInstead: 'An MVC controller under `Module.Web/Controllers/Api`, with `[Authorize(permission)]` and a route matching the module convention.',
    note: 'Verified by grep across `src/`: no `MapGet` or `MapPost` usage. Controllers are not legacy here — they are the mechanism the platform\'s permission, documentation and serialization behaviour is attached to.',
    gotchas: [
      'A minimal-API endpoint will not appear in your module\'s Swagger document the way a controller does, so it becomes an undocumented part of your API.',
      'Permission enforcement, Newtonsoft serialization and polymorphic type support all come from the MVC pipeline. Step outside it and you inherit none of them.'
    ],
    docs: [
      { label: 'Swagger endpoints', page: 'Tutorials-and-How-tos/How-tos/swagger-api' }
    ],
    seeAlso: ['swagger', 'json-serialization', 'permissions'],
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'assets',
    symbol: 'As',
    name: 'Assets & blob storage',
    family: 'ops',
    adoption: 'module',
    layer: 'infrastructure',
    tags: ['blob', 'images', 'files', 'azure', 'cdn', 'iblobstorageprovider'],
    oneLiner: 'Binary storage for images, imports and exports — behind a provider the platform core does not ship.',
    pattern: 'Port and adapter selected by configuration. `VirtoCommerce.Assets` owns the contract and the asset UI; a provider module supplies the backing store — `VirtoCommerce.AzureBlobAssets` for Azure Blob, `VirtoCommerce.FileSystemAssets` for local disk.',
    whenToUse: [
      'Anything binary: product images, attachments, generated documents, import and export files',
      'Handing a caller a public URL for a stored blob, via `IBlobUrlResolver`',
      'Claim-check for background jobs — store the file, pass its URL in the payload'
    ],
    avoid: [
      'The filesystem provider on more than one instance. Instances do not share a disk, so an upload lands on whichever node served the request',
      'Storing binaries in the relational database',
      'Serving large assets through the API instead of putting a CDN in front of blob storage',
      'Assuming the abstraction exists in a bare platform — without the module there is no blob API at all'
    ],
    api: [
      { name: 'IBlobStorageProvider — OpenRead / OpenWrite / SearchAsync / RemoveAsync', file: '(vc-module-assets — VirtoCommerce.AssetsModule.Core.Assets)' },
      { name: 'IBlobUrlResolver.GetAbsoluteUrl', file: '(vc-module-assets — turns a blob key into a public URL)' },
      { name: 'IAssetEntryService / IAssetEntrySearchService', file: '(vc-module-assets — the managed asset catalogue)' },
      { name: 'VirtoCommerce.Assets + AzureBlobAssets | FileSystemAssets', file: '(module ids from the vc-modules registry)' }
    ],
    snippet: {
      lang: 'csharp',
      code:
'// The provider is injected; which store backs it is a deployment choice.\n' +
'public class ReportService(IBlobStorageProvider blobs, IBlobUrlResolver urls)\n' +
'{\n' +
'    public async Task<string> Save(string name, Stream content)\n' +
'    {\n' +
'        var blobUrl = $"reports/{name}";\n' +
'\n' +
'        await using (var target = await blobs.OpenWriteAsync(blobUrl))\n' +
'        {\n' +
'            await content.CopyToAsync(target);\n' +
'        }\n' +
'\n' +
'        // Hand back an absolute URL — never a local path, which means nothing\n' +
'        // to another instance or to the browser.\n' +
'        return urls.GetAbsoluteUrl(blobUrl);\n' +
'    }\n' +
'}'
    },
    note: 'There is no blob abstraction in `vc-platform` itself — verified by grep. `IBlobStorageProvider` lives in the Assets module, so a platform without it installed has no way to store a file beyond raw `System.IO`.',
    gotchas: [
      'Blob URLs are keys, not paths. Resolve to an absolute URL with `IBlobUrlResolver` before handing one to a client.',
      '`IBlobStorageProvider` exposes sync and async pairs (`OpenRead` / `OpenReadAsync`). Prefer the async ones in request and job paths.',
      'The filesystem provider is a development convenience. Scaling out without switching to blob storage produces missing-image bugs that affect only some requests.',
      'Assets live outside the database, so they are outside its transactions and outside its backup.'
    ],
    docs: [
      { label: 'Configuring asset blob storage', page: 'Getting-Started/Post-Installation-Steps/03-configuring-asset-blob-storage' },
      { label: 'vc-module-assets (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-assets' }
    ],
    seeAlso: ['file-operations', 'search', 'backup-restore', 'background-jobs'],
    molecule: 'deployment',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'seq-logging',
    symbol: 'Sq',
    name: 'Seq & log sinks',
    family: 'ops',
    adoption: 'module',
    layer: 'infrastructure',
    tags: ['seq', 'serilog', 'sink', 'structured', 'observability', 'application insights'],
    oneLiner: 'Sending structured logs somewhere you can query them, instead of reading console output.',
    pattern: 'Serilog sink added by configuration. The platform logs through `ILogger<T>` onto Serilog; a sink module such as `VirtoCommerce.SeqLog` forwards those events to a log server, where the structured properties become queryable fields.',
    whenToUse: [
      'Any environment where you cannot attach a debugger — which is every shared environment',
      'Following one request or job across instances by correlating on a structured property',
      'Turning "it failed yesterday for one customer" into a query rather than an archaeology exercise'
    ],
    avoid: [
      'Shipping personal data or secrets to a log server — it is a searchable index of whatever you send it',
      'Debug level in production without a retention and volume plan',
      'Relying on the console sink in a container nobody is watching'
    ],
    api: [
      { name: 'VirtoCommerce.SeqLog (module)', file: '(vc-module-seq-log — a configuration-only Serilog sink module)' },
      { name: 'Serilog configuration', file: 'src/VirtoCommerce.Platform.Web/Program.cs' },
      { name: 'ILogger<T> (what you write against)', file: 'src/VirtoCommerce.Platform.Core/Logger/' }
    ],
    note: 'The module ships no interfaces of its own — it is wiring. You keep logging through `ILogger<T>` and the sink decides where events land; Application Insights is the other documented option.',
    gotchas: [
      'A sink is only as useful as your message templates. `LogInformation("Reindexed {StoreId}", id)` is queryable; an interpolated string is one opaque line and no sink can recover the property.',
      'Sinks are configured, not coded — "logs are not arriving" is almost always the `Serilog` section rather than your code.',
      'Log volume is a cost. The same sink that makes an incident tractable makes a chatty debug statement expensive.'
    ],
    docs: [
      { label: 'Seq module', page: 'Fundamentals/Logging/seq-module' },
      { label: 'Logging overview', page: 'Fundamentals/Logging/overview' },
      { label: 'Extended logging', page: 'Fundamentals/Logging/extended-logging' },
      { label: 'vc-module-seq-log (GitHub)', href: 'https://github.com/VirtoCommerce/vc-module-seq-log' }
    ],
    seeAlso: ['logging', 'health-checks', 'developer-tools'],
    molecule: 'observability',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'optional-dependency',
    symbol: 'Od',
    name: 'Optional dependency',
    family: 'modularity',
    adoption: 'platform',
    layer: 'modules',
    tags: ['manifest', 'optional', 'graceful degradation', 'composability', 'ioptionaldependency'],
    oneLiner: 'Declare a dependency you can live without, and the module still loads when it is absent.',
    pattern: 'Optional dependency, declared and resolved. `optional="true"` in the manifest takes the module out of the dependency solver and out of the installed-check; `IOptionalDependency<T>` is the runtime half, so code can ask whether the service is actually there.',
    whenToUse: [
      'A capability that enriches your module but is not required — the digital catalog runs without Pricing',
      'Building a module set small enough to deploy on its own: an optional edge is an edge you can cut',
      'Integrating with a module a customer may not have licensed or installed',
      'Any `serviceCollection` consumer that must not throw when the provider module is missing'
    ],
    avoid: [
      'Marking a real requirement optional to make an install succeed — it will fail later, at a worse moment',
      'Resolving the service in `Initialize` and caching it; ask through `IOptionalDependency<T>` at the point of use',
      'Optional dependencies as a substitute for events. If you only need to be told something happened, handle the event instead'
    ],
    api: [
      { name: 'IOptionalDependency<T> — HasValue / Value', file: 'src/VirtoCommerce.Platform.Core/Modularity/IOptionalDependency.cs' },
      { name: 'OptionalDependencyManager<T> — the registered implementation', file: 'src/VirtoCommerce.Platform.Modules/OptionalDependencyManager.cs' },
      { name: 'ManifestDependency.Optional — the optional="true" attribute', file: 'src/VirtoCommerce.Platform.Core/Modularity/ManifestDependency.cs' },
      { name: 'ModuleCatalog.SolveDependencies — optional edges are not added to the solver', file: 'src/VirtoCommerce.Platform.Core/Modularity/ModuleCatalog.cs' },
      { name: 'ModuleBootstrapper — required-dependency errors and their cascade', file: 'src/VirtoCommerce.Platform.Modules/ModuleBootstrapper.cs' },
      { name: 'IPricingEvaluatorService.EvaluateProductPricesAsync — the optional service in the snippet', file: '(vc-module-pricing/src/VirtoCommerce.PricingModule.Core/Services/IPricingEvaluatorService.cs)' }
    ],
    snippet: {
      lang: 'csharp',
      code: '// 1. Declare it in module.manifest:\n//\n//    <dependency id="VirtoCommerce.Pricing" version="3.1003.0" optional="true" />\n//\n//    xCatalog does exactly this for Pricing, Inventory and Marketing, which is why a\n//    catalog-only host is a viable module set.\n\n// 2. Ask for it at the point of use, not at registration:\npublic class ProductPriceEnricher\n{\n    private readonly IOptionalDependency<IPricingEvaluatorService> _pricing;\n\n    public ProductPriceEnricher(IOptionalDependency<IPricingEvaluatorService> pricing)\n    {\n        _pricing = pricing;\n    }\n\n    public async Task EnrichAsync(Product product, PriceEvaluationContext context)\n    {\n        // HasValue is false when the Pricing module is not installed in THIS host.\n        if (!_pricing.HasValue)\n        {\n            return;   // degrade, do not throw\n        }\n\n        // Evaluation takes a context, not an id: a pricelist assignment depends on\n        // store, catalog, customer and quantity, not on the product alone.\n        var prices = await _pricing.Value.EvaluateProductPricesAsync(context);\n        product.Prices = prices.Where(x => x.ProductId == product.Id).ToList();\n    }\n}'
    },
    gotchas: [
      'A **required** dependency that is missing is a hard gate: the module records `Module dependency {id} {version} is not installed` and every module depending on it is skipped with `Module skipped because its dependency has errors`. Optional edges are excluded from that cascade.',
      '`IOptionalDependency<T>` resolves through `IServiceProvider.GetService<T>()` on every access, so `HasValue` reflects the host it is running in — the same code is correct in a full host and in a subset host.',
      'Optional in the manifest and optional in code are two separate decisions. Declaring the edge optional without guarding the resolve gives you a null reference instead of a clear install error.',
      'The dependency solver ignores optional edges entirely, so an optional dependency does **not** guarantee load order. Do not rely on the other module having initialised first.'
    ],
    docs: [
      { label: 'Modularity overview', page: 'Fundamentals/Modularity/01-overview' },
      { label: 'Extensibility overview', page: 'Extensibility/overview' }
    ],
    seeAlso: ['module-manifest', 'module-catalog', 'dependency-injection', 'host-composition', 'module-lifecycle'],
    molecule: 'ecommerce-modules',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'host-composition',
    symbol: 'Ho',
    name: 'Host composition',
    family: 'modularity',
    adoption: 'platform',
    layer: 'solution',
    tags: ['composability', 'decomposition', 'vc-package.json', 'module subset', 'role', 'deployment'],
    oneLiner: 'One image, several hosts: choose the module set per host, then choose the role per host in configuration.',
    pattern: 'Composition at deployment time, on two axes. **Which modules** a host contains comes from its package manifest; **which role** it plays comes from configuration. Neither needs a code change, and the second needs no separate image.',
    whenToUse: [
      'Isolating a workload so it cannot degrade another — background jobs away from checkout',
      'Scaling one business capability on its own: a catalog-read host, sized for shopper traffic',
      'Giving the back office and the storefront API separate failure domains',
      'Cutting the surface of a host on purpose — fewer modules, fewer endpoints, smaller blast radius'
    ],
    avoid: [
      'Expecting a single module to become a service. The unit of deployment is a host with a module set, never one module',
      'Splitting by role before Redis is in place — cache coherence, the SignalR backplane and the migration lock all depend on it',
      'A module subset whose dependency graph does not close; the missing module takes its dependents down with it',
      'More than one shape at once. Get all-in-one measured first, then split the axis your metrics point at'
    ],
    api: [
      { name: 'BackgroundJobs:Mode — Producer | Worker | Both', file: 'src/VirtoCommerce.Platform.Web/appsettings.json' },
      { name: 'PushNotifications:ScalabilityMode — RedisBackplane | AzureSignalRService | None', file: 'src/VirtoCommerce.Platform.Core/PushNotifications/PushNotificationOptions.cs' },
      { name: 'ExecuteSynchronized — module migrations run one host at a time', file: 'src/VirtoCommerce.Platform.Web/Extensions/ApplicationBuilderExtensions.cs' },
      { name: 'ModuleBootstrapper — discovery, probing and the resolved module set', file: 'src/VirtoCommerce.Platform.Modules/ModuleBootstrapper.cs' },
      { name: 'vc-build install -PackageManifestPath — a different module set per image', file: '(vc-build CLI; manifest is vc-package.json)' }
    ],
    snippet: {
      lang: 'bash',
      code: '# Axis 1 — WHICH MODULES. Two manifests, two images, one source tree.\n# vc-package.json         full host: everything the solution has\n# vc-package.catalog.json catalog read path only\n\nvc-build install -PackageManifestPath vc-package.catalog.json \\\n                 -ProbingPath ./app_data/modules\n\n# Axis 2 — WHICH ROLE. Same image, same modules, configuration only.\n#\n#   storefront API   BackgroundJobs:Mode=Producer   ScalabilityMode=None            ARR off\n#   back office      BackgroundJobs:Mode=Producer   ScalabilityMode=RedisBackplane  ARR on\n#   job workers      BackgroundJobs:Mode=Worker     ScalabilityMode=None            ARR off\n#\n# As environment variables, so one image serves all three:\nexport BackgroundJobs__Mode=Worker\nexport PushNotifications__ScalabilityMode=None'
    },
    gotchas: [
      'The dependency graph is the ceiling on a module subset. `VirtoCommerce.Orders` declares 11 dependencies — Cart, Catalog, Customer, Inventory, Notifications, Payment, Search, Shipping, Store, Assets, Core — so "orders on its own" is not a module set that exists.',
      'Splitting by role changes nothing about the data: same modules, same database, same migrations. Splitting by module set is what changes the surface.',
      'Every host that shares a database runs migrations at startup. That is safe only under the distributed lock, which needs Redis — see [[distributed-lock]].',
      'A host without a module still shares the platform tables: settings, dynamic properties, permissions and module state are common ground.',
      'Two images means two things to promote. The DevOps view has one image on purpose; a second one doubles the release surface.'
    ],
    docs: [
      { label: 'Scalability options (S · M · L · XL)', page: 'Fundamentals/Scalability/scalability-options' },
      { label: 'Scaling configuration on Azure', page: 'Fundamentals/Scalability/scaling-configuration-on-azure-cloud' },
      { label: 'Package management (vc-package.json)', page: 'CLI-tools/package-management' },
      { label: 'Configuring environments', page: 'Tutorials-and-How-tos/How-tos/configuring-environments' }
    ],
    seeAlso: ['module-catalog', 'optional-dependency', 'module-database', 'background-jobs', 'distributed-lock', 'vc-build'],
    molecule: 'deployment',
    verifiedAgainst: '3.1053.0'
  },

  {
    id: 'scalability',
    symbol: 'Sc',
    name: 'Scalability',
    family: 'ops',
    adoption: 'platform',
    layer: 'infrastructure',
    tags: ['horizontal', 'vertical', 'scale out', 'scale up', 'readiness', 'replicas', 'performance', 'gc', 'container'],
    oneLiner: 'What is ready to scale out today, what will only ever scale up, and what has to be true before the second instance.',
    pattern: 'Stateless request path with shared state behind it. Every instance keeps a local memory cache and holds nothing else worth protecting, so capacity is a replica count; the state that cannot be replicated — one database writer, one index, one lock — is what you scale up instead. **Readiness is per level, not per platform**: some levels are ready out of the box, some are ready only vertically, and two are not ready at all.',
    whenToUse: [
      '**Ready to scale out, no work needed:** the request path (REST and XAPI) — it is stateless; the per-instance memory cache, once Redis carries invalidation; background jobs, via `Mode: Producer` and `Mode: Worker` hosts; a host per module subset; search, as an external cluster; assets, behind a cloud blob provider',
      '**Every module can have its own database — one line, no code.** Each module registers its own `DbContext` and resolves its connection string by module id first: add `ConnectionStrings:VirtoCommerce.Pricing` and Pricing moves to its own server, migrations and all. The platform does this for identity itself with `Auth:ConnectionString`. It works because modules hold ids and copied values across a boundary, never foreign keys — so the split is a configuration change rather than a schema project. See [[module-database]] and [[cross-module-references]].',
      '**Ready to scale up only:** the relational database — one writer, and read replicas where the provider allows; a single index rebuild; a single large import. More instances do not divide this work, a bigger machine does',
      '**Not ready, and no configuration changes it:** a transaction spanning two module databases, and a single module as its own service. Both are design constraints rather than settings — see [[cross-module-references]] and [[host-composition]]',
      'Before a launch, to decide the shape rather than discover it — the readiness list above is the checklist',
      'When one workload is degrading another: the answer is usually another host, not a bigger one'
    ],
    avoid: [
      'Scaling up first. It buys headroom you pay for around the clock and hides the coupling that stopped you scaling out',
      'Adding the second instance before the four prerequisites are met: Redis configured, a cloud blob provider instead of `FileSystemAssets`, a real search provider instead of Lucene, and the bearer-token certificate shared. Each of these works on one instance and fails on two',
      'Scaling the database to fix catalog browse. The index is the read path; the database is not in it',
      'Sizing every host the same. A 512 MB request host and a reindex worker want different machines',
      'Quoting a scalability number without saying which level it applies to — the platform layer and your custom modules scale on different curves'
    ],
    api: [
      { name: 'BackgroundJobs:Mode — Producer | Worker | Both, the horizontal split for jobs', file: 'src/VirtoCommerce.Platform.Web/appsettings.json' },
      { name: 'ConnectionStrings:RedisConnectionString — the scale-out prerequisite', file: 'src/VirtoCommerce.Platform.Caching/ServiceCollectionExtensions.cs' },
      { name: 'PushNotifications:ScalabilityMode — RedisBackplane | AzureSignalRService | None', file: 'src/VirtoCommerce.Platform.Core/PushNotifications/PushNotificationOptions.cs' },
      { name: 'SqlServer:CompatibilityLevel / ParameterTranslationMode — EF Core 10 query translation', file: 'src/VirtoCommerce.Platform.Data.SqlServer/Extensions/DbContextOptionsBuilderExtensions.cs' },
      { name: 'DbContextRepositoryBase — command timeout inherited from the connection string', file: 'src/VirtoCommerce.Platform.Data/Infrastructure/DbContextRepositoryBase.cs' },
      { name: 'ExecuteSynchronized — migrations serialised across instances', file: 'src/VirtoCommerce.Platform.Web/Extensions/ApplicationBuilderExtensions.cs' },
      { name: 'RedisPlatformMemoryCache — per-instance cache, shared invalidation', file: 'src/VirtoCommerce.Platform.Caching/Redis/RedisPlatformMemoryCache.cs' }
    ],
    snippet: {
      lang: 'json',
      code: '// The scale-out floor. Without the first line, a second instance is a second truth.\n{\n  "ConnectionStrings": {\n    "VirtoCommerce": "Data Source=sql;Initial Catalog=VirtoCommerce3;Connect Timeout=30;...",\n    "RedisConnectionString": "redis:6380,ssl=True,abortConnect=False"\n  },\n\n  // Horizontal split by role. Same image on every host, one key apart.\n  //   request hosts  Producer  — enqueue, never drain\n  //   worker hosts   Worker    — drain, serve nothing\n  "BackgroundJobs": { "Mode": "Producer" },\n\n  // Back office only: push notifications need the backplane and sticky sessions.\n  // Stateless request hosts set None and turn ARR affinity off.\n  "PushNotifications": { "ScalabilityMode": "RedisBackplane" },\n\n  // Cheaper than either kind of scaling: how EF Core 10 translates against SQL Server.\n  // Below level 120 it reaches for OPENJSON, which performs badly.\n  "SqlServer": {\n    "CompatibilityLevel": 120,\n    "ParameterTranslationMode": "Constant"\n  },\n\n  // Each module can own its database. The key is the module id from module.manifest,\n  // and the fallback is the shared string — so this is additive, per module, no code.\n  "ConnectionStrings": {\n    "VirtoCommerce.Pricing": "Data Source=sql-pricing;Initial Catalog=Pricing;...",\n    "VirtoCommerce.Orders":  "Data Source=sql-orders;Initial Catalog=Orders;..."\n  }\n}\n\n// runtimeconfig.json — the runtime side, and only if a measurement asked for it.\n// Defaults are already right for most hosts: Server GC for multi-core, Workstation on\n// 1 vCPU, DATAS sizing the heap to the app since .NET 9.\n{\n  "runtimeOptions": {\n    "configProperties": {\n      "System.GC.HeapHardLimitPercent": 75,   // the container-aware default\n      "System.GC.HighMemoryPercent": 90       // raise it for a small process, per Microsoft\n    }\n  }\n}'
    },
    gotchas: [
      '`Connect Timeout` in the connection string becomes the **command** timeout: `DbContextRepositoryBase` reads it off the connection and calls `SetCommandTimeout` with it. Raise it there and every query gets it, which is rarely what you want.',
      'The migration lock is a scale-out dependency, not a nicety. Module migrations run inside `ExecuteSynchronized`, which degrades to a no-op without Redis — so two instances starting together can both migrate.',
      '.NET reads its container limits and sizes the GC heap to them — in a container the GC treats the **container limit** as total physical memory, with a default heap hard limit of **75%** of it. Work that needs memory in bursts still does not fit: put a reindex or a bulk import on the jobs host and size that one for it.',
      '**Server GC is the ASP.NET Core default, and it is not available on a single-core machine** — a 1-vCPU container silently runs Workstation GC. That is usually what you want there: Microsoft\'s guidance is that Server GC suits a machine the process dominates, while for **small containers and high-density hosting Workstation GC can be more performant**, and their own sample shows the working set dropping from ~500 MB to ~70 MB on the switch. Decide it per host role rather than once for the solution.',
      '**DATAS is on by default from .NET 9**, which changes the shape of the question. It sizes the heap to the application rather than to the machine, starting at one heap and adding heaps under load — so the same image behaves similarly on a small container and a large one. Microsoft measures ~80% working-set improvement against a 2–3% throughput cost; if you are chasing that last 2%, `System.GC.DynamicAdaptationMode: 0` turns it off, and you own the consequences.',
      'GC settings are read **once, at process start**, and are per process — so they belong in the image or the host\'s environment, never at machine level. `System.GC.HeapHardLimitPercent` bounds the heap, and for a small process Microsoft suggests **raising** `System.GC.HighMemoryPercent` rather than lowering it, because a 1 GB process can run comfortably with less headroom than a 64 GB one.',
      'A Redis **cluster** is not a drop-in replacement for a single instance — the platform uses operations whose semantics change across slots. Scale Redis up before you scale it out.',
      'Sticky sessions belong on hosts serving the Commerce Manager UI and nowhere else. On stateless request hosts they defeat the load balancer for no benefit.',
      'Two levels quietly refuse to scale out at all: `LuceneSearch` and `FileSystemAssets`. Both work on one instance and break on the second, which is why the provider swap is part of scaling rather than a later cleanup.'
    ],
    docs: [
      { label: 'Scalability options (S · M · L · XL)', page: 'Fundamentals/Scalability/scalability-options' },
      { label: 'Scaling configuration on Azure', page: 'Fundamentals/Scalability/scaling-configuration-on-azure-cloud' },
      { label: 'Essential caching', page: 'Fundamentals/Caching/01-overview' },
      { label: 'Indexed search overview', page: 'Fundamentals/Indexed-Search/overview' },
      { label: 'Memory and GC in ASP.NET Core (Microsoft)', href: 'https://learn.microsoft.com/aspnet/core/performance/memory' },
      { label: 'GC runtime configuration (Microsoft)', href: 'https://learn.microsoft.com/dotnet/core/runtime-config/garbage-collector' },
      { label: 'DATAS — heap sized to the app (Microsoft)', href: 'https://learn.microsoft.com/dotnet/standard/garbage-collection/datas' },
      { label: 'Workstation vs Server GC (Microsoft)', href: 'https://learn.microsoft.com/dotnet/standard/garbage-collection/workstation-server-gc' }
    ],
    seeAlso: ['host-composition', 'module-database', 'cross-module-references', 'redis-cache-bus', 'distributed-lock', 'background-jobs', 'search'],
    molecule: 'deployment',
    verifiedAgainst: '3.1059.0'
  }

];
