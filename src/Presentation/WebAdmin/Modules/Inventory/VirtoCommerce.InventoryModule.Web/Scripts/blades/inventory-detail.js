angular.module('virtoCommerce.inventoryModule.blades')
.controller('inventoryDetailController', ['$scope', 'dialogService', 'inventories', function ($scope, dialogService, inventories) {

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
        });
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "Inventory has been modified. Do you want to save changes?",
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

    $scope.bladeToolbarCommands = [
        {
            name: "Save", icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty(); // && formScope && formScope.$valid;
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
    // datepicker
    //$scope.today = function () {
    //    $scope.dt = new Date();
    //};
    //$scope.today();

    //$scope.clear = function () {
    //    $scope.dt = null;
    //};
    //$scope.open = function ($event) {
    //    $event.preventDefault();
    //    $event.stopPropagation();

    //    $scope.opened = true;
    //};
    //$scope.dateOptions = {
    //    formatYear: 'yy',
    //    startingDay: 1
    //};
    $scope.datepickers = {
        pod: false,
        bod: false
    }
    $scope.formData = {};
    $scope.today = function () {
        $scope.formData.pod = new Date();

        // ***** Q1  *****
        $scope.formData.bod = new Date();
    };
    $scope.today();

    $scope.showWeeks = true;
    $scope.toggleWeeks = function () {
        $scope.showWeeks = !$scope.showWeeks;
    };

    $scope.clear = function () {
        $scope.dt = null;
    };

    // Disable weekend selection
    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    //$scope.toggleMin = function () {
    //    $scope.minDate = ($scope.minDate) ? null : new Date();
    //};
    //$scope.toggleMin();
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