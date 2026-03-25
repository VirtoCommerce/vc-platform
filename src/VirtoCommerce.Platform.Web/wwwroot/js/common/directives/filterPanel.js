angular.module('platformWebApp')
    .directive('vaFilterPanel', ['$document', function ($document) {
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
                filterTitle: '@?'
            },
            link: function (scope, element) {
                scope.showPanel = false;

                scope.togglePanel = function ($event) {
                    if ($event) {
                        $event.stopPropagation();
                    }
                    scope.showPanel = !scope.showPanel;
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

                $document.on('click', onDocumentClick);
                scope.$on('$destroy', function () {
                    $document.off('click', onDocumentClick);
                });
            }
        };
    }]);
