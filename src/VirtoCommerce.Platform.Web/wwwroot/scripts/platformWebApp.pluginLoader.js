// Backoffice modularity client-side loader.
//
// Calls GET /api/apps/platform/manifest to fetch the topologically ordered,
// permission-filtered set of legacy AngularJS module bundles, then injects the
// corresponding <script> and <link rel="stylesheet"> elements into the page.
//
// AngularJS auto-bootstrap is suspended via `NG_DEFER_BOOTSTRAP!` (set inline in
// _Layout.cshtml before angular.js parses) so we can wait until every plugin
// script has loaded — and pushed its AngularJS module name onto the shared
// `AppDependencies` array — before AngularJS resolves dependencies on the
// `platformWebApp` module.
//
// On any failure we still call angular.resumeBootstrap() so the platform shell
// at least renders an error state instead of staying frozen on a blank page.
//
// LOCATION: this file MUST live outside `wwwroot/js/`. The platform's webpack
// build globs every `.js` under `wwwroot/js/**` into `dist/app.js`; including
// this loader there causes a double execution (once via the bundle, once via
// the explicit <script> tag in _Layout.cshtml), each call hitting
// /api/apps/platform/manifest and the second resumeBootstrap() throwing
// `[ng:btstrpd] App already bootstrapped`.
//
// See docs/developer-guide/backoffice-modularity-framework.md for the full flow.

(function () {
    'use strict';

    // Idempotency guard. The IIFE may be evaluated more than once if the file
    // accidentally ends up in two places (e.g. a stale `dist/app.js` bundle
    // containing this loader plus the standalone <script> tag). Short-circuit
    // on the second run so we don't fire a duplicate fetch or a second
    // resumeBootstrap() that AngularJS would reject with `[ng:btstrpd]`.
    if (window.__platformWebApp_pluginLoaderRan) {
        return;
    }
    window.__platformWebApp_pluginLoaderRan = true;

    var ENDPOINT = '/api/apps/platform/manifest';
    var TYPE_SCRIPT = 'script';
    var TYPE_STYLE = 'style';

    fetch(ENDPOINT, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } })
        .then(function (response) {
            if (!response.ok) {
                throw new Error(`GET ${ENDPOINT} returned HTTP ${response.status}`);
            }
            return response.json();
        })
        .then(loadPlugins)
        .catch(function (err) {
            // Log and continue: better to bootstrap an empty shell than to hang forever.
            // The shell will surface the failure via its normal error channels.
            console.error('[platformWebApp.pluginLoader] failed to load plugins:', err);
            resumeAngularBootstrap();
        });

    function loadPlugins(payload) {
        var plugins = (payload && payload.plugins) || [];

        // 1) Inject stylesheets in parallel — order doesn't matter for correctness.
        plugins.forEach(function (plugin) {
            collectFilesByType(plugin, TYPE_STYLE).forEach(injectStylesheet);
        });

        // 2) Inject scripts sequentially. Order matters: each plugin's script
        //    pushes its AngularJS module name onto the shared AppDependencies
        //    array; the topological order returned by the server must be
        //    preserved so dependent modules find their prerequisites.
        var scripts = [];
        plugins.forEach(function (plugin) {
            scripts.push.apply(scripts, collectFilesByType(plugin, TYPE_SCRIPT));
        });

        return scripts
            .reduce(function (chain, file) {
                return chain.then(function () {
                    return injectScript(file);
                });
            }, Promise.resolve())
            .then(resumeAngularBootstrap);
    }

    function collectFilesByType(plugin, type) {
        var out = [];
        if (plugin.entry && plugin.entry.type === type) {
            out.push(plugin.entry);
        }
        (plugin.contentFiles || []).forEach(function (cf) {
            if (cf && cf.type === type) {
                out.push(cf);
            }
        });
        return out;
    }

    function withCacheBust(file) {
        if (!file || !file.path) {
            return null;
        }
        return file.hash ? `${file.path}?v=${encodeURIComponent(file.hash)}` : file.path;
    }

    function injectStylesheet(file) {
        var href = withCacheBust(file);
        if (!href) {
            return;
        }
        var link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = href;
        document.head.appendChild(link);
    }

    function injectScript(file) {
        return new Promise(function (resolve) {
            var src = withCacheBust(file);
            if (!src) {
                resolve();
                return;
            }
            var script = document.createElement('script');
            script.src = src;
            script.type = 'text/javascript';
            // async=false on dynamically-created scripts preserves execution order
            // when multiple are appended in sequence (ES spec / WHATWG).
            script.async = false;
            script.onload = function () {
                resolve();
            };
            script.onerror = function () {
                // Don't block remaining plugins — log and continue. AngularJS will
                // surface the missing dependency as a clearer error than a frozen page.
                console.error('[platformWebApp.pluginLoader] failed to load script:', src);
                resolve();
            };
            document.head.appendChild(script);
        });
    }

    function resumeAngularBootstrap() {
        var ng = window.angular;
        if (!ng || typeof ng.resumeBootstrap !== 'function') {
            return;
        }
        // Defense in depth: if AngularJS already bootstrapped this element
        // (e.g. due to a duplicate loader execution slipping past the guard above,
        // or if NG_DEFER_BOOTSTRAP! was never honored), calling resumeBootstrap
        // a second time throws [ng:btstrpd]. Detect and skip cleanly.
        try {
            var rootEl = ng.element(document.documentElement);
            if (rootEl && typeof rootEl.injector === 'function' && rootEl.injector()) {
                return;
            }
        } catch (_) {
            // jqLite may not be ready yet — proceed with resumeBootstrap, which
            // will throw if there's a real conflict.
        }
        ng.resumeBootstrap();
    }
})();
