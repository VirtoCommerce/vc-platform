angular.module('platformWebApp')
    .controller('platformWebApp.propertyValueListController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.settings', 'platformWebApp.dynamicProperties.dictionaryItemsApi', 'platformWebApp.i18n', '$timeout', 'platformWebApp.dynamicProperties.api', function ($scope, bladeNavigationService, dialogService, settings, dictionaryItemsApi, i18n, $timeout, dynamicPropertiesApi) {
        var blade = $scope.blade;
        blade.updatePermission = 'platform:dynamic_properties:update';
        blade.headIcon = 'far fa-plus-square';
        blade.title = "platform.blades.propertyValue-list.title";
        blade.currentLanguage = i18n.getLanguage();

        blade.propertiesVisible = true;
        blade.propertyVisibleCommand = {
            name: 'catalog.blades.property-list.labels.hide-empty-values', icon: 'fas fa-eye-slash',
            executeMethod: function () {
                $scope.switchPropertiesVisibility();
            },
            canExecuteMethod: function () {
                return true;
            }
        };

        blade.refresh = function () {
            blade.data = blade.currentEntity;
            blade.emptyProperties = [];

            if (blade.refreshWidgetCount) blade.refreshWidgetCount(blade.currentEntity.objectType);
            dynamicPropertiesApi.search({ objectType: blade.currentEntity.objectType, take: blade.dynamicPropertyCount },
                function (response) {
                    var rawProperties = response.results;
                    var dynamicProperties = angular.copy(blade.currentEntity.dynamicProperties);
                    _.each(response.results, function (prop) {
                        prop.values = [];
                        var filteredProperty = _.find(dynamicProperties, function (o) { return o.name === prop.name; });
                        if (filteredProperty) {
                            prop.values = filteredProperty.values;
                        }
                    })

                    _.each(rawProperties, function (x) {
                        x.values.sort(function (a, b) {
                            return a.value && b.value
                                ? (a.value.name
                                    ? a.value.name.localeCompare(b.value.name)
                                    : angular.isString(a.value) && angular.isString(b.value)
                                        ? a.value.localeCompare(b.value)
                                        : a.value < b.value ? -1 : a.value > b.value ? 1 : 0)
                                : -1;
                        });
                    });

                    if (_.any(rawProperties, function (x) { return x.isMultilingual; })) {
                        settings.getValues({ id: 'VirtoCommerce.Core.General.Languages' }, function (data) {
                            $scope.languages = data;

                            // wait for va-generic-value-input to initialize empty values and repeat init
                            $timeout(function () {
                                blade.origEntity = angular.copy(blade.currentEntities);
                            });
                        });
                    }

                    blade.origEntity = rawProperties;
                    blade.currentEntities = angular.copy(rawProperties);
                    blade.isLoading = false;
                });


        };

        function isDirty() {
            return !angular.equals(blade.currentEntities, blade.origEntity) && blade.hasUpdatePermission();
        }

        function canSave() {
            return isDirty() && formScope && formScope.$valid;
        }

        $scope.cancelChanges = function () {
            angular.copy(blade.origEntity, blade.currentEntities);
            $scope.bladeClose();
        };

        $scope.saveChanges = function () {
            if (isDirty()) {
                angular.copy(blade.currentEntities, blade.data.dynamicProperties);
                angular.copy(blade.currentEntities, blade.origEntity);
            }
            $scope.bladeClose();
        };

        var formScope;
        $scope.setForm = function (form) { formScope = form; }

        $scope.editDictionary = function (property) {
            var newBlade = {
                id: "propertyDictionary",
                isApiSave: true,
                currentEntity: property,
                controller: 'platformWebApp.propertyDictionaryController',
                template: '$(Platform)/Scripts/app/dynamicProperties/blades/property-dictionary.tpl.html',
                onChangesConfirmedFn: function () {
                    blade.currentEntities = angular.copy(blade.currentEntities);
                }
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        blade.onClose = function (closeCallback) {
            bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, $scope.saveChanges, closeCallback, "platform.dialogs.properties-save.title", "platform.dialogs.properties-save.message");
        };

        blade.toolbarCommands = [
            {
                name: "platform.commands.ok", icon: 'fas fa-check',
                executeMethod: $scope.saveChanges,
                canExecuteMethod: canSave
            },
            {
                name: "platform.commands.cancel", icon: 'fas fa-ban',
                executeMethod: $scope.cancelChanges,
                canExecuteMethod: () => true
            },
            {
                name: "platform.commands.reset", icon: 'fa fa-undo',
                executeMethod: function () {
                    angular.copy(blade.origEntity, blade.currentEntities);
                },
                canExecuteMethod: isDirty
            },
            {
                name: "platform.commands.manage-type-properties", icon: 'fa fa-edit',
                executeMethod: function () {
                    var newBlade = {
                        id: 'dynamicPropertyList',
                        objectType: blade.data.objectType,
                        controller: 'platformWebApp.dynamicPropertyListController',
                        template: '$(Platform)/Scripts/app/dynamicProperties/blades/dynamicProperty-list.tpl.html',
                        parentRefresh: blade.refresh
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                },
                canExecuteMethod: function () {
                    return angular.isDefined(blade.data.objectType);
                }
            },
            blade.propertyVisibleCommand
        ];

        $scope.getDictionaryValues = function (property, callback) {
            dictionaryItemsApi.query({ id: property.objectType, propertyId: property.id }, callback);
        }

        $scope.switchPropertiesVisibility = function () {
            blade.propertiesVisible = !blade.propertiesVisible;

            if (blade.propertiesVisible) {
                blade.propertyVisibleCommand.name = 'platform.commands.hide-empty-property-values';
                blade.propertyVisibleCommand.icon = 'fas fa-eye-slash';

                showEmptyProperties();
            }
            else {
                blade.propertyVisibleCommand.name = 'platform.commands.show-empty-property-values';
                blade.propertyVisibleCommand.icon = 'fas fa-eye';

                hideEmptyProperties();
            }
        };

        $scope.isPropertyHasValues = function (property) {
            return !blade.emptyProperties.includes(property);
        }

        function hideEmptyProperties() {
            // control visibility of multilanguage properties separately
            _.each(blade.currentEntities, function (property) {
                if (property.isMultilingual) {
                    property.$$hiddenLanguages = [];
                    _.each($scope.languages, function (language) {
                        var languageFound = _.some(property.values, function (propertyValue) {
                            return propertyValue.value && propertyValue.value !== '' && propertyValue.locale === language;
                        });

                        if (!languageFound) {
                            property.$$hiddenLanguages.push(language);
                        }
                    });
                }
            })

            _.each(blade.currentEntities, function (property) {
                // required properties and switchers can’t be hidden
                if (!property.isRequired &&
                    property.valueType !== 'Boolean' &&
                    allPropertiesEmpty(property.values)
                ) {
                    blade.emptyProperties.push(property)
                }
            });
        }

        function allPropertiesEmpty(propertyValues) {
            var result = _.all(propertyValues, function (value) {
                return !value.value || value.value === '';
            });
            return result;
        }

        function showEmptyProperties() {
            _.each(blade.currentEntities, function (property) {
                if (property.$$hiddenLanguages) {
                    property.$$hiddenLanguages = null;
                }
            });

            blade.emptyProperties = [];
        }

        blade.refresh();
    }]);
