/**
 * Heavily adapted from the `type="number"` directive in Angular's
 * /src/ng/directive/input.js
 */

'use strict';

angular.module('platformWebApp')
// TODO: Replace with tested localized version (see below)
.directive('money', ['$timeout', function ($timeout) {
    'use strict';

    var NUMBER_REGEXP = /^\s*(\-|\+)?(\d+|(\d*(\.\d*)))\s*$/;

    function link(scope, el, attrs, ngModelCtrl) {
        var min = parseFloat(attrs.min || 0);
        var precision = parseFloat(attrs.precision || 2);
        var lastValidValue;

        function round(num) {
            var d = Math.pow(10, precision);
            return Math.round(num * d) / d;
        }

        function formatPrecision(value) {
            return parseFloat(value).toFixed(precision);
        }

        function formatViewValue(value) {
            return ngModelCtrl.$isEmpty(value) ? '' : '' + value;
        }


        ngModelCtrl.$parsers.push(function (value) {
            if (angular.isUndefined(value)) {
                value = '';
            }

            // Handle leading decimal point, like ".5"
            if (value.indexOf('.') === 0) {
                value = '0' + value;
            }

            // Allow "-" inputs only when min < 0
            if (value.indexOf('-') === 0) {
                if (min >= 0) {
                    value = null;
                    ngModelCtrl.$setViewValue('');
                    ngModelCtrl.$render();
                } else if (value === '-') {
                    value = '';
                }
            }

            var empty = ngModelCtrl.$isEmpty(value);
            if (empty || NUMBER_REGEXP.test(value)) {
                lastValidValue = (value === '')
                  ? null
                  : (empty ? value : parseFloat(value));
            } else {
                // Render the last valid input in the field
                ngModelCtrl.$setViewValue(formatViewValue(lastValidValue));
                ngModelCtrl.$render();
            }

            ngModelCtrl.$setValidity('number', true);
            return lastValidValue;
        });
        ngModelCtrl.$formatters.push(formatViewValue);

        var minValidator = function (value) {
            if (!ngModelCtrl.$isEmpty(value) && value < min) {
                ngModelCtrl.$setValidity('min', false);
                return undefined;
            } else {
                ngModelCtrl.$setValidity('min', true);
                return value;
            }
        };
        ngModelCtrl.$parsers.push(minValidator);
        ngModelCtrl.$formatters.push(minValidator);

        if (attrs.max) {
            var max = parseFloat(attrs.max);
            var maxValidator = function (value) {
                if (!ngModelCtrl.$isEmpty(value) && value > max) {
                    ngModelCtrl.$setValidity('max', false);
                    return undefined;
                } else {
                    ngModelCtrl.$setValidity('max', true);
                    return value;
                }
            };

            ngModelCtrl.$parsers.push(maxValidator);
            ngModelCtrl.$formatters.push(maxValidator);
        }

        // Round off
        if (precision > -1) {
            ngModelCtrl.$parsers.push(function (value) {
                return value ? round(value) : value;
            });
            ngModelCtrl.$formatters.push(function (value) {
                return value ? formatPrecision(value) : value;
            });
        }

        el.bind('blur', function () {
            $timeout(function () {
                var value = ngModelCtrl.$modelValue;
                if (value) {
                    ngModelCtrl.$viewValue = formatPrecision(value);
                    ngModelCtrl.$render();
                }
            });
        });
    }

    return {
        restrict: 'A',
        require: 'ngModel',
        link: link
    };
}]);
// Custom directive to support localized currency float number parsing & validation
//.directive('money', ['platformWebApp.currencyFormat', function (currencyFormat) {
//    return {
//        restrict: 'A',
//        require: 'ngModel',
//        link: function (scope, elem, attrs, ctrl) {
//            ctrl.$parsers.unshift(function(viewValue) {
//                return currencyFormat.validate(viewValue, attrs.minExclusive, attrs.min, attrs.maxExclusive, attrs.max, attrs.fraction, ctrl.$setValidity);
//            });

//            ctrl.$formatters.unshift(function (modelValue) {
//                return currencyFormat.format(modelValue, attrs.minExclusive, attrs.min, attrs.maxExclusive, attrs.max, attrs.fraction);
//            });

//            scope.$on('$localeChangeSuccess', function () {
//                ctrl.$viewValue = ctrl.$formatters.reduceRight((prev, fn) => fn(prev), ctrl.$modelValue);
//                ctrl.$render();
//            });
//        }
//    };
//}]);