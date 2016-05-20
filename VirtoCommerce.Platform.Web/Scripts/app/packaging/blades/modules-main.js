angular.module('platformWebApp')
.controller('platformWebApp.modulesMainController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.modules', 'platformWebApp.moduleHelper', function ($scope, bladeNavigationService, modules, moduleHelper) {
    var blade = $scope.blade;
    var nodeUpdate, nodeAvailable, nodeInstalled;
    $scope.selectedNodeId = null;

    blade.refresh = function () {
        blade.isLoading = true;

        return modules.query().$promise.then(function (results) {
            moduleHelper.allmodules = results;
            _.each(results, function (x) {
                if (x.tags) {
                    x.tagsArray = x.tags.split(' ');
                }
            });

            var newResults = [];
            var grouped = _.groupBy(results, 'id');
            _.each(grouped, function (vals, key) {
                // vals.sort(function (a, b) { return moduleHelper.versionCompare(a.version, b.version); })
                var latest = _.last(vals);
                latest.$all = vals;
                newResults.push(latest);
            });

            // pre-calculate installedVersion AND latest available version ($latestModule) properties
            _.each(newResults, function (x) {
                if (!x.isInstalled) {
                    var foundInstalledModule;
                    if (foundInstalledModule = _.findWhere(x.$all, { isInstalled: true })) {
                        _.each(x.$all, function (m) {
                            if (m === foundInstalledModule) {
                                m.$latestModule = x;
                            } else {
                                m.installedVersion = foundInstalledModule.version;
                            }
                        });
                    }
                }
            });

            nodeUpdate.entities = _.filter(newResults, function (x) { return !x.isInstalled && x.installedVersion; });
            nodeAvailable.entities = newResults;
            nodeInstalled.entities = _.where(results, { isInstalled: true });

            openFirstBladeInitially();
            blade.isLoading = false;

            // return results for current child list
            return _.findWhere(blade.currentEntities, { mode: $scope.selectedNodeId }).entities;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
        });
    };

    blade.openBlade = function (data) {
        $scope.selectedNodeId = data.mode;

        var newBlade = {
            id: 'modulesList',
            mode: data.mode,
            currentEntities: data.entities,
            title: data.name,
            parentRefresh: blade.refresh,
            subtitle: 'platform.blades.modules-list.subtitle',
            controller: 'platformWebApp.modulesListController',
            template: '$(Platform)/Scripts/app/packaging/blades/modules-list.tpl.html'
        };

        if (data.mode === 'advanced') {
            angular.extend(newBlade, {
                controller: 'platformWebApp.moduleDetailController',
                template: '$(Platform)/Scripts/app/packaging/blades/module-detail.tpl.html'
            });
        }

        bladeNavigationService.showBlade(newBlade, blade);
    };

    blade.toolbarCommands = [
          {
              name: "platform.commands.refresh", icon: 'fa fa-refresh',
              executeMethod: blade.refresh,
              canExecuteMethod: function () { return true; }
          }
    ];

    blade.headIcon = 'fa-cubes';

    blade.currentEntities = [
        nodeUpdate = { name: 'platform.blades.modules-main.labels.updates', mode: 'update' },
        nodeAvailable = { name: 'platform.blades.modules-main.labels.available', mode: 'available' },
        nodeInstalled = { name: 'platform.blades.modules-main.labels.installed', mode: 'installed' },
        { name: 'platform.blades.modules-main.labels.advanced', mode: 'advanced' }
    ];

    var openFirstBladeInitially = _.once(function () { blade.openBlade(blade.currentEntities[0]); });

    blade.refresh();
}]);
