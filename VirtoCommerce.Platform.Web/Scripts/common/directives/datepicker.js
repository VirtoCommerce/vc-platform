angular.module('ui.bootstrap.datepicker')
.config(['$provide', function ($provide) {
    $provide.decorator('datepickerDirective', ['$delegate', '$locale', function ($delegate, $locale) {
        var directive = $delegate[0];
        directive.compile = function () {
            return function (scope, element, attrs, ctrls) {
                var controller = ctrls[0];
                // 0 is Sunday in angular-js and Monday in angular-ui datepicker
                var firstDayOfWeek = $locale.DATETIME_FORMATS.FIRSTDAYOFWEEK + 1;
                firstDayOfWeek = firstDayOfWeek === 7 ? 0 : firstDayOfWeek;
                controller.startingDay = firstDayOfWeek;
                controller.currentText = controller.currentText ? controller.currentText : "{{ platform.datepicker.current }}";
                directive.link.apply(this, arguments);
            }
        };
        return $delegate;
    }]);
    $provide.decorator('datepickerPopupDirective', ['$delegate', '$filter', function ($delegate, $filter) {
        var directive = $delegate[0];
        directive.compile = function () {
            return function (scope, element, attrs, ctrls) {
                attrs.currentText = attrs.currentText || $filter('translate')('platform.commands.today');
                attrs.clearText = attrs.clearText || $filter('translate')('platform.commands.clear');
                attrs.closeText = attrs.closeText || $filter('translate')('platform.commands.close');
                directive.link.apply(this, arguments);
            }
        };
        return $delegate;
    }]);
}]);