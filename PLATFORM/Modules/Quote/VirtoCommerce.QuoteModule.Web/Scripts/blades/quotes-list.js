angular.module('virtoCommerce.quoteModule')
.controller('virtoCommerce.quoteModule.quotesListController', ['$scope', 'virtoCommerce.quoteModule.quotes', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'uiGridConstants', 'platformWebApp.uiGridHelper',
    function ($scope, quotes, bladeNavigationService, dialogService, uiGridConstants, uiGridHelper) {
        $scope.uiGridConstants = uiGridConstants;
        var blade = $scope.blade;

        blade.refresh = function () {
            blade.isLoading = true;
            quotes.search({
                keyword: filter.keyword,
                start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                count: $scope.pageSettings.itemsPerPageCount
            }, function (data) {
                blade.isLoading = false;

                $scope.pageSettings.totalItems = data.totalCount;
                blade.currentEntities = data.quoteRequests;
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
                subtitle: 'quotes.blades.quote-detail.subtitle',
                controller: 'virtoCommerce.quoteModule.quoteDetailController',
                template: 'Modules/$(VirtoCommerce.Quote)/Scripts/blades/quote-detail.tpl.html'
            };

            bladeNavigationService.showBlade(newBlade, blade);
        };

        $scope.deleteList = function (list) {
            var dialog = {
                id: "confirmDeleteItem",
                title: "quotes.dialogs.quote-requests-delete.title",
                message: "quotes.dialogs.quote-requests-delete.message",
                callback: function (remove) {
                    if (remove) {
                        bladeNavigationService.closeChildrenBlades(blade, function () {
                            var itemIds = _.pluck(list, 'id');
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
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.refresh,
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
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: function () {
                    $scope.deleteList($scope.gridApi.selection.getSelectedRows());
                },
                canExecuteMethod: function () {
                    return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
                },
                permission: 'quote:update'
            }
        ];

        //pagination settings
        $scope.pageSettings = {};
        $scope.pageSettings.totalItems = 0;
        $scope.pageSettings.currentPage = 1;
        $scope.pageSettings.numPages = 5;
        $scope.pageSettings.itemsPerPageCount = 20;

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
            uiGridHelper.initialize($scope, gridOptions);
        };

        $scope.$watch('pageSettings.currentPage', blade.refresh);
        //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
        //blade.refresh();
    }]);
