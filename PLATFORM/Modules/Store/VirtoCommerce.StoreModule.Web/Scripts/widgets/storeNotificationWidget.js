angular.module('virtoCommerce.storeModule')
.controller('virtoCommerce.storeModule.storeNotificationWidgetController', ['$scope', '$state', function ($scope, $state) {
	var blade = $scope.widget.blade;

	blade.showNotifications = function () {
		$state.go('workspace.newnotifications', { objectId: blade.currentEntity.id, objectTypeId: 'Store' });
	};
}]);