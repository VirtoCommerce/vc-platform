angular.module('platformWebApp')
    .controller('platformWebApp.signInLogDashboardController',
        ['$scope', 'platformWebApp.accounts', 'platformWebApp.bladeNavigationService',
            function ($scope, accounts, bladeNavigationService) {
                var blade = $scope.blade;

                blade.title = blade.title || 'platform.blades.sign-in-log-dashboard.title';
                blade.subtitle = blade.subtitle || 'platform.blades.sign-in-log-dashboard.subtitle';
                blade.headIcon = 'fas fa-clipboard-list';

                blade.periods = [
                    { label: 'platform.blades.sign-in-log.filter.period-30m', value: '30m', minutes: 30 },
                    { label: 'platform.blades.sign-in-log.filter.period-1h', value: '1h', minutes: 60 },
                    { label: 'platform.blades.sign-in-log.filter.period-24h', value: '24h', minutes: 1440 },
                    { label: 'platform.blades.sign-in-log.filter.period-7d', value: '7d', minutes: 1440 * 7 }
                ];

                blade.period = '24h';

                function periodStartDate(periodValue) {
                    var period = _.findWhere(blade.periods, { value: periodValue });
                    if (!period || !period.minutes) {
                        return null;
                    }

                    return new Date(Date.now() - period.minutes * 60000).toISOString();
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
                        blade.chart = buildChart(stats);
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

                // Plain SVG bars: the marks are rectangles, so a charting library would add ~40 KB
                // to the admin bundle for geometry we can compute here.
                var CHART = { width: 660, height: 126, left: 34, right: 8, top: 8, bottom: 22 };

                function buildChart(stats) {
                    var points = stats.timeline || [];

                    if (!points.length) {
                        return null;
                    }

                    var peak = 0;
                    points.forEach(function (p) {
                        peak = Math.max(peak, p.succeededCount + p.failedCount);
                    });

                    // A flat-zero window still needs a scale, otherwise every bar divides by zero.
                    var scaleMax = Math.max(peak, 1);
                    var plotWidth = CHART.width - CHART.left - CHART.right;
                    var plotHeight = CHART.height - CHART.top - CHART.bottom;
                    var slot = plotWidth / points.length;
                    var barWidth = Math.max(1, Math.min(18, slot - 2));
                    var baseline = CHART.top + plotHeight;

                    var bars = points.map(function (p, i) {
                        var x = CHART.left + i * slot + (slot - barWidth) / 2;
                        var okHeight = Math.round(p.succeededCount / scaleMax * plotHeight);
                        var badHeight = Math.round(p.failedCount / scaleMax * plotHeight);

                        return {
                            x: x,
                            width: barWidth,
                            okY: baseline - okHeight,
                            okHeight: okHeight,
                            // 2px surface gap keeps the two fills from reading as one block.
                            badY: baseline - okHeight - badHeight - (okHeight && badHeight ? 2 : 0),
                            badHeight: badHeight,
                            point: p,
                            tooltip: `${formatBucket(p.timestamp, stats.timelineGranularity)} — ${p.succeededCount} ok, ${p.failedCount} failed`
                        };
                    });

                    return {
                        bars: bars,
                        baseline: baseline,
                        peak: peak,
                        scaleMax: scaleMax,
                        gridlines: [0, 0.5, 1].map(function (f) {
                            return { y: baseline - f * plotHeight, label: Math.round(f * scaleMax) };
                        }),
                        ticks: buildTicks(points, stats.timelineGranularity, slot),
                        granularity: stats.timelineGranularity
                    };
                }

                function formatBucket(timestamp, granularity) {
                    var d = new Date(timestamp);

                    if (granularity === 'Day') {
                        return d.toLocaleDateString(undefined, { month: 'short', day: 'numeric' });
                    }

                    return d.toLocaleTimeString(undefined, { hour: '2-digit', minute: '2-digit' });
                }

                // At most six labels, whatever the bucket count, so they never collide.
                function buildTicks(points, granularity, slot) {
                    var stride = Math.max(1, Math.ceil(points.length / 6));
                    var ticks = [];

                    for (var i = 0; i < points.length; i += stride) {
                        ticks.push({
                            x: CHART.left + i * slot + slot / 2,
                            label: formatBucket(points[i].timestamp, granularity)
                        });
                    }

                    return ticks;
                }

                $scope.chartGeometry = CHART;

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
                            label: `platform.blades.sign-in-log.stats.${d.label}`,
                            hint: `platform.blades.sign-in-log.stats.${d.label}-hint`,
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

                // userName, not keyword: the panel groups by the exact column, while keyword is a
                // substring match that also spans the operator name and the IP. Drilling into
                // "admin - 6" through keyword returned b2badmin@test.com, admin@vc-demostore.com and
                // rows where admin was only the operator - a list that did not match the number clicked.
                $scope.openByUserName = function (entry) {
                    $scope.openList(
                        { succeeded: 'false', userName: entry.key },
                        'platform.blades.sign-in-log-dashboard.presets.by-user');
                };

                $scope.openByStore = function (entry) {
                    $scope.openList({ storeId: entry.key }, 'platform.blades.sign-in-log-dashboard.presets.by-store');
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
