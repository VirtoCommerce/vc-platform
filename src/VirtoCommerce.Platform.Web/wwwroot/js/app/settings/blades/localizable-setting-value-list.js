angular.module('platformWebApp')
    .controller('platformWebApp.localizableSettingValueListController', [
        '$scope',
        'platformWebApp.dialogService', 'platformWebApp.bladeNavigationService', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
        'platformWebApp.localizableSettingService',
        function ($scope, dialogService, bladeNavigationService, bladeUtils, uiGridHelper, localizableSettingService) {
            var blade = $scope.blade;
            var settingName = blade.settingName;
            blade.headIcon = 'fa fa-wrench';
            blade.title = `settings.${settingName}.title`;
            blade.subtitle = 'platform.blades.localizable-setting-value-list.subtitle';

            blade.searchText = '';
            $scope.languages = [];
            $scope.currentEntities = [];

            blade.refresh = function () {
                refreshBlade();
            };

            function refreshBlade(refreshParent) {
                localizableSettingService.getItemsAndLanguagesAsync(settingName).then(function (data) {
                    initializeBlade(data.items, data.languages, refreshParent);
                });
            }

            function initializeBlade(items, languages, refreshParent) {
                if (!items) {
                    return;
                }

                let id = 1;
                items.forEach(function (item) {
                    item.id = id;
                    id++;
                });

                $scope.originalEntities = items;
                $scope.currentEntities = angular.copy($scope.originalEntities);
                $scope.languages = languages;

                $scope.applySorting();

                $scope.currentEntities.forEach(function (item) {
                    $scope.languages.forEach(function (language) {
                        const localizedValue = _.find(item.localizedValues, function (x) {
                            return x.languageCode === language || (!x.languageCode && language === blade.defaultLanguage)
                        });
                        item[language] = localizedValue ? localizedValue.value : '';
                    });
                });

                blade.isLoading = false;

                // Update parent blade
                if (refreshParent && blade.parentRefresh) {
                    // Pass aliases to the parent blade for compatibility with the platform behavior
                    const aliases = _.pluck(items, 'alias');
                    const isLocalizable = true;
                    blade.parentRefresh(aliases, isLocalizable);
                }
            }

            $scope.applySorting = function () {
                // Order both current and original arrays to avoid issues with angular.equals (like 'miss-dirtying')
                sort($scope.currentEntities);
                sort($scope.originalEntities);
            };

            function sort(entities) {
                entities.sort(function (a, b) {
                    return a.alias.localeCompare(b.alias);
                })

                if (blade.orderDesc) {
                    entities.reverse();
                }
            }

            $scope.filteredEntities = function () {
                const lowerCasedSearchText = blade.searchText.toLowerCase();
                return $scope.currentEntities ? _.filter($scope.currentEntities, function (x) {
                    return !x.alias || x.alias.toLowerCase().includes(lowerCasedSearchText);
                }) : [];
            };

            $scope.setGridOptions = function (gridOptions) {
                uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                    if (gridApi && gridApi.core) {
                        uiGridHelper.bindRefreshOnSortChanged($scope);
                    }
                });
                bladeUtils.initializePagination($scope);
            };

            blade.toolbarCommands = [
                {
                    name: 'platform.commands.add', icon: 'fas fa-plus',
                    executeMethod: function () {
                        $scope.selectItem({
                            id: $scope.originalEntities.length + 1,
                            alias: '',
                            localizedValues: []
                        })
                    },
                    canExecuteMethod: function () {
                        return true;
                    }
                },
                {
                    name: 'platform.commands.delete', icon: 'fas fa-trash-alt',
                    executeMethod: function () {
                        $scope.deleteItems($scope.gridApi.selection.getSelectedRows());
                    },
                    canExecuteMethod: function () {
                        return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
                    },
                }
            ];

            $scope.selectItem = function (selectedItem) {
                if (selectedItem.alias) {
                    $scope.selectedNodeId = selectedItem.alias;
                }
                const newBlade = {
                    id: 'propertyDictionaryDetails',
                    controller: 'platformWebApp.localizableSettingValueDetailsController',
                    template: '$(Platform)/Scripts/app/settings/blades/localizable-setting-value-details.html',
                    dictionaryItem: selectedItem,
                    allItems: $scope.currentEntities,
                    languages: $scope.languages,
                    settingName: settingName,
                    onSaveChanges: onItemsChanged,
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.deleteItem = function (selectedItem) {
                $scope.deleteItems([selectedItem]);
            }

            $scope.deleteItems = function (selectedItems) {
                const dialog = {
                    id: 'confirmDeleteLocalizableSettingValue',
                    title: 'platform.dialogs.localizable-setting-value-delete.title',
                    message: 'platform.dialogs.localizable-setting-value-delete.message',
                    callback: function (remove) {
                        if (remove) {
                            bladeNavigationService.closeChildrenBlades(blade, function () {
                                const aliases = _.pluck(selectedItems, 'alias');
                                localizableSettingService.deleteItemsAsync(settingName, aliases).then(onItemsChanged);
                            });
                        }
                    }
                };
                dialogService.showConfirmationDialog(dialog);
            };

            function onItemsChanged() {
                refreshBlade(true);
            }
        }]);
