angular.module('virtoCommerce.helpdeskModule')
.controller('virtoCommerce.helpdeskModule.openTicketsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.helpdeskModule.user_tickets', 'virtoCommerce.helpdeskModule.zendesk_dashboard_link', function ($scope, bladeNavigationService, items, zendeskLink) {
    
    $scope.$watch('blade.currentEntity', function () {
        $scope.openTickets = 0;
        if ($scope.blade.currentEntity) {
            $scope.blade.isLoading = true;
            items.query({ status: "open", email: $scope.blade.currentEntity.emails[0] }, function(data) {
                $scope.openTickets = data.length;
                $scope.blade.isLoading = false;
            }, function() {
                $scope.blade.isLoading = false;
            });

            $scope.zendeskLink = "";
            zendeskLink.query({ email: $scope.blade.currentEntity.emails[0] }, function (data) {
                $scope.zendeskLink = data[0];
            }, function (error) {
            });
        }
    });

}]);