angular.module('platformWebApp')
.factory('mainMenuService', ['$filter', function ($filter) {

	var menuItems = [];

	function sortByParentFirst(a, b) {
		return a.path.split('/').length - b.path.split('/').length; 
	};
	function sortByPriority(a, b) {
			if (angular.isDefined(a.priority) && angular.isDefined(b.priority)) {
				return a.priority -b.priority;
			}
			return -1;
	};
	function constructTree() {

		//clear arrays
		//menuTree.splice(0, menuTree.length)
		//console.log('------------------constructTree---------------------');
		angular.forEach(menuItems.sort(sortByParentFirst), function (x) {
			//console.log(x.path);
			if (!angular.isDefined(x.parent)) {
				x.parent = null;
			}
			if (!angular.isDefined(x.children)) {
				x.children = [];
			}
			if (x.parent == null) {
				var pathParts = x.path.split('/');
				var path = x.path;
				var parentPath = null;
				if (pathParts.length > 1) {
					pathParts.pop();
					parentPath = pathParts.join('/');
				}

				var parent = _.find(menuItems, function (y) { return y.path == parentPath });
				if (angular.isDefined(parent)) {
					x.parent = parent;
					parent.children.push(x);
				}
			}
		});
		//sort tree items
		menuItems.sort(sortByPriority);
	};


	function findByPath(path) {
		return _.find(menuItems, function (x) { return x.path == path; });
	};

	function addMenuItem(menuItem) {
		menuItems.push(menuItem);
		constructTree();
	}

	var retVal = {
		menuItems: menuItems,
		addMenuItem: addMenuItem,
		findByPath: findByPath
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
			scope.menuItems = mainMenuService.menuItems;
		
			scope.selectMenuItem = function (menuItem) {
				scope.currentMenuItem = menuItem;
				scope.showSubMenu = angular.isDefined(menuItem.children) && menuItem.children.length > 0;
		
				//run action
				if (angular.isDefined(menuItem.action)) {
					menuItem.action();
				}
			};

		}
	}
}]);

