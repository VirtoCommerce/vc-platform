'use strict';

angular.module('platformWebApp')
.directive('smartFloat', ['platformWebApp.numberFormat', function (numberFormat) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$parsers.unshift(function(viewValue) {
                return numberFormat.validate(viewValue, attrs.numType, attrs.minExclusive, attrs.min, attrs.maxExclusive, attrs.max, attrs.fraction, ctrl.$setValidity);
            });

            ctrl.$formatters.unshift(function (modelValue) {
                return numberFormat.format(modelValue, attrs.numType, attrs.minExclusive, attrs.min, attrs.maxExclusive, attrs.max, attrs.fraction);
            });

            scope.$on('$localeChangeSuccess', function () {
                ctrl.$viewValue = ctrl.$formatters.reduceRight((prev, fn) => fn(prev), ctrl.$modelValue);
                ctrl.$render();
            });
        }
    };
}]);