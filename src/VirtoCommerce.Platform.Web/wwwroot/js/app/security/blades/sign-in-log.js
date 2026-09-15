angular.module('platformWebApp')
    .controller('platformWebApp.signInLogController',
        ['$scope', 'platformWebApp.accounts', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
            function ($scope, accounts, bladeUtils, uiGridHelper) {
                $scope.uiGridConstants = uiGridHelper.uiGridConstants;
                var blade = $scope.blade;
                blade.title = "platform.blades.sign-in-log.title";
                blade.headIcon = 'fas fa-clipboard-list';

                // Period selector for both the grid and the statistics strip.
                $scope.periods = [
                    { key: '24h', hours: 24 },
                    { key: '7d', hours: 24 * 7 },
                    { key: '30d', hours: 24 * 30 }
                ];

                var filter = $scope.filter = { period: $scope.periods[0] };

                function buildCriteria() {
                    var startDate = new Date();
                    startDate.setHours(startDate.getHours() - filter.period.hours);

                    return {
                        userId: blade.userId,
                        keyword: filter.keyword,
                        succeeded: filter.succeeded,
                        signInTypes: filter.signInTypes,
                        failureReasons: filter.failureReasons,
                        ipAddress: filter.ipAddress,
                        organizationId: filter.organizationId,
                        startDate: startDate.toISOString()
                    };
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
                    });

                    accounts.getSignInLogStats({}, criteria, function (stats) {
                        blade.stats = stats;
                    });
                };

                // Every statistics row is a click-through into the filtered list.
                $scope.filterByIp = function (entry) {
                    filter.ipAddress = entry.key;
                    filter.criteriaChanged();
                };

                $scope.filterByFailureReason = function (entry) {
                    filter.failureReasons = [entry.key];
                    filter.succeeded = false;
                    filter.criteriaChanged();
                };

                $scope.filterByUserName = function (entry) {
                    filter.keyword = entry.key;
                    filter.criteriaChanged();
                };

                $scope.isImpersonation = function (entity) {
                    return entity && (entity.signInType === 'Impersonation' || entity.signInType === 'ImpersonationRevert');
                };

                $scope.setPeriod = function (period) {
                    filter.period = period;
                    filter.criteriaChanged();
                };

                $scope.resetFilters = function () {
                    filter.keyword = null;
                    filter.succeeded = undefined;
                    filter.signInTypes = null;
                    filter.failureReasons = null;
                    filter.ipAddress = null;
                    filter.organizationId = null;
                    filter.criteriaChanged();
                };

                blade.toolbarCommands = [
                    {
                        name: "platform.commands.refresh", icon: 'fa fa-refresh',
                        executeMethod: blade.refresh,
                        canExecuteMethod: function () { return true; }
                    },
                    {
                        name: "platform.commands.reset-filters", icon: 'fa fa-eraser',
                        executeMethod: $scope.resetFilters,
                        canExecuteMethod: function () { return true; }
                    }
                ];

                filter.criteriaChanged = function () {
                    if ($scope.pageSettings.currentPage > 1) {
                        $scope.pageSettings.currentPage = 1;
                    } else {
                        blade.refresh();
                    }
                };

                $scope.setGridOptions = function (gridOptions) {
                    uiGridHelper.initialize($scope, gridOptions, function () {
                        uiGridHelper.bindRefreshOnSortChanged($scope);
                    });
                    bladeUtils.initializePagination($scope);
                };
            }]);
