angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.pricelistListController', ['$scope', 'virtoCommerce.pricingModule.pricelists', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService',
function ($scope, pricelists, bladeNavigationService, dialogService) {
    var selectedNode = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        $scope.blade.selectedAll = false;

        pricelists.query({}, function (data) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = data;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, $scope.blade);
        });
    };

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'listItemChild',
            currentEntityId: selectedNode.id,
            title: selectedNode.name,
            subtitle: $scope.blade.subtitle,
            controller: 'virtoCommerce.pricingModule.pricelistDetailController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/pricelist-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.toggleAll = function () {
        angular.forEach($scope.blade.currentEntities, function (item) {
            item.selected = $scope.blade.selectedAll;
        });
    };

    function isItemsChecked() {
        return $scope.blade.currentEntities && _.any($scope.blade.currentEntities, function (x) { return x.selected; });
    }

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "pricing.dialogs.pricelists-delete.title",
            message: "pricing.dialogs.pricelists-delete.message",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = _.where($scope.blade.currentEntities, { selected: true });
                    var itemIds = _.pluck(selection, 'id');
                    pricelists.remove({ ids: itemIds }, function (data, headers) {
                        $scope.blade.refresh();
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.blade.headIcon = 'fa-usd';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: function () {
                $scope.blade.refresh();
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
                    title: 'New Price list',
                    subtitle: $scope.blade.subtitle,
                    isNew: true,
                    controller: 'virtoCommerce.pricingModule.pricelistDetailController',
                    template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/pricelist-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'pricing:create'
        },
        //{
        //    name: "Clone", icon: 'fa fa-files-o',
        //    executeMethod: function () {
        //    },
        //    canExecuteMethod: function () {
        //        return false;
        //    },
        //    permission: 'pricing:update'
        //},
        {
            name: "platform.commands.delete", icon: 'fa fa-trash-o',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return isItemsChecked();
            },
            permission: 'pricing:delete'
        }
    ];

    // actions on load
    $scope.blade.refresh();
}]);