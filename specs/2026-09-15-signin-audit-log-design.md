# Sign-in audit log and login-on-behalf tracking — design

- **Date:** 2026-09-15
- **Status:** Draft, pending review
- **Scope:** `VirtoCommerce.Platform.Core`, `.Security`, `.Data.*`, `.Web`

## 1. Problem

[docs/user-guide/login-on-behalf.md](../docs/user-guide/login-on-behalf.md) states:

> All operations are strictly logged to avoid possible claims from customers.

This is not true today. Neither login-on-behalf nor ordinary sign-in produces any
persisted record. There is no way to answer "who logged in on behalf of
b2badmin@test.com, when, and from where", and no way to see failed sign-in
attempts at all.

This spec adds a durable, queryable record of every sign-in attempt —
successful and failed — including login-on-behalf, with statistics to support
daily administration.

## 2. Current state (verified)

### 2.1 Login on behalf

The admin SPA registers a toolbar command on the account detail blade
([app.js:630](../src/VirtoCommerce.Platform.Web/wwwroot/js/app/app.js:630)) which opens
`{storeUrl}/account/impersonate/{userId}`. vc-frontend re-authenticates the
operator with their password, then calls `/connect/token` with the custom
`impersonate` grant.

The backend is a single block,
[AuthorizationController.cs:309-378](../src/VirtoCommerce.Platform.Web/Controllers/Api/AuthorizationController.cs:309).
It checks `platform:security:loginOnBehalf`, builds a ticket for the *target*
user, and stamps `vc_operator_user_id` / `vc_operator_name`. Reverting is the
same grant with no `user_id`.

Attribution works: `CreatedBy` / `ModifiedBy` resolve to the operator, not the
customer, via
[HttpContextUserResolver.cs:29](../src/VirtoCommerce.Platform.Security/HttpContextUserResolver.cs:29).

The impersonate branch publishes **no domain event of any kind**.

### 2.2 Sign-in events

| Entry point | On success | On failure |
| --- | --- | --- |
| `SecurityController.Login` (admin SPA cookie) | `BeforeUserLoginEvent`, `UserLoginEvent` | nothing ([:173-177](../src/VirtoCommerce.Platform.Web/Controllers/Api/SecurityController.cs:173)) |
| `/connect/token` password grant | `BeforeUserLoginEvent`, `UserLoginEvent` | nothing ([:152-185](../src/VirtoCommerce.Platform.Web/Controllers/Api/AuthorizationController.cs:152)) |
| `/connect/token` impersonate grant | nothing | nothing |
| `/connect/token` refresh / auth-code / client-credentials | nothing | nothing |
| `ExternalSignInService` (IdP) | `BeforeUserLoginEvent`, `UserLoginEvent` | nothing |
| `ApiKeyAuthenticationHandler` | nothing (per **request**, not per session) | nothing |
| `SecurityController.Logout` | `UserLogoutEvent` | — |

`LogChangesUserChangedEventHandler` declares `IEventHandler<UserLoginEvent>` and
`IEventHandler<UserLogoutEvent>`, both registered in DI
([ApplicationBuilderExtensions.cs:39-40](../src/VirtoCommerce.Platform.Web/Security/ApplicationBuilderExtensions.cs:39)),
and both method bodies are `return Task.CompletedTask;`
([LogChangesUserChangedEventHandler.cs:55-63](../src/VirtoCommerce.Platform.Security/Handlers/LogChangesUserChangedEventHandler.cs:55)).
The wiring exists end to end and produces zero rows.

`UserLoginEvent` takes an `ApplicationUser`. The most security-relevant failure —
unknown username — has no user object, so the existing event cannot represent it.

### 2.3 Why not Active Sessions

`UserSessionsSearchService` projects over *currently valid* OpenIddict refresh
tokens
([UserSessionsSearchService.cs:25-30](../src/VirtoCommerce.Platform.Security/Services/UserSessionsSearchService.cs:25)).
Rows disappear on logout, expiry or token pruning. It is a session manager, not a
ledger. It stays in scope only for live visibility (§4.7).

### 2.4 Why not the change log

`OperationLogEntity` was evaluated and rejected for three concrete reasons:

