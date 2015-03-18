angular.module('virtoCommerce.catalogModule')
.controller('virtualCatalogDetailController', ['$rootScope', '$scope', 'bladeNavigationService', '$injector', 'catalogs', 'dialogService', function ($rootScope, $scope, bladeNavigationService, $injector, catalogs, dialogService) {

    $scope.currentBlade = $scope.blade;

    $scope.currentBlade.refresh = function (parentRefresh) {
    	//Refresh only when has id
    	if (angular.isDefined($scope.currentBlade.currentEntityId)) {
    		catalogs.get({ id: $scope.currentBlade.currentEntityId }, function (data) {
    			initializeBlade(data);
    			if (parentRefresh) {
    				$scope.currentBlade.parentBlade.refresh();
    			}
    		});
    	}
    	else {
    		initializeBlade($scope.currentBlade.currentEntity);

    	}

    }

    function initializeBlade(data) {
        $scope.currentBlade.currentEntityId = data.id;
        $scope.currentBlade.title = data.name;

        $scope.currentBlade.currentEntity = angular.copy(data);
        $scope.currentBlade.origEntity = data;
        $scope.currentBlade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.currentBlade.currentEntity, $scope.currentBlade.origEntity);
    };

    function saveChanges() {
    	$scope.currentBlade.isLoading = true;
    	if (angular.isDefined($scope.currentBlade.currentEntityId)) {
    		catalogs.update({}, $scope.currentBlade.currentEntity, function (data, headers) {
    			$scope.currentBlade.refresh(true);
    		});
    	}
    	else {
    		catalogs.create({}, $scope.currentBlade.currentEntity, function (data, headers) {
    			$scope.currentBlade.currentEntityId = data.id;
    			$scope.currentBlade.refresh(true);
    		});
    	}
    };

    function closeThisBlade(closeCallback) {
        if ($scope.currentBlade.childrenBlades.length > 0) {
            var callback = function () {
                if ($scope.currentBlade.childrenBlades.length == 0) {
                    closeCallback();
                };
            };
            angular.forEach($scope.currentBlade.childrenBlades, function (child) {
                bladeNavigationService.closeBlade(child, callback);
            });
        }
        else {
            closeCallback();
        }
    };

    $scope.currentBlade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The virtual catalog has been modified. Do you want to save changes?"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    saveChanges();
                }
                closeThisBlade(closeCallback);
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeThisBlade(closeCallback);
        }
    };

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
                angular.copy($scope.currentBlade.origEntity, $scope.currentBlade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];

    if ($scope.currentBlade.currentEntity != null) {
        initializeBlade($scope.currentBlade.currentEntity);
    } else {
        $scope.currentBlade.refresh(false);
    }
}]);
