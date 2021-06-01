angular.module('platformWebApp')
    .directive('uiScrollDropDown2', [function() {
        const defaultPageSize = 20;
        const defaultResponseGroup = 'none';
        const defaultSelectedItemsPropertyName = 'objectIds';

        return {
            restrict: 'E',
            require: 'ngModel',
            replace: true,
            scope: {
                data: '&',
                disabled: '=?',
                multiple: '=?',
                pageSize: '=?',
                placeholder: '=?',
                required: '=?',
                entitiesToHide: '=?',
                responseGroup: '=?',
                selectedItemsPropertyName: '=?'
            },
            templateUrl: '$(Platform)/Scripts/common/directives/uiScroll2.tpl.html',
            link: function ($scope, element, attrs, ngModelController) {
                $scope.context = {
                    modelValue: null,
                    required: angular.isDefined(attrs.required) && (attrs.required === '' || attrs.required.toLowerCase() === 'true'),
                    multiple: angular.isDefined(attrs.multiple) && (attrs.multiple === '' || attrs.multiple.toLowerCase() === 'true')
                };

                // PageSize amount must be enough to show scrollbar in dropdown list container.
                // If scrollbar doesn't appear auto loading won't work.
                var pageSize = $scope.pageSize || defaultPageSize;
                var responseGroup = $scope.responseGroup || defaultResponseGroup;
                var selectedItemsPropertyName = $scope.selectedItemsPropertyName || defaultSelectedItemsPropertyName;

                $scope.items = [];
                $scope.isNoItems = true;
                var lastSearchPhrase = '';
                var totalCount = 0;
                var hiddenCount = angular.isArray($scope.entitiesToHide) ? $scope.entitiesToHide.length : 0;

                $scope.fetch = function ($select) {
                    load();

                    if (!$scope.disabled) {
                        $scope.fetchNext($select);
                    }
                };

                $scope.fetchNext = ($select) => {
                    //if (!$select.open)
                    //    return;

                    $select.page = $select.page || 0;

                    if (lastSearchPhrase !== $select.search && totalCount > $scope.items.length) {
                        lastSearchPhrase = $select.search;
                        $select.page = 0;
                    }

                    var criteria = {
                        searchPhrase: $select.search,
                        take: pageSize,
                        skip: $select.page * pageSize,
                        responseGroup: responseGroup
                    };

                    return $scope.data({ criteria: criteria }).$promise.then((data) => {
                            join(data.results);
                            $select.page++;

                            if ($select.page * pageSize < data.totalCount) {
                                $scope.$broadcast('scrollCompleted');
                            }

                            totalCount = Math.max(totalCount, data.totalCount - hiddenCount);
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

                        $scope.data({ criteria: criteria }).$promise.then((data) => {
                            join(data.results);
                        });
                    }
                }

                function join(newItems) {
                    newItems = _.reject(newItems, x => _.any($scope.items, y => y.id === x.id) || _.indexOf($scope.entitiesToHide, x.id) > -1);
                    if (_.any(newItems)) {
                        $scope.items = $scope.items.concat(newItems);
                        $scope.isNoItems = $scope.items.length === 0;
                    }
                }
            }
        }
    }]);
