angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeLanguagesListController', ['$scope', 'platformWebApp.settings', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, settings, bladeNavigationService, dialogService) {
    var blade = $scope.blade;
    var promise = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }).$promise;

    function initializeBlade(data) {
        blade.data = data;
        promise.then(function (promiseData) {
            blade.defaultValue = data.defaultLanguage;

            // construct complex objects
            promiseData = _.map(promiseData, function (x) { return { code: x }; });

            _.each(promiseData, function (x) {
                x.isChecked = _.some(data.languages, function (curr) { return curr === x.code; });
            });

            blade.origEntity = promiseData;
            blade.currentEntities = angular.copy(promiseData);
            blade.isLoading = false;
        });
    };

    $scope.selectItem = function (listItem) {
        if (blade.defaultValue !== listItem.code) {
            listItem.isChecked = !listItem.isChecked;
            if (listItem.isChecked && !blade.defaultValue) {
                blade.defaultValue = listItem.code;
            }
        }
    };
    
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
        return !angular.equals(blade.currentEntities, blade.origEntity);
    };

    $scope.cancelChanges = function () {
        blade.currentEntities = blade.origEntity;
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return blade.defaultValue;
    }

    $scope.saveChanges = function () {
        var checkedEntities = _.where(blade.currentEntities, { isChecked: true });
        blade.data.languages = _.pluck(checkedEntities, 'code');
        blade.data.defaultLanguage = blade.defaultValue;

        angular.copy(blade.currentEntities, blade.origEntity);
        $scope.bladeClose();
    };

    blade.headIcon = 'fa fa-language';

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);