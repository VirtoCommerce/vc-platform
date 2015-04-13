angular.module('virtoCommerce.marketingModule')
.controller('addPublishingFirstStepController', ['$scope', 'bladeNavigationService', function ($scope, bladeNavigationService) {
	$scope.setForm = function (form) {
		$scope.formScope = form;
	}

	var blade = $scope.blade;

	blade.initializeBlade = function () {
		blade.entity = {
			id: 'First',
			name: 'First',
			description: 'First',
			priority: 0,
			isActive: true,
			startDate: new Date().getTime(),
			endDate: new Date().getTime(),
			contentItems: [],
			contentPlaces: []
		};

		blade.isLoading = false;
	}

	blade.nextStep = function () {
		bladeNavigationService.closeBlade(blade);

		var newBlade = {
			id: 'listItemChild',
			title: 'Edit publising element',
			subtitle: 'Edit publising element',
			entity: blade.entity,
			isNew: false,
			controller: 'addPublishingSecondStepController',
			template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/publishing/add-second-step.tpl.html'
		};
		bladeNavigationService.showBlade(newBlade, blade.parentBlade);
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