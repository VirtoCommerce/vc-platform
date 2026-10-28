angular.module('platformWebApp')
    // Global search providers for security: users and roles found through the security API.
    // A result opens Security > Users (or Roles) > the user (role), as if the user had clicked through.
    .run(['$http', 'platformWebApp.globalSearchService', 'platformWebApp.bladeNavigationService', function ($http, globalSearchService, bladeNavigationService) {

            // Security workspace > list of the given entity ('account' or 'role') > selectNode(node).
            // The detail blades refresh their parent list after a save, so they are opened under it.
            // The users and roles lists share the blade id 'securityDetails': the list is recognised by its
            // controller, an open list of the right kind is reused, and otherwise only the newly opened list
            // (not the one being replaced) is used.
            function openInSecurity(entityName, node) {
                var controller = 'platformWebApp.' + entityName + 'ListController';
                function isList(b) {
                    return b.id === 'securityDetails' && b.controller === controller;
                }
                return globalSearchService.goToWorkspace('workspace.securityModule')
                    .then(function () { return globalSearchService.waitForBlade('security', 'openBlade'); })
                    .then(function (mainBlade) {
                        var current = _.find(bladeNavigationService.stateBlades(), isList);
                        if (current && angular.isFunction(current.selectNode)) {
                            return current;
                        }
                        mainBlade.openBlade(_.findWhere(mainBlade.currentEntities, { entityName: entityName }));
                        return globalSearchService.waitForBlade(function (b) { return isList(b) && b !== current; }, 'selectNode');
                    })
                    .then(function (listBlade) {
                        listBlade.selectNode(node);
                    });
            }

            // Server-side keyword search; the results are re-ordered so the closest matches come first
            // (results the server matched on other fields are kept after them)
            function search(url, query, context, fields) {
                return $http.post(url, { keyword: query, take: context.take, skip: 0 }, { timeout: context.cancel })
                    .then(function (response) {
                        var results = response.data.results || [];
                        var ranked = globalSearchService.rankItems(results, query, fields, results.length);
                        return ranked.concat(_.difference(results, ranked));
                    });
            }

            globalSearchService.registerProvider({
                id: 'platform.security.users',
                title: 'platform.blades.account-list.title',
                order: 40,
                minLength: 2,
                delay: 200,
                aliases: ['user', 'users'],
                permission: 'platform:security:read',
                // the security workspace itself needs its own permission
                isAvailable: function () {
                    return globalSearchService.hasPermission('platform:security:access');
                },
                search: function (query, context) {
                    return search('api/platform/security/users/search', query, context, function (user) {
                        return [user.userName, user.email];
                    }).then(function (users) {
                        return _.map(users, function (user) {
                            return {
                                id: user.id,
                                title: user.userName,
                                description: _.filter([user.email, user.userType], _.identity).join(' · '),
                                icon: 'fas fa-user',
                                execute: function () {
                                    openInSecurity('account', user);
                                }
                            };
                        });
                    });
                }
            });

            globalSearchService.registerProvider({
                id: 'platform.security.roles',
                title: 'platform.blades.role-list.title',
                order: 50,
                minLength: 2,
                delay: 200,
                aliases: ['role', 'roles'],
                permission: 'platform:security:read',
                isAvailable: function () {
                    return globalSearchService.hasPermission('platform:security:access');
                },
                search: function (query, context) {
                    return search('api/platform/security/roles/search', query, context, function (role) {
                        return [role.name, role.description];
                    }).then(function (roles) {
                        return _.map(roles, function (role) {
                            return {
                                id: role.id,
                                title: role.name,
                                description: role.description || '',
                                icon: 'fas fa-user-shield',
                                execute: function () {
                                    openInSecurity('role', role);
                                }
                            };
                        });
                    });
                }
            });
        }]);
