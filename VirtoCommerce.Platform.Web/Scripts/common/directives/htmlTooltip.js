angular.module('platformWebApp')
.directive('vaTooltip', ['$http', '$templateCache', '$compile', '$parse', '$timeout', function ($http, $templateCache, $compile, $parse, $timeout)
{
    //va-tooltip = path to template or pure tooltip string
    //tooltip-updater = scope item to watch for changes when template has to be reloaded [optional (only if template is dynamic)]
    //All other attributes can be added for standart boostrap tooltip behavior (ex. tooltip-placement)
    return {
        restrict: 'A',
        scope: true,
        compile: function (tElem, tAttrs)
        {
            //Add bootstrap directive
            if (!tElem.attr('tooltip-html-unsafe'))
            {
                tElem.attr('tooltip-html-unsafe', '{{tooltip}}');
            }
            return function (scope, element, attrs)
            {
                scope.tooltip = attrs.vaTooltip;
                var tplUrl = $parse(scope.tooltip)(scope);
                function loadTemplate()
                {
                    $http.get(tplUrl, { cache: $templateCache }).success(function (tplContent)
                    {
                        var container = $('<div/>');
                        container.html($compile(tplContent.trim())(scope));
                        $timeout(function ()
                        {
                            scope.tooltip = container.html();
                        });
                    });
                }
                //remove our direcive to avoid infinite loop
                element.removeAttr('va-tooltip');
                //compile element to attach tooltip binding
                $compile(element)(scope);

                if (angular.isDefined(attrs.tooltipUpdater))
                {
                    scope.$watch(attrs.tooltipUpdater, function ()
                    {
                        loadTemplate();
                    });
                } else
                {
                    loadTemplate();
                }
            };
        }
    };
}]);
