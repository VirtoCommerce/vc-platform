angular.module('platformWebApp')
.directive('charsLeftTo255', function () {
    return {
        restrict: 'A',
        compile: function compile() {
            return {
                post: function postLink(scope, iElement, iAttrs) {
                    iElement.bind('keyup', function () {
                        scope.$apply(function () {
                            if (scope.maxlength255) {
                                var diff = 255 - iElement.val().length;
                                scope.charsLeftTo255 = diff >= 0 ? "Maximum 255 characters(" + diff + " remaining)" :
                                    diff < 0 ? "Maximum 255 characters(" + (-diff) + " to many)" : "";
                                var elem = iElement.parent().siblings("span");
                                if (diff >= 0)
                                    elem.attr("style", "color: #999;");
                                if (diff < 0)
                                    elem.attr("style", "color: #e51400;");
                            }
                        });
                    });
                }
            }
        }
    }
})
