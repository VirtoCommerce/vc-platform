angular.module('platformWebApp')
    .controller('platformWebApp.signInLogListController',
        ['$scope', 'platformWebApp.accounts', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeNavigationService',
            function ($scope, accounts, bladeUtils, uiGridHelper, bladeNavigationService) {
                $scope.uiGridConstants = uiGridHelper.uiGridConstants;
                var blade = $scope.blade;

                blade.title = blade.title || 'platform.blades.sign-in-log.title';
                blade.headIcon = 'fas fa-clipboard-list';

                blade.periods = [
                    { label: 'platform.blades.sign-in-log.filter.period-30m', value: '30m', minutes: 30 },
                    { label: 'platform.blades.sign-in-log.filter.period-1h', value: '1h', minutes: 60 },
                    { label: 'platform.blades.sign-in-log.filter.period-6h', value: '6h', minutes: 360 },
                    { label: 'platform.blades.sign-in-log.filter.period-24h', value: '24h', minutes: 1440 },
                    { label: 'platform.blades.sign-in-log.filter.period-7d', value: '7d', minutes: 1440 * 7 },
                    { label: 'platform.blades.sign-in-log.filter.period-30d', value: '30d', minutes: 1440 * 30 },
                    { label: 'platform.blades.sign-in-log.filter.period-all', value: '', minutes: 0 },
                    { label: 'platform.blades.sign-in-log.filter.period-custom', value: 'custom', minutes: 0 }
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
                        ipAddress: seed.ipAddress || null,
                        storeId: seed.storeId || '',
                        dateFrom: null,
                        dateTo: null
                    };
                }

                var filter = $scope.filter = angular.extend(defaults(), {
                    hasActiveFilters: function () {
                        return filter.period !== '24h' ||
                               filter.succeeded !== '' ||
                               filter.signInType !== '' ||
                               !!filter.failureReason ||
                               !!filter.ipAddress ||
                               filter.storeId !== '' ||
                               !!filter.dateFrom ||
                               !!filter.dateTo;
                    },

                    // Clear resets to the plain defaults, not to the dashboard preset: the point of
                    // the button is to widen the view, not to bounce back to where you came from.
                    clearFilters: function () {
                        filter.period = '24h';
                        filter.succeeded = '';
                        filter.signInType = '';
                        filter.failureReason = null;
                        filter.ipAddress = null;
                        filter.storeId = '';
                        filter.dateFrom = null;
                        filter.dateTo = null;
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

                // The platform has no store catalogue, so the stores on offer are the ones that
                // actually appear in the log. One unfiltered call, once, on open.
                blade.stores = [];
                accounts.getSignInLogStats({}, {}, function (stats) {
                    blade.stores = _.pluck(stats.signInsByStore || [], 'key');
                    blade.recordingEnabled = stats.recordingEnabled;
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
                        storeId: filter.storeId || null,
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

                    if (filter.period === 'custom') {
                        // Date inputs give a local calendar day; widen "to" to the end of it so a
                        // single-day range does not silently exclude everything after midnight.
                        if (filter.dateFrom) {
                            criteria.startDate = new Date(filter.dateFrom).toISOString();
                        }

                        if (filter.dateTo) {
                            var to = new Date(filter.dateTo);
                            to.setHours(23, 59, 59, 999);
                            criteria.endDate = to.toISOString();
                        }
                    } else {
                        var period = _.findWhere(blade.periods, { value: filter.period });
                        if (period && period.minutes) {
                            criteria.startDate = new Date(Date.now() - period.minutes * 60000).toISOString();
                        }
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

                $scope.selectedRecordId = null;

                // Rows open a read-only detail blade: the grid shows five columns, the record has
                // eighteen fields, and an audit trail is worth reading in full.
                $scope.selectRecord = function (record) {
                    $scope.selectedRecordId = record.id;

                    bladeNavigationService.showBlade({
                        id: 'signInLogDetail',
                        record: record,
                        controller: 'platformWebApp.signInLogDetailController',
                        template: '$(Platform)/Scripts/app/security/blades/sign-in-log-detail.html'
                    }, blade);
                };

                $scope.setGridOptions = function (gridOptions) {
                    uiGridHelper.initialize($scope, gridOptions, function () {
                        uiGridHelper.bindRefreshOnSortChanged($scope);
                    });
                    bladeUtils.initializePagination($scope);
                };
            }]);
