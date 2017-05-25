angular.module('ng')
.config(['$provide', function ($provide) {
    $provide.decorator('dateFilter', ['$delegate', 'moment', 'amMoment', function ($delegate, moment, amMoment) {
        var filter = function (date, format, timeZone) {
            if (moment.isMoment(date)) {
                timeZone = date.format("ZZ");
                date = date.toDate();
            }
            return $delegate.apply(this, [date, format, timeZone]);
        };
        return filter;
    }]);
}]);