1. **It poisons the platform cache-invalidation signal.**
   `ChangeLogService.SaveChangesAsync` ends with `Reset()` →
   `ChangeLogCacheRegion.ExpireRegion()`
   ([ChangeLogService.cs:85](../src/VirtoCommerce.Platform.Data/ChangeLog/ChangeLogService.cs:85)).
   That region also backs `ILastModifiedDateTime.LastModified`, served by the
   `[AllowAnonymous]` `/api/changes/lastmodifieddate` endpoint that external
   clients poll to decide whether to invalidate their caches. A row per sign-in
   attempt would tell every downstream consumer the platform changed,
   continuously, and flush all cached change-log searches on each write.
2. **`ObjectId` semantics break.** It holds an entity id. An unknown-username
   attempt has no id, so raw user input would end up in an id column.
3. **The useful fields have nowhere to go.** IP address is the single most
   valuable field for failed-attempt analysis; `OperationLog` has only a
   2048-char free-text `Detail`. Aggregation would mean `LIKE` scans over a table
   with no retention policy.

The change log therefore plays **no part** in this design.

## 3. Scope

### In scope

Recording sign-in attempts (success and failure) across password, external
provider, impersonation, impersonation-revert and logout paths; storing them in a
dedicated table; surfacing them through a search API, an account-blade widget, a
list blade and a statistics view; retention via a cron-scheduled cleanup job.

### Out of scope — no authentication logic changes

This spec is **purely additive**. The impersonate branch gains exactly one line
of behaviour: publishing an event. Specifically **not** included, by decision:

- Restricting which users may be impersonated
- Re-checking the operator's permission on subsequent impersonate calls
- Calling `CanSignInAsync` on the impersonation target
- Bounding impersonation session lifetime
- Customer consent / opt-in, one-time-code handoff, storefront banner,
  reason-required-before-start, customer notification (see §9)

### Residual risk to record

`platform:security:loginOnBehalf` remains equivalent to "become any user,
including an administrator", because the issued ticket is built from the target
user's full claims and nothing restricts the target
([:333-354](../src/VirtoCommerce.Platform.Web/Controllers/Api/AuthorizationController.cs:333)).
After this work that path is **logged, not prevented**. This is a deliberate
detection-over-prevention position. Nobody should later read the presence of an
audit trail as implying a control exists.

## 4. Design

### 4.1 Data model — `UserSignInLog`

New entity in **`SecurityDbContext`** (not `PlatformDbContext`: it belongs with
identity, and it keeps `ChangeLogCacheRegion` untouched). Migrations required for
all three providers: SqlServer, PostgreSql, MySql.

Migrations are generated per provider, from within each
`src/VirtoCommerce.Platform.Data.<Provider>` folder, using the command the
provider Readmes already document
([Data.SqlServer/Readme.md:22](../src/VirtoCommerce.Platform.Data.SqlServer/Readme.md:22)):

```cmd
dotnet ef migrations add <migration-name> --context SecurityDBContext
```

Requires `dotnet-ef` 10.0.10. Each provider has an `IDesignTimeDbContextFactory`
for both contexts (e.g. `SqlServerDbContextFactory`), so no extra wiring is
needed. Migrations are never hand-written.

| Column | Type | Notes |
| --- | --- | --- |
| `Id` | string | |
| `CreatedDate` | datetime | UTC |
| `UserName` | string(256) | **as typed**; the only identity field on unknown-user attempts |
| `UserId` | string(128), null | null when the account does not exist |
| `Succeeded` | bool | |
| `FailureReason` | string(64), null | see enum below |
| `SignInType` | string(32) | see enum below |
| `Provider` | string(128), null | IdP name for external sign-in |
| `OperatorUserId` | string(128), null | impersonation only |
| `OperatorUserName` | string(256), null | impersonation only, denormalized |
| `IpAddress` | string(64), null | |
| `UserAgent` | string(512), null | |
| `ClientId` | string(128), null | OAuth client |
| `SessionId` | string(128), null | OpenIddict `AuthorizationId` |
| `StoreId` | string(128), null | from `ApplicationUser.StoreId` |
| `StoreName` | string(256), null | enriched, denormalized |
| `MemberId` | string(128), null | from `ApplicationUser.MemberId` |
| `OrganizationId` | string(128), null | enriched |
| `OrganizationName` | string(256), null | enriched, denormalized |

`SignInType`: `Password`, `External`, `Impersonation`, `ImpersonationRevert`,
`ClientCredentials`, `Logout`.

`FailureReason`: `UserNotFound`, `InvalidPassword`, `LockedOut`, `NotAllowed`,
`RequiresTwoFactor`, `PasswordExpired`, `PasswordLoginDisabled`,
`DuplicateEmail`, `Forbidden`.

