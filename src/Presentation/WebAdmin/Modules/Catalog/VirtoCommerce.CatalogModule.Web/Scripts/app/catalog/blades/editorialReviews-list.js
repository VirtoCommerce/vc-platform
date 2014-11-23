angular.module('catalogModule.blades.editorialReviewsList', ['ngSanitize'])
.controller('editorialReviewsListController', ['$rootScope', '$scope', 'bladeNavigationService', 'items', function ($rootScope, $scope, bladeNavigationService, items) {
    //$scope.blade.origEntity = {};
    //$scope.blade.currentEntities = {};

    $scope.blade.refresh = function (parentRefresh) {
        if (parentRefresh) {
            $scope.blade.isLoading = true;
            $scope.blade.parentBlade.refresh().$promise.then(function (data) {
                initializeBlade(data.reviews);
            });
        } else {
            initializeBlade($scope.blade.currentEntities);
        }
    }

    function initializeBlade(data) {
        $scope.blade.currentEntities = angular.copy(data);
        $scope.blade.origItem = data;
        $scope.blade.isLoading = false;
    };

    $scope.openBlade = function (data) {
        var newBlade = {
            id: 'editorialReview',
            currentEntity: data,
            languages: $scope.blade.parentBlade.item.catalog.languages,
            title: 'Review',
            subtitle: 'Product Review',
            controller: 'editorialReviewDetailController',
            template: 'Modules/Catalog/VirtoCommerce.CatalogModule.Web/Scripts/app/catalog/blades/editorialReview-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Add", icon: 'icon-plus',
            executeMethod: function () {
                var data = { languageCode: $scope.blade.parentBlade.item.catalog.defaultLanguage.languageCode };
                $scope.openBlade(data);
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    $scope.blade.refresh(false);

}]);
