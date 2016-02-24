angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemAssociationsListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.catalogModule.items', 'filterFilter', 'uiGridConstants', 'platformWebApp.uiGridHelper', function ($scope, bladeNavigationService, dialogService, items, filterFilter, uiGridConstants, uiGridHelper) {
    $scope.uiGridConstants = uiGridConstants;
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';

    blade.refresh = function () {
        blade.isLoading = true;
        blade.parentBlade.refresh().$promise.then(function (data) {
            initializeBlade(data.associations);
        });
    };

    function initializeBlade(data) {
        blade.currentEntities = angular.copy(data);
        blade.isLoading = false;

        blade.currentEntities.sort(function (a, b) {
            return a.priority - b.priority;
        });
    };

    $scope.selectNode = function (listItem) {
        $scope.selectedNodeId = listItem.productId;

        var newBlade = {
            id: 'associationDetail',
            itemId: listItem.productId,
            // productType: listItem.productType,
            title: listItem.productCode,
            // subtitle: 'catalog.blades.item-detail.subtitle-variation',
            controller: 'virtoCommerce.catalogModule.itemDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.deleteList = function (list) {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            var dialog = {
                id: "confirmDeleteItem",
                title: "catalog.dialogs.association-delete.title",
                message: "catalog.dialogs.association-delete.message",
                callback: function (remove) {
                    if (remove) {
                        blade.isLoading = true;

                        var undeletedEntries = _.difference(blade.currentEntities, list);
                        items.update({ id: blade.currentEntityId, associations: undeletedEntries }, function () {
                            blade.refresh();
                        },
                        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                    }
                }
            }

            dialogService.showConfirmationDialog(dialog);
        });
    }

    function openAddEntityWizard() {
        var newBlade = {
            id: "associationWizard",
            associations: blade.currentEntities,
            controller: 'virtoCommerce.catalogModule.associationWizardController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newAssociation/association-wizard.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.toolbarCommands = [
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityWizard();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                $scope.deleteList($scope.gridApi.selection.getSelectedRows());
            },
            canExecuteMethod: function () {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
            },
            permission: blade.updatePermission
        }
    ];

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions,
        function (gridApi) {
            // gridApi.grid.registerRowsProcessor($scope.singleFilter, 90);
            gridApi.draggableRows.on.rowFinishDrag($scope, function () {
                for (var i = 0; i < blade.currentEntities.length; i++) {
                    blade.currentEntities[i].priority = i + 1;
                }

                items.update({ id: blade.currentEntityId, associations: blade.currentEntities },
                    blade.refresh,
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            });
        });
    };

    //$scope.singleFilter = function (renderableRows) {
    //    var visibleCount = 0;
    //    renderableRows.forEach(function (row) {
    //        row.visible = _.any(filterFilter([row.entity], blade.searchText));
    //        if (row.visible) visibleCount++;
    //    });

    //    $scope.filteredEntitiesCount = visibleCount;
    //    return renderableRows;
    //};


    initializeBlade(blade.currentEntities);
}]);
