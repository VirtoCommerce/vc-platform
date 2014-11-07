angular.module('catalogModule.blades.categoriesItemsAdd', [])
.controller('categoriesItemsAddController', ['$scope', 'dialogService', 'categories', 'items', function ($scope, dialogService, categories, items) {
    var pb = $scope.blade.parentBlade;

    $scope.addCategory = function () {
        categories.newCategory({ catalogId: pb.catalogId, parentCategoryId: pb.categoryId },
            function (data) {
                pb.showCategoryBlade(data.id, data);
                $scope.bladeClose();
                pb.setSelectedItem(data);
                pb.refresh();
            });
    };

    $scope.addProduct = function () {
        items.newItem({ catalogId: pb.catalogId, categoryId: pb.categoryId }, function (data) {
            pb.showItemBlade(data.id, data.name);
            $scope.bladeClose();
            pb.setSelectedItem(data);
            pb.refresh();
        });
    };

    $scope.addVariation = function () {
        items.newVariation({ itemId: pb.currentItemId }, function (data) {
            pb.showItemBlade(data.id, data.name);
            $scope.bladeClose();
            pb.setSelectedItem(data);
            pb.refresh();
        });
    };

    $scope.blade.isLoading = false;
}]);
