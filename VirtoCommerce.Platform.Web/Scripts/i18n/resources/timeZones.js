// Full list of time zones defined in tz database
// see https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
angular.module('platformWebApp')
.factory('platformWebApp.common.timeZones', ['moment', function(moment) {
    return {
        get: function(id) {
            return _.find(this.query(), function(x) { return x.id.toLowerCase() === id.toLowerCase(); });
        },
        contains: function (id) {
            return _.some(this.query(), function (x) { return x.id.toLowerCase() === id.toLowerCase(); });
        },
        query: function () {
            // Get time zone from moment list of time zones, append UTC offset to name (UTC ±XX:XX Continent/City) and sort by UTC offset
            return _.sortBy(_.map(moment.tz.names(), function(x) {
                return {
                    id: x,
                    utcOffset: parseInt(moment.tz(x).format('ZZ')),
                    name: '(UTC ' + moment.tz(x).format('Z') + ') ' + x
                };
            }), 'utcOffset');
        }
    };
}]);