angular.module('platformWebApp')
.directive('vaUiGridAutoResize', ['gridUtil', function (gridUtil) {
    return {
        require: 'uiGrid',
        scope: false,
        link: function ($scope, $elm, $attrs, uiGridCtrl) {
            $scope.$watch(function() {
                return {
                    height: gridUtil.elementHeight($elm),
                    width: gridUtil.elementWidth($elm)
                };
            }, function (newDimensions, oldDimensions) {
                if (newDimensions.height !== oldDimensions.height || newDimensions.width !== oldDimensions.width) {
                    uiGridCtrl.grid.gridHeight = newDimensions.height;
                    uiGridCtrl.grid.gridWidth = newDimensions.width;
                    uiGridCtrl.grid.api.core.raise.gridDimensionChanged(oldDimensions.height, oldDimensions.width, newDimensions.height, newDimensions.width);
                    uiGridCtrl.grid.refresh();
                }
            }, true);
        }
    };
}]);