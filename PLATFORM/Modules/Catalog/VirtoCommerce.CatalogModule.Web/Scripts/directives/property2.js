angular.module('virtoCommerce.catalogModule')
.directive('vaProperty2', ['$compile', '$filter', '$parse', '$templateCache', '$http', function ($compile, $filter, $parse, $templateCache, $http) {
   
    return {
        restrict: 'E',
        require: 'ngModel',
        replace: true,
        transclude: true,
        templateUrl: 'Modules/$(VirtoCommerce.Catalog)/Scripts/directives/property2.tpl.html',
        scope: { getPropValues: "&" },
        link: function (scope, element, attr, ngModelController, linker) {

            scope.currentEntity = ngModelController.$modelValue;

            scope.context = {};
            scope.context.currentPropValues = [];
            scope.context.allDictionaryValues = [];
            scope.context.langValuesMap = {};


            scope.$watch('context.langValuesMap', function (newValue, oldValue) {
                if (newValue != oldValue) {
                    scope.context.currentPropValues = [];
                    angular.forEach(scope.context.langValuesMap, function (langGroup, languageCode) {

                        angular.forEach(langGroup.currentPropValues, function (propValue) {
                            propValue.languageCode = languageCode;
                            scope.context.currentPropValues.push(propValue);
                        })
                    });
                }
            }, true);

            scope.$watch('context.currentPropValues', function (newValue, oldValue) {
                //reflect only real changes
                if (newValue.length != scope.currentEntity.values.length || difference(newValue, scope.currentEntity.values).length > 0) {
                    //Prevent reflect changing when use null value for empty initial values
                    if (!(scope.currentEntity.values.length == 0 && newValue[0].value == null)) {
                        scope.currentEntity.values = newValue;
                        ngModelController.$setViewValue(scope.currentEntity);
                    }
                }
            }, true);


            ngModelController.$render = function () {
                scope.currentEntity = ngModelController.$modelValue;

                scope.context.currentPropValues = angular.copy(scope.currentEntity.values);
                if (needAddEmptyValue(scope.currentEntity, scope.context.currentPropValues)) {
                    scope.context.currentPropValues.push({ value: null });
                }

                if (scope.currentEntity.dictionary) {
                    loadDictionaryValues(scope.currentEntity);
                }
                if (scope.currentEntity.multilanguage) {
                    initLanguagesValuesMap(scope.currentEntity);
                }

                chageValueTemplate(scope.currentEntity.valueType);
            };

            var difference = function (one, two) {
                var containsEquals = function (obj, target) {
                    if (obj == null) return false;
                    return _.any(obj, function (value) {
                        return value.value == target.value
                    });
                };
                return _.filter(one, function (value) { return !containsEquals(two, value); });
            };

            function needAddEmptyValue(property, values) {
                return !property.multivalue && !property.dictionary && values.length == 0;
            };

            function initLanguagesValuesMap(property) {

                //Group values by language 
                angular.forEach(property.catalog.languages, function (language) {
                    //Currently select values
                    var currentPropValues = _.where(scope.context.currentPropValues, { languageCode: language.languageCode });
                    //need add empty value for single  value type
                    if (needAddEmptyValue(property, currentPropValues)) {
                        currentPropValues.push({ value: null, languageCode: language.languageCode });
                    }
                    //All possible dict values
                    var allValues = _.where(scope.context.allDictionaryValues, { languageCode: language.languageCode });

                    var langValuesGroup = {
                        allValues: allValues,
                        currentPropValues: currentPropValues
                    };
                    scope.context.langValuesMap[language.languageCode] = langValuesGroup;
                });
            };


            function loadDictionaryValues(property) {
                var selectedValues = property.values;
                scope.getPropValues()(property.id).then(function (result) {
                    scope.context.allDictionaryValues = [];
                    scope.context.currentPropValues = [];

                    angular.forEach(result, function (dictValue) {
                        //Need select already selected value
                        //Dictionary values it a same type like a standart values
                        dictValue.selected = angular.isDefined(_.find(selectedValues, function (value) { return value.valueId == dictValue.valueId }));
                        scope.context.allDictionaryValues.push(dictValue);
                        if (dictValue.selected) {
                            //add selected value
                            scope.context.currentPropValues.push(dictValue);
                        }

                    });

                    if (property.multilanguage) {
                        initLanguagesValuesMap(scope.currentEntity);
                    }

                    return result;
                });

            };

            function getTemplateName(property) {
            	var result = property.valueType;
              
                if (property.dictionary) {
                    result += '-dictionary';
                }
                if (property.multivalue) {
                    result += '-multivalue';
                }
                if (property.multilanguage) {
                    result += '-multilang';
                }
                result += '.html';
                return result;
            };

            function chageValueTemplate(valueType) {
                var templateName = getTemplateName(scope.currentEntity);


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
            };

            /* Datepicker */
            scope.datepickers = {
                DateTime: false
            }

            scope.showWeeks = true;
            scope.toggleWeeks = function () {
                scope.showWeeks = !scope.showWeeks;
            };
            
            scope.clear = function () {
                scope.currentEntity.valueType = null;
            };
            scope.today = new Date();

            scope.open = function($event, which) {
                $event.preventDefault();
                $event.stopPropagation();

                scope.datepickers[which] = true;
            };

            scope.dateOptions = {
                formatYear: 'yyyy',
            };

            scope.formats = ['shortDate', 'dd-MMMM-yyyy', 'yyyy/MM/dd'];
            scope.format = scope.formats[0];

            linker(function (clone) {
                element.append(clone);
            });
        }
    }
}]);