angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.promotionListController', ['$scope', 'virtoCommerce.marketingModule.promotions', 'platformWebApp.dialogService', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
    function ($scope, promotions, dialogService, bladeUtils, uiGridHelper) {
        var blade = $scope.blade;
        var bladeNavigationService = bladeUtils.bladeNavigationService;

        blade.refresh = function () {
            blade.isLoading = true;

            promotions.search({
                responseGroup: 'withPromotions',
                keyword: filter.keyword,
                sort: uiGridHelper.getSortExpression($scope),
                start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                count: $scope.pageSettings.itemsPerPageCount
            }, function (data) {
                blade.isLoading = false;

                $scope.pageSettings.totalItems = data.totalCount;
                blade.currentEntities = data.promotions;
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

        $scope.selectNode = function (node) {
            $scope.selectedNodeId = node.id;

            var newBlade = {
                id: 'listItemChild',
                currentEntityId: node.id,
                title: node.name,
                subtitle: blade.subtitle,
                controller: 'virtoCommerce.marketingModule.promotionDetailController',
                template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/blades/promotion-detail.tpl.html'
            };

            bladeNavigationService.showBlade(newBlade, blade);
        };

        $scope.deleteList = function (list) {
            var dialog = {
                id: "confirmDeleteItem",
                title: "marketing.dialogs.promotions-delete.title",
                message: "marketing.dialogs.promotions-delete.message",
                callback: function (remove) {
                    if (remove) {
                        closeChildrenBlades();

                        var itemIds = _.pluck(list, 'id');
                        promotions.remove({ ids: itemIds }, function (data, headers) {
                            blade.refresh();
                        }, function (error) {
                            bladeNavigationService.setError('Error ' + error.status, blade);
                        });
                    }
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }

        function closeChildrenBlades() {
            angular.forEach(blade.childrenBlades.slice(), function (child) {
                bladeNavigationService.closeBlade(child);
            });
        }

        blade.headIcon = 'fa-area-chart';

        blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.refresh,
                canExecuteMethod: function () {
                    return true;
                }
            },
            {
                name: "platform.commands.add", icon: 'fa fa-plus',
                executeMethod: function () {
                    closeChildrenBlades();

                    var newBlade = {
                        id: 'listItemChild',
                        title: 'marketing.blades.promotion-detail.title-new',
                        subtitle: blade.subtitle,
                        isNew: true,
                        controller: 'virtoCommerce.marketingModule.promotionDetailController',
                        template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/promotion/blades/promotion-detail.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                },
                canExecuteMethod: function () {
                    return true;
                },
                permission: 'marketing:create'
            },
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: function () {
                    $scope.deleteList($scope.gridApi.selection.getSelectedRows());
                },
                canExecuteMethod: function () {
                    return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
                },
                permission: 'marketing:delete'
            }
        ];

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

        // actions on load
        //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
        //blade.refresh();
    }]);