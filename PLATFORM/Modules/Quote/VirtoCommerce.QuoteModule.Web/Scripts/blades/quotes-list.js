angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quotesListController', ['$scope', 'virtoCommerce.quoteModule.quotes', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'uiGridConstants', 'platformWebApp.uiGridHelper',
    function ($scope, quotes, bladeNavigationService, dialogService, uiGridConstants, uiGridHelper) {
        var blade = $scope.blade;

        //pagination settings
        $scope.pageSettings = {};
        $scope.pageSettings.totalItems = 0;
        $scope.pageSettings.currentPage = 1;
        $scope.pageSettings.numPages = 5;
        $scope.pageSettings.itemsPerPageCount = 20;

        $scope.filter = { searchKeyword: undefined };

        blade.refresh = function () {
            blade.isLoading = true;
            quotes.search({
                q: $scope.filter.searchKeyword,
                start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                count: $scope.pageSettings.itemsPerPageCount
            }, function (data) {
                blade.isLoading = false;

                $scope.pageSettings.totalItems = data.totalCount;
                blade.currentEntities = data.quoteRequests;
                uiGridHelper.onDataLoaded($scope.gridOptions, blade.currentEntities);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }

        $scope.selectNode = function (node) {
            $scope.selectedNodeId = node.id;

            var newBlade = {
                id: 'quoteDetails',
                currentEntityId: node.id,
                title: node.number,
                subtitle: 'Quote details',
                controller: 'virtoCommerce.quoteModule.quoteDetailController',
                template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-detail.tpl.html'
            };

            bladeNavigationService.showBlade(newBlade, blade);
        };

        function isItemsChecked() {
            return blade.currentEntities && _.any(blade.currentEntities, function (x) { return x.$selected; });
        }

        function deleteChecked() {
            var dialog = {
                id: "confirmDeleteItem",
                title: "Delete confirmation",
                message: "Are you sure you want to delete selected Quote Requests?",
                callback: function (remove) {
                    if (remove) {
                        bladeNavigationService.closeChildrenBlades(blade, function () {
                            var selection = $scope.gridApi.selection.getSelectedRows();
                            var itemIds = _.pluck(selection, 'id');
                            quotes.remove({ ids: itemIds },
                                blade.refresh,
                                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); }
                                );
                        });
                    }
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }

        blade.headIcon = 'fa-file-text-o';

        blade.toolbarCommands = [
            {
                name: "Refresh", icon: 'fa fa-refresh',
                executeMethod: function () {
                    blade.refresh();
                },
                canExecuteMethod: function () {
                    return true;
                }
            },
            //{
            //    name: "Add", icon: 'fa fa-plus',
            //    executeMethod: function () {
            //        openBladeNew();
            //    },
            //    canExecuteMethod: function () {
            //        return true;
            //    },
            //    permission: 'quote:create'
            //}
            {
                name: "Delete", icon: 'fa fa-trash-o',
                executeMethod: function () {
                    deleteChecked();
                },
                canExecuteMethod: function () {
                    return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
                },
                permission: 'quote:update'
            }
        ];

        // ui-grid
        uiGridHelper.initialize($scope, {
            rowTemplate: "<div ng-click=\"grid.appScope.selectNode(row.entity)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.uid\" ui-grid-one-bind-id-grid=\"rowRenderIndex + '-' + col.uid + '-cell'\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader, '__selected': row.entity.id === grid.appScope.selectedNodeId }\" role=\"{{col.isRowHeader ? 'rowheader' : 'gridcell'}}\" ui-grid-cell style='cursor:pointer'></div>",
            rowHeight: 45,
            columnDefs: [
                        { name: 'number', displayName: 'Quote #', cellTooltip: true },
                        { name: 'status' },
                        { name: 'items.length', displayName: 'Items count' },
                        { name: 'customerName', displayName: 'Customer', cellTooltip: true }, // cellTemplate
                        {
                            name: 'createdDate', displayName: 'Created',
                            sort: {
                                direction: uiGridConstants.DESC
                            },
                            cellTemplate: 'am-time-ago.cell.html'
                        }
            ]
        });


        $scope.$watch('pageSettings.currentPage', blade.refresh);
        //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
        //blade.refresh();
    }]);
