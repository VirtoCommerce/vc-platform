angular.module('platformWebApp')
.controller('platformWebApp.security.scopeValuePickFromSimpleListController', ['$scope', function ($scope) {
    var blade = $scope.blade;

    function initializeBlade() {
        blade.selectedIds = _.map(blade.currentEntity.assignedScopes, x => x.scope);
        $scope.items = _.map(blade.currentEntity.assignedScopes, function (x) {
            var result = {};
            result.id = x.scope;
            result.name = x.label;
            return result;
        });
        blade.isLoading = false;
    }       

    $scope.search = function (criteria) {
        return blade.dataService.search(criteria);
    };

    $scope.selectItem = function (item) {
        $scope.items.push(item);
    };

    $scope.removeItem = function (item) {
        $scope.items = _.reject($scope.items, x => item.id === x.id);
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    };

    $scope.isValid = function () {
    	return true;
    };

    $scope.saveChanges = function () {
        var selection = _.map($scope.items, function (x) {
            return angular.extend({}, blade.currentEntity.scopeOriginal, { scope: x.id, label: x.name });
        });
        blade.onChangesConfirmedFn(selection);
        $scope.bladeClose();
    };

    blade.headIcon = 'fas fa-key';
    initializeBlade();
}]);
