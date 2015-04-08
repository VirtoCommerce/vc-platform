angular.module('virtoCommerce.marketingModule')
.controller('dynamicContentListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.blade

	$scope.selectedNodeId = null;

	function initializeBlade() {
		var entities = [
            { id: 'DC_1', name: 'Content items', subname: 'Content items list', entityName: 'items' },
            { id: 'DC_2', name: 'Content placeholders', subname: 'Content items list', entityName: 'placeholders' },
            { id: 'DC_3', name: 'Content publishing', subname: 'Content items list', entityName: 'publishing' }];
		blade.currentEntities = entities;
		blade.isLoading = false;
	};

	blade.openBlade = function (data) {
		$scope.selectedNodeId = data.id;

		var newBlade = {
			id: 'marketingDetails',
			title: data.name,
			subtitle: 'Marketing service',
			controller: data.entityName + 'DynamicContentListController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/' + data.entityName + '/list.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade);
	}

	$scope.bladeHeadIco = 'fa fa-flag';

	initializeBlade();
}]);
