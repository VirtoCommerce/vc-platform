angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogLanguagesController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, settings, dialogService) {
    $scope.selectedItem = null;
    var promise = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }).$promise;

    function initializeBlade(data) {
        promise.then(function (promiseData) {
            promiseData = _.map(promiseData, function (x) { return { languageCode: x }; });

            _.each(promiseData, function (x) {
                x.isChecked = _.some(data.languages, function (curr) { return curr.languageCode.toLowerCase() === x.languageCode.toLowerCase(); });
            });
            if (data.defaultLanguage) {
                var defaultLang = _.find(promiseData, function (x) { return x.languageCode.toLowerCase() === data.defaultLanguage.languageCode.toLowerCase(); });
                if (defaultLang) {
                    defaultLang.isDefault = true;
                }
            }

            $scope.blade.currentEntities = angular.copy(promiseData);
            $scope.blade.origEntity = promiseData;
            $scope.blade.isLoading = false;
        });
    };
    
    $scope.selectItem = function (listItem) {
        $scope.selectedItem = listItem;
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
        return !angular.equals($scope.blade.currentEntities, $scope.blade.origEntity);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return true;
    }

    $scope.saveChanges = function () {
        $scope.blade.data.languages = _.where($scope.blade.currentEntities, { isChecked: true });

        var defaultLang = _.findWhere($scope.blade.currentEntities, { isDefault: true });
        if (defaultLang) {
            $scope.blade.data.defaultLanguage = defaultLang;
        }

        angular.copy($scope.blade.currentEntities, $scope.blade.origEntity);
        $scope.bladeClose();
    };

    $scope.bladeHeadIco = 'fa fa-archive';

    $scope.bladeToolbarCommands = [
        {
            name: "Set default", icon: 'fa fa-edit',
            executeMethod: function () {
                _.each($scope.blade.currentEntities, function (x) {
                    x.isDefault = x.languageCode === $scope.selectedItem.languageCode;
                });
            },
            canExecuteMethod: function () {
                return $scope.selectedItem && $scope.selectedItem.isChecked;
            }
        }
    ];

    $scope.$watch('blade.parentBlade.currentEntity', function (currentEntity) {
        $scope.blade.data = currentEntity;
        initializeBlade($scope.blade.data);
    });

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);