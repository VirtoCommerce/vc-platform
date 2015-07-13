angular.module('platformWebApp')
.controller('platformWebApp.dynamicPropertyDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.settings', 'platformWebApp.dynamicProperties.api', function ($scope, bladeNavigationService, dialogService, settings, dynamicPropertiesApi) {
    var blade = $scope.blade;
    blade.headIcon = 'fa-plus-square-o';
    var promise = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }).$promise;

    function refresh() {
        if (blade.isNew) {
            initializeBlade({ valueType: 'ShortText', displayNames: [] });
        } else {
            initializeBlade(blade.origEntity);
        }
    }

    function initializeBlade(data) {
        promise.then(function (promiseData) {
            promiseData.sort();

            // add missing languages
            _.each(promiseData, function (x) {
                if (_.all(data.displayNames, function (dn) { return dn.locale.toLowerCase() !== x.toLowerCase(); })) {
                    data.displayNames.push({ locale: x });
                }
            });

            blade.origEntity = data;
            blade.currentEntity = angular.copy(data);
            blade.isLoading = false;
        });
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        blade.currentEntity.displayNames = _.filter(blade.currentEntity.displayNames, function (x) { return x.name; });

        blade.confirmChangesFn(blade.currentEntity);
        $scope.bladeClose();
    };

    function deleteEntry() {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete this dynamic property?",
            callback: function (remove) {
                if (remove) {
                    blade.deleteFn();
                    $scope.bladeClose();
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    if (!blade.isNew) {
        blade.toolbarCommands = [
            {
                name: "Save", icon: 'fa fa-save',
                executeMethod: function () {
                    $scope.saveChanges();
                },
                canExecuteMethod: function () {
                    return isDirty() && $scope.formScope && $scope.formScope.$valid;
                }
            },
            {
                name: "Reset", icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origEntity, blade.currentEntity);
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
                    return !isDirty() && !blade.isNew;
                }
            }
        ];
    }

    // on load: 
    refresh();
}]);