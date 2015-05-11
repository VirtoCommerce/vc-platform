angular.module('virtoCommerce.helpdeskModule')
.controller('virtoCommerce.helpdeskModule.openTicketsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.helpdeskModule.user_tickets', function ($scope, bladeNavigationService, items) {
    
    $scope.$watch('blade.currentEntity', function () {
        $scope.openTickets = 0;
        if ($scope.blade.currentEntity) {
            $scope.blade.isLoading = true;
            items.query({ status: "open", email: $scope.blade.currentEntity.emails[0] }, function(data) {
                $scope.openTickets = data.length;
                $scope.blade.isLoading = false;
            }, function() {
                $scope.openTickets = 0;
                $scope.blade.isLoading = false;
            });
        }
    });

}]);