angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.pricingMainController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.selectedNodeId = null;

    function initializeBlade() {
        var entities = [
            { name: 'Price lists', entityName: 'pricelist', icon: 'fa-usd' },
            { name: 'Price list assignments', entityName: 'assignment', icon: 'fa-anchor' }];
        $scope.blade.currentEntities = entities;
        $scope.blade.isLoading = false;

        $scope.blade.openBlade(entities[0]);
    };

    $scope.blade.openBlade = function (data) {
        $scope.selectedNodeId = data.entityName;

        var newBlade = {
            id: 'pricingList',
            title: data.name,
            subtitle: 'Merchandise management',
            controller: 'virtoCommerce.pricingModule.'+ data.entityName + 'ListController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/' + data.entityName + '-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.bladeHeadIco = 'fa-usd';

    initializeBlade();
}]);
