angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.categoriesItemsAddController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', function ($scope, bladeNavigationService, categories, items) {
    var pb = $scope.blade.parentBlade;

    $scope.addCategory = function () {
        categories.newCategory({ catalogId: pb.catalogId, parentCategoryId: pb.categoryId },
            function (data) {
                $scope.bladeClose(function() {
                    var newBlade = {
                        id: "newCategoryWizard",
                        currentEntity: data,
                        title: "New category",
                        subtitle: 'Fill category information',
                        controller: 'virtoCommerce.catalogModule.newCategoryWizardController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newCategory/category-wizard.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, pb);

                });
            });
    };

    $scope.addLinkedCategory = function () {
        $scope.bladeClose(function () {
            var newBlade = {
                id: 'selectCatalog',
                title: 'Select Catalog',
                subtitle: 'Creating a Link inside virtual catalog',
                controller: 'virtoCommerce.catalogModule.catalogsSelectController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalogs-select.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade.parentBlade);
        });
    };

    $scope.addProduct = function () {
        if (!angular.isDefined(pb.categoryId)) {
            items.newItemInCatalog({ catalogId: pb.catalogId }, function (data) {
                $scope.bladeClose(function() {
                    pb.showNewItemWizard(data);
                    });
            });
        }
        else {
            items.newItemInCategory({ catalogId: pb.catalogId, categoryId: pb.categoryId }, function (data) {
                $scope.bladeClose(function () {
                    pb.showNewItemWizard(data);
                });
            });
        }
    };

    $scope.addVariation = function () {
        items.newVariation({ itemId: pb.currentItemId }, function (data) {
            $scope.bladeClose(function () {
                pb.showNewVariationWizard(data);
            });
        });
    };

    $scope.blade.isLoading = false;
}]);
