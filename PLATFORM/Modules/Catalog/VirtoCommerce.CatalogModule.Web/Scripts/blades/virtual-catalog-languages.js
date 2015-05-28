angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.virtualcatalogLanguagesController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, settings, dialogService) {
    var promise = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }).$promise;

    function initializeBlade(data) {
        $scope.blade.currentEntity = data.defaultLanguage.languageCode;

        promise.then(function (promiseData) {
            promiseData = _.map(promiseData, function (x) { return { languageCode: x }; });

            var defaultLang = _.find(promiseData, function (x) { return x.languageCode.toLowerCase() === $scope.blade.currentEntity.toLowerCase(); });
            if (defaultLang) {
                $scope.blade.currentEntity = defaultLang.languageCode;
            }

            $scope.blade.currentEntities = angular.copy(promiseData);
            $scope.blade.isLoading = false;
        });
    };

    $scope.blade.onClose = function (closeCallback) {
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
        return !angular.equals($scope.blade.currentEntity, $scope.blade.data.defaultLanguage.languageCode);
    };

    $scope.cancelChanges = function () {
        $scope.blade.currentEntity = $scope.blade.data.defaultLanguage.languageCode;
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return true;
    }

    $scope.saveChanges = function () {
        $scope.blade.data.languages[0].languageCode = $scope.blade.currentEntity;
        $scope.blade.data.defaultLanguage.languageCode = $scope.blade.currentEntity;

        $scope.bladeClose();
    };

    $scope.blade.headIcon = 'fa fa-archive';

    $scope.$watch('blade.parentBlade.currentEntity', function (currentEntity) {
        $scope.blade.data = currentEntity;
        initializeBlade($scope.blade.data);
    });

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);