angular.module('platformWebApp')
    .config(['$provide', function ($provide) {
        // Provide default format
        $provide.decorator('currencyFilter', ['$delegate', function ($delegate) {
            var filter = function (currency, symbol, fractionSize) {
                currency = $delegate.apply(this, [currency, "¤", fractionSize]);
                var result = currency ? currency.replace(/\s*¤\s*/g, "") : currency;
                if (result && symbol) {
                    result += "\u00a0" + symbol;
                }
                return result;
            };
            return filter;
        }]);

        // Add support for moment
        $provide.decorator('dateFilter', ['$delegate', 'moment', 'platformWebApp.i18n', 'platformWebApp.common.timeZones', function ($delegate, moment, i18n, timeZones) {
            var filter = function (date, format, timeZone) {
                if (moment.isMoment(date)) {
                    timeZone = timeZones.get(i18n.getTimeZone()).utcOffset.formatted.replace(':', '');
                    date = date.toDate();
                }
                return $delegate.apply(this, [date, format, timeZone]);
            };
            return filter;
        }]);

        // Add support for tag type 'number'
        $provide.decorator('tagsInputDirective', ['$delegate', 'platformWebApp.numberFormat', 'tiUtil', function ($delegate, numberFormat, tiUtil) {
            var directive = $delegate[0];
            var INTEGER_MAX_VALUE = 2147483647;
            var INTEGER_MIN_VALUE = -2147483648;
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
                        var isValidInteger = true;
                        if (isNumber) {
                            value = numberFormat.validate(tagText, attrs.numType, attrs.min, attrs.min, attrs.max, attrs.max, attrs.fraction);
                            if (angular.isDefined(value)) {
                                setTagValue(tag, value);
                            }
                            if (attrs.numType === "integer") {
                                if (value > INTEGER_MAX_VALUE || value < INTEGER_MIN_VALUE) {
                                    isValidInteger = false;
                                }
                            }
                        }
                        return tagText &&
                            tagText.length >= options.minLength &&
                            tagText.length <= options.maxLength &&
                            options.allowedTagsPattern.test(tagText) &&
                            !tiUtil.findInObjectArray(scope.tagList.items, tag, options.keyProperty || options.displayProperty) &&
                            (!isNumber || angular.isDefined(value)) &&
                            onTagAdding({ $tag: tag }) && isValidInteger;
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
                            return numberFormat.format(scope.data[options.displayProperty], options.numType, options.min, options.min, options.max, options.max, options.fraction);
                        }
                    }
                };
            };
            return $delegate;
        }]);

        // Fix bugs & add features for datepicker popup
        $provide.decorator('datepickerPopupDirective', ['$delegate', 'platformWebApp.angularToMomentFormatConverter', 'uiDatetimePickerConfig', 'timepickerConfig', '$filter', '$locale', 'moment', 'platformWebApp.i18n', 'platformWebApp.common.timeZones',
            function ($delegate, formatConverter, datepickerPopupConfig, timepickerConfig, $filter, $locale, moment, i18n, timeZones) {
                //delete bootstrap directive
                $delegate.shift();

                //use custom date time picker directive
                var directive = $delegate[0];

                // Offset (minutes west of UTC) for the user's profile timezone; same source as dateFilter decorator
                // so the popup's wall-clock matches the input field exactly.
                function getUserTzOffsetMinutes(sampleDate) {
                    var tz = timeZones.get(i18n.getTimeZone());
                    var fallback = sampleDate ? sampleDate.getTimezoneOffset() : 0;
                    if (!tz || !tz.utcOffset || !tz.utcOffset.formatted) {
                        return fallback;
                    }
                    var offsetStr = tz.utcOffset.formatted.replace(':', '');
                    var parsed = Date.parse('Jan 01, 1970 00:00:00 ' + offsetStr) / 60000;
                    return isNaN(parsed) ? fallback : parsed;
                }

                // Shift a real instant into a Date whose browser-local fields render the user's profile-TZ wall-clock.
                function toUserTzWallClock(value) {
                    if (!value) return value;
                    var d = angular.isDate(value) ? new Date(value.getTime()) : new Date(value);
                    var diff = getUserTzOffsetMinutes(d) - d.getTimezoneOffset();
                    return new Date(d.getTime() - diff * 60000);
                }

                // Reverse of toUserTzWallClock — used when the picker commits back to the model.
                function fromUserTzWallClock(value) {
                    if (!value) return value;
                    var d = angular.isDate(value) ? new Date(value.getTime()) : new Date(value);
                    var diff = getUserTzOffsetMinutes(d) - d.getTimezoneOffset();
                    return new Date(d.getTime() + diff * 60000);
                }

                directive.compile = function (tElem, tAttrs) {
                    tElem.attr("datepicker-popup-original", tAttrs.datepickerPopup);
                    return function (scope, element, attrs, ctrls) {
                        var ngModelCtrl = ctrls[0];
                        // datepicker has some bugs and limitations to support date & time formats,
                        // also, it doesn't support localized input,
                        // so limit format number & convert to date via moment to prevent random occurence of errors
                        var applyFormat = function (newFormat, oldFormat) {
                            if (newFormat !== oldFormat) {
                                var format = newFormat || datepickerPopupConfig.dateFormat;
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

                        // scope.date is bound to the internal date/time pickers. Shift it to the user-profile-TZ wall-clock
                        // so the popup renders the same hours/minutes as the input field (which formats via user TZ
                        // through the dateFilter decorator below).
                        ngModelCtrl.$formatters.splice(1, 1, function (value) {
                            var format = attrs.datepickerPopup;
                            if (ngModelCtrl.$isEmpty(value)) {
                                scope.date = value;
                                return value;
                            }
                            scope.date = toUserTzWallClock(value);
                            return $filter('date')(moment(value), format);
                        });

                        ngModelCtrl.$parsers.unshift(function (value) {
                            if (!value) {
                                //Allow to enter empty value
                                return value;
                            }
                            // Picker commits a Date representing user-TZ wall-clock — convert back to the real instant.
                            if (angular.isDate(value)) {
                                return fromUserTzWallClock(value);
                            }
                            var format = formatConverter.convert(attrs.datepickerPopup);
                            var date = moment(value, format, moment.locale(), true);
                            return date.isValid() ? date.toDate() : undefined;
                        });

                        // "Now" / "Today" buttons in the popup use new Date() (browser wall-clock); shift so they land
                        // in user-TZ wall-clock like the rest of the picker state.
                        var originalSelect = scope.select;
                        if (angular.isFunction(originalSelect)) {
                            scope.select = function (opt) {
                                if (opt === 'today' || opt === 'now') {
                                    var nowShifted = toUserTzWallClock(new Date());
                                    var date;
                                    if (angular.isDate(scope.date)) {
                                        date = new Date(scope.date);
                                        date.setFullYear(nowShifted.getFullYear(), nowShifted.getMonth(), nowShifted.getDate());
                                        date.setHours(nowShifted.getHours(), nowShifted.getMinutes(), nowShifted.getSeconds(), nowShifted.getMilliseconds());
                                    } else {
                                        date = nowShifted;
                                    }
                                    scope.dateSelection(date);
                                    return;
                                }
                                return originalSelect.apply(this, arguments);
                            };
                        }
                    }
                };
                return $delegate;
            }]);

        $provide.decorator('timepickerDirective', ['$delegate', 'timepickerConfig', '$locale', 'platformWebApp.settings.helper', 'platformWebApp.i18n', 'platformWebApp.userProfile',
            function ($delegate, timepickerConfig, $locale, settings, i18n, userProfile) {
                $delegate.shift();
                var directive = $delegate[0];

                var timeSettings = userProfile.timeSettings;
                timepickerConfig.showMeridian = timeSettings.showMeridian;

                var compile = directive.compile;
                directive.compile = function (tElem, tAttrs) {
                    var link = compile.apply(this, arguments);
                    return function (scope, element, attrs, ctrls) {
                        //set 24 hour format
                        timeSettings = i18n.getTimeSettings();
                        scope.showMeridian = timeSettings.showMeridian;

                        function cnahgeTimeSettings(showMeridian) {
                            timeSettings.showMeridian = showMeridian;
                            i18n.changeTimeSettings(timeSettings);
                            userProfile.save();
                        }

                        scope.$watch('showMeridian', function (showMeridian) {
                            cnahgeTimeSettings(showMeridian);
                        });

                        link.apply(this, arguments);
                    };
                };
                return $delegate;
            }]);
    }]);
