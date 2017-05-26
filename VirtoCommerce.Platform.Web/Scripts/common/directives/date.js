angular.module('ng')
.config(['$provide', function ($provide) {
    $provide.decorator('dateFilter', ['$delegate', 'moment', function ($delegate, moment) {
        var filter = function (date, format, timeZone) {
            date = moment.isMoment(date) ? date : moment(date);
            if (moment.isMoment(date)) {
                timeZone = date.format("ZZ");
                date = date.toDate();
            }
            return $delegate.apply(this, [date, format, timeZone]);
        };
        return filter;
    }]);
}]);