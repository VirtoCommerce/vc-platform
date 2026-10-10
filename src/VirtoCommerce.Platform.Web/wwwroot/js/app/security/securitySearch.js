angular.module('platformWebApp')
    // Global search providers for security: users and roles found through the security API.
    // A result opens Security > Users (or Roles) > the user (role), as if the user had clicked through.
    .run(['$q', '$http', '$state', '$timeout', 'platformWebApp.globalSearchService', 'platformWebApp.bladeNavigationService',
        function ($q, $http, $state, $timeout, globalSearchService, bladeNavigationService) {

            // Resolves with the open blade that has the given id once it is ready (has the given method)
            function waitForBlade(id, method) {
                var deferred = $q.defer();
                var attempts = 0;
                (function poll() {
                    var blade = _.find(bladeNavigationService.stateBlades(), function (b) { return b.id === id; });
                    if (blade && (!method || angular.isFunction(blade[method]))) {
                        deferred.resolve(blade);
                    } else if (++attempts > 40) {
                        deferred.reject();
                    } else {
                        $timeout(poll, 100);
                    }
                })();
                return deferred.promise;
            }

            // Security workspace > list of the given entity ('account' or 'role') > selectNode(node)
            function openInSecurity(entityName, node) {
                var transition = $state.current.name === 'workspace.securityModule' ? $q.resolve() : $state.go('workspace.securityModule');
                return $q.when(transition)
                    .then(function () { return waitForBlade('security', 'openBlade'); })
                    .then(function (mainBlade) {
                        var entity = _.findWhere(mainBlade.currentEntities, { entityName: entityName });
                        mainBlade.openBlade(entity);
                        return waitForBlade('securityDetails', 'selectNode');
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
