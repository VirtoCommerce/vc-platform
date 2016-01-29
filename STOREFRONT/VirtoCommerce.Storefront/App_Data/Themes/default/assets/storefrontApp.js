var storefrontApp = angular.module('storefrontApp', ['ngRoute']);

storefrontApp.factory('httpErrorInterceptor', ['$q', '$rootScope', function ($q, $rootScope) {
    var httpErrorInterceptor = {};

    httpErrorInterceptor.responseError = function (rejection) {
        $rootScope.$broadcast('storefrontError', {
            type: 'error',
            title: rejection.data.message,
            message: rejection.data.stackTrace
        });
        return $q.reject(rejection);
    };
    httpErrorInterceptor.requestError = function (rejection) {
        $rootScope.$broadcast('storefrontError', {
            type: 'error',
            title: rejection.data.message,
            message: rejection.data.stackTrace
        });
        return $q.reject(rejection);
    };

    return httpErrorInterceptor;
}])

storefrontApp.config(['$interpolateProvider', '$routeProvider', '$httpProvider', function ($interpolateProvider, $routeProvider, $httpProvider) {
    //Add interceptor
    $httpProvider.interceptors.push('httpErrorInterceptor');

    $routeProvider
        .when('/shipping-address', {
            templateUrl: 'storefront.checkout.shipping-address.tpl'
        })
        .when('/shipping-method', {
            templateUrl: 'storefront.checkout.shipping-method.tpl'
        })
        .when('/payment-method', {
            templateUrl: 'storefront.checkout.payment-method.tpl'
        });

    return $interpolateProvider.startSymbol('{(').endSymbol(')}');
}]);