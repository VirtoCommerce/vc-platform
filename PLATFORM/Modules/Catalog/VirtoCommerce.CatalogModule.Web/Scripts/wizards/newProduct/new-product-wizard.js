angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newProductWizardController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

    var initialName = blade.item.name;
    var lastGeneratedName = blade.item.name;

    $scope.createItem = function () {
        blade.isLoading = true;

        blade.item.$updateitem(null,
            function (dbItem) {
                blade.parentBlade.refresh(true);

                var newBlade = {
                    id: blade.id,
                    itemId: dbItem.id,
                    title: dbItem.name,
                    controller: 'virtoCommerce.catalogModule.itemDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade.parentBlade);
            });
    }

    $scope.openBlade = function (type) {
        var newBlade = null;
        switch (type) {
            case 'properties':
                newBlade = {
                    id: "newProductProperties",
                    item: blade.item,
                    title: blade.item.name,
                    subtitle: 'item properties',
                    bottomTemplate: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/common/wizard-ok-action.tpl.html',
                    controller: 'virtoCommerce.catalogModule.newProductWizardPropertiesController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-property-list.tpl.html'
                };
                break;
            case 'images':
                newBlade = {
                    id: "newProductImages",
                    item: blade.item,
                    title: blade.item.name,
                    subtitle: 'item images',
                    bottomTemplate: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/common/wizard-ok-action.tpl.html',
                    controller: 'virtoCommerce.catalogModule.newProductWizardImagesController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-image-detail.tpl.html'
                };
                break;
            case 'seo':
                newBlade = {
                    id: "newProductSeoDetail",
                    item: blade.item,
                    title: blade.item.name,
                    subtitle: 'Seo details',
                    bottomTemplate: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/common/wizard-ok-action.tpl.html',
                    controller: 'virtoCommerce.catalogModule.newProductSeoDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/seo-detail.tpl.html'
                };
                break;
            case 'review':
                if (blade.item.reviews != undefined && blade.item.reviews.length > 0) {
                    newBlade = {
                        id: "newProductEditorialReviewsList",
                        currentEntities: blade.item.reviews,
                        title: blade.item.name,
                        subtitle: 'Product Reviews',
                        controller: 'virtoCommerce.catalogModule.newProductWizardReviewsController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/editorialReviews-list.tpl.html'
                    };
                } else {
                    newBlade = {
                        id: 'editorialReviewWizard',
                        currentEntity: { languageCode: getCatalog().defaultLanguage.languageCode },
                        languages: getCatalog().languages,
                        title: 'Review',
                        subtitle: 'Product Review',
                        bottomTemplate: 'Modules/$(VirtoCommerce.Catalog)/Scripts/wizards/common/wizard-ok-action.tpl.html',
                        controller: 'virtoCommerce.catalogModule.editorialReviewDetailWizardStepController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/editorialReview-detail.tpl.html'
                    };
                }
                break;
        }

        if (newBlade != null) {
            bladeNavigationService.showBlade(newBlade, blade);
        }
    }

    $scope.codeValidator = function (value) {
        var pattern = /[$+;=%{}[\]|\\\/@ ~#!^*&()?:'<>,]/;
        return !pattern.test(value);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.getUnfilledProperties = function () {
        return _.filter(blade.item.properties, function (p) {
            return p != undefined && p.values.length > 0 && p.values[0].value.length > 0;
        });
    }

    function getCatalog() {
        var parentBlade = blade.parentBlade;
        while (!parentBlade.catalog) {
            parentBlade = parentBlade.parentBlade;
        }
        return parentBlade.catalog;
    }

    $scope.$watch('blade.item.properties', function (currentEntities) {
        if (lastGeneratedName === blade.item.name
        && blade.childrenBlades.length > 0
        && blade.childrenBlades[0].controller === 'virtoCommerce.catalogModule.newProductWizardPropertiesController') {
            lastGeneratedName = initialName;
            _.each(currentEntities, function (x) {
                lastGeneratedName += ', ' + x.values[0].value;
            });
            blade.item.name = lastGeneratedName;
        }
    });


    blade.isLoading = false;
}]);


