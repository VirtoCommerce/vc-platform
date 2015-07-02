angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeLanguagesController', ['$scope', 'platformWebApp.settings', 'platformWebApp.dialogService', function ($scope, settings, dialogService) {
    var blade = $scope.blade;

    function initializeBlade(data) {
        blade.data = data;

        var newModel = {
            defaultValue: data.defaultLanguage,
            selectedValues: _.without(data.languages, data.defaultLanguage)
        };

        blade.origEntity = newModel;
        blade.currentEntity = angular.copy(newModel);
        blade.isLoading = false;
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
    }

    $scope.cancelChanges = function() {
        blade.currentEntity = blade.origEntity;
        $scope.bladeClose();
    };

    $scope.saveChanges = function () {
        blade.data.defaultLanguage = blade.currentEntity.defaultValue;
        blade.data.languages = _.union([blade.data.defaultLanguage], blade.currentEntity.selectedValues);

        angular.copy(blade.currentEntity, blade.origEntity);
        $scope.bladeClose();
    };

    blade.headIcon = 'fa-language';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired

    $scope.languages = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' });
}]);