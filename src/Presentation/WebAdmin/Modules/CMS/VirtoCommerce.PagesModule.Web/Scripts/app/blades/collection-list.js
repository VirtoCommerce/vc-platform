angular.module('virtoCommerce.content.blades.collectionList', [
    'virtoCommerce.content.resources.contents',
    'virtoCommerce.content.blades.itemList'
])
.controller('collectionListController', ['$rootScope', '$scope', 'bladeNavigationService', 'dialogService', 'contents', function ($rootScope, $scope, bladeNavigationService, dialogService, contents) {
    $scope.selectedEntityId = null;

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        contents.getCollections({}, function (results) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = results;
        });
    };

    $scope.selectItem = function (listItem) {
        var newBlade = {
            id: 'contentItemsList',
            title: 'Contents Collection',
            subtitle: 'Browsing "' + listItem.id + '"',
            currentEntity: listItem,
            controller: 'contentItemsListController',
            collectionId: listItem.id,
            template: 'Modules/Content/VirtoCommerce.ContentModule.Web/Scripts/app/blades/item-list.tpl.html'
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
                openAddEntityBlade();
            },
            canExecuteMethod: function () {
                return true;
            }
        }
    ];

    function openAddEntityBlade() {
        closeChildrenBlades();

        var newBlade = {
            id: "contentItemCreate",
            title: "Content Item",
            currentEntity: null,
            collectionId: null,
            subtitle: 'Create new content item',
            controller: 'contentItemDetailsController',
            template: 'Modules/Content/VirtoCommerce.ContentModule.Web/Scripts/app/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }


    $scope.blade.refresh();
}]);
