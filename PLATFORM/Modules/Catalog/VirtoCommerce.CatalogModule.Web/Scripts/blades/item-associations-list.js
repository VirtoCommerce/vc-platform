﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemAssociationsListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.catalogModule.items', 'filterFilter', 'uiGridConstants', 'platformWebApp.uiGridHelper', function ($scope, bladeNavigationService, dialogService, items, filterFilter, uiGridConstants, uiGridHelper) {
    $scope.uiGridConstants = uiGridConstants;
    var blade = $scope.blade;

    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    blade.refresh = function () {
        blade.isLoading = true;
        blade.parentBlade.refresh().$promise.then(function (data) {
            initializeBlade(data.associations);
        });
    };

    function initializeBlade(data) {
        blade.currentEntities = angular.copy(data);
        blade.origItem = data;
        blade.isLoading = false;
        $scope.pageSettings.totalItems = data.length;

        blade.currentEntities.sort(function (a, b) {
            return a.priority > b.priority;
        });
    };

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
            permission: 'catalog:update'
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                bladeNavigationService.closeChildrenBlades(blade, function () {
                    var dialog = {
                        id: "confirmDeleteItem",
                        title: "catalog.dialogs.association-delete.title",
                        message: "catalog.dialogs.association-delete.message",
                        callback: function (remove) {
                            if (remove) {
                                blade.isLoading = true;

                                var undeletedEntries = _.difference(blade.currentEntities, $scope.gridApi.selection.getSelectedRows());
                                items.update({ id: blade.currentEntityId, associations: undeletedEntries }, function () {
                                    blade.refresh();
                                },
                                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                            }
                        }
                    }

                    dialogService.showConfirmationDialog(dialog);
                });
            },
            canExecuteMethod: function () {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
            },
            permission: 'catalog:update'
        }
    ];

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions,
        function (gridApi) {
            gridApi.grid.registerRowsProcessor($scope.singleFilter, 90);
            $scope.$watch('pageSettings.currentPage', gridApi.pagination.seek);
            gridApi.draggableRows.on.rowDropped($scope, function (info, dropTarget) {
                for (var i = 0; i < blade.currentEntities.length; i++) {
                    blade.currentEntities[i].priority = i + 1;
                }

                items.update({ id: blade.currentEntityId, associations: blade.currentEntities },
                    blade.refresh,
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            });
        });
    };

    $scope.singleFilter = function (renderableRows) {
        var visibleCount = 0;
        renderableRows.forEach(function (row) {
            row.visible = _.any(filterFilter([row.entity], blade.searchText));
            if (row.visible) visibleCount++;
        });

        $scope.filteredEntitiesCount = visibleCount;
        return renderableRows;
    };


    initializeBlade(blade.currentEntities);
}]);
