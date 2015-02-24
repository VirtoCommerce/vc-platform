angular.module('virtoCommerce.content.themeModule.blades.editAsset', [
	'virtoCommerce.content.themeModule.resources.themes'
])
.controller('editAssetController', ['$scope', 'dialogService', 'themes', function ($scope, dialogService, themes) {
	var blade = $scope.blade;

	function initializeBlade() {
		themes.getAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId, assetId: blade.choosenAssetId }, function (data) {
			blade.isLoading = false;
			blade.currentEntity = angular.copy(data);
			blade.origEntity = data;
		});

        blade.isLoading = false;
    };

    function isDirty() {
    	return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    function saveChanges(data) {
    	blade.isLoading = true;

    	themes.updateAsset({ storeId: blade.choosenStoreId, themeId: blade.choosenThemeId }, blade.currentEntity, function () {
            $scope.blade.parentBlade.refresh(true);
        });
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The Review has been modified. Do you want to save changes?",
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

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete this Editorial Review?",
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

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                saveChanges($scope.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        },
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.currentEntity);
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
                return $scope.blade.parentBlade.currentEntities.indexOf($scope.blade.origEntity) >= 0 && !isDirty();
            }
        }
    ];

    initializeBlade();
    //$scope.$watch('blade.parentBlade.currentEntities', function (newEntities, oldEntities) {
    //    if (!angular.equals(newEntities, oldEntities)) {
    //        var currentChild = angular.isDefined($scope.choosenAssetId)
    //            ? _.find(newEntities, function (ent) { return ent.id === $scope.choosenAssetId; })
    //            : _.find(newEntities, function (ent) { return ent.content === $scope.currentEntity.content; });

    //        initializeBlade(currentChild);
    //    }
    //}, true);
}]);
