angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.taxProviderListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    
    function initializeBlade(data) {
        $scope.blade.currentEntities = data;
        $scope.blade.isLoading = false;

        $scope.blade.currentEntities.sort(function (a, b) {
            return a.priority > b.priority;
        });
    };

    $scope.selectNode = function (node) {
        $scope.selectedNodeId = node.code;

        var newBlade = {
            id: 'taxProviderList',
            origEntity: node,
            title: $scope.blade.title,
            subtitle: 'Edit tax provider',
            controller: 'virtoCommerce.storeModule.taxProviderDetailController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/taxProvider-detail.tpl.html'
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

    $scope.$watch('blade.parentBlade.currentEntity.taxProviders', function (currentEntities) {
        initializeBlade(currentEntities);
    });

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.taxProviders' gets fired
}]);