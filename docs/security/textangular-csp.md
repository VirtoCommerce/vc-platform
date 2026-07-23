# textAngular XSS (GHSA-7h4w-6p98-r3wx) — CSP mitigation plan

## Status

**Accepted with compensating control planned.** The advisory is allowlisted in [src/VirtoCommerce.Platform.Web/.nsprc](../../src/VirtoCommerce.Platform.Web/.nsprc) with expiry 2027-07-16.

## Why we did not remove the package

Fix #4 in [npm-adut-fix-found-eager-cocoa.md](../../../.claude/plans/npm-adut-fix-found-eager-cocoa.md) attempted to uninstall `textangular` outright. It failed cleanly for two reasons:

1. **`textAngular-sanitize` was the source of the `ngSanitize` Angular module.** Removing textAngular took ngSanitize with it, and the admin SPA cannot bootstrap without ngSanitize. This is fixable by adding `angular-sanitize` explicitly.
2. **`_AppDependencies.cshtml` compiles into `VirtoCommerce.Platform.Web.Views.dll` at publish time.** Removing `"textAngular"` from the .cshtml source has no effect until the platform is republished, so any in-place upgrade breaks Angular bootstrap. Third-party modules that still declare `['textAngular']` as a peer would also break until their own next release.

Given the ripple through third-party modules the platform can't audit, and the fact that the actual XSS is only exploitable by an authenticated operator pasting attacker-controlled markup into the admin's rich-text editor, we chose to keep the package and mitigate at a different layer.

## The mitigation — Content Security Policy

Ship a strong CSP via HTTP response headers on the admin SPA:

- `script-src 'self'` (no `'unsafe-inline'`, no `'unsafe-eval'`) — blocks any `<script>` tag or `javascript:` URL that textAngular's `<div contenteditable>` might allow through its sanitizer.
- `object-src 'none'` — no plugins / embeds.
- `base-uri 'self'` — blocks `<base>` redirection.
- `frame-ancestors 'self'` — blocks clickjacking against the admin.
- `require-trusted-types-for 'script'` — belt-and-suspenders where supported.
- `report-uri` / `report-to` — collect violations before enforcing widely.

The platform already uses `NetEscapades.AspNetCore.SecurityHeaders` (see `Microsoft.Extensions.Compliance.Abstractions.dll` in [artifacts/publish](../../artifacts/publish/)). The CSP policy should be added to the security-headers configuration and rolled out first in **Report-Only** mode until admin blades stop generating violations.

## Path forward

- **Short term:** implement CSP as above (separate PR, tracked as its own security hardening task). Once CSP is deployed and enforced, the textAngular XSS surface is closed even without patching the library.
- **Long term:** replace textAngular in `vc-module-notification` with a maintained editor (Quill / TinyMCE / plain contentEditable + explicit sanitizer). The consumer is a single blade — see [vc-module-notification/src/VirtoCommerce.NotificationsModule.Web/Scripts/blades/notifications-edit-template.tpl.html](../../../vc-module-notification/src/VirtoCommerce.NotificationsModule.Web/Scripts/blades/notifications-edit-template.tpl.html). This also lets us drop the dep entirely and delete the allowlist entry.
- **Longer term:** AngularJS admin SPA retirement — see [angularjs-eol.md](angularjs-eol.md).
