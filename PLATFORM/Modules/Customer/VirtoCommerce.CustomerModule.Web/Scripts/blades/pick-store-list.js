angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.pickStoreListController', ['$scope', 'virtoCommerce.storeModule.stores', 'platformWebApp.bladeUtils', 'uiGridConstants', 'platformWebApp.uiGridHelper',
    function ($scope, stores, bladeUtils, uiGridConstants, uiGridHelper) {
        $scope.uiGridConstants = uiGridConstants;

        var blade = $scope.blade;
        var bladeNavigationService = bladeUtils.bladeNavigationService;

        blade.refresh = function () {
            blade.isLoading = true;
            stores.search({
                keyword: filter.keyword ? filter.keyword : undefined,
                sort: uiGridHelper.getSortExpression($scope),
                skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                take: $scope.pageSettings.itemsPerPageCount
            }, function (data) {
                blade.isLoading = false;
                $scope.pageSettings.totalItems = data.totalCount;
                blade.currentEntities = data.stores;
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

        blade.selectNode = function (node) {
            $scope.selectedNodeId = node.id;
            $scope.bladeClose(function () {
                blade.onAfterNodeSelected(node);
            });
        };

        blade.headIcon = 'fa-archive';

        blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.refresh,
                canExecuteMethod: function () {
                    return true;
                }
            }
        ];

        // simple and advanced filtering
        var filter = $scope.filter = {};

        filter.criteriaChanged = function () {
            if ($scope.pageSettings.currentPage > 1) {
                $scope.pageSettings.currentPage = 1;
            } else {
                blade.refresh();
            }
        };

        // ui-grid
        $scope.setGridOptions = function (gridOptions) {
            uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                uiGridHelper.bindRefreshOnSortChanged($scope);
            });
            bladeUtils.initializePagination($scope);
        };

    }]);