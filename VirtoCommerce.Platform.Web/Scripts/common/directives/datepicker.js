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
                directive.link.apply(this, arguments);
            }
        };
        return $delegate;
    }]);
    $provide.decorator('datepickerPopupDirective', ['$delegate', 'datepickerPopupConfig', '$filter', '$locale', '$log', function ($delegate, datepickerPopupConfig, $filter, $locale, $log) {
        var directive = $delegate[0];
        var compile = directive.compile;
        directive.compile = function (tElement, tAttrs) {
            // datepicker has some bugs and limitations to support date & time formats,
            // also, it doesn't support localized input,
            // so limit format number to prevent random occurence of errors
            // Formats 'YYYY', 'YY', 'Y', 'DD', 'D'
            var formats = ['yyyy', 'yy', 'y', 'MMMM', 'MMM', 'MM', 'M', 'dd', 'd', 'EEEE', 'EEE'];
            var invalidFormats = ['Q', 'Do', 'DDD', 'ddd', 'DDDD', 'dddd', 'e', 'E', 'X', 'x',
                'HH', 'H', 'hh', 'h', 'kk', 'k', 'mm', 'm', 's', 'S', 'ss', 'SS', 'sss', 'SSS', 'a', 'A', 'Z', 'ZZ', 'ww', 'WW', 'w', 'W', 'G', 'GG', 'gg', 'GGG', 'GGGG', 'gggg'];
            var invalidAdditionalFormats = ['medium', 'short', 'mediumTime', 'shortTime'];
            var additionalFormats = ['fullDate', 'longDate', 'mediumDate', 'shortDate'];
            var format = tAttrs.datepickerPopup ? tAttrs.datepickerPopup : datepickerPopupConfig.datepickerPopup;
            var formatParts = format.split(" ");
            var isInvalid = (_.intersection(invalidFormats, formatParts).length > 0 || invalidAdditionalFormats.includes(format)) && !additionalFormats.includes(format);
            if (isInvalid) {
                $log.error("Invalid date format for datepicker popup");
            }
            if (additionalFormats.includes(format)) {
                format = $locale.DATETIME_FORMATS[format];
            }
            tAttrs.datepickerPopup = format;

            var link = compile.apply(this, arguments);
            return function (scope, element, attrs, ngModelCtrl) {
                attrs.currentText = attrs.currentText || $filter('translate')('platform.commands.today');
                attrs.clearText = attrs.clearText || $filter('translate')('platform.commands.clear');
                attrs.closeText = attrs.closeText || $filter('translate')('platform.commands.close');

                link.apply(this, arguments);
                
                // convert localized date to javascript date object for correct validation
                ngModelCtrl.$parsers.unshift(value => {
                    var output = null;
                    if (value) {
                        formatParts = attrs.datepickerPopup.split(" ");
                        for (var i = 0; i < formatParts.length; i++) {
                            if (formats.includes(formatParts[i])) {
                                formatParts[i] = formatParts[i].toUpperCase();
                            }
                        }
                        var date = moment.utc(value, formatParts.join(" ").replace(/'/g, ""), moment.locale(), true);
                        output = date.isValid() ? date.format() : undefined;
                    }
                    return output;
                });
            }
        };
        return $delegate;
    }]);
}]);