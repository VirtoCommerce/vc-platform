var storefrontApp = angular.module('storefrontApp');

storefrontApp.controller('mainController', ['$scope', '$location', '$window', 'customerService', function ($scope, $location, $window, customerService) {
    getCustomer();

    //Base store url populated in layout and can be used for construction url inside controller
    $scope.baseUrl = {};

    $scope.currentPath = $location.$$path.replace('/', '');

    //For outside app redirect (To reload the page after changing the URL, use the lower-level API)
    $scope.outerRedirect = function (absUrl) {
        $window.location.href = absUrl;
    };

    //change in the current URL or change the current URL in the browser (for app route)
    $scope.innerRedirect = function (path) {
        $location.path(path);
        $scope.currentPath = $location.$$path.replace('/', '');
    };

    $scope.errorDetailsVisible = false;
    $scope.toggleErrorDetails = function (isVisible) {
        $scope.errorDetailsVisible = !isVisible;
    }

    $scope.getObjectSize = function (obj) {
        var size = 0, key;
        for (key in obj) {
            if (obj.hasOwnProperty(key)) {
                size++;
            }
        }
        return size;
    }

    function getCustomer() {
        customerService.getCurrentCustomer().then(function (response) {
            $scope.customer = response.data;
        });
    }
}]);