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
    $provide.decorator('datepickerPopupDirective', ['$delegate', 'datepickerPopupConfig', '$filter', 'platformWebApp.dateUtils', '$locale',
    function ($delegate, datepickerPopupConfig, $filter, dateUtils, $locale) {
        var directive = $delegate[0];
        directive.compile = function (tElem, tAttrs) {
            tElem.attr("datepicker-popup-original", tAttrs.datepickerPopup);
            return function (scope, element, attrs, ngModelCtrl) {
                attrs.currentText = attrs.currentText || $filter('translate')('platform.commands.today');
                attrs.clearText = attrs.clearText || $filter('translate')('platform.commands.clear');
                attrs.closeText = attrs.closeText || $filter('translate')('platform.commands.close');

                // datepicker has some bugs and limitations to support date & time formats,
                // also, it doesn't support localized input,
                // so limit format number & convert to date via moment to prevent random occurence of errors
                var applyFormat = function (newFormat, oldFormat) {
                    if (newFormat !== oldFormat) {
                        var format = newFormat || datepickerPopupConfig.datepickerPopup;
                        dateUtils.validate(format, dateUtils.isInvalidDate);
                        if (dateUtils.additionalFormats.includes(format)) {
                            format = $locale.DATETIME_FORMATS[format];
                        }
                        attrs.datepickerPopup = format;
                    }
                };
                attrs.$observe('datepickerPopupOriginal', function (value, oldValue) {
                    applyFormat(value, oldValue);
                });
                applyFormat(attrs.datepickerPopup, undefined);

                directive.link.apply(this, arguments);
                
                // convert localized date to javascript date object for correct validation
                ngModelCtrl.$parsers.unshift(function(value) {
                    var output = null;
                    if (value) {
                        var format = dateUtils.convert(attrs.datepickerPopup);
                        var date = moment(value, format, moment.locale(), true);
                        output = date.isValid() ? date.toDate() : undefined;
                    }
                    return output;
                });
            }
        };
        return $delegate;
    }]);
}]);