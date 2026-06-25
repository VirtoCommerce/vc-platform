angular.module('platformWebApp')
// Curated 64-color palette for per-widget color markers.
// Golden-angle hue distribution (deterministic), cycling 3 lightness/saturation
// bands tuned for white card surfaces. Same widget id -> same color, always.
.constant('platformWebApp.widgetColorPalette', (function () {
    // Perceptually-uniform OKLCH palette for module color markers (UX best practice for panels):
    //  - hue spread (golden-ratio low-discrepancy) over the wheel EXCLUDING reserved status arcs
    //    (amber ~35deg and green ~140deg) so a module bar never reads as a warning/success color;
    //  - CONSTANT chroma -> every bar has the same perceived vividness (consistent visual weight);
    //  - lightness varied on an independent low-discrepancy sequence to separate close hues.
    // OKLCH keeps perceived lightness/saturation equal across hues (no muddy colors, unlike HSL).
    var reserved = [[25, 60], [110, 155]];  // amber/warning + green/success hue zones (degrees)
    var segments = [], start = 0, span = 0, r, s;
    for (r = 0; r < reserved.length; r++) { segments.push([start, reserved[r][0]]); start = reserved[r][1]; }
    segments.push([start, 360]);
    for (s = 0; s < segments.length; s++) { span += segments[s][1] - segments[s][0]; }
    function allowedHue(t) {
        var x = t * span;
        for (var k = 0; k < segments.length; k++) {
            var len = segments[k][1] - segments[k][0];
            if (x <= len) { return segments[k][0] + x; }
            x -= len;
        }
        return 360;
    }
    function frac(v) { return v - Math.floor(v); }
    var palette = [];
    for (var i = 0; i < 64; i++) {
        var hue = allowedHue(frac(i * 0.6180339887));
        var lightness = Math.round((0.50 + frac(i * 0.7548776662) * 0.16) * 100); // 50..66%
        palette.push('oklch(' + lightness + '% 0.13 ' + hue.toFixed(1) + ')');
    }
    return palette;
})())
.factory('platformWebApp.widgetService', function () {

    var retVal = {
        widgetsMap: [],
        registerWidget: function (widget, containerName) {
            if (!this.widgetsMap[containerName]) {
                this.widgetsMap[containerName] = [];
            }
            this.widgetsMap[containerName].push(widget);
        },
        unregisterWidget: function (widget, containerName) {
            if (this.widgetsMap[containerName]) {
                var index = this. widgetsMap[containerName].indexOf(widget);
                if (index !== -1) {
                    this.widgetsMap[containerName].splice(index, 1);
                }
            }
        },
        getWidgets: function (containerName) {
            return this.widgetsMap[containerName] || [];
        },
        clearWidgets: function (containerName) {
            this.widgetsMap[containerName] = [];
        }
    };
    return retVal;
})
.directive('vaWidgetContainer', ['$compile', '$localStorage', 'platformWebApp.widgetService', 'platformWebApp.widgetColorPalette', function ($compile, $localStorage, widgetService, widgetColorPalette) {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '$(Platform)/Scripts/app/navigation/widget/widgetContainer.tpl.html',
        scope: {
            data: '=?',
            gridsterOpts: '=?',
            group: '@',
            blade: '='
        },
        link: function (scope, element, attr) {
            if (!scope.gridsterOpts) { scope.gridsterOpts = {}; }
            scope.$storage = $localStorage;

            scope.$watch('gridsterOpts', function () {
                var groupWidgets = _.filter(widgetService.widgetsMap[scope.group], function (w) { return !angular.isFunction(w.isVisible) || w.isVisible(scope.blade); });
                scope.widgets = angular.copy(groupWidgets);
                angular.forEach(scope.widgets, function (w) {
                    w.blade = scope.blade;
                    w.widgetsInContainer = scope.widgets;
                });
            }, true);

            scope.getKey = function (prefix, widget) {
                return (prefix + widget.controller + widget.template + scope.group).hashCode();
            }

            // Deterministic color marker grouped by MODULE, so all widgets from the same
            // module share one color. The module is the controller prefix up to the
            // "*Module" segment, e.g.
            //   virtoCommerce.pricingModule.pricesWidgetController       -> virtoCommerce.pricingModule
            //   virtoCommerce.orderModule.dashboard.statisticsWidgetCtrl -> virtoCommerce.orderModule
            // Controllers without a "*Module" segment (e.g. platformWebApp.*) fall back to
            // their first namespace segment.
            scope.getWidgetColor = function (widget) {
                var controller = widget.controller || '';
                var match = controller.match(/^(.*?Module)\b/);
                var key = match ? match[1] : controller.split('.')[0];
                return widgetColorPalette[Math.abs((key || '').hashCode()) % widgetColorPalette.length];
            }
        }
    }
}])
.directive('vaWidget', ['$compile', 'platformWebApp.widgetService', 'platformWebApp.authService', function ($compile, widgetService, authService) {
    return {
        link: function (scope, element, attr) {

            if (!scope.widget.permission || authService.checkPermission(scope.widget.permission)) {
                element.attr('ng-controller', scope.widget.controller);
                element.attr('ng-model', 'widget');
                element.removeAttr("va-widget");
                $compile(element)(scope);
            }

        }
    }
}]);
