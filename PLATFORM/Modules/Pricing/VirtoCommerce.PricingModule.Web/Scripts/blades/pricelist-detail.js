angular.module('virtoCommerce.pricingModule')
    .controller('virtoCommerce.pricingModule.pricelistDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.pricingModule.pricelists', 'platformWebApp.settings', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, pricelists, settings, dialogService) {
        $scope.blade.refresh = function (parentRefresh) {
            if ($scope.blade.isNew) {
                initializeBlade({ productPrices: [], assignments: [] });
            } else {
                pricelists.get({ id: $scope.blade.currentEntityId }, function (data) {
                    initializeBlade(data);
                    if (parentRefresh) {
                        $scope.blade.parentBlade.refresh();
                    }
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
            }
        };

        function initializeBlade(data) {
            if (!$scope.blade.isNew) {
                $scope.blade.title = data.name;
            }

            $scope.blade.currentEntity = angular.copy(data);
            $scope.blade.origEntity = data;
            $scope.blade.isLoading = false;
        };

        function isDirty() {
            return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
        };

        $scope.cancelChanges = function () {
            angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
            $scope.bladeClose();
        };
        $scope.saveChanges = function () {
            $scope.blade.isLoading = true;

            if ($scope.blade.isNew) {
                pricelists.save({}, $scope.blade.currentEntity, function (data) {
                    $scope.blade.isNew = undefined;
                    $scope.blade.currentEntityId = data.id;
                    initializeBlade(data);
                    initializeToolbar();
                    $scope.blade.parentBlade.refresh();
                }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                });
            } else {
                pricelists.update({}, $scope.blade.currentEntity, function (data) {
                    $scope.blade.refresh(true);
                }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                });
            }
        };

        $scope.setForm = function (form) {
            $scope.formScope = form;
        };

        $scope.blade.onClose = function (closeCallback) {
            closeChildrenBlades();
            if (isDirty()) {
                var dialog = {
                    id: "confirmCurrentBladeClose",
                    title: "prising.dialogs.pricelist-save.title",
                    message: "prising.dialogs.pricelist-save.message"
                };
                dialog.callback = function (needSave) {
                    if (needSave) {
                        $scope.saveChanges();
                    }
                    closeCallback();
                };
                dialogService.showConfirmationDialog(dialog);
            } else {
                closeCallback();
            }
        };

        function closeChildrenBlades() {
            angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
                bladeNavigationService.closeBlade(child);
            });
        }

        $scope.blade.headIcon = 'fa-anchor';

        function initializeToolbar() {
            if (!$scope.blade.isNew) {
                $scope.blade.toolbarCommands = [
                    {
                        name: "platform.commands.save",
                        icon: 'fa fa-save',
                        executeMethod: function () {
                            $scope.saveChanges();
                        },
                        canExecuteMethod: function () {
                            return isDirty() && $scope.formScope && $scope.formScope.$valid;
                        },
                        permission: 'pricing:update'
                    },
                    {
                        name: "platform.commands.reset",
                        icon: 'fa fa-undo',
                        executeMethod: function () {
                            angular.copy($scope.blade.origEntity, $scope.blade.currentEntity);
                        },
                        canExecuteMethod: function () {
                            return isDirty();
                        },
                        permission: 'pricing:update'
                    }
                ];
            }
        }

        initializeToolbar();
        $scope.blade.refresh(false);
        $scope.currencies = settings.getValues({ id: 'VirtoCommerce.Core.General.Currencies' });
    }]);