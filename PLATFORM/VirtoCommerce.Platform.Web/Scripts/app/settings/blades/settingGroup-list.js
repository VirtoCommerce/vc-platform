angular.module('platformWebApp')
.controller('platformWebApp.settingGroupListController', ['$injector', '$scope', 'platformWebApp.settings', 'platformWebApp.bladeNavigationService',
function ($injector, $scope, settings, bladeNavigationService) {
    var settingsTree;
    var blade = $scope.blade;

    blade.refresh = function () {
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
                        lastParent[path] = { displayName: path, id: lastParentId };
                    }

                    if (i < paths.length - 1) {
                        if (!lastParent[path].children) {
                            lastParent[path].children = {};
                        }
                        lastParent = lastParent[path].children;
                    } else {
                        if (!lastParent[path].settings) {
                            lastParent[path].settings = [];
                        }
                        setting.displayName = path;
                        lastParent[path].settings.push(setting);
                    }
                });
            });

            blade.isLoading = false;

            // restore previous selection
            if (blade.searchText) {
                $scope.blade.currentEntities = settingsTree;

                // open previous settings detail blade if possible
                if ($scope.selectedNodeId) {
                    $scope.selectNode(_.findWhere(blade.allSettings, { name: $scope.selectedNodeId }));
                }
            } else {
                var lastchildren = settingsTree;
                for (var i = 1; i < blade.breadcrumbs.length; i++) {
                    lastchildren = lastchildren[blade.breadcrumbs[i].name].children;
                }
                $scope.blade.currentEntities = lastchildren;

                // open previous settings detail blade if possible
                if ($scope.selectedNodeId && lastchildren[$scope.selectedNodeId]) {
                    $scope.selectNode(lastchildren[$scope.selectedNodeId]);
                }
            }
        },
        function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    };

    $scope.selectNode = function (node) {
        bladeNavigationService.closeChildrenBlades(blade, function () {
            $scope.selectedNodeId = node.name ? node.name : node.displayName;
            if (node.children) {
                blade.searchText = null;
                $scope.blade.currentEntities = node.children;

                setBreadcrumbs(node);
            } else {
                var newBlade = {
                    id: 'settingsSection',
                    data: node.settings ? node.settings : [node],
                    title: node.settings ? 'Setting values' : 'Setting ' + node.displayName,
                    controller: 'platformWebApp.settingsDetailController',
                    template: 'Scripts/app/settings/blades/settings-detail.tpl.html'
                };

                bladeNavigationService.showBlade(newBlade, blade);
            }
        });
    };

    //Breadcrumbs
    function setBreadcrumbs(node) {
        var foundBreadcrumb = _.findWhere(blade.breadcrumbs, { id: node.id });
        if (foundBreadcrumb) {
            var idx = blade.breadcrumbs.indexOf(foundBreadcrumb);
            if (idx < blade.breadcrumbs.length - 1) {
                blade.breadcrumbs.splice(idx + 1, blade.breadcrumbs.length - 1 - idx);
            }
        } else {
            var breadCrumb = {
                id: node.id,
                name: node.displayName,
                navigate: function () {
                    $scope.selectNode(node);
                }
            };

            blade.breadcrumbs.push(breadCrumb);
        }
    }

    blade.breadcrumbs = [{
        id: null,
        name: "all",
        navigate: function () {
            $scope.selectNode({ id: null, children: settingsTree });
        }
    }];

    blade.headIcon = 'fa fa-wrench';

    $scope.$watch('blade.searchText', function (newVal) {
        if (newVal) {
            $scope.blade.currentEntities = settingsTree;
            setBreadcrumbs({ id: null });
        }
    });

    // actions on load
    blade.refresh();
}]);