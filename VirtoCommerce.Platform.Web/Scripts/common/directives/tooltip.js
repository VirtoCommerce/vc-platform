angular.module('platformWebApp')
.directive('vaTooltip', [function ()
{
    return {
        restrict: 'A',
        priority: 10000,
        compile: function (tElem, tAttrs)
        {
            var attributes = ["tooltip-placement", "tooltip-animation", "tooltip-popup-delay", "tooltip-trigger", "tooltip-enable", "tooltip-append-to-body", "tooltip-class"];
            for (var i = 0; i < attributes.length; i++) {
                var attribute = attributes[i];
                if (!tElem.attr[attribute]) {
                    tElem.attr(attribute, '{{' + tAttrs.$normalize(attribute) + '}}');
                }
            }
        }
    };
}]);