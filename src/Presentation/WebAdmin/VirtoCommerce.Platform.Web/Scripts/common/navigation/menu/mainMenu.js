angular.module('platformWebApp.mainMenu', [
])
.factory('mainMenuService', ['$filter', function ($filter) {

	var retVal = {
		menuItems: [],
		addMenuItem: function (menuItem) {
			this.menuItems.push(menuItem);
			this.menuItems.sort(function (a, b) {
				if (angular.isDefined(a.priority) && angular.isDefined(b.priority)) {
					return a.priority - b.priority;
				}
				return -1;
			});
		}
	};
	return retVal;
}])
.directive('vaMainMenu', ['$compile', '$filter', '$state', 'mainMenuService', function ($compile, $filter, $state, mainMenuService) {

	return {
		restrict: 'E',
		replace: true,
		templateUrl: 'Scripts/common/navigation/menu/mainMenu.tpl.html',
		link: function (scope, element, attr) {

			scope.currentMenuItem = undefined;
			scope.menuItems = [];
		
			function prepareMenu() {
				angular.forEach(mainMenuService.menuItems, function (menuItem) {
					var topMenu = $filter('filter')(scope.menuItems, function (value) { return value.group == menuItem.group })[0];
					if (!angular.isDefined(topMenu)) {
						topMenu = angular.copy(menuItem);
						topMenu.subMenuItems = [];
						scope.menuItems.push(topMenu);
					};
					topMenu.subMenuItems.push(menuItem);
				});
			};

			scope.selectMenuItem = function (menuItem) {
				if (angular.isDefined(menuItem.subMenuItems) && menuItem.subMenuItems.length > 1) {
					scope.currentMenuItem = menuItem;
					scope.showSubMenu = true;
				}
				else
				{
					scope.showSubMenu = false;
					$state.go(menuItem.state);
				}
			};

			prepareMenu();
		}
	}
}]);

