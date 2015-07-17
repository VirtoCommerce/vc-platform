angular.module('platformWebApp')
.directive('vaDynamicProperty', ['$compile', '$templateCache', '$http', 'platformWebApp.dynamicProperties.dictionaryItemsApi', function ($compile, $templateCache, $http, dictionaryItemsApi) {
    return {
        restrict: 'E',
        require: 'ngModel',
        replace: true,
        transclude: true,
        templateUrl: 'Scripts/app/dynamicProperties/directives/dynamicProperty.tpl.html',
        scope: { getPropValues: "&" },
        link: function (scope, element, attr, ngModelController, linker) {
            scope.currentEntity = ngModelController.$modelValue;
            var property = scope.currentEntity.property;

            scope.context = {};
            scope.context.currentPropValues = [];
            scope.context.allDictionaryValues = [];

            scope.$watch('context.currentPropValues', function (newValue) {
                //reflect only real changes
                if (newValue.length != scope.currentEntity.values.length || difference(newValue).length > 0) {
                    if (property.isMultilingual || property.isDictionary) {
                        scope.currentEntity.values = angular.copy(newValue);
                    } else {
                        //Prevent reflect changing when use null value for empty initial values
                        if (!(scope.currentEntity.values.length == 0 && newValue[0].value == null)) {
                            scope.currentEntity.values = _.pluck(newValue, 'value');
                            if (property.valueType === 'DateTime') { // fix for nice displaying on repeated blade open without saving.
                                scope.currentEntity.values = _.map(scope.currentEntity.values, function (x) { return x.toISOString(); });
                            }
                        }
                    }
                    ngModelController.$setViewValue(scope.currentEntity);
                }
            }, true);


            ngModelController.$render = function () {
                scope.currentEntity = ngModelController.$modelValue;
                property = scope.currentEntity.property;

                if (property.isMultilingual) {
                    scope.context.currentPropValues = angular.copy(scope.currentEntity.values);
                } else {
                    scope.context.currentPropValues = _.map(scope.currentEntity.values, function (x) { return { value: x } });
                }

                addEmptyValueIfNeeded();

                if (property.isDictionary) {
                    loadDictionaryValues();
                }

                chageValueTemplate();
            };

            var difference = function (one) {
                return _.filter(one, function (value) { return _.all(scope.currentEntity.values, function (x) { return x !== value.value; }); });
            }

            //need add empty value for single value type
            function addEmptyValueIfNeeded() {
                if (!property.isArray && !property.isDictionary && scope.context.currentPropValues.length == 0) {
                    scope.context.currentPropValues.push({});
                }
            }

            function loadDictionaryValues() {
                dictionaryItemsApi.query({ id: property.objectType, propertyId: property.id }, function (result) {
                    //blade.origEntity = data;
                    //blade.currentEntities = angular.copy(data);

                    scope.context.allDictionaryValues = [];
                    scope.context.currentPropValues = [];

                    angular.forEach(result, function (dictValue) {
                        dictValue.value = dictValue.name;
                        //Need select already selected values.
                        //Dictionary values are of same type like standard values
                        dictValue.selected = _.any(scope.currentEntity.values, function (x) { return x.name == dictValue.name; });
                        scope.context.allDictionaryValues.push(dictValue);
                        if (dictValue.selected) {
                            //add selected value
                            scope.context.currentPropValues.push(dictValue);
                        }
                    });

                    if (property.isMultilingual) {
                        // initLanguagesValuesMap();
                    }
                }, function (error) { });
            }

            function getTemplateName() {
                var result = 'd' + property.valueType;

                if (property.isDictionary) {
                    result += '-dictionary';
                }
                if (property.isArray) {
                    result += '-multivalue';
                }
                if (property.isMultilingual) {
                    result += '-multilang';
                }
                result += '.html';
                return result;
            }

            function chageValueTemplate() {
                var templateName = getTemplateName();

                //load input template and display
                $http.get(templateName, { cache: $templateCache }).success(function (tplContent) {
                    //Need to add ngForm to isolate form validation into sub form
                    //var innerContainer = "<div id='innerContainer' />";

                    //We must destroy scope of elements we are removing from DOM to avoid angular firing events
                    var el = element.find('#valuePlaceHolder #innerContainer');
                    if (el.length > 0) {
                        el.scope().$destroy();
                    }
                    var container = element.find('#valuePlaceHolder');
                    var result = container.html(tplContent.trim());

                    //Create new scope, otherwise we would destroy our directive scope
                    var newScope = scope.$new();
                    $compile(result)(newScope);
                });
            }

            // datepicker
            scope.datepickers = {}
            scope.open = function ($event, which) {
                $event.preventDefault();
                $event.stopPropagation();
                scope.datepickers[which] = true;
            };

            linker(function (clone) {
                element.append(clone);
            });
        }
    }
}]);