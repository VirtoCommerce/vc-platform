# Global Search — Developer Guide

## Overview

The search box in the back-office header (**Ctrl+K** / **⌘K**) searches the platform and every installed module from one place. The platform does not know what a module can find: each module registers one or more **search providers** with `platformWebApp.globalSearchService` from its AngularJS `run` block, and the search box shows the results of all providers grouped by provider.

A provider can answer in two ways:

- **Sync (client-side):** filter data that is already in the browser (menu items, a cached list) and return the items immediately.
- **Async (API):** call the module's search endpoint and return a promise. The group shows *Searching…* until it answers and *Search failed* if it fails or takes longer than 8 seconds; other groups are not held back.

Built-in providers:

| Provider id | Group | Source | Permission | Aliases |
|---|---|---|---|---|
| `platform.menu` | Menu | main menu items (client) | per menu item | `menu` |
| `platform.apps` | Apps | apps menu (client) | per app | `app`, `apps` |
| `platform.settings` | Settings | global settings schema, loaded once per session | `platform:setting:access` | `setting`, `settings` |
| `platform.security.users` | Users | `api/platform/security/users/search` | `platform:security:read` + `platform:security:access` | `user`, `users` |
| `platform.security.roles` | Roles | `api/platform/security/roles/search` | `platform:security:read` + `platform:security:access` | `role`, `roles` |

Source: `src/VirtoCommerce.Platform.Web/wwwroot/js/app/navigation/search/globalSearch.js` (service, built-in providers, search box) and `js/app/security/securitySearch.js` (an API provider to copy from).

## How a search runs

```
keystroke ──► 150 ms debounce ──► parse "alias: text" ──► usable providers (permission, isAvailable, minLength)
                                                        │
              sync providers ───────────────────────────┼──► group rendered immediately
              async providers ── wait provider.delay ───┴──► API call (context.cancel) ──► group rendered on arrival
                                                                   │
              8 s without an answer, or an error ──────────────────┴──► group shows "Search failed"

next keystroke ──► previous search cancelled: pending delays dropped, context.cancel resolved, late answers ignored
```

- Groups keep a fixed position (provider `order`), so the list does not jump when a slow group arrives. A provider whose `match` fits the query moves to the top.
- The highlighted row stays on the same item while other groups arrive.
- A provider that finishes with no items is hidden; a pending or failed one keeps its place.

## Quick start

### 1. Sync provider (client-side data)

```javascript
angular.module('virtoCommerce.storeModule')
    .run(['platformWebApp.globalSearchService', 'virtoCommerce.storeModule.stores',
        function (globalSearchService, stores) {
            var cache = null; // a small list: load once, filter in the browser

            globalSearchService.registerProvider({
                id: 'stores',
                title: 'stores.search.group-title',   // translation key
                order: 130,
                permission: 'store:read',
                aliases: ['store', 'stores'],
                search: function (query, context) {
                    cache = cache || stores.search({ take: 1000 }).$promise.then(function (r) { return r.results; });
                    return cache.then(function (list) {
                        var ranked = globalSearchService.rankItems(list, query, function (s) {
                            return [s.name, s.id, s.url];  // searchable texts, most important first
                        }, context.take);
                        return ranked.map(function (s) {
                            return {
                                id: s.id,
                                title: s.name,
                                description: s.url,
                                icon: 'fas fa-store',
                                execute: function () {
                                    globalSearchService.openBlade({
                                        state: 'workspace.storeModule',
                                        parentBladeId: 'storeList',      // the id of the module's list blade
                                        blade: {
                                            id: 'storeDetails',
                                            currentEntityId: s.id,
                                            title: s.name,
                                            controller: 'virtoCommerce.storeModule.storeDetailController',
                                            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/store-detail.tpl.html'
                                        }
                                    });
                                }
                            };
                        });
                    });
                }
            });
        }]);
```

A provider that returns a plain array from memory (no promise) is fully synchronous.

### 2. Async provider (API search)

```javascript
angular.module('virtoCommerce.orderModule')
    .run(['$http', 'platformWebApp.globalSearchService', function ($http, globalSearchService) {
        globalSearchService.registerProvider({
            id: 'orders',
            title: 'orders.search.group-title',
            order: 100,
            permission: 'order:read',
            minLength: 2,          // don't call the API for one letter
            delay: 250,            // wait for a pause in typing before calling the API
            aliases: ['order', 'orders'],
            match: /^CO\d{3,}/i,   // an order number: show Orders first
            search: function (query, context) {
                return $http.post('api/order/customerOrders/search',
                    { keyword: query, take: context.take, sort: 'createdDate:desc' },
                    { timeout: context.cancel })                       // aborted when the user types on
                    .then(function (response) {
                        return {
                            total: response.data.totalCount,           // "Show all 245"
                            showAll: function () { /* open the orders list blade filtered by query */ },
                            items: response.data.results.map(function (order) {
                                return {
                                    id: order.id,
                                    title: order.number,
                                    description: [order.customerName, order.storeId].filter(Boolean).join(' · '),
                                    badge: order.status,
                                    meta: order.total + ' ' + order.currency,
                                    icon: 'fas fa-file-invoice',
                                    execute: function () {
                                        globalSearchService.openBlade({
                                            state: 'workspace.orderModule',
                                            blade: angular.extend({ customerOrder: order, title: order.number },
                                                /* the detail blade definition the module's order list uses */ {})
                                        });
                                    }
                                };
                            })
                        };
                    });
            }
        });
    }]);
```

