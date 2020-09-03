angular.module('platformWebApp')
    .directive('vaCharsCount', function () {
            return {
                template: '<div class="chars-counter"><span>{{!!vaField ? vaField.length : 0}}</span> / <span>{{vaMaxlength}}</span></div>',
                scope: {
                    vaField: "@",
                    vaMaxlength: "@",
                    vaType: "@"
                },
                link: function (scope, element) {
                    scope.$watch('vaField', function () {
                        if (scope.vaField.length > scope.vaMaxlength) {
                            element.addClass('text-' + scope.vaType);
                        } else {
                            element.removeClass('text-' + scope.vaType);
                        }
                    });
                },
            }
        }
    );
