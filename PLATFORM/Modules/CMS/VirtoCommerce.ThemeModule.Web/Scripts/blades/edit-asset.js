angular.module('virtoCommerce.content.themeModule.blades.editAsset', [
	'virtoCommerce.content.themeModule.resources.themes'
])
.controller('editAssetController', ['$scope', 'dialogService', 'themes', function ($scope, dialogService, themes) {
	var blade = $scope.blade;

	function initializeBlade() {
		if (!blade.newAsset) {
			themes.getAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId, assetId: blade.choosenAssetId }, function (data) {
				blade.isLoading = false;
				blade.currentEntity = angular.copy(data);
				blade.origEntity = data;
			});

			$scope.bladeToolbarCommands = [
			{
				name: "Save", icon: 'fa fa-save',
				executeMethod: function () {
					saveChanges();
				},
				canExecuteMethod: function () {
					return isDirty();
				}
			},
			{
				name: "Reset", icon: 'fa fa-undo',
				executeMethod: function () {
					angular.copy(blade.origEntity, blade.currentEntity);
				},
				canExecuteMethod: function () {
					return isDirty();
				}
			},
			{
				name: "Delete", icon: 'fa fa-trash-o',
				executeMethod: function () {
					deleteEntry();
				},
				canExecuteMethod: function () {
					return !isDirty();
				}
			}];
		}
		else {
			$scope.bladeToolbarCommands = [
			{
				name: "Save", icon: 'fa fa-save',
				executeMethod: function () {
					saveChanges();
				},
				canExecuteMethod: function () {
					return isCanSave();
				}
			}];

			blade.isLoading = false;
		}
    };

    function isDirty() {
    	return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    function saveChanges() {
    	blade.isLoading = true;

    	themes.updateAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId }, blade.currentEntity, function () {
    		blade.parentBlade.refresh(true);
    		blade.choosenAssetId = blade.currentEntity.id;
    		blade.title = blade.currentEntity.id;
    		blade.subtitle = 'Edit asset';
    		blade.newAsset = false;
    		initializeBlade();
        });
    };

    function deleteEntry() {
    	var dialog = {
    		id: "confirmDelete",
    		title: "Delete confirmation",
    		message: "Are you sure you want to delete this asset?",
    		callback: function (remove) {
    			if (remove) {
    				$scope.blade.isLoading = true;

    				themes.deleteAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId, assetIds: blade.choosenAssetId }, function () {
    					$scope.bladeClose();
    					$scope.blade.parentBlade.refresh(true);
    				});
    			}
    		}
    	}
    	dialogService.showConfirmationDialog(dialog);
    }

    function isCanSave() {
    	if (!angular.isUndefined(blade.currentEntity)) {
    		if (!angular.isUndefined(blade.currentEntity.name) && !angular.isUndefined(blade.currentEntity.content)) {
    			return true;
    		}
    		return false;
    	}
    	else {
    		return false;
    	}
    }

    blade.onClose = function (closeCallback) {
    	if ((isDirty() && !blade.newAsset) || (isCanSave() && blade.newAsset)) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The asset has been modified. Do you want to save changes?",
                callback: function (needSave) {
                    if (needSave) {
                        saveChanges();
                    }
                    closeCallback();
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    $scope.bladeHeadIco = 'fa fa-archive';

    initializeBlade();
}]);
