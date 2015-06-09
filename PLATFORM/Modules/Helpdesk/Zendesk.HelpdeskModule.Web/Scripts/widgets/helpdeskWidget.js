angular.module('virtoCommerce.helpdeskModule')
.controller('virtoCommerce.helpdeskModule.helpdeskWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.helpdeskModule.zendesk_res_authlink', function ($scope, bladeNavigationService, authLink) {

    $scope.widget.refresh = function () {
        $scope.authLink = "";
        authLink.query({}, function (data) {
            $scope.authLink = data[0];
        }, function (error) {
        });
    }

    $scope.widget.refresh();
}]);