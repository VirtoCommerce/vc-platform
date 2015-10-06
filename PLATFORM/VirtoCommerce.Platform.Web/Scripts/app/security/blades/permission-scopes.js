angular.module('platformWebApp')
.controller('platformWebApp.permissionScopesController', ['$q', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.permissionScopeResolver', function ($q, $scope, bladeNavigationService, dialogService, permissionScopeResolver) {
    var blade = $scope.blade;
    

    function initializeBlade() {
        var tmpScopes = [];
        _.each(blade.permission.availableScopes, function (x) {
            var resolvedScope = permissionScopeResolver.resolve(x.type);
            if (resolvedScope) {
            	resolvedScope.scopeOriginal = x;
            	resolvedScope.assignedScopes = _.filter(blade.permission.assignedScopes, function (y) {
            		return y.type.toLowerCase() == x.type.toLowerCase();
                    });

            	resolvedScope.assignedScopesLabels = getNodeAssignedScopesLabels(resolvedScope);
            	resolvedScope.$selected = resolvedScope.assignedScopes.length > 0;
            	tmpScopes.push(resolvedScope);

            }
        });
        $scope.availableScopes = tmpScopes;
        blade.isLoading = false;
    }

    blade.selectNode = function (node) {
        if (node.selectFn) {
        	$scope.selectedNodeId = node.type;
        	if (node.selectFn) {
        		node.selectFn(blade, function (assignedScopes) {
        			node.assignedScopes = assignedScopes;
        			node.$selected = true;
        			node.assignedScopesLabels = getNodeAssignedScopesLabels(node);
        		});
        	}
        } else {
            $scope.selectedNodeId = undefined;
            node.$selected = !node.$selected;
        }
    };

    function getNodeAssignedScopesLabels(node) {
    	return _.map(node.assignedScopes, function (x) { return angular.isDefined(x.label) ? x.label : ''; });
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    };

    $scope.isValid = function () {
        return true;
    };

    $scope.saveChanges = function () {
    	blade.permission.assignedScopes = _.flatten(_.map(_.where($scope.availableScopes, { $selected: true }), function (x) {
    		return x.assignedScopes.length > 0 ? x.assignedScopes : [ x.scopeOriginal ];
    	}));
 
        $scope.bladeClose();
    };

    blade.headIcon = 'fa-key';
    initializeBlade();
}]);