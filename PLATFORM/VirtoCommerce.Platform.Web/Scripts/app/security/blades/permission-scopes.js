angular.module('platformWebApp')
.controller('platformWebApp.permissionScopesController', ['$q', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.permissionScopeResolver', function ($q, $scope, bladeNavigationService, dialogService, permissionScopeResolver) {
    var blade = $scope.blade;
    
    function initializeBlade() {
        var tmpScopes = [];
        _.each(blade.permission.availableScopes, function (x) {
            var resolvedScope = permissionScopeResolver.resolve(x.type);
            if (resolvedScope) {
                resolvedScope.scopeOriginal = x.scope;

                resolvedScope.scopes = resolvedScope.hasConstantConfiguration ?
                    [x.scope] :
                    _.filter(blade.permission.scopes, function (y) {
                        return y.lastIndexOf(resolvedScope.scopeOriginal, 0) === 0;
                    });

                resolvedScope.$selected = _.any(blade.permission.scopes, function (y) { return y.lastIndexOf(resolvedScope.scopeOriginal, 0) === 0; });
                tmpScopes.push(resolvedScope);
            }
        });
        $scope.availableScopes = tmpScopes;
        blade.isLoading = false;
    }

    blade.selectNode = function (node) {
        if (node.selectFn) {
            $scope.selectedNodeId = node.type;
            node.selectFn(blade, function (scopes) {
                node.scopes = scopes;
                node.$selected = true;
            });
        } else {
            $scope.selectedNodeId = undefined;
            node.$selected = !node.$selected;
        }
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    };

    $scope.isValid = function () {
        return true;
    };

    $scope.saveChanges = function () {
        blade.permission.scopes = _.flatten(_.map(_.where($scope.availableScopes, { $selected: true }), function (x) {
            return x.scopes ? x.scopes : x.scopeOriginal;
        }));

        $scope.bladeClose();
    };

    blade.headIcon = 'fa-key';
    initializeBlade();
}]);