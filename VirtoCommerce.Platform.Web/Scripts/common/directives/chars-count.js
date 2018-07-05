angular.module('platformWebApp')
    .directive('vaCharsCount', function () {
        return {
            template: `<div class="chars-counter">
                           <span>{{vaField == undefined ? '0' : vaField.length}}</span> / <span>{{vaMaxlength}}</span> 
                       </div>`,
            scope: {
                vaField: "=",
                vaMaxlength: "@"
            },
            link: function (scope, element) {
                scope.$parent.vaMaxlength = scope.vaMaxlength;

                scope.$watch('vaField', function (newValue, oldValue) {
                    scope.$parent.vaField = scope.vaField;

                    if (newValue != undefined) {
                        if (newValue.length > scope.vaMaxlength) {
                            element.addClass('invalid');
                        } else {
                            element.removeClass('invalid');
                        }
                    }
                });
            },
        }
    }
);
