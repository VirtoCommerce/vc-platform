angular.module('platformWebApp')
    .directive('vaFilterPanel', ['$document', '$rootScope', function ($document, $rootScope) {
        // Shared coordination event so only one transient overlay (this filter panel,
        // a row context menu, ...) is visible at a time. Payload is the opener's kind;
        // an overlay ignores events from its own kind (same-kind coordination is handled locally).
        var overlayOpenedEvent = 'platformWebApp.transientOverlay.opened';
        var overlayKind = 'filterPanel';
        return {
            restrict: 'E',
            templateUrl: '$(Platform)/Scripts/common/directives/filterPanel.tpl.html',
            transclude: true,
            scope: {
                hasActiveFilters: '&',
                onClearFilters: '&',
                searchText: '=?',
                hideSearch: '=?',
                searchPlaceholder: '@?',
                filterTitle: '@?',
                hideFilter: '=?'
            },
            link: function (scope, element) {
                scope.showPanel = false;

                scope.togglePanel = function ($event) {
                    if ($event) {
                        $event.stopPropagation();
                    }
                    scope.showPanel = !scope.showPanel;
                    if (scope.showPanel) {
                        // Opening this panel dismisses any other open transient overlay.
                        $rootScope.$broadcast(overlayOpenedEvent, overlayKind);
                    }
                };

                scope.clearFilters = function () {
                    scope.onClearFilters();
                    scope.showPanel = false;
                };

                // Close panel on outside click
                function onDocumentClick(e) {
                    if (scope.showPanel && !element[0].contains(e.target)) {
                        scope.$apply(function () {
                            scope.showPanel = false;
                        });
                    }
                }

                // Close this panel when another kind of transient overlay opens.
                var unsubscribeOverlay = scope.$on(overlayOpenedEvent, function (event, sourceKind) {
                    if (sourceKind !== overlayKind && scope.showPanel) {
                        scope.showPanel = false;
                    }
                });

                $document.on('click', onDocumentClick);
                scope.$on('$destroy', function () {
                    $document.off('click', onDocumentClick);
                    unsubscribeOverlay();
                });
            }
        };
    }]);
