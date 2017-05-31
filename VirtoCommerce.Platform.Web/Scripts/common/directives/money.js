'use strict';

angular.module('platformWebApp')
.directive('money', ['platformWebApp.currencyFormat', function (currencyFormat) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$parsers.unshift(function(viewValue) {
                return currencyFormat.validate(viewValue, attrs.minExclusive, attrs.min, attrs.maxExclusive, attrs.max, attrs.fraction, ctrl.$setValidity);
            });

            ctrl.$formatters.unshift(function (modelValue) {
                return currencyFormat.format(modelValue, attrs.minExclusive, attrs.min, attrs.maxExclusive, attrs.max, attrs.fraction);
            });

            scope.$on('$localeChangeSuccess', function () {
                ctrl.$viewValue = ctrl.$formatters.reduceRight((prev, fn) => fn(prev), ctrl.$modelValue);
                ctrl.$render();
            });
        }
    };
}]);