angular.module('platformWebApp')
.directive('vaTooltipOptions', [function () {
    return {
        restrict: 'A',
        scope: false,
        compile: function () {
            return function (scope, element, attrs) {
                if (attrs.tooltipPlacement) {
                    scope.tooltipPlacement = attrs.tooltipPlacement;
                }
                if (attrs.tooltipAnimation) {
                    scope.tooltipAnimation = attrs.tooltipAnimation;
                }
                if (attrs.tooltipPopupDelay) {
                    scope.tooltipPopupDelay = attrs.tooltipPopupDelay;
                }
                if (attrs.tooltipTrigger) {
                    scope.tooltipTrigger = attrs.tooltipTrigger;
                }
                if (attrs.tooltipEnable) {
                    scope.tooltipEnable = attrs.tooltipEnable;
                }
                if (attrs.tooltipAppendToBody) {
                    scope.tooltipAppendToBody = attrs.tooltipAppendToBody;
                }
                if (attrs.tooltipClass) {
                    scope.tooltipClass = attrs.tooltipClass;
                }
            };
        }
    };
}]);
