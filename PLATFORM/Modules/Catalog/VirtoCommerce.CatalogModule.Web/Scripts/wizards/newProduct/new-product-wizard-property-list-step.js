angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newProductWizardPropertiesController', ['$scope', 'virtoCommerce.catalogModule.properties', 'platformWebApp.bladeNavigationService', function ($scope, properties, bladeNavigationService) {
    $scope.blade.isLoading = false;
    $scope.blade.item = angular.copy($scope.blade.item);

    $scope.saveChanges = function () {
        $scope.blade.parentBlade.item.properties = $scope.blade.item.properties;
        $scope.bladeClose();
    };

    //property-details calls refresh with update property
    $scope.blade.refresh = function (prop) {
        var foundProp = _.findWhere($scope.blade.item.properties, { id: prop.id });
        if (foundProp != undefined) {
            var idx = $scope.blade.item.properties.indexOf(foundProp);
            // update property but leave current values. Need to change reference
            var values = foundProp.values;
            foundProp = angular.copy(prop);
            foundProp.values = values;
            $scope.blade.item.properties.splice(idx, 1, foundProp);            
        } else {
            $scope.blade.item.properties.push(prop);
        }

    };

    $scope.editProperty = function (prop) {
        var newBlade = {
            id: 'editCategoryProperty',
            currentEntityId: prop.id,
            title: 'catalog.blades.property-detail.title-category',
            subtitle: 'catalog.blades.property-detail.subtitle-category',
            controller: 'virtoCommerce.catalogModule.propertyDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/property-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.getPropValues = function (propId, keyword) {
        return properties.values({ propertyId: propId, keyword: keyword }).$promise.then(function (result) {
            return result;
        });
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

}]);
