angular.module('virtoCommerce.orderModule.blades')
.controller('customerOrderDetailController', ['$scope', 'dialogService', 'bladeNavigationService', 'customerOrders', 'notificationService', function ($scope, dialogService, bladeNavigationService, customerOrders, notificationService) {
	$scope.blade.currentEntity = {};

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        customerOrders.get({ id: $scope.blade.currentEntityId }, function (results) {
        	$scope.blade.origEntity = results;

        	var copy = angular.copy(results);
        	$scope.blade.currentEntity = copy;
        	$scope.blade.operation = copy;
        	$scope.blade.customerOrder = copy;

            $scope.blade.isLoading = false;
        },
        function (error) {
            $scope.blade.isLoading = false;
            notificationService.setError('Error ' + error.status, $scope.blade);
        });
    }

    function isDirty() {

    	var retVal = !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    	console.log(retVal);
    	return retVal;
    };

    function saveChanges() {
        $scope.blade.isLoading = true;
        customerOrders.update({}, $scope.blade.currentEntity, function (data, headers) {
            $scope.blade.refresh();
        });
    };

    $scope.bladeToolbarCommands = [
        {
            name: "New document", icon: 'fa fa-plus',
            executeMethod: function () {
                openAddEntityWizard();
            },
            canExecuteMethod: function () {
                return true;
            }
        },
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
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: function () {
                return isDirty();
            }
        }
    ];

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The order has been modified. Do you want to save changes?",
                callback: function (needSave) {
                    if (needSave) {
                        saveChanges();
                    }
                    closeChildrenBlades();
                    closeCallback();
                }
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeChildrenBlades();
            closeCallback();
        }
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    // actions on load
    $scope.toolbarTemplate = 'Modules/Order/VirtoCommerce.OrderModule.Web/Scripts/blades/customerOrder-detail-toolbar.tpl.html';
    $scope.blade.refresh();

}]);