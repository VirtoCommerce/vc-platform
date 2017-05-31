angular.module('platformWebApp')
.config(['$provide', function ($provide) {
    // Provide default format
    $provide.decorator('currencyFilter', ['$delegate', function ($delegate) {
        var filter = function (currency, symbol, fractionSize) {
            var result = $delegate.apply(this, [currency, "¤", fractionSize]).replace(/\s*¤\s*/g, "");
            if (symbol) {
                result += "\u00a0" + symbol;
            }
            return result;
        };
        return filter;
    }]);
    // Add support for moment
    $provide.decorator('dateFilter', ['$delegate', 'moment', function ($delegate, moment) {
        var filter = function (date, format, timeZone) {
            if (moment.isMoment(date)) {
                timeZone = date.format("ZZ");
                date = date.toDate();
            }
            return $delegate.apply(this, [date, format, timeZone]);
        };
        return filter;
    }]);
    // Add support for tag type 'number'
    $provide.decorator('tagsInputDirective', ['$delegate', 'platformWebApp.numberFormat', 'tiUtil', function ($delegate, numberFormat, tiUtil) {
        var directive = $delegate[0];
        directive.compile = function () {
            return function (scope, element, attrs) {

                /* Copy-paste, because there is no extension point */

                var options = scope.options;
                var events = scope.events;
                var onTagAdding = tiUtil.handleUndefinedResult(scope.onTagAdding, true);

                var getTagValue = function (tag) {
                    return tiUtil.safeToString(tag[options.displayProperty]);
                };
                var setTagValue = function (tag, text) {
                    tag[options.displayProperty] = text;
                };

                var tagIsValid = function (tag) {
                    var tagText = getTagValue(tag);

                    // Validate tag
                    var isNumber = angular.isDefined(attrs.tagsNumber);
                    var value = undefined;
                    if (isNumber) {
                        value = numberFormat.validate(tagText, attrs.numType, attrs.min, attrs.max, attrs.fraction);
                        if (angular.isDefined(value)) {
                            setTagValue(tag, value);
                        }
                    }
                    return tagText &&
                        tagText.length >= options.minLength &&
                        tagText.length <= options.maxLength &&
                        options.allowedTagsPattern.test(tagText) &&
                        !tiUtil.findInObjectArray(scope.tagList.items, tag, options.keyProperty || options.displayProperty) &&
                        (!isNumber || angular.isDefined(value)) &&
                        onTagAdding({ $tag: tag });
                };

                scope.tagList.add = function (tag) {
                    var tagText = getTagValue(tag);

                    if (options.replaceSpacesWithDashes) {
                        tagText = tiUtil.replaceSpacesWithDashes(tagText);
                    }

                    setTagValue(tag, tagText);

                    if (tagIsValid(tag)) {
                        scope.tagList.items.push(tag);
                        events.trigger('tag-added', { $tag: tag });
                    }
                    else if (tagText) {
                        events.trigger('invalid-tag', { $tag: tag });
                    }

                    return tag;
                };

                if (angular.isDefined(attrs.tagsNumber)) {
                    angular.extend(scope.options, { tagsNumber: attrs.tagsNumber, numType: attrs.numType, min: attrs.min, max: attrs.max, fraction: attrs.fraction });
                }

                directive.link.apply(this, arguments);
            };
        };
        return $delegate;
    }]);
    $provide.decorator('tiTagItemDirective', ['$delegate', 'platformWebApp.numberFormat', function ($delegate, numberFormat) {
        var directive = $delegate[0];
        var compile = directive.compile;
        directive.compile = function () {
            var link = compile.apply(this, arguments);
            return function (scope, element, attrs, tagsInputCtrl) {
                link.apply(this, arguments);

                // Format tag
                // Not real double-registration, this method just return object with needed methods
                var tagsInput = tagsInputCtrl.registerTagItem();
                var options = tagsInput.getOptions();
                if (angular.isDefined(options.tagsNumber)) {
                    scope.$getDisplayText = function () {
                        return numberFormat.format(scope.data[options.displayProperty], options.numType, options.min, options.max, options.fraction);
                    }
                }
            };
        };
        return $delegate;
    }]);
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
    $provide.decorator('datepickerPopupDirective', ['$delegate', 'datepickerPopupConfig', '$filter', 'platformWebApp.angularToMomentFormatConverter', '$locale',
    function ($delegate, datepickerPopupConfig, $filter, formatConverter, $locale) {
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
                        formatConverter.validate(format, formatConverter.isInvalidDate);
                        if (formatConverter.additionalFormats.includes(format)) {
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
                ngModelCtrl.$formatters.splice(1, 1, function (value) {
                    var format = attrs.datepickerPopup;
                    scope.date = value;
                    return ngModelCtrl.$isEmpty(value) ? value : $filter('date')(moment(value), format);
                });
                ngModelCtrl.$parsers.unshift(function (value) {
                    var output = undefined;
                    if (value) {
                        var format = formatConverter.convert(attrs.datepickerPopup);
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