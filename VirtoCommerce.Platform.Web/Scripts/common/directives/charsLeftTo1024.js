angular.module('platformWebApp')
    .directive('charsLeftTo1024', function () {
        return {
            restrict: 'A',
            compile: function compile() {
                return {
                    post: function postLink(scope, iElement, iAttrs) {
                        iElement.bind('keyup', function () {
                            scope.$apply(function () {
                                if (scope.maxlength1024) {
                                    var diff = scope.maxlength1024 - iElement.val().length;
                                    scope.charsLeftTo1024 = diff >= 0 ? "Maximum " + scope.maxlength1024 + " characters(" + diff + " remaining)" :
                                        diff < 0 ? "Maximum " + scope.maxlength1024 + " characters(" + (-diff) + " to many)" : "";
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
