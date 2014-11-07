angular.module('catalogModule.widget.catalogLanguagesWidget', [
])
.controller('catalogLanguagesWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService)
{

    $scope.currentBlade = $scope.widget.blade;

    $scope.openBlade = function (manageDefaultLanguage) {
        var blade = {
            id: "catalogLanguages",
            currentEntityId: $scope.currentBlade.currentEntityId,
            manageDefaultLanguage: manageDefaultLanguage,
            title: "Language management",
            subtitle: 'Language selection',
            controller: 'catalogLanguagesController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/catalog-languages.tpl.html'
        };
        bladeNavigationService.showBlade(blade, $scope.currentBlade);
    };

}]);
