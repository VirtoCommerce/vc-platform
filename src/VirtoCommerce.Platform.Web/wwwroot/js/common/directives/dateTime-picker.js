//replace datetimeDirective
angular.module('platformWebApp')
    .constant('uiDatetimePickerConfig',
        {
            dateFormat: 'medium',
            defaultTime: '00:00:00',
            html5Types: {
                date: 'yyyy-MM-dd',
                'datetime-local': 'yyyy-MM-ddTHH:mm:ss.sss',
                'month': 'yyyy-MM'
            },
            initialPicker: 'date',
            reOpenDefault: false,
            enableDate: true,
            enableTime: true,
            buttonBar: {
                show: true,
                now: {
                    show: true
                },
                today: {
                    show: true
                },
                clear: {
                    show: true
                },
                date: {
                    show: true
                },
                time: {
                    show: true
                },
                close: {
                    show: true
                }
            },
            closeOnDateSelection: false,
            appendToBody: false,
            altInputFormats: [],
            ngModelOptions: {},
            showMeridian: false
        })
    .controller('DateTimePickerController',
        [
            '$scope', '$element', '$attrs', '$compile', '$parse', '$document', '$timeout', '$position', 'dateFilter',
            'dateParser', 'uiDatetimePickerConfig', '$rootScope', 'timepickerConfig',
            function (scope, element, attrs, $compile, $parse, $document, $timeout, $uibPosition, dateFilter, uibDateParser, uiDatetimePickerConfig, $rootScope, timepickerConfig) {
                var dateFormat = uiDatetimePickerConfig.dateFormat, ngModel, ngModelOptions, $popup,
                    cache = {},
                    watchListeners = [],
                    closeOnDateSelection = angular.isDefined(attrs.closeOnDateSelection)
                        ? scope.$parent.$eval(attrs.closeOnDateSelection)
                        : uiDatetimePickerConfig.closeOnDateSelection,
                    appendToBody = angular.isDefined(attrs.datepickerAppendToBody)
                        ? scope.$parent.$eval(attrs.datepickerAppendToBody)
                        : uiDatetimePickerConfig.appendToBody,
                    altInputFormats = angular.isDefined(attrs.altInputFormats)
                        ? scope.$parent.$eval(attrs.altInputFormats)
                        : uiDatetimePickerConfig.altInputFormats;

                // taken from UI Bootstrap 2.5.0
                uibDateParser.fromTimezone = function fromTimezone(date, timezone) {
                    return date && timezone ? convertTimezoneToLocal(date, timezone, true) : date;
                }

                uibDateParser.toTimezone = function toTimezone(date, timezone) {
                    return date && timezone ? convertTimezoneToLocal(date, timezone) : date;
                }

                function timezoneToOffset(timezone, fallback) {
                    timezone = timezone.replace(/:/g, '');
                    var requestedTimezoneOffset = Date.parse('Jan 01, 1970 00:00:00 ' + timezone) / 60000;
                    return isNaN(requestedTimezoneOffset) ? fallback : requestedTimezoneOffset;
                }

                function addDateMinutes(date, minutes) {
                    date = new Date(date.getTime());
                    date.setMinutes(date.getMinutes() + minutes);
                    return date;
                }

                function convertTimezoneToLocal(date, timezone, reverse) {
                    reverse = reverse ? -1 : 1;
                    var dateTimezoneOffset = date.getTimezoneOffset();
                    var timezoneOffset = timezoneToOffset(timezone, dateTimezoneOffset);
                    return addDateMinutes(date, reverse * (timezoneOffset - dateTimezoneOffset));
                }

                this.init = function (_ngModel) {
                    ngModel = _ngModel;
                    ngModelOptions = ngModel.$options || uiDatetimePickerConfig.ngModelOptions;

                    scope.watchData = {};
                    scope.buttonBar = angular.isDefined(attrs.buttonBar)
                        ? scope.$parent.$eval(attrs.buttonBar)
                        : uiDatetimePickerConfig.buttonBar;

                    // determine which pickers should be available. Defaults to date and time
                    scope.enableDate = angular.isDefined(scope.enableDate)
                        ? scope.enableDate
                        : uiDatetimePickerConfig.enableDate;
                    scope.enableTime = angular.isDefined(scope.enableTime)
                        ? scope.enableTime
                        : uiDatetimePickerConfig.enableTime;

                    // determine default picker
                    scope.initialPicker = angular.isDefined(attrs.initialPicker)
                        ? attrs.initialPicker
                        : (scope.enableDate ? uiDatetimePickerConfig.initialPicker : 'time');

                    // determine the picker to open when control is re-opened
                    scope.reOpenDefault = angular.isDefined(attrs.reOpenDefault)
                        ? attrs.reOpenDefault
                        : uiDatetimePickerConfig.reOpenDefault;

                    // check if an illegal combination of options exists
                    if (scope.initialPicker == 'date' && !scope.enableDate) {
                        throw new Error(
                            "datetimePicker can't have initialPicker set to date and have enableDate set to false.");
                    }

                    // default picker view
                    scope.showPicker = !scope.enableDate ? 'time' : scope.initialPicker;

                    var isHtml5DateInput = false;

                    if (uiDatetimePickerConfig.html5Types[attrs.type]) {
                        dateFormat = uiDatetimePickerConfig.html5Types[attrs.type];
                        isHtml5DateInput = true;
                    } else {
                        dateFormat = attrs.datepickerPopup || uiDatetimePickerConfig.dateFormat;
                        attrs.$observe('datetimePicker',
                            function (value) {
                                var newDateFormat = value || uiDatetimePickerConfig.dateFormat;

                                if (newDateFormat !== dateFormat) {
                                    dateFormat = newDateFormat;
                                    ngModel.$modelValue = null;

                                    if (!dateFormat) {
                                        throw new Error('datetimePicker must have a date format specified.');
                                    }
                                }
                            });
                    }

                    // popup element used to display calendar
                    var popupEl = angular.element('' +
                        '<div date-picker-wrap>' +
                        '<div datepicker></div>' +
                        '</div>' +
                        '<div time-picker-wrap>' +
                        '<div timepicker style="margin:0 auto"></div>' +
                        '</div>');

                    scope.ngModelOptions = angular.copy(ngModelOptions);
                    scope.ngModelOptions.timezone = null;

                    // get attributes from directive
                    popupEl.attr({
                        'ng-model': 'date',
                        'ng-model-options': 'ngModelOptions',
                        'ng-change': 'dateSelection(date)'
                    });

                    // datepicker element
                    var datepickerEl = angular.element(popupEl.children()[0]);

                    if (isHtml5DateInput) {
                        if (attrs.type === 'month') {
                            datepickerEl.attr('datepicker-mode', '"month"');
                            datepickerEl.attr('min-mode', 'month');
                        }
                    }

                    if (attrs.datepickerOptions) {
                        var options = scope.$parent.$eval(attrs.datepickerOptions);

                        if (options && options.initDate) {
                            scope.initDate = uibDateParser.fromTimezone(options.initDate, ngModelOptions.timezone);
                            datepickerEl.attr('init-date', 'initDate');
                            delete options.initDate;
                        }

                        angular.forEach(options,
                            function (value, option) {
                                datepickerEl.attr(cameltoDash(option), value);
                            });
                    }

                    // set datepickerMode to day by default as need to create watch
                    // else disabled cannot pass in mode
                    if (!angular.isDefined(attrs['datepickerMode'])) {
                        attrs['datepickerMode'] = 'day';
                    }

                    if (attrs.dateDisabled) {
                        datepickerEl.attr('date-disabled', 'dateDisabled({ date: date, mode: mode })');
                    }

                    angular.forEach([
                        'formatDay', 'formatMonth', 'formatYear', 'formatDayHeader', 'formatDayTitle',
                        'formatMonthTitle', 'showWeeks', 'startingDay', 'yearRows', 'yearColumns'
                    ],
                        function (key) {
                            if (angular.isDefined(attrs[key])) {
                                datepickerEl.attr(cameltoDash(key), attrs[key]);
                            }
                        });

                    if (attrs.customClass) {
                        datepickerEl.attr('custom-class', 'customClass({ date: date, mode: mode })');
                    }

                    angular.forEach(['minMode', 'maxMode', 'datepickerMode', 'shortcutPropagation'], function (key) {
                        if (attrs[key]) {
                            var getAttribute = $parse(attrs[key]);

                            watchListeners.push(scope.$parent.$watch(getAttribute, function (value) {
                                scope.watchData[key] = value;
                            }));

                            datepickerEl.attr(cameltoDash(key), 'watchData.' + key);

                            // Propagate changes from datepicker to outside
                            if (key === 'datepickerMode') {
                                var setAttribute = getAttribute.assign;
                                watchListeners.push(scope.$watch('watchData.' + key, function (value, oldvalue) {
                                    if (angular.isFunction(setAttribute) && value !== oldvalue) {
                                        setAttribute(scope.$parent, value);
                                    }
                                }));
                            }
                        }
                    });

                    // timepicker element
                    var timepickerEl = angular.element(popupEl.children()[1]);

                    if (attrs.timepickerOptions) {
                        var options = scope.$parent.$eval(attrs.timepickerOptions);
                        angular.forEach(options, function (value, option) {
                            scope.watchData[option] = value;
                            timepickerEl.attr(cameltoDash(option), 'watchData.' + option);
                        });
                    }

                    // watch attrs - NOTE: minDate and maxDate are used with datePicker and timePicker.  By using the minDate and maxDate
                    // with the timePicker, you can dynamically set the min and max time values.  This cannot be done using the min and max values
                    // with the timePickerOptions
                    angular.forEach(['minDate', 'maxDate', 'initDate'], function (key) {
                        if (attrs[key]) {
                            var getAttribute = $parse(attrs[key]);

                            watchListeners.push(scope.$parent.$watch(getAttribute, function (value) {
                                scope.watchData[key] = value;
                            }));

                            datepickerEl.attr(cameltoDash(key), 'watchData.' + key);

                            if (key == 'minDate') {
                                timepickerEl.attr('min', 'watchData.minDate');
                            } else if (key == 'maxDate')
                                timepickerEl.attr('max', 'watchData.maxDate');
                        }
                    });

                    // do not check showWeeks attr, as should be used via datePickerOptions

                    if (!isHtml5DateInput) {
                        // Internal API to maintain the correct ng-invalid-[key] class
                        ngModel.$$parserName = 'datetime';
                        ngModel.$validators.datetime = validator;
                        ngModel.$parsers.unshift(parseDate);
                        ngModel.$formatters.push(function (value) {
                            if (ngModel.$isEmpty(value)) {
                                scope.date = value;
                                return value;
                            }
                            scope.date = uibDateParser.fromTimezone(value, ngModelOptions.timezone);
                            dateFormat = dateFormat.replace(/M!/, 'MM')
                                .replace(/d!/, 'dd');

                            return dateFilter(scope.date, dateFormat);
                        });
                    } else {
                        ngModel.$formatters.push(function (value) {
                            scope.date = uibDateParser.fromTimezone(value, ngModelOptions.timezone);
                            return value;
                        });
                    }

                    // Detect changes in the view from the text box
                    ngModel.$viewChangeListeners.push(function () {
                        scope.date = parseDateString(ngModel.$viewValue);
                    });

                    element.bind('keydown', inputKeydownBind);

                    $popup = $compile(popupEl)(scope);
                    // Prevent jQuery cache memory leak (template is now redundant after linking)
                    popupEl.remove();

                    if (appendToBody) {
                        $document.find('body').append($popup);
                    } else {
                        element.after($popup);
                    }
                };

                // get text
                scope.getText = function (key) {
                    return scope.buttonBar[key].text || uiDatetimePickerConfig.buttonBar[key].text;
                };

                // determine if button is to be shown or not
                scope.doShow = function (key) {
                    if (angular.isDefined(scope.buttonBar[key].show))
                        return scope.buttonBar[key].show;
                    else
                        return uiDatetimePickerConfig.buttonBar[key].show;
                };

                // Inner change
                scope.dateSelection = function (dt) {
                    // check if timePicker is being shown and merge dates, so that the date
                    // part is never changed, only the time

                    //set date whis this time (00:00:00:0000)
                    if (scope.enableTime && scope.showPicker === 'date') {
                        var defaultTimeForDate = 0;

                        // only proceed if dt is a date
                        if (dt || dt != null) {
                            // check if our scope.date is null, and if so, set to todays date
                            if (!angular.isDefined(scope.date) || scope.date == null) {
                                scope.date = new Date();
                            }

                            // dt will not be undefined if the now or today button is pressed
                            if (dt && dt != null) {
                                // get the existing date and update the time
                                var date = new Date(dt);
                                date.setHours(defaultTimeForDate);
                                date.setMinutes(defaultTimeForDate);
                                date.setSeconds(defaultTimeForDate);
                                date.setMilliseconds(defaultTimeForDate);
                                dt = date;
                            }
                        }
                    }

                    if (scope.enableTime && scope.showPicker === 'time') {
                        // only proceed if dt is a date
                        if (dt || dt != null) {
                            // check if our scope.date is null, and if so, set to todays date
                            if (!angular.isDefined(scope.date) || scope.date == null) {
                                scope.date = new Date();
                            }

                            // dt will not be undefined if the now or today button is pressed
                            if (dt && dt != null) {
                                // get the existing date and update the time
                                var date = new Date(scope.date);
                                date.setHours(dt.getHours());
                                date.setMinutes(dt.getMinutes());
                                date.setSeconds(dt.getSeconds());
                                date.setMilliseconds(dt.getMilliseconds());
                                dt = date;
                            }
                        }
                    }

                    if (angular.isDefined(dt)) {
                        if (!scope.date) {
                            var defaultTime = angular.isDefined(attrs.defaultTime)
                                ? attrs.defaultTime
                                : uiDatetimePickerConfig.defaultTime;
                            var t = new Date('2001-01-01 ' + defaultTime);

                            if (!isNaN(t) && dt != null) {
                                dt.setHours(t.getHours());
                                dt.setMinutes(t.getMinutes());
                                dt.setSeconds(t.getSeconds());
                                dt.setMilliseconds(t.getMilliseconds());
                            }
                        }
                        scope.date = dt;
                    }

                    var newDate = scope.date ? dateFilter(scope.date, dateFormat, ngModelOptions.timezone) : null;

                    element.val(newDate);
                    ngModel.$setViewValue(scope.date);

                    if (closeOnDateSelection) {
                        // do not close when using timePicker as make impossible to choose a time
                        if (scope.showPicker != 'time' && date != null) {
                            // if time is enabled, swap to timePicker
                            if (scope.enableTime) {
                                scope.open('time');
                            } else {
                                scope.close(false);
                            }
                        }
                    }
                };

                scope.keydown = function (evt) {
                    if (evt.which === 27) {
                        scope.close(false);
                        element[0].focus();
                    }
                };

                scope.$watch('isOpen',
                    function (value) {
                        scope.dropdownStyle = {
                            display: value ? 'block' : 'none'
                        };

                        if (value) {
                            cache['openDate'] = scope.date;

                            var position = appendToBody ? $uibPosition.offset(element) : $uibPosition.position(element);

                            if (appendToBody) {
                                scope.dropdownStyle.top = (position.top + element.prop('offsetHeight')) + 'px';
                            } else {
                                scope.dropdownStyle.top = undefined;
                            }

                            scope.dropdownStyle.left = position.left + 'px';

                            $timeout(function () { scope.$broadcast('datepicker.focus'); $document.bind('click', documentClickBind); }, 0, false); scope.open(scope.showPicker);
                        } else {
                            $document.unbind('click', documentClickBind);
                        }
                    });

                scope.isDisabled = function (date) {
                    if (date === 'today' || date === 'now') {
                        date = new Date();
                    }

                    return scope.watchData.minDate && scope.compare(date, scope.watchData.minDate) < 0 ||
                        scope.watchData.maxDate && scope.compare(date, scope.watchData.maxDate) > 0;
                };

                scope.compare = function (date1, date2) {
                    return new Date(date1.getFullYear(), date1.getMonth(), date1.getDate()) -
                        new Date(date2.getFullYear(), date2.getMonth(), date2.getDate());
                };

                scope.select = function (opt) {
                    var date = null;
                    if (opt === 'today' || opt == 'now') {
                        var now = new Date();
                        if (angular.isDate(scope.date)) {
                            date = new Date(scope.date);
                            date.setFullYear(now.getFullYear(), now.getMonth(), now.getDate());
                            date.setHours(now.getHours(), now.getMinutes(), now.getSeconds(), now.getMilliseconds());
                        } else {
                            date = now;
                        }
                    }

                    scope.dateSelection(date);

                    if (opt == 'clear')
                        scope.close();
                };

                scope.open = function (picker, evt) {
                    if (angular.isDefined(evt)) {
                        evt.preventDefault();
                        evt.stopPropagation();
                    }

                    // need to delay this, else timePicker never shown
                    $timeout(function () {
                        scope.showPicker = picker;
                    },
                        0);

                    // in order to update the timePicker, we need to update the model reference!
                    // as found here https://angular-ui.github.io/bootstrap/#/timepicker
                    $timeout(function () {
                        if (scope.date == null) {
                            scope.date = new Date();
                        } else {
                            scope.date = new Date(scope.date);
                        }
                    },
                        50);
                };

                scope.close = function (closePressed) {
                    scope.isOpen = false;

                    // if enableDate and enableTime are true, reopen the picker in date mode first
                    if (scope.enableDate && scope.enableTime)
                        scope.showPicker = scope.reOpenDefault === false ? 'date' : scope.reOpenDefault;

                    // if a on-close-fn has been defined, lets call it
                    // we only call this if closePressed is defined!
                    if (angular.isDefined(closePressed))
                        scope.whenClosed({
                            args: {
                                closePressed: closePressed,
                                openDate: cache['openDate'] || null,
                                closeDate: scope.date
                            }
                        });

                    element[0].focus();
                };

                scope.$on('$destroy',
                    function () {
                        if (scope.isOpen === true) {
                            if (!$rootScope.$$phase) {
                                scope.$apply(function () {
                                    scope.close();
                                });
                            }
                        }

                        watchListeners.forEach(function (a) { a(); });
                        $popup.remove();
                        element.unbind('keydown', inputKeydownBind);
                        $document.unbind('click', documentClickBind);
                    });

                function documentClickBind(evt) {
                    var popup = $popup[0];
                    var dpContainsTarget = element[0].contains(evt.target);

                    // The popup node may not be an element node
                    // In some browsers (IE only) element nodes have the 'contains' function
                    var popupContainsTarget = popup.contains !== undefined && popup.contains(evt.target);

                    if (scope.isOpen && !(dpContainsTarget || popupContainsTarget)) {
                        scope.$apply(function () {
                            scope.close(false);
                        });
                    }
                }

                function inputKeydownBind(evt) {
                    if (evt.which === 27 && scope.isOpen) {
                        evt.preventDefault();
                        evt.stopPropagation();
                        scope.$apply(function () {
                            scope.close(false);
                        });
                        element[0].focus();
                    } else if (evt.which === 40 && !scope.isOpen) {
                        evt.preventDefault();
                        evt.stopPropagation();
                        scope.$apply(function () {
                            scope.isOpen = true;
                        });
                    }
                }

                function cameltoDash(string) {
                    return string.replace(/([A-Z])/g, function ($1) { return '-' + $1.toLowerCase(); });
                }

                function parseDateString(viewValue) {
                    var date = uibDateParser.parse(viewValue, dateFormat, scope.date);
                    if (isNaN(date)) {
                        for (var i = 0; i < altInputFormats.length; i++) {
                            date = uibDateParser.parse(viewValue, altInputFormats[i], scope.date);
                            if (!isNaN(date)) {
                                return date;
                            }
                        }
                    }
                    return date;
                }

                function parseDate(viewValue) {
                    if (angular.isNumber(viewValue)) {
                        // presumably timestamp to date object
                        viewValue = new Date(viewValue);
                    }

                    if (!viewValue) {
                        return null;
                    } else if (angular.isDate(viewValue) && !isNaN(viewValue)) {
                        return viewValue;
                    } else if (angular.isString(viewValue)) {
                        var date = parseDateString(viewValue);
                        if (!isNaN(date)) {
                            return uibDateParser.toTimezone(date, ngModelOptions.timezone);
                        }

                        return undefined;
                    } else {
                        return undefined;
                    }
                }

                function validator(modelValue, viewValue) {
                    var value = modelValue || viewValue;

                    if (!(attrs.ngRequired || attrs.required) && !value) {
                        return true;
                    }

                    if (angular.isNumber(value)) {
                        value = new Date(value);
                    }

                    if (!value) {
                        return true;
                    } else if (angular.isDate(value) && !isNaN(value)) {
                        return true;
                    } else if (angular.isDate(new Date(value)) && !isNaN(new Date(value).valueOf())) {
                        return true;
                    } else if (angular.isString(value)) {
                        return !isNaN(parseDateString(viewValue));
                    } else {
                        return false;
                    }
                }
            }
        ])
    .directive('datepickerPopup',
        function () {
            return {
                restrict: 'A',
                require: ['ngModel', 'datepickerPopup'],
                controller: 'DateTimePickerController',
                scope: {
                    isOpen: '=?',
                    enableDate: '=?',
                    enableTime: '=?',
                    initialPicker: '=?',
                    reOpenDefault: '=?',
                    dateDisabled: '&',
                    customClass: '&',
                    whenClosed: '&'
                },
                link: function (scope, element, attrs, ctrls) {
                    var ngModel = ctrls[0],
                        ctrl = ctrls[1];

                    ctrl.init(ngModel);
                }
            };
        })
    .directive('datePickerWrap',
        function () {
            return {
                restrict: 'EA',
                replace: true,
                transclude: true,
                templateUrl: 'template/date-picker.html'
            };
        })
    .directive('timePickerWrap',
        function () {
            return {
                restrict: 'EA',
                replace: true,
                transclude: true,
                templateUrl: 'template/time-picker.html'
            };
        });

