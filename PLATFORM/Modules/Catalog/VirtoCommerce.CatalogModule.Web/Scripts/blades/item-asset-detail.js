angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.itemAssetController', ['$rootScope', '$scope', 'virtoCommerce.catalogModule.items', 'platformWebApp.bladeNavigationService', '$filter', 'FileUploader', 'platformWebApp.dialogService', '$injector', function ($rootScope, $scope, items, bladeNavigationService, $filter, FileUploader, dialogService, $injector) {
    var blade = $scope.blade;
    $scope.item = {};
    $scope.origItem = {};

    blade.refresh = function (parentRefresh) {
        items.get({ id: blade.itemId }, function (data) {
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
        return !angular.equals($scope.item, $scope.origItem);
    };

    $scope.reset = function () {
        angular.copy($scope.origItem, $scope.item);
    };

    blade.onClose = function (closeCallback) {
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
        blade.isLoading = true;
        items.update({}, { id: blade.itemId, assets: $scope.item.assets }, function (data) {
            blade.refresh(true);
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    function initialize() {
        if (!$scope.uploader) {
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
    blade.refresh();

}]);
