angular.module('platformWebApp')
    // Global search: a registry of search providers queried from the header search box.
    // Developer guide: docs/developer-guide/global-search.md
    //
    // A provider is registered once, e.g. from a module's run block:
    //
    //   globalSearchService.registerProvider({
    //       id: 'orders',                          // unique id
    //       title: 'orders.search.group-title',    // group title (translation key or text)
    //       order: 100,                            // group position, lower first (platform: 10-50, modules: 100+)
    //       permission: 'order:read',              // optional: provider skipped without it (string or array, any of)
    //       isAvailable: function () { ... },      // optional: false when the user has nothing to find here
    //       minLength: 2,                          // optional: shortest query the provider answers (default 1)
    //       delay: 250,                            // optional: extra debounce in ms, for providers that call an API
    //       aliases: ['order', 'orders'],          // optional: "order: CO123" searches this provider only
    //       match: /^CO\d+/i,                      // optional: a query like this puts the group first (RegExp or function)
    //       search: function (query, context) {    // items, { items, total, showAll }, or a promise of either
    //           return $http.post('api/order/customerOrders/search', { keyword: query, take: context.take },
    //               { timeout: context.cancel }).then(...);
    //       }
    //   });
    //
    // Items: { id, title, description?, icon? (css classes), iconUrl?, badge?, meta?, permission?, external?, execute() }.
    // Items with a permission the user lacks are dropped. Client-side providers can use rankItems() below.
    .factory('platformWebApp.globalSearchService', ['$q', '$state', '$timeout', '$translate', 'platformWebApp.authService', 'platformWebApp.bladeNavigationService',
        function ($q, $state, $timeout, $translate, authService, bladeNavigationService) {
            var providers = [];
            var defaultTake = 8;
            var scopedTake = 20;
            var providerTimeout = 8000;

            function hasPermission(permission) {
                if (!permission || (angular.isArray(permission) && !permission.length)) {
                    return true;
                }
                var list = angular.isArray(permission) ? permission : [permission];
                return _.some(list, function (p) {
                    return authService.checkPermission(p);
                });
            }

            // A provider takes part when the user has its permission and it reports something to search
            function isUsable(provider) {
                return hasPermission(provider.permission) &&
                    (!angular.isFunction(provider.isAvailable) || provider.isAvailable());
            }

            function translate(text) {
                if (!text) {
                    return '';
                }
                var result = $translate.instant(text);
                return result || text;
            }

            // Match score of a text against the query words: every word must be found.
            // Whole-text prefix > word prefix > substring; earlier fields weigh more.
            function scoreText(text, words) {
                if (!text) {
                    return 0;
                }
                var value = text.toString().toLowerCase();
                var total = 0;
                for (var i = 0; i < words.length; i++) {
                    var word = words[i];
                    var index = value.indexOf(word);
                    if (index < 0) {
                        return 0;
                    }
                    if (index === 0) {
                        total += 3;
                    } else if (/[\s\-_./>]/.test(value.charAt(index - 1))) {
                        total += 2;
                    } else {
                        total += 1;
                    }
                }
                return total;
            }

            // Rank client-side items: fields is a function returning the searchable texts of an item, most important first
            function rankItems(items, query, fields, take) {
                var words = query.toLowerCase().split(/\s+/).filter(function (w) { return w; });
                if (!words.length) {
                    return [];
                }
                var ranked = [];
                angular.forEach(items, function (item) {
                    var texts = fields(item);
                    var best = 0;
                    // all words in one field, or spread over the fields
                    for (var i = 0; i < texts.length; i++) {
                        var s = scoreText(texts[i], words);
                        if (s) {
                            best = Math.max(best, s * (texts.length - i));
                        }
                    }
                    if (!best && scoreText(texts.join(' '), words)) {
                        best = 1;
                    }
                    if (best) {
                        ranked.push({ item: item, score: best });
                    }
                });
                ranked.sort(function (a, b) {
                    return b.score - a.score;
                });
                return _.map(ranked.slice(0, take || defaultTake), function (r) { return r.item; });
            }

            // The words that scope a search to one provider ("orders: CO123"): its aliases and its translated title
            function aliasesOf(provider) {
                return _.map(_.compact((provider.aliases || []).concat([translate(provider.title)])), function (a) {
                    return a.toString().toLowerCase();
                });
            }

            // "alias: text" -> { text, scope: [providers] } when the alias belongs to a provider; the plain query otherwise
            function parseQuery(query) {
                var text = (query || '').trim();
                var parts = /^([^:\s][^:]{0,40}?)\s*:\s*(.*)$/.exec(text);
                if (parts) {
                    var alias = parts[1].toLowerCase();
                    var scope = _.filter(providers, function (p) { return _.contains(aliasesOf(p), alias); });
                    if (scope.length) {
                        return { text: parts[2].trim(), scope: scope };
                    }
                }
                return { text: text, scope: null };
            }

            function matchesPattern(provider, text) {
                if (!provider.match) {
                    return false;
                }
                return angular.isFunction(provider.match) ? !!provider.match(text) : provider.match.test(text);
            }

            // A provider answer: an array of items or { items, total, showAll }
            function normalize(answer, take) {
                var result = angular.isArray(answer) || !answer ? { items: answer || [] } : answer;
                var items = _.filter(result.items || [], function (item) { return item && hasPermission(item.permission); });
                return {
                    items: items.slice(0, take),
                    total: angular.isNumber(result.total) ? result.total : null,
                    showAll: angular.isFunction(result.showAll) ? result.showAll : null
                };
            }

            // Resolves with the open blade that has the given id (or satisfies the given predicate) once it is ready
            // (has the given method). Use a predicate when a blade id is shared, e.g. by lists replacing each other.
            function waitForBlade(idOrPredicate, method, timeoutMs) {
                var deferred = $q.defer();
                var deadline = Date.now() + (timeoutMs || 5000);
                var test = angular.isFunction(idOrPredicate) ? idOrPredicate : function (b) { return b.id === idOrPredicate; };
                (function poll() {
                    var blade = _.find(bladeNavigationService.stateBlades(), test);
                    if (blade && (!method || angular.isFunction(blade[method]))) {
                        deferred.resolve(blade);
                    } else if (Date.now() > deadline) {
                        deferred.reject(new Error('The blade did not open'));
                    } else {
                        $timeout(poll, 100);
                    }
                })();
                return deferred.promise;
            }

            // Switches to a workspace state (nothing to do when it is already active)
            function goToWorkspace(stateName, params) {
                if ($state.current.name === stateName && !params) {
                    return $q.resolve();
                }
                return $q.when($state.go(stateName, params));
            }

            var service = {
                registerProvider: function (provider) {
                    if (!provider || !provider.id || !angular.isFunction(provider.search)) {
                        throw new Error('A search provider needs an id and a search function');
                    }
                    service.unregisterProvider(provider.id);
                    providers.push(provider);
                    providers.sort(function (a, b) {
                        return (a.order || 0) - (b.order || 0);
                    });
                },
                unregisterProvider: function (id) {
                    providers = _.reject(providers, function (p) { return p.id === id; });
                },
                getProviders: function () {
                    return providers.slice();
                },
                // True when the user can use search at all (at least one provider is usable); the search box is hidden otherwise
                isAvailable: function () {
                    return _.some(providers, isUsable);
                },
                hasPermission: hasPermission,
                rankItems: rankItems,
                translate: translate,
                parseQuery: parseQuery,
                waitForBlade: waitForBlade,
                goToWorkspace: goToWorkspace,
                // Opens a blade from a search result: switches to the workspace, waits for the parent blade
                // (when given) and shows the blade under it.
                //   { state, stateParams?, parentBladeId?, parentReady? (method the parent must have), blade }
                openBlade: function (options) {
                    return goToWorkspace(options.state, options.stateParams)
                        .then(function () {
                            return options.parentBladeId ? waitForBlade(options.parentBladeId, options.parentReady) : null;
                        })
                        .then(function (parentBlade) {
                            bladeNavigationService.showBlade(options.blade, parentBlade || undefined);
                            return options.blade;
                        });
                },
                // Runs every usable provider. Groups are reported through options.onGroup(groups) as each provider
                // answers, in display order, each { provider, title, status: 'pending' | 'done' | 'error', items,
                // total, showAll }. The promise resolves with the groups that found something, once all are done.
                // A provider that fails or times out ends as 'error' instead of failing the whole search.
                search: function (query, options) {
                    options = options || {};
                    var parsed = parseQuery(query);
                    var text = parsed.text;
                    var cancel = $q.defer();
                    var cancelled = false;
                    var context = {
                        take: options.take || (parsed.scope ? scopedTake : defaultTake),
                        cancel: cancel.promise,
                        scoped: !!parsed.scope
                    };
                    var usable = _.filter(parsed.scope || providers, function (p) {
                        return text.length >= (p.minLength || 1) && isUsable(p);
                    });
                    // providers whose pattern matches the query come first; the rest keep their order
                    var matched = _.filter(usable, function (p) { return matchesPattern(p, text); });
                    usable = matched.concat(_.difference(usable, matched));

                    var groups = _.map(usable, function (provider) {
                        return { provider: provider, title: translate(provider.title), status: 'pending', items: [], total: null, showAll: null };
                    });
                    function report() {
                        if (!cancelled && angular.isFunction(options.onGroup)) {
                            options.onGroup(groups.slice());
                        }
                    }

                    var calls = _.map(groups, function (group) {
                        var provider = group.provider;
                        var done = $q.defer();
                        // whichever comes first: the provider's answer or the timeout (AngularJS $q has no race())
                        var timer = null;
                        var delayTimer = $timeout(function () {
                            if (cancelled) {
                                done.resolve();
                                return;
                            }
                            timer = setTimeout(function () {
                                done.resolve({ status: 'error' });
                            }, providerTimeout);
                            $q.when()
                                .then(function () { return provider.search(text, context); })
                                .then(function (answer) {
                                    done.resolve(angular.extend({ status: 'done' }, normalize(answer, context.take)));
                                })
                                .catch(function () {
                                    done.resolve({ status: 'error' });
                                });
                        }, provider.delay || 0);
                        return done.promise.then(function (result) {
                            clearTimeout(timer);
                            $timeout.cancel(delayTimer);
                            if (cancelled || !result) {
                                return;
                            }
                            angular.extend(group, result);
                            report();
                        });
                    });

                    report();
                    var promise = $q.all(calls).then(function () {
                        return _.filter(groups, function (g) { return g.status === 'done' && g.items.length; });
                    });
                    promise.cancel = function () {
                        cancelled = true;
                        cancel.resolve();
                    };
                    return promise;
                }
            };
            return service;
        }])
    // Built-in client-side providers: main menu, apps and settings
    .run(['$state', '$window', '$translate', '$rootScope', 'platformWebApp.globalSearchService', 'platformWebApp.mainMenuService', 'platformWebApp.webApps', 'platformWebApp.settingsV2',
        function ($state, $window, $translate, $rootScope, globalSearchService, mainMenuService, webApps, settingsV2) {

            function getAppPlacement(app) {
                if (app && app.placement) {
                    return app.placement;
                }
                return app && app.supportEmbeddedMode ? 'MainMenu' : 'AppMenu';
            }

            // Apps from the apps menu; embedded (MainMenu) apps are main menu items and are found there
            function visibleApps(apps) {
                return _.filter(apps || [], function (app) {
                    return app.id !== 'platform' && getAppPlacement(app) === 'AppMenu';
                });
            }

            // Main menu: every item with an action the user may see; the path of parent groups is the description
            function permittedMenuItems() {
                return _.filter(mainMenuService.menuItems, function (m) {
                    return angular.isFunction(m.action) && m.path !== 'more' &&
                        globalSearchService.hasPermission(m.permission) &&
                        (!m.group || globalSearchService.hasPermission(m.group.permission));
                });
            }
            globalSearchService.registerProvider({
                id: 'platform.menu',
                title: 'platform.search.groups.menu',
                order: 10,
                aliases: ['menu'],
                // Home and the user profile are open to everyone; search is offered once the user may open
                // at least one permission-protected item
                isAvailable: function () {
                    return _.some(mainMenuService.menuItems, function (m) {
                        return angular.isFunction(m.action) && m.permission && globalSearchService.hasPermission(m.permission);
                    });
                },
                search: function (query, context) {
                    var items = permittedMenuItems();
                    var ranked = globalSearchService.rankItems(items, query, function (m) {
                        return [globalSearchService.translate(m.title), m.group ? globalSearchService.translate(m.group.title) : '', m.path];
                    }, context.take);
                    return _.map(ranked, function (m) {
                        return {
                            id: m.path,
                            title: globalSearchService.translate(m.title),
                            description: m.group ? globalSearchService.translate(m.group.title) : '',
                            icon: m.iconUrl ? null : m.icon,
                            iconUrl: m.iconUrl,
                            execute: function () { m.action(); }
                        };
                    });
                }
            });

            // Apps: applications from the apps menu the user may open; they open in a new tab
            globalSearchService.registerProvider({
                id: 'platform.apps',
                title: 'platform.search.groups.apps',
                order: 20,
                aliases: ['app', 'apps'],
                // the apps list is loaded at startup and cached by webApps
                isAvailable: function () {
                    return _.some(visibleApps(webApps.apps), function (app) {
                        return globalSearchService.hasPermission(app.permission);
                    });
                },
                search: function (query, context) {
                    return webApps.loadApps().then(function (apps) {
                        var visible = visibleApps(apps);
                        var ranked = globalSearchService.rankItems(visible, query, function (app) {
                            return [app.title, app.description, app.id];
                        }, context.take);
                        return _.map(ranked, function (app) {
                            return {
                                id: app.id,
                                title: app.title,
                                description: app.description || '',
                                icon: app.iconUrl ? null : 'fa fa-th-large',
                                iconUrl: app.iconUrl,
                                permission: app.permission,
                                external: true,
                                execute: function () {
                                    $window.open(app.relativeUrl, '_blank');
                                }
                            };
                        });
                    });
                }
            });

            // Settings: the global settings schema is loaded once per session and filtered on the client;
            // a result opens the setting in the settings workspace (deep link)
            var settingsSchema = null;
            // 'VirtoCommerce.Search.IndexingJobs.ScanInterval' -> 'Scan interval' (settings without a translated title)
            function humanize(name) {
                var last = (name || '').split('.').pop();
                var words = last.replace(/([a-z0-9])([A-Z])/g, '$1 $2').replace(/[_-]+/g, ' ').toLowerCase();
                return words.charAt(0).toUpperCase() + words.slice(1);
            }
            $rootScope.$on('loginStatusChanged', function () {
                settingsSchema = null;
            });
            globalSearchService.registerProvider({
                id: 'platform.settings',
                title: 'platform.search.groups.settings',
                order: 30,
                minLength: 2,
                aliases: ['setting', 'settings'],
                permission: 'platform:setting:access',
                search: function (query, context) {
                    if (!settingsSchema) {
                        settingsSchema = settingsV2.getGlobalSchema({}).$promise.catch(function (error) {
                            settingsSchema = null;
                            throw error;
                        });
                    }
                    return settingsSchema.then(function (schema) {
                        var settings = _.map(schema, function (s) {
                            var key = 'settings.' + s.name + '.title';
                            var title = $translate.instant(key);
                            return { setting: s, title: title !== key ? title : (s.displayName || humanize(s.name)) };
                        });
                        var ranked = globalSearchService.rankItems(settings, query, function (x) {
                            return [x.title, x.setting.groupName, x.setting.name];
                        }, context.take);
                        return _.map(ranked, function (x) {
                            return {
                                id: x.setting.name,
                                title: x.title,
                                description: (x.setting.groupName || '').split('|').join(' › '),
                                icon: 'fa fa-sliders',
                                execute: function () {
                                    $state.go('workspace.settings', { group: x.setting.groupName, setting: x.setting.name });
                                }
                            };
                        });
                    });
                }
            });
        }])
    .directive('vaGlobalSearch', ['$document', '$timeout', 'platformWebApp.globalSearchService', function ($document, $timeout, globalSearchService) {
        return {
            restrict: 'E',
            replace: true,
            scope: {},
            templateUrl: '$(Platform)/Scripts/app/navigation/search/globalSearch.tpl.html',
            link: function (scope, element) {
                var pending;
                function getInput() {
                    return element[0].querySelector('.global-search__input');
                }
                var debounce;
                // the highlighted entry is kept by reference, so it stays put while later groups arrive
                var activeEntry = null;

                scope.isMac = /Mac|iPhone|iPad/.test(navigator.platform || navigator.userAgent);
                scope.state = { query: '', text: '', open: false, loading: false, groups: [], active: -1, expanded: false };
                scope.isAvailable = globalSearchService.isAvailable;

                // Everything the arrow keys move through: the items, then each group's "Show all" row
                function entries() {
                    return _.flatten(_.map(scope.state.groups, function (g) {
                        return g.showAllEntry ? g.items.concat([g.showAllEntry]) : g.items;
                    }));
                }

                function setActiveEntry(entry) {
                    activeEntry = entry || null;
                    scope.state.active = entry ? entry.$index : -1;
                }

                // Groups as shown: pending and failed groups keep their place, empty finished ones are left out
                function applyGroups(groups) {
                    scope.state.groups = _.filter(groups, function (g) {
                        return g.status !== 'done' || g.items.length;
                    });
                    angular.forEach(scope.state.groups, function (g) {
                        g.showAllEntry = g.status === 'done' && g.showAll && g.total > g.items.length ?
                            { isShowAll: true, total: g.total, execute: g.showAll } : null;
                    });
                    var list = entries();
                    angular.forEach(list, function (entry, index) { entry.$index = index; });
                    setActiveEntry(_.contains(list, activeEntry) ? activeEntry : list[0]);
                    scope.state.loading = _.some(groups, function (g) { return g.status === 'pending'; });
                }

                function runSearch() {
                    var query = scope.state.query.trim();
                    if (pending) {
                        pending.cancel();
                        pending = null;
                    }
                    activeEntry = null;
                    scope.state.text = globalSearchService.parseQuery(query).text;
                    if (!query) {
                        scope.state.groups = [];
                        scope.state.loading = false;
                        scope.state.active = -1;
                        return;
                    }
                    scope.state.loading = true;
                    var request = globalSearchService.search(query, {
                        onGroup: function (groups) {
                            if (pending === request || !request) {
                                applyGroups(groups);
                            }
                        }
                    });
                    pending = request;
                    request.finally(function () {
                        if (pending === request) {
                            scope.state.loading = false;
                            pending = null;
                        }
                    });
                }

                scope.onChange = function () {
                    scope.state.open = true;
                    $timeout.cancel(debounce);
                    debounce = $timeout(runSearch, 150);
                };

                scope.focus = function () {
                    // also reopens the results of a query left in the box (the input may still have focus)
                    scope.state.open = true;
                    scope.state.expanded = true;
                    $timeout(function () { getInput().focus(); });
                };

                scope.onFocus = function () {
                    scope.state.open = true;
                };

                scope.close = function () {
                    scope.state.open = false;
                    scope.state.expanded = false;
                    getInput().blur();
                };

                scope.clear = function () {
                    scope.state.query = '';
                    runSearch();
                    getInput().focus();
                };

                scope.select = function (entry) {
                    if (!entry) {
                        return;
                    }
                    scope.state.query = '';
                    scope.state.groups = [];
                    if (pending) {
                        pending.cancel();
                        pending = null;
                    }
                    scope.close();
                    entry.execute();
                };

                scope.setActive = function (entry) {
                    setActiveEntry(entry);
                };

                scope.onKeydown = function (event) {
                    var list = entries();
                    var index = scope.state.active;
                    switch (event.key) {
                        case 'ArrowDown':
                            event.preventDefault();
                            setActiveEntry(list.length ? list[(index + 1) % list.length] : null);
                            break;
                        case 'ArrowUp':
                            event.preventDefault();
                            setActiveEntry(list.length ? list[(index - 1 + list.length) % list.length] : null);
                            break;
                        case 'Enter':
                            event.preventDefault();
                            scope.select(list[index]);
                            break;
                        case 'Escape':
                            event.preventDefault();
                            if (scope.state.query) {
                                scope.state.query = '';
                                runSearch();
                            } else {
                                scope.close();
                            }
                            break;
                    }
                    $timeout(function () {
                        var active = element[0].querySelector('.global-search__item--active');
                        if (active && active.scrollIntoView) {
                            active.scrollIntoView({ block: 'nearest' });
                        }
                    });
                };

                // Ctrl+K / Cmd+K focuses the search box from anywhere
                function onShortcut(event) {
                    if ((event.ctrlKey || event.metaKey) && !event.altKey && (event.key === 'k' || event.key === 'K')) {
                        if (!globalSearchService.isAvailable()) {
                            return;
                        }
                        event.preventDefault();
                        scope.$apply(scope.focus);
                    }
                }

                function closeFromOutside() {
                    scope.$apply(function () {
                        scope.state.open = false;
                        scope.state.expanded = false;
                    });
                    getInput().blur();
                }

                // A press outside the search closes it. The root itself is hit only through the mobile backdrop
                // (::before): that closes on its own click instead (onBackdropClick), because closing on the press
                // removes the backdrop and the tap's click would land on the blade or button underneath.
                function onOutsideClick(event) {
                    var outside = event.target !== element[0] && !element[0].contains(event.target);
                    if (outside && (scope.state.open || scope.state.expanded)) {
                        closeFromOutside();
                    }
                }

                function onBackdropClick(event) {
                    if (event.target === element[0] && (scope.state.open || scope.state.expanded)) {
                        event.preventDefault();
                        event.stopPropagation();
                        closeFromOutside();
                    }
                }

                $document.on('keydown', onShortcut);
                $document.on('mousedown touchstart', onOutsideClick);
                element.on('click', onBackdropClick);
                scope.$on('$destroy', function () {
                    $document.off('keydown', onShortcut);
                    $document.off('mousedown touchstart', onOutsideClick);
                    element.off('click', onBackdropClick);
                    $timeout.cancel(debounce);
                    if (pending) {
                        pending.cancel();
                    }
                });
            }
        };
    }])
    // Wraps the matched words of a text in <mark> (HTML-escaped)
    .filter('vaHighlight', ['$sce', function ($sce) {
        function escape(text) {
            return text.replace(/[&<>"']/g, function (c) {
                return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
            });
        }
        return function (text, query) {
            text = escape((text || '').toString());
            var words = (query || '').trim().split(/\s+/).filter(function (w) { return w; })
                .map(function (w) { return escape(w).replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); });
            if (!words.length) {
                return $sce.trustAsHtml(text);
            }
            var pattern = new RegExp('(' + words.join('|') + ')', 'gi');
            return $sce.trustAsHtml(text.replace(pattern, '<mark>$1</mark>'));
        };
    }]);
