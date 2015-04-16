angular.module('virtoCommerce.marketingModule')
.controller('addPlaceholderElementController', ['$scope', 'bladeNavigationService', 'categories', 'items', function ($scope, bladeNavigationService, categories, items) {
	var blade = $scope.blade;

	blade.addFolder = function () {
		var data = { outline: '', name: '', description: '', parentFolderId: blade.choosenFolder};
		blade.parentBlade.addNewFolder(data);
	};

	blade.addPlaceholder = function () {
		var data = { name: '', description: '', imageUrl: 'http://mini.s-shot.ru/1024x768/JPEG/1024/Z100/?kitmall.ru', folderId: blade.choosenFolder };
		blade.parentBlade.addNewPlaceholder(data);
	};

	$scope.blade.isLoading = false;
}]);
