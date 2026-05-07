// Virto Commerce Extension-Point Inspector
//
// Console-gated developer helper that highlights Platform Manager extension points
// on the current page and copies ready-to-paste registration snippets.
//
// Usage (browser DevTools console):
//   vcExt.help()             show help
//   vcExt.show()             overlay all extension points on the current page
//   vcExt.hide()             remove the overlay
//   vcExt.toggle()           show/hide
//   vcExt.list()             console.table of every registered item across all 5 services
//   vcExt.copy(type, id?)    copy a registration snippet
//                              type: 'menu' | 'blade' | 'toolbar' | 'widget' | 'metaform'
(function (global) {
    'use strict';

    if (global.vcExt) {
        return;
    }

    var ROOT_ID = 'vc-ext-inspector-root';
    var TOAST_ID = 'vc-ext-toast';
    var DATA_VC_EXT_ATTR = 'data-vc-ext';
    var ATTR_ID = 'id';
    var ATTR_NG_CONTROLLER = 'ng-controller';
    var BLADE_HOST_SELECTOR = '[ng-model="blade"]';
    var COPY_BUTTON_TEXT = 'Copy snippet';

    var POSITION_FIXED = 'position:fixed';
    var WHITE_FG = 'color:#fff';
    var BG_PREFIX = 'background:';

    var COLOR_TOAST_OK = '#16a34a';
    var COLOR_TOAST_FAIL = '#dc2626';

    var SVC_MAIN_MENU = 'platformWebApp.mainMenuService';
    var SVC_BLADE = 'platformWebApp.bladeNavigationService';
    var SVC_WIDGET = 'platformWebApp.widgetService';
    var SVC_TOOLBAR = 'platformWebApp.toolbarService';
    var SVC_METAFORMS = 'platformWebApp.metaFormsService';

    var TYPE_COLORS = {
        menu:     { border: '#7c3aed', bg: 'rgba(124,58,237,0.12)' },
        blade:    { border: '#2563eb', bg: 'rgba(37,99,235,0.10)' },
        toolbar:  { border: '#ea580c', bg: 'rgba(234,88,12,0.10)' },
        widget:   { border: COLOR_TOAST_OK, bg: 'rgba(22,163,74,0.10)' },
        metaform: { border: '#0d9488', bg: 'rgba(13,148,136,0.10)' }
    };

    var state = {
        active: false,
        overlays: [],
        observer: null,
        rebuildTimer: null,
        copyLockUntil: 0,
        repositionLoopRunning: false,
        // Names captured live from metaFormsService.registerMetaFields. Populated
        // by the decorator below as modules call register, so we can match every
        // <va-metaform> directive to its actual registry key (e.g. 'productDetail2'
        // for the catalog item page) instead of falling back to the host:bladeId
        // placeholder.
        metaFormNames: new Set()
    };

    // Install an Angular decorator at IIFE-evaluation time so every future
    // metaFormsService.registerMetaFields(name, …) call records the form name.
    // Decorators run when the service is instantiated, before any .run() block
    // that injects it — so by the time module code calls register, this wrapper
    // is already in place.
    try {
        if (typeof angular !== 'undefined' && angular.module) {
            angular.module('platformWebApp').decorator(
                'platformWebApp.metaFormsService',
                ['$delegate', function ($delegate) {
                    var origRegister = $delegate.registerMetaFields;
                    $delegate.registerMetaFields = function (formName) {
                        if (formName) {
                            state.metaFormNames.add(formName);
                        }
                        return origRegister.apply(this, arguments);
                    };
                    return $delegate;
                }]
            );
        }
    } catch (e) {
        // Angular not yet available, or 'platformWebApp' module not registered
        // at this point — fall back to the seed list in collectKnownMetaFormNames.
    }

    function getInjector() {
        try {
            if (typeof angular === 'undefined' || !angular.element) {
                return null;
            }
            var injector = angular.element(document).injector();
            return injector || null;
        } catch (e) {
            return null;
        }
    }

    function getService(name) {
        var injector = getInjector();
        if (!injector) {
            return null;
        }
        try {
            return injector.get(name);
        } catch (e) {
            return null;
        }
    }

    function copyToClipboard(text) {
        // Synchronous textarea-based copy preserves the user gesture and is reliable
        // even if the page is not focused or navigator.clipboard is unavailable.
        var ok = false;
        var ta = document.createElement('textarea');
        ta.value = text;
        ta.setAttribute('readonly', '');
        ta.setAttribute(DATA_VC_EXT_ATTR, '1');
        ta.style.cssText = POSITION_FIXED + ';top:0;left:0;width:1px;height:1px;opacity:0;pointer-events:none;';
        document.body.appendChild(ta);
        ta.focus();
        ta.select();
        try {
            ok = document.execCommand('copy');
        } catch (e) {
            ok = false;
        }
        ta.remove();

        if (navigator.clipboard && navigator.clipboard.writeText) {
            navigator.clipboard.writeText(text).catch(function () { /* best-effort, ignore */ });
        }
        return ok;
    }

    function showToast(message, color) {
        var existing = document.getElementById(TOAST_ID);
        if (existing) {
            existing.remove();
        }

        var toast = document.createElement('div');
        toast.id = TOAST_ID;
        toast.setAttribute(DATA_VC_EXT_ATTR, '1');
        toast.textContent = message;
        toast.style.cssText = [
            POSITION_FIXED,
            'bottom:32px',
            'left:50%',
            'transform:translateX(-50%) translateY(20px)',
            BG_PREFIX + (color || COLOR_TOAST_OK),
            WHITE_FG,
            'padding:10px 18px',
            'border-radius:6px',
            'font:600 13px/1.4 Consolas,Menlo,monospace',
            'box-shadow:0 6px 18px rgba(0,0,0,0.35)',
            'z-index:2147483647',
            'pointer-events:none',
            'opacity:0',
            'transition:opacity 180ms ease, transform 180ms ease',
            'max-width:80vw',
            'white-space:pre-line',
            'text-align:center'
        ].join(';');
        document.body.appendChild(toast);

        requestAnimationFrame(function () {
            toast.style.opacity = '1';
            toast.style.transform = 'translateX(-50%) translateY(0)';
        });
        setTimeout(function () {
            toast.style.opacity = '0';
            toast.style.transform = 'translateX(-50%) translateY(20px)';
            setTimeout(function () {
                if (toast.parentNode) {
                    toast.parentNode.removeChild(toast);
                }
            }, 220);
        }, 1600);
    }

    // -------- Snippet templates --------

    var SNIPPETS = {
        menu: function (item) {
            var path = (item && item.path) || '<group>/<item>';
            var icon = (item && item.icon) || 'fa fa-cube';
            var title = (item && item.title) || 'platform.menu.<key>';
            var permission = (item && item.permission) || '<permission>';
            return [
                'mainMenuService.addMenuItem({',
                `    path: '${path}',`,
                `    icon: '${icon}',`,
                `    title: '${title}',`,
                '    priority: 100,',
                "    action: function () { $state.go('<state-name>'); },",
                `    permission: '${permission}'`,
                '});'
            ].join('\n');
        },
        blade: function (id, controller) {
            var bladeId = id || '<bladeId>';
            var ctrl = controller || '<yourModule>.<yourController>';
            return [
                'var blade = {',
                `    id: '${bladeId}',`,
                "    title: 'platform.blades.<title-key>',",
                `    controller: '${ctrl}',`,
                `    template: '$(YourModule)/Scripts/blades/${bladeId}.tpl.html'`,
                '};',
                'bladeNavigationService.showBlade(blade, parentBlade);'
            ].join('\n');
        },
        toolbar: function (controller) {
            var ctrl = controller || '<yourModule>.<yourController>';
            return [
                'toolbarService.register({',
                "    name: 'platform.commands.<command-key>',",
                "    icon: 'fa fa-cog',",
                '    index: 100,',
                '    executeMethod: function (blade) { /* ... */ },',
                '    canExecuteMethod: function (blade) { return true; }',
                `}, '${ctrl}');`
            ].join('\n');
        },
        widget: function (group) {
            var g = group || '<containerName>';
            return [
                'widgetService.registerWidget({',
                "    controller: '<yourModule>.<yourWidgetController>',",
                "    template: '$(YourModule)/Scripts/widgets/<widget>.tpl.html'",
                `}, '${g}');`
            ].join('\n');
        },
        metaform: function (formName) {
            var name = formName || '<formName>';
            return [
                `metaFormsService.registerMetaFields('${name}', [{`,
                "    name: '<fieldName>',",
                "    title: 'platform.blades.<form>.labels.<field>',",
                "    valueType: 'ShortText',",
                '    priority: 100',
                '}]);'
            ].join('\n');
        }
    };

    // -------- Mutation filtering --------
    // Avoid an infinite rebuild loop: ignore mutations caused by the inspector's
    // own DOM (overlay root, label text, toast, copy textarea). Anything tagged
    // with [data-vc-ext] is internal.

    function isInternalNode(node) {
        if (!node || node.nodeType !== 1) {
            return false;
        }
        if (node.hasAttribute && node.hasAttribute(DATA_VC_EXT_ATTR)) {
            return true;
        }
        return !!(node.closest && node.closest(`[${DATA_VC_EXT_ATTR}]`));
    }

    function nodeListHasExternal(nodes) {
        for (var node of nodes) {
            if (node.nodeType === 1 && !isInternalNode(node)) {
                return true;
            }
        }
        return false;
    }

    function hasExternalChange(mutation) {
        if (mutation.type === 'childList') {
            return nodeListHasExternal(mutation.addedNodes) || nodeListHasExternal(mutation.removedNodes);
        }
        if (mutation.type === 'attributes') {
            return !isInternalNode(mutation.target);
        }
        return true;
    }

    // -------- DOM scanning --------

    function findHosts() {
        var hosts = [];

        var nav = document.querySelector('nav.nav-bar');
        if (nav) {
            hosts.push({ type: 'menu', el: nav, name: '', service: SVC_MAIN_MENU });
        }

        document.querySelectorAll(BLADE_HOST_SELECTOR).forEach(function (el) {
            var bladeId = el.getAttribute(ATTR_ID) || el.id || '';
            var controller = el.getAttribute(ATTR_NG_CONTROLLER) || '';
            hosts.push({ type: 'blade', el: el, name: bladeId, controller: controller });
        });

        document.querySelectorAll('.blade-toolbar').forEach(function (el) {
            var bladeEl = el.closest(BLADE_HOST_SELECTOR);
            var controller = bladeEl ? (bladeEl.getAttribute(ATTR_NG_CONTROLLER) || '') : '';
            var bladeId = bladeEl ? (bladeEl.getAttribute(ATTR_ID) || '') : '';
            hosts.push({ type: 'toolbar', el: el, name: controller || bladeId, controller: controller });
        });

        document.querySelectorAll('[gridster-opts]').forEach(function (el) {
            var group = el.getAttribute('group') || '';
            hosts.push({ type: 'widget', el: el, name: group });
        });

        document.querySelectorAll('[registered-inputs]').forEach(function (el) {
            var bladeEl = el.closest(BLADE_HOST_SELECTOR);
            var bladeId = bladeEl ? (bladeEl.getAttribute(ATTR_ID) || '') : '';
            var formName = inferMetaFormName(el, bladeId);
            hosts.push({
                type: 'metaform',
                el: el,
                name: formName || `host: ${bladeId}`,
                inferredName: formName
            });
        });

        return hosts;
    }

    function inferMetaFormName(metaformEl, bladeId) {
        try {
            var injector = getInjector();
            if (!injector) {
                return null;
            }
            var svc = injector.get(SVC_METAFORMS);
            if (!svc || typeof svc.getMetaFields !== 'function') {
                return null;
            }

            // The directive's `registered-inputs` attribute is the expression the
            // parent scope evaluates to get the fields array (e.g. 'blade.metaFields',
            // 'blade.metaFields1', etc.). Production builds disable Angular debug
            // info, so angular.element(el).scope() returns null. Instead, walk the
            // $rootScope tree and resolve the expression on each scope until we find
            // an array reference equal to one of the registered form arrays.
            var expr = metaformEl.getAttribute('registered-inputs');
            if (!expr) {
                return null;
            }
            var parts = expr.split('.');

            var $rootScope = injector.get('$rootScope');
            var candidates = collectKnownMetaFormNames(bladeId);
            var found = null;

            function visit(scope) {
                if (found) {
                    return;
                }
                var v = scope;
                for (var i = 0; i < parts.length && v != null; i++) {
                    v = v[parts[i]];
                }
                if (Array.isArray(v)) {
                    for (var name of candidates) {
                        var fields = svc.getMetaFields(name);
                        if (fields && fields === v) {
                            found = name;
                            return;
                        }
                    }
                }
                var c = scope.$$childHead;
                while (c && !found) {
                    visit(c);
                    c = c.$$nextSibling;
                }
            }
            visit($rootScope);
            return found;
        } catch (e) {
            return null;
        }
    }

    function collectKnownMetaFormNames(bladeId) {
        // Merge: live-captured names from the decorator wins (most accurate),
        // plus seed list for forms registered before vcExt loaded (e.g. the
        // decorator may have failed to install if Angular wasn't ready).
        var seen = new Set();
        state.metaFormNames.forEach(function (n) { seen.add(n); });

        var seeds = [
            // platform
            'accountDetails', 'roleDetail', 'roleDetails',
            // store
            'storeDetail',
            // security
            'apiAccountDetail', 'oAuthApplicationDetail',
            // catalog (https://github.com/VirtoCommerce/vc-module-catalog/.../catalog.js)
            'productDetail', 'productDetail1', 'productDetail2',
            'categoryDetail', 'categoryDetails',
            'variationDetail',
            // customer
            'memberDetail',
            // orders
            'orderDetail', 'orderOperationDetail'
        ];
        seeds.forEach(function (n) { seen.add(n); });

        if (bladeId) {
            seen.add(bladeId);
            seen.add(bladeId.replace(/Detail$/, 'Details'));
            seen.add(bladeId.replace(/Details$/, 'Detail'));
        }
        return Array.from(seen);
    }

    // -------- Counts from registries --------
    // One resolver per type so describe() stays linear and easy to extend.

    var COUNT_RESOLVERS = {
        menu: function () {
            var svc = getService(SVC_MAIN_MENU);
            if (!svc || !svc.menuItems) {
                return null;
            }
            return `${svc.menuItems.length} items`;
        },
        widget: function (host) {
            if (!host.name) {
                return null;
            }
            var svc = getService(SVC_WIDGET);
            if (!svc || !svc.widgetsMap) {
                return null;
            }
            var items = svc.widgetsMap[host.name] || [];
            return `${items.length} widgets`;
        },
        toolbar: function (host) {
            if (!host.controller) {
                return null;
            }
            var svc = getService(SVC_TOOLBAR);
            if (!svc || !svc.toolbarCommandsMap) {
                return null;
            }
            var items = svc.toolbarCommandsMap[host.controller] || [];
            return `${items.length} commands`;
        },
        metaform: function (host) {
            if (!host.inferredName) {
                return null;
            }
            var svc = getService(SVC_METAFORMS);
            if (!svc) {
                return null;
            }
            var fields = svc.getMetaFields(host.inferredName);
            if (!fields) {
                return null;
            }
            return `${fields.length} fields`;
        }
    };

    function describe(host) {
        var resolver = COUNT_RESOLVERS[host.type];
        var countStr = resolver ? resolver(host) : null;
        var parts = [`[${host.type}]`];
        if (host.name) {
            parts.push(`:: ${host.name}`);
        }
        if (countStr) {
            parts.push(`(${countStr})`);
        }
        return parts.join(' ');
    }

    // -------- Overlay rendering --------

    function ensureRoot() {
        var root = document.getElementById(ROOT_ID);
        if (root) {
            attachRootDelegate(root);
            return root;
        }
        root = document.createElement('div');
        root.id = ROOT_ID;
        root.setAttribute(DATA_VC_EXT_ATTR, '1');
        root.style.cssText = `${POSITION_FIXED};top:0;left:0;width:0;height:0;pointer-events:none;z-index:2147483600;`;
        document.body.appendChild(root);
        attachRootDelegate(root);
        return root;
    }

    var LABEL_HEIGHT = 22;

    function applyLabelPosition(label, rect) {
        // The label is a direct sibling of the box (NOT a child) so that real mouse
        // hit-testing isn't confused by the box's pointer-events:none ancestor when
        // the label sits outside the box's geometric bounds. Position the label in
        // viewport coords directly.
        var insideTop = rect.top < LABEL_HEIGHT;
        var labelTop = insideTop ? rect.top : (rect.top - LABEL_HEIGHT);
        label.style.left = `${rect.left}px`;
        label.style.top = `${labelTop}px`;
        label.style.borderRadius = insideTop ? '0 0 3px 0' : '3px 3px 0 0';
    }

    function makeOverlay(host) {
        var color = TYPE_COLORS[host.type] || TYPE_COLORS.blade;
        var rect = host.el.getBoundingClientRect();
        if (rect.width === 0 || rect.height === 0) {
            return null;
        }

        var box = document.createElement('div');
        box.className = 'vc-ext-overlay';
        // Tag directly (not just via the root ancestor) so isInternalNode() can
        // still recognise this element as inspector-owned after rebuild() detaches
        // it from the root — closest() can't traverse to a former parent on a
        // disconnected node, and without the direct attribute the resulting
        // childList mutation would be misclassified as external and re-trigger
        // scheduleRebuild() in an infinite loop.
        box.setAttribute(DATA_VC_EXT_ATTR, '1');
        box.style.cssText = [
            POSITION_FIXED,
            'pointer-events:none',
            `border:2px dashed ${color.border}`,
            BG_PREFIX + color.bg,
            'box-sizing:border-box',
            'transition:none',
            `left:${rect.left}px`,
            `top:${rect.top}px`,
            `width:${rect.width}px`,
            `height:${rect.height}px`
        ].join(';');

        var label = document.createElement('div');
        label.className = 'vc-ext-label';
        label.setAttribute(DATA_VC_EXT_ATTR, '1');
        // CSS grid (1fr auto) keeps the layout predictable across browsers: the text
        // column takes whatever's left after the auto-sized button column, and the
        // text truncates with ellipsis when it doesn't fit. Flex with shrink-to-fit
        // is fragile here — long unbreakable descriptors interact badly with
        // white-space:nowrap and intrinsic min-content sizing.
        label.style.cssText = [
            POSITION_FIXED,
            'pointer-events:auto',
            BG_PREFIX + color.border,
            WHITE_FG,
            'font:600 11px/1.4 Consolas,Menlo,monospace',
            'padding:2px 8px',
            'grid-template-columns:minmax(0,1fr) auto',
            'align-items:center',
            'gap:8px',
            'max-width:400px',
            'box-sizing:border-box',
            'overflow:hidden',
            'box-shadow:0 1px 4px rgba(0,0,0,0.25)'
        ].join(';');
        // Set display via the property API: when packed inside cssText alongside
        // the `font:` shorthand and many other declarations, some browsers silently
        // drop `display:grid`. Setting it after via the property setter is reliable.
        label.style.display = 'grid';
        applyLabelPosition(label, rect);

        var text = document.createElement('span');
        text.textContent = describe(host);
        text.style.cssText = 'overflow:hidden;text-overflow:ellipsis;white-space:nowrap;min-width:0;';
        label.appendChild(text);

        var copyBtn = document.createElement('button');
        copyBtn.type = 'button';
        copyBtn.textContent = COPY_BUTTON_TEXT;
        copyBtn.setAttribute('data-vc-ext-action', 'copy');
        copyBtn.style.cssText = [
            'pointer-events:auto',
            'background:rgba(255,255,255,0.18)',
            WHITE_FG,
            'border:1px solid rgba(255,255,255,0.4)',
            'border-radius:2px',
            'padding:1px 6px',
            'font:600 10px/1.4 Consolas,Menlo,monospace',
            'cursor:pointer',
            'white-space:nowrap'
        ].join(';');
        copyBtn.__vcHost = host;
        label.appendChild(copyBtn);

        return { box: box, label: label, host: host };
    }

    function attachRootDelegate(root) {
        if (root.__vcDelegated) {
            return;
        }
        root.__vcDelegated = true;

        function handleCopy(ev) {
            var btn = ev.target && ev.target.closest && ev.target.closest('button[data-vc-ext-action="copy"]');
            if (!btn) {
                return;
            }
            // Guard against double-fire (mousedown + click on the same press)
            if (state.copyLockUntil && Date.now() < state.copyLockUntil) {
                ev.stopPropagation();
                ev.preventDefault();
                return;
            }
            state.copyLockUntil = Date.now() + 500;
            ev.stopPropagation();
            ev.preventDefault();
            var host = btn.__vcHost;
            if (!host) {
                return;
            }
            var snippet = snippetFor(host);
            var ok = copyToClipboard(snippet);
            btn.textContent = ok ? 'Copied!' : 'Copy failed';
            setTimeout(function () {
                if (btn.isConnected) {
                    btn.textContent = COPY_BUTTON_TEXT;
                }
            }, 1200);
            var typeLabel = `[${host.type}]${host.name ? ` ${host.name}` : ''}`;
            if (ok) {
                showToast(`Snippet copied to clipboard\n${typeLabel}`, COLOR_TOAST_OK);
            } else {
                showToast(`Copy failed — see console\n${typeLabel}`, COLOR_TOAST_FAIL);
                console.warn(`[Virto] Copy failed. Snippet:\n${snippet}`);
            }
        }

        // mousedown fires immediately on press — before any subsequent DOM mutations
        // (Angular digest, blade animation, observer-triggered rebuild) can tear the
        // button down. click + mouseup may fire on a removed element and be lost.
        // Both listeners are guarded by copyLockUntil so a single press fires once.
        root.addEventListener('mousedown', handleCopy, true);
        root.addEventListener('click', handleCopy, true);
    }

    function snippetFor(host) {
        switch (host.type) {
            case 'menu':
                return SNIPPETS.menu(null);
            case 'blade':
                return SNIPPETS.blade(host.name, host.controller);
            case 'toolbar':
                return SNIPPETS.toolbar(host.controller);
            case 'widget':
                return SNIPPETS.widget(host.name);
            case 'metaform':
                return SNIPPETS.metaform(host.inferredName);
            default:
                return '';
        }
    }

    // Reposition: read all rects FIRST, then apply all styles. Batching prevents
    // layout-thrash that would otherwise occur when each iteration toggles between
    // a getBoundingClientRect (forces layout) and a style write (invalidates it).
    function reposition() {
        var entries = state.overlays;
        var rects = new Array(entries.length);
        for (var i = 0; i < entries.length; i++) {
            rects[i] = entries[i].host.el.getBoundingClientRect();
        }
        for (var j = 0; j < entries.length; j++) {
            var entry = entries[j];
            var rect = rects[j];
            if (rect.width === 0 || rect.height === 0) {
                entry.box.style.display = 'none';
                entry.label.style.display = 'none';
                continue;
            }
            // Skip writes when the rect hasn't moved (cheap shallow compare via cached values).
            if (entry._lastTop === rect.top && entry._lastLeft === rect.left
                && entry._lastWidth === rect.width && entry._lastHeight === rect.height) {
                continue;
            }
            entry._lastTop = rect.top;
            entry._lastLeft = rect.left;
            entry._lastWidth = rect.width;
            entry._lastHeight = rect.height;
            entry.box.style.display = '';
            entry.box.style.left = `${rect.left}px`;
            entry.box.style.top = `${rect.top}px`;
            entry.box.style.width = `${rect.width}px`;
            entry.box.style.height = `${rect.height}px`;
            // Restore to 'grid' (not '') — clearing the inline display would let
            // the browser's default 'block' win, breaking the column layout that
            // keeps the Copy snippet button on the same line as the label text.
            entry.label.style.display = 'grid';
            applyLabelPosition(entry.label, rect);
        }
    }

    // Pause the observer around any inspector-internal DOM work so the data-vc-ext
    // filter doesn't have to do it. Belt-and-suspenders against any mutation that
    // could slip past the filter (e.g. a host element being modified by Angular at
    // the same instant we're updating overlays).
    function withObserverPaused(fn) {
        var wasObserving = !!state.observer;
        if (wasObserving) {
            state.observer.disconnect();
        }
        try {
            fn();
        } finally {
            if (wasObserving && state.active) {
                attachObserver();
            }
        }
    }

    // Diff the current overlays against findHosts() output:
    //   • host element already present  → keep the existing overlay (no DOM churn)
    //   • host element new              → create + append
    //   • old entry whose host vanished → remove from DOM and drop
    // Avoids the previous tear-down-everything-and-rebuild cycle which caused layout
    // thrash and visible lag whenever Angular's digest mutated the DOM.
    function rebuild() {
        withObserverPaused(function () {
            var root = ensureRoot();
            var newHosts = findHosts();

            var existingByEl = new Map();
            state.overlays.forEach(function (e) { existingByEl.set(e.host.el, e); });

            var nextOverlays = [];
            var seen = new Set();

            newHosts.forEach(function (host) {
                var existing = existingByEl.get(host.el);
                if (existing && existing.host.type === host.type) {
                    // Refresh fields that may have changed (count, name, controller, inferredName)
                    existing.host = host;
                    var span = existing.label.firstChild;
                    if (span) {
                        span.textContent = describe(host);
                    }
                    var btn = existing.label.querySelector('button[data-vc-ext-action="copy"]');
                    if (btn) {
                        btn.__vcHost = host;
                    }
                    nextOverlays.push(existing);
                    seen.add(existing);
                    return;
                }
                var entry = makeOverlay(host);
                if (entry) {
                    root.appendChild(entry.box);
                    root.appendChild(entry.label);
                    nextOverlays.push(entry);
                }
            });

            state.overlays.forEach(function (e) {
                if (!seen.has(e)) {
                    if (e.box.parentNode) e.box.parentNode.removeChild(e.box);
                    if (e.label.parentNode) e.label.parentNode.removeChild(e.label);
                }
            });

            state.overlays = nextOverlays;
        });
        // After diffing, place the surviving overlays at their current rects.
        reposition();
    }

    function scheduleRebuild() {
        if (state.rebuildTimer) {
            return;
        }
        state.rebuildTimer = setTimeout(function () {
            state.rebuildTimer = null;
            // Skip if a copy is in progress so the user's button isn't torn down mid-press
            if (state.copyLockUntil && Date.now() < state.copyLockUntil) {
                scheduleRebuild();
                return;
            }
            if (state.active) {
                rebuild();
            }
        }, 250);
    }

    // Continuous rAF loop tracks blade-open / blade-close CSS transforms (which fire
    // neither scroll nor resize and don't trigger childList mutations). The loop
    // self-stops when the inspector is hidden. Cost is ~1 cheap getBoundingClientRect
    // per overlay per frame — and reposition() short-circuits when nothing moved.
    function startRepositionLoop() {
        if (state.repositionLoopRunning) {
            return;
        }
        state.repositionLoopRunning = true;
        function tick() {
            if (!state.active) {
                state.repositionLoopRunning = false;
                return;
            }
            reposition();
            requestAnimationFrame(tick);
        }
        requestAnimationFrame(tick);
    }

    // -------- Public API --------

    function attachObserver() {
        state.observer = new MutationObserver(function (mutations) {
            for (var mutation of mutations) {
                if (hasExternalChange(mutation)) {
                    scheduleRebuild();
                    return;
                }
            }
        });
        state.observer.observe(document.body, {
            childList: true,
            subtree: true,
            attributes: true,
            attributeFilter: [ATTR_ID, 'ng-model', 'group', 'registered-inputs']
        });
    }

    function show() {
        if (state.active) {
            rebuild();
            return;
        }
        state.active = true;
        attachObserver();
        rebuild();
        // rAF loop tracks blade animation (CSS transform) — scroll/resize alone miss it.
        startRepositionLoop();

        console.log('[Virto] Extension-point overlay enabled. Type vcExt.hide() to remove.');
    }

    function hide() {
        state.active = false;
        if (state.observer) {
            state.observer.disconnect();
            state.observer = null;
        }
        if (state.rebuildTimer) {
            clearTimeout(state.rebuildTimer);
            state.rebuildTimer = null;
        }
        var root = document.getElementById(ROOT_ID);
        if (root) {
            root.parentNode.removeChild(root);
        }
        state.overlays = [];
    }

    function toggle() {
        if (state.active) {
            hide();
        } else {
            show();
        }
    }

    function list() {
        var rows = [];

        var menuSvc = getService(SVC_MAIN_MENU);
        if (menuSvc && menuSvc.menuItems) {
            menuSvc.menuItems.forEach(function (m) {
                rows.push({
                    type: 'menu',
                    name: m.path,
                    detail: m.title || '',
                    priority: m.priority,
                    permission: m.permission || ''
                });
            });
        }

        var bladeSvc = getService(SVC_BLADE);
        if (bladeSvc && bladeSvc.blades) {
            Object.keys(bladeSvc.blades).forEach(function (stateName) {
                (bladeSvc.blades[stateName] || []).forEach(function (b) {
                    rows.push({
                        type: 'blade',
                        name: b.id,
                        detail: b.controller || '',
                        priority: '',
                        permission: b.updatePermission || ''
                    });
                });
            });
        }

        var widgetSvc = getService(SVC_WIDGET);
        if (widgetSvc && widgetSvc.widgetsMap) {
            Object.keys(widgetSvc.widgetsMap).forEach(function (group) {
                (widgetSvc.widgetsMap[group] || []).forEach(function (w) {
                    rows.push({
                        type: 'widget',
                        name: group,
                        detail: w.controller || '',
                        priority: '',
                        permission: w.permission || ''
                    });
                });
            });
        }

        var tbSvc = getService(SVC_TOOLBAR);
        if (tbSvc && tbSvc.toolbarCommandsMap) {
            Object.keys(tbSvc.toolbarCommandsMap).forEach(function (ctrl) {
                (tbSvc.toolbarCommandsMap[ctrl] || []).forEach(function (cmd) {
                    rows.push({
                        type: 'toolbar',
                        name: ctrl,
                        detail: cmd.name || '',
                        priority: cmd.index,
                        permission: cmd.permission || ''
                    });
                });
            });
        }

        var mfSvc = getService(SVC_METAFORMS);
        if (mfSvc) {
            collectKnownMetaFormNames('').forEach(function (n) {
                var fields = mfSvc.getMetaFields(n);
                if (fields && fields.length) {
                    fields.forEach(function (f) {
                        rows.push({
                            type: 'metaform',
                            name: n,
                            detail: f.name || '',
                            priority: f.priority,
                            permission: ''
                        });
                    });
                }
            });
            console.log('[Virto] metaform list shows known form names only (registry is private). Add custom names via collectKnownMetaFormNames seed.');
        }

        if (rows.length === 0) {
            console.warn('[Virto] No registered extension points found. Is the platform UI bootstrapped yet?');
            return rows;
        }
        console.table(rows);
        return rows;
    }

    function copy(type, id) {
        var snippet;
        switch (type) {
            case 'menu':
                snippet = SNIPPETS.menu(null);
                break;
            case 'blade':
                snippet = SNIPPETS.blade(id, null);
                break;
            case 'toolbar':
                snippet = SNIPPETS.toolbar(id);
                break;
            case 'widget':
                snippet = SNIPPETS.widget(id);
                break;
            case 'metaform':
                snippet = SNIPPETS.metaform(id);
                break;
            default:
                console.warn("[Virto] Unknown type. Use one of: 'menu', 'blade', 'toolbar', 'widget', 'metaform'");
                return;
        }
        var ok = copyToClipboard(snippet);
        var idSuffix = id ? ` ${id}` : '';
        if (ok) {
            showToast(`Snippet copied to clipboard\n[${type}]${idSuffix}`, COLOR_TOAST_OK);
            console.log(`[Virto] Snippet copied to clipboard:\n${snippet}`);
        } else {
            showToast(`Copy failed — see console\n[${type}]`, COLOR_TOAST_FAIL);
            console.warn(`[Virto] Copy failed. Snippet:\n${snippet}`);
        }
    }

    function help() {
        console.log([
            'Virto Commerce Extension-Point Inspector',
            '----------------------------------------',
            'vcExt.show()             Highlight all extension points on the current page.',
            'vcExt.hide()             Remove the overlay.',
            'vcExt.toggle()           Toggle overlay.',
            'vcExt.list()             Print a table of every registered item across all 5 services.',
            "vcExt.copy(type, id?)    Copy a registration snippet. type ∈ 'menu' | 'blade' | 'toolbar' | 'widget' | 'metaform'.",
            '',
            'Highlighted points: main menu, blade, blade toolbar, widget container, meta form.',
            'Reference: https://docs.virtocommerce.org/platform/developer-guide/latest/Extensibility/key-extensibility-points/'
        ].join('\n'));
    }

    global.vcExt = {
        show: show,
        hide: hide,
        toggle: toggle,
        list: list,
        copy: copy,
        help: help
    };

    console.log('[Virto] Extension-point inspector loaded. Type vcExt.help() to begin.');

})(window);
