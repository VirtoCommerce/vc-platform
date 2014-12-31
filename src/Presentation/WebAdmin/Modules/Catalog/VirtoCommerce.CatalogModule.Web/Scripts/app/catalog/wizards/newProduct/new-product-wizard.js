angular.module('catalogModule.wizards.newProductWizard', [])
.controller('newProductWizardController', ['$scope', 'bladeNavigationService', 'dialogService', 'items', function ($scope, bladeNavigationService, dialogService, items) {
    $scope.createItem = function () {
        $scope.blade.item.$updateitem(null,
        function (dbItem) {
            $scope.bladeClose();

            //TODO: need better way to find category list blade.
            var categoryListBlade = $scope.blade.parentBlade;

            if (categoryListBlade.controller != 'categoriesItemsListController') {
                categoryListBlade = categoryListBlade.parentBlade;
            }

            categoryListBlade.refresh();

            var newBlade = {
                id: "listItemDetail",
                itemId: dbItem.id,
                title: dbItem.name,
                style: 'gray',
                subtitle: 'Item details',
                controller: 'itemDetailController',
                template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-detail.tpl.html'
            };

            bladeNavigationService.showBlade(newBlade, categoryListBlade);
        });
    }

    $scope.openBlade = function (type) {
        $scope.blade.onClose(function () {
            var newBlade = null;
            switch (type) {
                case 'properties':
                    newBlade = {
                        id: "newProductProperties",
                        item: $scope.blade.item,
                        title: $scope.blade.item.name,
                        subtitle: 'item properties',
                        isNew: true,
                        controller: 'newProductWizardPropertiesController',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-property-detail.tpl.html'
                    };
                    break;
                case 'images':
                    newBlade = {
                        id: "newProductImages",
                        item: $scope.blade.item,
                        title: $scope.blade.item.name,
                        subtitle: 'item images',
                        controller: 'newProductWizardImagesController',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-image-detail.tpl.html'
                    };
                    break;
                case 'seo':
                    newBlade = {
                        id: "newProductSeoDetail",
                        seoInfos: $scope.blade.item.seoInfos,
                        title: $scope.blade.item.name,
                        subtitle: 'Seo details',
                        controller: 'newProductSeoDetailController',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/seo-detail.tpl.html'
                    };
                    break;
                case 'review':
                    if ($scope.blade.item.reviews != undefined && $scope.blade.item.reviews.length > 0) {
                        newBlade = {
                            id: "newProductEditorialReviewsList",
                            currentEntities: $scope.blade.item.reviews,
                            title: $scope.blade.item.name,
                            subtitle: 'Product Reviews',
                            controller: 'newProductWizardReviewsController',
                            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/editorialReviews-list.tpl.html'
                        };
                    } else {
                        newBlade = {
                            id: 'editorialReviewWizard',
                            currentEntity: { languageCode: $scope.blade.parentBlade.catalog.defaultLanguage.languageCode },
                            languages: $scope.blade.parentBlade.catalog.languages,
                            title: 'Review',
                            subtitle: 'Product Review',
                            controller: 'editorialReviewDetailWizardStepController',
                            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/editorialReview-detail.tpl.html'
                        };
                    }
                    break;
            }

            if (newBlade != null) {
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            }
        });
    }

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }


    $scope.blade.onClose = function (closeCallback) {

        if ($scope.blade.childrenBlades.length > 0) {
            var callback = function () {
                if ($scope.blade.childrenBlades.length == 0) {
                    closeCallback();
                };
            };
            angular.forEach($scope.blade.childrenBlades, function (child) {
                bladeNavigationService.closeBlade(child, callback);
            });
        }
        else {
            closeCallback();
        }
    };

    $scope.getUnfilledProperties = function () {
        return _.filter($scope.blade.item.properties, function (p) {
            return p != undefined && p.values.length > 0 && p.values[0].value.length > 0;
        });
    }

    $scope.blade.isLoading = false;
}]);


