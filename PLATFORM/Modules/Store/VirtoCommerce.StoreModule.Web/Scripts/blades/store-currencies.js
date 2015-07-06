angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeCurrenciesController', ['$q', '$scope', 'platformWebApp.settings', 'platformWebApp.dialogService', function ($q, $scope, settings, dialogService) {
    var blade = $scope.blade;

    //function asyncQueryWithCustomCode() {
    //    var d = $q.defer();
    //    settings.getValues({ id: 'VirtoCommerce.Core.General.Currencies' }, function (data) {
    //        // custom code goes here
    //        d.resolve(data);
    //    });
    //    return d.promise;
    //}

    function initializeBlade(data) {
        blade.data = data;

        var newModel = {
            defaultValue: data.defaultCurrency,
            selectedValues: _.without(data.currencies, data.defaultCurrency)
        };

        blade.origEntity = newModel;
        blade.currentEntity = angular.copy(newModel);
        blade.isLoading = false;
    };

    blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The Currencies has been modified. Do you want to save changes?"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    $scope.saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    $scope.cancelChanges = function () {
        blade.currentEntity = blade.origEntity;
        $scope.bladeClose();
    }


    $scope.saveChanges = function () {
        blade.data.defaultCurrency = blade.currentEntity.defaultValue;
        blade.data.currencies = _.union([blade.data.defaultCurrency], blade.currentEntity.selectedValues);

        angular.copy(blade.currentEntity, blade.origEntity);
        $scope.bladeClose();
    };

    blade.headIcon = 'fa-money';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired

    $scope.currencies = settings.getValues({ id: 'VirtoCommerce.Core.General.Currencies' });
}]);