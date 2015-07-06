angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogLanguagesController', ['$scope', 'platformWebApp.settings', 'platformWebApp.dialogService', function ($scope, settings, dialogService) {
    var blade = $scope.blade;
    var promise = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }).$promise;
    $scope.languages = [];

    function initializeBlade(data) {
        blade.data = data;

        promise.then(function (promiseData) {
            $scope.languages = promiseData;

            var defaultValue = _.find(promiseData, function (x) { return x.toLowerCase() === data.defaultLanguage.languageCode.toLowerCase(); });
            var languages = _.pluck(data.languages, 'languageCode');

            var newModel = {
                defaultValue: defaultValue,
                selectedValues: _.without(languages, defaultValue)
            };

            blade.origEntity = newModel;
            blade.currentEntity = angular.copy(newModel);
            blade.isLoading = false;
        });
    }

    blade.onClose = function (closeCallback) {
        if (isDirty()) {
            var dialog = {
                id: "confirmItemChange",
                title: "Save changes",
                message: "The Languages has been modified. Do you want to save changes?"
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
        var selectedValues = blade.data.virtual ? [] : _.map(blade.currentEntity.selectedValues, function (x) { return { languageCode: x }; });
        var defaultValue = _.find(selectedValues, function (x) { return x.languageCode.toLowerCase() === blade.currentEntity.defaultValue.toLowerCase(); });
        if (defaultValue) {
            defaultValue.isDefault = true;
        } else {
            defaultValue = {
                languageCode: blade.currentEntity.defaultValue,
                isDefault: true
            };
            selectedValues.push(defaultValue);
        }

        blade.data.defaultLanguage = defaultValue;
        blade.data.languages = selectedValues;

        angular.copy(blade.currentEntity, blade.origEntity);
        $scope.bladeClose();
    };

    blade.headIcon = 'fa-language';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);