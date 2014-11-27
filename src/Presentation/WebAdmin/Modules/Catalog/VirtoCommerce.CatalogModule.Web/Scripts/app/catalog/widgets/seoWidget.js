angular.module('catalogModule.widget.seoWidget', [
])
.controller('seoWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openSeoBlade = function () {
        var blade = {
            id: "seoDetail",
            seoUrlKeywordType: getSeoUrlKeywordType(),
            parentEntity: getParentEntity(),
            title: $scope.currentBlade.title,
            subtitle: 'Seo details',
            controller: 'seoDetailController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/seo-detail.tpl.html'
        };

        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

    function getSeoUrlKeywordType() {
        if (angular.isDefined($scope.currentBlade.currentEntity)) {
            return 0;  // SeoUrlKeywordTypes enum
        } else {
            return 1;
        }
    }

    function getParentEntity() {
        if (angular.isDefined($scope.currentBlade.currentEntity)) {
            return $scope.currentBlade.currentEntity;
        } else {
            return $scope.currentBlade.item;
        }
    }
}]);
