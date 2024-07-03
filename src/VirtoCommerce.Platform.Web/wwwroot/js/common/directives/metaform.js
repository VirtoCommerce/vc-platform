angular.module('platformWebApp')
    .factory('platformWebApp.metaFormsService', [function () {
        var registeredMetaFields = {};

        return {
            registerMetaFields: function (metaFormName, metaFields) {
                if (!registeredMetaFields[metaFormName]) {
                    registeredMetaFields[metaFormName] = [];
                }
                Array.prototype.push.apply(registeredMetaFields[metaFormName], metaFields);
                registeredMetaFields[metaFormName] = _.sortBy(registeredMetaFields[metaFormName], 'priority');
            },
            getMetaFields: function (metaFormName) {
                return registeredMetaFields[metaFormName];
            },
            unregisterMetaFields: function (metaFormName, ...metaFieldsToFind) {
                metaFieldsToFind.forEach(metaFieldToFind => this.unregisterMetaField(metaFormName, metaFieldToFind));
            },
            unregisterMetaFieldWithTemplateUrl: function (metaFormName, templateUrl) {
                this.unregisterMetaField(metaFormName, x => x.templateUrl === templateUrl, `template URL '${templateUrl}'`);
            },
            unregisterMetaField: function (metaFormName, metaFieldToFind, metaFieldDescription) {
                let metaFieldIndex;
                const metaFields = registeredMetaFields[metaFormName];

                // metaFieldToFind can be a search filter, a metaField name or the metaField object itself (which is the previous way of unregistering a metaField)
                if (_.isFunction(metaFieldToFind)) {
                    metaFieldIndex = _.findIndex(metaFields, metaFieldToFind);
                } else if (typeof metaFieldToFind === 'string') {
                    metaFieldIndex = _.findIndex(metaFields, x => x.name === metaFieldToFind);
                    metaFieldDescription ??= `name '${metaFieldToFind}'`;
                } else {
                    metaFieldIndex = _.findIndex(metaFields, metaFieldToFind);
                }

                if (metaFieldIndex < 0) {
                    throw new Error(metaFieldDescription
                        ? `The metaForm '${metaFormName}' doesn't contain a field with ${metaFieldDescription}, the field could not be removed.`
                        : `The metaForm '${metaFormName}' doesn't contain the field to be removed.`);
                }

                metaFields.splice(metaFieldIndex, 1);
            },
            clearMetaFields: function (metaFormName) {
                registeredMetaFields[metaFormName] = [];
            }
        };
    }])
    .directive('vaMetaform', [function () {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                blade: '=',
                registeredInputs: '=',
                columnCount: '@?'
            },
            templateUrl: '$(Platform)/Scripts/common/directives/metaform.tpl.html',
            link: function (scope) {
                var columnCount = scope.columnCount ? parseInt(scope.columnCount, 10) : 1;

                var filteredInputs = _.filter(scope.registeredInputs, function (x) { return !x.isVisibleFn || x.isVisibleFn(scope.blade); });

                var resultingRows = [];
                var currentRow;
                var columnCountInRow = columnCount;
                _.each(filteredInputs, function (x) {
                    x = angular.copy(x);
                    x.ngBindingModel = x.name;
                    x.colSpan = x.colSpan || (x.spanAllColumns ? columnCount : 1);
                    columnCountInRow += x.colSpan;
                    let isNewRow = columnCountInRow > columnCount;

                    if (isNewRow) {
                        currentRow = [x];
                        resultingRows.push(currentRow);
                        columnCountInRow = x.colSpan;
                    } else {
                        currentRow.push(x);
                    }
                });

                // generate empty columns
                _.each(resultingRows, function (row) {
                    var colSpans = _.reduce(row, function (memo, item) { return memo + item.colSpan; }, 0);
                    for (var i = colSpans; i < columnCount; i++) {
                        row.push({ templateUrl: 'emptyColumn.html' });
                    }
                });
                scope.bladeInputGroups = resultingRows;
            }
        }
    }]);