Put the provider in its own file (for example `Scripts/search/orderSearch.js`) so it is bundled with the module's other scripts. No platform change is needed to add a provider.

## Provider reference

| Property | Type | Required | Description |
|---|---|---|---|
| `id` | string | yes | Unique id. Registering the same id again replaces the provider. Prefix with the module name: `orders`, `catalog.products`. |
| `title` | string | yes | Group heading: a translation key or text. Its translation is also an alias. |
| `search(query, context)` | function | yes | Returns items, `{ items, total, showAll }`, or a promise of either. |
| `order` | number | | Group position, lower first. The platform uses 10–50; modules should use 100 and up. |
| `permission` | string \| string[] | | The provider is skipped unless the user has it (an array means *any of*). |
| `isAvailable()` | function | | Extra check, e.g. a second permission or a module setting. Return `false` when there is nothing to find. |
| `minLength` | number | | Shortest query the provider answers. Default `1`; use `2`–`3` for API providers. |
| `delay` | number (ms) | | Extra wait after the 150 ms debounce before `search` is called. Use `200`–`300` for API providers so a request is not sent for every keystroke. Default `0`. |
| `aliases` | string[] | | Typing `alias: text` searches only this provider (case-insensitive). The translated `title` works as an alias too. |
| `match` | RegExp \| function(query) | | When it matches the query, the group is shown first. Use it for recognisable formats: order numbers, SKUs, emails. |

### `context`

| Property | Description |
|---|---|
| `take` | Number of items to return: 8, or 20 when the user scoped the search with an alias. |
| `cancel` | A promise resolved when the search is superseded. Pass it as the `$http` `timeout`. |
| `scoped` | `true` when the user typed this provider's alias, so the group is the only one shown. |

### Result

Return either an array of items, or:

```javascript
{
    items: [...],               // the items to show (at most context.take are used)
    total: 245,                 // optional: how many the server found
    showAll: function () {}     // optional: adds a "Show all 245" row when total > items.length
}
```

### Item

| Property | Description |
|---|---|
| `id` | Stable id. |
| `title` | First line; the query words are highlighted. |
| `description` | Second line (customer · store, email · type, …). |
| `icon` | Icon classes (`fas fa-user`, `fa fa-sliders`). Font Awesome 4.7 and 5 are available. |
| `iconUrl` | An image instead of the icon, e.g. a product thumbnail. |
| `badge` | A short status pill on the right (`New`, `Paid`). |
| `meta` | A short right-aligned value (`120.50 USD`, a date). |
| `permission` | The item is dropped unless the user has it. |
| `external` | Shows an external-link mark (for results that open a new tab). |
| `execute()` | Required. Runs on click or Enter; the search box closes first. |

## Service reference — `platformWebApp.globalSearchService`

| Method | Description |
|---|---|
| `registerProvider(provider)` | Adds or replaces a provider. Can be called at any time; the next search uses it. |
| `unregisterProvider(id)` | Removes a provider. |
| `getProviders()` | The registered providers, in order. |
| `isAvailable()` | `true` when at least one provider is usable for the current user. The search box is hidden otherwise. |
| `search(query, { onGroup, take })` | Runs a search. `onGroup(groups)` is called whenever a group changes; each group is `{ provider, title, status: 'pending' \| 'done' \| 'error', items, total, showAll }`. The promise resolves with the groups that found something; `promise.cancel()` stops it. Used by the search box; useful in tests. |
| `rankItems(items, query, fields, take)` | Filters and orders client-side items: every query word must appear; whole-text prefix beats word prefix beats substring; earlier fields weigh more. `fields(item)` returns the searchable texts. Also useful to re-order API results (see `securitySearch.js`). |
| `hasPermission(permission)` | `true` when the user has the permission (string, or array meaning *any of*). |
| `translate(keyOrText)` | `$translate.instant` that falls back to the text. |
| `parseQuery(query)` | `{ text, scope }`: the query without an alias prefix, and the providers the alias selects (`null` for all). |
| `openBlade({ state, stateParams?, parentBladeId?, parentReady?, blade })` | Opens a result: switches to the workspace `state` (skipped if already there), waits for the blade with id `parentBladeId` (and, if given, for it to have the method `parentReady`), then shows `blade` under it. Returns a promise. |
| `goToWorkspace(state, params?)` | Switches to a workspace state; resolves when done. |
| `waitForBlade(idOrPredicate, method?, timeoutMs?)` | Resolves with the open blade that has this id, or satisfies `predicate(blade)`, once it is ready (has `method`); rejects after 5 s. Use it for multi-step navigation; use a predicate when blades share an id (see below). |

