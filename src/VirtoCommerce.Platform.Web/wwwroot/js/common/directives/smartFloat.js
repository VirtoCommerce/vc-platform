'use strict';

angular.module('platformWebApp')
// TODO: Replace with tested localized version (see below)
.directive('smartFloat', ['$filter', '$compile', function ($filter, $compile) {
    var INTEGER_REGEXP = /^\-?\d+$/; //Integer number
    var FLOAT_REGEXP_1 = /^[-+]?\$?\d+.(\d{3})*(,\d*)$/; //Numbers like: 1.123,56
    var FLOAT_REGEXP_2 = /^[-+]?\$?\d+,(\d{3})*(.\d*)$/; //Numbers like: 1,123.56
    var FLOAT_REGEXP_3 = /^[-+]?\$?\d+(.\d*)?$/; //Numbers like: 1123.56
    var FLOAT_REGEXP_4 = /^[-+]?\$?\d+(,\d*)?$/; //Numbers like: 1123,56

    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            var fraction = attrs.fraction ? attrs.fraction : 2;
            if (attrs.numType === "float") {
                ctrl.$parsers.unshift(function (viewValue) {                    
                    if (FLOAT_REGEXP_1.test(viewValue)) {
                        ctrl.$setValidity('float', true);
                        return parseFloat(viewValue.replace('.', '').replace(',', '.'));
                    } else if (FLOAT_REGEXP_2.test(viewValue)) {
                        ctrl.$setValidity('float', true);
                        return parseFloat(viewValue.replace(',', ''));
                    } else if (FLOAT_REGEXP_3.test(viewValue)) {
                        ctrl.$setValidity('float', true);
                        return parseFloat(viewValue);
                    } else if (FLOAT_REGEXP_4.test(viewValue)) {
                        ctrl.$setValidity('float', true);
                        return parseFloat(viewValue.replace(',', '.'));
                    } else {
                        //Allow to use empty values
                        ctrl.$setValidity('float', !viewValue);
                        return viewValue;
                    }
                });

                ctrl.$formatters.unshift(
                    function (modelValue) {
                        return $filter('number')(parseFloat(modelValue), fraction);
                    }
                );
            }
            else {
                ctrl.$parsers.unshift(function (viewValue) {
                    if (INTEGER_REGEXP.test(viewValue)) {
                        ctrl.$setValidity('integer', true);
                        return viewValue;
                    }
                    else {
                        //Allow to use empty values
                        ctrl.$setValidity('integer', !viewValue);
                        return viewValue;
                    }
                });
            }
        }
    };
}]);
// Custom directive to support localized integer and float number parsing & validation
//.directive('smartFloat', ['platformWebApp.numberFormat', function (numberFormat) {
//    return {
//        restrict: 'A',
//        require: 'ngModel',
//        link: function (scope, elem, attrs, ctrl) {
//            ctrl.$parsers.unshift(function(viewValue) {
//                return numberFormat.validate(viewValue, attrs.numType, attrs.minExclusive, attrs.min, attrs.maxExclusive, attrs.max, attrs.fraction, ctrl.$setValidity);
//            });

//            ctrl.$formatters.unshift(function (modelValue) {
//                return numberFormat.format(modelValue, attrs.numType, attrs.minExclusive, attrs.min, attrs.maxExclusive, attrs.max, attrs.fraction);
//            });

//            scope.$on('$localeChangeSuccess', function () {
//                ctrl.$viewValue = ctrl.$formatters.reduceRight((prev, fn) => fn(prev), ctrl.$modelValue);
//                ctrl.$render();
//            });
//        }
//    };
//}]);