angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemAssetController', ['$scope', '$translate', 'virtoCommerce.catalogModule.items', 'platformWebApp.bladeNavigationService', '$filter', 'FileUploader', function ($scope, $translate, items, bladeNavigationService, $filter, FileUploader) {
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';
    $scope.item = {};
    $scope.origItem = {};

    blade.refresh = function (parentRefresh) {
        items.get({ id: blade.itemId }, function (data) {
            if ($scope.uploader)
                $scope.uploader.url = 'api/platform/assets?folderUrl=catalog/' + data.code;
            $scope.origItem = data;
            $scope.item = angular.copy(data);
            blade.isLoading = false;
            if (parentRefresh) {
                blade.parentBlade.refresh();
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    $scope.isDirty = function () {
        return !angular.equals($scope.item, $scope.origItem) && blade.hasUpdatePermission();
    };

    $scope.reset = function () {
        angular.copy($scope.origItem, $scope.item);
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded($scope.isDirty(), true, blade, $scope.saveChanges, closeCallback, "catalog.dialogs.asset-save.title", "catalog.dialogs.asset-save.message");
    };

    $scope.saveChanges = function () {
        blade.isLoading = true;
        items.update({}, { id: blade.itemId, assets: $scope.item.assets }, function (data) {
            blade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    function initialize() {
        if (!$scope.uploader && blade.hasUpdatePermission()) {
            // create the uploader
            var uploader = $scope.uploader = new FileUploader({
                scope: $scope,
                headers: { Accept: 'application/json' },
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

            uploader.onAfterAddingAll = function (addedItems) {
                bladeNavigationService.setError(null, blade);
            };

            uploader.onErrorItem = function (item, response, status, headers) {
                bladeNavigationService.setError(item._file.name + ' failed: ' + (response.message ? response.message : status), blade);
            };
        }
    };

    $scope.toggleAssetSelect = function (e, asset) {
        if (e.ctrlKey == 1) {
            asset.selected = !asset.selected;
        } else {
            if (asset.selected) {
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
        $translate('catalog.blades.item-asset-detail.labels.copy-url-prompt').then(function (promptMessage) {
            window.prompt(promptMessage, data.url);
        });
    }

    $scope.blade.headIcon = 'fa-chain';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return $scope.isDirty();
            },
            permission: blade.updatePermission
        }
    ];

    initialize();
    blade.refresh();

}]);
