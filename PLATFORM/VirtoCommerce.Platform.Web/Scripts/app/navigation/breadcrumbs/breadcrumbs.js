angular.module('platformWebApp')
.directive('vaBreadcrumb', ['$compile', function ($compile)
{
    return {
    	restrict: 'E',
    	require: 'ngModel',
        replace: true,
        scope: { },
        templateUrl: 'Scripts/app/navigation/breadcrumbs/breadcrumbs.tpl.html',
        link: function (scope, element, attr, ngModelController, linker) {
        	scope.breadcrumbs = {};
        	ngModelController.$render = function () {
        		scope.breadcrumbs = ngModelController.$modelValue;
        	};

        	scope.innerNavigate = function (breadcrumb) {
        		breadcrumb.navigate(breadcrumb);
        	};
        }
    }
}])