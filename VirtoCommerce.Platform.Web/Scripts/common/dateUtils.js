angular.module('platformWebApp')
.factory('platformWebApp.dateUtils', ['$locale', '$log', function ($locale, $log) {
    // Service provide variables and functions for angular to moment convertion and validation
    var result = {
        pairs: [
            { 'yyyy': 'YYYY' }, { 'yy': 'YY' }, { 'y': 'Y' },
            { 'dd': 'DD' }, { 'd': 'D' },
            { 'eee': 'ddd' }, { 'EEEE': 'dddd' },
            { 'sss': 'SSS' },
            { 'Z': 'ZZ' }
        ],
        valid: ["yyyy', 'yy', 'y', 'MMMM', 'MMM', 'MM', 'M', 'dd', 'd', 'EEEE', 'EEE', , 'ww', 'w"],
        invalid: ['G', 'GG', 'GGG', 'GGGG'],
        validDate: ["yyyy', 'yy', 'y', 'MMMM', 'MMM', 'MM', 'M', 'dd', 'd', 'EEEE', 'EEE', 'ww', 'w"],
        validTime: ['HH', 'H', 'hh', 'h', 'mm', 'm', 'ss', 's', 'sss', 'a', 'Z'],
        additionalFormats: ['medium', 'short', 'mediumTime', 'shortTime', 'fullDate', 'longDate', 'mediumDate', 'shortDate'],
        additionalDateFormats: ['fullDate', 'longDate', 'mediumDate', 'shortDate'],
        additionalTimeFormats: ['mediumTime', 'shortTime'],
        additionalMixedFormats: ['medium', 'short'],
        isInvalid: function(originalFormat) {
            return !this.additionalFormats.includes(originalFormat) && !this.invalid.includes(originalFormat);
        },
        isInvalidDate: function (originalFormat) {
            return _.every([this.additionalTimeFormats, this.additionalMixedFormats, this.validTime, this.invalid].forEach(function (formats) {
                return _.every(formats, function (format) {
                    return originalFormat.indexOf(format) < 0;
                });
            }));
        },
        isInvalidTime: function (originalFormat) {
            return _.every([this.additionalDateFormats, this.additionalMixedFormats, this.validDate, this.invalid].forEach(function (formats) {
                return _.all(formats, function (format) {
                    return originalFormat.indexOf(format) < 0;
                });
            }));
        },
        isInvalidMixed: function (originalFormat) {
            var result = _.every([this.additionalTimeFormats, this.additionalDateFormats, this.invalid].forEach(function (formats) {
                return _.every(formats, function (format) {
                    return originalFormat.indexOf(format) < 0;
                });
            }));
            if (result) {
                result |= !_.some(this.additionalMixedFormas, function(mixedFormat) {
                    return originalFormat.indexOf(mixedFormat) >= 0;
                });
                if (result) {
                    result |= _.every([
                        _.some(this.validDate, function(format) {
                            return originalFormat.indexOf(format) >= 0;
                        }), _.some(this.validTime, function (format) {
                            return originalFormat.indexOf(format) >= 0;
                        })
                    ]);
                    return result;
                }
            }
            return false;
        },
        validate: function(originalFormat, validator) {
            var formatParts = originalFormat.split("'");
            if (_.some(formatParts.forEach(function(formatPart) {
                return validator(formatPart);
            }))) {
                $log.error("Invalid date format for datepicker popup");
                return false;
            }
            return true;
        },
        convert: function (originalFormat) {
            var format = originalFormat;
            if (this.additionalFormats.includes(format)) {
                format = $locale.DATETIME_FORMATS[format];
            }
            var formatParts = format.split("'");
            for (var i = 0; i < formatParts.length; i++) {
                if (!_.every(this.invalid, function (token) { return formatParts.indexOf(token) < 0 })) {
                    $log.error("Moment doesn't support formats for era");
                    return undefined;
                }
                var replace = function(formatPart) {
                    this.pairs.forEach(function (token) {
                        var key = Object.keys(token)[0];
                        formatPart = formatPart.replace(new RegExp(RegExp.escape(key), "g"), token[key]);
                    });
                    return formatPart;
                };
                formatParts[i] = replace.apply(this, [formatParts[i]]);
            }
            return formatParts.join('');
        }
    };
    return result;
}]);