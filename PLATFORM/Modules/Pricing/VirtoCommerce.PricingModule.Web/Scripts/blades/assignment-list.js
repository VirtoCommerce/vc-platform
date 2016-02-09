angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.assignmentListController', ['$scope', 'virtoCommerce.pricingModule.pricelistAssignments', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'uiGridConstants', 'platformWebApp.uiGridHelper',
function ($scope, assignments, bladeNavigationService, dialogService, uiGridConstants, uiGridHelper) {
    $scope.uiGridConstants = uiGridConstants;
    var blade = $scope.blade;

    blade.refresh = function () {
        blade.isLoading = true;

        assignments.query({}, function (data) {
            blade.isLoading = false;
            blade.currentEntities = data;
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
            controller: 'virtoCommerce.pricingModule.assignmentDetailController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/assignment-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };
    
    function isItemsChecked() {
        return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
    }

    $scope.deleteList = function (list) {
        var dialog = {
            id: "confirmDeleteItem",
            title: "pricing.dialogs.assignments-delete.title",
            message: "pricing.dialogs.assignments-delete.message",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var itemIds = _.pluck(list, 'id');
                    assignments.remove({ ids: itemIds }, function () {
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

    blade.headIcon = 'fa-anchor';
    blade.subtitle = 'pricing.blades.pricelist-assignment-list.subtitle';    

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
                    title: 'pricing.blades.assignment-detail.new-title',
                    subtitle: blade.subtitle,
                    isNew: true,
                    controller: 'virtoCommerce.pricingModule.assignmentDetailController',
                    template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/assignment-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'pricing:create'
        },
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                $scope.deleteList($scope.gridApi.selection.getSelectedRows());
            },
            canExecuteMethod: function () {
                return isItemsChecked();
            },
            permission: 'pricing:delete'
        }
    ];

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions);
    };

    // actions on load
    blade.refresh();
}]);