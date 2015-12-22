var storefrontApp = angular.module('storefrontApp');

storefrontApp.directive('vcContentPlace', ['marketingService', function (marketingService) {
    return {
        restrict: 'E',
        link: function (scope, element, attrs) {
            marketingService.getDynamicContent(attrs.id).then(function (response) {
                element.html(response.data);
            });
        },
        replace: true
    }
}]);