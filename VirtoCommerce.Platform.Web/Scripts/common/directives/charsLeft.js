angular.module('platformWebApp')
.directive('charsLeft', function () {
    return {
        restrict: 'A',
        compile: function compile() {
            return {
                post: function postLink(scope, iElement, iAttrs) {
                    iElement.bind('keyup', function () {
                        scope.$apply(function () {
                            scope.charsLeftToMax = scope.maxlength - iElement.val().length;
                        });
                    });
                }
            }
        }
    }
})
