angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemAssetController', ['$rootScope', '$scope', 'virtoCommerce.catalogModule.items', 'platformWebApp.bladeNavigationService', '$filter', 'FileUploader', 'platformWebApp.dialogService', '$injector', function ($rootScope, $scope, items, bladeNavigationService, $filter, FileUploader, dialogService, $injector) {
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
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
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
    	items.update({}, $scope.item, function (data) {
            $scope.currentBlade.refresh(true);
    	},
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    function initialize() {
        if (!$scope.uploader) {
            // create the uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
                url: 'api/platform/assets?folderUrl=catalog',
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
            if(asset.selected) {
                asset.selected = false;
            } else {
                asset.selected = true;
            }
        }
    }

    $scope.removeAction = function (asset) {
    	var idx = $scope.item.assets.indexOf(asset);
    	if (idx >= 0) {
    		$scope.item.assets.splice(idx, 1);
    	}
    };

    $scope.copyUrl = function (data) {
        window.prompt("Copy to clipboard: Ctrl+C, Enter", data.url);
    }

    $scope.blade.headIcon = 'fa-chain';

    $scope.blade.toolbarCommands = [

        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return $scope.isDirty();
            },
            permission: 'catalog:update'
        }		
    ];

    initialize();
    $scope.currentBlade.refresh();

}]);
