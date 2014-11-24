angular.module('catalogModule.wizards.newProductWizard', [
])
.controller('newProductWizardController', ['$scope', 'bladeNavigationService', 'dialogService', 'items', function ($scope, bladeNavigationService, dialogService, items)
{
    $scope.blade.isLoading = false;
    $scope.bladeActions = "Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-product-wizard-actions.tpl.html";


    $scope.createItem = function ()
    {

        $scope.blade.item.$updateitem(null,
        function (dbItem)
        {
            $scope.bladeClose();
            $scope.blade.parentBlade.setSelectedItem(dbItem);
            $scope.blade.parentBlade.refresh();
        });
    }

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
                        style: "actions",
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
                        style: "actions",
                        subtitle: 'item images',
                        controller: 'newProductWizardImagesController',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-image-detail.tpl.html'
                    };
                    break;
                case 'seo':
                    //TODO
                    break;
                case 'review':
                    newBlade = {
                        id: "editorialReviewsList",
                        currentEntities: $scope.blade.item.reviews,
                        title: $scope.blade.item.name,
                        style: "actions",
                        subtitle: 'Product Reviews',
                        controller: 'newProductWizardReviewsController',
                        template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/editorialReviews-list.tpl.html'
                    };
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


}]);


