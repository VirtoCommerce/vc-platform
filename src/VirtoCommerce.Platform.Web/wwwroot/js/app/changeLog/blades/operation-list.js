angular.module('platformWebApp')
.controller('platformWebApp.changeLog.operationListController', [
    '$scope',
    'platformWebApp.bladeNavigationService',
    'platformWebApp.changeLogApi',
    'platformWebApp.uiGridHelper',
    'platformWebApp.bladeUtils',
    function ($scope, bladeNavigationService, changeLogApi, uiGridHelper, bladeUtils) {
        $scope.uiGridConstants = uiGridHelper.uiGridConstants;
        var blade = $scope.blade;
        blade.headIcon = blade.headIcon || 'fa fa-clock-o';

        // --- Filter state ---

        var filter = $scope.filter = {
            keyword: '',
            operationType: '',
            datePreset: '',
            startDate: null,
            endDate: null,

            hasActiveFilters: function () {
                return filter.operationType || filter.datePreset;
            },

            clearFilters: function () {
                filter.operationType = '';
                filter.datePreset = '';
                filter.startDate = null;
                filter.endDate = null;
                filter.criteriaChanged();
            },

            criteriaChanged: function () {
                computeDateRange();
                if ($scope.pageSettings.currentPage > 1) {
                    $scope.pageSettings.currentPage = 1;
                } else {
                    blade.refresh();
                }
            }
        };

        function computeDateRange() {
            if (filter.datePreset === 'custom') {
                return;
            }
            var now = new Date();
            var startOfToday = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            filter.startDate = null;
            filter.endDate = null;

            switch (filter.datePreset) {
                case 'today':
                    filter.startDate = startOfToday;
                    break;
                case 'yesterday':
                    filter.startDate = new Date(startOfToday.getTime() - 86400000);
                    filter.endDate = startOfToday;
                    break;
                case 'last7':
                    filter.startDate = new Date(startOfToday.getTime() - 7 * 86400000);
                    break;
                case 'last30':
                    filter.startDate = new Date(startOfToday.getTime() - 30 * 86400000);
                    break;
                default:
                    break;
            }
        }

        blade.searchText = '';
        $scope.$watch('blade.searchText', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                filter.keyword = newVal;
                filter.criteriaChanged();
            }
        });

        // --- Filter options ---

        blade.operationTypes = [
            { label: 'platform.blades.operation-list.filter.all', value: '' },
            { label: 'platform.blades.operation-list.filter.type-added', value: 'Added' },
            { label: 'platform.blades.operation-list.filter.type-modified', value: 'Modified' },
            { label: 'platform.blades.operation-list.filter.type-deleted', value: 'Deleted' }
        ];

        blade.datePresets = [
            { label: 'platform.blades.operation-list.filter.date-any', value: '' },
            { label: 'platform.blades.operation-list.filter.date-today', value: 'today' },
            { label: 'platform.blades.operation-list.filter.date-yesterday', value: 'yesterday' },
            { label: 'platform.blades.operation-list.filter.date-last7', value: 'last7' },
            { label: 'platform.blades.operation-list.filter.date-last30', value: 'last30' },
            { label: 'platform.blades.operation-list.filter.date-custom', value: 'custom' }
        ];

        // --- Blade operations ---

        blade.refresh = function () {
            blade.isLoading = true;

            var objectType = blade.tenantType ? blade.tenantType.split('.').pop() : undefined;

            var searchCriteria = {
                keyword: filter.keyword || undefined,
                objectIds: blade.tenantId ? [blade.tenantId] : undefined,
                objectType: objectType,
                sort: uiGridHelper.getSortExpression($scope),
                skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                take: $scope.pageSettings.itemsPerPageCount
            };

            if (filter.operationType) {
                searchCriteria.operationTypes = [filter.operationType];
            }
            if (filter.startDate) {
                searchCriteria.startDate = filter.startDate;
            }
            if (filter.endDate) {
                searchCriteria.endDate = filter.endDate;
            }

            changeLogApi.search(searchCriteria, function (data) {
                blade.isLoading = false;
                $scope.pageSettings.totalItems = data.totalCount;
                blade.currentEntities = data.results;
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

        blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.refresh,
                canExecuteMethod: function () { return true; }
            }
        ];

        // --- ui-grid ---

        $scope.setGridOptions = function (gridOptions) {
            uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                uiGridHelper.bindRefreshOnSortChanged($scope);
            });
            bladeUtils.initializePagination($scope);
        };
    }
]);
