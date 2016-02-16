angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.editorialReviewsListController', ['$timeout', '$scope', 'platformWebApp.bladeNavigationService', function ($timeout, $scope, bladeNavigationService) {

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
            title: 'catalog.blades.editorialReview-detail.title',
            subtitle: 'catalog.blades.editorialReview-detail.subtitle',
            controller: 'virtoCommerce.catalogModule.editorialReviewDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/editorialReview-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }
        
    function openAddEntityBlade() {
        var data = {
            isNew: true,
            languageCode: $scope.blade.parentBlade.item.catalog.defaultLanguage.languageCode
        };
        $scope.openBlade(data);
    }

    $scope.blade.headIcon = 'fa-comments';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityBlade();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: 'catalog:update'
        }
    ];

    $scope.blade.refresh(false);

    // open blade for new review 
    if (!_.some($scope.blade.currentEntities)) {
        $timeout(openAddEntityBlade, 60, false);
    }
}]);