//replace timeDirective
angular.module('platformWebApp')
    .constant('timepickerConfig', {
        hourStep: 1,
        minuteStep: 1,
        showMeridian: true,
        meridians: null,
        readonlyInput: false,
        mousewheel: true,
        arrowkeys: true,
        showSpinners: true
    })

    .controller('TimepickerController', ['$scope', '$attrs', '$parse', '$log', '$locale', 'timepickerConfig', function ($scope, $attrs, $parse, $log, $locale, timepickerConfig) {
        var selected = new Date(),
            ngModelCtrl = { $setViewValue: angular.noop }, // nullModelCtrl
            meridians = angular.isDefined($attrs.meridians) ? $scope.$parent.$eval($attrs.meridians) : timepickerConfig.meridians || $locale.DATETIME_FORMATS.AMPMS;

        var _24Hour = '24 Hrs';

        this.init = function (ngModelCtrl_, inputs) {
            ngModelCtrl = ngModelCtrl_;
            ngModelCtrl.$render = this.render;

            ngModelCtrl.$formatters.unshift(function (modelValue) {
                return modelValue ? new Date(modelValue) : null;
            });

            var hoursInputEl = inputs.eq(0),
                minutesInputEl = inputs.eq(1);

            var mousewheel = angular.isDefined($attrs.mousewheel) ? $scope.$parent.$eval($attrs.mousewheel) : timepickerConfig.mousewheel;
            if (mousewheel) {
                this.setupMousewheelEvents(hoursInputEl, minutesInputEl);
            }

            var arrowkeys = angular.isDefined($attrs.arrowkeys) ? $scope.$parent.$eval($attrs.arrowkeys) : timepickerConfig.arrowkeys;
            if (arrowkeys) {
                this.setupArrowkeyEvents(hoursInputEl, minutesInputEl);
            }

            $scope.readonlyInput = angular.isDefined($attrs.readonlyInput) ? $scope.$parent.$eval($attrs.readonlyInput) : timepickerConfig.readonlyInput;
            this.setupInputEvents(hoursInputEl, minutesInputEl);
        };

        var hourStep = timepickerConfig.hourStep;
        if ($attrs.hourStep) {
            $scope.$parent.$watch($parse($attrs.hourStep), function (value) {
                hourStep = parseInt(value, 10);
            });
        }

        var minuteStep = timepickerConfig.minuteStep;
        if ($attrs.minuteStep) {
            $scope.$parent.$watch($parse($attrs.minuteStep), function (value) {
                minuteStep = parseInt(value, 10);
            });
        }

        // 12H / 24H mode
        $scope.showMeridian = timepickerConfig.showMeridian;
        if ($attrs.showMeridian) {
            $scope.$parent.$watch($parse($attrs.showMeridian), function (value) {
                $scope.showMeridian = !!value;

                if (ngModelCtrl.$error.time) {
                    // Evaluate from template
                    var hours = getHoursFromTemplate(), minutes = getMinutesFromTemplate();
                    if (angular.isDefined(hours) && angular.isDefined(minutes)) {
                        selected.setHours(hours);
                        refresh();
                    }
                } else {
                    updateTemplate();
                }
            });
        }

        // Get $scope.hours in 24H mode if valid
        function getHoursFromTemplate() {
            var hours = parseInt($scope.hours, 10);
            var valid = ($scope.showMeridian) ? (hours > 0 && hours < 13) : (hours >= 0 && hours < 24);
            if (!valid) {
                return undefined;
            }

            if ($scope.showMeridian) {
                if (hours === 12) {
                    hours = 0;
                }
                if ($scope.meridian === meridians[1]) {
                    hours = hours + 12;
                }
            }
            return hours;
        }

        function getMinutesFromTemplate() {
            var minutes = parseInt($scope.minutes, 10);
            return (minutes >= 0 && minutes < 60) ? minutes : undefined;
        }

        function pad(value) {
            return (angular.isDefined(value) && value.toString().length < 2) ? '0' + value : value.toString();
        }

        // Respond on mousewheel spin
        this.setupMousewheelEvents = function (hoursInputEl, minutesInputEl) {
            var isScrollingUp = function (e) {
                if (e.originalEvent) {
                    e = e.originalEvent;
                }
                //pick correct delta variable depending on event
                var delta = (e.wheelDelta) ? e.wheelDelta : -e.deltaY;
                return (e.detail || delta > 0);
            };

            hoursInputEl.bind('mousewheel wheel', function (e) {
                $scope.$apply((isScrollingUp(e)) ? $scope.incrementHours() : $scope.decrementHours());
                e.preventDefault();
            });

            minutesInputEl.bind('mousewheel wheel', function (e) {
                $scope.$apply((isScrollingUp(e)) ? $scope.incrementMinutes() : $scope.decrementMinutes());
                e.preventDefault();
            });
        };

        // Respond on up/down arrowkeys
        this.setupArrowkeyEvents = function (hoursInputEl, minutesInputEl) {
            hoursInputEl.bind('keydown', function (e) {
                if (e.which === 38) { // up
                    e.preventDefault();
                    $scope.incrementHours();
                    $scope.$apply();
                }
                else if (e.which === 40) { // down
                    e.preventDefault();
                    $scope.decrementHours();
                    $scope.$apply();
                }
            });

            minutesInputEl.bind('keydown', function (e) {
                if (e.which === 38) { // up
                    e.preventDefault();
                    $scope.incrementMinutes();
                    $scope.$apply();
                }
                else if (e.which === 40) { // down
                    e.preventDefault();
                    $scope.decrementMinutes();
                    $scope.$apply();
                }
            });
        };

        this.setupInputEvents = function (hoursInputEl, minutesInputEl) {
            if ($scope.readonlyInput) {
                $scope.updateHours = angular.noop;
                $scope.updateMinutes = angular.noop;
                return;
            }

            var invalidate = function (invalidHours, invalidMinutes) {
                ngModelCtrl.$setViewValue(null);
                ngModelCtrl.$setValidity('time', false);
                if (angular.isDefined(invalidHours)) {
                    $scope.invalidHours = invalidHours;
                }
                if (angular.isDefined(invalidMinutes)) {
                    $scope.invalidMinutes = invalidMinutes;
                }
            };

            $scope.updateHours = function () {
                var hours = getHoursFromTemplate();

                if (angular.isDefined(hours)) {
                    selected.setHours(hours);
                    refresh('h');
                } else {
                    invalidate(true);
                }
            };

            hoursInputEl.bind('blur', function (e) {
                if (!$scope.invalidHours && $scope.hours < 10) {
                    $scope.$apply(function () {
                        $scope.hours = pad($scope.hours);
                    });
                }
            });

            $scope.updateMinutes = function () {
                var minutes = getMinutesFromTemplate();

                if (angular.isDefined(minutes)) {
                    selected.setMinutes(minutes);
                    refresh('m');
                } else {
                    invalidate(undefined, true);
                }
            };

            minutesInputEl.bind('blur', function (e) {
                if (!$scope.invalidMinutes && $scope.minutes < 10) {
                    $scope.$apply(function () {
                        $scope.minutes = pad($scope.minutes);
                    });
                }
            });
        };

        this.render = function () {
            var date = ngModelCtrl.$viewValue;

            if (isNaN(date)) {
                ngModelCtrl.$setValidity('time', false);
                $log.error('Timepicker directive: "ng-model" value must be a Date object, a number of milliseconds since 01.01.1970 or a string representing an RFC2822 or ISO 8601 date.');
            } else {
                if (date) {
                    selected = date;
                }
                makeValid();
                updateTemplate();
            }
        };

        // Call internally when we know that model is valid.
        function refresh(keyboardChange) {
            makeValid();
            ngModelCtrl.$setViewValue(new Date(selected));
            updateTemplate(keyboardChange);
        }

        function makeValid() {
            ngModelCtrl.$setValidity('time', true);
            $scope.invalidHours = false;
            $scope.invalidMinutes = false;
        }

        function updateTemplate(keyboardChange) {
            var hours = selected.getHours(), minutes = selected.getMinutes();

            if ($scope.showMeridian) {
                hours = (hours === 0 || hours === 12) ? 12 : hours % 12; // Convert 24 to 12 hour system
            }

            $scope.hours = keyboardChange === 'h' ? hours : pad(hours);
            if (keyboardChange !== 'm') {
                $scope.minutes = pad(minutes);
            }
            if (!$scope.showMeridian) {
                $scope.meridian = _24Hour;
            }
            else if ($scope.showMeridian) {
                $scope.meridian = selected.getHours() < 12 ? meridians[0] : meridians[1];
            }
        }

        function addMinutes(minutes) {
            var dt = new Date(selected.getTime() + minutes * 60000);
            selected.setHours(dt.getHours(), dt.getMinutes());
            refresh();
        }

        $scope.showSpinners = angular.isDefined($attrs.showSpinners) ?
            $scope.$parent.$eval($attrs.showSpinners) : timepickerConfig.showSpinners;

        $scope.incrementHours = function () {
            addMinutes(hourStep * 60);
        };
        $scope.decrementHours = function () {
            addMinutes(- hourStep * 60);
        };
        $scope.incrementMinutes = function () {
            addMinutes(minuteStep);
        };
        $scope.decrementMinutes = function () {
            addMinutes(- minuteStep);
        };

        //fix change am/pm/24 jump through 12:00
        $scope.toggleMeridian = function () {
            if ($scope.meridian === meridians[0]) {
                $scope.showMeridian = true;
                addMinutes(12 * 60 * ((selected.getHours() < 12) ? 1 : -1));
                return true;
            }
            if ($scope.meridian === meridians[1]) {
                $scope.showMeridian = false;
                addMinutes(12 * 60 * ((selected.getHours() < 12) ? 1 : -1));
                return true;
            }
            if ($scope.meridian === _24Hour) {
                $scope.showMeridian = true;
                addMinutes(selected.getHours() <= 12 ? 0 : 12 * 60 * ((selected.getHours() >= 12) ? 1 : -1));
                return true;
            }
        };
    }])

    .directive('timepicker', function () {
        return {
            restrict: 'EA',
            require: ['timepicker', '?^ngModel'],
            controller: 'TimepickerController',
            replace: true,
            scope: {},
            templateUrl: 'template/timepicker.html',
            link: function (scope, element, attrs, ctrls) {
                var timepickerCtrl = ctrls[0], ngModelCtrl = ctrls[1];

                if (ngModelCtrl) {
                    timepickerCtrl.init(ngModelCtrl, element.find('input'));
                }
            }
        };
    });

