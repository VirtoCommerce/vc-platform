angular.module('platformWebApp.widget', [
])
.factory('widgetService', function () {

	var retVal = {
		widgets: [],
		registerWidget: function (widget) {
			this.widgets.push(widget);
		}
	};
	return retVal;
})
.directive('vaWidgetContainer', ['$compile', '$filter', 'widgetService', function ($compile, $filter, widgetService) {
	return {
		restrict: 'E',
		replace: true,
		templateUrl: 'Scripts/common/navigation/widget/widgetContainer.tpl.html',
		scope: {
		    group: '@',
            blade: '='
		},
		link: function (scope, element, attr) {
		    scope.widgets = $filter('filter')(widgetService.widgets, { group: scope.group });
		    angular.forEach(scope.widgets, function(w) { w.blade = scope.blade; } );
		}
	}
}])
.directive('vaWidget', ['$compile', 'widgetService', function ($compile, widgetService) {
	return {
		terminal: true,
		priority: 1000,
		link: function (scope, element, attr) {
		    element.attr('ng-controller', scope.widget.controller);
		    element.attr('ng-model', 'widget');
			element.removeAttr("va-widget");
			$compile(element)(scope);
		}
	}
}]);