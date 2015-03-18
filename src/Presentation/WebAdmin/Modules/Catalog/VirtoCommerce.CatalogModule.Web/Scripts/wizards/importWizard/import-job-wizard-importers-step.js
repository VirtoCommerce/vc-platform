angular.module('virtoCommerce.catalogModule')
.controller('importJobImportersController', ['$scope', function ($scope) {
    $scope.blade.refresh = function () {
        $scope.selectedImporter = $scope.blade.parentBlade.item.entityImporter;
        $scope.blade.isLoading = false;
        $scope.importers = [
            "Product",
            //"Sku",
            //"Bundle",
            //"Package",
            //"DynamicKit",
            "Category"
            //"Association",
            //"Price",
            //"ItemRelation",
            //"Inventory",
            //"Customer",
            //"Jurisdiction",
            //"JurisdictionGroup",
            //"TaxCategory",
            //"TaxValue",
            //"ItemAsset",
            //"Localization",
            //"Seo"
        ];

    };

    $scope.setImporter = function (importer) {
        $scope.selectedImporter = importer;
        $scope.blade.parentBlade.item.entityImporter = importer;
        $scope.bladeClose();
    }

    $scope.blade.refresh();

}]);


