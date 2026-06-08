# Background Processing Hub — Design Spec

**Status:** Draft for review
**Date:** 2026-06-06
**Author:** Oleg Zhuk
**Ticket:** VCST-5208 (follow-on)
**Source vision:** `background-processing-hub-overview-v7.html`

---

## 1. Goal

Remove Hangfire as a **hard, welded-in dependency** of the Virto Commerce platform and replace it with a
provider-agnostic **Background Processing Hub**:

- The platform depends only on **contracts** (a new `VirtoCommerce.Platform.BackgroundJobs` project — working
  name; the original suggestion was `VirtoCommerce.BackgroundJob`). The platform assembly never references
  `Hangfire.*` again.
- Job **engines** live in installable modules behind one `IJobEngine` port, selected by configuration —
  exactly the way search providers (Elasticsearch / Lucene / Azure Search) work today: **one active engine at
  a time, chosen in config.**
- Hangfire ships as the **reference engine** in a new `VirtoCommerce.Hangfire` module (the migrated
  `VirtoCommerce.Platform.Hangfire` project, still published as a NuGet package). RabbitMQ ships as the second
  engine (`VirtoCommerce.RabbitMQ`) to prove the abstraction against a push/scale-to-zero model.

"Without breaking changes" is defined precisely in §7. It is **not** binary compatibility for third-party
DLLs; it is **behavioural** compatibility once an engine module is installed, plus a clear, guided migration
path for modules that call Hangfire directly.

---

## 2. Problem — Hangfire has leaked, and the platform can't boot without it

Grounded in the current `vc-platform` source:

| Coupling point | Where | What it uses |
|---|---|---|
| Recurring-job facade | `src/VirtoCommerce.Platform.Hangfire/RecurringJobService.cs` | Hangfire `IRecurringJobManager`, `RecurringJobOptions` |
| Recurring-job contract | `src/VirtoCommerce.Platform.Hangfire/IRecurringJobService.cs` | published type other modules depend on |
| Platform recurring jobs | `src/VirtoCommerce.Platform.Web/Security/ApplicationBuilderExtensions.cs:57-84` | `IRecurringJobService.WatchJobSetting`, `RecurringJob.AddOrUpdate<AutoAccountLockoutJob>` |
| **Module install/uninstall** | `src/VirtoCommerce.Platform.Web/Controllers/Api/ModulesController.cs:535` | `BackgroundJob.Enqueue(() => ModuleBackgroundJob(...))` |
| Hangfire server registration | `src/VirtoCommerce.Platform.Web/Program.cs:114-116` | `AddHangfireServer`, gated on `VirtoCommerce:Hangfire:UseHangfireServer` |
| Storage + dashboard + DI | `src/VirtoCommerce.Platform.Hangfire/Extensions/*.cs` | SqlServer/Postgres/MySql/Memory storage, `/hangfire` dashboard, console |
| DevTools entry | `ApplicationBuilderExtensions.UseHangfire` | registers `/hangfire` in `IDeveloperToolRegistrar` |
| Jobs REST API | `src/VirtoCommerce.Platform.Web/Controllers/Api/JobsController.cs` | Hangfire monitoring API |

**The load-bearing constraint:** the platform's own module-management runs *as a background job*
(`ModulesController.cs:535`), and the platform's recurring maintenance jobs (token pruning, account lockout)
require `IRecurringJobService`. So the platform currently **requires a job engine present at boot**. Any
extraction must answer "what happens when no engine is installed?" — see §6.

Beyond the platform, the same patterns are reinvented against Hangfire across modules: Import
(`BackgroundJob.Enqueue`, `PerformContext`, cancellation), Bulk Actions (a duplicate `IBackgroundJobExecutor`),
Subscription (`[DisableConcurrentExecution]`), Indexing. These are out of scope for the platform repo but
inform the contract design.

---

## 3. Principles

1. **Platform is passive.** It enqueues and orchestrates; engines execute. It must be able to run as nothing
   but a callback target.
2. **Depend on contracts, not engines.** No platform code and no future module references `Hangfire.*` or any
   provider type — they reference the Hub contracts.
