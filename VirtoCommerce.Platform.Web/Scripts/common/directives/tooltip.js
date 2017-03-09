angular.module('platformWebApp')
.directive('vaTooltip', [function ()
{
    return {
        restrict: 'A',
        priority: 10000,
        compile: function (tElem, tAttrs)
        {
            if (!tElem.attr('tooltip-placement')) {
                tElem.attr('tooltip-placement', '{{tooltipPlacement}}');
            }
            if (!tElem.attr('tooltip-animation')) {
                tElem.attr('tooltip-animation', '{{tooltipAnimation}}');
            }
            if (!tElem.attr('tooltip-popup-delay')) {
                tElem.attr('tooltip-popup-delay', '{{tooltipPopupDelay}}');
            }
            if (!tElem.attr('tooltip-trigger')) {
                tElem.attr('tooltip-trigger', '{{tooltipTrigger}}');
            }
            if (!tElem.attr('tooltip-enable')) {
                tElem.attr('tooltip-enable', '{{tooltipEnable}}');
            }
            if (!tElem.attr('tooltip-append-to-body')) {
                tElem.attr('tooltip-append-to-body', '{{tooltipAppendToBody}}');
            }
            if (!tElem.attr('tooltip-class')) {
                tElem.attr('tooltip-class', '{{tooltipClass}}');
            }
        }
    };
}]);