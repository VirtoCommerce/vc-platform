angular.module('platformWebApp')
.factory('platformWebApp.permissionScopeResolver', [ function () {
	var scopes = [];
	//type - permission scope type
	//title - scope display name
	//permission - original permission object returned from api
	//selectFn - function called when user select scope in UI
	function register(scope) {
		scopes.push(scope);
	}

	function resolve(type) {
	    return angular.copy(_.find(scopes, function (x) { return x.type.toUpperCase() == type.toUpperCase(); }));
	}

	var retVal = {
		register: register,
		resolve: resolve
	};
	return retVal;
}]);