3. **Messages, not delegates.** Work crosses every boundary as a serialized envelope (see §5).
4. **One port, many adapters.** Hexagonal — the same pattern as cache, search, and storage providers.
5. **Idempotency is a default.** Push delivery is at-least-once; dedup keys and idempotent handlers are built in.
6. **Observe once, everywhere.** One OpenTelemetry model across every engine and boundary.
7. **Claim-check large data.** Store the blob, pass a reference (imports already work on file URLs).
8. **Backward compatible first.** Ship the abstraction with Hangfire as the only engine and zero behaviour
   change, then add engines.

---

## 4. Target architecture

```
┌─ PLATFORM — zero Hangfire reference ─────────────────────────────────┐
│ VirtoCommerce.Platform.BackgroundJobs           (NEW platform project)│
│   Contracts:                                                          │
│     IJobEngine            — the provider port (one active at a time)  │
│     IBackgroundJob<T>     — handler, resolved from DI, overridable    │
│     IBackgroundJobProcessor — enqueue / schedule facade (messages)    │
│     IRecurringJobService  — moved here as a contract (was in Hangfire)│
│     IJobDispatcher        — shared execution path (deserialize→run)   │
│     JobEnvelope, EnqueueOptions, RecurringJobEnvelope, Cron builder   │
│     IBackgroundProcessingHealth, IJobProgress                         │
│   Behaviour:                                                          │
│     NoEngineBackgroundJobProcessor — active when no IJobEngine is     │
│       registered; surfaces a guided "install an engine via vc-build"  │
│       message instead of silently dropping work (see §6)              │
│   Hooks (contracts only): DevTools descriptor, Jobs admin API surface │
│                                                                       │
│ Migrated platform consumers (no Hangfire types):                      │
│   ModulesController, PruneExpiredTokensJob, AutoAccountLockoutJob     │
└───────────────────────────────────────────────────────────────────────┘
                  │  config: BackgroundProcessing:Provider = "Hangfire" | "RabbitMQ"
        ┌─────────┴──────────┐
┌───────▼────────────┐  ┌────▼─────────────────┐
│ vc-module-hangfire │  │ vc-module-rabbitmq   │   only one active,
│ = VirtoCommerce.   │  │ = VirtoCommerce.     │   like search providers
│   Hangfire         │  │   RabbitMQ           │
│  (ex Platform.     │  │  IJobEngine (push/   │
│   Hangfire project,│  │   pull), KEDA-ready  │
│   still a NuGet)   │  │                      │
│  IJobEngine (pull) │  │  DevTools reg,       │
│  Hangfire storage, │  │  DLQ, delayed-msg    │
│   dashboard,       │  │                      │
│   console, server  │  │                      │
│  + expression-style│  │                      │
│   Enqueue sugar    │  │                      │
│   for legacy users │  │                      │
└────────────────────┘  └──────────────────────┘
        ▲                          ▲
        │  later engines behind the same port (Phase 4/5):
        └── Cloud Run / ACA Jobs (push) · Temporal (durable, optional)
```

**Project / repo naming (open — see §13):**
- Platform project: `VirtoCommerce.Platform.BackgroundJobs` (vs. the original `VirtoCommerce.BackgroundJob`).
- Engine modules: `VirtoCommerce.Hangfire` (repo `vc-module-hangfire`), `VirtoCommerce.RabbitMQ`
  (repo `vc-module-rabbitmq`).

---

## 5. The load-bearing decision — messages, not delegates (Hybrid contract)

**Decision (locked):** the Hub's first-class contract is **message/payload based**; expression-style
`Enqueue(() => svc.Method(args))` sugar lives **only inside the Hangfire module**, for legacy and direct users.

Why hybrid and not the alternatives:
- A pure `Expression<Func<T,Task>>` contract in Core would re-export Hangfire-shaped coupling under a new name —
  RabbitMQ and push engines cannot honour it. Rejected.
- Pure messages-only with no bridge anywhere leaves direct-`BackgroundJob.Enqueue` modules with no landing
  spot. Rejected as the *whole* answer.
- Hybrid keeps Core engine-agnostic (RabbitMQ-ready) **and** gives direct-Hangfire modules a real home (the
  Hangfire NuGet) without polluting the platform.

### Core contracts (illustrative)

