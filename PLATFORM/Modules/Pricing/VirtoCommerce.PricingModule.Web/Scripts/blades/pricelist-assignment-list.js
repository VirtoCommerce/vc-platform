angular.module('virtoCommerce.pricingModule')
.controller('virtoCommerce.pricingModule.pricelistAssignmentListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var selectedNode = null;

    function initializeBlade(data) {
        $scope.blade.currentEntities = data;
        $scope.blade.isLoading = false;
    }

    $scope.selectNode = function (node) {
        selectedNode = node;
        $scope.selectedNodeId = selectedNode.id;

        var newBlade = {
            id: 'pricelistChildChild',
            currentEntityId: selectedNode.id,
            title: selectedNode.name,
            subtitle: 'pricing.blades.assignment-detail.edit-subtitle',
            controller: 'virtoCommerce.pricingModule.assignmentDetailController',
            template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/assignment-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };
    
    $scope.blade.headIcon = 'fa-usd';

    $scope.$watch('blade.parentBlade.currentEntity.assignments', function (currentEntities) {
        // $scope.blade.data = currentEntities;
        initializeBlade(currentEntities);
    });

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.assignments' gets fired
}]);