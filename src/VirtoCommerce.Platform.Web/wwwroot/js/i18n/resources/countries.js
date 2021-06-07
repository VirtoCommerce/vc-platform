angular.module('platformWebApp')
    .factory('platformWebApp.common.countries', ['$translate', '$resource', function ($translate, $resource) {
        return {
            get: function (id) {
                return _.findWhere(this.query(), { id: this.normalize(id) });
            },
            contains: function (id) {
                return _.map(this.query(), function (entry) { return entry.id }).includes(this.normalize(id));
            },
            normalize: function (id) {
                var result = undefined;
                if (!!id) {
                    result = id.toUpperCase();
                }
                return result;
            },
            query: function () {
                var results = $resource('api/platform/common/countries').query((data) => {
                    for (var i = 0; i < data.length; i++) {
                        var translateKey = 'platform.countries.' + data[i].id;
                        var translated = $translate.instant(translateKey);
                        data[i].displayName = translated === translateKey ? data[i].name : translated;
                    }
                });

                return results;
            }
        };
    }]);
