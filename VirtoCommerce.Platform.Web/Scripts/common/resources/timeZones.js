// Full list of time zones defined in tz database
// see https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
angular.module('platformWebApp')
.factory('platformWebApp.common.timeZones', ['moment', function(moment) {
    return {
        get: function(id) {
            return _.findWhere(this.query(), { id: this.normalize(id) });
        },
        normalize: function(id) {
            var toTitleCase = function(str) {
                return str.charAt(0).toUpperCase() + str.substr(1).toLowerCase();
            }
            var parts = id.split(/[\/_]/);
            var continent = toTitleCase(parts.shift());
            parts.forEach(function(x) {
                return toTitleCase(x);
            });
            var city = parts.join('_');
            return continent + "/" + city;
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