angular.module('virtoCommerce.marketingModule')
.controller('virtoCommerce.marketingModule.placeholdersDynamicContentListController', ['$scope', 'virtoCommerce.marketingModule.dynamicContent.search', 'virtoCommerce.marketingModule.dynamicContent.folders', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', function ($scope, marketing_dynamicContents_res_search, marketing_dynamicContents_res_folders, bladeNavigationService, dialogService) {
    var blade = $scope.blade;
    blade.currentEntity = {};

    function refresh() {
        marketing_dynamicContents_res_search.search({ folder: blade.choosenFolder, respGroup: '20' }, function (data) {
            blade.currentEntity.childrenFolders = data.contentFolders;
            blade.currentEntity.placeholders = data.contentPlaces;
            setBreadcrumbs();
            blade.isLoading = false;
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    blade.initialize = function () {
        if (blade.choosenFolder === undefined) {
            blade.choosenFolder = 'ContentPlace';
        }
        refresh();
    };

    blade.addNew = function () {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.placeholders.add.title',
            subtitle: 'marketing.blades.placeholders.add.subtitle',
            choosenFolder: blade.choosenFolder,
            controller: 'virtoCommerce.marketingModule.addPlaceholderElementController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/add.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.addNewFolder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.placeholders.folder-details.title-new',
            subtitle: 'marketing.blades.placeholders.folder-details.subtitle-new',
            entity: data,
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addFolderPlaceholderController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/folder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.editFolder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.placeholders.folder-details.title',
            subtitle: 'marketing.blades.placeholders.folder-details.subtitle',
            entity: data,
            isNew: false,
            controller: 'virtoCommerce.marketingModule.addFolderPlaceholderController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/folder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.addNewPlaceholder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.placeholders.content-item-details.title-new',
            subtitle: 'marketing.blades.placeholders.content-item-details.subtitle-new',
            entity: data,
            isNew: true,
            controller: 'virtoCommerce.marketingModule.addPlaceholderController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/placeholder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.editPlaceholder = function (data) {
        blade.closeChildrenBlades();

        var newBlade = {
            id: 'listItemChild',
            title: 'marketing.blades.placeholders.content-item-details.title',
            subtitle: 'marketing.blades.placeholders.content-item-details.subtitle',
            entity: data,
            isNew: false,
            controller: 'virtoCommerce.marketingModule.addPlaceholderController',
            template: 'Modules/$(VirtoCommerce.Marketing)/Scripts/dynamicContent/blades/placeholders/placeholder-details.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, blade);
    }

    blade.closeChildrenBlades = function () {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.folderClick = function (placeholderFolder) {
        blade.closeChildrenBlades();

        if (angular.isUndefined(blade.choosenFolder) || !angular.equals(blade.choosenFolder, placeholderFolder.id)) {
            blade.isLoading = true;
            blade.choosenFolder = placeholderFolder.id;
            blade.currentEntity = placeholderFolder;
            refresh();
        }
    }

    function setBreadcrumbs() {
        if (blade.breadcrumbs) {
            var breadcrumbs;
            var index = _.findLastIndex(blade.breadcrumbs, { id: blade.choosenFolder });
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
            blade.breadcrumbs = [(generateBreadcrumb({ id: 'ContentPlace', name: 'Places' }))];
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
            id: "confirmDeleteContentPlaceholdersFolder",
            title: "marketing.dialiogs.placeholders-folder-delete.title",
            message: "marketing.dialiogs.placeholders-folder-delete.message",
            callback: function (remove) {
                if (remove) {
                    marketing_dynamicContents_res_folders.delete({ ids: [data.id] }, function () {
                        var pathSteps = data.outline.split(';');
                        var id = pathSteps[pathSteps.length - 2];
                        blade.choosenFolder = id;
                        blade.initialize();
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

    blade.headIcon = 'fa-location-arrow';

    blade.initialize();
}]);
