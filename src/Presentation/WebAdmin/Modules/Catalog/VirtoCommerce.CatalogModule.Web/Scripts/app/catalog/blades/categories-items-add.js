angular.module('catalogModule.blades.categoriesItemsAdd', [])
.controller('categoriesItemsAddController', ['$scope', 'bladeNavigationService', 'categories', 'items', function ($scope, bladeNavigationService, categories, items) {
    var pb = $scope.blade.parentBlade;

    $scope.addCategory = function () {
        categories.newCategory({ catalogId: pb.catalogId, parentCategoryId: pb.categoryId },
            function (data) {
                pb.showCategoryBlade(data.id, data);
                $scope.bladeClose();
                pb.setSelectedItem(data);
                pb.refresh();
            });
    };

    $scope.addLinkedCategory = function () {
        $scope.bladeClose();

        var newBlade = {
            id: 'selectCatalog',
            title: 'Select Catalog',
            subtitle: 'Creating a Link inside virtual catalog',
            controller: 'catalogsSelectController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/catalogs-select.tpl.html',
            isClosingDisabled: true
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
    };

    $scope.addProduct = function () {
    	if (!angular.isDefined(pb.categoryId)) {
    		items.newItemInCatalog({ catalogId: pb.catalogId }, function (data) {
    			pb.showNewItemWizard(data);
    			$scope.bladeClose();
    		});
    	}
    	else {
    		items.newItemInCategory({ catalogId: pb.catalogId, categoryId: pb.categoryId }, function (data) {
    			pb.showNewItemWizard(data);
    			$scope.bladeClose();
    		});
    	}
    };

    $scope.addVariation = function () {
        items.newVariation({ itemId: pb.currentItemId }, function (data) {
            pb.showNewVariationWizard(data);
            $scope.bladeClose();
        });
    };

    $scope.blade.isLoading = false;
}]);
