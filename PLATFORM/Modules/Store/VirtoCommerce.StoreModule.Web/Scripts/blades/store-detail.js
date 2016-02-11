angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.storeModule.stores', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.settings', 'platformWebApp.settings.helper', 'platformWebApp.dialogService', 'virtoCommerce.coreModule.currency.currencyUtils',
    function ($scope, bladeNavigationService, stores, catalogs, settings, settingsHelper, dialogService, currencyUtils) {
        var blade = $scope.blade;
        blade.updatePermission = 'store:update';

        blade.refresh = function (parentRefresh) {
            stores.get({ id: blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    blade.parentBlade.refresh();
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }

        function initializeBlade(data) {
            data.additionalLanguages = _.without(data.languages, data.defaultLanguage);
            data.additionalCurrencies = _.without(data.currencies, data.defaultCurrency);

            blade.currentEntityId = data.id;
            blade.title = data.name;

            data.shippingMethods.sort(function (a, b) { return a.priority - b.priority; });
            data.paymentMethods.sort(function (a, b) { return a.priority - b.priority; });

            _.each(data.shippingMethods, function (x) { settingsHelper.fixValues(x.settings); })
            _.each(data.paymentMethods, function (x) { settingsHelper.fixValues(x.settings); })
            _.each(data.taxProviders, function (x) { settingsHelper.fixValues(x.settings); })

            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;

            //sets security scopes for scope bounded ACL
            if (blade.currentEntity.securityScopes && angular.isArray(blade.currentEntity.securityScopes)) {
                blade.securityScopes = blade.currentEntity.securityScopes;
            }
        }

        function isDirty() {
            return blade.hasUpdatePermission() && !angular.equals(blade.currentEntity, blade.origEntity);
        }

        function canSave() {
            return isDirty() && $scope.formScope && $scope.formScope.$valid;
        }

        $scope.saveChanges = function () {
            blade.isLoading = true;

            blade.currentEntity.languages = _.union([blade.currentEntity.defaultLanguage], blade.currentEntity.additionalLanguages);
            blade.currentEntity.currencies = _.union([blade.currentEntity.defaultCurrency], blade.currentEntity.additionalCurrencies);

            stores.update({}, blade.currentEntity, function (data) {
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        };

        function deleteEntry() {
            var dialog = {
                id: "confirmDelete",
                title: "stores.dialogs.store-delete.title",
                message: "stores.dialogs.store-delete.message",
                callback: function (remove) {
                    if (remove) {
                        blade.isLoading = true;

                        stores.remove({ ids: blade.currentEntityId }, function () {
                            $scope.bladeClose();
                            blade.parentBlade.refresh();
                        }, function (error) {
                            bladeNavigationService.setError('Error ' + error.status, blade);
                        });
                    }
                }
            }
            dialogService.showConfirmationDialog(dialog);
        }

        $scope.setForm = function (form) { $scope.formScope = form; };

        blade.onClose = function (closeCallback) {
            bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "stores.dialogs.store-save.title", "stores.dialogs.store-save.message");
        };

        blade.headIcon = 'fa-archive';
        blade.toolbarCommands = [
            {
                name: "platform.commands.save",
                icon: 'fa fa-save',
                executeMethod: $scope.saveChanges,
                canExecuteMethod: canSave,
                permission: 'store:update'
            },
            {
                name: "platform.commands.reset",
                icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origEntity, blade.currentEntity);
                },
                canExecuteMethod: isDirty,
                permission: 'store:update'
            },
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: deleteEntry,
                canExecuteMethod: function () { return true; },
                permission: 'store:delete'
            }
        ];

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

        blade.refresh(false);
        $scope.catalogs = catalogs.getCatalogs();
        $scope.storeStates = settings.getValues({ id: 'Stores.States' });
        $scope.languages = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' });
        $scope.currencyUtils = currencyUtils;
    }]);