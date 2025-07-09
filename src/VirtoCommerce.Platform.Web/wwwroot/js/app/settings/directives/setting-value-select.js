angular.module('platformWebApp')
    .directive('vaSettingValueSelect', function () {
        return {
            restrict: 'E',
            templateUrl: '$(Platform)/Scripts/app/settings/directives/setting-value-select.html',
            replace: true,
            require: 'ngModel',
            scope: {
                setting: '=',
                ngModel: '=',
                blade: '=?',
                label: '=?',
                description: '=?',
                placeholder: '=?',
                disabled: '=?',
                multiple: '=?',
                required: '=?',
                allowClear: '=?',
                onSelect: '&?',
                onRemove: '&?',
            },
            controller: [
                '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.localizableSettingService',
                function ($scope, bladeNavigationService, localizableSettingService) {
                    $scope.openSettingManagement = function () {
                        const newBlade = {
                            id: 'settingDetailChild',
                            controller: 'platformWebApp.settingDictionaryController',
                            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html',
                            currentEntityId: $scope.setting,
                            isApiSave: true,
                            parentRefresh: function (allowedValues, isLocalizable) {
                                if (isLocalizable) {
                                    getItems();
                                }
                                else {
                                    localizableSettingService.getItemsAndLanguagesAsync($scope.setting).then(function () {
                                        getItems();
                                    });
                                }
                            },
                        };
                        bladeNavigationService.showBlade(newBlade, $scope.blade);
                    }

                    $scope.selectValue = function (item, model) {
                        if ($scope.onSelect) {
                            $scope.onSelect({ item: item, model: model });
                        }
                    }

                    $scope.removeValue = function (item, model) {
                        if ($scope.onRemove) {
                            $scope.onRemove({ item: item, model: model });
                        }
                    }

                    function getItems() {
                        $scope.items = localizableSettingService.getValues($scope.setting);
                        $scope.hasItems = $scope.items.length > 0;
                    }

                    getItems();
                }],
            link: function (scope, element, attrs, ngModelController) {
                scope.context = {
                    modelValue: null,
                    multiple:   getBooleanAttributeValue('multiple'),
                    required:   getBooleanAttributeValue('required'),
                    allowClear: getBooleanAttributeValue('allowClear'),
                };

                function getBooleanAttributeValue(name) {
                    const value = attrs[name];
                    return angular.isDefined(value) && (value === '' || value === name || value.toLowerCase() === 'true');
                }

                scope.$watch('context.modelValue', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        ngModelController.$setViewValue(scope.context.modelValue);
                    }
                });

                ngModelController.$render = function () {
                    scope.context.modelValue = ngModelController.$modelValue;
                };
            }
        }
    });
