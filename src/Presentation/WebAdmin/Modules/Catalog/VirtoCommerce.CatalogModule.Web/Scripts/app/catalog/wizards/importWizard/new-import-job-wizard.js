angular.module('catalogModule.wizards.newImportJobWizard', [
       'angularFileUpload'
])
.controller('newImportJobWizardController', ['$scope', 'bladeNavigationService', 'dialogService', 'imports', 'FileUploader', function ($scope, bladeNavigationService, dialogService, imports, FileUploader)
{
    $scope.blade.isLoading = false;

    $scope.createItem = function ()
    {

        $scope.blade.item.$save(null,
        function (dbItem)
        {
            $scope.bladeClose();
            $scope.blade.parentBlade.refresh();
        });
    }

    function initialize()
    {
        if (!$scope.uploader)
        {
            // Creates a uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/assets/',
                method: 'PUT',
                autoUpload: true,
                removeAfterUpload: true
            });

            // ADDING FILTERS
            // Images only
            uploader.filters.push({
                name: 'csvFilter',
                fn: function (i /*{File|FileLikeObject}*/, options)
                {
                    var type = '|' + i.type.slice(i.type.lastIndexOf('/') + 1) + '|';
                    return '|csv|vnd.ms-excel|'.indexOf(type) !== -1;
                }
            });


            uploader.onSuccessItem = function (fileItem, asset, status, headers) {
                $scope.blade.item.templatePath = asset[0].url;
            };
        }
    };

    $scope.openBlade = function (type) {
        $scope.blade.onClose(function() {
            var newBlade = null;
            switch (type)
            {
                case 'properties':
                    newBlade = {
                        id: "newProductProperties",
                        item: $scope.blade.item,
                        title: $scope.blade.item.name,
                        bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-product-wizard-ok-action.tpl.html',
                        subtitle: 'item properties',
                        controller: 'newProductWizardPropertiesController',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-property-detail.tpl.html'
                    };
                    break;
                case 'images':
                    newBlade = {
                        id: "newProductImages",
                        item: $scope.blade.item,
                        title: $scope.blade.item.name,
                        bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-product-wizard-ok-action.tpl.html',
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
                        bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-product-wizard-ok-action.tpl.html',
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
                            bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-product-wizard-ok-action.tpl.html',
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
                            bladeActions: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-product-wizard-ok-action.tpl.html',
                            subtitle: 'Product Review',
                            controller: 'editorialReviewDetailWizardStepController',
                            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/editorialReview-detail.tpl.html'
                        };
                    }
                    break;
            }

            if (newBlade != null)
            {
                bladeNavigationService.showBlade(newBlade, $scope.blade);
            }
        });  
    }

    $scope.setForm = function (form)
    {
        $scope.formScope = form;
    }


    $scope.blade.onClose = function (closeCallback)
    {

        if ($scope.blade.childrenBlades.length > 0)
        {
            var callback = function ()
            {
                if ($scope.blade.childrenBlades.length == 0)
                {
                    closeCallback();
                };
            };
            angular.forEach($scope.blade.childrenBlades, function (child)
            {
                bladeNavigationService.closeBlade(child, callback);
            });
        }
        else
        {
            closeCallback();
        }
    };

    initialize();


}]);


