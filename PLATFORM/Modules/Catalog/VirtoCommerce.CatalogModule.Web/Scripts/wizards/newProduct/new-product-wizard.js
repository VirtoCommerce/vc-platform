﻿angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.newProductWizardController', ['$scope', 'platformWebApp.bladeNavigationService', '$http', function ($scope, bladeNavigationService, $http) {
    var blade = $scope.blade;
    blade.headIcon = blade.item.productType === 'Digital' ? 'fa fa-file-archive-o' : 'fa fa-truck';

    var initialName = blade.item.name ? blade.item.name : '';
    var lastGeneratedName = initialName;

    $scope.createItem = function () {
        blade.isLoading = true;

        blade.item.$update(null,
            function (dbItem) {
                blade.parentBlade.setSelectedItem(dbItem)
                blade.parentBlade.refresh(true);

                var newBlade = {
                    id: blade.id,
                    itemId: dbItem.id,
                    productType: dbItem.productType,
                    title: dbItem.name,
                    controller: 'virtoCommerce.catalogModule.itemDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade.parentBlade);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    $scope.openBlade = function (type) {
        var newBlade = null;
        switch (type) {
            case 'properties':
                newBlade = {
                    id: "newProductProperties",
                    item: blade.item,
                    title: blade.item.name,
                    subtitle: 'catalog.blades.item-property-list.subtitle',
                    bottomTemplate: '$(Platform)/Scripts/common/templates/ok.tpl.html',
                    controller: 'virtoCommerce.catalogModule.newProductWizardPropertiesController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-property-list.tpl.html'
                };
                break;
            case 'images':
                newBlade = {
                    id: "newProductImages",
                    item: blade.item,
                    title: blade.item.name,
                    subtitle: 'catalog.wizards.item-images.subtitle',
                    bottomTemplate: '$(Platform)/Scripts/common/templates/ok.tpl.html',
                    controller: 'virtoCommerce.catalogModule.newProductWizardImagesController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/images.tpl.html'
                };
                break;
            case 'seo':
                initializeSEO(blade.item, function () {
                    blade.currentEntity = blade.item; // reference for child blade
                    blade.seoLanguages = _.pluck(getCatalog().languages, 'languageCode');

                    newBlade = {
                        id: 'seoDetails',
                        store: { name: 'default store' },
                        updatePermission: 'catalog:create',
                        controller: 'virtoCommerce.coreModule.seo.seoDetailController',
                        template: 'Modules/$(VirtoCommerce.Core)/Scripts/SEO/blades/seo-detail.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                });
                break;
            case 'review':
                if (blade.item.reviews != undefined && blade.item.reviews.length > 0) {
                    newBlade = {
                        id: "newProductEditorialReviewsList",
                        currentEntities: blade.item.reviews,
                        title: blade.item.name,
                        subtitle: 'catalog.blades.editorialReviews-list.subtitle',
                        controller: 'virtoCommerce.catalogModule.newProductWizardReviewsController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/editorialReviews-list.tpl.html'
                    };
                } else {
                    newBlade = {
                        id: 'editorialReviewWizard',
                        currentEntity: { languageCode: getCatalog().defaultLanguage.languageCode },
                        languages: getCatalog().languages,
                        title: 'catalog.blades.editorialReview-detail.title',
                        subtitle: 'catalog.blades.editorialReview-detail.subtitle',
                        bottomTemplate: '$(Platform)/Scripts/common/templates/ok.tpl.html',
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
        var pattern = /[$+;=%{}[\]|\\\/@ ~!^*&()?:'<>,]/;
        return !pattern.test(value);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.getUnfilledProperties = function () {
        return _.filter(blade.item.properties, function (p) {
            return p && _.any(p.values) && p.values[0].value;
        });
    }

    function getCatalog() {
        var parentBlade = blade.parentBlade;
        while (!parentBlade.catalog) {
            parentBlade = parentBlade.parentBlade;
        }
        return parentBlade.catalog;
    }

    function initializeSEO(item, callback) {
        if (!item.seoInfos)
            item.seoInfos = [];
        var data = item.seoInfos;
        var seoLanguages = _.pluck(getCatalog().languages, 'languageCode');
        if (data.length < seoLanguages.length) {
            var stringForSlug = item.name;
            _.each(item.properties, function (prop) {
                _.each(prop.values, function (val) {
                    stringForSlug += ' ' + val.value;
                });
            });

            if (stringForSlug) {
                _.each(seoLanguages, function (lang) {
                    if (_.every(data, function (seoInfo) { return seoInfo.languageCode.toLowerCase().indexOf(lang.toLowerCase()) < 0; })) {
                        data.push({ languageCode: lang });
                    }
                });

                $http.get('api/catalog/getslug?text=' + stringForSlug)
                    .success(function (slug) {
                        _.each(data, function (seo) {
                            if (angular.isUndefined(seo.semanticUrl)) {
                                seo.semanticUrl = slug;
                            }
                        });
                        callback();
                    });
            } else
                callback();
        } else
            callback();
    }

    $scope.$watch('blade.item.properties', function (currentEntities) {
        // auto-generate item.name from property values if user didn't change it
        if ((lastGeneratedName == blade.item.name || (!lastGeneratedName && !blade.item.name))
            && _.any(blade.childrenBlades, function (x) { return x.controller === 'virtoCommerce.catalogModule.newProductWizardPropertiesController'; })) {
            lastGeneratedName = initialName;
            _.each(currentEntities, function (x) {
                if (_.any(x.values, function (val) { return val.value; })) {
                    var currVal = _.find(x.values, function (val) { return val.value; });
                    if (currVal) {
                        lastGeneratedName += (lastGeneratedName ? ', ' : '') + currVal.value;
                    }
                }
            });
            blade.item.name = lastGeneratedName;
        }
    });


    blade.isLoading = false;
}]);
