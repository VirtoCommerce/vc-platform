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
//
// Adding a new extension point? Add ONE entry to the PROBES registry below
// (see "PROBES" section). The compiler can't catch missing fields, but
// assertProbeShape() at module load fails-loud-logs anything malformed.
//
// Porting to vc-shell or another host? Replace only the Bridge object and
// each PROBES[type] body. The framework-free engine (overlay rendering,
// clipboard, toast, mutation filter, root delegate, rAF loop, STYLES) carries
// over verbatim.
(function (global) {
    'use strict';

    if (global.vcExt) {
        return;
    }

    // ════════════════════════════════════════════════════════════════════════
    // CONSTANTS
    // ════════════════════════════════════════════════════════════════════════

    var ROOT_ID = 'vc-ext-inspector-root';
    var TOAST_ID = 'vc-ext-toast';
    var DATA_VC_EXT_ATTR = 'data-vc-ext';
    var COPY_ACTION_ATTR = 'data-vc-ext-action';
    var COPY_ACTION_VALUE = 'copy';
    var ATTR_ID = 'id';
    var ATTR_NG_CONTROLLER = 'ng-controller';
    var BLADE_HOST_SELECTOR = '[ng-model="blade"]';
    var COPY_BUTTON_TEXT = 'Copy snippet';
    var LABEL_HEIGHT = 22;
    var COPY_LOCK_MS = 500;        // dedupe window for mousedown+click on the same press
    var REBUILD_DEBOUNCE_MS = 250; // collapses bursts of Angular DOM mutations
    var TOAST_VISIBLE_MS = 1600;
    var TOAST_FADE_MS = 220;
    var BUTTON_FLASH_MS = 1200;

    var COLOR_TOAST_OK = '#16a34a';
    var COLOR_TOAST_FAIL = '#dc2626';

    // AngularJS service injection keys.
    var SVC_MAIN_MENU = 'platformWebApp.mainMenuService';
    var SVC_BLADE = 'platformWebApp.bladeNavigationService';
    var SVC_WIDGET = 'platformWebApp.widgetService';
    var SVC_TOOLBAR = 'platformWebApp.toolbarService';
    var SVC_METAFORMS = 'platformWebApp.metaFormsService';

    // ════════════════════════════════════════════════════════════════════════
    // STYLES — every inline cssText is centralised here.
    // Adding new styles? Put them in this block. Do NOT scatter cssText
    // strings across functions.
    //
    // Why `display:grid` is set separately on the label (not in cssText):
    // when packed alongside the `font:` shorthand and many other
    // declarations, some browsers' CSSOM parser silently drops the
    // `display:grid` declaration. Setting it via the property setter after
    // cssText is reliable. See makeOverlay().
    // ════════════════════════════════════════════════════════════════════════

    var POSITION_FIXED = 'position:fixed';
    var WHITE_FG = 'color:#fff';

    var STYLES = {
        root: POSITION_FIXED + ';top:0;left:0;width:0;height:0;pointer-events:none;z-index:2147483600;',

        box: function (color) {
            return [
                POSITION_FIXED,
                'pointer-events:none',
                'border:2px dashed ' + color.border,
                'background:' + color.bg,
                'box-sizing:border-box',
                'transition:none'
            ].join(';');
        },

        label: function (color) {
            // grid-template-columns lets the text shrink with ellipsis while the
            // Copy button keeps its natural width. CSS grid is more predictable
            // than flex for this layout — flex shrink-to-fit interacts badly
            // with white-space:nowrap and intrinsic min-content.
            return [
                POSITION_FIXED,
                'pointer-events:auto',
                'background:' + color.border,
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
        },

        labelText: 'overflow:hidden;text-overflow:ellipsis;white-space:nowrap;min-width:0;',

        copyBtn: [
            'pointer-events:auto',
            'background:rgba(255,255,255,0.18)',
            WHITE_FG,
            'border:1px solid rgba(255,255,255,0.4)',
            'border-radius:2px',
            'padding:1px 6px',
            'font:600 10px/1.4 Consolas,Menlo,monospace',
            'cursor:pointer',
            'white-space:nowrap'
        ].join(';'),

        toast: function (color) {
            return [
                POSITION_FIXED,
                'bottom:32px',
                'left:50%',
                'transform:translateX(-50%) translateY(20px)',
                'background:' + color,
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
        },

        copyHelper: POSITION_FIXED + ';top:0;left:0;width:1px;height:1px;opacity:0;pointer-events:none;'
    };

    // ════════════════════════════════════════════════════════════════════════
    // STATE
    // ════════════════════════════════════════════════════════════════════════

    var state = {
        active: false,
        overlays: [],
        observer: null,
        rebuildTimer: null,
        copyInFlight: false,
        repositionLoopRunning: false,
        // Names captured live by Bridge.installDecorator. Populated as modules
        // call metaFormsService.registerMetaFields(name, …) so we can match
        // every <va-metaform> directive to its actual registry key (e.g.
        // 'productDetail2' on the catalog item page) instead of falling back
        // to the host:bladeId placeholder.
        metaFormNames: new Set()
    };

    // ════════════════════════════════════════════════════════════════════════
    // ANGULAR BRIDGE — PORT THIS for vc-shell.
    //
    // Everything outside this block (overlay engine, clipboard, toast,
    // mutation filter, delegate, rAF loop, STYLES, PROBES shape) is
    // framework-agnostic. To port the inspector to a different host UI,
    // replace this Bridge object and rewrite each PROBES[type] body.
    // ════════════════════════════════════════════════════════════════════════

    var Bridge = {
        getInjector: function () {
            try {
                if (typeof angular === 'undefined' || !angular.element) {
                    return null;
                }
                return angular.element(document).injector() || null;
            } catch (e) {
                return null;
            }
        },

        getService: function (name) {
            var injector = Bridge.getInjector();
            if (!injector) {
                return null;
            }
            try {
                return injector.get(name);
            } catch (e) {
                return null;
            }
        },

        // Walk $rootScope tree and resolve `expr` (e.g. 'blade.metaFields1') on
        // each scope. Calls match(arr) for each Array result; returns the first
        // truthy value match returns.
        //
        // Production builds run with $compileProvider.debugInfoEnabled(false),
        // which disables angular.element(el).scope() — so this is the only
        // reliable way to reach scope state from the DOM.
        walkScopeTree: function (expr, match) {
            var injector = Bridge.getInjector();
            if (!injector) {
                return null;
            }
            var $rootScope;
            try {
                $rootScope = injector.get('$rootScope');
            } catch (e) {
                return null;
            }
            var parts = expr.split('.');
            var found = null;
            (function visit(scope) {
                if (found) {
                    return;
                }
                var v = scope;
                for (var i = 0; i < parts.length && v != null; i++) {
                    v = v[parts[i]];
                }
                if (Array.isArray(v)) {
                    var hit = match(v);
                    if (hit) {
                        found = hit;
                        return;
                    }
                }
                var c = scope.$$childHead;
                while (c && !found) {
                    visit(c);
                    c = c.$$nextSibling;
                }
            })($rootScope);
            return found;
        },

        // Wrap metaFormsService.registerMetaFields so we record every form name
        // as it's registered. The decorator runs when the service is first
        // instantiated by the injector — strictly before any .run() block that
        // injects it. Fails silently if Angular isn't ready / the module isn't
        // registered yet (we fall back to the seed list).
        installRegisterCallback: function (onRegister) {
            try {
                if (typeof angular !== 'undefined' && angular.module) {
                    angular.module('platformWebApp').decorator(
                        'platformWebApp.metaFormsService',
                        ['$delegate', function ($delegate) {
                            var orig = $delegate.registerMetaFields;
                            $delegate.registerMetaFields = function (formName) {
                                if (formName) {
                                    onRegister(formName);
                                }
                                return orig.apply(this, arguments);
                            };
                            return $delegate;
                        }]
                    );
                }
            } catch (e) {
                // Angular not yet available, or 'platformWebApp' module not
                // registered at this point — fall back to the seed list.
            }
        }
    };

    Bridge.installRegisterCallback(function (name) {
        state.metaFormNames.add(name);
    });

    // ════════════════════════════════════════════════════════════════════════
    // SNIPPETS — registration call templates per type
    // ════════════════════════════════════════════════════════════════════════

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

    // ════════════════════════════════════════════════════════════════════════
    // METAFORM NAME INFERENCE
    //
    // angular.element(el).scope() returns null in production builds (debug
    // info disabled). Instead, evaluate the directive's `registered-inputs`
    // expression on each scope in the $rootScope tree and match the resulting
    // array reference against metaFormsService.getMetaFields(name) for every
    // candidate name (live-captured + seed list).
    // ════════════════════════════════════════════════════════════════════════

    function collectKnownMetaFormNames(bladeId) {
        var seen = new Set();
        state.metaFormNames.forEach(function (n) { seen.add(n); });

        var seeds = [
            // platform
            'accountDetails', 'roleDetail', 'roleDetails',
            // store
            'storeDetail',
            // security
            'apiAccountDetail', 'oAuthApplicationDetail',
            // catalog
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

    function inferMetaFormName(metaformEl, bladeId) {
        var expr = metaformEl.getAttribute('registered-inputs');
        if (!expr) {
            return null;
        }
        var svc = Bridge.getService(SVC_METAFORMS);
        if (!svc || typeof svc.getMetaFields !== 'function') {
            return null;
        }
        var candidates = collectKnownMetaFormNames(bladeId);
        return Bridge.walkScopeTree(expr, function (arr) {
            for (var i = 0; i < candidates.length; i++) {
                if (svc.getMetaFields(candidates[i]) === arr) {
                    return candidates[i];
                }
            }
            return null;
        });
    }

    // ════════════════════════════════════════════════════════════════════════
    // PROBES — single source of truth per extension point.
    //
    // ADDING A NEW EXTENSION POINT? Add ONE entry to this object. The
    // engine derives findHosts, describe, snippetFor, vcExt.list, and
    // vcExt.copy from this registry.
    //
    // Each probe must define:
    //   color           { border, bg }     outline + tint colour
    //   countLabel      string             noun for the count badge ('widgets')
    //   find()          → Host[]           DOM scan (Host = { el, name, ...extra })
    //   count(host)     → number | null    items registered for this host
    //   snippet(host)   → string           snippet from a clicked overlay
    //   manualSnippet(id?) → string        snippet for vcExt.copy(type, id)
    //   list()          → Row[]            flat rows for vcExt.list()
    //                                      (Row = { name, detail, priority, permission })
    // ════════════════════════════════════════════════════════════════════════

    var PROBES = {
        menu: {
            color: { border: '#7c3aed', bg: 'rgba(124,58,237,0.12)' },
            countLabel: 'items',
            find: function () {
                var nav = document.querySelector('nav.nav-bar');
                return nav ? [{ el: nav, name: '' }] : [];
            },
            count: function () {
                var s = Bridge.getService(SVC_MAIN_MENU);
                return (s && s.menuItems) ? s.menuItems.length : null;
            },
            snippet: function () { return SNIPPETS.menu(); },
            manualSnippet: function () { return SNIPPETS.menu(); },
            list: function () {
                var s = Bridge.getService(SVC_MAIN_MENU);
                if (!s || !s.menuItems) {
                    return [];
                }
                return s.menuItems.map(function (m) {
                    return {
                        name: m.path,
                        detail: m.title || '',
                        priority: m.priority,
                        permission: m.permission || ''
                    };
                });
            }
        },

        blade: {
            color: { border: '#2563eb', bg: 'rgba(37,99,235,0.10)' },
            countLabel: '',
            find: function () {
                return qsa(BLADE_HOST_SELECTOR).map(function (el) {
                    return {
                        el: el,
                        name: el.getAttribute(ATTR_ID) || el.id || '',
                        controller: el.getAttribute(ATTR_NG_CONTROLLER) || ''
                    };
                });
            },
            count: function () { return null; },
            snippet: function (host) { return SNIPPETS.blade(host.name, host.controller); },
            manualSnippet: function (id) { return SNIPPETS.blade(id, null); },
            list: function () {
                var s = Bridge.getService(SVC_BLADE);
                if (!s || !s.blades) {
                    return [];
                }
                var rows = [];
                Object.keys(s.blades).forEach(function (stateName) {
                    (s.blades[stateName] || []).forEach(function (b) {
                        rows.push({
                            name: b.id,
                            detail: b.controller || '',
                            priority: '',
                            permission: b.updatePermission || ''
                        });
                    });
                });
                return rows;
            }
        },

        toolbar: {
            color: { border: '#ea580c', bg: 'rgba(234,88,12,0.10)' },
            countLabel: 'commands',
            find: function () {
                return qsa('.blade-toolbar').map(function (el) {
                    var bladeEl = el.closest(BLADE_HOST_SELECTOR);
                    var controller = bladeEl ? (bladeEl.getAttribute(ATTR_NG_CONTROLLER) || '') : '';
                    var bladeId = bladeEl ? (bladeEl.getAttribute(ATTR_ID) || '') : '';
                    return {
                        el: el,
                        name: controller || bladeId,
                        controller: controller
                    };
                });
            },
            count: function (host) {
                if (!host.controller) {
                    return null;
                }
                var s = Bridge.getService(SVC_TOOLBAR);
                if (!s || !s.toolbarCommandsMap) {
                    return null;
                }
                return (s.toolbarCommandsMap[host.controller] || []).length;
            },
            snippet: function (host) { return SNIPPETS.toolbar(host.controller); },
            manualSnippet: function (id) { return SNIPPETS.toolbar(id); },
            list: function () {
                var s = Bridge.getService(SVC_TOOLBAR);
                if (!s || !s.toolbarCommandsMap) {
                    return [];
                }
                var rows = [];
                Object.keys(s.toolbarCommandsMap).forEach(function (ctrl) {
                    (s.toolbarCommandsMap[ctrl] || []).forEach(function (cmd) {
                        rows.push({
                            name: ctrl,
                            detail: cmd.name || '',
                            priority: cmd.index,
                            permission: cmd.permission || ''
                        });
                    });
                });
                return rows;
            }
        },

        widget: {
            color: { border: '#16a34a', bg: 'rgba(22,163,74,0.10)' },
            countLabel: 'widgets',
            find: function () {
                return qsa('[gridster-opts]').map(function (el) {
                    return { el: el, name: el.getAttribute('group') || '' };
                });
            },
            count: function (host) {
                if (!host.name) {
                    return null;
                }
                var s = Bridge.getService(SVC_WIDGET);
                if (!s || !s.widgetsMap) {
                    return null;
                }
                return (s.widgetsMap[host.name] || []).length;
            },
            snippet: function (host) { return SNIPPETS.widget(host.name); },
            manualSnippet: function (id) { return SNIPPETS.widget(id); },
            list: function () {
                var s = Bridge.getService(SVC_WIDGET);
                if (!s || !s.widgetsMap) {
                    return [];
                }
                var rows = [];
                Object.keys(s.widgetsMap).forEach(function (group) {
                    (s.widgetsMap[group] || []).forEach(function (w) {
                        rows.push({
                            name: group,
                            detail: w.controller || '',
                            priority: '',
                            permission: w.permission || ''
                        });
                    });
                });
                return rows;
            }
        },

        metaform: {
            color: { border: '#0d9488', bg: 'rgba(13,148,136,0.10)' },
            countLabel: 'fields',
            find: function () {
                return qsa('[registered-inputs]').map(function (el) {
                    var bladeEl = el.closest(BLADE_HOST_SELECTOR);
                    var bladeId = bladeEl ? (bladeEl.getAttribute(ATTR_ID) || '') : '';
                    var formName = inferMetaFormName(el, bladeId);
                    return {
                        el: el,
                        name: formName || ('host: ' + bladeId),
                        inferredName: formName
                    };
                });
            },
            count: function (host) {
                if (!host.inferredName) {
                    return null;
                }
                var s = Bridge.getService(SVC_METAFORMS);
                if (!s) {
                    return null;
                }
                var fields = s.getMetaFields(host.inferredName);
                return fields ? fields.length : null;
            },
            snippet: function (host) { return SNIPPETS.metaform(host.inferredName); },
            manualSnippet: function (id) { return SNIPPETS.metaform(id); },
            list: function () {
                var s = Bridge.getService(SVC_METAFORMS);
                if (!s) {
                    return [];
                }
                var rows = [];
                collectKnownMetaFormNames('').forEach(function (n) {
                    var fields = s.getMetaFields(n);
                    if (fields && fields.length) {
                        fields.forEach(function (f) {
                            rows.push({
                                name: n,
                                detail: f.name || '',
                                priority: f.priority,
                                permission: ''
                            });
                        });
                    }
                });
                return rows;
            }
        }
    };

    // Fail-loud at module load if a probe is missing required fields.
    (function assertProbeShape() {
        var REQUIRED = ['color', 'countLabel', 'find', 'count', 'snippet', 'manualSnippet', 'list'];
        Object.keys(PROBES).forEach(function (type) {
            REQUIRED.forEach(function (field) {
                if (PROBES[type][field] == null) {
                    console.error('[Virto] Probe "' + type + '" is missing required field "' + field + '"');
                }
            });
        });
    })();

    // Derived helpers — DO NOT add a new switch on host.type. Use PROBES.

    function findHosts() {
        var out = [];
        Object.keys(PROBES).forEach(function (type) {
            PROBES[type].find().forEach(function (h) {
                h.type = type;
                out.push(h);
            });
        });
        return out;
    }

    function describe(host) {
        var p = PROBES[host.type];
        var n = p.count(host);
        var parts = ['[' + host.type + ']'];
        if (host.name) {
            parts.push(':: ' + host.name);
        }
        if (n != null) {
            parts.push('(' + n + ' ' + p.countLabel + ')');
        }
        return parts.join(' ');
    }

    function snippetFor(host) {
        return PROBES[host.type].snippet(host);
    }

    // ════════════════════════════════════════════════════════════════════════
    // FRAMEWORK-FREE ENGINE — REUSE for vc-shell.
    //
    // Below this line, nothing references AngularJS. Everything works against
    // generic DOM, MutationObserver, requestAnimationFrame, and the PROBES
    // registry shape.
    // ════════════════════════════════════════════════════════════════════════

    function qsa(sel) {
        return Array.prototype.slice.call(document.querySelectorAll(sel));
    }

    // -------- Mutation filtering --------
    // Avoid an infinite rebuild loop: ignore mutations caused by the inspector's
    // own DOM (overlay root, label, box, toast, copy textarea). Anything tagged
    // with [data-vc-ext] is internal.

    function isInternalNode(node) {
        if (!node || node.nodeType !== 1) {
            return false;
        }
        if (node.hasAttribute && node.hasAttribute(DATA_VC_EXT_ATTR)) {
            return true;
        }
        return !!(node.closest && node.closest('[' + DATA_VC_EXT_ATTR + ']'));
    }

    function nodeListHasExternal(nodes) {
        for (var i = 0; i < nodes.length; i++) {
            var n = nodes[i];
            if (n.nodeType === 1 && !isInternalNode(n)) {
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

    // -------- Clipboard --------
    // Synchronous textarea + execCommand preserves the user gesture. Reliable
    // even when the page isn't focused or navigator.clipboard is unavailable.
    // navigator.clipboard.writeText is fired as a best-effort second write.

    function copyToClipboard(text) {
        var ok = false;
        var ta = document.createElement('textarea');
        ta.value = text;
        ta.setAttribute('readonly', '');
        ta.setAttribute(DATA_VC_EXT_ATTR, '1');
        ta.style.cssText = STYLES.copyHelper;
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

    // -------- Toast --------

    function showToast(message, color) {
        var existing = document.getElementById(TOAST_ID);
        if (existing) {
            existing.remove();
        }

        var toast = document.createElement('div');
        toast.id = TOAST_ID;
        toast.setAttribute(DATA_VC_EXT_ATTR, '1');
        toast.textContent = message;
        toast.style.cssText = STYLES.toast(color || COLOR_TOAST_OK);
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
            }, TOAST_FADE_MS);
        }, TOAST_VISIBLE_MS);
    }

    // -------- Overlay rendering --------
    //
    // Layout: each host has TWO sibling elements under the inspector root:
    //   • a "box" — pointer-events:none border outline tracking the host's rect
    //   • a "label" — pointer-events:auto, contains the type/name text + Copy button
    //
    // The label is a SIBLING of the box (not a child) deliberately. Real mouse
    // hit-testing rejects descendants of a pointer-events:none ancestor when
    // the click point falls outside the ancestor's geometric rect — even though
    // elementFromPoint still returns the descendant. Sibling layout avoids it.

    function applyLabelPosition(label, rect) {
        var insideTop = rect.top < LABEL_HEIGHT;
        var labelTop = insideTop ? rect.top : (rect.top - LABEL_HEIGHT);
        label.style.left = rect.left + 'px';
        label.style.top = labelTop + 'px';
        label.style.borderRadius = insideTop ? '0 0 3px 0' : '3px 3px 0 0';
    }

    function makeOverlay(host) {
        var color = PROBES[host.type].color;
        var rect = host.el.getBoundingClientRect();
        if (rect.width === 0 || rect.height === 0) {
            return null;
        }

        var box = document.createElement('div');
        box.className = 'vc-ext-overlay';
        // Tag directly so isInternalNode() recognises it after rebuild() detaches
        // it from the root — closest() can't traverse to a former parent on a
        // disconnected node, and without the direct attribute the resulting
        // childList mutation would be misclassified as external and re-trigger
        // scheduleRebuild() in an infinite loop.
        box.setAttribute(DATA_VC_EXT_ATTR, '1');
        box.style.cssText = STYLES.box(color);
        box.style.left = rect.left + 'px';
        box.style.top = rect.top + 'px';
        box.style.width = rect.width + 'px';
        box.style.height = rect.height + 'px';

        var label = document.createElement('div');
        label.className = 'vc-ext-label';
        label.setAttribute(DATA_VC_EXT_ATTR, '1');
        label.style.cssText = STYLES.label(color);
        // Set display via the property setter: when packed inside cssText
        // alongside the `font:` shorthand and many other declarations, some
        // browsers silently drop `display:grid`. This setter is reliable.
        label.style.display = 'grid';
        applyLabelPosition(label, rect);

        var text = document.createElement('span');
        text.textContent = describe(host);
        text.style.cssText = STYLES.labelText;
        label.appendChild(text);

        var copyBtn = document.createElement('button');
        copyBtn.type = 'button';
        copyBtn.textContent = COPY_BUTTON_TEXT;
        copyBtn.setAttribute(COPY_ACTION_ATTR, COPY_ACTION_VALUE);
        copyBtn.style.cssText = STYLES.copyBtn;
        copyBtn.__vcHost = host;
        label.appendChild(copyBtn);

        return { box: box, label: label, host: host };
    }

    function ensureRoot() {
        var root = document.getElementById(ROOT_ID);
        if (root) {
            return root;
        }
        root = document.createElement('div');
        root.id = ROOT_ID;
        root.setAttribute(DATA_VC_EXT_ATTR, '1');
        root.style.cssText = STYLES.root;
        document.body.appendChild(root);

        // Click delegation on the root in capture phase. This survives overlay
        // rebuilds (the buttons inside change, but the root is permanent) and
        // beats any Angular click handlers via the capture flag.
        //
        // Both mousedown AND click are wired. mousedown fires immediately on
        // press — before any subsequent DOM mutation can tear the button
        // down — and is what makes copy survive a mid-press rebuild. The click
        // is belt-and-suspenders. copyInFlight + setTimeout deduplicates them.
        root.addEventListener('mousedown', handleRootCopy, true);
        root.addEventListener('click', handleRootCopy, true);

        return root;
    }

    function handleRootCopy(ev) {
        var btn = ev.target && ev.target.closest && ev.target.closest('button[' + COPY_ACTION_ATTR + '="' + COPY_ACTION_VALUE + '"]');
        if (!btn) {
            return;
        }
        if (state.copyInFlight) {
            ev.stopPropagation();
            ev.preventDefault();
            return;
        }
        state.copyInFlight = true;
        setTimeout(function () { state.copyInFlight = false; }, COPY_LOCK_MS);
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
        }, BUTTON_FLASH_MS);

        var typeLabel = '[' + host.type + ']' + (host.name ? ' ' + host.name : '');
        if (ok) {
            showToast('Snippet copied to clipboard\n' + typeLabel, COLOR_TOAST_OK);
        } else {
            showToast('Copy failed — see console\n' + typeLabel, COLOR_TOAST_FAIL);
            console.warn('[Virto] Copy failed. Snippet:\n' + snippet);
        }
    }

    // -------- Reposition + diff rebuild --------
    //
    // reposition: read all rects FIRST, then apply all styles. Batching prevents
    // the layout-thrash that would otherwise occur when each iteration toggles
    // between a getBoundingClientRect (forces layout) and a style write
    // (invalidates it). Per-entry rect cache short-circuits writes when nothing
    // moved, so the 60fps rAF loop is nearly free when the page is idle.

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
            if (entry._lastTop === rect.top && entry._lastLeft === rect.left
                && entry._lastWidth === rect.width && entry._lastHeight === rect.height) {
                continue;
            }
            entry._lastTop = rect.top;
            entry._lastLeft = rect.left;
            entry._lastWidth = rect.width;
            entry._lastHeight = rect.height;
            entry.box.style.display = '';
            entry.box.style.left = rect.left + 'px';
            entry.box.style.top = rect.top + 'px';
            entry.box.style.width = rect.width + 'px';
            entry.box.style.height = rect.height + 'px';
            // Restore to 'grid' (not '') — clearing the inline display would
            // let the browser's default 'block' win, breaking the column layout
            // that keeps the Copy snippet button on the same line as the text.
            entry.label.style.display = 'grid';
            applyLabelPosition(entry.label, rect);
        }
    }

    // Pause the observer around inspector-internal DOM work. Belt-and-suspenders
    // against any mutation that could slip past the [data-vc-ext] filter.
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
    // Avoids tear-down-everything-and-rebuild which caused layout thrash and
    // visible lag whenever Angular's digest mutated the DOM.
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
                    existing.host = host;
                    var span = existing.label.firstChild;
                    if (span) {
                        span.textContent = describe(host);
                    }
                    var btn = existing.label.querySelector('button[' + COPY_ACTION_ATTR + '="' + COPY_ACTION_VALUE + '"]');
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
        reposition();
    }

    function scheduleRebuild() {
        if (state.rebuildTimer) {
            return;
        }
        state.rebuildTimer = setTimeout(function () {
            state.rebuildTimer = null;
            // Defer if a copy is in flight so the user's button isn't torn down mid-press.
            if (state.copyInFlight) {
                scheduleRebuild();
                return;
            }
            if (state.active) {
                rebuild();
            }
        }, REBUILD_DEBOUNCE_MS);
    }

    // Continuous rAF loop tracks blade-open / blade-close CSS transforms (which
    // fire neither scroll nor resize and don't trigger childList mutations).
    // The loop self-stops when the inspector is hidden. Cost is one cheap
    // getBoundingClientRect per overlay per frame — and reposition() short-
    // circuits when nothing moved.
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

    function attachObserver() {
        state.observer = new MutationObserver(function (mutations) {
            for (var i = 0; i < mutations.length; i++) {
                if (hasExternalChange(mutations[i])) {
                    scheduleRebuild();
                    return;
                }
            }
        });
        state.observer.observe(document.body, {
            childList: true,
            subtree: true,
            attributes: true,
            // 'class' is intentionally NOT in this list — Angular toggles classes
            // on every digest, which would re-trigger the observer continuously.
            attributeFilter: [ATTR_ID, 'ng-model', 'group', 'registered-inputs']
        });
    }

    // ════════════════════════════════════════════════════════════════════════
    // PUBLIC API
    // ════════════════════════════════════════════════════════════════════════

    function show() {
        if (state.active) {
            rebuild();
            return;
        }
        state.active = true;
        attachObserver();
        rebuild();
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
        if (root && root.parentNode) {
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
        Object.keys(PROBES).forEach(function (type) {
            PROBES[type].list().forEach(function (row) {
                row.type = type;
                rows.push(row);
            });
        });
        if (rows.length === 0) {
            console.warn('[Virto] No registered extension points found. Is the platform UI bootstrapped yet?');
            return rows;
        }
        // Reorder for nicer console.table: type first.
        var ordered = rows.map(function (r) {
            return {
                type: r.type,
                name: r.name,
                detail: r.detail,
                priority: r.priority,
                permission: r.permission
            };
        });
        console.table(ordered);
        return ordered;
    }

    function copy(type, id) {
        var p = PROBES[type];
        if (!p) {
            console.warn('[Virto] Unknown type "' + type + '". Use one of: ' + Object.keys(PROBES).join(', '));
            return;
        }
        var snippet = p.manualSnippet(id);
        var ok = copyToClipboard(snippet);
        var idSuffix = id ? ' ' + id : '';
        if (ok) {
            showToast('Snippet copied to clipboard\n[' + type + ']' + idSuffix, COLOR_TOAST_OK);
            console.log('[Virto] Snippet copied to clipboard:\n' + snippet);
        } else {
            showToast('Copy failed — see console\n[' + type + ']', COLOR_TOAST_FAIL);
            console.warn('[Virto] Copy failed. Snippet:\n' + snippet);
        }
    }

    function help() {
        var types = Object.keys(PROBES).map(function (t) { return "'" + t + "'"; }).join(' | ');
        console.log([
            'Virto Commerce Extension-Point Inspector',
            '----------------------------------------',
            'vcExt.show()             Highlight all extension points on the current page.',
            'vcExt.hide()             Remove the overlay.',
            'vcExt.toggle()           Toggle overlay.',
            'vcExt.list()             Print a table of every registered item.',
            'vcExt.copy(type, id?)    Copy a registration snippet.',
            '                         type ∈ ' + types + '.',
            '',
            'Highlighted points: ' + Object.keys(PROBES).join(', ') + '.',
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
