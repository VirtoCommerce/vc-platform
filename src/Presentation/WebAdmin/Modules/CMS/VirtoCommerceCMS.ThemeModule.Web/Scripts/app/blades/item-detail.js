angular.module('virtoCommerce.content.blades.itemDetails', [
    'virtoCommerce.content.resources.contents',
    'codemwnci.markdown-edit-preview'
])
.controller('contentItemDetailsController', ['$rootScope', '$scope', 'bladeNavigationService', 'dialogService', 'contents', function ($rootScope, $scope, bladeNavigationService, dialogService, contents) {
    $scope.selectedEntityId = null;

    //alert($scope.blade.currentEntity.id);
    $scope.blade.refresh = function (parentRefresh) {
        $scope.blade.isLoading = true;

        if ($scope.blade.currentEntity == null) {
            $scope.blade.origEntity = null;
            $scope.blade.isLoading = false;
        } else {
            contents.getItem(
            {
                collectionId: $scope.blade.collectionId,
                itemId: $scope.blade.itemId
            }, function(results) {
                $scope.blade.isLoading = false;
                $scope.blade.currentEntity = angular.copy(results);;
                $scope.blade.origEntity = results;
            });
        }

        if (parentRefresh) {
            $scope.blade.parentBlade.refresh();
        }
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    function saveChanges() {
        $scope.blade.isLoading = true;

        var itemId = $scope.blade.itemId;
        if(itemId == null)
            itemId = $scope.blade.currentEntity.id;

        contents.saveItem(
        {
            collectionId: $scope.blade.collectionId,
            itemId: itemId,
        }, $scope.blade.currentEntity, function (data, headers) {
            if ($scope.blade.origEntity == null) {
                $scope.bladeClose();
                $scope.blade.parentBlade.refresh();
            } else {
                $scope.blade.currentEntity = data;
                $scope.blade.itemId = $scope.blade.currentEntity.id;
                $scope.blade.refresh(true);
            }
        });
    };

    function publishChanges() {
        $scope.blade.isLoading = true;
        contents.publishItem(
        {
            collectionId: $scope.blade.collectionId,
            itemId: $scope.blade.itemId,
        }, $scope.blade.currentEntity, function (data, headers) {
            $scope.blade.refresh(true);
        });
    };

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'icon-floppy',
            executeMethod: function () {
                saveChanges();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        /*
        {
            name: "Publish", icon: 'icon-floppy',
            executeMethod: function () {
                publishChanges();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
        */
        {
            name: "Reset", icon: 'icon-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];

    $scope.blade.refresh(false);
}]);