```csharp
// Handler — overridable type, DI-resolved, partner-extensible (last registration wins)
public interface IBackgroundJob<TPayload>
{
    Task ExecuteAsync(TPayload payload, JobExecutionContext context, CancellationToken cancellationToken);
}

// Facade used by module/platform code
public interface IBackgroundJobProcessor
{
    Task<string> EnqueueAsync<TPayload>(TPayload payload, EnqueueOptions options = null, CancellationToken cancellationToken = default);
    Task<string> ScheduleAsync<TPayload>(TPayload payload, TimeSpan delay, EnqueueOptions options = null, CancellationToken cancellationToken = default);
}

// The provider port — one active implementation, chosen by config
public interface IJobEngine
{
    string ProviderName { get; }
    JobEngineKind Kind { get; }            // Pull · Push · Workflow
    Task<string> SubmitAsync(JobEnvelope envelope, SubmitOptions options, CancellationToken cancellationToken = default);
    Task ScheduleRecurringAsync(RecurringJobEnvelope e, CancellationToken cancellationToken = default);
    Task RemoveRecurringAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string jobId, CancellationToken cancellationToken = default);
    Task<JobEngineHealth> GetHealthAsync(CancellationToken cancellationToken = default);
}

// The serialized unit of work — a message, never a delegate
public sealed record JobEnvelope
{
    public required string JobType { get; init; }
    public required string PayloadJson { get; init; }
    public required string PayloadType { get; init; }     // concrete (AbstractTypeFactory) type
    public string? UniqueKey { get; init; }               // at-least-once → collapse duplicates
    public IReadOnlyDictionary<string,string> Headers { get; init; } // correlation · trace · tenant
    public int Attempt { get; init; } = 1;
}

// Shared execution path — pull engines call it in-process; push engines call it from the callback controller
public interface IJobDispatcher
{
    Task DispatchAsync(JobEnvelope envelope, JobExecutionContext context, CancellationToken cancellationToken);
}
```

### Partner extensibility (uses the tools partners already know)
- **Payload** is created via `AbstractTypeFactory<TPayload>.TryCreateInstance()` (never `new`) so a partner can
  `OverrideType<Base, AcmeDerived>()`; the envelope carries the concrete `PayloadType` so the derived type
  round-trips through the broker. A `DynamicProperties` bag covers no-code light extension.
- **Handler** is a DI-registered interface — partners **replace** (last registration wins), **decorate**,
  **subclass + `base.ExecuteAsync`**, or compose an **`IJobMiddleware<T>`** pipeline.
- **Guards:** payloads stay serializable and **additive** (add fields, never remove — a job may sit in a queue
  across a deploy).

