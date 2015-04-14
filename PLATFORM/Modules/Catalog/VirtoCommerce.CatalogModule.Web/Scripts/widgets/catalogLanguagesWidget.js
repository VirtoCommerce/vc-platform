angular.module('virtoCommerce.catalogModule')
.controller('catalogLanguagesWidgetController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    $scope.openBlade = function () {
        var newBlade = {
            id: "catalogChildBlade",
            title: blade.title,
            subtitle: 'Manage languages',
            controller: 'catalogLanguagesController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalog-languages.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };
}]);