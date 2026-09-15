angular.module('platformWebApp')
    .controller('platformWebApp.signInLogListController',
        ['$scope', 'platformWebApp.accounts', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
            function ($scope, accounts, bladeUtils, uiGridHelper) {
                $scope.uiGridConstants = uiGridHelper.uiGridConstants;
                var blade = $scope.blade;

                blade.title = blade.title || 'platform.blades.sign-in-log.title';
                blade.headIcon = 'fas fa-clipboard-list';

                blade.periods = [
                    { label: 'platform.blades.sign-in-log.filter.period-24h', value: '24h', hours: 24 },
                    { label: 'platform.blades.sign-in-log.filter.period-7d', value: '7d', hours: 24 * 7 },
                    { label: 'platform.blades.sign-in-log.filter.period-30d', value: '30d', hours: 24 * 30 },
                    { label: 'platform.blades.sign-in-log.filter.period-all', value: '', hours: 0 }
                ];

                blade.outcomes = [
                    { label: 'platform.blades.sign-in-log.filter.all', value: '' },
                    { label: 'platform.blades.sign-in-log.labels.succeeded', value: 'true' },
                    { label: 'platform.blades.sign-in-log.labels.failed', value: 'false' }
                ];

                // A comma-joined value expands into several signInTypes, so "On behalf" can cover
                // both the grant and the revert without needing a multi-select.
                blade.signInTypeOptions = [
                    { label: 'platform.blades.sign-in-log.filter.all', value: '' },
                    { label: 'platform.blades.sign-in-log.filter.type-password', value: 'Password' },
                    { label: 'platform.blades.sign-in-log.filter.type-external', value: 'External' },
                    { label: 'platform.blades.sign-in-log.filter.type-impersonation', value: 'Impersonation,ImpersonationRevert' },
                    { label: 'platform.blades.sign-in-log.filter.type-logout', value: 'Logout' }
                ];

                // Seeded by the dashboard; falls back to the plain defaults when opened directly.
                var seed = blade.initialFilter || {};

                function defaults() {
                    return {
                        keyword: seed.keyword || null,
                        period: seed.period !== undefined ? seed.period : '24h',
                        succeeded: seed.succeeded !== undefined ? seed.succeeded : '',
                        signInType: seed.signInType || '',
                        failureReason: seed.failureReason || null,
                        ipAddress: seed.ipAddress || null
                    };
                }

                var filter = $scope.filter = angular.extend(defaults(), {
                    hasActiveFilters: function () {
                        return filter.period !== '24h' ||
                               filter.succeeded !== '' ||
                               filter.signInType !== '' ||
                               !!filter.failureReason ||
                               !!filter.ipAddress;
                    },

                    // Clear resets to the plain defaults, not to the dashboard preset: the point of
                    // the button is to widen the view, not to bounce back to where you came from.
                    clearFilters: function () {
                        filter.period = '24h';
                        filter.succeeded = '';
                        filter.signInType = '';
                        filter.failureReason = null;
                        filter.ipAddress = null;
                        blade.searchText = '';
                        filter.keyword = null;
                        filter.criteriaChanged();
                    },

                    criteriaChanged: function () {
                        if ($scope.pageSettings.currentPage > 1) {
                            $scope.pageSettings.currentPage = 1;
                        } else {
                            blade.refresh();
                        }
                    }
                });

                blade.searchText = filter.keyword || '';
                $scope.$watch('blade.searchText', function (newVal, oldVal) {
                    if (newVal !== oldVal) {
                        filter.keyword = newVal;
                        filter.criteriaChanged();
                    }
                });

                function buildCriteria() {
                    var criteria = {
                        userId: blade.userId,
                        keyword: filter.keyword,
                        ipAddress: filter.ipAddress,
                        // Server defaults to CreatedDate descending when this is empty.
                        sort: uiGridHelper.getSortExpression($scope)
                    };

                    if (filter.succeeded !== '') {
                        criteria.succeeded = filter.succeeded === 'true';
                    }

                    if (filter.signInType) {
                        criteria.signInTypes = filter.signInType.split(',');
                    }

                    if (filter.failureReason) {
                        criteria.failureReasons = [filter.failureReason];
                    }

                    var period = _.findWhere(blade.periods, { value: filter.period });
                    if (period && period.hours) {
                        var startDate = new Date();
                        startDate.setHours(startDate.getHours() - period.hours);
                        criteria.startDate = startDate.toISOString();
                    }

                    return criteria;
                }

                blade.refresh = function () {
                    blade.isLoading = true;

                    accounts.searchSignInLog({}, angular.extend(buildCriteria(), {
                        skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                        take: $scope.pageSettings.itemsPerPageCount
                    }), function (data) {
                        blade.isLoading = false;
                        $scope.pageSettings.totalItems = data.totalCount;
                        blade.currentEntities = data.results;

                        if (blade.refreshCountCallback && angular.isFunction(blade.refreshCountCallback)) {
                            blade.refreshCountCallback(data.totalCount);
                        }
                    }, function () {
                        blade.isLoading = false;
                    });
                };

                $scope.isImpersonation = function (entity) {
                    return !!entity &&
                        (entity.signInType === 'Impersonation' || entity.signInType === 'ImpersonationRevert');
                };

                blade.toolbarCommands = [
                    {
                        name: 'platform.commands.refresh', icon: 'fa fa-refresh',
                        executeMethod: function () { blade.refresh(); },
                        canExecuteMethod: function () { return true; }
                    }
                ];

                $scope.setGridOptions = function (gridOptions) {
                    uiGridHelper.initialize($scope, gridOptions, function () {
                        uiGridHelper.bindRefreshOnSortChanged($scope);
                    });
                    bladeUtils.initializePagination($scope);
                };
            }]);
