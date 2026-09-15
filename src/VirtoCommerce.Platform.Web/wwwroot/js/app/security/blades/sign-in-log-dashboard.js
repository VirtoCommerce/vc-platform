angular.module('platformWebApp')
    .controller('platformWebApp.signInLogDashboardController',
        ['$scope', 'platformWebApp.accounts', 'platformWebApp.bladeNavigationService',
            function ($scope, accounts, bladeNavigationService) {
                var blade = $scope.blade;

                blade.title = blade.title || 'platform.blades.sign-in-log-dashboard.title';
                blade.subtitle = blade.subtitle || 'platform.blades.sign-in-log-dashboard.subtitle';
                blade.headIcon = 'fas fa-clipboard-list';

                blade.periods = [
                    { label: 'platform.blades.sign-in-log.filter.period-24h', value: '24h', hours: 24 },
                    { label: 'platform.blades.sign-in-log.filter.period-7d', value: '7d', hours: 24 * 7 },
                    { label: 'platform.blades.sign-in-log.filter.period-30d', value: '30d', hours: 24 * 30 },
                    { label: 'platform.blades.sign-in-log.filter.period-all', value: '', hours: 0 }
                ];

                blade.period = '24h';

                function periodStartDate(periodValue) {
                    var period = _.findWhere(blade.periods, { value: periodValue });
                    if (!period || !period.hours) {
                        return null;
                    }

                    var startDate = new Date();
                    startDate.setHours(startDate.getHours() - period.hours);
                    return startDate.toISOString();
                }

                blade.refresh = function () {
                    blade.isLoading = true;

                    var criteria = { userId: blade.userId };
                    var startDate = periodStartDate(blade.period);
                    if (startDate) {
                        criteria.startDate = startDate;
                    }

                    accounts.getSignInLogStats({}, criteria, function (stats) {
                        blade.isLoading = false;
                        blade.stats = stats;
                        blade.tiles = buildTiles(stats);
                    }, function () {
                        blade.isLoading = false;
                    });
                };

                /**
                 * Movement against the same-length window immediately before this one.
                 * Returns null when there is nothing to compare against ("all time", or both
                 * windows empty), so the tile stays quiet rather than showing a meaningless 0%.
                 */
                function delta(current, previous) {
                    if (previous === null || previous === undefined) {
                        return null;
                    }

                    var change = current - previous;

                    if (previous === 0) {
                        // No baseline: a percentage would be infinite, so report the absolute move.
                        return change === 0 ? null : { absolute: change, direction: 'up' };
                    }

                    if (change === 0) {
                        return { percent: 0, direction: 'flat' };
                    }

                    return {
                        percent: Math.round(Math.abs(change) / previous * 100),
                        direction: change > 0 ? 'up' : 'down'
                    };
                }

                /**
                 * More failures is bad; more of anything else is not a judgement the data supports,
                 * so only the failure tile reads red or green.
                 */
                function deltaClass(d, higherIsWorse) {
                    if (!d || d.direction === 'flat') {
                        return '__flat';
                    }

                    if (!higherIsWorse) {
                        return '__neutral';
                    }

                    return d.direction === 'up' ? '__worse' : '__better';
                }

                // Tiles are built here rather than in the template: ng-if outranks ng-init, so a
                // delta computed inline would never be evaluated before its own visibility check.
                function buildTiles(stats) {
                    var definitions = [
                        { label: 'total', value: stats.totalCount, previous: stats.previousTotalCount, modifier: '', worse: false, action: $scope.openAll },
                        { label: 'failed', value: stats.failedCount, previous: stats.previousFailedCount, modifier: '__alert', worse: true, action: $scope.openFailed },
                        { label: 'users', value: stats.distinctUserCount, previous: stats.previousDistinctUserCount, modifier: '', worse: false, action: $scope.openSucceeded },
                        { label: 'impersonation', value: stats.impersonationCount, previous: stats.previousImpersonationCount, modifier: '__impersonation', worse: false, action: $scope.openImpersonation }
                    ];

                    return definitions.map(function (d) {
                        var movement = delta(d.value, d.previous);

                        return {
                            label: 'platform.blades.sign-in-log.stats.' + d.label,
                            hint: 'platform.blades.sign-in-log.stats.' + d.label + '-hint',
                            value: d.value,
                            delta: movement,
                            deltaClass: deltaClass(movement, d.worse),
                            modifier: d.modifier,
                            action: d.action
                        };
                    });
                }

                $scope.setPeriod = function (periodValue) {
                    blade.period = periodValue;
                    blade.refresh();
                };

                /**
                 * Opens the sign-in log list as a child blade, seeded with the dashboard's period
                 * plus whatever the clicked element represents. Passing an empty override is the
                 * "all records" entry point.
                 */
                $scope.openList = function (initialFilter, subtitle) {
                    var newBlade = {
                        id: 'signInLogList',
                        userId: blade.userId,
                        subtitle: subtitle,
                        initialFilter: angular.extend({ period: blade.period }, initialFilter || {}),
                        controller: 'platformWebApp.signInLogListController',
                        template: '$(Platform)/Scripts/app/security/blades/sign-in-log-list.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                };

                $scope.openAll = function () {
                    $scope.openList({}, 'platform.blades.sign-in-log-dashboard.presets.all');
                };

                $scope.openSucceeded = function () {
                    $scope.openList({ succeeded: 'true' }, 'platform.blades.sign-in-log-dashboard.presets.succeeded');
                };

                $scope.openFailed = function () {
                    $scope.openList({ succeeded: 'false' }, 'platform.blades.sign-in-log-dashboard.presets.failed');
                };

                $scope.openImpersonation = function () {
                    $scope.openList(
                        { signInType: 'Impersonation,ImpersonationRevert' },
                        'platform.blades.sign-in-log-dashboard.presets.impersonation');
                };

                $scope.openByIp = function (entry) {
                    $scope.openList(
                        { succeeded: 'false', ipAddress: entry.key },
                        'platform.blades.sign-in-log-dashboard.presets.by-ip');
                };

                $scope.openByUserName = function (entry) {
                    $scope.openList(
                        { succeeded: 'false', keyword: entry.key },
                        'platform.blades.sign-in-log-dashboard.presets.by-user');
                };

                $scope.openByFailureReason = function (entry) {
                    $scope.openList(
                        { succeeded: 'false', failureReason: entry.key },
                        'platform.blades.sign-in-log-dashboard.presets.by-reason');
                };

                blade.toolbarCommands = [
                    {
                        name: 'platform.commands.refresh', icon: 'fa fa-refresh',
                        executeMethod: function () { blade.refresh(); },
                        canExecuteMethod: function () { return true; }
                    },
                    {
                        name: 'platform.blades.sign-in-log-dashboard.commands.view-all', icon: 'fa fa-list',
                        executeMethod: function () { $scope.openAll(); },
                        canExecuteMethod: function () { return true; }
                    }
                ];

                blade.refresh();
            }]);