These are derived from the existing `SignInResult` and the existing early-return
branches, so none of them requires an authentication logic change. The exact set
is provisional: implementation should keep only the values actually reachable
from the call sites in §4.3 and drop the rest rather than persist a reason that
can never occur.

**`SessionId` is the join key to Active Sessions.** The log row says when and why;
the session list says whether it is still live and offers Terminate.

**Names are denormalized deliberately.** An audit row records what was true at
the time. If an organization is renamed, or a contact moves between
organizations, historical rows must not silently re-attribute themselves. The
same reasoning already applies to the existing `vc_operator_name` claim.

**Indexes (initial):** `(CreatedDate)`, `(UserId, CreatedDate desc)`,
`(IpAddress, CreatedDate desc)`.

Deferred until real query patterns exist: `(Succeeded, CreatedDate)`,
`(OrganizationId, CreatedDate)`, `(StoreId, CreatedDate)`. Each index is
maintenance cost on the highest-write table in the system; three is enough to
serve the statistics in §4.8 acceptably.

### 4.2 Event model

New `UserSignInAttemptEvent : DomainEvent` in
`VirtoCommerce.Platform.Core.Security.Events`, carrying the §4.1 fields (minus
the enriched ones, which are filled downstream).

`UserLoginEvent`, `BeforeUserLoginEvent` and `UserLogoutEvent` are unchanged and
continue to be published exactly as today — they are public surface that modules
subscribe to.

New handler `LogUserSignInEventHandler` subscribes only to
`UserSignInAttemptEvent`. The two empty stubs in
`LogChangesUserChangedEventHandler` stay empty; that handler keeps its single
responsibility.

### 4.3 Capture points

Publish `UserSignInAttemptEvent` at:

| Location | Cases |
| --- | --- |
| `SecurityController.Login` | success; `user == null` early return; failed `PasswordSignInAsync` |
| `SecurityController.Logout` | success |
| `/connect/token` password grant | success; user-not-found; failed sign-in; `PasswordLoginDisabled`; `DuplicateEmailException` |
| `/connect/token` impersonate grant | granted; reverted; `Forbid()`; target not found |
| `ExternalSignInService` | success; failure |

**Excluded — `ApiKeyAuthenticationHandler`.** It runs per HTTP request, not per
session; logging it would produce a row per API call.

**Excluded — refresh-token grant**, except when the refreshed token carries
`vc_operator_user_id`. Ordinary token rotation is high-volume and low-value;
extension of an impersonation session is worth a row.

### 4.4 Unknown-user attempts

A failed attempt for a username that does not exist has no `ApplicationUser`, and
therefore no store, member or organization. **These rows are left unattributed**
(`StoreId`, `MemberId`, `OrganizationId` all null), by decision. Accepting a
client-supplied `store_id` parameter was considered and rejected: it is
unvalidated client input and could not be presented as authoritative.

Consequence: per-store and per-organization failure statistics cover only
attempts against accounts that exist. The statistics view must label these
breakdowns accordingly so nobody reads a per-store failure count as complete.

### 4.5 Enrichment extension point

`StoreId` and `MemberId` come straight from `ApplicationUser`
(`ApplicationUser.StoreId` already exists in `SecurityDbContext`, 128 chars,
nullable, and is already filterable via `UserSearchService`).

**Organization is not a platform concept.** The platform knows `MemberId` and
exposes it as the `memberId` claim
([CustomUserClaimsPrincipalFactory.cs:40](../src/VirtoCommerce.Platform.Security/CustomUserClaimsPrincipalFactory.cs:40)),
but resolving a member to an organization lives in vc-module-customer.
`VirtoCommerce.Platform.Security` must not depend on a module.

```csharp
public interface IUserSignInLogEnricher
{
    int Priority { get; }
    Task EnrichAsync(UserSignInLog record);
}
```

Resolved as an ordered `IEnumerable<>`, run in priority order. The platform ships
no implementation; vc-module-customer registers one that fills
`OrganizationId` / `OrganizationName` / `StoreName`.

**Enricher failures are caught and logged, never propagated.** A broken enricher
must not block a sign-in or lose the row — the row is written with whatever
enrichment succeeded.

### 4.6 Settings

Group `Platform|Security`, following the shape of the existing descriptors in
[PlatformConstants.cs:217](../src/VirtoCommerce.Platform.Core/PlatformConstants.cs:217):

