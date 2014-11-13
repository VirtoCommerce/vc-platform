angular.module('platformWebApp.breadcrumbs', [
])
.directive('vaBreadcrumb', ['$compile', function ($compile)
{
    return {
        restrict: 'E',
        replace: true,
        scope: {breadcrumbs: '='},
        templateUrl: 'Scripts/common/navigation/breadcrumbs/breadcrumbs.tpl.html',
        link: function (scope)
        {
        }
    }
}])