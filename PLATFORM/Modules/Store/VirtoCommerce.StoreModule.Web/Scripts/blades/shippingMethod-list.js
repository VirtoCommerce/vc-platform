angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.shippingMethodListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    
    function initializeBlade(data) {
        $scope.blade.currentEntities = data;
        $scope.blade.isLoading = false;
    };

    $scope.selectNode = function (node) {
        $scope.selectedNodeId = node.id;

        var newBlade = {
            id: 'shippingMethodList',
            origEntity: node,
            title: node.name,
            subtitle: 'Edit shipping method',
            controller: 'virtoCommerce.storeModule.shippingMethodDetailController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/shippingMethod-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
    
    $scope.bladeHeadIco = 'fa-archive';

    $scope.$watch('blade.parentBlade.currentEntity.shippingMethods', function (currentEntities) {
        initializeBlade(currentEntities);
    });

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.shippingMethods' gets fired
}]);