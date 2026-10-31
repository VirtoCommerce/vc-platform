angular.module('platformWebApp')
    .controller('platformWebApp.signInMethodsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
        $scope.openBlade = function () {
            var newBlade = {
                id: 'signInMethodsBlade',
                controller: 'platformWebApp.signInMethodsListController',
                template: '$(Platform)/Scripts/app/userProfile/blades/sign-in-methods-list.html'
            };

            bladeNavigationService.showBlade(newBlade, $scope.widget.blade);
        };
    }]);
