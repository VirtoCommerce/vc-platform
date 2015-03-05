angular.module('virtoCommerce.content.pagesModule.blades.editPage', [
	'virtoCommerce.content.pagesModule.resources.pages'
])
.controller('editPageController', ['$scope', 'dialogService', 'pages', function ($scope, dialogService, pages) {
	var blade = $scope.blade;

	function initializeBlade() {
		if (!blade.newPage) {
			pages.getPage({ storeId: blade.choosenStoreId, pageName: blade.choosenPageName }, function (data) {
				blade.isLoading = false;
				blade.currentEntity = angular.copy(data);
				blade.origEntity = data;
			});

			$scope.bladeToolbarCommands = [
			{
				name: "Save page", icon: 'fa fa-save',
				executeMethod: function () {
					saveChanges();
				},
				canExecuteMethod: function () {
					return isDirty();
				}
			},
			{
				name: "Reset page", icon: 'fa fa-undo',
				executeMethod: function () {
					angular.copy(blade.origEntity, blade.currentEntity);
				},
				canExecuteMethod: function () {
					return isDirty();
				}
			},
			{
				name: "Delete page", icon: 'fa fa-trash-o',
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
				name: "Save page", icon: 'fa fa-save',
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

    	pages.update({ storeId: blade.choosenStoreId }, blade.currentEntity, function () {
    		blade.parentBlade.refresh(true);
    		blade.choosenPageName = blade.currentEntity.name;
    		blade.title = blade.currentEntity.name;
    		blade.subtitle = 'Edit page';
    		blade.newPage = false;
    		initializeBlade();
        });
    };

    function deleteEntry() {
    	var dialog = {
    		id: "confirmDelete",
    		title: "Delete confirmation",
    		message: "Are you sure you want to delete this page?",
    		callback: function (remove) {
    			if (remove) {
    				blade.isLoading = true;

    				pages.delete({ storeId: blade.choosenStoreId, pageNames: blade.choosenPageName }, function () {
    					$scope.bladeClose();
    					blade.parentBlade.refresh();
    				});
    			}
    		}
    	}
    	dialogService.showConfirmationDialog(dialog);
    }

    function isCanSave() {
    	return (!(angular.isUndefined(blade.currentEntity.name) || blade.currentEntity.name === null) &&
			!(angular.isUndefined(blade.currentEntity.content) || blade.currentEntity.content === null));
    }

    blade.onClose = function (closeCallback) {
    	if ((isDirty() && !blade.newPage) || (isCanSave() && blade.newPage)) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The page has been modified. Do you want to save changes?",
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
