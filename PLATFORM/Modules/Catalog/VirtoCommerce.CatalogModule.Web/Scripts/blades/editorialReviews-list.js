angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.editorialReviewsListController', ['$rootScope', '$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.items', function ($rootScope, $scope, bladeNavigationService, items) {
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
            controller: 'virtoCommerce.catalogModule.editorialReviewDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/editorialReview-detail.tpl.html'
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

    function openAddEntityBlade() {
        var data = {
            isNew: true,
            languageCode: $scope.blade.parentBlade.item.catalog.defaultLanguage.languageCode
        };
        $scope.openBlade(data);
    }

    $scope.bladeToolbarCommands = [
        {
            name: "Add", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityBlade();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'catalog:items:manage'
        }
    ];

    $scope.blade.refresh(false);

    // open blade for new review 
    if (!_.some($scope.blade.currentEntities)) {
        openAddEntityBlade();
    }
}]);
