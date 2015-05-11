angular.module('virtoCommerce.catalogModule')
.controller('virtoCommerce.catalogModule.importJobProgressController', ['$scope', '$interval', 'platformWebApp.bladeNavigationService', 'virtoCommerce.catalogModule.imports', function ($scope, $interval, bladeNavigationService, imports) {
	$scope.blade.refresh = function () {

		imports.get({ id: $scope.blade.job.id }, function (data) {
			$scope.blade.isLoading = data.progressStatus == 'Running';
			$scope.job = data;
		});

	};

	function stopRefresh() {
		if (angular.isDefined(intervalPromise)) {
			$interval.cancel(intervalPromise);
		}
	};

	$scope.$on('$destroy', function () {
		// Make sure that the interval is destroyed too
		stopRefresh();
	});

	$scope.blade.refresh();
	var intervalPromise = $interval($scope.blade.refresh, 1500);
}]);
