angular.module('platformWebApp')
.directive('vaDynamicProperty', ['$compile', '$templateCache', '$http', 'platformWebApp.objCompareService', 'platformWebApp.dynamicProperties.dictionaryItemsApi', function ($compile, $templateCache, $http, objComparer, dictionaryItemsApi) {

    return {
        restrict: 'E',
        require: 'ngModel',
        replace: true,
        transclude: true,
        templateUrl: 'Scripts/app/dynamicProperties/directives/dynamicProperty.tpl.html',
        scope: { languages: "=" },
        link: function (scope, element, attr, ngModelController, linker) {

            scope.currentEntity = ngModelController.$modelValue;

            scope.context = {};
            scope.context.currentPropValues = [];
            scope.context.allDictionaryValues = [];
       
            scope.$watch('context.currentPropValues', function (newValue, oldValue) {
                //reflect only real changes
            	if (newValue.length != scope.currentEntity.values.length || !objComparer.equal(newValue, scope.currentEntity.values)) {
                	if (scope.currentEntity.isDictionary) {
                		scope.currentEntity.values = _.map(newValue, function (x) { return { value: x } });
                	}
					else if (scope.currentEntity.isArray && scope.currentEntity.isMultilingual && !scope.currentEntity.isDictionary)
                	{
                		//ungrouping values for multilingual array properties
                		scope.currentEntity.values = [];
                		angular.forEach(newValue, function (x) {
                			var values = _.map(x.values, function (y) { return { locale: x.locale, value: y.value }; });
                			scope.currentEntity.values = scope.currentEntity.values.concat(values);
                		});
                	}
                	else {
                		scope.currentEntity.values = newValue;
                	}
                    ngModelController.$setViewValue(scope.currentEntity);
      
                }
            }, true);

            scope.$watch('languages', function (languages) {
            	if (scope.currentEntity.isMultilingual && !scope.currentEntity.isDictionary) {
            		initLanguagesValuesMap(scope.currentEntity, languages);
            	}
            });

            ngModelController.$render = function () {
            	scope.currentEntity = ngModelController.$modelValue;

            	scope.context.currentPropValues = angular.copy(scope.currentEntity.values);
            	if (needAddEmptyValue(scope.currentEntity, scope.context.currentPropValues)) {
            		scope.context.currentPropValues.push({ value: null });
            	}

            	if (scope.currentEntity.isDictionary) {
            		loadDictionaryValues(scope.currentEntity);
            	}

				//Group values for multilingual array properties
            	if (scope.currentEntity.isMultilingual && scope.currentEntity.isArray && !scope.currentEntity.isDictionary) {

            		var groupByLocaleObj = _.groupBy(scope.context.currentPropValues, 'locale');
            		scope.context.currentPropValues = [];
            		for (var key in groupByLocaleObj) {
            			if (groupByLocaleObj.hasOwnProperty(key)) {
            				scope.context.currentPropValues.push({ locale: key, values: groupByLocaleObj[key] });
            			}
            		}
	           	}
            	chageValueTemplate(scope.currentEntity.valueType);
            };

            var difference = function (one, two) {
                var containsEquals = function (obj, target) {
                    if (obj == null) return false;
                    return _.any(obj, function (value) {
                    	return value.value == target.value || angular.equals(value.values, target.values);
                    });
                };
                return _.filter(one, function (value) { return !containsEquals(two, value); });
            };

            function needAddEmptyValue(property, values) {
                return !property.isArray && !property.isDictionary && !property.isMultilingual && values.length == 0;
            };

            function initLanguagesValuesMap(property, languages) {
                //Group values by language 
            	angular.forEach(languages, function (language) {
                    //Currently select values
                	var currentPropValues = _.filter(scope.context.currentPropValues, function (x) { return x.locale == language; });
                    //need add empty value for single  value type
                	if (currentPropValues.length == 0) {
                		scope.context.currentPropValues.push({ value: null, locale: language });
                    }
                });
            };


            function loadDictionaryValues(property) {
            	var selectedValues = property.values;
            	dictionaryItemsApi.query({ id: property.objectType, propertyId: property.id }, function (result) {
                    scope.context.allDictionaryValues = [];
                    scope.context.currentPropValues = [];

                    angular.forEach(result, function (dictValue) {
                    	//Need seldictValueect already selected value
                        //Dictionary values it a same type like a standart values
                        dictValue.selected = angular.isDefined(_.find(selectedValues, function (x) { return x.value.id == dictValue.id }));
                        scope.context.allDictionaryValues.push(dictValue);
                        if (dictValue.selected) {
                            //add selected value
                            scope.context.currentPropValues.push(dictValue);
                        }
                    });
                    return result;
                });

            };

            function getTemplateName(property) {
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

            scope.open = function ($event, which) {
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