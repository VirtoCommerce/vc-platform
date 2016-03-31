angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemImageWidgetController', ['$scope', 'virtoCommerce.catalogModule.items', 'virtoCommerce.catalogModule.categories', 'platformWebApp.bladeNavigationService', function ($scope, items, categories, bladeNavigationService) {

    $scope.openBlade = function () {
        var blade = {
            id: "itemImage",
            currentEntityId: $scope.blade.currentEntityId,
            title: $scope.blade.title,
            currentResource: ($scope.blade.currentEntity && angular.isDefined($scope.blade.currentEntity.isVirtual)) ? categories : items,
            subtitle: 'catalog.widgets.itemImage.blade-subtitle',
            controller: 'virtoCommerce.catalogModule.imagesController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/images.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.blade);
    };

    function setCurrentEntities(images) {
        if (images) {
            $scope.currentEntities = images;
        }
    }
    $scope.$watch('blade.item.images', setCurrentEntities);
    $scope.$watch('blade.currentEntity.images', setCurrentEntities);
}]);
