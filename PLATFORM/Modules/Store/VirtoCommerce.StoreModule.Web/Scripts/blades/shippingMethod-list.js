angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.shippingMethodListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    
    function initializeBlade(data) {
        $scope.blade.currentEntities = data;
        $scope.blade.isLoading = false;
    };

    $scope.selectNode = function (node) {
        $scope.selectedNodeId = node.code;

        var newBlade = {
            id: 'shippingMethodList',
            data: node,
            title: $scope.blade.title,
            subtitle: 'stores.blades.shippingMethod-detail.subtitle',
            controller: 'virtoCommerce.storeModule.shippingMethodDetailController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/shippingMethod-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
    
    $scope.sortableOptions = {
        stop: function (e, ui) {
            for (var i = 0; i < $scope.blade.currentEntities.length; i++) {
                $scope.blade.currentEntities[i].priority = i + 1;
            }
        },
        axis: 'y',
        cursor: "move"
    };

    $scope.blade.headIcon = 'fa-archive';

    $scope.$watch('blade.parentBlade.currentEntity.shippingMethods', initializeBlade);

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.shippingMethods' gets fired
}]);