angular.module('virtoCommerce.marketingModule')
.controller('addContentItemsElementController', ['$scope', 'bladeNavigationService', 'categories', 'items', function ($scope, bladeNavigationService, categories, items) {
	var blade = $scope.blade;

	blade.addFolder = function () {
		var data = { id: '78-77', name: '', description: '', parentId: blade.choosenFolder, items: [] };
		blade.parentBlade.addNewFolder(data);
	};

	blade.addContentItem = function () {
		var data = { id: '77-78', name: '', description: '', contentType: 'CategoryWithImages', categoryId: '', imageUrl: '', externalImageUrl: '', message: '', categoryCode: '', title: '', sortField: '', itemCount: 0, newItems: false, flashFilePath: '', link1Url: '', link2Url: '', link3Url: '', rawHtml: '', razorHtml: '', alternativeText: '', targetUrl: '', productCode: '', parentId: blade.choosenFolder };
		blade.parentBlade.addNewContentItem(data);
	};

	$scope.blade.isLoading = false;
}]);