| Setting | Type | Default |
| --- | --- | --- |
| `VirtoCommerce.Platform.Security.SignInLogEnabled` | Boolean | `true` |
| `VirtoCommerce.Platform.Security.SignInLogRetentionDays` | Integer | `90` |
| `VirtoCommerce.Platform.Security.CronSignInLogCleanupJob` | Cron | `0 0 */1 * *` |

**Impersonation rows ignore `SignInLogEnabled` and are always written.** They are
roughly ten rows a day and they are the compliance anchor; a switch that silently
disables the impersonation trail is a switch that will eventually be flipped.

### 4.7 API and permissions

New permission `platform:security:sign_in_log:read`, added to
`PlatformConstants.Security.Permissions.AllPermissions`, following the
`platform:security:oauth_applications:read` naming pattern.

- `POST /api/platform/security/sign-in-log/search` → paged `UserSignInLog`
- `POST /api/platform/security/sign-in-log/stats` → §4.8 aggregates

Both behind the new permission.

**Active Sessions** gains impersonation visibility: add `IsImpersonated` and
`OperatorUserName` to `UserSession` and populate them in
`UserSessionsSearchService`. The existing Terminate command then becomes a
working kill switch for a live impersonation session, and `SessionId` links a log
row to its session.

### 4.8 Admin UI

**Widget** — new *Sign-in activity* widget on the account detail blade: a badge
with the attempt count for the selected user, opening the sign-in log blade
pre-filtered to that user. Login-on-behalf rows are visually distinct in the
list, with a quick filter chip to show only those. No change-log row is written
for impersonation.

**List blade** — searchable by user, IP, outcome, failure reason, sign-in type,
store, organization and date range.

**Statistics strip** at the top of the blade, period selector 24h / 7d / 30d.

*Tier 1 — four tiles:*

| Tile | Purpose |
| --- | --- |
| Total sign-ins + success rate, delta vs previous period | Baseline; the delta is what makes it readable at a glance |
| Failed attempts + delta | Primary anomaly signal |
| Distinct users signed in | Catches "our biggest customer stopped logging in" |
| Login-on-behalf sessions | The compliance number; front-page placement makes the feature self-policing |

*Tier 2 — four tables, top 10 each, every row a click-through to the filtered list:*

- **Top IPs by failed attempts** — credential-stuffing signal
- **Top accounts by failed attempts** — targeted brute force, or a locked-out user
  who has not called support
- **Failure reason breakdown** — separates an attack (`UserNotFound`,
  `InvalidPassword`) from an operational problem (`LockedOut`, `PasswordExpired`,
  `NotAllowed`); tells the administrator whether to call security or call the
  customer
- **Sign-ins by store / organization** — B2B view; surfaces "org X has not signed
  in for 10 days" or "everyone at org Y is failing since Tuesday". Labelled as
  covering known accounts only (§4.4)

*Tier 3 — timeline:* success vs failure, per hour (24h) or per day (7d/30d).

*Two badges in the list itself:*

- **New IP for this user** — classic account-takeover tell
- **Impersonation** — an on-behalf sign-in must never read as a normal one

*Explicit non-goal:* alerting and notifications. The blade is diagnostic.

**Implementation constraint:** all statistics are SQL aggregates against indexed
columns, cached 1–5 minutes via `IPlatformMemoryCache`. Never materialise rows —
at ~18M rows/year a naive count is a table scan.

### 4.9 Retention

Recurring Hangfire job deleting rows older than `SignInLogRetentionDays`, in
batches, registered through `RecurringJobScheduleBuilder.FromSettings(...)` with
`CronSignInLogCleanupJob` — the same pattern as the token-prune job at
[Startup.cs:555](../src/VirtoCommerce.Platform.Web/Startup.cs:555).

This is not optional. A store at 50k logins/day produces roughly 18M rows/year.

### 4.10 Write path

Two constraints, both easy to get wrong:

1. **Write after `DelayedResponse.FailAsync()`.** The password paths use
   `DelayedResponse` to equalise response timing and defeat user-enumeration
   timing attacks. A synchronous DB write on the user-found branch only would
   make it measurably slower than the user-not-found branch and reopen that side
   channel.
2. **Buffer and batch writes** (channel + background flush). `/connect/token` and
   `/api/platform/security/login` are unauthenticated. Unbatched writes turn a
   credential-stuffing run into a database amplification vector. Buffering also
   keeps the log off the sign-in latency path.

Dropped-write behaviour under buffer pressure must be explicit: bounded channel,
oldest-dropped, with a counter logged. Silent loss in an audit trail is worse
than a visible gap.

## 5. Phasing

