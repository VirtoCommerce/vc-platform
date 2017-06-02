// Full list of time zones defined in tz database
// see https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
angular.module('platformWebApp')
.factory('platformWebApp.common.timeZones', ['moment', function(moment) {
    return {
        get: function(id) {
            return _.findWhere(this.query(), { id: this.normalize(id) });
        },
        contains: function (id) {
            return _.map(this.query(), function (entry) { return entry.id }).includes(this.normalize(id));
        },
        normalize: function(id) {
            var result = undefined;
            if (!!id) {
                var toTitleCase = function(str) {
                    return str.capitalize();
                }
                var parts = id.split(/[\/_]/);
                var continent = toTitleCase(parts.shift());
                parts.forEach(function(x) {
                    return toTitleCase(x);
                });
                var city = parts.join('_');
                result = continent + "/" + city;
            }
            return result;
        },
        query: function() {
            return _.map(moment.tz.names(), function(x) {
                return {
                    id: x,
                    name: x
                };
            });
        }
    };
}]);