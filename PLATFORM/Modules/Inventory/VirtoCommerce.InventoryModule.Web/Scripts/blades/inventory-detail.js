angular.module('virtoCommerce.inventoryModule')
.controller('virtoCommerce.inventoryModule.inventoryDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.inventoryModule.inventories', function ($scope, bladeNavigationService, inventories) {
    var blade = $scope.blade;
    blade.updatePermission = 'inventory:update';

    blade.refresh = function () {
        blade.isLoading = true;
        blade.parentBlade.refresh().then(function (results) {
            var data = _.findWhere(results, { fulfillmentCenterId: blade.data.fulfillmentCenterId });

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
        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
    }

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        return isDirty() && $scope.formScope && $scope.formScope.$valid;
    }

    $scope.setForm = function (form) { $scope.formScope = form; };

    $scope.saveChanges = function () {
        blade.isLoading = true;
        inventories.update({ id: blade.itemId }, blade.currentEntity, function () {
            blade.refresh();
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "inventory.dialogs.inventory-save.title", "inventory.dialogs.inventory-save.message");
    };

    blade.headIcon = 'fa-cubes';
    blade.toolbarCommands = [
        {
            name: "platform.commands.save", icon: 'fa fa-save',
            executeMethod: $scope.saveChanges,
            canExecuteMethod: canSave,
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.reset", icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
            },
            canExecuteMethod: isDirty,
            permission: blade.updatePermission
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
    initializeBlade(blade.data);
}]);