1. **Storage and capture** — entity, three provider migrations, event, handler,
   buffered writer, capture points, `SignInLogEnabled` setting
2. **Enrichment and retention** — `IUserSignInLogEnricher`, store/member
   population, cleanup job, cron and retention settings
3. **Surfacing** — search API, stats API, permission, list blade, account widget,
   Active Sessions impersonation columns, docs rewrite

Phase 1 is independently shippable and delivers the compliance trail.

Each phase ends at a checkpoint: its unit tests pass before the next phase
starts. After phase 3, work pauses and the platform is deployed and run so the
admin UI can be verified in a browser — the widget, the list blade and the
statistics view cannot be proven by unit tests alone.

## 6. Testing

- Unit: `LogUserSignInEventHandler` maps each `SignInType` / `FailureReason`
  combination; enricher exceptions are swallowed and the row still persists;
  `SignInLogEnabled=false` suppresses ordinary sign-ins but **not** impersonation
- Unit: buffered writer flushes on batch size and on interval; bounded channel
  drops oldest and increments the counter
- Integration: each capture point in §4.3 produces exactly one row with the
  expected shape, including the `user == null` path
- Integration: impersonate grant and revert produce `Impersonation` /
  `ImpersonationRevert` rows carrying operator identity, and `SessionId` matches
  the row returned by `UserSessionsSearchService`
- Integration: migrations apply cleanly on SqlServer, PostgreSql and MySql
- Regression: `/api/changes/lastmodifieddate` is unaffected by sign-in volume —
  the defect that ruled out the change log (§2.4) must not reappear

Conventions: xUnit + Moq + FluentAssertions.

**Unit tests are the primary verification gate.** Every phase in §5 completes
only when its tests pass — no phase is reported done on the strength of a
successful build.

Placement follows the existing layout:

| Subject | Project |
| --- | --- |
| `LogUserSignInEventHandler`, buffered writer, enrichers | `tests/VirtoCommerce.Platform.Tests/Security/` |
| Capture points in `SecurityController` / `AuthorizationController` | `tests/VirtoCommerce.Platform.Web.Tests/` |

`tests/VirtoCommerce.Platform.Web.Tests/Security/SecurityMockHelper.cs` already
provides `UserManager` / `SignInManager` mocks and should be reused rather than
re-invented.

Browser verification of the admin UI (widget, list blade, statistics) happens
after phase 3, against a running platform — see the checkpoint in §5.

## 7. Documentation

[docs/user-guide/login-on-behalf.md](../docs/user-guide/login-on-behalf.md)
currently claims "All operations are strictly logged." After this work that
becomes true. The page needs rewriting to describe the permission model, where
the trail lives, and — honestly — what the feature does **not** restrict (§3).

## 8. Related findings not addressed here

Found during review, deliberately left out of scope, recommended as separate work:

- **`changelog/search` and `changelog/v2/search` carry no permission** beyond
  `[Authorize]`
  ([ChangeLogController.cs:105-121](../src/VirtoCommerce.Platform.Web/Controllers/Api/ChangeLogController.cs:105)).
  Any authenticated user can read the entire change log. Pre-existing, unrelated
  to this feature, worth a one-line fix.
- The four impersonation guardrails listed in §3, and the residual risk recorded
  there.

## 9. Possible follow-up

Parity with the mature implementations surveyed (Adobe Commerce, SAP ASM, Oracle)
would add: customer consent / opt-in, one-time-code handoff, a storefront banner
contract, reason-required-before-start, and customer notification. All touch
vc-frontend and vc-module-customer, and belong in their own spec.

## 10. References

- [Provide shopper assistance — Adobe Commerce](https://experienceleague.adobe.com/en/docs/commerce-admin/customers/customer-accounts/manage/login-as-customer)
- [LoginAsCustomerLogging module reference](https://developer.adobe.com/commerce/php/module-reference/module-login-as-customer-logging)
- [Assisted Service Module — SAP/Spartacus](https://sap.github.io/spartacus-docs/2.x/asm/)
- [Sign-in logs in Microsoft Entra ID](https://learn.microsoft.com/en-us/entra/identity/monitoring-health/concept-sign-ins)
- [Quickstart: analyze a failed sign-in attempt — Entra](https://learn.microsoft.com/en-us/entra/identity/monitoring-health/quickstart-analyze-sign-in)
- [Review an Impersonation Audit Record — Oracle](https://docs.oracle.com/en/cloud/saas/sales/faprm/review-an-impersonation-audit-record.html)
