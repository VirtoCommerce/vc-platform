angular.module('platformWebApp')
    .controller('platformWebApp.dynamicPropertyListController', ['$timeout', '$scope', 'platformWebApp.bladeUtils', 'platformWebApp.dynamicProperties.api', 'platformWebApp.uiGridHelper', function ($timeout, $scope, bladeUtils, dynamicPropertiesApi, uiGridHelper) {
    var blade = $scope.blade;
    blade.headIcon = 'far fa-plus-square';
    blade.title = blade.objectType;
    blade.subtitle = 'platform.blades.dynamicProperty-list.subtitle';
    $scope.uiGridConstants = uiGridHelper.uiGridConstants;
    $scope.hasMore = true;
    $scope.items = [];

    var bladeNavigationService = bladeUtils.bladeNavigationService;
    blade.isLoading = true;

    blade.refresh = function (parentRefresh) {
        $scope.items = [];

        if ($scope.pageSettings.currentPage !== 1) {
            $scope.pageSettings.currentPage = 1;
        }

        loadData(function () {
            if (parentRefresh && blade.parentRefresh) {
                blade.parentRefresh();
            }
        });

        resetStateGrid();
    };

    function loadData(callback) {
        blade.isLoading = true;

        var pagedDataQuery = {
            objectType: blade.objectType, 
            skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            take: $scope.pageSettings.itemsPerPageCount
        }
        dynamicPropertiesApi.search(
            pagedDataQuery,
            function (data) {
                blade.isLoading = false;
                $scope.pageSettings.totalItems = data.totalCount;
                $scope.items = $scope.items.concat(data.results);
                $scope.hasMore = data.results.length === $scope.pageSettings.itemsPerPageCount;

                if (callback) {
                    callback();
                }
            });
    }

    function resetStateGrid() {
        if ($scope.gridApi) {
            $scope.items = [];
            $scope.gridApi.infiniteScroll.resetScroll(true, true);
            $scope.gridApi.infiniteScroll.dataLoaded();
        }
    }

    blade.setSelectedItem = function (listItem) {
        $scope.selectedNodeId = listItem.id;
    };

    $scope.selectItem = function (e, listItem) {
        blade.setSelectedItem(listItem);

        var newBlade = {
            subtitle: 'platform.blades.dynamicProperty-detail.subtitle',
            currentEntity: listItem
        };
        openDetailsBlade(newBlade);
    };

    function openDetailsBlade(node) {
        var newBlade = {
            id: "dynamicPropertyDetail",
            objectType: blade.objectType,
            controller: 'platformWebApp.dynamicPropertyDetailController',
            template: '$(Platform)/Scripts/app/dynamicProperties/blades/dynamicProperty-detail.tpl.html'
        };
        angular.extend(newBlade, node);

        bladeNavigationService.showBlade(newBlade, blade);
    }

    function showMore() {
        if ($scope.hasMore) {
            ++$scope.pageSettings.currentPage;
            $scope.gridApi.infiniteScroll.saveScrollPercentage();
            loadData(function () {
                $scope.gridApi.infiniteScroll.dataLoaded();
            });
        }
    }

    blade.toolbarCommands = [
       {
           name: "Refresh", icon: 'fa fa-refresh',
           executeMethod: blade.refresh,
           canExecuteMethod: function () {
               return true;
           }
       },
       {
           name: "platform.commands.add-new-property", icon: 'fas fa-plus',
           executeMethod: function () {
               $scope.selectedNodeId = undefined;
               var newBlade = {
                   subtitle: 'platform.blades.dynamicProperty-detail.subtitle-new',
                   isNew: true,
                   onChangesConfirmedFn: function (entry) {
                       $scope.selectedNodeId = entry.id;
                   }
               };
               openDetailsBlade(newBlade);
           },
           canExecuteMethod: function () {
               return true;
           },
           permission: 'platform:dynamic_properties:create'
       }
    ];

    $scope.setGridOptions = function (gridOptions) {
        bladeUtils.initializePagination($scope, true);
        $scope.pageSettings.itemsPerPageCount = 20;

        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
            //update gridApi for current grid
            $scope.gridApi = gridApi;

            $scope.gridApi.infiniteScroll.on.needLoadMoreData($scope, showMore);
        });

        // need to call refresh after digest cycle as we do not "$watch" for $scope.pageSettings.currentPage
        $timeout(function () {
            blade.refresh();
        });
    };

}]);
