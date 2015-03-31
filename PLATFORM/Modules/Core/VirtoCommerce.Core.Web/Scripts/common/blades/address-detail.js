angular.module('virtoCommerce.coreModule')
.controller('coreAddressDetailController', ['$scope', 'countries', 'dialogService', function ($scope, countries, dialogService) {
    $scope.addressTypes = ['Billing', 'Shipping'];
    function initializeBlade() {
    	
    	if ($scope.blade.currentEntity.isNew)
    	{
    		$scope.blade.currentEntity.addressType =  $scope.addressTypes[1];
    	}
     
    	$scope.blade.origEntity = angular.copy($scope.blade.currentEntity);
        $scope.blade.isLoading = false;
    };

    function isDirty() {
    	return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return $scope.formScope && $scope.formScope.$valid;
    }

    $scope.saveChanges = function () {
    	if ($scope.blade.confirmChangesFn) {
    		$scope.blade.confirmChangesFn($scope.blade.currentEntity);
    	};
    	angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The Address has been modified. Do you want to save changes?",
                callback: function (needSave) {
                    if (needSave) {
                    	$scope.saveChanges();
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
            message: "Are you sure you want to delete this Address?",
            callback: function (remove) {
            	if (remove) {
            		if ($scope.blade.deleteFn) {
            			$scope.blade.deleteFn($scope.blade.currentEntity);
            		};
            		$scope.bladeClose();
            	}
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    $scope.bladeHeadIco = 'fa fa-user';

    $scope.bladeToolbarCommands = [
        {
            name: "Reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
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
                return !$scope.blade.currentEntity.isNew && !isDirty();
            }
        }
    ];

    $scope.$watch('blade.currentEntity.countryCode', function (countryCode) {
        if (countryCode) {
            $scope.blade.currentEntity.countryName = _.findWhere($scope.countries, { code: countryCode }).name;
        }
    });


    // on load
    $scope.countries = countries.query();
    initializeBlade();
}]);