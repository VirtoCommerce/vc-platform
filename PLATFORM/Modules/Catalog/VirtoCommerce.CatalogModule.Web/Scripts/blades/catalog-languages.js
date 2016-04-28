angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.catalogLanguagesController', ['$scope', 'platformWebApp.settings', 'platformWebApp.bladeNavigationService', function ($scope, settings, bladeNavigationService) {
    var blade = $scope.blade;
    blade.updatePermission = 'catalog:update';
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
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), true, blade, $scope.saveChanges, closeCallback, "catalog.dialogs.language-save.title", "catalog.dialogs.language-save.message");
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    };

    $scope.cancelChanges = function () {
        blade.currentEntity = blade.origEntity;
        $scope.bladeClose();
    }

    $scope.saveChanges = function () {
        if (!blade.hasUpdatePermission()) return;

        var selectedValues = blade.data.isVirtual ? [] : _.map(blade.currentEntity.selectedValues, function (x) { return { languageCode: x }; });
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

    $scope.openDictionarySettingManagement = function () {
        var newBlade = {
            id: 'settingDetailChild',
            isApiSave: true,
            currentEntityId: 'VirtoCommerce.Core.General.Languages',
            parentRefresh: function (data) { $scope.languages = data; },
            controller: 'platformWebApp.settingDictionaryController',
            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);