angular.module('virtoCommerce.marketingModule')
.controller('publishingDynamicContentListController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	var blade = $scope.blade;
	blade.currentEntity = undefined;
	blade.currentEntities = [];

	$scope.selectedNodeId = null;

	blade.initialize = function () {
		blade.isLoading = false;
	};

	blade.addNewPublishing = function () {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'add_publishing_element',
			title: 'New publising element',
			subtitle: 'New publising element',
			isNew: true,
			controller: 'addPublishingFirstStepController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/publishing/publishing-main-step.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.editPublising = function (data) {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'edit_publishing_element',
			title: 'Edit publising element',
			subtitle: 'Edit publising element',
			entity: data,
			isNew: false,
			controller: 'addPublishingController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/publishing-main-step.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.closeChildrenBlades = function () {
		angular.forEach(blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	blade.publishingClick = function (data) {
		blade.currentEntity = data;
	}

	blade.publishingCheck = function (data) {
		return angular.equals(data, blade.currentEntity);
	}

	blade.deleteLink = function (data) {

	}

	$scope.bladeToolbarCommands = [
        {
        	name: "Refresh", icon: 'fa fa-refresh',
        	executeMethod: function () {
        		$scope.blade.refresh();
        	},
        	canExecuteMethod: function () {
        		return true;
        	}
        },
        {
        	name: "Add", icon: 'fa fa-plus',
        	executeMethod: function () {
        		blade.addNewPublishing();
        	},
        	canExecuteMethod: function () {
        		return true;
        	}
        }
	];

	$scope.bladeHeadIco = 'fa fa-flag';

	blade.testData = function () {
		blade.currentEntities.push(
			{
				id: 'First',
				name: 'First',
				description: 'First',
				priority: 0,
				isActive: true,

				startDate: null,
				endDate: null,

				contentItems: [
					{ id: Math.floor((Math.random() * 1000000000) + 1).toString(), name: 'Slider', description: 'Slider', contentType: 'CategoryWithImages', categoryId: 'Slider', imageUrl: 'Slider', externalImageUrl: 'Slider', message: 'Slider', categoryCode: '', title: '', sortField: '', itemCount: 1, newItems: false, flashFilePath: '', link1Url: '', link2Url: '', link3Url: '', rawHtml: '', razorHtml: '', alternativeText: '', targetUrl: '', productCode: '', parentId: 'Main' }
				],
				contentPlaces: [
					{ id: 'Main-Default-Slider', name: 'Main-Default-Slider', description: 'Main-Default-Slider', descriptionImageUrl: 'http://mini.s-shot.ru/1024x768/JPEG/1024/Z100/?kitmall.ru', parentId: blade.choosenFolder }
				],
			});
	}

	blade.testData();
	blade.initialize();
}]);
