angular.module('platformWebApp')
    .controller('platformWebApp.localizableSettingValueDetailsController', [
        '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.localizableSettingService',
        function ($scope, bladeNavigationService, localizableSettingService) {
            var blade = $scope.blade;
            blade.headIcon = 'fa fa-wrench';
            blade.title = 'platform.blades.localizable-setting-value-details.title';

            $scope.isValid = true;
            $scope.blade.isLoading = false;

            $scope.validationRules = {
                maxLength: 512
            };

            $scope.setForm = function (form) {
                blade.formScope = form;
            };

            blade.toolbarCommands = [
                {
                    name: "platform.commands.save", icon: 'fas fa-save',
                    executeMethod: saveChanges,
                    canExecuteMethod: canSave
                }
            ];

            function initializeBlade() {
                blade.currentEntity = angular.copy(blade.dictionaryItem);
                blade.originalEntity = blade.dictionaryItem;
                blade.isLoading = false;
            }

            function isDirty() {
                return !angular.equals(blade.currentEntity, blade.originalEntity) && blade.hasUpdatePermission();
            }

            function canSave() {
                return isDirty() && blade.formScope && blade.formScope.$valid;
            }

            blade.onClose = function (closeCallback) {
                bladeNavigationService.showConfirmationIfNeeded(isDirty(), $scope.isValid, blade, $scope.saveChanges, closeCallback,
                    "platform.dialogs.localizable-setting-value-save.title",
                    "platform.dialogs.localizable-setting-value-save.message");
            };

            function saveChanges() {
                const items = [convertItem(blade.currentEntity)];

                blade.allItems.forEach(function (item) {
                    if (item.id !== blade.currentEntity.id) {
                        items.push(convertItem(item))
                    }
                });

                localizableSettingService.saveItemsAsync(blade.settingName, items).then(function () {
                    angular.copy(blade.currentEntity, blade.originalEntity);

                    // Update parent blade
                    if (blade.onSaveChanges) {
                        blade.onSaveChanges();
                    }
                });
            }

            function convertItem(item) {
                const result = {
                    alias: item.alias,
                    localizedValues: []
                };

                blade.languages.forEach(function (language) {
                    if (item[language]) {
                        const localizedValue = {
                            languageCode: language,
                            value: item[language]
                        };
                        result.localizedValues.push(localizedValue);
                    }
                });

                return result;
            }

            initializeBlade();
        }]);
