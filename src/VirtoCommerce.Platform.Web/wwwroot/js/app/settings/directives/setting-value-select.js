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
                required: '=?',
                allowClear: '=?',
                onSelect: '&?',
            },
            controller: [
                '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.localizableSettingsApi',
                function ($scope, bladeNavigationService, localizableSettingsApi) {
                    $scope.items = [];

                    $scope.openSettingManagement = function () {
                        const newBlade = {
                            id: 'settingDetailChild',
                            controller: 'platformWebApp.settingDictionaryController',
                            template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html',
                            currentEntityId: $scope.setting,
                            isApiSave: true,
                            parentRefresh: function () {
                                getItems();
                            },
                        };
                        bladeNavigationService.showBlade(newBlade, $scope.blade);
                    }

                    $scope.selectValue = (item, model) => {
                        if ($scope.onSelect) {
                            $scope.onSelect({ item: item, model: model });
                        }
                    }

                    function getItems() {
                        $scope.isLoading = true;

                        localizableSettingsApi.getValues({ name: $scope.setting }, function (response) {
                            $scope.isLoading = false;
                            $scope.items = response;
                            $scope.hasItems = $scope.items.length > 0;
                        });
                    }

                    getItems();
                }],
            link: function (scope, element, attrs, ngModelController) {
                scope.context = {
                    modelValue: null,
                    disabled: angular.isDefined(attrs.disabled) && (attrs.disabled === '' || attrs.disabled.toLowerCase() === 'true'),
                    required: angular.isDefined(attrs.required) && (attrs.required === '' || attrs.required.toLowerCase() === 'true'),
                    allowClear: angular.isDefined(attrs.allowClear) && (attrs.allowClear === '' || attrs.allowClear.toLowerCase() === 'true'),
                };

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
