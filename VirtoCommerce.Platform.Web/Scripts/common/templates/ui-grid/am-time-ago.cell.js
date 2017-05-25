angular.module('platformWebApp')
.controller('platformWebApp.common.templates.am-time-ago.cell', ['$scope', 'platformWebApp.userProfile', function ($scope, userProfile) {
    if (userProfile.useTimeAgo == undefined) {
        userProfile.load().then(function() {
            $scope.useTimeAgo = userProfile.useTimeAgo;
        });
    } else {
        $scope.useTimeAgo = userProfile.useTimeAgo;
    }
    userProfile.registerOnChangeCallback(function(oldState, newState) {
        $scope.useTimeAgo = newState.useTimeAgo;
    });
}]);