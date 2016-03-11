angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.editorialReviewsListController', ['$timeout', '$scope', 'platformWebApp.bladeNavigationService', function ($timeout, $scope, bladeNavigationService) {
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';

    blade.refresh = function (parentRefresh) {
        if (parentRefresh) {
            blade.isLoading = true;
            blade.parentBlade.refresh().$promise.then(function (data) {
                initializeBlade(data.reviews);
            });
        } else {
            initializeBlade(blade.currentEntities);
        }
    }

    function initializeBlade(data) {
        blade.currentEntities = angular.copy(data);
        blade.origItem = data;
        blade.isLoading = false;
    };

    $scope.openBlade = function (data) {
        var newBlade = {
            id: 'editorialReview',
            currentEntity: data,
            languages: blade.parentBlade.item.catalog.languages,
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
            languageCode: blade.parentBlade.item.catalog.defaultLanguage.languageCode
        };
        $scope.openBlade(data);
    }

    blade.headIcon = 'fa-comments';

    blade.toolbarCommands = [
        {
            name: "platform.commands.add", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityBlade();
            },
            canExecuteMethod: function () {
                return true;
            },
            permission: blade.updatePermission
        }
    ];

    blade.refresh(false);

    // open blade for new review 
    if (!_.some(blade.currentEntities)) {
        $timeout(openAddEntityBlade, 60, false);
    }
}]);
