angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.paymentMethodListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

    function initializeBlade(data) {
        $scope.blade.currentEntities = data;
        $scope.blade.isLoading = false;
    };

    $scope.selectNode = function (node) {
        $scope.selectedNodeId = node.code;

        var newBlade = {
            id: 'paymentMethodList',
            data: node,
            title: $scope.blade.title,
            subtitle: 'stores.blades.paymentMethod-detail.subtitle',
            controller: 'virtoCommerce.storeModule.paymentMethodDetailController',
            template: 'Modules/$(VirtoCommerce.Store)/Scripts/blades/paymentMethod-detail.tpl.html'
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

    $scope.$watch('blade.parentBlade.currentEntity.paymentMethods', initializeBlade);

    // actions on load
    // $scope.$watch('blade.parentBlade.currentEntity.paymentMethods' gets fired
}]);