### Recurring jobs
`IRecurringJobService` moves into the platform `BackgroundJobs` project as a **contract** (it was previously a
Hangfire type). A fluent `Cron` builder (`Cron.Daily().At(2,30)`, `.Describe()`, `.Next(3)`) plus an
`OverlapPolicy` (`Allow` / `Skip` / `Queue` / `Cancel`) are enforced **centrally** by the Hub via a distributed
lock keyed on the job id — so overlap protection holds across the whole worker fleet, not just one process
(stronger than Hangfire's `[DisableConcurrentExecution]`). The setting-driven variant
(`AddOrUpdateSettingDriven<T>`) preserves today's enabler-setting + cron-setting + live re-registration on
`ObjectSettingChangedEvent`.

---

## 6. No engine installed — behaviour and the chicken-and-egg

The platform must **boot and remain usable with no engine module installed.** When no `IJobEngine` is
registered, the DI container binds `NoEngineBackgroundJobProcessor`, which:

- Does **not** silently no-op. Any enqueue/schedule call throws / returns a typed result carrying a clear,
  actionable message: *"No background-job engine is installed. Install one with the Virto Commerce CLI, e.g.
  `vc-build install -Module VirtoCommerce.Hangfire`, then set `BackgroundProcessing:Provider`."*
- Surfaces the same guidance in the admin UI where a job would have been started, and in the Jobs blade /
  DevTools.

**Chicken-and-egg resolution.** Today module install runs as a Hangfire job (`ModulesController.cs:535`). After
extraction:
- The **first** engine module is installed via the **`vc-build` CLI**, which copies module files to disk
  directly — it does not require a running background job. (This is already how the CLI bootstraps a platform.)
- The UI-driven "install module" flow, which *does* enqueue a background job, detects the no-engine state and
  shows the CLI hint instead of enqueuing into nothing. Once an engine is installed and active, UI-driven
  module management works again unchanged.
- `ModulesController.cs:326` already contains a precedent fallback ("can't use `BackgroundJob.Enqueue` because
  Hangfire tables might be missing in new DB") — the no-engine path generalises that existing guard.

---

## 7. Backward compatibility & migration

**What "no breaking changes" means here:**

| Audience | Guarantee |
|---|---|
| Default distribution | Ships with `VirtoCommerce.Hangfire` installed + active by default → **zero behavioural change** out of the box. |
| Modules using the Hub contracts | Work unchanged across any engine. |
| Modules calling Hangfire directly (`BackgroundJob.Enqueue(() => UpdateProductsAsync(ids))`) | **Not binary-compatible.** They reference the migrated `VirtoCommerce.Hangfire` NuGet and must **install + enable the Hangfire engine module**. We recommend migrating to the Hub contracts. The expression-style sugar in the Hangfire module gives them a working bridge meanwhile. |
| Platform internal consumers | Migrated in this work (see §8). |

**Migration of `VirtoCommerce.Platform.Hangfire` → `VirtoCommerce.Hangfire`:**
- Move the entire project (RecurringJobService impl, storage extensions, dashboard, console,
  `HangfireAuthorizationHandler`, `HangfireUserContextMiddleware`, `JobCancellationTokenWrapper`,
  `SettingCronJob`/`SettingCronJobBuilder` implementation, `HangfireOptions`) into the new module.
- Keep it **packable as a NuGet** so direct users have a dependency to reference.
- The module implements `IJobEngine` (pull/`Kind=Pull`), registers its DevTools `/hangfire` entry, the
  `JobsController` monitoring surface, and the expression-style `Enqueue` sugar.
- The platform's `Program.cs` / `Startup.cs` lose `AddHangfireServer` / `UseHangfire`; engine bootstrap moves
  into the module's `Module.cs` `Initialize`/`PostInitialize`.

---

## 8. In-repo (vc-platform) consumer migration — Phase 1 scope

| Consumer | Today | After |
|---|---|---|
| `ModulesController.ModuleBackgroundJob` | `BackgroundJob.Enqueue(() => ModuleBackgroundJob(options, notification))` | `EnqueueAsync(new ModuleManagementPayload{...})` + an `IBackgroundJob<ModuleManagementPayload>` handler; no-engine guard shows CLI hint |
| `PruneExpiredTokensJob` | `IRecurringJobService.WatchJobSetting(... ToJob<PruneExpiredTokensJob>(x => x.Process()))` | same `IRecurringJobService` (now a Core contract), setting-driven recurring registration |
| `AutoAccountLockoutJob` | `RecurringJob.AddOrUpdate<AutoAccountLockoutJob>("id", j => j.Process(cancellationToken), cron)` | `IRecurringJobService` recurring registration via the Hub |
| `JobsController` | Hangfire monitoring API in platform Web | moves to the Hangfire module (engine-specific monitoring) or reads engine-agnostic Hub telemetry |
| `/hangfire` DevTools entry | registered in platform | registered by the engine module |

---

## 9. Phased delivery

The phases are independently shippable; **each gets its own implementation plan.** Phase 1 alone satisfies the
original "extract Hangfire without breaking changes" ask.

| Phase | Deliverable | Outcome |
|---|---|---|
| **1** | `VirtoCommerce.Platform.BackgroundJobs` contracts + `NoEngineBackgroundJobProcessor`; migrate in-repo consumers; move `Platform.Hangfire` → `VirtoCommerce.Hangfire` module (NuGet); config-based engine selection; CLI hint. | **Zero behaviour change; Hangfire dependency removed from platform.** |
| **2** | Engine-independent progress (`IJobProgress` over SignalR) + OpenTelemetry health; Jobs admin blade + DLQ inspector + recurring-schedule view. | One operational model; Hangfire-dashboard replacement. |
| **3** | Event→job binder on the in-process bus (e.g. loyalty/email on order-created). | Non-blocking order side-effects. |
| **4** | `VirtoCommerce.RabbitMQ` push engine (durable queues, DLX, delayed-message plugin), KEDA scale-to-zero (YAML, outside the engine), push callback API, Map/Reduce coordinator. | Scale-to-zero; enterprise fan-out. |
| **5** | Optional Temporal / Durable Functions engine. | Durable, multi-step workflows. |

### Phase 4 notes (RabbitMQ default + push)
- RabbitMQ is the **in-the-box** broker (ships in Docker): durable queues, per-message ACK/NACK, dead-letter
  exchange, `x-delay` delayed messages, management plugin for health + KEDA queue-depth reads.
- **KEDA is a scaler, not a transport** — `SubmitAsync` is byte-for-byte identical with or without it. The one
  real code change is hardening consumer shutdown (SIGTERM mid-job must NACK-and-requeue, never drop).
- Multi-tenant: one shared cluster partitioned per tenant by **vhost**; one KEDA `ScaledObject` per tenant
  scales that tenant's workers independently with per-tenant max-replica caps.
- Push round-trip: platform enqueues an envelope + callback URL → external runtime scales from zero → generic
  relay container (zero business logic) POSTs back to the platform callback → `IJobDispatcher` runs the real
  handler in-process under an idempotency guard (`5xx` ⇒ provider retries).
- Map/Reduce: `IMapReduceJob<TPartition,TResult>` with `PartitionAsync` / `MapAsync` / `ReduceAsync`; fan-out
  publishes N partition messages, an atomic per-batch completion counter (SQL/Redis) trips the single reduce
  on the last ACK (e.g. blue-green index alias swap).

---

## 10. Observability (Phase 2+)

One source (OpenTelemetry emitted by the Hub — `job.duration`, `queue.lag`, `retry.count`), two audiences:
- **Global / SRE:** Prometheus (RabbitMQ + KEDA + worker metrics) → Grafana, Alertmanager (backlog/DLQ alerts),
  Jaeger/Tempo traces spanning enqueue → dispatch → execute → callback.
- **Tenant admin (inside the VC admin):** reuse the existing push-notification/SignalR progress UI; **new
  blades** for a queryable jobs list (filter by state/queue), DLQ inspect & replay, and recurring-schedule
  management. Verify before building whether a newer VC-Shell already ships a jobs/operations-history screen to
  extend.

---

## 11. Risks

- **Payload versioning.** Jobs can sit in a queue across a deploy. Keep payloads additive, version the
  envelope, and for Hangfire's pull model keep enqueuer and worker code in lockstep.
- **Silent no-engine drops.** Mitigated by `NoEngineBackgroundJobProcessor` raising a guided error rather than
  no-oping (§6).
- **Two enqueue styles during transition.** Accepted and explicitly scoped to the Hangfire module.
- **Cancellation.** Recent work migrated `ICancellationToken` → `CancellationToken`; the Hub standardises on
  `CancellationToken` end-to-end.

---

## 12. Out of scope

- Rewriting third-party modules (Import, Bulk Actions, Subscription, Indexing) — they migrate on their own
  cadence; the contracts are designed to receive them.
- Changing the storefront (vc-frontend) — background processing is backend/admin only.

---

## 13. Open questions

| # | Question | Status |
|---|---|---|
| Q1 | Payload contract: messages vs delegates | **Resolved — Hybrid (§5).** |
| Q2 | DX surface for v1: which sugar (self-describing request, fluent builder, attribute defaults) to commit vs defer? | Open |
| Q3 | Handler registration: source generator (compile-time serializability checks) vs runtime assembly scan? | Open |
| Q4 | First push provider: ACA Jobs vs Cloud Run? | Open (Phase 4) |
| Q5 | Idempotency store: SQL dedup ledger (reuse platform DB) vs Redis? Retention window? | Open |
| Q6 | Temporal in v1 or out? First module to migrate: Import (complex, high value) vs Subscription (simple, low risk)? | Open |
| Q7 | Final naming: `VirtoCommerce.Platform.BackgroundJobs` vs `VirtoCommerce.BackgroundJob`; module repo names. | Open |
```
