angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.seoWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    $scope.currentBlade = $scope.widget.blade;

    $scope.openSeoBlade = function () {
        var blade = {
            id: "seoDetail",
            seoUrlKeywordType: getSeoUrlKeywordType(),
            parentEntity: getParentEntity(),
            title: $scope.currentBlade.title,
            subtitle: 'Seo details',
            controller: 'virtoCommerce.catalogModule.seoDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/seo-detail.tpl.html'
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
