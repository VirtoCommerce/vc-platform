angular.module('platformWebApp')
 .factory('platformWebApp.urlHelper', [function() {
    function getUrlParameter(name)
    {
        var query = new URLSearchParams(window.location.search);

        if (query.has(name)) {
            return query.get(name);
        }

        return false;
    };

    return { getUrlParameter: getUrlParameter };
}]);
