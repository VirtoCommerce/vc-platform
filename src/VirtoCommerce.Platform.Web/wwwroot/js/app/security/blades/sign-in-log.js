angular.module('platformWebApp')
    .controller('platformWebApp.signInLogController',
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

                blade.signInTypes = ['Password', 'External', 'Impersonation', 'ImpersonationRevert', 'Logout'];

                var filter = $scope.filter = {
                    keyword: null,
                    period: '24h',
                    succeeded: '',
                    signInType: '',
                    ipAddress: null,

                    hasActiveFilters: function () {
                        return filter.period !== '24h' ||
                               filter.succeeded !== '' ||
                               filter.signInType !== '' ||
                               !!filter.ipAddress;
                    },

                    clearFilters: function () {
                        filter.period = '24h';
                        filter.succeeded = '';
                        filter.signInType = '';
                        filter.ipAddress = null;
                        filter.criteriaChanged();
                    },

                    criteriaChanged: function () {
                        if ($scope.pageSettings.currentPage > 1) {
                            $scope.pageSettings.currentPage = 1;
                        } else {
                            blade.refresh();
                        }
                    }
                };

                blade.searchText = '';
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
                        criteria.signInTypes = [filter.signInType];
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

                    var criteria = buildCriteria();

                    accounts.searchSignInLog({}, angular.extend({}, criteria, {
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

                    accounts.getSignInLogStats({}, angular.extend({}, criteria, { sort: null }), function (stats) {
                        blade.stats = stats;
                    });
                };

                // Every statistics row is a click-through into the filtered list.
                $scope.filterByIp = function (entry) {
                    filter.ipAddress = entry.key;
                    filter.criteriaChanged();
                };

                $scope.filterByFailureReason = function () {
                    filter.succeeded = 'false';
                    filter.criteriaChanged();
                };

                $scope.filterByUserName = function (entry) {
                    blade.searchText = entry.key;
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
