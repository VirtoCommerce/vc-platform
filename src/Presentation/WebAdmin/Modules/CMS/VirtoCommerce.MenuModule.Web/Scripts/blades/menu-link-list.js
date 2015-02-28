angular.module('virtoCommerce.content.menuModule.blades.menuLinkList', [
    'virtoCommerce.content.menuModule.resources.menus',
	'angularUUID2'
])
.controller('menuLinkListController', ['$scope', 'menus', 'bladeNavigationService', 'dialogService', 'uuid2', function ($scope, menus, bladeNavigationService, dialogService, uuid2) {
	blade = $scope.blade;

	blade.refresh = function () {
		if (blade.newList) {
			blade.currentEntity = { id: uuid2.newguid(), name: null, storeId: blade.choosenStoreId, menuLinks: [] };
			blade.choosenListId = blade.currentEntity.id;
			$scope.bladeToolbarCommands = [{
				name: "Add", icon: 'fa fa-plus',
				executeMethod: function () {
					var newEntity = { id: uuid2.newguid(), link: null, name: null, menuLinkListId: blade.choosenListId };
					blade.currentEntity.menuLinks.push(newEntity);
				},
				canExecuteMethod: function () {
					return true;
				},
			},

			{
				name: "Save", icon: 'fa fa-save',
				executeMethod: function () {
					saveChanges();
				},
				canExecuteMethod: function () {
					return canSave();
				}
			}];

			blade.isLoading = false;
		}
		else {
			blade.isLoading = true;
			menus.getList({ storeId: blade.choosenStoreId, listId: blade.choosenListId }, function (data) {
				blade.origEntity = data;
				blade.currentEntity = angular.copy(data);
				blade.isLoading = false;

				$scope.bladeToolbarCommands = [{
					name: "Add", icon: 'fa fa-plus',
					executeMethod: function () {
						var newEntity = { id: uuid2.newguid(), link: null, name: null, menuLinkListId: blade.choosenListId };
						blade.currentEntity.menuLinks.push(newEntity);
					},
					canExecuteMethod: function () {
						return true;
					}
				},

				{
					name: "Save", icon: 'fa fa-save',
					executeMethod: function () {
						saveChanges();
					},
					canExecuteMethod: function () {
						return canSave();
					}
				},

				{
					name: "Reset", icon: 'fa fa-undo',
					executeMethod: function () {
						angular.copy(blade.origEntity, blade.currentEntity);
					},
					canExecuteMethod: function () {
						return !angular.equals(blade.origEntity, blade.currentEntity);
					}
				},

				{
					name: "Delete", icon: 'fa fa-trash-o',
					executeMethod: function () {
						deleteList();
					},
					canExecuteMethod: function () {
						return true;
					}
				}];
			});
		}
	}

	blade.refresh();

	function saveChanges() {
		//checkForNull();
		blade.isLoading = true;
		menus.update({ storeId: blade.choosenStoreId }, blade.currentEntity, function (data) {
			blade.newList = false;
			blade.refresh();
			blade.parentBlade.refresh();
		});
	};

	function canSave() {
		var listNameIsRight = !((angular.isUndefined(blade.currentEntity.name)) || (blade.currentEntity.name === null)) && !angular.equals(blade.currentEntity, blade.origEntity);
		var linksIsRight = blade.currentEntity.menuLinks.length == _.reject(
				blade.currentEntity.menuLinks,
				function (link) {
					return !(!(angular.isUndefined(link.name) || link.name === null) &&
						!(angular.isUndefined(link.link) || link.link === null));
				}).length;
		return listNameIsRight && linksIsRight && blade.currentEntity.menuLinks.length > 0;
	}

	function deleteList() {
		var dialog = {
			id: "confirmDelete",
			title: "Delete confirmation",
			message: "Are you sure you want to delete this link list?",
			callback: function (remove) {
				if (remove) {
					blade.isLoading = true;

					menus.delete({ storeId: blade.choosenStoreId, listId: blade.choosenListId }, function () {
						$scope.bladeClose();
						blade.parentBlade.refresh();
					});
				}
			}
		}
		dialogService.showConfirmationDialog(dialog);
	}

	$scope.deleteLink = function (data) {
		blade.currentEntity.menuLinks = _.reject(
				blade.currentEntity.menuLinks,
				function (link) {
					return link.id == data.id;
				});
	};



	//$scope.blade.refresh = function (parentRefresh) {
	//	if (parentRefresh) {
	//		$scope.blade.isLoading = true;
	//		$scope.blade.parentBlade.refresh().$promise.then(function (results) {
	//			var data = _.find(results, function (x) { return x.id === $scope.blade.data.id; });
	//			initializeBlade(data);
	//		});
	//	} else {
	//		initializeBlade($scope.blade.data);
	//	}
	//}

	//function initializeBlade(data) {
	//	$scope.blade.currentEntity = angular.copy(data);
	//	$scope.blade.origEntity = data;
	//	$scope.blade.isLoading = false;
	//};

	

	////function isItemsChecked() {
	////    return $scope.blade.currentEntity && _.any($scope.blade.currentEntity.prices, function (x) { return x.selected; });
	////}

	//function deleteChecked() {
	//	// var selection = _.where($scope.blade.currentEntity.prices, { selected: true });
	//	var selection = [$scope.selectedItem];
	//	_.each(selection, function (item) {
	//		$scope.blade.currentEntity.prices.splice($scope.blade.currentEntity.prices.indexOf(item), 1);
	//	});
	//}

	//$scope.selectItem = function (listItem) {
	//	$scope.selectedItem = listItem;
	//};

	//$scope.blade.onClose = function (closeCallback) {
	//	closeChildrenBlades();

	//	if (isDirty()) {
	//		var dialog = {
	//			id: "confirmItemChange",
	//			title: "Save changes",
	//			message: "The Prices information has been modified. Do you want to save changes?"
	//		};
	//		dialog.callback = function (needSave) {
	//			if (needSave) {
	//				saveChanges();
	//			}
	//			closeCallback();
	//		};
	//		dialogService.showConfirmationDialog(dialog);
	//	}
	//	else {
	//		closeCallback();
	//	}
	//};

	//function closeChildrenBlades() {
	//	angular.forEach($scope.blade.childrenBlades.slice(), function (child) {
	//		bladeNavigationService.closeBlade(child);
	//	});
	//}

	//function isDirty() {
	//	return !angular.equals($scope.blade.currentEntity, $scope.blade.origEntity);
	//};

	//$scope.checkAll = function (selected) {
	//	angular.forEach($scope.blade.currentEntity.prices, function (item) {
	//		item.selected = selected;
	//	});
	//};
}]);
