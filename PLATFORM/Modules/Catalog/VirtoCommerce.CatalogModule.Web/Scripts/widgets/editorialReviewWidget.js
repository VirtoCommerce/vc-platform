angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.editorialReviewWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openBlade = function () {
        var blade = {
            id: "editorialReviewsList",
            currentEntityId: $scope.currentBlade.currentEntityId,
            currentEntities: $scope.currentBlade.item.reviews,
            title: $scope.currentBlade.title,
            subtitle: 'Product Reviews',
            controller: 'virtoCommerce.catalogModule.editorialReviewsListController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/editorialReviews-list.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
