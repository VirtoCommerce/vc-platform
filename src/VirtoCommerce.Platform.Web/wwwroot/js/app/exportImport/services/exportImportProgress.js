// Shared helpers used by both export-main and import-main blade controllers.
// Single source of truth for:
//   * clipboard helpers (copyToClipboard / copyItemErrors / copyDetailedLog)
//   * progress log parsing (parseProgressLog) — the only per-blade variance is the verb
//     (Exporting/Imported... vs Importing/Imported...), wired via the `verb` parameter.
//   * shared scope vars and detailed-log toggle (progressItems, progressStats, showDetailedLog, toggleDetailedLog)
//
// Usage from a blade controller:
//   exportImportProgress.attach($scope, blade, 'export');  // or 'import'
//   // later, when a push notification arrives:
//   exportImportProgress.parseProgressLog($scope, blade);
angular.module('platformWebApp')
    .factory('platformWebApp.exportImport.progressService', [function () {
        var VERB_PATTERNS = {
            'export': {
                start: /^Exporting '(.+?)'/,
                success: /^Successfully exported '(.+?)'/,
                failure: /^Failed to export '(.+?)'/,
            },
            'import': {
                start: /^Importing '(.+?)'/,
                success: /^Successfully imported '(.+?)'/,
                failure: /^Failed to import '(.+?)'/,
            },
        };

        function copyToClipboard(text, $event) {
            if ($event) { $event.stopPropagation(); }
            if (!text) { return; }
            var done = function () { /* intentional no-op; reserved for future visual feedback */ };
            if (navigator.clipboard && navigator.clipboard.writeText) {
                navigator.clipboard.writeText(text).then(done, done);
            } else {
                var ta = document.createElement('textarea');
                ta.value = text;
                ta.style.position = 'fixed';
                ta.style.opacity = '0';
                document.body.appendChild(ta);
                ta.select();
                try { document.execCommand('copy'); } catch (e) { /* ignore */ }
                document.body.removeChild(ta);
                done();
            }
        }

        function parseProgressLog($scope, blade, verb) {
            var patterns = VERB_PATTERNS[verb];
            var progressLog = (blade.notification && blade.notification.progressLog) || [];
            var items = [];
            var currentItem = null;

            _.each(progressLog, function (entry) {
                var msg = entry.message || '';
                var startMatch = msg.match(patterns.start);

                if (startMatch) {
                    if (currentItem && currentItem.status === 'active') {
                        currentItem.status = 'done';
                    }
                    currentItem = {
                        id: items.length,
                        name: startMatch[1],
                        status: 'active',
                        messages: []
                    };
                    items.push(currentItem);
                } else if (entry.level === 'Error' && currentItem) {
                    currentItem.status = 'error';
                    currentItem.messages.push(entry);
                } else if (currentItem && currentItem.status === 'active') {
                    if (msg.match(patterns.success)) {
                        currentItem.status = 'done';
                    } else if (msg.match(patterns.failure)) {
                        currentItem.status = 'error';
                    }
                    currentItem.messages.push(entry);
                }
            });

            // Fallback: retro-attach legacy `errors[]` to the matching item by name match so a failed
            // item still shows red even if the backend reported errors out-of-band (no Error-level entry).
            var legacyErrors = (blade.notification && blade.notification.errors) || [];
            _.each(legacyErrors, function (errorText) {
                var match = _.find(items, function (i) {
                    return errorText && errorText.indexOf(i.name) !== -1;
                });
                if (match) {
                    match.status = 'error';
                    if (!_.any(match.messages, function (m) { return m.message === errorText; })) {
                        match.messages.push({ level: 'Error', message: errorText });
                    }
                }
            });

            // If errorCount > 0 but no item was flagged, mark the last finished item as error so the
            // user gets a visible breadcrumb (better than all-green-but-banner-says-errors).
            if (blade.notification && blade.notification.errorCount > 0 &&
                !_.any(items, function (i) { return i.status === 'error'; }) && items.length > 0) {
                var last = items[items.length - 1];
                last.status = 'error';
                _.each(legacyErrors, function (e) {
                    last.messages.push({ level: 'Error', message: e });
                });
            }

            var completed = _.filter(items, function (i) { return i.status === 'done' || i.status === 'error'; }).length;
            var total = (blade.notification && blade.notification.totalCount) || items.length;

            $scope.progressItems = items;
            $scope.progressStats = {
                total: total,
                completed: completed,
                percent: total > 0 ? Math.round((completed / total) * 100) : 0
            };
        }

        return {
            /**
             * Install shared scope members on $scope. `verb` is 'export' or 'import' — selects the
             * regex patterns used by parseProgressLog. Must be called once during controller init.
             */
            attach: function ($scope, blade, verb) {
                if (!VERB_PATTERNS[verb]) {
                    throw new Error("exportImportProgress: unknown verb '" + verb + "'. Expected 'export' or 'import'.");
                }
                $scope.progressItems = [];
                $scope.progressStats = { total: 0, completed: 0, percent: 0 };
                $scope.showDetailedLog = false;
                $scope.toggleDetailedLog = function () { $scope.showDetailedLog = !$scope.showDetailedLog; };
                $scope.copyToClipboard = copyToClipboard;
                $scope.copyItemErrors = function (item, $event) {
                    var lines = _.pluck(item.messages || [], 'message');
                    copyToClipboard((item.name ? item.name + '\n' : '') + lines.join('\n'), $event);
                };
                $scope.copyDetailedLog = function ($event) {
                    var entries = (blade.notification && blade.notification.progressLog) || [];
                    var lines = _.map(entries, function (e) { return `[${e.level || 'Info'}] ${e.message || ''}`; });
                    copyToClipboard(lines.join('\n'), $event);
                };
                // Stash the verb so callers can refresh from `new-notification-event` without
                // re-passing it. Hidden under a single-prefix scope key.
                $scope._exportImportVerb = verb;
            },
            /**
             * Recompute $scope.progressItems / progressStats from blade.notification.progressLog.
             * Call from the controller's "new-notification-event" handler after copying the notification.
             */
            parseProgressLog: function ($scope, blade) {
                parseProgressLog($scope, blade, $scope._exportImportVerb);
            },
        };
    }]);
