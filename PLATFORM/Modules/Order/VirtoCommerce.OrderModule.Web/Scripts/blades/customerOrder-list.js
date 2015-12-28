﻿angular.module('virtoCommerce.orderModule')
.controller('virtoCommerce.orderModule.customerOrderListController', ['$scope', 'virtoCommerce.orderModule.order_res_customerOrders', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.authService', 'uiGridConstants', 'platformWebApp.uiGridHelper', 'dateFilter',
function ($scope, order_res_customerOrders, bladeNavigationService, dialogService, authService, uiGridConstants, uiGridHelper, dateFilter) {
    var blade = $scope.blade;
    $scope.uiGridConstants = uiGridConstants;

    //pagination settings
    $scope.pageSettings = {};
    $scope.pageSettings.totalItems = 0;
    $scope.pageSettings.currentPage = 1;
    $scope.pageSettings.numPages = 5;
    $scope.pageSettings.itemsPerPageCount = 20;

    $scope.filter = {
        searchKeyword: undefined
    };

    blade.refresh = function () {
        blade.isLoading = true;

        var criteria = {
            keyword: $scope.filter.searchKeyword,
            start: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
            count: $scope.pageSettings.itemsPerPageCount
        };
        order_res_customerOrders.search(criteria, function (data) {
            blade.isLoading = false;

            $scope.pageSettings.totalItems = angular.isDefined(data.totalCount) ? data.totalCount : 0;
            $scope.objects = data.customerOrders;
        },
	   function (error) {
	       bladeNavigationService.setError('Error ' + error.status, blade);
	   });
    };

    $scope.$watch('pageSettings.currentPage', function (newPage) {
        blade.refresh();
    });

    $scope.selectNode = function (node) {
        $scope.selectedNodeId = node.id;
        var newBlade = {
            id: 'orderDetail',
            title: 'orders.blades.customerOrder-detail.title',
            titleValues: { customer: node.customerName },
            subtitle: 'orders.blades.customerOrder-detail.subtitle',
            customerOrder: node,
            controller: 'virtoCommerce.orderModule.operationDetailController',
            template: 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/customerOrder-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.deleteList = function (list) {
        var dialog = {
            id: "confirmDeleteItem",
            title: "orders.dialogs.orders-delete.title",
            message: "orders.dialogs.orders-delete.message",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var itemIds = _.pluck(list, 'id');
                    order_res_customerOrders.remove({ ids: itemIds }, function (data, headers) {
                        blade.refresh();
                    },
                    function (error) {
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

    blade.headIcon = 'fa-file-text';

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
                      name: "platform.commands.delete", icon: 'fa fa-trash-o',
                      executeMethod: function () {
                          $scope.deleteList($scope.gridApi.selection.getSelectedRows());
                      },
                      canExecuteMethod: function () {
                          return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
                      },
                      permission: 'order:delete'
                  }
    ];

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        var createdDateColumn = _.findWhere(gridOptions.columnDefs, { name: 'createdDate' });
        if (createdDateColumn) { // custom tooltip
            createdDateColumn.cellTooltip = function (row, col) { return dateFilter(row.entity.createdDate, 'medium'); }
        }
        uiGridHelper.initialize($scope, gridOptions);
    };


    // actions on load
    //No need to call this because page 'pageSettings.currentPage' is watched!!! It would trigger subsequent duplicated req...
    //blade.refresh();
}]);