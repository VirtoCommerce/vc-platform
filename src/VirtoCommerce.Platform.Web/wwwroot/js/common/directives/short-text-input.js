angular.module('platformWebApp')
    .directive('vaShortTextInput', function () {
        return {
            restrict: 'E',
            templateUrl: '$(Platform)/Scripts/common/directives/short-text-input.html',
            require: ['^form', 'ngModel'],
            scope: {
                ngModel: "=",
                name: "=",
                form: "=",
                validationRules: "=",
                required: "@"
            },
            replace: true,
            transclude: true
        }
    });
