angular.module('virtoCommerce.marketingModule')
.controller('addPublishingFirstStepController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

	var blade = $scope.blade;

	blade.entity = {
		id: '',
		name: '',
		description: '',
		priority: 0,
		isActive: true,
		startDate: '',
		endDate: '',
		contentItems: [],
		contentPlaces: []
	};

	blade.initializeBlade = function () {
		blade.isLoading = false;

		if (blade.isNew) {
			blade.entity = {
				id: '',
				name: '',
				description: '',
				priority: 0,
				isActive: true,
				startDate: '',
				endDate: '',
				contentItems: [],
				contentPlaces: []
			};
		}
	}

	blade.addPlaceholders = function () {
		var newBlade = {
			id: 'publishing_add_placeholders',
			title: 'Add placeholders elements',
			subtitle: 'Add placeholders elements',
			entity: blade.entity,
			controller: 'addPublishingPlaceholdersStepController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/publishing/add-placeholders.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.addContentItems = function () {
		blade.closeChildrenBlades();

		var newBlade = {
			id: 'publishing_add_content_items',
			title: 'Add content items elements',
			subtitle: 'Add content items elements',
			entity: blade.entity,
			controller: 'addPublishingContentItemsStepController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/publishing/add-content-items.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, $scope.blade);
	}

	blade.closeChildrenBlades = function () {
		angular.forEach(blade.childrenBlades.slice(), function (child) {
			bladeNavigationService.closeBlade(child);
		});
	}

	blade.saveChanges = function () {
		
	}

	blade.availableSave = function () {
		return !$scope.formScope.$invalid &&
			blade.entity.contentItems.length > 0 &&
			blade.entity.contentPlaces.length > 0;
	}

	// datepicker
	$scope.datepickers = {
		endDate: false,
		startDate: false,
	}

	$scope.showWeeks = true;
	$scope.toggleWeeks = function () {
		$scope.showWeeks = !$scope.showWeeks;
	};

	$scope.clear = function () {
		$scope.blade.currentEntity.birthDate = null;
	};
	$scope.today = new Date();

	$scope.open = function ($event, which) {
		$event.preventDefault();
		$event.stopPropagation();

		$scope.datepickers[which] = true;
	};

	$scope.dateOptions = {
		'year-format': "'yyyy'",
		'starting-day': 1
	};

	$scope.formats = ['shortDate', 'dd-MMMM-yyyy', 'yyyy/MM/dd'];
	$scope.format = $scope.formats[0];

	blade.initializeBlade();
}]);