angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.promotionListController', ['$scope', 'virtoCommerce.marketingModule.promotions', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'uiGridConstants', 'platformWebApp.uiGridHelper',
    function ($scope, promotions, bladeNavigationService, dialogService, uiGridConstants, uiGridHelper) {
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

            promotions.search({
                respGroup: 'withPromotions',
                keyword: $scope.filter.searchKeyword,
                start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                count: $scope.pageSettings.itemsPerPageCount
            }, function (data) {
                blade.isLoading = false;

                $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
                blade.currentEntities = data.promotions;
                uiGridHelper.onDataLoaded($scope.gridOptions, blade.currentEntities);
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

        function deleteChecked() {
            var dialog = {
                id: "confirmDeleteItem",
                title: "marketing.dialogs.promotions-delete.title",
                message: "marketing.dialogs.promotions-delete.message",
                callback: function (remove) {
                    if (remove) {
                        closeChildrenBlades();

                        var selection = $scope.gridApi.selection.getSelectedRows();
                        var itemIds = _.pluck(selection, 'id');
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
                executeMethod: function () {
                    blade.refresh();
                },
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
                    deleteChecked();
                },
                canExecuteMethod: function () {
                    return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
                },
                permission: 'marketing:delete'
            }
        ];

        // ui-grid
        $scope.setGridOptions = function (gridOptions) {
            uiGridHelper.initialize($scope, gridOptions);
        };


        $scope.$watch('pageSettings.currentPage', function () {
            blade.refresh();
        });

        // actions on load
        //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
        //blade.refresh();
    }]);