### Opening the right blade

Open results the way a user would get there, so the blades behave as usual (a detail blade usually refreshes its parent list after a save):

```javascript
// one step: workspace > detail blade under the workspace's list blade
globalSearchService.openBlade({
    state: 'workspace.customerModule',
    parentBladeId: 'memberList',
    blade: { id: 'memberDetail', currentEntity: member, /* controller, template ... */ }
});

// several steps: Security > Users list > user (see securitySearch.js)
function isUsersList(b) { return b.id === 'securityDetails' && b.controller === 'platformWebApp.accountListController'; }

globalSearchService.goToWorkspace('workspace.securityModule')
    .then(function () { return globalSearchService.waitForBlade('security', 'openBlade'); })
    .then(function (main) {
        var open = _.find(bladeNavigationService.stateBlades(), isUsersList);
        if (open && open.selectNode) {
            return open;                                   // the right list is already open: reuse it
        }
        main.openBlade(_.findWhere(main.currentEntities, { entityName: 'account' }));
        return globalSearchService.waitForBlade(function (b) { return isUsersList(b) && b !== open; }, 'selectNode');
    })
    .then(function (list) { list.selectNode(user); });
```

**Blades that share an id.** Several lists can use the same blade id (Users and Roles are both `securityDetails`). Replacing one with the other is not instant: if a child blade has unsaved changes, the user is asked first. Waiting for the id alone can resolve the list that is still on screen and open the result there. Recognise the blade by its controller (or another property), reuse it when the right one is already open, and otherwise wait for a new instance.

Use the same blade definition (id, controller, template, data) that the module's own list blade passes to `bladeNavigationService.showBlade`, so there is one way to open an entity.

## Suggested providers per module

| Module | Endpoint | Title / description | Permission | Notes |
|---|---|---|---|---|
| Orders | `POST api/order/customerOrders/search` (`keyword`) | number / customer · store; `badge` status, `meta` total | `order:read` | `match` for the order number format; `showAll` opens the orders list |
| Customers | `POST api/members/search` (`keyword`) | name / email · member type | `customer:read` | aliases `customer`, `contact`, `organization` |
| Catalog | the module's product search (`api/catalog/listentries` with `keyword`) | name / SKU · catalog; thumbnail as `iconUrl` | `catalog:read` | `match` for SKU patterns; categories as a second provider |
| Stores | `POST api/stores/search` | name / URL | `store:read` | few stores: cache and filter in the browser (sync) |

Check each module's own read permission and blade ids; the table shows the usual names.

## Permissions

- Set `permission` on the provider; the provider is skipped (no API call) without it. Use `isAvailable()` for a second requirement, e.g. the workspace's `:access` permission.
- Set `permission` on items that need more than the provider (the platform drops them).
- The server must still enforce permissions on the endpoint; the client checks only decide what to show.
- When no provider is usable for a user, the search box is hidden.

## Localization

- Add the group title key to the module's localization files, e.g. `orders.search.group-title`.
- The translated title is also an alias, so `Bestellungen: CO123` works in German without extra configuration.
- The platform texts (*Searching…*, *Search failed*, *Show all {{count}}*, hints) are in `platform.search.*`.

## Guidelines

- Keep `search` fast: return at most `context.take` items and request only the fields you show (response groups).
- Always pass `context.cancel` to `$http` so superseded requests are aborted.
- Use `minLength` and `delay` for API providers; a client-side provider needs neither.
- Rank server results when the API does not (see `securitySearch.js`): `rankItems` first, then the remaining results.
- Don't throw for "nothing found": return an empty array. Errors and timeouts show *Search failed* for your group only.
- Keep titles short; put secondary data in `description`, `badge` and `meta`.
- Register providers in a `run` block of the module's AngularJS module; don't register from blade controllers.
- In `execute`, don't assume the screen is empty: the user may have blades open, possibly with unsaved changes. Use `openBlade`/`waitForBlade` instead of fixed timeouts.

## Testing a provider

In the browser console of the back office:

```javascript
var search = angular.element(document.body).injector().get('platformWebApp.globalSearchService');
search.getProviders().map(function (p) { return p.id; });            // is it registered?
search.search('CO123', { onGroup: function (g) { console.log(g); } }) // groups as they arrive
    .then(function (groups) { console.log('done', groups); });
```

Check: results with and without the permission; `alias: text` scoping; a slow or failing endpoint (the group shows *Searching…*, then *Search failed* after 8 s, without blocking other groups); typing quickly (only the last request completes); light, dark and classic looks; a phone-sized window.
