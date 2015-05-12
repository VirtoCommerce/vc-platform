angular.module('virtoCommerce.helpdeskModule')
.controller('helpdeskWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'zendesk_res_authlink', function ($scope, bladeNavigationService, authLink) {

    $scope.widget.refresh = function () {
        $scope.authLink = "";
        authLink.query({}, function (data) {
            $scope.authLink = data[0];
        });
    }

    $scope.widget.refresh();
}]);