angular.module('platformWebApp')
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
.directive('vaWidgetContainer', ['$compile', '$localStorage', 'platformWebApp.widgetService', function ($compile, $localStorage, widgetService) {
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
