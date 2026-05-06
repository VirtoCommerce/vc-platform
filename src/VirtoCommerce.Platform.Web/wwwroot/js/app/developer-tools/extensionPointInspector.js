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
    var TYPE_COLORS = {
        menu:      { border: '#7c3aed', bg: 'rgba(124,58,237,0.12)' },
        blade:     { border: '#2563eb', bg: 'rgba(37,99,235,0.10)' },
        toolbar:   { border: '#ea580c', bg: 'rgba(234,88,12,0.10)' },
        widget:    { border: '#16a34a', bg: 'rgba(22,163,74,0.10)' },
        metaform:  { border: '#0d9488', bg: 'rgba(13,148,136,0.10)' }
    };

    var state = {
        active: false,
        overlays: [],
        observer: null,
        rafScheduled: false,
        rebuildTimer: null,
        copyLockUntil: 0,
        scrollListener: null,
        resizeListener: null
    };

    function getInjector() {
        try {
            if (typeof angular === 'undefined' || !angular.element) return null;
            var injector = angular.element(document).injector();
            return injector || null;
        } catch (e) {
            return null;
        }
    }

    function getService(name) {
        var injector = getInjector();
        if (!injector) return null;
        try { return injector.get(name); } catch (e) { return null; }
    }

    function copyToClipboard(text) {
        // Synchronous textarea-based copy preserves the user gesture and is reliable
        // even if the page is not focused or navigator.clipboard is unavailable.
        var ok = false;
        var ta = document.createElement('textarea');
        ta.value = text;
        ta.setAttribute('readonly', '');
        ta.setAttribute('data-vc-ext', '1');
        ta.style.cssText = 'position:fixed;top:0;left:0;width:1px;height:1px;opacity:0;pointer-events:none;';
        document.body.appendChild(ta);
        ta.focus();
        ta.select();
        try { ok = document.execCommand('copy'); } catch (e) { ok = false; }
        ta.remove();

        if (navigator.clipboard && navigator.clipboard.writeText) {
            try { navigator.clipboard.writeText(text); } catch (e) { /* ignore */ }
        }
        return ok;
    }

    function showToast(message, color) {
        var existing = document.getElementById('vc-ext-toast');
        if (existing) existing.remove();

        var toast = document.createElement('div');
        toast.id = 'vc-ext-toast';
        toast.setAttribute('data-vc-ext', '1');
        toast.textContent = message;
        toast.style.cssText = [
            'position:fixed',
            'bottom:32px',
            'left:50%',
            'transform:translateX(-50%) translateY(20px)',
            'background:' + (color || '#16a34a'),
            'color:#fff',
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
            setTimeout(function () { if (toast.parentNode) toast.parentNode.removeChild(toast); }, 220);
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
                "mainMenuService.addMenuItem({",
                "    path: '" + path + "',",
                "    icon: '" + icon + "',",
                "    title: '" + title + "',",
                "    priority: 100,",
                "    action: function () { $state.go('<state-name>'); },",
                "    permission: '" + permission + "'",
                "});"
            ].join('\n');
        },
        blade: function (id, controller) {
            var bladeId = id || '<bladeId>';
            var ctrl = controller || '<yourModule>.<yourController>';
            return [
                "var blade = {",
                "    id: '" + bladeId + "',",
                "    title: 'platform.blades.<title-key>',",
                "    controller: '" + ctrl + "',",
                "    template: '$(YourModule)/Scripts/blades/" + bladeId + ".tpl.html'",
                "};",
                "bladeNavigationService.showBlade(blade, parentBlade);"
            ].join('\n');
        },
        toolbar: function (controller) {
            var ctrl = controller || '<yourModule>.<yourController>';
            return [
                "toolbarService.register({",
                "    name: 'platform.commands.<command-key>',",
                "    icon: 'fa fa-cog',",
                "    index: 100,",
                "    executeMethod: function (blade) { /* ... */ },",
                "    canExecuteMethod: function (blade) { return true; }",
                "}, '" + ctrl + "');"
            ].join('\n');
        },
        widget: function (group) {
            var g = group || '<containerName>';
            return [
                "widgetService.registerWidget({",
                "    controller: '<yourModule>.<yourWidgetController>',",
                "    template: '$(YourModule)/Scripts/widgets/<widget>.tpl.html'",
                "}, '" + g + "');"
            ].join('\n');
        },
        metaform: function (formName) {
            var name = formName || '<formName>';
            return [
                "metaFormsService.registerMetaFields('" + name + "', [{",
                "    name: '<fieldName>',",
                "    title: 'platform.blades.<form>.labels.<field>',",
                "    valueType: 'ShortText',",
                "    priority: 100",
                "}]);"
            ].join('\n');
        }
    };

    // -------- Mutation filtering --------
    // Avoid an infinite rebuild loop: ignore mutations caused by the inspector's
    // own DOM (overlay root, label text, toast, copy textarea). Anything tagged
    // with [data-vc-ext] is internal.

    function isInternalNode(node) {
        if (!node || node.nodeType !== 1) return false;
        if (node.hasAttribute && node.hasAttribute('data-vc-ext')) return true;
        return !!(node.closest && node.closest('[data-vc-ext]'));
    }

    function hasExternalChange(mutation) {
        if (mutation.type === 'childList') {
            var i;
            for (i = 0; i < mutation.addedNodes.length; i++) {
                if (mutation.addedNodes[i].nodeType === 1 && !isInternalNode(mutation.addedNodes[i])) return true;
            }
            for (i = 0; i < mutation.removedNodes.length; i++) {
                if (mutation.removedNodes[i].nodeType === 1 && !isInternalNode(mutation.removedNodes[i])) return true;
            }
            return false;
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
            hosts.push({ type: 'menu', el: nav, name: '', service: 'mainMenuService' });
        }

        document.querySelectorAll('[ng-model="blade"]').forEach(function (el) {
            var bladeId = el.getAttribute('id') || el.id || '';
            var controller = el.getAttribute('ng-controller') || '';
            hosts.push({ type: 'blade', el: el, name: bladeId, controller: controller });
        });

        document.querySelectorAll('.blade-toolbar').forEach(function (el) {
            var bladeEl = el.closest('[ng-model="blade"]');
            var controller = bladeEl ? (bladeEl.getAttribute('ng-controller') || '') : '';
            var bladeId = bladeEl ? (bladeEl.getAttribute('id') || '') : '';
            hosts.push({ type: 'toolbar', el: el, name: controller || bladeId, controller: controller });
        });

        document.querySelectorAll('[gridster-opts]').forEach(function (el) {
            var group = el.getAttribute('group') || '';
            hosts.push({ type: 'widget', el: el, name: group });
        });

        document.querySelectorAll('[registered-inputs]').forEach(function (el) {
            var bladeEl = el.closest('[ng-model="blade"]');
            var bladeId = bladeEl ? (bladeEl.getAttribute('id') || '') : '';
            var formName = inferMetaFormName(el, bladeId);
            hosts.push({ type: 'metaform', el: el, name: formName || ('host: ' + bladeId), inferredName: formName });
        });

        return hosts;
    }

    function inferMetaFormName(metaformEl, bladeId) {
        try {
            var scope = angular.element(metaformEl).isolateScope() || angular.element(metaformEl).scope();
            if (!scope || !scope.registeredInputs) return null;
            var metaFormsService = getService('platformWebApp.metaFormsService');
            if (!metaFormsService || typeof metaFormsService.getMetaFields !== 'function') return null;
            var candidates = collectKnownMetaFormNames(bladeId);
            for (var i = 0; i < candidates.length; i++) {
                var fields = metaFormsService.getMetaFields(candidates[i]);
                if (fields && fields === scope.registeredInputs) {
                    return candidates[i];
                }
            }
        } catch (e) { /* fall through */ }
        return null;
    }

    function collectKnownMetaFormNames(bladeId) {
        var seeds = ['accountDetails', 'roleDetail', 'roleDetails', 'storeDetail', 'apiAccountDetail', 'oAuthApplicationDetail'];
        if (bladeId) {
            seeds.push(bladeId);
            seeds.push(bladeId.replace(/Detail$/, 'Details'));
            seeds.push(bladeId.replace(/Details$/, 'Detail'));
        }
        return seeds;
    }

    // -------- Counts from registries --------

    function describe(host) {
        var name = host.name || '';
        var count = '';

        if (host.type === 'menu') {
            var menuSvc = getService('platformWebApp.mainMenuService');
            if (menuSvc && menuSvc.menuItems) count = '(' + menuSvc.menuItems.length + ' items)';
        } else if (host.type === 'widget') {
            var widgetSvc = getService('platformWebApp.widgetService');
            if (widgetSvc && widgetSvc.widgetsMap && host.name) {
                var ws = widgetSvc.widgetsMap[host.name] || [];
                count = '(' + ws.length + ' widgets)';
            }
        } else if (host.type === 'toolbar') {
            var tbSvc = getService('platformWebApp.toolbarService');
            if (tbSvc && tbSvc.toolbarCommandsMap && host.controller) {
                var cmds = tbSvc.toolbarCommandsMap[host.controller] || [];
                count = '(' + cmds.length + ' commands)';
            }
        } else if (host.type === 'metaform') {
            var mf = getService('platformWebApp.metaFormsService');
            if (mf && host.inferredName) {
                var fields = mf.getMetaFields(host.inferredName);
                if (fields) count = '(' + fields.length + ' fields)';
            }
        }

        var prefix = '[' + host.type + ']';
        var parts = [prefix];
        if (name) parts.push(':: ' + name);
        if (count) parts.push(count);
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
        root.setAttribute('data-vc-ext', '1');
        root.style.cssText = 'position:fixed;top:0;left:0;width:0;height:0;pointer-events:none;z-index:2147483600;';
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
        label.style.left = rect.left + 'px';
        label.style.top = labelTop + 'px';
        label.style.borderRadius = insideTop ? '0 0 3px 0' : '3px 3px 0 0';
    }

    function makeOverlay(host) {
        var color = TYPE_COLORS[host.type] || TYPE_COLORS.blade;
        var rect = host.el.getBoundingClientRect();
        if (rect.width === 0 || rect.height === 0) return null;

        var box = document.createElement('div');
        box.className = 'vc-ext-overlay';
        box.style.cssText = [
            'position:fixed',
            'pointer-events:none',
            'border:2px dashed ' + color.border,
            'background:' + color.bg,
            'box-sizing:border-box',
            'transition:none',
            'left:' + rect.left + 'px',
            'top:' + rect.top + 'px',
            'width:' + rect.width + 'px',
            'height:' + rect.height + 'px'
        ].join(';');

        var label = document.createElement('div');
        label.className = 'vc-ext-label';
        label.style.cssText = [
            'position:fixed',
            'pointer-events:auto',
            'background:' + color.border,
            'color:#fff',
            'font:600 11px/1.4 Consolas,Menlo,monospace',
            'padding:2px 8px',
            'white-space:nowrap',
            'display:flex',
            'align-items:center',
            'gap:8px',
            'max-width:400px',
            'box-sizing:border-box',
            'overflow:hidden',
            'box-shadow:0 1px 4px rgba(0,0,0,0.25)'
        ].join(';');
        applyLabelPosition(label, rect);

        var text = document.createElement('span');
        text.textContent = describe(host);
        text.style.cssText = 'overflow:hidden;text-overflow:ellipsis;white-space:nowrap;flex:1 1 auto;min-width:0;';
        label.appendChild(text);

        var copyBtn = document.createElement('button');
        copyBtn.type = 'button';
        copyBtn.textContent = 'Copy snippet';
        copyBtn.setAttribute('data-vc-ext-action', 'copy');
        copyBtn.style.cssText = [
            'pointer-events:auto',
            'background:rgba(255,255,255,0.18)',
            'color:#fff',
            'border:1px solid rgba(255,255,255,0.4)',
            'border-radius:2px',
            'padding:1px 6px',
            'font:600 10px/1.4 Consolas,Menlo,monospace',
            'cursor:pointer',
            'flex:0 0 auto'
        ].join(';');
        copyBtn.__vcHost = host;
        label.appendChild(copyBtn);

        return { box: box, label: label, host: host };
    }

    function attachRootDelegate(root) {
        if (root.__vcDelegated) return;
        root.__vcDelegated = true;

        function handleCopy(ev) {
            var btn = ev.target && ev.target.closest && ev.target.closest('button[data-vc-ext-action="copy"]');
            if (!btn) return;
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
            if (!host) return;
            var snippet = snippetFor(host);
            var ok = copyToClipboard(snippet);
            btn.textContent = ok ? 'Copied!' : 'Copy failed';
            setTimeout(function () { if (btn.isConnected) btn.textContent = 'Copy snippet'; }, 1200);
            var typeLabel = '[' + host.type + ']' + (host.name ? ' ' + host.name : '');
            if (ok) {
                showToast('Snippet copied to clipboard\n' + typeLabel, '#16a34a');
            } else {
                showToast('Copy failed — see console\n' + typeLabel, '#dc2626');
                console.warn('[Virto] Copy failed. Snippet:\n' + snippet);
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

    function reposition() {
        state.overlays.forEach(function (entry) {
            var rect = entry.host.el.getBoundingClientRect();
            if (rect.width === 0 || rect.height === 0) {
                entry.box.style.display = 'none';
                entry.label.style.display = 'none';
                return;
            }
            entry.box.style.display = '';
            entry.box.style.left = rect.left + 'px';
            entry.box.style.top = rect.top + 'px';
            entry.box.style.width = rect.width + 'px';
            entry.box.style.height = rect.height + 'px';
            entry.label.style.display = '';
            applyLabelPosition(entry.label, rect);
        });
    }

    function rebuild() {
        var root = ensureRoot();
        while (root.firstChild) root.removeChild(root.firstChild);
        state.overlays = [];

        findHosts().forEach(function (host) {
            var entry = makeOverlay(host);
            if (entry) {
                root.appendChild(entry.box);
                root.appendChild(entry.label);
                state.overlays.push(entry);
            }
        });
    }

    function scheduleRebuild() {
        if (state.rebuildTimer) return;
        state.rebuildTimer = setTimeout(function () {
            state.rebuildTimer = null;
            // Skip if a copy is in progress so the user's button isn't torn down mid-press
            if (state.copyLockUntil && Date.now() < state.copyLockUntil) {
                scheduleRebuild();
                return;
            }
            if (state.active) rebuild();
        }, 120);
    }

    function scheduleReposition() {
        if (state.rafScheduled) return;
        state.rafScheduled = true;
        requestAnimationFrame(function () {
            state.rafScheduled = false;
            if (state.active) reposition();
        });
    }

    // -------- Public API --------

    function show() {
        if (state.active) {
            rebuild();
            return;
        }
        state.active = true;
        rebuild();

        state.observer = new MutationObserver(function (mutations) {
            for (var i = 0; i < mutations.length; i++) {
                if (hasExternalChange(mutations[i])) {
                    scheduleRebuild();
                    return;
                }
            }
        });
        state.observer.observe(document.body, { childList: true, subtree: true, attributes: true, attributeFilter: ['id', 'ng-model', 'group', 'registered-inputs'] });

        state.scrollListener = function () { scheduleReposition(); };
        state.resizeListener = function () { scheduleReposition(); };
        window.addEventListener('scroll', state.scrollListener, true);
        window.addEventListener('resize', state.resizeListener);

        console.log('[Virto] Extension-point overlay enabled. Type vcExt.hide() to remove.');
    }

    function hide() {
        state.active = false;
        if (state.observer) {
            state.observer.disconnect();
            state.observer = null;
        }
        if (state.scrollListener) {
            window.removeEventListener('scroll', state.scrollListener, true);
            state.scrollListener = null;
        }
        if (state.resizeListener) {
            window.removeEventListener('resize', state.resizeListener);
            state.resizeListener = null;
        }
        var root = document.getElementById(ROOT_ID);
        if (root) root.parentNode.removeChild(root);
        state.overlays = [];
    }

    function toggle() {
        if (state.active) hide(); else show();
    }

    function list() {
        var rows = [];
        var menuSvc = getService('platformWebApp.mainMenuService');
        if (menuSvc && menuSvc.menuItems) {
            menuSvc.menuItems.forEach(function (m) {
                rows.push({ type: 'menu', name: m.path, detail: m.title || '', priority: m.priority, permission: m.permission || '' });
            });
        }

        var bladeSvc = getService('platformWebApp.bladeNavigationService');
        if (bladeSvc && bladeSvc.blades) {
            Object.keys(bladeSvc.blades).forEach(function (state) {
                (bladeSvc.blades[state] || []).forEach(function (b) {
                    rows.push({ type: 'blade', name: b.id, detail: b.controller || '', priority: '', permission: b.updatePermission || '' });
                });
            });
        }

        var widgetSvc = getService('platformWebApp.widgetService');
        if (widgetSvc && widgetSvc.widgetsMap) {
            Object.keys(widgetSvc.widgetsMap).forEach(function (group) {
                (widgetSvc.widgetsMap[group] || []).forEach(function (w) {
                    rows.push({ type: 'widget', name: group, detail: w.controller || '', priority: '', permission: w.permission || '' });
                });
            });
        }

        var tbSvc = getService('platformWebApp.toolbarService');
        if (tbSvc && tbSvc.toolbarCommandsMap) {
            Object.keys(tbSvc.toolbarCommandsMap).forEach(function (ctrl) {
                (tbSvc.toolbarCommandsMap[ctrl] || []).forEach(function (cmd) {
                    rows.push({ type: 'toolbar', name: ctrl, detail: cmd.name || '', priority: cmd.index, permission: cmd.permission || '' });
                });
            });
        }

        var mfSvc = getService('platformWebApp.metaFormsService');
        if (mfSvc) {
            collectKnownMetaFormNames('').forEach(function (n) {
                var fields = mfSvc.getMetaFields(n);
                if (fields && fields.length) {
                    fields.forEach(function (f) {
                        rows.push({ type: 'metaform', name: n, detail: f.name || '', priority: f.priority, permission: '' });
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
            case 'menu':     snippet = SNIPPETS.menu(null); break;
            case 'blade':    snippet = SNIPPETS.blade(id, null); break;
            case 'toolbar':  snippet = SNIPPETS.toolbar(id); break;
            case 'widget':   snippet = SNIPPETS.widget(id); break;
            case 'metaform': snippet = SNIPPETS.metaform(id); break;
            default:
                console.warn("[Virto] Unknown type. Use one of: 'menu', 'blade', 'toolbar', 'widget', 'metaform'");
                return;
        }
        var ok = copyToClipboard(snippet);
        if (ok) {
            showToast('Snippet copied to clipboard\n[' + type + ']' + (id ? ' ' + id : ''), '#16a34a');
            console.log('[Virto] Snippet copied to clipboard:\n' + snippet);
        } else {
            showToast('Copy failed — see console\n[' + type + ']', '#dc2626');
            console.warn('[Virto] Copy failed. Snippet:\n' + snippet);
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
