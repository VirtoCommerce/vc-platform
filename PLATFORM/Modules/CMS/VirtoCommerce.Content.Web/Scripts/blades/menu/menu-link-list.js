angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.menuLinkListController', ['$rootScope', '$scope', 'virtoCommerce.contentModule.menus', 'virtoCommerce.storeModule.stores', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'virtoCommerce.contentModule.menuLinkList-associationTypesService', function ($rootScope, $scope, menus, menusStores, bladeNavigationService, dialogService, associationTypesService) {
    var blade = $scope.blade;
    blade.updatePermission = 'content:update';
    blade.selectedItemIds = [];

    blade.initialize = function () {
        menusStores.get({ id: blade.chosenStoreId }, function (data) {
            blade.languages = data.languages;
            blade.defaultStoreLanguage = data.defaultLanguage;

            if (blade.newList) {
                blade.currentEntity = { title: undefined, language: blade.defaultStoreLanguage, storeId: blade.chosenStoreId, menuLinks: [] };
                blade.chosenListId = blade.currentEntity.id;
                $scope.blade.toolbarCommands = [{
                    name: "content.commands.add-link", icon: 'fa fa-plus',
                    executeMethod: function () {
                        var newEntity = { url: undefined, title: undefined, type: undefined, priority: 0, isActive: false, language: undefined, menuLinkListId: blade.chosenListId };
                        blade.currentEntity.menuLinks.push(newEntity);
                        blade.recalculatePriority();
                    },
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: blade.updatePermission
                },
				{
				    name: "content.commands.save-list", icon: 'fa fa-save',
				    executeMethod: blade.saveChanges,
				    canExecuteMethod: canSave,
				    permission: blade.updatePermission
				}];

                blade.isLoading = false;
            }
            else {
                blade.isLoading = true;
                menus.getList({ storeId: blade.chosenStoreId, listId: blade.chosenListId }, function (data) {
                    _.each(data.menuLinks, function (x) {
                        if (x.associatedObjectType) {
                            x.associatedObject = _.findWhere($scope.associatedObjectTypes, { id: x.associatedObjectType });
                        }
                    });
                    data.menuLinks = _.sortBy(data.menuLinks, 'priority').reverse();
                    blade.origEntity = data;
                    blade.currentEntity = angular.copy(data);
                    blade.isLoading = false;

                    $scope.blade.toolbarCommands = [{
                        name: "content.commands.add-link", icon: 'fa fa-plus',
                        executeMethod: function () {
                            var newEntity = { url: undefined, title: undefined, isActive: false, priority: 0, menuLinkListId: blade.chosenListId };
                            blade.currentEntity.menuLinks.push(newEntity);
                            blade.recalculatePriority();
                        },
                        canExecuteMethod: function () { return true; },
                        permission: blade.updatePermission
                    },
					{
					    name: "content.commands.save-list", icon: 'fa fa-save',
					    executeMethod: blade.saveChanges,
					    canExecuteMethod: canSave,
					    permission: blade.updatePermission
					},
					{
					    name: "content.commands.reset-list", icon: 'fa fa-undo',
					    executeMethod: function () {
					        angular.copy(blade.origEntity, blade.currentEntity);
					    },
					    canExecuteMethod: function () {
					        return !angular.equals(blade.origEntity, blade.currentEntity) && blade.hasUpdatePermission();
					    },
					    permission: blade.updatePermission
					},
					{
					    name: "content.commands.delete-list", icon: 'fa fa-trash-o',
					    executeMethod: function () {
					        blade.deleteList();
					    },
					    canExecuteMethod: function () {
					        return true;
					    },
					    permission: 'content:delete'
					},
					{
					    name: "content.commands.delete-links", icon: 'fa fa-trash-o',
					    executeMethod: function () {
					        blade.deleteLinks();
					    },
					    canExecuteMethod: function () {
					        return blade.selectedItemIds.length > 0;
					    },
					    permission: 'content:delete'
					}];
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    $scope.selected = function (id) {
        return _.contains(blade.selectedItemIds, id);
    };

    blade.saveChanges = function () {
        //checkForNull();
        blade.isLoading = true;
        menus.checkList({ storeId: blade.chosenStoreId, id: blade.currentEntity.id, name: blade.currentEntity.name, language: blade.currentEntity.language }, function (data) {
            if (Boolean(data.result)) {
                menus.update({ storeId: blade.chosenStoreId }, blade.currentEntity, function (data) {
                    blade.parentBlade.initialize();
                    blade.newList = false;
                    blade.isLoading = false;
                    blade.origEntity = angular.copy(blade.currentEntity);
                    $rootScope.$broadcast("cms-menus-changed", blade.chosenStoreId);
                },
                function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
            }
            else {
                blade.isLoading = false;
                var dialog = {
                    id: "errorInName",
                    title: "content.dialogs.name-must-unique.title",
                    message: "content.dialogs.name-must-unique.message",
                    callback: function (remove) {

                    }
                }
                dialogService.showNotificationDialog(dialog);
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity) && blade.hasUpdatePermission();
    }

    function canSave() {
        var listNameIsRight = !((angular.isUndefined(blade.currentEntity.name)) || (blade.currentEntity.name === null));
        var linksAreRight = _.all(blade.currentEntity.menuLinks, function (x) {
            return x.title && x.url && (!x.associatedObjectType || x.associatedObjectId);
        });
        return isDirty() && listNameIsRight && linksAreRight && _.any(blade.currentEntity.menuLinks);
    }

    blade.deleteList = function () {
        var dialog = {
            id: "confirmDelete",
            title: "content.dialogs.link-list-delete.title",
            message: "content.dialogs.link-list-delete.message",
            callback: function (remove) {
                if (remove) {
                    blade.isLoading = true;

                    menus.delete({ storeId: blade.chosenStoreId, listId: blade.chosenListId }, function () {
                        $scope.bladeClose();
                        blade.parentBlade.initialize();
                        $rootScope.$broadcast("cms-menus-changed", blade.chosenStoreId);
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    };

    blade.deleteLinks = function () {
        var dialog = {
            id: "confirmDelete",
            title: "content.dialogs.links-delete.title",
            message: "content.dialogs.links-delete.message",
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
    };

    blade.onClose = function (closeCallback) {
        bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, blade.saveChanges, closeCallback, "content.dialogs.link-list-save.title", "content.dialogs.link-list-save.message");
    };

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

    $scope.blade.headIcon = 'fa-archive';

    $scope.sortableOptions = {
        stop: function (e, ui) {
            blade.recalculatePriority();
        },
        axis: 'y'
    };

    $scope.associatedObjectTypes = associationTypesService.objects;
    blade.initialize();
}]);
