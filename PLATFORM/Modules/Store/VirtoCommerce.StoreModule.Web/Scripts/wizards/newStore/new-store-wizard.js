angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.newStoreWizardController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.storeModule.stores', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.settings', 'platformWebApp.dialogService', 'virtoCommerce.coreModule.currency.currencyUtils',
    function ($scope, bladeNavigationService, stores, catalogs, settings, dialogService, currencyUtils) {
        var blade = $scope.blade;

        function initializeBlade(data) {
            blade.currentEntityId = data.id;

            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;
        };

        $scope.saveChanges = function () {
            blade.isLoading = true;
            blade.currentEntity.languages = [blade.currentEntity.defaultLanguage];
            blade.currentEntity.currencies = [blade.currentEntity.defaultCurrency];

            stores.save(blade.currentEntity, function (data) {
                blade.parentBlade.refresh();
                blade.parentBlade.selectNode(data);
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        };

        $scope.setForm = function (form) {
            $scope.formScope = form;
        }

        $scope.openLanguagesDictionarySettingManagement = function () {
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

        $scope.openStatesDictionarySettingManagement = function () {
            var newBlade = {
                id: 'settingDetailChild',
                isApiSave: true,
                currentEntityId: 'Stores.States',
                parentRefresh: function (data) { $scope.storeStates = data; },
                controller: 'platformWebApp.settingDictionaryController',
                template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        $scope.catalogs = catalogs.getCatalogs();
        $scope.languages = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' });
        $scope.currencyUtils = currencyUtils;
        $scope.storeStates = settings.getValues({ id: 'Stores.States' });
        initializeBlade(blade.currentEntity);
    }]);