angular.module('virtoCommerce.catalogModule')
.controller('newProductWizardPropertiesController', ['$scope', 'properties', 'bladeNavigationService', function ($scope, properties, bladeNavigationService) {
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
            //must copy values
            prop.values = foundProp.values;
            $scope.blade.item.properties.splice(idx, 1, prop);
        } else {
            $scope.blade.item.properties.push(prop);
        }

    };

    $scope.editProperty = function (prop) {
        var newBlade = {
            id: 'editCategoryProperty',
            currentEntityId: prop.id,
            title: 'Edit category property',
            subtitle: 'enter property information',
            controller: 'propertyDetailController',
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
