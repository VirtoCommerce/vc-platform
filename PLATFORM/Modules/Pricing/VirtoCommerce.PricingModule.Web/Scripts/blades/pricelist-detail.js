angular.module('virtoCommerce.pricingModule')
    .controller('virtoCommerce.pricingModule.pricelistDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.pricingModule.pricelists', 'platformWebApp.settings', 'platformWebApp.dialogService', 'virtoCommerce.coreModule.currency.currencyUtils',
        function ($scope, bladeNavigationService, pricelists, settings, dialogService, currencyUtils) {
            var blade = $scope.blade;

            blade.refresh = function (parentRefresh) {
                if (blade.isNew) {
                    initializeBlade({ productPrices: [], assignments: [] });
                } else {
                    pricelists.get({ id: blade.currentEntityId }, function (data) {
                        initializeBlade(data);
                        if (parentRefresh) {
                            blade.parentBlade.refresh();
                        }
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
                }
            };

            function initializeBlade(data) {
                if (!blade.isNew) {
                    blade.title = data.name;
                }

                blade.currentEntity = angular.copy(data);
                blade.origEntity = data;
                blade.isLoading = false;
            };

            function isDirty() {
                return !angular.equals(blade.currentEntity, blade.origEntity);
            };

            $scope.cancelChanges = function () {
                angular.copy(blade.origEntity, blade.currentEntity);
                $scope.bladeClose();
            };
            $scope.saveChanges = function () {
                blade.isLoading = true;

                if (blade.isNew) {
                    pricelists.save({}, blade.currentEntity, function (data) {
                        blade.isNew = undefined;
                        blade.currentEntityId = data.id;
                        initializeBlade(data);
                        initializeToolbar();
                        blade.parentBlade.refresh();
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                    });
                } else {
                    pricelists.update({}, blade.currentEntity, function (data) {
                        blade.refresh(true);
                    }, function (error) {
                        bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                    });
                }
            };

            $scope.setForm = function (form) {
                $scope.formScope = form;
            };

            blade.onClose = function (closeCallback) {
                closeChildrenBlades();
                if (isDirty()) {
                    var dialog = {
                        id: "confirmCurrentBladeClose",
                        title: "pricing.dialogs.pricelist-save.title",
                        message: "pricing.dialogs.pricelist-save.message"
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
                angular.forEach(blade.childrenBlades.slice(), function (child) {
                    bladeNavigationService.closeBlade(child);
                });
            }

            blade.headIcon = blade.parentBlade.headIcon;

            function initializeToolbar() {
                if (!blade.isNew) {
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
                            permission: 'pricing:update'
                        },
                        {
                            name: "platform.commands.reset",
                            icon: 'fa fa-undo',
                            executeMethod: function () {
                                angular.copy(blade.origEntity, blade.currentEntity);
                            },
                            canExecuteMethod: isDirty,
                            permission: 'pricing:update'
                        }
                    ];
                }
            }

            initializeToolbar();
            blade.refresh(false);
            $scope.currencyUtils = currencyUtils;
        }]);