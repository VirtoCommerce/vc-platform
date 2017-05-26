'use strict';

angular.module('platformWebApp')
.directive('smartFloat', ['platformWebApp.numberUtils', function (numberUtils) {
        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function(viewValue) {
                    return numberUtils.validate(viewValue, attrs.numType, attrs.min, attrs.max, attrs.fraction, ctrl.$setValidity);
                });

                ctrl.$formatters.unshift(function (modelValue) {
                    return numberUtils.format(modelValue, attrs.numType, attrs.min, attrs.max, attrs.fraction);
                });

                scope.$on('$localeChangeSuccess', function () {
                    ctrl.$viewValue = ctrl.$formatters.reduceRight((prev, fn) => fn(prev), ctrl.$modelValue);
                    ctrl.$render();
                });
            }
        };
    }
]);