angular.module('notifications.blades.history', [
   'catalogModule.resources.catalogs',
   'angularMoment'
])
.controller('notificationsHistoryController', ['$http', '$rootScope', '$scope', 'bladeNavigationService', 'dialogService',
function ($http, $rootScope, $scope, bladeNavigationService, dialogService) {
    
    var serviceBase = 'api/notification/';
    $scope.notifications = [];
    $scope.blade.refresh = function () {
        $scope.blade.isLoading = true;

        $http.get(serviceBase + 'allRecent').success(function (data, status, headers, config) {
            $scope.notifications = data.notifyEvents;
            $scope.blade.isLoading = false;
        });

    };

    

    // actions on load
    $scope.blade.refresh();
}]);