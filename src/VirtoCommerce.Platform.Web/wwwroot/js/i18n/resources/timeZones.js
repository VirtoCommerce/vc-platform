// Full list of time zones defined in tz database
// see https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
angular.module('platformWebApp')
.factory('platformWebApp.common.timeZones', ['moment', function(moment) {
    var result = {
        get: function(id) {
            return _.find(this.query(), function(x) { return x.id.toLowerCase() === id.toLowerCase(); });
        },
        contains: function (id) {
            return _.some(this.query(), function (x) { return x.id.toLowerCase() === id.toLowerCase(); });
        },
        utcOffset: function (id) {
            var offset = moment.tz.zone(id).offset(moment().valueOf());
            // UTC offset has inverted sign. Compare to zero to avoid -0
            offset = offset === 0 ? 0 : offset * -1;
            var minutes = offset % 60;
            var hours = (offset - minutes) / 60;
            // format: ±HHMM
            var pad = function (n, withSign) {
                var result = '';
                if (withSign) {
                    result = n >= 0 ? '+' : '-';
                    n = n < 0 ? n * -1 : n;
                }
                result += n < 10 ? '0' + n : n;
                return result;
            }
            return { hours: hours, minutes: minutes, formatted: pad(hours, true) + ':' + pad(minutes) };
        },
        query: function () {
            // Get time zone from moment list of time zones, append UTC offset to name (UTC ±XX:XX Continent/City) and sort by UTC offset
            return _.map(moment.tz.names(), function (x) {
                var utcOffset = result.utcOffset(x);
                return {
                    id: x,
                    utcOffset: utcOffset,
                    name: '(UTC ' + utcOffset.formatted + ') ' + x
                };
            }).sort(function (a, b) {
                if (!a.utcOffset || !b.utcOffset) {
                    return !b.utcOffset ? -1 : !a.utcOffset ? 1 : 0;
                }
                return a.utcOffset.hours === b.utcOffset.hours ? a.utcOffset.minutes - b.utcOffset.minutes : a.utcOffset.hours - b.utcOffset.hours;
            });
        }
    };
    return result;
}]);