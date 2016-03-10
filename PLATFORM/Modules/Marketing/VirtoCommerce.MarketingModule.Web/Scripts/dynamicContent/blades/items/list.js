angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.itemsDynamicContentListController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.folders', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_folders, bladeNavigationService, dialogService) {
    var blade = $scope.blade;
    blade.currentEntity = {};

    function refresh() {
        marketing_dynamicContents_res_search.search({ folderId: blade.chosenFolder, responseGroup: '18' }, function (data) {
            blade.currentEntity.childrenFolders = data.contentFolders;
            blade.currentEntity.items = data.contentItems;
            setBreadcrumbs();
            blade.isLoading = false;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    blade.initializeBlade = function () {
        if (blade.chosenFolder === undefined) {
            blade.chosenFolder = 'ContentItem';
        }
        refresh();
    };

    blade.addNew = function () {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.items.add.title',
            subtitle: 'marketing.blades.items.add.subtitle',
            chosenFolder: blade.chosenFolder,
            controller: 'virtoCommerce.marketingModule.addContentItemsElementController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/add.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.addNewFolder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.items.folder-details.title-new',
            subtitle: 'marketing.blades.items.folder-details.subtitle-new',
            entity: data,
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addFolderContentItemsController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/folder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.editFolder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.items.folder-details.title',
            subtitle: 'marketing.blades.items.folder-details.subtitle',
            entity: data,
            isNew: false,
            controller: 'virtoCommerce.marketingModule.addFolderContentItemsController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/folder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.addNewContentItem = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.items.content-item-details.title-new',
            subtitle: 'marketing.blades.items.content-item-details.subtitle-new',
            entity: data,
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addContentItemsController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/content-item-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.editContentItem = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.items.content-item-details.title',
            subtitle: 'marketing.blades.items.content-item-details.subtitle',
            entity: data,
            isNew: false,
            controller: 'virtoCommerce.marketingModule.addContentItemsController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/items/content-item-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.closeChildrenBlades = function () {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.folderClick = function (contentItemFolder) {
        blade.isLoading = true;
        blade.closeChildrenBlades();

        if (angular.isUndefined(blade.chosenFolder) || !angular.equals(blade.chosenFolder, contentItemFolder.id)) {
            blade.chosenFolder = contentItemFolder.id;
            blade.currentEntity = contentItemFolder;
            refresh();
        }
    }

    function setBreadcrumbs() {
        if (blade.breadcrumbs) {
            var breadcrumbs;
            var index = _.findLastIndex(blade.breadcrumbs, { id: blade.chosenFolder });
            if (index > -1) {
                //Clone array (angular.copy leaves the same reference)
                breadcrumbs = blade.breadcrumbs.slice(0, index + 1);
            }
            else {
                breadcrumbs = blade.breadcrumbs.slice(0);
                breadcrumbs.push(generateBreadcrumb(blade.currentEntity));
            }
            blade.breadcrumbs = breadcrumbs;
        } else {
            blade.breadcrumbs = [(generateBreadcrumb({ id: 'ContentItem', name: 'Items' }))];
        }
    }

    function generateBreadcrumb(node) {
        return {
            id: node.id,
            name: node.name,
            navigate: function () {
                blade.folderClick(node);
            }
        }
    }

    blade.deleteFolder = function (data) {
        var dialog = {
            id: "confirmDeleteContentItemsFolder",
            title: "marketing.dialogs.content-item-folder-delete.title",
            message: "marketing.dialogs.content-item-folder-delete.message",
            callback: function (remove) {
                if (remove) {
                    marketing_dynamicContents_res_folders.delete({ ids: [data.id] }, function () {
                        var pathSteps = data.outline.split(';');
                        var id = pathSteps[pathSteps.length - 2];
                        blade.chosenFolder = id;
                        blade.initializeBlade();
                    },
					function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                }
            }
        };

        dialogService.showConfirmationDialog(dialog);
    }

    blade.toolbarCommands = [
		{
		    name: "platform.commands.add", icon: 'fa fa-plus',
		    executeMethod: function () {
		        blade.addNew();
		    },
		    canExecuteMethod: function () {
		        return true;
		    },
		    permission: 'marketing:create'
		},
		{
		    name: "marketing.commands.edit-folder", icon: 'fa fa-pencil-square-o',
		    executeMethod: function () {
		        blade.editFolder(blade.currentEntity);
		    },
		    canExecuteMethod: function () {
		        return !angular.isUndefined(blade.currentEntity);
		    },
		    permission: 'marketing:update'
		}
    ];

    blade.headIcon = 'fa-inbox';

    blade.initializeBlade();
}]);
