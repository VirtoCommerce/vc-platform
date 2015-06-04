angular.module('platformWebApp')
.controller('platformWebApp.accountRolesListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {

    function initializeBlade(data) {
        $scope.blade.currentEntity = data;
        $scope.blade.isLoading = false;
    };

    $scope.blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        closeCallback();
    };

    function closeChildrenBlades() {
        angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    $scope.blade.headIcon = 'fa-lock';

    $scope.blade.toolbarCommands = [
           {
               name: "Manage roles", icon: 'fa fa-edit',
               executeMethod: function () {
                   var newBlade = {
                       id: "accountChildBladeChild",
                       promise: $scope.blade.promise,
                       title: $scope.blade.title,
                       subtitle: 'Manage roles',
                       controller: 'platformWebApp.accountRolesController',
                       template: 'Scripts/common/security/blades/account-roles.tpl.html'
                   };

                   bladeNavigationService.showBlade(newBlade, $scope.blade);
               },
               canExecuteMethod: function () {
                   return true;
               },
               permission: 'platform:security:manage'
           }
    ];

    $scope.$watch('blade.parentBlade.currentEntity', initializeBlade);

    // on load: 
    // $scope.$watch('blade.parentBlade.currentEntity' gets fired
}]);