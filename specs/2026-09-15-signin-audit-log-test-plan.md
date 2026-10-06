# Sign-in audit log — test checklist

Scope: PR #3117. Unit tests cover the mapping, the query shapes and the publish points; everything
here is what unit tests cannot answer — real OpenIddict token flows, a real identity provider, a real
database under load, and the back office in a browser.

## Test first, because it is unverified end to end

These three carry the most risk. Nothing else in the plan matters if the first one fails.

| # | Why it is risky |
|---|---|
| A1 | The impersonate grant now **creates the OpenIddict authorization itself** and attaches it to the ticket, relying on OpenIddict honouring a pre-attached id instead of creating its own ad-hoc one. Verified by reading OpenIddict's authorization-code path, never executed against a live token endpoint. |
| A2 | The refresh grant now **pins the rotated token to the incoming authorization**. This changes token storage behaviour for every session in the system, not just impersonated ones. |
| A3 | `Host` is read from `HttpRequest.Host`. Correct behind a reverse proxy only when forwarded-headers handling is configured. |

---

## 1. Environment

- [ ] Migrations applied cleanly on **SqlServer**, **PostgreSql**, **MySql** (each from a database that already has data)
- [ ] MySql only: confirm `OpenIddictTokens/Authorizations/Applications/Scopes` column widths are **unchanged** (still `varchar(255)`) — the migration deliberately excludes that drift
- [ ] `UserSignInLog` table has four indexes: `CreatedDate`, `(UserId, CreatedDate)`, `(IpAddress, CreatedDate)`, `SessionId`
- [ ] SPA rebuilt (`npm run webpack:build`) and platform restarted — localization strings are cached server-side
- [ ] Settings present under **Platform > Sign In Log** with defaults: recording on, retention 30, cleanup job on, cron `0 1 */1 * *`
- [ ] Rollback: `Down` drops only `UserSignInLog`

## 2. Recording coverage

One row per attempt, no duplicates, no missing rows. Check the row's fields, not just its existence.

**Back office (cookie sign-in)**
- [ ] Successful sign-in → `Password` / succeeded
- [ ] Wrong password → `InvalidPassword`
- [ ] Unknown user name → row written, `UserId` null, `UserName` preserved as typed
- [ ] Locked-out account → `LockedOut`
- [ ] Sign-out → `Logout`

**Token endpoint — password grant**
- [ ] Success, wrong password, unknown user, locked out
- [ ] Password login disabled for a non-administrator → `PasswordLoginDisabled`
- [ ] Duplicate email (with `RequireUniqueEmail`) → `DuplicateEmail`
- [ ] **Expired password** (token request validator rejects after a correct password) → `NotAllowed`, **not** `InvalidPassword`

**Refresh grant**
- [ ] Ordinary refresh → **no** row (deliberate: high volume, low value)
- [ ] Refresh of an impersonated session → one `Impersonation` row per refresh, operator preserved

## 3. SSO / external identity providers

Run against at least one real provider (Azure AD / Auth0 / Okta), not a stub.

- [ ] First-time SSO sign-in that **auto-creates** a platform user → `External` / succeeded, `Provider` set to the login provider, `UserId` on the new account
- [ ] Repeat SSO sign-in for an existing linked account → succeeded, same `UserId`
- [ ] SSO for a user whose account is **locked out** → `LockedOut`, no session issued
- [ ] SSO where the provider is **not linked** to any account and auto-create is off → `NotAllowed` row is written **before** the `AuthenticationException` surfaces
- [ ] SSO where no platform user can be found or created → `UserNotFound` row written before the exception
- [ ] Provider returning no user name → still fails safely (no row expected; the exception path has no identity to record)
- [ ] `Provider` column shows the provider name, and the detail blade renders it
- [ ] SSO sign-in carries `StoreId`/`MemberId` when the account has them
- [ ] Two providers configured → rows distinguish them
- [ ] SSO **into a storefront domain** → `Host` matches the domain used, not the platform's own

## 4. Login on behalf — and the Active Sessions kill switch

This is the compliance anchor. Test the whole round trip, not just the grant.

- [ ] Operator with `loginOnBehalf` impersonates a customer → `Impersonation` row: `UserName` = customer, `OperatorUserName` = operator, **`SessionId` populated** *(risk A1)*
- [ ] **Active Sessions** for that customer shows the session flagged as impersonated, with the operator's name
- [ ] **Terminate** that session → the operator's impersonated token stops working
- [ ] Let the impersonated session **refresh**, then re-check Active Sessions — the flag and operator must survive rotation *(risk A2)*
- [ ] Revert to operator → `ImpersonationRevert` row where `UserName` = **the customer being left** and `OperatorUserName` = **the real operator** (these were inverted before the fix)
- [ ] Revert row's `SessionId` matches the grant row's, so the pair can be correlated
- [ ] Chained impersonation (operator → customer A → customer B) records the original operator throughout
- [ ] User **without** `loginOnBehalf` attempts impersonation → `Forbidden` row naming the would-be operator, 403 returned
- [ ] Unauthenticated call to the impersonate grant → `Forbidden` row with null operator, 401 returned
- [ ] Impersonating a non-existent `user_id` → `UserNotFound` row, `UserId` falls back to the attempted id
- [ ] Impersonation while a token request validator rejects → `NotAllowed` row
- [ ] Ordinary (non-impersonated) session still shows **unflagged** in Active Sessions

## 5. Settings and retention

