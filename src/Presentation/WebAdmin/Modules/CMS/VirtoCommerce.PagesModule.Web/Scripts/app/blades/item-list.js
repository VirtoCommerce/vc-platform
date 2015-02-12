angular.module('virtoCommerce.content.blades.itemList', [
    'virtoCommerce.content.resources.contents',
    'virtoCommerce.content.blades.itemDetails'
])
.controller('contentItemsListController', ['$rootScope', '$scope', '$filter', 'bladeNavigationService', 'dialogService', 'contents', function ($rootScope, $scope, $filter, bladeNavigationService, dialogService, contents) {
    $scope.selectedEntityId = null;

    //alert($scope.blade.currentEntity.id);
    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        contents.getItems(
        {
            collectionId: $scope.blade.collectionId
        }, function (results) {
            $scope.blade.isLoading = false;
            $scope.blade.currentEntities = results;
        });
    };

    $scope.selectItem = function (listItem) {
        var newBlade = {
            id: 'itemDetails',
            title: 'Content Item',
            subtitle: 'Modifying "' + listItem.id + '"',
            currentEntity: listItem,
            collectionId: $scope.blade.collectionId,
            itemId: listItem.id,
            controller: 'contentItemDetailsController',
            template: 'Modules/Content/VirtoCommerce.ContentModule.Web/Scripts/app/blades/item-detail.tpl.html'
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
        },
        {
            name: "Delete", icon: 'icon-remove',
            executeMethod: function () {
                deleteChecked();
            },
            canExecuteMethod: function () {
                return isItemsChecked();
            }
        }
    ];

    $scope.delete = function () {
        if (isItemsChecked()) {
            deleteChecked();
        } else {
            var dialog = {
                id: "notifyNoTargetContentItem",
                title: "Message",
                message: "Nothing selected. Check some Items first."
            };
            dialogService.showNotificationDialog(dialog);
        }

        //preventCategoryListingOnce = true;
    };

    function deleteChecked() {
        var dialog = {
            id: "confirmDeleteItem",
            title: "Delete confirmation",
            message: "Are you sure you want to delete selected Items?",
            callback: function (remove) {
                if (remove) {
                    closeChildrenBlades();

                    var selection = $filter('filter')($scope.blade.currentEntities, { selected: true }, true);

                    var itemIds = [];
                    angular.forEach(selection, function(listItem) {
                        itemIds.push(listItem.id);
                    });

                    if (itemIds.length > 0) {
                        contents.remove({ collectionId: $scope.blade.collectionId, ids: itemIds }, function (data, headers) {
                            //$scope.blade.parentBlade.refresh();
                            $scope.blade.refresh();
                        });
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.checkAll = function (selected) {
        angular.forEach($scope.blade.currentEntities, function (item) {
            item.selected = selected;
        });
    };

    function isItemsChecked() {
        if ($scope.blade.currentEntities) {
            return _.any($scope.blade.currentEntities, function (x) { return x.selected; });
        } else {
            return false;
        }
    }

    function openAddEntityBlade() {
        closeChildrenBlades();

        var newBlade = {
            id: "contentItemCreate",
            title: "Content Item",
            currentEntity: null,
            collectionId: $scope.blade.collectionId,
            subtitle: 'Create new content item',
            controller: 'contentItemDetailsController',
            template: 'Modules/Content/VirtoCommerce.ContentModule.Web/Scripts/app/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    $scope.blade.refresh();
}]);
