angular.module('virtoCommerce.contentModule')
.controller('virtoCommerce.contentModule.pagesListController', ['$scope', 'virtoCommerce.contentModule.pages', 'virtoCommerce.contentModule.stores', 'platformWebApp.bladeNavigationService', function ($scope, pages, pagesStores, bladeNavigationService) {
    $scope.selectedNodeId = null;

    var blade = $scope.blade;
    blade.updatePermission = 'content:update';

    blade.isPages = function () {
        return blade.type === 'pages';
    }

    blade.steps = blade.isPages() ? ['Pages'] : ['Blogs'];
    blade.selectedStep = 0;

    blade.initialize = function () {
        blade.isLoading = true;
        pages.getFolders({ storeId: blade.storeId }, function (data) {
            blade.isLoading = false;
            if (blade.isPages()) {
                data.folders = _.reject(data.folders, function (folder) { return folder.folderName === 'blogs' });
            }
            else {
                data = _.find(data.folders, function (folder) { return folder.folderName === 'blogs' });
            }

            blade.pagesCatalog = data;
            blade.currentPageCatalog = data;

            for (var i = 1; i < blade.steps.length; i++) {
                blade.currentPageCatalog = _.find(blade.currentPageCatalog.folders, function (folder) { return folder.folderName === blade.steps[i] });
            }

            blade.parentBlade.refresh(blade.storeId, blade.type);
            blade.defaultButtons();
        },
	    function (error) { bladeNavigationService.setError('Error ' + error.status, $scope.blade); });
    }

    blade.openBlade = function (data) {
        $scope.selectedNodeId = data.pageName;

        pages.getPage({ storeId: blade.storeId, language: data.language ? data.language : "undef", pageName: data.id }, function (page) {
            if (page.language !== 'files') {
                var parts = page.content.split('---');
                var body = '';
                var metadata = '';
                if (parts.length > 2) {
                    body = parts[2].trim();
                    metadata = parts[1].trim();
                }
                else {
                    body = parts[0];
                }

                var newBlade = {
                    id: 'editPageBlade',
                    choosenStoreId: blade.storeId,
                    choosenPageName: data.id,
                    choosenPageLanguage: data.language,
                    newPage: false,
                    body: body,
                    metadata: metadata,
                    title: 'content.blades.edit-page.title',
                    titleValues: { name: data.name },
                    subtitle: 'content.blades.edit-page.subtitle',
                    controller: 'virtoCommerce.contentModule.editPageController',
                    template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-page.tpl.html'
                };

                bladeNavigationService.showBlade(newBlade, blade);
            }
            else {
                var newBlade = {
                    id: 'editPageBlade',
                    choosenStoreId: blade.storeId,
                    choosenPageName: data.id,
                    choosenPageLanguage: data.language,
                    newPage: false,
                    title: 'content.blades.edit-page.title',
                    titleValues: { name: data.name },
                    subtitle: 'content.blades.edit-page.subtitle',
                    controller: 'virtoCommerce.contentModule.editPageController',
                    template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-page.tpl.html'
                };

                bladeNavigationService.showBlade(newBlade, blade);
            }
        });
    }

    blade.openBladeNew = function (isBytes) {
        $scope.selectedNodeId = null;

        var path = '';
        if (blade.isPages()) {
            if (blade.steps.length > 1) {
                path = blade.steps.slice(1).join('/') + '/';
            }
        }
        else {
            var steps = angular.copy(blade.steps);
            steps[0] = 'blogs';
            path = steps.join('/') + '/';
        }

        if (!isBytes) {
            var newBlade = {
                id: 'addPageBlade',
                choosenStoreId: blade.storeId,
                currentEntity: { name: path + 'new_page.md', pageName: 'new_page', content: null, contentType: 'text/html', language: null, storeId: blade.storeId },
                newPage: true,
                body: '',
                metadata: '',
                title: 'content.blades.edit-page.title-new',
                subtitle: 'content.blades.edit-page.subtitle-new',
                controller: 'virtoCommerce.contentModule.editPageController',
                template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-page.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        }
        else {
            var newBlade = {
                id: 'addPageBlade',
                choosenStoreId: blade.storeId,
                path: path,
                currentEntity: { name: path + 'new_file', pageName: 'new_file', content: null, contentType: null, language: null, storeId: blade.storeId },
                newPage: true,
                title: 'content.blades.edit-page.title-new-file',
                subtitle: 'content.blades.edit-page.subtitle-new-file',
                controller: 'virtoCommerce.contentModule.editPageController',
                template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-page.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, $scope.blade);
        }
    }
    
    blade.folderClick = function (data) {
        blade.steps.push(data.folderName);

        blade.currentPageCatalog = data;

        if (blade.currentPageCatalog.folderName === 'blogs') {
            $scope.blade.toolbarCommands = [
                {
                    name: "content.commands.add-blog", icon: 'fa fa-plus',
                    executeMethod: function () {
                        blade.openBlogNew(true, { name: undefined });
                    },
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: 'content:create'
                }];
        }
        else {
            blade.defaultButtons();
        }

        if (blade.currentPageCatalog.folderName !== 'blogs' && _.find(blade.steps, function (step) { return step === 'Blogs'; }) !== undefined) {
            $scope.blade.toolbarCommands.push(
            {
                name: "content.commands.manage-blog", icon: 'fa fa-edit',
                executeMethod: function () {
                    blade.openBlogNew(false, { name: blade.currentPageCatalog.folderName });
                },
                canExecuteMethod: function () {
                    return true;
                },
                permission: blade.updatePermission
            });
        }
    }

    blade.checkPreviousStep = function () {
        blade.selectedStep = 0;
        blade.stepsClick();
    }

    blade.stepsClick = function () {
        blade.currentPageCatalog = blade.pagesCatalog;
        var index = blade.selectedStep + 1;

        blade.steps.splice(index);

        for (var i = 1; i < index; i++) {
            blade.currentPageCatalog = _.find(blade.currentPageCatalog.folders, function (folder) { return folder.folderName === blade.steps[i] });
        }

        if (blade.currentPageCatalog.folderName === 'blogs') {
            $scope.blade.toolbarCommands = [
                {
                    name: "content.commands.add-blog", icon: 'fa fa-plus',
                    executeMethod: function () {
                        blade.openBlogNew(true, { name: undefined });
                    },
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: 'content:create'
                }
            ];
        }
        else {
            blade.defaultButtons();
        }

        if (blade.currentPageCatalog.folderName !== 'blogs' && _.find(blade.steps, function (step) { return step === 'Blogs'; }) !== undefined) {
            $scope.blade.toolbarCommands.push(
            {
                name: "content.commands.manage-blog", icon: 'fa fa-edit',
                executeMethod: function () {
                    blade.openBlogNew(false, { name: blade.currentPageCatalog.folderName });
                },
                canExecuteMethod: function () {
                    return true;
                },
                permission: blade.updatePermission
            });
        }
    }

    blade.showDivider = function (index) {
        if (index === blade.steps.length - 1) {
            return false;
        }

        return true;
    }

    $scope.blade.headIcon = 'fa-archive';

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

    blade.openBlogNew = function (isNew, data) {
        var title = isNew ? 'Add blog' : 'Edit blog';
        var subTitle = isNew ? 'Create new blog' : 'Edit blog folder';

        var newBlade = {
            id: 'openBlogNew',
            choosenStoreId: blade.storeId,
            isNew: isNew,
            entity: data,
            title: title,
            subtitle: subTitle,
            controller: 'virtoCommerce.contentModule.editBlogController',
            template: 'Modules/$(VirtoCommerce.Content)/Scripts/blades/pages/edit-blog.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.defaultButtons = function () {
        if ((blade.currentPageCatalog && blade.currentPageCatalog.folderName === 'blogs') || (!blade.currentPageCatalog && blade.isBlogsBlade)) {
            $scope.blade.toolbarCommands = [
                {
                    name: "content.commands.add-blog", icon: 'fa fa-plus',
                    executeMethod: function () {
                        blade.openBlogNew(true, { name: undefined });
                    },
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: 'content:create'
                }
            ];
        }
        else {
            $scope.blade.toolbarCommands = [
                {
                    name: "content.commands.add-page", icon: 'fa fa-plus',
                    executeMethod: function () {
                        blade.openBladeNew(false);
                    },
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: 'content:create'
                },
                {
                    name: "content.commands.add-file", icon: 'fa fa-plus',
                    executeMethod: function () {
                        blade.openBladeNew(true);
                    },
                    canExecuteMethod: function () {
                        return true;
                    },
                    permission: 'content:create'
                }
            ];
        }
    }

    blade.initialize();
}]);
