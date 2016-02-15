angular.module('virtoCommerce.inventoryModule')
.controller('virtoCommerce.inventoryModule.inventoryFulfillmentcentersListController', ['$scope', '$timeout', 'platformWebApp.bladeNavigationService', function ($scope, $timeout, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.selectedItem = null;
    var openFirstEntityDetailsOnce = _.once(function () {
        if (_.any(blade.currentEntities))
            $timeout(function () {
                $scope.openBlade(blade.currentEntities[0]);
            }, 0, false);
    });

    blade.refresh = function () {
        blade.isLoading = true;
        return blade.parentWidgetRefresh().$promise.then(function (results) {
            blade.isLoading = false;
            blade.currentEntities = results;

            openFirstEntityDetailsOnce();
            return results;
        });
    }

    $scope.openBlade = function (data) {
        $scope.selectedItem = data;

        var newBlade = {
            id: "inventoryDetailBlade",
            itemId: blade.itemId,
            data: data,
            title: data.fulfillmentCenter.name,
            subtitle: 'inventory.blades.inventory-detail.subtatle',
            controller: 'virtoCommerce.inventoryModule.inventoryDetailController',
            template: 'Modules/$(VirtoCommerce.Inventory)/Scripts/blades/inventory-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    blade.headIcon = 'fa-cubes';
    blade.toolbarCommands = [
        {
            name: "platform.commands.refresh", icon: 'fa fa-refresh',
            executeMethod: blade.refresh,
            canExecuteMethod: function () {
                return true;
            }
        },
		{
		    name: "core.blades.fulfillment-center-list.subtitle", icon: 'fa fa-wrench',
		    executeMethod: function () {
		        var newBlade = {
		            id: 'fulfillmentCenterList',
		            controller: 'virtoCommerce.coreModule.fulfillment.fulfillmentListController',
		            template: 'Modules/$(VirtoCommerce.Core)/Scripts/fulfillment/blades/fulfillment-center-list.tpl.html'
		        };
		        bladeNavigationService.showBlade(newBlade, blade.parentBlade);
		    },
		    canExecuteMethod: function () { return true; },
		    permission: 'core:fulfillment:create'
		}
    ];

    blade.refresh();
}]);