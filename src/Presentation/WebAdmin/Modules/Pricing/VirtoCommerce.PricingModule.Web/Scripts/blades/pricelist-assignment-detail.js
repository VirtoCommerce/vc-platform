angular.module('virtoCommerce.pricingModule')
.controller('pricelistAssignmentDetailController', ['$scope', 'catalogs', 'dialogService', function ($scope, catalogs, dialogService) {

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

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    //$scope.isValid = function () {
    //    return $scope.formScope && $scope.formScope.$valid;
    //}

    $scope.saveChanges = function () {
        angular.copy($scope.blade.currentEntity, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The catalog assignment has been modified. Do you want to save changes?",
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

    $scope.bladeHeadIco = 'fa fa-usd';

    $scope.bladeToolbarCommands = [
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
    $scope.datepickers = {
        str: false,
        end: false
    }

    $scope.showWeeks = true;
    $scope.toggleWeeks = function () {
        $scope.showWeeks = !$scope.showWeeks;
    };

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
    $scope.catalogs = catalogs.query();
    initializeBlade($scope.blade.origEntity);
}]);