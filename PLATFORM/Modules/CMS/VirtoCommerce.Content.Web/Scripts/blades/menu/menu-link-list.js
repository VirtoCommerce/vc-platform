angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.menuLinkListController', ['$scope', 'virtoCommerce.contentModule.menus', 'virtoCommerce.contentModule.stores', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'uuid2', function ($scope, menus, menusStores, bladeNavigationService, dialogService, uuid2) {
    var blade = $scope.blade;
    blade.selectedItemIds = [];

    blade.initialize = function () {
        menusStores.get({ id: blade.choosenStoreId }, function (data) {
            blade.languages = data.languages;
            blade.defaultStoreLanguage = data.defaultLanguage;

            if (blade.newList) {
                blade.currentEntity = { id: uuid2.newguid(), title: undefined, language: blade.defaultStoreLanguage, storeId: blade.choosenStoreId, menuLinks: [] };
                blade.choosenListId = blade.currentEntity.id;
                $scope.blade.toolbarCommands = [{
                    name: "Add link", icon: 'fa fa-plus',
                    executeMethod: function () {
                        var newEntity = { id: uuid2.newguid(), url: undefined, title: undefined, type: undefined, priority: 0, isActive: false, language: undefined, menuLinkListId: blade.choosenListId };
                        blade.currentEntity.menuLinks.push(newEntity);
                        blade.recalculatePriority();
                    },
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: 'content:manage'
                },
				{
				    name: "Save list", icon: 'fa fa-save',
				    executeMethod: function () {
				        blade.saveChanges();
				    },
				    canExecuteMethod: function () {
				        return canSave();
				    },
				    permission: 'content:manage'
				}];

                blade.isLoading = false;
            }
            else {
                blade.isLoading = true;
                menus.getList({ storeId: blade.choosenStoreId, listId: blade.choosenListId }, function (data) {
                    data.menuLinks = _.sortBy(data.menuLinks, 'priority').reverse();
                    blade.origEntity = data;
                    blade.currentEntity = angular.copy(data);
                    blade.isLoading = false;

                    $scope.blade.toolbarCommands = [{
                        name: "Add link", icon: 'fa fa-plus',
                        executeMethod: function () {
                            var newEntity = { id: uuid2.newguid(), url: undefined, title: undefined, isActive: false, priority: 0, menuLinkListId: blade.choosenListId };
                            blade.currentEntity.menuLinks.push(newEntity);
                            blade.recalculatePriority();
                        },
                        canExecuteMethod: function () {
                            return true;
                        },
                        permission: 'content:manage'
                    },
					{
					    name: "Save list", icon: 'fa fa-save',
					    executeMethod: function () {
					        blade.saveChanges();
					    },
					    canExecuteMethod: function () {
					        return canSave();
					    },
					    permission: 'content:manage'
					},
					{
					    name: "Reset list", icon: 'fa fa-undo',
					    executeMethod: function () {
					        angular.copy(blade.origEntity, blade.currentEntity);
					    },
					    canExecuteMethod: function () {
					        return !angular.equals(blade.origEntity, blade.currentEntity);
					    },
					    permission: 'content:manage'
					},
					{
					    name: "Delete list", icon: 'fa fa-trash-o',
					    executeMethod: function () {
					        blade.deleteList();
					    },
					    canExecuteMethod: function () {
					        return true;
					    },
					    permission: 'content:manage'
					},
					{
					    name: "Delete links", icon: 'fa fa-trash-o',
					    executeMethod: function () {
					        blade.deleteLinks();
					    },
					    canExecuteMethod: function () {
					        return blade.selectedItemIds.length > 0;
					    },
					    permission: 'content:manage'
					}];
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    $scope.selected = function (id) {
        return _.contains(blade.selectedItemIds, id);
    }

    blade.initialize();

    blade.saveChanges = function () {
        //checkForNull();
        blade.isLoading = true;
        menus.checkList({ storeId: blade.choosenStoreId, id: blade.currentEntity.id, name: blade.currentEntity.name, language: blade.currentEntity.language }, function (data) {
            if (Boolean(data.result)) {
                menus.update({ storeId: blade.choosenStoreId }, blade.currentEntity, function (data) {
                    blade.newList = false;
                    blade.initialize();
                    blade.parentBlade.initialize();
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
            }
            else {
                blade.isLoading = false;
                var dialog = {
                    id: "errorInName",
                    title: "Name not unique",
                    message: "Name must be unique for this language!",
                    callback: function (remove) {

                    }
                }
                dialogService.showNotificationDialog(dialog);
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    function canSave() {
        var listNameIsRight = !((angular.isUndefined(blade.currentEntity.name)) || (blade.currentEntity.name === null)) && !angular.equals(blade.currentEntity, blade.origEntity);
        var linksIsRight = blade.currentEntity.menuLinks.length == _.reject(
				blade.currentEntity.menuLinks,
				function (link) {
				    return !(!(angular.isUndefined(link.title) || link.title === null) &&
						!(angular.isUndefined(link.url) || link.url === null));
				}).length;
        return listNameIsRight && linksIsRight && blade.currentEntity.menuLinks.length > 0;
    }

    blade.deleteList = function () {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete this link list?",
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;

                    menus.delete({ storeId: blade.choosenStoreId, listId: blade.choosenListId }, function () {
                        $scope.bladeClose();
                        blade.parentBlade.initialize();
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.deleteLinks = function () {
        var dialog = {
            id: "confirmDelete",
            title: "Delete confirmation",
            message: "Are you sure you want to delete this links?",
            callback: function (remove) {
                if (remove) {
                    for (var i = 0; i < blade.selectedItemIds.length; i++) {
                        blade.currentEntity.menuLinks = _.reject(
							blade.currentEntity.menuLinks,
							function (link) {
							    return link.id == blade.selectedItemIds[i];
							});
                    }
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    blade.selectItem = function (id) {
        if (_.contains(blade.selectedItemIds, id)) {
            blade.selectedItemIds = _.reject(blade.selectedItemIds, function (linkId) {
                return linkId === id;
            });
        }
        else {
            blade.selectedItemIds.push(id);
        }
    }

    blade.recalculatePriority = function () {
        for (var i = 0; i < blade.currentEntity.menuLinks.length; i++) {
            blade.currentEntity.menuLinks[i].priority = (blade.currentEntity.menuLinks.length - 1 - i) * 10;
        }
    }

    $scope.deleteLink = function (data) {
        blade.currentEntity.menuLinks = _.reject(
				blade.currentEntity.menuLinks,
				function (link) {
				    return link.id == data.id;
				});
    };

    blade.getFlag = function (lang) {
        switch (lang) {
            case 'ru-RU':
                return 'ru';

            case 'en-US':
                return 'us';

            case 'fr-FR':
                return 'fr';

            case 'zh-CN':
                return 'ch';

            case 'ru-RU':
                return 'ru';

            case 'ja-JP':
                return 'jp';

            case 'de-DE':
                return 'de';
        }
    }

    $scope.blade.headIcon = 'fa fa-archive';



    $scope.sortableOptions = {
        update: function (e, ui) {

        },
        stop: function (e, ui) {
            blade.recalculatePriority();
        }
    };
}]);
