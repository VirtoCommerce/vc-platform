angular.module('platformWebApp')
.factory('platformWebApp.metaFormsService', [function () {
    var registeredMetaFields = { };

    return {
        registerMetaFields: function (metaFormName, metaFields) {
            if (!registeredMetaFields[metaFormName]) {
                registeredMetaFields[metaFormName] = [];
            }
            Array.prototype.push.apply(registeredMetaFields[metaFormName], metaFields);
            registeredMetaFields[metaFormName] = _.sortBy(registeredMetaFields[metaFormName], 'priority');
        },
        getMetaFields: function(metaFormName) {
            return registeredMetaFields[metaFormName];
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

            var resultingGroups = [];
            var isGroupStart = true, currentGroup;
            _.each(filteredInputs, function (x) {
                x = angular.copy(x);
                x.ngBindingModel = x.name;

                if (isGroupStart || (x.templateUrl && x.spanAllColumns)) {
                    currentGroup = [x];
                    resultingGroups.push(currentGroup);
                } else {
                    currentGroup.push(x);
                }
                isGroupStart = (x.templateUrl && x.spanAllColumns) || currentGroup.length == columnCount;
            });

            // generate empty columns
            _.each(resultingGroups, function (gr) {
                while (columnCount > gr.length) {
                    gr.push({ templateUrl: 'emptyColumn.html' });
                }
            });
            scope.bladeInputGroups = resultingGroups;
        }
    }
}]);