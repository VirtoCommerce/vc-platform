angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.storeModule.stores', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.settings', 'platformWebApp.settings.helper', 'platformWebApp.dialogService', 'platformWebApp.authService',
    function ($scope, bladeNavigationService, stores, catalogs, settings, settingsHelper, dialogService, authService) {
        var blade = $scope.blade;

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

            data.shippingMethods.sort(function (a, b) { return a.priority > b.priority; });
            data.paymentMethods.sort(function (a, b) { return a.priority > b.priority; });

            _.each(data.shippingMethods, function (x) { settingsHelper.fixValues(x.settings); })
            _.each(data.paymentMethods, function (x) { settingsHelper.fixValues(x.settings); })
            _.each(data.taxProviders, function (x) { settingsHelper.fixValues(x.settings); })

            blade.currentEntity = angular.copy(data);
            blade.origEntity = data;
            blade.isLoading = false;

            //sets security scopes for scope bounded ACL
            if (blade.currentEntity && blade.currentEntity.securityScopes && angular.isArray(blade.currentEntity.securityScopes)) {
                blade.securityScopes = blade.currentEntity.securityScopes;
            }
        };

        function isDirty() {
            return authService.checkPermission('store:update', blade.securityScopes) && !angular.equals(blade.currentEntity, blade.origEntity);
        };

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

        $scope.setForm = function (form) {
            $scope.formScope = form;
        }

        blade.onClose = function (closeCallback) {
            closeChildrenBlades();
            if (isDirty()) {
                var dialog = {
                    id: "confirmCurrentBladeClose",
                    title: "stores.dialogs.store-save.title",
                    message: "stores.dialogs.store-save.message"
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

        function closeChildrenBlades() {
            angular.forEach(blade.childrenBlades.slice(), function (child) {
                bladeNavigationService.closeBlade(child);
            });
        }

        blade.headIcon = 'fa-archive';

        blade.toolbarCommands = [
            {
                name: "platform.commands.save",
                icon: 'fa fa-save',
                executeMethod: function () {
                    $scope.saveChanges();
                },
                canExecuteMethod: function () {
                    return isDirty() && $scope.formScope && $scope.formScope.$valid;
                },
                permission: 'store:update'
            },
            {
                name: "platform.commands.reset",
                icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origEntity, blade.currentEntity);
                },
                canExecuteMethod: function () {
                    return isDirty();
                },
                permission: 'store:update'
            },
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: function () {
                    deleteEntry();
                },
                canExecuteMethod: function () {
                    return !isDirty();
                },
                permission: 'store:delete'
            }
        ];


        blade.refresh(false);
        $scope.catalogs = catalogs.getCatalogs();
        $scope.storeStates = settings.getValues({ id: 'Stores.States' });
        $scope.languages = settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' });
        $scope.currencies = settings.getValues({ id: 'VirtoCommerce.Core.General.Currencies' });
    }]);