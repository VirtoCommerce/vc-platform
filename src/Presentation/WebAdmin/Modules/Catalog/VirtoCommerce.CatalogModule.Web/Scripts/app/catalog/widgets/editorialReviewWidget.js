angular.module('catalogModule.widget.editorialReviewWidget', [])
.controller('editorialReviewWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openBlade = function () {
        var blade = {
            id: "editorialReviewsList",
            currentEntityId: $scope.currentBlade.currentEntityId,
            currentEntities: $scope.currentBlade.item.reviews,
            title: $scope.currentBlade.title,
            subtitle: 'Product Reviews',
            controller: 'editorialReviewsListController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/editorialReviews-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
