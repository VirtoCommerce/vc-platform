angular.module('platformWebApp.mainMenu', [
])
.factory('mainMenuService', ['$filter', function ($filter) {

	var menuItems = [];
	var menuTree = [];

	function constructTree() {

		//clear arrays
		menuTree.splice(0, menuTree.length)
		//console.log('------------------constructTree---------------------');
		angular.forEach(menuItems.sort(function (a, b) { return a.path.split('/').length - b.path.split('/').length; }), function (x) {
			//console.log(x.path);
			var copy = angular.copy(x);
			copy.parent = null;
			copy.children = [];
			menuTree.push(copy);

			var pathParts = x.path.split('/');
			var path = x.path;
			var parentPath = null;
			if (pathParts.length > 1) {
				pathParts.pop();
				parentPath = pathParts.join('/');
			}

			var parent = _.find(menuTree, function (y) { return y.path == parentPath });
			if (angular.isDefined(parent)) {
				copy.parent = parent;
				parent.children.push(copy);
			}
		});
		//sort tree items
		menuTree.sort(function (a, b) {
			if (angular.isDefined(a.priority) && angular.isDefined(b.priority)) {
				return a.priority - b.priority;
			}
			return -1;
		});
	};


	function clearByPath(path) {
		menuItems = _.filter(menuItems, function (x) { return x.path.substring(0, path.length) != path; });
		constructTree();
	};

	function addMenuItem(menuItem) {
		menuItems.push(menuItem);
		constructTree();
	}

	function addMenuItems(bulkItems) {
		angular.forEach(bulkItems, function (x) {
			menuItems.push(x);
		});
		constructTree();
	}

	var retVal = {
		menuItems: menuItems,
		menuTree: menuTree,
		addMenuItem: addMenuItem,
		addMenuItems: addMenuItems,
		clearByPath: clearByPath
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

			scope.getRootLevelMenuItems = function () {
				return _.where(mainMenuService.menuTree, { parent: null });
			};

			scope.selectMenuItem = function (menuItem) {
				if (angular.isDefined(menuItem.children) && menuItem.children.length > 0) {
					scope.currentMenuItem = menuItem;
					scope.showSubMenu = true;
					//Custom action
					if (angular.isDefined(menuItem.customAction)) {
						menuItem.customAction();
					}
				}
				else if (angular.isDefined(menuItem.state)) {
					scope.showSubMenu = false;
					//Navigate to state
					$state.go(menuItem.state, menuItem.stateParams);
				}
			};

		}
	}
}]);

