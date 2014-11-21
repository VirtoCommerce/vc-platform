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

    $scope.openBlade = function (type)
    {
        switch (type)
        {
            case 'properties':
                //TODO
                break;
            case 'images':
                var newBlade = {
                    id: "newProductImages",
                    item: $scope.blade.item,
                    title: $scope.blade.item.name,
                    style: "actions",
                    subtitle: 'item images',
                    controller: 'newProductWizardImagesController',
                    template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/item-image-detail.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, $scope.blade);


                break;
            case 'seo':
                //TODO
                break;
            case 'review':
                //TODO
                break;
        }
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


