angular.module('platformWebApp')
    // Global search: a registry of search providers queried from the header search box.
    //
    // A provider is registered once (e.g. from a module's run block):
    //
    //   globalSearchService.registerProvider({
    //       id: 'orders',                          // unique id
    //       title: 'orders.search.group-title',    // group title (translation key or text)
    //       order: 200,                            // group position, lower first
    //       permission: 'order:read',              // optional: provider skipped without it (string or array, any of)
    //       minLength: 3,                          // optional: shortest query the provider answers (default 1)
    //       isAvailable: function () { ... },      // optional: false when the user has nothing to find here
    //       search: function (query, context) {    // returns items or a promise of items
    //           return $http.get('api/orders/search', { params: { keyword: query, take: context.take }, timeout: context.cancel })
    //               .then(function (r) { return r.data.results.map(function (o) {
    //                   return { id: o.id, title: o.number, description: o.customerName, icon: 'fa fa-file-text',
    //                            execute: function () { ... open the order blade ... } };
    //               }); });
    //       }
    //   });
    //
    // Items: { id, title, description?, icon? (css classes), iconUrl?, permission?, execute() }.
    // Items with a permission the user lacks are dropped. Client-side providers can use rankItems() below.
    .factory('platformWebApp.globalSearchService', ['$q', '$translate', 'platformWebApp.authService', function ($q, $translate, authService) {
        var providers = [];
        var defaultTake = 8;
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
            // Runs every permitted provider; resolves to [{ provider, title, items }] in provider order.
            // A provider that fails or times out is skipped instead of failing the whole search.
            search: function (query, options) {
                options = options || {};
                var text = (query || '').trim();
                var cancel = $q.defer();
                var permitted = _.filter(providers, function (p) {
                    return text.length >= (p.minLength || 1) && isUsable(p);
                });
                var context = { take: options.take || defaultTake, cancel: cancel.promise };

                var calls = _.map(permitted, function (provider) {
                    // whichever comes first: the provider's answer or the timeout (AngularJS $q has no race())
                    var done = $q.defer();
                    var timer = setTimeout(function () { done.resolve(null); }, providerTimeout);
                    $q.when()
                        .then(function () { return provider.search(text, context); })
                        .then(function (items) {
                            items = _.filter(items || [], function (item) { return item && hasPermission(item.permission); });
                            done.resolve({ provider: provider, title: translate(provider.title), items: items.slice(0, context.take) });
                        })
                        .catch(function () { done.resolve(null); });
                    return done.promise.finally(function () { clearTimeout(timer); });
                });

                var promise = $q.all(calls).then(function (groups) {
                    return _.filter(groups, function (g) { return g && g.items.length; });
                });
                promise.cancel = function () { cancel.resolve(); };
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

                scope.isMac = /Mac|iPhone|iPad/.test(navigator.platform || navigator.userAgent);
                scope.state = { query: '', open: false, loading: false, groups: [], active: -1, expanded: false };
                scope.isAvailable = globalSearchService.isAvailable;

                function flatItems() {
                    return _.flatten(_.map(scope.state.groups, function (g) { return g.items; }));
                }

                function runSearch() {
                    var query = scope.state.query.trim();
                    if (pending) {
                        pending.cancel();
                        pending = null;
                    }
                    if (!query) {
                        scope.state.groups = [];
                        scope.state.loading = false;
                        scope.state.active = -1;
                        return;
                    }
                    scope.state.loading = true;
                    var request = globalSearchService.search(query);
                    pending = request;
                    request.then(function (groups) {
                        if (pending !== request) {
                            return;
                        }
                        var index = 0;
                        angular.forEach(groups, function (g) {
                            angular.forEach(g.items, function (item) { item.$index = index++; });
                        });
                        scope.state.groups = groups;
                        scope.state.active = index ? 0 : -1;
                    }).finally(function () {
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

                scope.select = function (item) {
                    if (!item) {
                        return;
                    }
                    scope.state.query = '';
                    scope.state.groups = [];
                    scope.close();
                    item.execute();
                };

                scope.setActive = function (item) {
                    scope.state.active = item.$index;
                };

                scope.onKeydown = function (event) {
                    var items = flatItems();
                    switch (event.key) {
                        case 'ArrowDown':
                            event.preventDefault();
                            scope.state.active = items.length ? (scope.state.active + 1) % items.length : -1;
                            break;
                        case 'ArrowUp':
                            event.preventDefault();
                            scope.state.active = items.length ? (scope.state.active - 1 + items.length) % items.length : -1;
                            break;
                        case 'Enter':
                            event.preventDefault();
                            scope.select(items[scope.state.active]);
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

                function onOutsideClick(event) {
                    // the root itself is hit only through the mobile backdrop (::before), which counts as outside
                    var outside = event.target === element[0] || !element[0].contains(event.target);
                    if (outside && (scope.state.open || scope.state.expanded)) {
                        scope.$apply(function () {
                            scope.state.open = false;
                            scope.state.expanded = false;
                        });
                        getInput().blur();
                    }
                }

                $document.on('keydown', onShortcut);
                $document.on('mousedown touchstart', onOutsideClick);
                scope.$on('$destroy', function () {
                    $document.off('keydown', onShortcut);
                    $document.off('mousedown touchstart', onOutsideClick);
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
