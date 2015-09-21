angular.module('platformWebApp')
.factory('platformWebApp.securityRoleScopeService', [function () {

	var allScopeGetters = [];
	
	function registerScopeGetter(getScopeFn) {
		allScopeGetters.push(getScopeFn);
	}

	var retVal = {
		allScopeGetters: allScopeGetters,
		registerScopeGetter: registerScopeGetter
	};
	return retVal;
}])