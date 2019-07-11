angular.module('platformWebApp')
.controller('platformWebApp.settingGroupListController', ['$window', 'platformWebApp.modules', '$scope', 'platformWebApp.settings', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService',
function ($window, modules, $scope, settings, bladeNavigationService, dialogService) {
    var settingsTree;
    var blade = $scope.blade;

    blade.refresh = function (disableOpenAnimation) {
        blade.isLoading = true;

        settings.query({}, function (results) {
            results = _.sortBy(results, 'groupName');
            blade.allSettings = results;
            settingsTree = {};
            _.each(results, function (setting) {
                var paths = (setting.groupName ? setting.groupName : 'General').split('|');
                var lastParent = settingsTree;
                var lastParentId = '';
                _.each(paths, function (path, i) {
                    lastParentId += '|' + path;
                    if (!lastParent[path]) {
                        var treeNode = { name: path, groupName: lastParentId.substring(1) }
                        lastParent[path] = treeNode;
                        if (setting.groupName && _.all(blade.allSettings, function (x) { return x.groupName !== treeNode.groupName; })) {
                            blade.allSettings.push(treeNode);
                        }
                    }

                    if (i < paths.length - 1) {
                        if (!lastParent[path].children) {
                            lastParent[path].children = {};
                        }
                        lastParent = lastParent[path].children;
                    }
                });
            });

            blade.isLoading = false;

            // restore previous selection
            if (blade.searchText) {
                blade.currentEntities = settingsTree;
            } else {
                // reconstruct tree by breadCrumbs
                var lastchildren = settingsTree;
                for (var i = 1; i < blade.breadcrumbs.length; i++) {
                    lastchildren = lastchildren[blade.breadcrumbs[i].name].children;
                }
                blade.currentEntities = lastchildren;
            }

            // open previous settings detail blade if possible
            if ($scope.selectedNodeId) {
                $scope.selectNode({ groupName: $scope.selectedNodeId }, disableOpenAnimation);
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    $scope.selectNode = function (node, disableOpenAnimation) {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            $scope.selectedNodeId = node.groupName;
            if (node.children) {
                blade.searchText = null;
                blade.currentEntities = node.children;

                setBreadcrumbs(node);
            } else {
                var selectedSettings = _.filter(blade.allSettings, function (x) { return x.groupName === node.groupName || (node.groupName === 'General' && !x.groupName); });
                var newBlade = {
                    id: 'settingsSection',
                    data: selectedSettings,
                    title: 'platform.blades.settings-detail.title',
                    disableOpenAnimation: disableOpenAnimation,
                    controller: 'platformWebApp.settingsDetailController',
                    template: '$(Platform)/Scripts/app/settings/blades/settings-detail.tpl.html'
                };

                bladeNavigationService.showBlade(newBlade, blade);
            }
        });
    };

    //Breadcrumbs
    function setBreadcrumbs(node) {
        blade.breadcrumbs.splice(1, blade.breadcrumbs.length - 1);

        if (node.groupName) {
            var lastParentId = '';
            var lastchildren = settingsTree;
            var paths = node.groupName.split('|');
            _.each(paths, function (path) {
                lastchildren = lastchildren[path].children;
                lastParentId += '|' + path;
                var breadCrumb = {
                    id: lastParentId.substring(1),
                    name: path,
                    children: lastchildren,
                    navigate: function () {
                        $scope.selectNode({ groupName: this.id, children: this.children });
                    }
                };

                blade.breadcrumbs.push(breadCrumb);
            });
        }
    }

    blade.breadcrumbs = [{
        id: null,
        name: "platform.navigation.bread-crumb-top",
        navigate: function () {
            $scope.selectNode({ groupName: null, children: settingsTree });
        }
    }];

    blade.headIcon = 'fa-wrench';

    $scope.$watch('blade.searchText', function (newVal) {
        if (newVal) {
            blade.currentEntities = settingsTree;
            setBreadcrumbs({ groupName: null });
        }
    });

    blade.toolbarCommands = [
          {
              name: "platform.commands.restart", icon: 'fa fa-bolt',
              executeMethod: function () { restart(); },
              canExecuteMethod: function () { return !blade.isLoading; },
              permission: 'platform:module:manage'
          }
    ];

    function restart() {
        var dialog = {
            id: "confirmRestart",
            title: "platform.dialogs.app-restart.title",
            message: "platform.dialogs.app-restart.message",
            callback: function (confirm) {
                if (confirm) {
                    blade.isLoading = true;
                    modules.restart(function () {
                        $window.location.reload();
                    });
                }
            }
        }
        dialogService.showConfirmationDialog(dialog);
    }

    // actions on load
    blade.refresh();
}]);