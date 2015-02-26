'use strict';

angular.module('platformWebApp.inputNumber', [])
    .directive('vaNumber', function ($compile, $timeout) {
        return {
            restrict: 'A',
            scope: {
                min: '=',
                max: '=',
                step: '='
            },
            require: 'ngModel',
            link: function (scope, el, attrs, ctrl) {
                scope.html = '<a class="up" ng-click="sum(\'up\')" data-type="up"></a> <a class="down" ng-click="sum(\'down\')" data-type="down"></a>';

                el.after($compile(scope.html)(scope));
                el.prop('readonly', true);

                scope.sum = function (type) {
                    var sum = 0,
                        value = parseInt(ctrl.$modelValue),
                        step  = parseInt(scope.step);

                    if(type == 'up') {
                        sum = value + step;

                        if (sum > scope.max) {
                            sum = scope.max;
                        }
                    }
                    else {
                        sum = value - step;

                        if (sum < scope.min) {
                            sum = scope.min || 0;
                        }
                    }

                    ctrl.$setViewValue(sum);
                    ctrl.$render();
                }
            }
        };
    });