angular.module('platformWebApp')
.controller('platformWebApp.newAccountWizardController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.accounts', 'platformWebApp.roles', function ($scope, bladeNavigationService, accounts, roles) {
    var blade = $scope.blade;
    var promise = roles.search({ count: 10000 }).$promise;

    function initializeBlade(data) {
        promise.then(function (promiseData) {
            blade.isLoading = false;
            blade.currentEntities = promiseData.roles;
        });
    };

    blade.selectNode = function (node) {
        $scope.selectedNodeId = node.id;

        var newBlade = {
            id: 'roleDetails',
            data: node,
            title: node.name,
            subtitle: 'platform.blades.role-detail.subtitle',
            controller: 'platformWebApp.roleDetailController',
            template: '$(Platform)/Scripts/app/security/blades/role-detail.tpl.html'
        };

        bladeNavigationService.showBlade(newBlade, blade);
    };

    $scope.saveChanges = function () {
        if (blade.currentEntity.password != blade.currentEntity.newPassword2) {
            blade.error = 'Error: passwords don\'t match!';
            return;
        }

        blade.isLoading = true;
        blade.error = undefined;
        var postData = angular.copy(blade.currentEntity);
        postData.newPassword2 = undefined;
        postData.roles = _.where(blade.currentEntities, { $selected: true });

        accounts.save({}, postData, function (data) {
            blade.parentBlade.refresh();
            blade.parentBlade.selectNode(data);
        }, function (error) {
            var errText = 'Error ' + error.status;
            if (error.data && error.data.message) {
                errText = errText + ": " + error.data.message;
            }
            bladeNavigationService.setError(errText, $scope.blade);
        });
    };
    
    blade.headIcon = 'fa-key';
    
    // actions on load
    initializeBlade(blade.currentEntity);
}]);