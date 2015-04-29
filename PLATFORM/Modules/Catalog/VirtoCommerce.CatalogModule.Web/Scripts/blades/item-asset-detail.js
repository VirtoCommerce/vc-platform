angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemAssetController', ['$rootScope', '$scope', 'virtoCommerce.catalogModule.items', 'bladeNavigationService', '$filter', 'FileUploader', 'dialogService', '$injector', function ($rootScope, $scope, items, bladeNavigationService, $filter, FileUploader, dialogService, $injector) {
    $scope.currentBlade = $scope.blade;
    $scope.item = {};
    $scope.origItem = {};

    $scope.currentBlade.refresh = function (parentRefresh) {
        items.get({ id: $scope.currentBlade.itemId }, function (data) {
            $scope.origItem = data;
            $scope.item = angular.copy(data);
            $scope.currentBlade.isLoading = false;
            if (parentRefresh) {
                $scope.currentBlade.parentBlade.refresh();
            }
        });
    }

    $scope.isDirty = function () {
        return !angular.equals($scope.item, $scope.origItem);
    };

    $scope.reset = function () {
        angular.copy($scope.origItem, $scope.item);
    };

    $scope.currentBlade.onClose = function (closeCallback) {
        if ($scope.isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The assets has been modified. Do you want to save changes?"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    $scope.saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };


    $scope.saveChanges = function () {
    	$scope.currentBlade.isLoading = true;
    	items.updateitem({}, $scope.item, function (data) {
            $scope.currentBlade.refresh(true);
        });
    };

    function initialize() {
        if (!$scope.uploader) {
            // Creates a uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/assets',
                method: 'POST',
                autoUpload: true,
                removeAfterUpload: true
            });

            uploader.onSuccessItem = function (fileItem, assets, status, headers) {
                angular.forEach(assets, function (asset) {
                    asset.itemId = $scope.item.id;
                    //ADD uploaded asset to the item
                    $scope.item.assets.push(asset);
                });
            };
        }
    };

    $scope.toggleAssetSelect = function (e, asset) {
        if (e.ctrlKey == 1) {
            asset.selected = !asset.selected;
        } else {
            angular.forEach($scope.item.assets, function (i) {
                i.selected = false;
            });
            asset.selected = true;
        }
    }

    $scope.removeAction = function (selectedAssets) {
        if (selectedAssets == undefined) {
            selectedAssets = $filter('filter')($scope.item.assets, { selected: true });
        }

        angular.forEach(selectedAssets, function (asset) {
            var idx = $scope.item.assets.indexOf(asset);
            if (idx >= 0) {
                $scope.item.assets.splice(idx, 1);
            }
        });
    };

    $scope.copyUrl = function (data) {
        window.prompt("Copy to clipboard: Ctrl+C, Enter", data.url);
    }

    $scope.bladeToolbarCommands = [

        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return $scope.isDirty();
            },
            permission: 'catalog:items:manage'
        },
		{
		    name: "Remove", icon: 'fa fa-trash-o', executeMethod: function () { $scope.removeAction(); },
		    canExecuteMethod: function () {
		        var retVal = false;
		        if (angular.isDefined($scope.item.assets)) {
		            var selectedAssets = $filter('filter')($scope.item.assets, { selected: true });
		            retVal = selectedAssets.length > 0;
		        }
		        return retVal;
		    },
		    permission: 'catalog:items:manage'
		}
    ];

    initialize();
    $scope.currentBlade.refresh();

}]);
