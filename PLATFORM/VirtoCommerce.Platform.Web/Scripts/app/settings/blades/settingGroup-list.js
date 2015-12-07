angular.module('platformWebApp')
.controller('platformWebApp.settingGroupListController', ['$injector', '$scope', 'platformWebApp.settings', 'platformWebApp.bladeNavigationService',
function ($injector, $scope, settings, bladeNavigationService) {
    var settingsTree;
    var blade = $scope.blade;

    blade.refresh = function (disableOpenAnimation) {
        blade.isLoading = true;

        settings.query({}, function (results) {
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
                        if (_.all(blade.allSettings, function (x) { return x.groupName !== treeNode.groupName; })) {
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
                $scope.blade.currentEntities = settingsTree;
            } else {
                // reconstruct tree by breadCrumbs
                var lastchildren = settingsTree;
                for (var i = 1; i < blade.breadcrumbs.length; i++) {
                    lastchildren = lastchildren[blade.breadcrumbs[i].name].children;
                }
                $scope.blade.currentEntities = lastchildren;
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
                $scope.blade.currentEntities = node.children;

                setBreadcrumbs(node);
            } else {
                var selectedSettings = _.where(blade.allSettings, { groupName: node.groupName });
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
            $scope.blade.currentEntities = settingsTree;
            setBreadcrumbs({ groupName: null });
        }
    });

    // actions on load
    blade.refresh();
}]);