angular.module('catalogModule.widget.seoWidget', [
])
.controller('seoWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openSeoBlade = function () {
        var blade = {
            id: "seoDetail",
            currentEntityId: $scope.currentBlade.currentEntityId,
            seoUrlKeywordType: getSeoUrlKeywordType(),
            seoInfos: getSeoInfos(),
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

    function getSeoInfos() {
        if (angular.isDefined($scope.currentBlade.currentEntity)) {
            return $scope.currentBlade.currentEntity.seoInfos;
        } else {
            return $scope.currentBlade.item.seoInfos;
        }
    }
}]);
