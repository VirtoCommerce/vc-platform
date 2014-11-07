angular.module('catalogModule.blades.catalogDetail', [])
.controller('catalogDetailController', ['$rootScope', '$scope', 'bladeNavigationService', '$injector', 'catalogs', 'dialogService', function ($rootScope, $scope, bladeNavigationService, $injector, catalogs, dialogService) {
    $scope.currentBlade = $scope.blade;
    //$scope.currentBlade.origEntity = {};
    //$scope.currentBlade.currentEntity = {};

    $scope.currentBlade.refresh = function (parentRefresh) {
        catalogs.get({ id: $scope.currentBlade.currentEntityId }, function (data) {
            initializeBlade(data);
            if (parentRefresh) {
                $scope.currentBlade.parentBlade.refresh();
            }
        });
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
        catalogs.update({}, $scope.currentBlade.currentEntity, function (data, headers) {
            $scope.currentBlade.refresh(true);
        });
    };

    $scope.currentBlade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The catalog has been modified. Do you want to save changes?"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    $scope.bladeToolbarCommands = [
	    {
	        name: "Save", icon: 'icon-floppy',
	        executeMethod: function () {
	            saveChanges();
	        },
	        canExecuteMethod: function () {
	            return isDirty();
	        }
	    },
        {
            name: "Reset", icon: 'icon-undo',
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
    }

    $scope.currentBlade.refresh(false);
    

}]);
