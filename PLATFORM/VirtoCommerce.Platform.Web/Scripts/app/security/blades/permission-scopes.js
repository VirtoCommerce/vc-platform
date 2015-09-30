angular.module('platformWebApp')
.controller('platformWebApp.permissionScopesController', ['$q', '$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.permissionScopeResolver',  function ($q, $scope, bladeNavigationService, dialogService, permissionScopeResolver) {

	$scope.blade.isLoading = false;
	$scope.availableScopes = _.filter(_.map($scope.blade.permission.availableScopes, function (x) {
		var resolvedScope = permissionScopeResolver.resolve(x.type);
		if(resolvedScope)
		{
			resolvedScope.origScope = x;
		}
		return resolvedScope;
	}), function (x) { return angular.isDefined(x) && x != null; });

	
	$scope.blade.selectNode = function (node) {
	
		if (node.selectFn)
		{
			node.selectFn(node, function (choosedScopes) {
				node.choosedScopes = choosedScopes;
			});
		}
	};

	$scope.cancelChanges = function () {
		$scope.bladeClose();
	};

	$scope.isValid = function () {
		return true;
	};

	$scope.saveChanges = function () {
		$scope.blade.permission.scopes = _.flatten(_.map(_.where($scope.availableScopes, { $selected: true }), function (x) {
			if(!x.choosedScopes)
			{
				return x.origScope.scope;
			}
			return x.choosedScopes;
		}));
		$scope.bladeClose();
	};

	$scope.blade.headIcon = 'fa-key';

}]);