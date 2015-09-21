angular.module('platformWebApp')
.directive('vaPermission', ['platformWebApp.authService', function (authService) {
	return {
		link: function (scope, element, attrs) {
			if (attrs.vaPermission) {
				var permissionValue = attrs.vaPermission.trim();
				var notPermissionFlag = permissionValue[0] === '!';
				if (notPermissionFlag) {
					permissionValue = permissionValue.slice(1).trim();
				}

				function toggleVisibilityBasedOnPermission() {
					var hasPermission = authService.checkPermission(permissionValue);

					if (hasPermission && !notPermissionFlag || !hasPermission && notPermissionFlag)
						element.show();
					else
						element.hide();
				}

				toggleVisibilityBasedOnPermission();
				scope.$on('loginStatusChanged', toggleVisibilityBasedOnPermission);
			}
		}
	};
}]);