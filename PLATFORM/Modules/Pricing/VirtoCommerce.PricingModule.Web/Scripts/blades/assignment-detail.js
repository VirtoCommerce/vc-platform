angular.module('virtoCommerce.pricingModule')
.controller('assignmentDetailController', ['$scope', 'catalogs', 'pricelists', 'pricelistAssignments', 'dialogService', 'bladeNavigationService', function ($scope, catalogs, pricelists, assignments, dialogService, bladeNavigationService) {
    var blade = $scope.blade;

    blade.refresh = function (parentRefresh) {
        if (blade.isNew) {
            initializeBlade({ priority: 1 });
        } else if (blade.isApiSave) {
            assignments.get({ id: blade.currentEntityId }, function (data) {
                initializeBlade(data);
            });
            if (parentRefresh) {
                blade.parentBlade.refresh();
            }
        } else {
            initializeBlade(blade.origEntity);
        }
    };

    function initializeBlade(data) {
        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;

        if (!$scope.blade.isNew) {
            $scope.bladeToolbarCommands = [
                {
                    name: "Save",
                    icon: 'fa fa-save',
                    executeMethod: function () {
                        $scope.saveChanges();
                    },
                    canExecuteMethod: function () {
                        return isDirty() && $scope.formScope && $scope.formScope.$valid;
                    }
                },
                {
                    name: "Reset",
                    icon: 'fa fa-undo',
                    executeMethod: function () {
                        angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                    },
                    canExecuteMethod: function () {
                        return isDirty();
                    }
                }
            ];

            if (!blade.isApiSave) {
                $scope.bladeToolbarCommands.splice(0, 1); // remove save button
            }
        }
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
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
        if (blade.isNew) {
            blade.isLoading = true;

            assignments.save({}, blade.currentEntity, function (data) {
                blade.isNew = undefined;
                blade.isApiSave = true;
                blade.currentEntityId = data.id;
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        } else if (blade.isApiSave) {
            blade.isLoading = true;

            assignments.update({}, blade.currentEntity, function (data) {
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        } else {
            angular.copy(blade.currentEntity, blade.origEntity);
            $scope.bladeClose();
        }
    };

    blade.onClose = function (closeCallback) {
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

    $scope.format = 'shortDate';


    // actions on load
    $scope.catalogs = catalogs.query();
    if (blade.isNew || blade.isApiSave) {
        $scope.pricelists = pricelists.query();
    }
    blade.refresh();
}]);