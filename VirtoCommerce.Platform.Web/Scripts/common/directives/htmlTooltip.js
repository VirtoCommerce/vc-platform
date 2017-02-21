angular.module('platformWebApp')
.directive('vaTooltip', ['$compile', '$sce', function ($compile, $sce)
{
    // va-tooltip = path to template or pure tooltip string
    // tooltip-updater = scope item to watch for changes when template has to be reloaded [optional (only if template is dynamic)]
    return {
        restrict: 'A',
        scope: true,
        // prevent to double-compile some angular directives
        // try to change priority to solve issue, if you have
        terminal: true,
        priority: 100,
        compile: function (tElem, tAttrs)
        {
            // add bootstrap directive
            if (tAttrs.vaTooltipHtml != null && !tElem.attr('tooltip-html'))
            {
                tElem.attr('tooltip-html', '{{tooltipHtml}}');
            }
            else if (tAttrs.vaTooltip != null && !tElem.attr('tooltip')) {
                tElem.attr('tooltip', "{{tooltip}}");
            }
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
            return function (scope, element, attrs) {
                scope.tooltip = attrs.vaTooltip;
                scope.tooltipHtml = attrs.vaTooltipHtml;

                var loadTemplate;
                if (scope.tooltipHtml != null) {
                    var tplUrl = $parse(scope.tooltipHtml)(scope);
                    loadTemplate = function() {
                        $http.get(tplUrl, { cache: $templateCache }).success(function(tplContent) {
                            var container = $('<div/>');
                            container.html($compile(tplContent.trim())(scope));
                            $timeout(function() {
                                scope.tooltipHtml = $sce.trustAsHtml(container.html());
                            });
                        });
                    }
                }

                // remove our direcive to avoid infinite loop
                element.removeAttr('va-tooltip');
                // compile element to attach tooltip binding
                $compile(element)(scope);

                if (scope.tooltipHtml != null) {
                    if (angular.isDefined(attrs.tooltipUpdater)) {
                        scope.$watch(attrs.tooltipUpdater, function() {
                            loadTemplate();
                        });
                    } else {
                        loadTemplate();
                    }
                }
            };
        }
    };
}]);