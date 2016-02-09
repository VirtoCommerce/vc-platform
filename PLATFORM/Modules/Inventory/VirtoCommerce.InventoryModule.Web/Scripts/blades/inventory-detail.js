angular.module('virtoCommerce.inventoryModule')
.controller('virtoCommerce.inventoryModule.inventoryDetailController', ['$scope', 'platformWebApp.dialogService', 'virtoCommerce.inventoryModule.inventories', function ($scope, dialogService, inventories) {

    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;
        $scope.blade.parentBlade.refresh().then(function (results) {
            var data = _.findWhere(results, { fulfillmentCenterId: $scope.blade.data.fulfillmentCenterId });
            
            // parse date fields
            if (data.preorderAvailabilityDate) {
                data.preorderAvailabilityDate = new Date(data.preorderAvailabilityDate);
            }
            if (data.backorderAvailabilityDate) {
                data.backorderAvailabilityDate = new Date(data.backorderAvailabilityDate);
            }

            initializeBlade(data);
        });
    }

    function initializeBlade(data) {
        $scope.blade.currentEntity = angular.copy(data);
        $scope.blade.origEntity = data;
        $scope.blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.saveChanges = function () {
        $scope.blade.isLoading = true;
        inventories.update({ id: $scope.blade.itemId }, $scope.blade.currentEntity, function () {
            $scope.blade.refresh();
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "inventory.dialogs.inventory-save.title",
                message: "inventory.dialogs.inventory-save.message",
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

    $scope.blade.headIcon = 'fa-cubes';

    $scope.blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty(); // && formScope && formScope.$valid;
            },
            permission: 'customer:update'
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            },
            canExecuteMethod: isDirty,
            permission: 'customer:update'
        }
    ];
    // datepicker
    $scope.datepickers = {
        pod: false,
        bod: false
    }
            
    // Disable weekend selection
    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    $scope.today = new Date();

    $scope.open = function ($event, which) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.datepickers[which] = true;
    };

    $scope.dateOptions = {
        'year-format': "'yyyy'",
    };

    $scope.formats = ['shortDate', 'dd-MMMM-yyyy', 'yyyy/MM/dd'];
    $scope.format = $scope.formats[0];


    // on load
    initializeBlade($scope.blade.data);
}]);