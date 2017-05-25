angular.module('ng')
.config(['$provide', function ($provide) {
    $provide.decorator('dateFilter', ['$delegate', 'moment', 'amMoment', function ($delegate, moment, amMoment) {
        var filter = function (date, format, timeZone) {
            var t = moment.defaultZone;
            if (moment.isMoment(date)) {
                timeZone = date.format("ZZ");
                date = date.toDate();
            }
            return $delegate.apply(this, [date, format, timeZone]);
        };
        return filter;
    }]);
}]);