angular.module('platformWebApp').run(['$templateCache', function ($templateCache) {
    'use strict';

    $templateCache.put('template/date-picker.html',
        "<ul class=\"dropdown-menu dropdown-menu-left datetime-picker-dropdown\" ng-if=\"isOpen && showPicker == 'date'\" ng-style=dropdownStyle style=left:inherit ng-keydown=keydown($event) ng-click=$event.stopPropagation()><li style=\"padding:0 5px 5px 5px\" class=date-picker-menu><div ng-transclude></div></li><li style=padding:5px ng-if=buttonBar.show><span class=\"btn-group pull-left\" style=margin-right:5px ng-if=\"doShow('today') || doShow('clear')\"><button type=button class=\"btn btn-sm btn-info\" ng-if=\"doShow('today')\" ng-click=\"select('today')\" ng-disabled=\"isDisabled('today')\">{{ 'platform.commands.today' | translate }}</button> <button type=button class=\"btn btn-sm btn-danger\" ng-if=\"doShow('clear')\" ng-click=\"select('clear')\">{{ 'platform.commands.clear' | translate }}</button></span> <span class=\"btn-group pull-right\" ng-if=\"(doShow('time') && enableTime) || doShow('close')\"><button type=button class=\"btn btn-sm btn-default\" ng-if=\"doShow('time') && enableTime\" ng-click=\"open('time', $event)\">{{ 'platform.commands.time' | translate}}</button> <button type=button class=\"btn btn-sm btn-success\" ng-if=\"doShow('close')\" ng-click=close(true)>{{ 'platform.commands.close' | translate}}</button></span></li></ul>"
    );

    $templateCache.put('template/time-picker.html',
        "<ul class=\"dropdown-menu dropdown-menu-left datetime-picker-dropdown\" ng-if=\"isOpen && showPicker == 'time'\" ng-style=dropdownStyle style=left:inherit ng-keydown=keydown($event) ng-click=$event.stopPropagation()><li style=\"padding:0 5px 5px 5px\" class=time-picker-menu><div ng-transclude></div></li><li style=padding:5px ng-if=buttonBar.show><span class=\"btn-group pull-left\" style=margin-right:5px ng-if=\"doShow('now') || doShow('clear')\"><button type=button class=\"btn btn-sm btn-info\" ng-if=\"doShow('now')\" ng-click=\"select('now')\" ng-disabled=\"isDisabled('now')\">{{ 'platform.commands.now' | translate}}</button> <button type=button class=\"btn btn-sm btn-danger\" ng-if=\"doShow('clear')\" ng-click=\"select('clear')\">{{ 'platform.commands.clear' | translate}}</button></span> <span class=\"btn-group pull-right\" ng-if=\"(doShow('date') && enableDate) || doShow('close')\"><button type=button class=\"btn btn-sm btn-default\" ng-if=\"doShow('date') && enableDate\" ng-click=\"open('date', $event)\">{{ 'platform.commands.date' | translate}}</button> <button type=button class=\"btn btn-sm btn-success\" ng-if=\"doShow('close')\" ng-click=close(true)>{{ 'platform.commands.close' | translate }}</button></span></li></ul>"
    );

    $templateCache.put("template/timepicker.html",
        "<table>\n" +
        "  <tbody>\n" +
        "    <tr class=\"text-center\" ng-show=\"::showSpinners\">\n" +
        "      <td><a ng-click=\"incrementHours()\" class=\"btn btn-link\"><span class=\"glyphicon glyphicon-chevron-up\"></span></a></td>\n" +
        "      <td>&nbsp;</td>\n" +
        "      <td><a ng-click=\"incrementMinutes()\" class=\"btn btn-link\"><span class=\"glyphicon glyphicon-chevron-up\"></span></a></td>\n" +
        "      <td></td>\n" +
        "    </tr>\n" +
        "    <tr>\n" +
        "      <td class=\"form-group\" ng-class=\"{'has-error': invalidHours}\">\n" +
        "        <input style=\"width:50px;\" type=\"text\" ng-model=\"hours\" ng-change=\"updateHours()\" class=\"form-control text-center\" ng-readonly=\"::readonlyInput\" maxlength=\"2\">\n" +
        "      </td>\n" +
        "      <td>:</td>\n" +
        "      <td class=\"form-group\" ng-class=\"{'has-error': invalidMinutes}\">\n" +
        "        <input style=\"width:50px;\" type=\"text\" ng-model=\"minutes\" ng-change=\"updateMinutes()\" class=\"form-control text-center\" ng-readonly=\"::readonlyInput\" maxlength=\"2\">\n" +
        "      </td>\n" +
        "      <td><button type=\"button\" class=\"btn btn-default text-center\" ng-click=\"toggleMeridian()\">{{meridian}}</button></td>\n" +
        "    </tr>\n" +
        "    <tr class=\"text-center\" ng-show=\"::showSpinners\">\n" +
        "      <td><a ng-click=\"decrementHours()\" class=\"btn btn-link\"><span class=\"glyphicon glyphicon-chevron-down\"></span></a></td>\n" +
        "      <td>&nbsp;</td>\n" +
        "      <td><a ng-click=\"decrementMinutes()\" class=\"btn btn-link\"><span class=\"glyphicon glyphicon-chevron-down\"></span></a></td>\n" +
        "      <td></td>\n" +
        "    </tr>\n" +
        "  </tbody>\n" +
        "</table>\n" +
        "");
}]);
