angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeCurrenciesListController', ['$q', '$scope', 'platformWebApp.settings', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($q, $scope, settings, bladeNavigationService, dialogService) {
    $scope.selectedItem = null;

    //function asyncQueryWithCustomCode() {
    //    var d = $q.defer();
    //    settings.getValues({ id: 'VirtoCommerce.Core.General.Currencies' }, function (data) {
    //        // custom code goes here
    //        d.resolve(data);
    //    });
    //    return d.promise;
    //}
    var promise = settings.getValues({ id: 'VirtoCommerce.Core.General.Currencies' }).$promise;

    function initializeBlade(data) {
        promise.then(function (promiseData) {
            promiseData = _.map(promiseData, function (x) { return { code: x }; });
            $scope.blade.currentEntities = promiseData;
            _.each($scope.blade.currentEntities, function (x) {
                x.isChecked = _.some(data.currencies, function (curr) { return curr === x.code; });
            });
            if (data.defaultCurrency) {
                var defaultEntity = _.findWhere($scope.blade.currentEntities, { code: data.defaultCurrency });
                if (defaultEntity) {
                    defaultEntity.isDefault = true;
                }
            }

            $scope.blade.origEntity = $scope.blade.currentEntities;
            $scope.blade.currentEntities = angular.copy($scope.blade.currentEntities);
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
        return !angular.equals($scope.blade.currentEntities, $scope.blade.origEntity);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    }

    $scope.isValid = function () {
        return true;
    }

    $scope.saveChanges = function () {
        var checkedEntities = _.where($scope.blade.currentEntities, { isChecked: true });
        $scope.blade.data.currencies = _.pluck(checkedEntities, 'code');

        var defaultEntity = _.findWhere($scope.blade.currentEntities, { isDefault: true });
        if (defaultEntity) {
            $scope.blade.data.defaultCurrency = defaultEntity.code;
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
                    x.isDefault = x.code === $scope.selectedItem.code;
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