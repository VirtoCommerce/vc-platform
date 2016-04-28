angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.pagesListController', ['$rootScope', '$scope', 'virtoCommerce.contentModule.contentApi', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeUtils', function ($rootScope, $scope, contentApi, bladeNavigationService, dialogService, uiGridHelper, bladeUtils) {
    var blade = $scope.blade;
    blade.updatePermission = 'content:update';

    $scope.selectedNodeId = null;

    blade.refresh = function () {
        blade.isLoading = true;
        contentApi.query(
            {
                contentType: blade.contentType,
                storeId: blade.storeId,
                keyword: blade.searchKeyword,
                folderUrl: blade.currentEntity.url
            },
            function (data) {
                $scope.pageSettings.totalItems = data.length;
                _.each(data, function (x) { x.isOpenable = true; });
                $scope.listEntries = data;
                blade.isLoading = false;

                //Set navigation breadcrumbs
                setBreadcrumbs();
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
    };

    function newFolder(value, prefix) {
        var result = prompt(prefix ? prefix + "\n\nEnter folder name:" : "Enter folder name:", value);
        if (result != null) {
            contentApi.createFolder(
                    { contentType: blade.contentType, storeId: blade.storeId },
                    { name: result, parentUrl: blade.currentEntity.url },
                    blade.refresh,
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        }
    }

    if (!isBlogs()) {
        $scope.rename = function (listItem) {
            var result = prompt("Enter new name", listItem.name);
            if (result) {
                contentApi.move({
                    contentType: blade.contentType,
                    storeId: blade.storeId,
                    oldUrl: listItem.url,
                    newUrl: listItem.url.substring(0, listItem.url.length - listItem.name.length) + result
                }, blade.refresh,
                function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            }
        };
    }

    $scope.copyUrl = function (data) {
        window.prompt("Copy to clipboard: Ctrl+C, Enter", data.url);
    };

    $scope.downloadUrl = function (data) {
        window.open(data.url, '_blank');
    };

    $scope.selectNode = function (listItem) {
        if (listItem.type === 'folder') {
            var newBlade = {
                id: blade.id,
                contentType: blade.contentType,
                storeId: blade.storeId,
                languages: blade.languages,
                currentEntity: listItem,
                breadcrumbs: blade.breadcrumbs,
                title: blade.title,
                subtitle: blade.subtitle,
                controller: blade.controller,
                template: blade.template,
                disableOpenAnimation: true,
                isClosingDisabled: blade.isClosingDisabled
            };
            bladeNavigationService.showBlade(newBlade, blade.parentBlade);
        } else {
            blade.selectedNodeId = listItem.url;
            openDetailsBlade(listItem, false);
        }
    };

    function openDetailsBlade(listItem, isNew) {
        var newBlade = {
            id: 'pageDetail',
            contentType: blade.contentType,
            storeId: blade.storeId,
            languages: blade.languages,
            folderUrl: blade.currentEntity.url,
            currentEntity: listItem,
            isNew: isNew,
            title: listItem.name,
            controller: 'virtoCommerce.contentModule.pageDetailController',
            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/page-detail.tpl.html'
        };

        if (isBlogs()) {
            if (isNew) {
                angular.extend(newBlade, {
                    title: 'content.blades.edit-page.title-new-post',
                    subtitle: 'content.blades.edit-page.subtitle-new-post',
                });
            } else {
                angular.extend(newBlade, {
                    subtitle: 'content.blades.edit-page.subtitle-post',
                });
            }
        } else {
            if (isNew) {
                angular.extend(newBlade, {
                    title: 'content.blades.edit-page.title-new',
                    subtitle: 'content.blades.edit-page.subtitle-new',
                });
            } else {
                angular.extend(newBlade, {
                    subtitle: 'content.blades.edit-page.subtitle',
                });
            }
        }

        bladeNavigationService.showBlade(newBlade, blade);
    }

    function openBlogDetailsBlade(listItem, isNew) {
        var newBlade = {
            id: 'blogDetail',
            contentType: blade.contentType,
            storeId: blade.storeId,
            currentEntity: listItem,
            isNew: isNew,
            title: listItem.name,
            subtitle: 'content.blades.edit-blog.subtitle',
            controller: 'virtoCommerce.contentModule.editBlogController',
            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-blog.tpl.html'
        };

        if (isNew) {
            angular.extend(newBlade, {
                title: 'content.blades.edit-blog.title-new',
                subtitle: 'content.blades.edit-blog.subtitle-new',
            });
        }

        bladeNavigationService.showBlade(newBlade, blade);
    }

    $scope.delete = function (data) {
        deleteList([data]);
    };

    function deleteList(selection) {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            var dialog = {
                id: "confirmDeleteItem",
                title: "platform.dialogs.folders-delete.title",
                message: "platform.dialogs.folders-delete.message",
                callback: function (remove) {
                    if (remove) {
                        var listEntryIds = _.pluck(selection, 'url');
                        contentApi.delete({
                            contentType: blade.contentType,
                            storeId: blade.storeId,
                            urls: listEntryIds
                        },
                        function () {
                            blade.refresh();
                            $rootScope.$broadcast("cms-statistics-changed", blade.storeId);
                        },
                        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                    }
                }
            }

            if (isBlogs() && !blade.currentEntity.type) {
                angular.extend(dialog, {
                    title: 'content.dialogs.blog-delete.title',
                    message: 'content.dialogs.blog-delete.message',
                });
            }

            dialogService.showConfirmationDialog(dialog);
        });
    }

    function isItemsChecked() {
        return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
    }

    function isPages() {
        return blade.contentType === 'pages';
    }

    function isBlogs() {
        return blade.contentType === 'blogs';
    }

    blade.toolbarCommands = [
          {
              name: "platform.commands.refresh", icon: 'fa fa-refresh',
              executeMethod: blade.refresh,
              canExecuteMethod: function () {
                  return true;
              }
          }
    ];

    if (isPages()) {
        blade.toolbarCommands.splice(1, 0,
            {
                name: 'platform.commands.new-folder', icon: 'fa fa-folder-o',
                executeMethod: function () { newFolder(undefined); },
                canExecuteMethod: function () { return true; },
                permission: 'content:create'
            },
            {
                name: "platform.commands.add", icon: 'fa fa-plus',
                executeMethod: function () { openDetailsBlade({}, true); },
                canExecuteMethod: function () { return true; },
                permission: 'content:create'
            }
        );
    } else if (isBlogs()) {
        if (blade.currentEntity.type && blade.currentEntity.type === 'folder') {
            blade.toolbarCommands.splice(1, 0, {
                name: "content.commands.add-post", icon: 'fa fa-plus',
                executeMethod: function () { openDetailsBlade({}, true); },
                canExecuteMethod: function () { return true; },
                permission: 'content:create'
            });
        } else {
            blade.toolbarCommands.splice(1, 0, {
                name: 'content.commands.add-blog', icon: 'fa fa-plus',
                executeMethod: function () { openBlogDetailsBlade({}, true); },
                canExecuteMethod: function () { return true; },
                permission: 'content:create'
            });
        }
    }

    blade.toolbarCommands.push({
        name: "platform.commands.delete", icon: 'fa fa-trash-o',
        executeMethod: function () { deleteList($scope.gridApi.selection.getSelectedRows()); },
        canExecuteMethod: isItemsChecked,
        permission: 'content:delete'
    });

    if (isBlogs() && !blade.currentEntity.type) {
        blade.contextMenuItems = [
            {
                name: 'platform.commands.manage', icon: 'fa fa-edit',
                action: function (data) { openBlogDetailsBlade(data); },
                permission: blade.updatePermission
            }
        ];
    }

    // ui-grid
    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions,
        function (gridApi) {
            $scope.$watch('pageSettings.currentPage', gridApi.pagination.seek);
        });
    };
    bladeUtils.initializePagination($scope, true);

    //Breadcrumbs
    function setBreadcrumbs() {
        if (blade.breadcrumbs) {
            //Clone array (angular.copy leaves the same reference)
            var breadcrumbs = blade.breadcrumbs.slice(0);

            //prevent duplicate items
            if (blade.currentEntity.url && _.all(breadcrumbs, function (x) { return x.id !== blade.currentEntity.url; })) {
                var breadCrumb = generateBreadcrumb(blade.currentEntity.url, blade.currentEntity.name);
                breadcrumbs.push(breadCrumb);
            }
            blade.breadcrumbs = breadcrumbs;
        } else {
            blade.breadcrumbs = [generateBreadcrumb(blade.currentEntity.url, 'all')];
        }
    }

    function generateBreadcrumb(id, name) {
        return {
            id: id,
            name: name,
            blade: blade,
            navigate: function (breadcrumb) {
                breadcrumb.blade.searchKeyword = null;
                breadcrumb.blade.disableOpenAnimation = true;
                bladeNavigationService.showBlade(breadcrumb.blade, breadcrumb.blade.parentBlade);
            }
        }
    }

    blade.headIcon = isBlogs() ? 'fa-inbox' : 'fa-folder-o';
    blade.refresh();
}]);
