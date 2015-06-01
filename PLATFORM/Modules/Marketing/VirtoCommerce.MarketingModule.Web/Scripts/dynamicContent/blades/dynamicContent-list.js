angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.dynamicContentListController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
    var blade = $scope.blade;

	$scope.selectedNodeId = null;

	function initializeBlade() {
		var entities = [
            { id: 'DC_1', name: 'Content items', subname: 'Content items list', description: 'Content items description', icon: 'fa-inbox', entityName: 'items' },
            { id: 'DC_2', name: 'Content placeholders', subname: 'Content placeholders list', description: 'Placeholders description', icon: 'fa-location-arrow', entityName: 'placeholders' },
            { id: 'DC_3', name: 'Content publishing', subname: 'Content publishing list', description: 'Publishing description', icon: 'fa-paperclip', entityName: 'publishing' }];
		blade.currentEntities = entities;
		blade.isLoading = false;
	};

	blade.openBlade = function (data) {
		$scope.selectedNodeId = data.id;

		var newBlade = {
			id: 'dynamicContentList',
			title: data.name,
			subtitle: 'Marketing service',
			controller: 'virtoCommerce.marketingModule.' + data.entityName + 'DynamicContentListController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/' + data.entityName + '/list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	$scope.blade.headIcon = 'fa fa-calendar-o';

	initializeBlade();
}]);
