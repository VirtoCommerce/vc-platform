angular.module('platformWebApp')
    .directive('uiScrollDropDown', [function () {
        const defaultPageSize = 20;
        const defaultResponseGroup = 'none';
        const defaultSelectedItemsPropertyName = 'objectIds';

        return {
            restrict: 'E',
            require: 'ngModel',
            replace: true,
            scope: {
                data: '&',
                onSelect: '&?',
                onRemove: '&?',
                filter: '&?',
                disabled: '=?',
                multiple: '=?',
                pageSize: '=?',
                placeholder: '=?',
                required: '=?',
                responseGroup: '=?',
                selectedItemsPropertyName: '=?' //TODO: delete this when storeIds/etc are changed to objectIds
            },
            templateUrl: '$(Platform)/Scripts/common/directives/uiScroll.tpl.html',
            link: function ($scope, element, attrs, ngModelController) {
                $scope.context = {
                    modelValue: null,
                    required: angular.isDefined(attrs.required) && (attrs.required === '' || attrs.required.toLowerCase() === 'true'),
                    multiple: angular.isDefined(attrs.multiple) && (attrs.multiple === '' || attrs.multiple.toLowerCase() === 'true')
                };

                $scope.items = [];
                $scope.isNoItems = true;
                // If binded data function returns a predefined list then disable pagination funciton
                $scope.paginationDisabled = false;

                // PageSize amount must be enough to show scrollbar in dropdown list container.
                // If scrollbar doesn't appear auto loading won't work.
                var pageSize = $scope.pageSize || defaultPageSize;
                var responseGroup = $scope.responseGroup || defaultResponseGroup;
                var selectedItemsPropertyName = $scope.selectedItemsPropertyName || defaultSelectedItemsPropertyName;
                var lastSearchPhrase = '';

                $scope.selectValue = (item, model) => {
                    if ($scope.onSelect) {
                        $scope.onSelect({ item: item, model: model });
                    }
                }

                $scope.removeValue = (item, model) => {
                    if ($scope.onRemove) {
                        $scope.onRemove({ item: item, model: model });
                    }
                }

                $scope.fetch = function ($select) {
                    load();

                    if (!$scope.disabled) {
                        $scope.fetchNext($select);
                    }
                };

                $scope.fetchNext = ($select) => {
                    if ($scope.paginationDisabled)
                        return;

                    $select.page = $select.page || 0;

                    if (lastSearchPhrase !== $select.search) {
                        lastSearchPhrase = $select.search;
                        $select.page = 0;
                    }

                    var criteria = {
                        searchPhrase: $select.search,
                        take: pageSize,
                        skip: $select.page * pageSize,
                        responseGroup: responseGroup
                    };

                    return $scope.data({ criteria: criteria }).$promise.then((x) => {
                        join(x.results, true);
                        $select.page++;

                        if ($select.page * pageSize < x.totalCount) {
                            $scope.$broadcast('scrollCompleted');
                        }
                    });
                };

                $scope.$watch('context.modelValue', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        ngModelController.$setViewValue($scope.context.modelValue);
                    }
                });

                ngModelController.$render = function () {
                    $scope.context.modelValue = ngModelController.$modelValue;
                };

                function load() {
                    var selectedIds = $scope.context.multiple ? $scope.context.modelValue : [$scope.context.modelValue];

                    if ($scope.isNoItems && _.any(selectedIds)) {
                        var criteria = {
                            take: selectedIds.length,
                            responseGroup: responseGroup
                        };

                        criteria[selectedItemsPropertyName] = selectedIds;

                        var result = $scope.data({ criteria: criteria });

                        if (result.$promise) {
                            result.$promise.then((x) => {
                                join(x.results);
                            });
                        }
                        else if (angular.isArray(result)) {
                            join(result);
                            $scope.paginationDisabled = true;
                        }
                    }
                }

                function join(newItems, callFilter) {
                    newItems = _.reject(newItems, x => _.any($scope.items, y => y.id === x.id));

                    if (callFilter) {
                        newItems = filterItems(newItems);
                    }

                    if (_.any(newItems)) {
                        $scope.items = $scope.items.concat(newItems);
                        $scope.isNoItems = $scope.items.length === 0;
                    }
                }

                function filterItems(items) {
                    if (!$scope.filter)
                        return items;

                    return $scope.filter({ items: items });
                }
            }
        }
    }]);
