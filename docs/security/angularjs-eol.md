# AngularJS 1.8 — accepted `npm audit` findings

## Summary

The Virto Commerce admin SPA in `src/VirtoCommerce.Platform.Web/wwwroot` is built on **AngularJS 1.8.3**, the framework's final release. AngularJS has been officially **End-of-Life since 2021-12-31**. Any security advisory published against `angular` after that date will **not** be patched by upstream, and there is no drop-in replacement inside the AngularJS 1.x line.

The advisories listed below (12 CVEs against the `angular` package plus 9 effect-only rows against `angular-*` peers that only exist because they depend on `angular`) are therefore explicitly **accepted** and suppressed from routine `npm audit` output via [src/VirtoCommerce.Platform.Web/.nsprc](../../src/VirtoCommerce.Platform.Web/.nsprc), read by `better-npm-audit`.

## Compensating controls

- The AngularJS code is loaded **only in the authenticated admin SPA**. There is no public-facing surface that renders AngularJS templates against untrusted user input.
- The public storefront runs on Vue 3 and is not affected by any of these advisories.
- All rendered content in admin blades comes from operators with platform-admin permissions — the same trust boundary as running arbitrary jobs or editing catalog data.
- The XSS/ReDoS/prototype-pollution advisories in this cluster all require an attacker to already be authenticated as a privileged user or to trick such a user into pasting attacker-controlled content into an admin form. That is a materially smaller blast radius than the same advisories against a public-facing SPA.

## The advisories

| GHSA | Severity | Title |
|---|---|---|
| GHSA-4w4v-5hc9-xrr2 | high | super-linear runtime due to backtracking |
| GHSA-89mq-4x47-5v83 | high | prototype pollution |
| GHSA-5cp4-xmrw-59wf | moderate | XSS via JQLite DOM manipulation |
| GHSA-prc3-vjfx-vhm9 | moderate | XSS in deprecated `angular` package |
| GHSA-m2h2-264f-f486 | moderate | ReDoS |
| GHSA-2vrf-hf26-jrp5 | moderate | ReDoS via `angular.copy()` |
| GHSA-2qqx-w9hr-q5gx | moderate | ReDoS via `$resource` |
| GHSA-qwqh-hm9m-p5hr | moderate | ReDoS via `<input type="url">` |
| GHSA-mhp6-pxh8-r675 | moderate | XSS |
| GHSA-j58c-ww9w-pwp5 | low | improper SVG element sanitization |
| GHSA-m9gf-397r-hwpg | low | image-source restriction bypass |
| GHSA-mqm9-c95h-x2p6 | low | image-source restriction bypass |

Effect-only rows suppressed as a consequence: `@uirouter/angularjs`, `angular-filter`, `angular-google-chart`, `angular-translate`, `angular-translate-loader-url`, `angular-translate-storage-cookie`, `angular-translate-storage-local`, `angular-ui-bootstrap`, `angular-ui-grid`. None of these packages has its own CVE; they appear only because they declare a peer/dep on the vulnerable `angular` package.

## Path forward

- **Short term:** Suppression is dated (`expiry: 2027-07-16` in `.nsprc`). When the entries expire, `better-npm-audit` will start failing on them again — that forces a scheduled review rather than a permanent forget.
- **Medium term:** If a specific advisory in this cluster becomes exploitable in our threat model (e.g., a way is discovered to feed untrusted data into admin templates), the affected entry should be removed from `.nsprc` and a targeted mitigation shipped — either via `patch-package`, a community fork such as `@code-lts/angularjs` (see Fix #6 in the audit plan), or a scoped code change.
- **Long term:** The AngularJS admin SPA is on the migration roadmap; retiring it is the only real fix for the whole cluster.

## Running audits

```
cd src/VirtoCommerce.Platform.Web
npm run audit          # better-npm-audit, respects .nsprc
npm audit              # raw npm audit, ignores .nsprc — use to see the full picture
```

Any new advisory that appears in `npm run audit` output is unallowlisted and must be triaged (either fixed, or explicitly added to `.nsprc` with a rationale and expiry).
