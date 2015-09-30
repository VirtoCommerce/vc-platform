angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.storeModule.stores', 'virtoCommerce.catalogModule.catalogs', 'platformWebApp.settings', 'platformWebApp.dialogService', 'platformWebApp.authService', function ($scope, bladeNavigationService, stores, catalogs, settings, dialogService, authService) {
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

        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;

    	//sets security scopes for scope bounded ACL
        blade.securityScopes = 'store:' + blade.currentEntity.name;
    };

    function isDirty() {
    	return  authService.checkPermission('store:update', blade.securityScopes) && !angular.equals(blade.currentEntity, blade.origEntity);
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
            title: "Delete confirmation",
            message: "Are you sure you want to delete this Store?",
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
                title: "Save changes",
                message: "The Store has been modified. Do you want to save changes?"
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
            name: "Save",
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
            name: "Reset",
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
            name: "Delete", icon: 'fa fa-trash-o',
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