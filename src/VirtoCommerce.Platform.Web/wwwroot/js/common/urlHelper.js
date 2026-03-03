angular.module('platformWebApp').factory('platformWebApp.urlHelper', [function () {
    function getSafeReturnUrl() {
        const returnUrl = getQueryValue('ReturnUrl');
        if (returnUrl && isLocalUrl(returnUrl)) {
            return returnUrl;
        }

        return undefined;
    }

    function getQueryValue(name) {
        const query = new URLSearchParams(window.location.search);
        return query.get(name);
    }

    function isLocalUrl(value) {
        try {
            const url = new URL(value, window.location.origin);
            return url.origin === window.location.origin;
        } catch (e) {
            return false;
        }
    }

    function getHashQueryValue(name) {
        var hash = window.location.hash || '';
        var queryIndex = hash.indexOf('?');
        if (queryIndex === -1) {
            return null;
        }
        var hashQuery = new URLSearchParams(hash.substring(queryIndex));
        return hashQuery.get(name);
    }

    function isEmbeddedMode() {
        // Check both regular query string (?EmbeddedMode=true)
        // and hash-based query string (#!/workspace?EmbeddedMode=true)
        return getQueryValue('EmbeddedMode') === 'true'
            || getHashQueryValue('EmbeddedMode') === 'true';
    }

    return {
        getSafeReturnUrl,
        getQueryValue,
        isLocalUrl,
        isEmbeddedMode
    };
}]);
