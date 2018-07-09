angular.module('platformWebApp')
    .directive('charsLeftToMax', ['$parse', '$compile', function ($parse, $compile) {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModel) {
            var idP = Math.round(Math.random() * 1000000000);
            $(element).parent('div').prev('label').after('<span class="form-label-limit" id=' + idP + '></span>');
            scope.$watch(function () {
                return ngModel.$viewValue;
            },
                function (newValue) {
                    if (angular.isDefined(newValue)) {
                        var remainingChar = attr.ngMaxlength - newValue.length;
                        var remaiSpan;
                        if (remainingChar >= 0)
                            remaiSpan = "Maximum " + attr.ngMaxlength + " characters(" + remainingChar + " remaining)";
                        if (remainingChar < 0)
                            remaiSpan = "Maximum " + attr.ngMaxlength + " characters(" + (-remainingChar) + " too many)";
                        $('#' + idP).html(remaiSpan);
                        if (remainingChar > attr.warningCount && remainingChar > attr.dangerCount) {
                            $('#' + idP).attr("style", "color: #999;");
                        } 
                        else {
                            if (remainingChar <= attr.warningCount && remainingChar > attr.dangerCount) {
                                $('#' + idP).attr("style", "color: #eea236;");
                            } 
                            else {
                                $('#' + idP).attr("style", "color: #e51400;");
                            }
                        }
                    }
                }
            );
        }
    };
}]);
