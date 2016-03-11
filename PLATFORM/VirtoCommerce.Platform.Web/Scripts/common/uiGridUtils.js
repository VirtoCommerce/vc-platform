angular.module('platformWebApp')
    .config(['$provide', 'uiGridConstants', function ($provide, uiGridConstants) {
        $provide.decorator('GridOptions', ['$delegate', function ($delegate) {
            var gridOptions = angular.copy($delegate);
            gridOptions.initialize = function (options) {
                var initOptions = $delegate.initialize(options);
                angular.extend(initOptions, {
                    data: _.any(initOptions.data) ? initOptions.data : 'blade.currentEntities',
                    rowHeight: initOptions.rowHeight === 30 ? 40 : initOptions.rowHeight,
                    enableGridMenu: true,
                    //enableVerticalScrollbar: uiGridConstants.scrollbars.NEVER,
                    //enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
                    saveFocus: false,
                    saveFilter: false,
                    saveGrouping: false,
                    savePinning: false,
                    saveSelection: false
                });
                return initOptions;
            };
            return gridOptions;
        }]);
    }])

    .factory('platformWebApp.uiGridHelper', ['$localStorage', '$timeout', 'uiGridConstants', '$translate', function ($localStorage, $timeout, uiGridConstants, $translate) {
        var retVal = {};
        retVal.uiGridConstants = uiGridConstants;
        retVal.initialize = function ($scope, gridOptions, externalRegisterApiCallback) {
            var savedState = $localStorage['gridState:' + $scope.blade.template];
            if (savedState) {
                // extend saved columns with custom columnDef information (e.g. cellTemplate, displayName)
                var foundDef;
                _.each(savedState.columns, function (x) {
                    if (foundDef = _.findWhere(gridOptions.columnDefs, { name: x.name })) {
                        var customSort = x.sort;
                        _.extend(x, foundDef);
                        x.sort = customSort;
                        x.wasPredefined = true;
                        gridOptions.columnDefs.splice(gridOptions.columnDefs.indexOf(foundDef), 1);
                    } else {
                        x.wasPredefined = false;
                    }
                });
                savedState.columns = _.reject(savedState.columns, function (x) { return x.cellTemplate && !x.wasPredefined; });
                gridOptions.columnDefs = _.union(gridOptions.columnDefs, savedState.columns);
            } else {
                // mark predefined columns
                _.each(gridOptions.columnDefs, function (x) { x.wasPredefined = true; })
            }

            // translate filter
            _.each(gridOptions.columnDefs, function (x) { x.headerCellFilter = 'translate'; })

            $scope.gridOptions = angular.extend({
                gridMenuTitleFilter: $translate,
                onRegisterApi: function (gridApi) {
                    //set gridApi on scope
                    $scope.gridApi = gridApi;

                    if (savedState) {
                        $timeout(function () {
                            gridApi.saveState.restore($scope, savedState);
                        }, 10);
                    }

                    gridApi.colResizable.on.columnSizeChanged($scope, saveState);
                    gridApi.colMovable.on.columnPositionChanged($scope, saveState);
                    gridApi.core.on.columnVisibilityChanged($scope, saveState);
                    gridApi.core.on.sortChanged($scope, saveState);
                    function saveState() {
                        $localStorage['gridState:' + $scope.blade.template] = gridApi.saveState.save();
                    }

                    gridApi.grid.registerDataChangeCallback(processMissingColumns, [uiGridConstants.dataChange.ROW]);


                    if (externalRegisterApiCallback) {
                        externalRegisterApiCallback(gridApi);
                    }
                }
            }, gridOptions);

            function processMissingColumns(grid) {
                var gridOptions = grid.options;

                if (!gridOptions.columnDefsGenerated && _.any(grid.rows)) {
                    var filteredColumns = _.filter(_.pairs(grid.rows[0].entity), function (x) {
                        return x[0] !== '$$hashKey' && !_.isArray(x[1]);
                    });

                    var allKeysFromEntity = _.map(filteredColumns, function (x) { return x[0]; });
                    // remove non-existing columns
                    _.each(gridOptions.columnDefs.slice(), function (x) {
                        if (!_.contains(allKeysFromEntity, x.name) && !x.wasPredefined) {
                            gridOptions.columnDefs = _.reject(gridOptions.columnDefs, function (d) { return d.name == x.name; });
                        }
                    });

                    // generate columnDefs for each undefined property
                    _.each(allKeysFromEntity, function (x) {
                        if (!_.findWhere(gridOptions.columnDefs, { name: x })) {
                            gridOptions.columnDefs.push({ name: x, visible: false });
                        }
                    });
                    gridOptions.columnDefsGenerated = true;
                }
            }
        };

        retVal.getSortExpression = function ($scope) {
            var columnDefs = $scope.gridApi ? $scope.gridApi.grid.columns : $scope.gridOptions.columnDefs;
            var sorts = _.filter(columnDefs, function (x) {
                return x.name !== '$path' && x.sort && (x.sort.direction === uiGridConstants.ASC || x.sort.direction === uiGridConstants.DESC);
            });

            sorts = _.sortBy(sorts, function (x) {
                return x.sort.priority;
            });
            sorts = _.map(sorts, function (x) {
                return (x.field ? x.field : x.name) + ':' + (x.sort.direction === uiGridConstants.ASC ? 'asc' : 'desc');
            });
            return sorts.join(';');
        };

        retVal.bindRefreshOnSortChanged = function ($scope) {
            $scope.gridApi.core.on.sortChanged($scope, function () {
                if (!$scope.blade.isLoading) $scope.blade.refresh();
            });
        };

        return retVal;
    }])
    // auto height and additional class for ui-grid
    .directive('uiGridHeight', ['$timeout', '$window', function ($timeout, $window) {
        return {
            restrict: 'A',
            link: {
                pre: function (scope, element) {
                    var bladeInner = $(element).parents('.blade-inner');
                    bladeInner.addClass('ui-grid-no-scroll');

                    var setGridHeight = function () {
                        $timeout(function () {
                            $(element).height(bladeInner.height());
                        });
                    };
                    scope.$watch('pageSettings.totalItems', setGridHeight);
                    angular.element($window).bind('resize', setGridHeight);
                }
            }
        };
    }])
;