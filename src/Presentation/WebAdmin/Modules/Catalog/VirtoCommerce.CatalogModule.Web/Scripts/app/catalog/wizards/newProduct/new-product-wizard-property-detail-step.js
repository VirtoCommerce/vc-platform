angular.module('catalogModule.wizards.newProductWizard.properties', [
    'ui.bootstrap.typeahead',
    'catalogModule.resources.properties'
])
.controller('newProductWizardPropertiesController', ['$scope', 'properties', 'bladeNavigationService', function ($scope, properties, bladeNavigationService)
{
    $scope.blade.isLoading = false;
    $scope.blade.item = angular.copy($scope.blade.item);

    $scope.saveChanges = function()
    {
        $scope.blade.parentBlade.item.properties = $scope.blade.item.properties;
        $scope.bladeClose();
    };

    $scope.editProperty = function (prop)
    {
        var newBlade = {
            id: 'editCategoryProperty',
            currentEntityId: prop.id,
            title: 'Edit category property',
            subtitle: 'enter property information',
            controller: 'propertyDetailController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/property-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.getPropValues = function (propId, keyword)
    {
        return properties.query({ propertyId: propId, keyword: keyword }).$promise.then(function (result)
        {
            return result;
        });
    };

}]);