- [ ] Recording **off** → ordinary sign-ins stop being recorded
- [ ] Recording **off** → login-on-behalf rows are **still** recorded
- [ ] Recording **off** → dashboard shows the banner; the list blade does not repeat it
- [ ] Retention 1 day + cleanup job → rows older than a day are removed, newer survive
- [ ] Retention **0 or negative** → nothing is ever deleted
- [ ] Cleanup job disabled → no deletion, even with retention set
- [ ] Changed cron expression is honoured after restart
- [ ] `platform:security:sign_in_log:read` required: a user without it gets 403 from both endpoints and sees no menu entry or widget

## 6. Search, filters and the back office

- [ ] Period presets (30 min / 1 h / 6 h / 24 h / 7 d / 30 d / all time) and custom range
- [ ] Custom range including "today" returns rows written after midnight
- [ ] Outcome, type (incl. "On behalf" covering grant + revert), IP filters
- [ ] Store filter: **All** returns everything; **Any store** only rows with a store; **No store** only rows without; a named store returns only its own
- [ ] Keyword matches user name, **operator name**, and IP address
- [ ] Sorting defaults to newest first; column sort and paging work past page 1
- [ ] Grid renders `operator` **On behalf of** `customer` for impersonation rows; ordinary rows show just the user
- [ ] Row click opens the read-only detail blade; no Outcome field (the badge covers it); `Host` present
- [ ] Dashboard tiles show counts plus the previous-period arrow and percentage (no "vs prev" caption)
- [ ] Timeline granularity changes with the period; a quiet stretch renders as zeroes, not a closed gap
- [ ] Each breakdown row opens the list pre-filtered to what was clicked
- [ ] Organization panel stays hidden while no module supplies organization names
- [ ] Account detail widget shows that user's recent attempts
- [ ] Spot-check at least **ru** and one RTL-free CJK locale (**ja** or **zh**) for truncated or missing strings

## 7. Data quality and edge cases

- [ ] Over-length user agent, user name and `Host` are truncated rather than failing the batch — and the rest of the batch still persists
- [ ] IPv6 client address recorded correctly
- [ ] Deleting a user **does not** delete their audit rows
- [ ] Passwords never appear in any column
- [ ] Multi-domain: sign in via two hostnames pointing at one deployment → `Host` distinguishes them *(risk A3)*
- [ ] Behind a reverse proxy with forwarded headers configured → `Host` is the external domain, not the internal one
- [ ] Behind a proxy **without** forwarded headers → confirm the value is the internal host, and that this is acceptable/documented

## 8. Load and performance

The buffered writer exists so that recording cannot slow a sign-in or amplify an attack. This section
is the reason it exists, so it is not optional.

**Setup.** Seed the table to a realistic size (≥ 5M rows) before measuring query paths. Drive load at
`/connect/token` (password grant) and `/api/platform/security/login`. Run each scenario twice —
**recording off** for the baseline, then **recording on** — changing nothing else.

- [ ] **L1 — Steady state, log off.** Record p50/p95/p99 latency and throughput at target RPS. This is the baseline.
- [ ] **L2 — Steady state, log on.** Same load. **Pass:** p95 within a few percent of L1; no sustained growth in latency over the run.
- [ ] **L3 — Failed-login burst (credential stuffing).** High-rate failures against unknown and known user names, log on. **Pass:** no database write per attempt (writes arrive batched); latency does not degrade as the burst continues; the platform stays responsive.
- [ ] **L4 — Buffer overflow.** Drive writes past `BufferCapacity` (lower it to force this). **Pass:** `DroppedCount` increments, a warning is logged, and **sign-ins keep succeeding** — the audit log degrades, authentication does not.
- [ ] **L5 — Timing side channel.** Compare the response-time distribution of *user found + wrong password* against *user not found*, with recording **on**. **Pass:** the distributions stay indistinguishable, matching the log-off baseline. This is what `DelayedResponse` protects and what moving enrichment off the request thread preserves.
- [ ] **L6 — Enricher on the request path.** Register a deliberately slow enricher (e.g. 200 ms). **Pass:** sign-in latency is **unchanged** — enrichment runs on the flush loop. Re-run L5 with it registered.
- [ ] **L7 — Database write cost.** Measure insert throughput and table/index growth during L3. Confirm batching is actually happening (batch size ≈ configured, not one row per insert).
- [ ] **L8 — Stats endpoint at volume.** Open the dashboard against the seeded table for each period. **Note:** `GetStats` issues roughly a dozen sequential queries; record the wall-clock time per period and decide whether it is acceptable before release.
- [ ] **L9 — Active Sessions at volume.** Open Active Sessions for a user with the log at full size; confirm the `SessionId` join uses its index and does not scan.
- [ ] **L10 — Cleanup job under load.** Run retention cleanup while traffic is live. **Pass:** deletes proceed in batches without blocking sign-ins or timing out.
- [ ] **L11 — Graceful shutdown.** Stop the platform with records still buffered. **Pass:** the tail is flushed, not lost.
- [ ] **L12 — Multi-instance.** With two or more platform instances behind a load balancer, confirm rows from all instances land correctly and the cleanup job does not double-run destructively.

## 9. Sign-off

- [ ] All three databases exercised
- [ ] At least one real SSO provider exercised
- [ ] L1/L2 comparison recorded with numbers, attached to the PR
- [ ] Risks A1–A3 explicitly confirmed or raised as defects
