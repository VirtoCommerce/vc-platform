angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.categoriesItemsAddController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.categories', 'virtoCommerce.catalogModule.items', function ($scope, bladeNavigationService, categories, items) {
    var blade = $scope.blade;
    var pb = blade.parentBlade;

    $scope.addCategory = function () {
        categories.newCategory({ catalogId: pb.catalogId, parentCategoryId: pb.categoryId },
            function (data) {
                $scope.bladeClose(function () {
                    var newBlade = {
                        id: "newCategoryWizard",
                        currentEntity: data,
                        title: "catalog.wizards.category.title",
                        subtitle: 'catalog.wizards.category.subtitle',
                        controller: 'virtoCommerce.catalogModule.newCategoryWizardController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newCategory/category-wizard.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, pb);

                });
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    $scope.addLinkedCategory = function () {
        $scope.bladeClose(function () {
            var newBlade = {
                id: 'selectCatalog',
                title: 'catalog.blades.catalogs-select.title',
                subtitle: 'catalog.blades.catalogs-select.subtitle',
                controller: 'virtoCommerce.catalogModule.catalogsSelectController',
                template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/catalogs-select.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, pb);
        });
    };

    $scope.addProduct = function (productType) {
        if (!angular.isDefined(pb.categoryId)) {
            items.newItemInCatalog({ catalogId: pb.catalogId }, function (data) {
                data.productType = productType;
                $scope.bladeClose(function () {
                    showNewItemWizard(data);
                });
            },
            function (error) { bladeNavigationService.setError('Error ' +error.status, $scope.blade); });
        }
        else {
            items.newItemInCategory({ catalogId: pb.catalogId, categoryId: pb.categoryId }, function (data) {
                        data.productType = productType;
                        $scope.bladeClose(function () {
                            showNewItemWizard(data);
                        });
                    },
                    function (error) { bladeNavigationService.setError('Error ' +error.status, $scope.blade); });
        }
    };

    function showNewItemWizard(data) {
        // take product and variation properties only
        data.properties = _.filter(data.properties, function (x) { return x.type === 'Product' || x.type === 'Variation'; });

        var newBlade = {
            id: 'listItemDetail',
            item: data,
            title: "catalog.wizards.new-product.title",
            subtitle: 'catalog.wizards.new-product.subtitle',
            controller: 'virtoCommerce.catalogModule.newProductWizardController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/newProduct/new-product-wizard.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, pb);
    };

    blade.isLoading = false;
}]);
