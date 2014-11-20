angular.module('catalogModule.wizards.newProductWizard', [
])
.controller('newProductWizardController', ['$scope', 'bladeNavigationService', 'dialogService', 'items', function ($scope, bladeNavigationService, dialogService, items)
{
    $scope.currentBlade = $scope.blade;
    $scope.currentBlade.isLoading = false;
    $scope.bladeActions = "Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/wizards/newProduct/new-product-wizard-actions.tpl.html";


    $scope.createItem = function ()
    {

        $scope.currentBlade.item.$updateitem(null,
        function (dbItem)
        {
            $scope.bladeClose();
            $scope.currentBlade.parentBlade.setSelectedItem(dbItem);
            $scope.currentBlade.parentBlade.refresh();
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
                //TODO
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


}]);


