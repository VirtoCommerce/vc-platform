angular.module('platformWebApp')
.directive('vaGenericInputsContainer', [function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            blade: '=',
            registeredInputs: '=',
            columnCount: '@?'
        },
        templateUrl: '$(Platform)/Scripts/common/directives/genericInputsContainer.tpl.html',
        link: function (scope) {
            scope.columnsCount = scope.columnCount ? parseInt(scope.columnCount, 10) : 1;

            var filteredInputs = _.filter(scope.registeredInputs, function (x) { return !x.isVisibleFn || x.isVisibleFn(scope.blade); });

            var resultingGroups = [];
            var isGroupStart = true, currentGroup;
            _.each(filteredInputs, function (x) {
                x = angular.copy(x);
                x.ngBindingModel = x.name;

                if (isGroupStart || (x.templateUrl && x.spanAllColumns)) {
                    isGroupStart = !isGroupStart;
                    currentGroup = [x];
                    resultingGroups.push(currentGroup);
                } else {
                    currentGroup.push(x);
                    isGroupStart = currentGroup.length == scope.columnCount;
                }
            });

            // generate empty columns
            _.each(resultingGroups, function (gr) {
                while (scope.columnsCount > gr.length) {
                    gr.push({ templateUrl: 'emptyColumn.html' });
                }
            });
            scope.bladeInputGroups = resultingGroups;

            scope.range = function (n) {
                return new Array(n);
            };
        }
    }
}])
//.component('vaGenericInputsContainer', {
//    templateUrl: '$(Platform)/Scripts/common/directives/genericInputsContainer.tpl.html',
//    bindings: {
//        blade: '=',
//        registeredInputs: '=',
//        columnCount: '@?'
//    },
//    controller: function () { }
//})
;